namespace Infrastructure.AD.Tests
{
	using System;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Extensions;
	using Repository.Context;
	using Repository.Repositories;

	[TestFixture]
	public sealed class GroupNameBuilderTests
	{
		[SetUp]
		public void SetUp()
		{
			_projectRepositoryMock = new Mock<IProjectRepository>();
			_roleRepositoryMock = new Mock<IRoleRepository>();

			_projectRepositoryMock
				.Setup(_ => _.GetById(ProjectId))
				.Returns(new Projects
				{
					Alias = ProjectAlias,
					DisplayName = ProjectName
				});

			_roleRepositoryMock
				.Setup(_ => _.GetById(RoleId))
				.Returns(new Roles
				{
					Alias = RoleAlias,
					DisplayName = RoleName
				});

			_target = new GroupNameBuilder(_projectRepositoryMock.Object, _roleRepositoryMock.Object);
		}

		private const long ProjectId = 234;

		private const long RoleId = 532;

		private const string ProjectAlias = "project";

		private const string ProjectName = "projectName";

		private const string RoleAlias = "group";

		private const string RoleName = "roleName";

		private IGroupNameBuilder _target;

		private Mock<IProjectRepository> _projectRepositoryMock;

		private Mock<IRoleRepository> _roleRepositoryMock;

		[Test]
		public void ShouldBuildGroupByRoleAlias()
		{
			var result = _target.Build(RoleId);

			var groupDescription = Resources.Resources.SdlRole.FormatWith(RoleName);

			result.Name.Should().BeEquivalentTo(RoleAlias);
			result.Description.Should().BeEquivalentTo(groupDescription);
		}

		[Test]
		public void ShouldBuildGroupNameInfo()
		{
			var groupName = $"{ProjectAlias}_{RoleAlias}";

			var groupDescription = Resources.Resources.SdlRoleInProject.FormatWith(RoleName, ProjectName);

			var result = _target.Build(RoleId, ProjectId);

			result.Name.Should().BeEquivalentTo(groupName);
			result.Description.Should().BeEquivalentTo(groupDescription);
		}

		[Test]
		public void ShouldThrownOnNonExistsProject()
		{
			_projectRepositoryMock
				.Setup(_ => _.GetById(ProjectId))
				.Returns((Projects) null);

			Assert.Throws<ArgumentException>(() => _target.Build(RoleId, ProjectId));
		}

		[Test]
		public void ShouldThrownOnNonExistsRole()
		{
			_roleRepositoryMock
				.Setup(_ => _.GetById(RoleId))
				.Returns((Roles) null);

			Assert.Throws<ArgumentException>(() => _target.Build(RoleId, ProjectId));
		}
	}
}