namespace Modules.Core.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Extensions;
	using Infrastructure.Plugins.Common.Contracts;
	using Modules.Core.Contracts.UI.Dto;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class SettingsHelper: ISettingsHelper
	{
		private readonly ISettingRepository _settingRepository;

		private readonly ISettingGroupRepository _settingGroupRepository;

		private readonly ISettingValidator _settingValidator;

		private readonly ISettingValuesRepository _settingValuesRepository;

		public SettingsHelper(
			[NotNull] ISettingValuesRepository settingValuesRepository,
			[NotNull] ISettingRepository settingRepository,
			[NotNull] ISettingGroupRepository settingGroupRepository,
			[NotNull] ISettingValidator settingValidator)
		{
			if(settingValuesRepository == null)
			{
				throw new ArgumentNullException(nameof(settingValuesRepository));
			}
			if(settingRepository == null)
			{
				throw new ArgumentNullException(nameof(settingRepository));
			}
			if(settingGroupRepository == null)
			{
				throw new ArgumentNullException(nameof(settingGroupRepository));
			}
			if(settingValidator == null)
			{
				throw new ArgumentNullException(nameof(settingValidator));
			}

			_settingValuesRepository = settingValuesRepository;
			_settingRepository = settingRepository;
			_settingGroupRepository = settingGroupRepository;
			_settingValidator = settingValidator;
		}

		/// <summary>
		///     Gets settings at the specified owner.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="entityId">The entity identifier.</param>
		/// <returns>Settings.</returns>
		public SettingGroupDto[] Get(SettingOwnerDto owner, long entityId)
		{
			var settings = _settingValuesRepository
				.Get(PluginType.ServerAgent, (int)owner, entityId)
				.ToDictionary(_ => _.SettingId, _ => _);

			var settingIds = settings.Keys.ToArray();

			// select all settings for the same owner but without value.
			var settingsWithoutValues =
				_settingRepository
					.Get(PluginType.ServerAgent, (int)owner)
					.Where(_ => settingIds.All(id => id != _.Id))
					.ToArray();

			if(settingsWithoutValues.Any())
			{
				InitializeEmptySettings(entityId, settingsWithoutValues, settings);
			}

			var mainSettings = settings.ToDictionary(
				_ => _.Key,
				_ => new EntityDtoMap<SettingValues, SettingValueDto>
				{
					Entity = _.Value,
					Dto = _.Value.ToDto()
				});

			foreach(
				var parentSettingGroup in
				mainSettings.Where(_ => _.Value.Entity.Settings.ParentSettingId.HasValue)
							.GroupBy(_ => _.Value.Entity.Settings.ParentSettingId.Value))
			{
				var childGroups = new Dictionary<string, SettingGroupDto>();

				// ReSharper disable once LoopCanBeConvertedToQuery
				foreach(var parentSettingGroupItem in parentSettingGroup.GroupBy(_ => _.Value.Entity.Settings.ParentSettingItemKey))
				{
					var items = parentSettingGroupItem.Select(_ => _.Value).ToList();
					var childGroup = ArrangeSettings(items);
					childGroups.Add(parentSettingGroupItem.Key, childGroup);
				}

				var listItemGroups =
					mainSettings[parentSettingGroup.Key]
						.Dto
						.DefaultValue
						.FromJson<SettingSubitem[]>()
						.Select(_ => _.Key)
						.Select(_ => childGroups.ContainsKey(_) ? childGroups[_] : null)
						.ToArray();
				mainSettings[parentSettingGroup.Key].Dto.ChildGroups.AddRange(listItemGroups);
			}

			var rootSettings =
				mainSettings.Where(_ => !_.Value.Entity.Settings.ParentSettingId.HasValue).Select(_ => _.Value).ToList();
			var rootGroup = ArrangeSettings(rootSettings);

			return rootGroup.Groups.ToArray();
		}

		/// <summary>
		///     Gets setting value at the specified owner and for the specified setting.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="entityId">The entity identifier.</param>
		/// <param name="settingKey">The setting key.</param>
		/// <returns>The setting value.</returns>
		public SettingValueDto Get(SettingOwnerDto owner, long entityId, string settingKey)
		{
			var settingValue = _settingValuesRepository
				.Get(PluginType.ServerAgent, (int)owner, entityId, settingKey);

			if(settingValue != null)
			{
				return settingValue.ToDto();
			}

			var setting = _settingRepository.Get(PluginType.ServerAgent, (int)owner, settingKey);
			if(setting == null)
			{
				return null;
			}

			var dto = setting.GetDefaultValueDto();

			var currentItem = dto.GetCurrentSubitem();
			dto.Value = setting.SettingType == (int)SettingType.List ? currentItem.Key : currentItem.Text;

			settingValue = new SettingValues
			{
				EntityId = entityId,
				SettingId = setting.Id,
				Value = dto.Value,
				ProjectId = entityId
			};

			_settingValuesRepository.Insert(settingValue);

			_settingGroupRepository.Save();

			dto.Id = settingValue.Id;

			return dto;
		}

		/// <summary>
		///     Copies the project settings to task settings.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="taskId">The task identifier.</param>
		public void CopyProjectSettingsToTaskSettings(long projectId, long taskId)
		{
			var projectSettings =
				_settingValuesRepository
					.Get(PluginType.ServerAgent, (int)SettingOwner.Project, projectId)
					.ToDictionary(_ => _.Settings.Code + "_" + _.Settings.ParentSettingItemKey, _ => _);

			var taskSettings = _settingRepository.Query().Where(_ => _.SettingOwner == (int)SettingOwner.Task).ToArray();
			foreach(var taskSetting in taskSettings)
			{
				var value = new SettingValues
				{
					EntityId = taskId,
					SettingId = taskSetting.Id,
					ProjectId = projectId
				};

				var key = taskSetting.Code + "_" + taskSetting.ParentSettingItemKey;
				value.Value =
					projectSettings.ContainsKey(key)
						? projectSettings[key].Value
						: taskSetting.GetDefaultValue();

				_settingValuesRepository.Insert(value);
			}

			_settingValuesRepository.Save();
		}

		/// <summary>
		///     Saves settings.
		/// </summary>
		/// <param name="values">The values.</param>
		/// <returns>A collection of values with errors. Key is identifier of value. Value is error message.</returns>
		[NotNull]
		public Dictionary<long, string> Save(SettingValueDto[] values)
		{
			var errors = new Dictionary<long, string>();

			foreach(var settingValueDto in values)
			{
				string errorMessage;
				if(!_settingValidator.IsValid(settingValueDto, out errorMessage))
				{
					errors.Add(settingValueDto.Id, errorMessage);
					continue;
				}

				var entity = _settingValuesRepository.GetById(settingValueDto.Id);
				entity.Value = settingValueDto.Value;

				_settingValuesRepository.Save();
			}

			return errors;
		}

		private SettingGroupDto ArrangeSettings(
			[NotNull] [ItemNotNull] IEnumerable<EntityDtoMap<SettingValues, SettingValueDto>> values)
		{
			var groups =
				new Dictionary<long, EntityDtoMap<SettingGroups, SettingGroupDto>>
				{
					{
						0, new EntityDtoMap<SettingGroups, SettingGroupDto>
						{
							Entity = null,
							Dto = new SettingGroupDto
							{
								Id = 0,
								Title = string.Empty,
								Values = new List<SettingValueDto>(),
								Groups = new List<SettingGroupDto>()
							}
						}
					}
				};

			foreach(var settingValues in values)
			{
				var parentGroupId = settingValues.Entity.Settings.SettingGroupId;
				if(!parentGroupId.HasValue)
				{
					groups[0].Dto.Values.Add(settingValues.Dto);
					continue;
				}

				var groupKey = parentGroupId.Value;

				if(groups.ContainsKey(groupKey))
				{
					groups[groupKey].Dto.Values.Add(settingValues.Dto);
				}
				else
				{
					var groupEntity =
						_settingGroupRepository.GetById(groupKey);

					var group =
						new SettingGroupDto
						{
							Id = groupKey,
							Title = groupEntity?.DisplayName,
							Values = new List<SettingValueDto>
							{
								settingValues.Dto
							},
							Groups = new List<SettingGroupDto>()
						};

					groups.Add(groupKey, new EntityDtoMap<SettingGroups, SettingGroupDto> {Entity = groupEntity, Dto = group});
				}
			}

			var groupItems = groups.Values.Skip(1).ToList();
			for(var index = 0; index < groupItems.Count; index++)
			{
				var item = groupItems[index];

				if(!item.Entity.ParentGroupId.HasValue)
				{
					groups[0].Dto.Groups.Add(item.Dto);
					continue;
				}

				var groupId = item.Entity.ParentGroupId.Value;
				if(groups.ContainsKey(groupId))
				{
					groups[groupId].Dto.Groups.Add(item.Dto);
					continue;
				}

				var newGroup = _settingGroupRepository.GetById(groupId);
				if(newGroup == null)
				{
					throw new InvalidOperationException();
				}

				var dto =
					new SettingGroupDto
					{
						Id = newGroup.Id,
						Title = newGroup.DisplayName,
						Values = new List<SettingValueDto>(),
						Groups = new List<SettingGroupDto>
						{
							item.Dto
						}
					};

				groups.Add(groupId, new EntityDtoMap<SettingGroups, SettingGroupDto> {Entity = newGroup, Dto = dto});
				groupItems.Add(new EntityDtoMap<SettingGroups, SettingGroupDto> {Entity = newGroup, Dto = dto});
			}

			return groups[0].Dto;
		}

		private void InitializeEmptySettings(
			long entityId,
			[NotNull] [ItemNotNull] IEnumerable<Settings> settings,
			[NotNull] IDictionary<long, SettingValues> values)
		{
			// ReSharper disable once LoopCanBePartlyConvertedToQuery
			foreach(var settingWithoutValue in settings)
			{
				var settingValue = new SettingValues
				{
					EntityId = entityId,
					Value = settingWithoutValue.GetDefaultValue(),
					SettingId = settingWithoutValue.Id,
					Settings = settingWithoutValue,
					ProjectId = entityId
				};

				_settingValuesRepository.Insert(settingValue);
				values.Add(settingValue.SettingId, settingValue);
			}

			_settingValuesRepository.Save();
		}

		private sealed class EntityDtoMap<TEntity, TDto>
		{
			public TDto Dto { get; set; }

			public TEntity Entity { get; set; }
		}
	}
}