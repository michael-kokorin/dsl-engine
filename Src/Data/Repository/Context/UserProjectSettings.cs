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
    
    public partial class UserProjectSettings
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public long UserId { get; set; }
        public long PreferedRoleId { get; set; }
    
        public virtual Roles Roles { get; set; }
        public virtual Users Users { get; set; }
        public virtual Projects Projects { get; set; }
    }
}
