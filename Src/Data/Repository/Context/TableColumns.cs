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
    
    public partial class TableColumns
    {
        public long Id { get; set; }
        public long TableId { get; set; }
        public Nullable<long> ReferenceTableId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string FieldName { get; set; }
        public string FieldDescription { get; set; }
        public int FieldType { get; set; }
        public int FieldDataType { get; set; }
        public long CultureId { get; set; }
    
        public virtual Tables Tables { get; set; }
        public virtual Tables Tables1 { get; set; }
        public virtual Cultures Cultures { get; set; }
    }
}
