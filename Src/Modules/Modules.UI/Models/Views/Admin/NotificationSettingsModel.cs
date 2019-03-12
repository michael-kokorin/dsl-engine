namespace Modules.UI.Models.Views.Admin
{
    using System.ComponentModel.DataAnnotations;

    using Modules.UI.Resources;

    public sealed class NotificationSettingsModel
    {
        [Display(ResourceType = typeof(Resources), Name = "NotificationSettingsModel_MailServerHost_Main_server_host")]
        [Required]
        public string MailServerHost { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "NotificationSettingsModel_MainServerPort_Main_server_port")]
        [Required]
        public int MainServerPort { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "NotificationSettingsModel_MailBox_Main_box_name")]
        [EmailAddress]
        [Required]
        public string MailBox { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "NotificationSettingsModel_IsSslEnabled_SSL_enabled")]
        [Required]
        public bool IsSslEnabled { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "NotificationSettingsModel_UserName_User_name")]
        [Required]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "NotificationSettingsModel_Password_Password")]
        [Required]
        public string Password { get; set; }
    }
}