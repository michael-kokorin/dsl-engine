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
    
    public partial class VcsPluginTelemetry
    {
        public long Id { get; set; }
        public System.DateTime DateTimeUtc { get; set; }
        public System.DateTime DateTimeLocal { get; set; }
        public Nullable<long> EntityId { get; set; }
        public Nullable<long> RelatedEntityId { get; set; }
        public string OperationName { get; set; }
        public string OperationSource { get; set; }
        public Nullable<long> OperationDuration { get; set; }
        public string UserSid { get; set; }
        public string UserLogin { get; set; }
        public int OperationStatus { get; set; }
        public Nullable<int> OperationHResult { get; set; }
        public string DisplayName { get; set; }
        public string TypeFullName { get; set; }
        public string AssemblyName { get; set; }
        public Nullable<long> DownloadedSourcesSize { get; set; }
        public Nullable<long> CommittedSize { get; set; }
        public string CreatedBranchName { get; set; }
    }
}
