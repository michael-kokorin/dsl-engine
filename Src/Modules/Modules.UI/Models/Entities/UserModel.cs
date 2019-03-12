namespace Modules.UI.Models.Entities
{
    using System.ComponentModel.DataAnnotations;

    using Modules.UI.Resources;

    public sealed class UserModel
    {
        [Display(ResourceType = typeof(Resources), Name = "UserModel_DisplayName_Display_name")]
        [Required]
        [StringLength(16, MinimumLength = 2)]
        public string DisplayName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "UserModel_Email_Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public long Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "UserModel_Login_Login")]
        public string Login { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "UserModel_Sid_SID")]
        public string Sid { get; set; }
    }
}