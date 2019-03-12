namespace Modules.UI.Models.Views.Admin
{
    using System.ComponentModel.DataAnnotations;

    using Modules.UI.Resources;

    public sealed class FileStorageSettingsModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "FileStorageSettingsModel_TempDirPath_You_must_specify_file_storage_path")]
        [Display(ResourceType = typeof(Resources), Name = "FileStorageSettingsModel_TempDirPath_Temp_directory", Description = "FileStorageSettingsModel_TempDirPath_For_example____contoso_temp_sdl")]
        public string TempDirPath { get; set; }
    }
}