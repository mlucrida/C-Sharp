//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Itemtransaction
    {
        public int transactionkey { get; set; }
        public int itemkey { get; set; }
        public int boxkey { get; set; }
        public System.DateTime in_time { get; set; }
        public Nullable<System.DateTime> out_time { get; set; }
    
        public virtual Box Box { get; set; }
        public virtual Item Item { get; set; }
    }
}
