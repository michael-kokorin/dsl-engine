namespace DbMigrations.Migrations.Migration_55
{
	using System.Data;

	using JetBrains.Annotations;

	// ReSharper disable once InconsistentNaming
	[UsedImplicitly]
	internal sealed class Migration_55: DbMigration
	{
		/// <summary>
		///     Performs migration.
		/// </summary>
		public override void Up(IDbTransformationProvider database)
		{
			database.AddColumn("data", "Settings", "OwnerPluginId", DbType.Int64, ColumnProperty.Null);
			database.AddColumn("data", "Settings", "IsAuth", DbType.Boolean, ColumnProperty.NotNull, 0);
			database.AddColumn("data", "SettingGroups", "OwnerPluginId", DbType.Int64, ColumnProperty.Null);

			database.ExecuteNonQuery(@"
			ALTER TABLE[data].[Settings] DROP CONSTRAINT[FK_Settings_SettingGroups_SettingGroupId];
			ALTER TABLE[data].[Settings] DROP CONSTRAINT[FK_Settings_Settings_ParentSettingId];
			ALTER TABLE[data].[SettingValues] DROP CONSTRAINT[FK_SettingValues_Settings_SettingId];
			ALTER TABLE[data].[Settings_l10n] DROP CONSTRAINT[FK_Settings_l10n_Settings_SourceId];
			
			BEGIN TRANSACTION;
			SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
			SET XACT_ABORT ON;

			CREATE TABLE[data].[tmp_ms_xx_Settings] (
	[Id]
		BIGINT IDENTITY(1, 1) NOT NULL,

[Code]                 NVARCHAR(400)  NOT NULL,

[DisplayName]          NVARCHAR(400)  NOT NULL,

[SettingGroupId]       BIGINT NULL,

[SettingType]          INT NOT NULL,
	[SettingOwner]
		INT NOT NULL,
	[ParentSettingId]
		BIGINT NULL,

	[DefaultValue]         NVARCHAR(MAX)  NULL,
	[ParentSettingItemKey] NVARCHAR(400)  NULL,
	[Conditions]           NVARCHAR(4000) NULL,
	[OwnerPluginId]
		BIGINT NULL,

	[IsAuth]               BIT CONSTRAINT[DF_Settings_IsAuth] DEFAULT((0)) NOT NULL,
CONSTRAINT[tmp_ms_xx_constraint_PK_Settings1] PRIMARY KEY CLUSTERED([Id] ASC)
);

IF EXISTS(SELECT TOP 1 1 

		   FROM[data].[Settings])

	BEGIN
		SET IDENTITY_INSERT[data].[tmp_ms_xx_Settings]
		ON;
		INSERT INTO[data].[tmp_ms_xx_Settings] ([Id], [Code], [DisplayName], [SettingGroupId], [SettingType], [SettingOwner], [ParentSettingId], [DefaultValue], [ParentSettingItemKey], [Conditions])
		SELECT[Id],
				 [Code],
				 [DisplayName],
				 [SettingGroupId],
				 [SettingType],
				 [SettingOwner],
				 [ParentSettingId],
				 [DefaultValue],
				 [ParentSettingItemKey],
				 [Conditions]
		FROM[data].[Settings]
		ORDER BY[Id] ASC;
		SET IDENTITY_INSERT[data].[tmp_ms_xx_Settings]
		OFF;
	END

DROP TABLE[data].[Settings];

EXECUTE sp_rename N'[data].[tmp_ms_xx_Settings]', N'Settings';

EXECUTE sp_rename N'[data].[tmp_ms_xx_constraint_PK_Settings1]', N'PK_Settings', N'OBJECT';

COMMIT TRANSACTION;

		SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
ALTER TABLE[data].[Settings]
		WITH NOCHECK

	ADD CONSTRAINT[FK_Settings_SettingGroups_SettingGroupId] FOREIGN KEY([SettingGroupId]) REFERENCES[data].[SettingGroups] ([Id]);
ALTER TABLE[data].[Settings]
		WITH NOCHECK

	ADD CONSTRAINT[FK_Settings_Settings_ParentSettingId] FOREIGN KEY([ParentSettingId]) REFERENCES[data].[Settings] ([Id]);
ALTER TABLE[data].[SettingValues]
		WITH NOCHECK

	ADD CONSTRAINT[FK_SettingValues_Settings_SettingId] FOREIGN KEY([SettingId]) REFERENCES[data].[Settings] ([Id]);
ALTER TABLE[data].[Settings_l10n]
		WITH NOCHECK

	ADD CONSTRAINT[FK_Settings_l10n_Settings_SourceId] FOREIGN KEY([SourceId]) REFERENCES[data].[Settings] ([Id]);
ALTER TABLE[data].[Settings]
		WITH NOCHECK

	ADD CONSTRAINT[FK_Settings_Plugins] FOREIGN KEY([OwnerPluginId]) REFERENCES[data].[Plugins] ([Id]);
ALTER TABLE[data].[SettingGroups]
		WITH NOCHECK

	ADD CONSTRAINT[FK_SettingGroups_Plugins] FOREIGN KEY([OwnerPluginId]) REFERENCES[data].[Plugins] ([Id]);

		ALTER TABLE[data].[Settings]
		WITH CHECK CHECK CONSTRAINT[FK_Settings_SettingGroups_SettingGroupId];

		ALTER TABLE[data].[Settings]
		WITH CHECK CHECK CONSTRAINT[FK_Settings_Settings_ParentSettingId];

		ALTER TABLE[data].[SettingValues]
		WITH CHECK CHECK CONSTRAINT[FK_SettingValues_Settings_SettingId];

		ALTER TABLE[data].[Settings_l10n]
		WITH CHECK CHECK CONSTRAINT[FK_Settings_l10n_Settings_SourceId];

		ALTER TABLE[data].[Settings]
		WITH CHECK CHECK CONSTRAINT[FK_Settings_Plugins];

		ALTER TABLE[data].[SettingGroups]
		WITH CHECK CHECK CONSTRAINT[FK_SettingGroups_Plugins];

ALTER TABLE [data].[SettingGroups] DROP CONSTRAINT [FK_SettingGroups_Plugins];
ALTER TABLE [data].[SettingGroups] DROP CONSTRAINT [FK_SettingGroups_SettingGroups_ParentGroupId];
ALTER TABLE [data].[SettingGroups_l10n] DROP CONSTRAINT [FK_SettingGroups_l10n_SettingGroups_SourceId];
ALTER TABLE [data].[Settings] DROP CONSTRAINT [FK_Settings_SettingGroups_SettingGroupId];
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [data].[tmp_ms_xx_SettingGroups] (
	[Id]            BIGINT         IDENTITY (1, 1) NOT NULL,
	[Code]          NVARCHAR (400) NOT NULL,
	[DisplayName]   NVARCHAR (400) NOT NULL,
	[ParentGroupId] BIGINT         NULL,
	[OwnerPluginId] BIGINT         NULL,
	CONSTRAINT [tmp_ms_xx_constraint_PK_SettingGroups1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
		   FROM   [data].[SettingGroups])
	BEGIN
		SET IDENTITY_INSERT [data].[tmp_ms_xx_SettingGroups] ON;
		INSERT INTO [data].[tmp_ms_xx_SettingGroups] ([Id], [Code], [DisplayName], [ParentGroupId], [OwnerPluginId])
		SELECT   [Id],
				 [Code],
				 [DisplayName],
				 [ParentGroupId],
				 [OwnerPluginId]
		FROM     [data].[SettingGroups]
		ORDER BY [Id] ASC;
		SET IDENTITY_INSERT [data].[tmp_ms_xx_SettingGroups] OFF;
	END

DROP TABLE [data].[SettingGroups];

EXECUTE sp_rename N'[data].[tmp_ms_xx_SettingGroups]', N'SettingGroups';

EXECUTE sp_rename N'[data].[tmp_ms_xx_constraint_PK_SettingGroups1]', N'PK_SettingGroups', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
ALTER TABLE [data].[SettingGroups] WITH NOCHECK
	ADD CONSTRAINT [FK_SettingGroups_Plugins] FOREIGN KEY ([OwnerPluginId]) REFERENCES [data].[Plugins] ([Id]);
ALTER TABLE [data].[SettingGroups] WITH NOCHECK
	ADD CONSTRAINT [FK_SettingGroups_SettingGroups_ParentGroupId] FOREIGN KEY ([ParentGroupId]) REFERENCES [data].[SettingGroups] ([Id]);
ALTER TABLE [data].[SettingGroups_l10n] WITH NOCHECK
	ADD CONSTRAINT [FK_SettingGroups_l10n_SettingGroups_SourceId] FOREIGN KEY ([SourceId]) REFERENCES [data].[SettingGroups] ([Id]);
ALTER TABLE [data].[Settings] WITH NOCHECK
	ADD CONSTRAINT [FK_Settings_SettingGroups_SettingGroupId] FOREIGN KEY ([SettingGroupId]) REFERENCES [data].[SettingGroups] ([Id]);
EXECUTE sp_refreshsqlmodule N'[l10n].[GetSettingGroups]';
ALTER TABLE [data].[SettingGroups] WITH CHECK CHECK CONSTRAINT [FK_SettingGroups_Plugins];

ALTER TABLE [data].[SettingGroups] WITH CHECK CHECK CONSTRAINT [FK_SettingGroups_SettingGroups_ParentGroupId];

ALTER TABLE [data].[SettingGroups_l10n] WITH CHECK CHECK CONSTRAINT [FK_SettingGroups_l10n_SettingGroups_SourceId];

ALTER TABLE [data].[Settings] WITH CHECK CHECK CONSTRAINT [FK_Settings_SettingGroups_SettingGroupId];");
		}
	}
}