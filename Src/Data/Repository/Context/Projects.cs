//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Repository.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class Projects
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Projects()
        {
            this.NotificationRules = new HashSet<NotificationRules>();
            this.PolicyRules = new HashSet<PolicyRules>();
            this.Queries = new HashSet<Queries>();
            this.Reports = new HashSet<Reports>();
            this.Roles = new HashSet<Roles>();
            this.UserProjectSettings = new HashSet<UserProjectSettings>();
            this.WorkflowRules = new HashSet<WorkflowRules>();
            this.Tasks = new HashSet<Tasks>();
            this.SettingValues = new HashSet<SettingValues>();
        }
    
        public long Id { get; set; }
        public string Alias { get; set; }
        public string DefaultBranchName { get; set; }
        public System.DateTime Created { get; set; }
        public long CreatedById { get; set; }
        public string DisplayName { get; set; }
        public System.DateTime Modified { get; set; }
        public long ModifiedById { get; set; }
        public Nullable<long> ItPluginId { get; set; }
        public Nullable<long> VcsPluginId { get; set; }
        public int SdlPolicyStatus { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> VcsLastSyncUtc { get; set; }
        public Nullable<System.DateTime> ItLastSyncUtc { get; set; }
        public bool VcsSyncEnabled { get; set; }
        public bool EnablePoll { get; set; }
        public Nullable<int> PollTimeout { get; set; }
        public bool CommitToVcs { get; set; }
        public bool CommitToIt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotificationRules> NotificationRules { get; set; }
        public virtual Plugins Plugins { get; set; }
        public virtual Plugins Plugins1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PolicyRules> PolicyRules { get; set; }
        public virtual SdlStatuses SdlStatuses { get; set; }
        public virtual Users Users { get; set; }
        public virtual Users Users1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Queries> Queries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reports> Reports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Roles> Roles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserProjectSettings> UserProjectSettings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkflowRules> WorkflowRules { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tasks> Tasks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SettingValues> SettingValues { get; set; }
    }
}
