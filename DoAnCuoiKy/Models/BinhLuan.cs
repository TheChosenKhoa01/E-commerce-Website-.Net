//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoAnCuoiKy.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BinhLuan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BinhLuan()
        {
            this.Replies = new HashSet<Reply>();
        }
    
        public int MaBL { get; set; }
        public string NoiDungBL { get; set; }
        public Nullable<int> MaThanhVien { get; set; }
        public Nullable<int> MaSP { get; set; }
        public Nullable<int> danhgia { get; set; }
        public Nullable<System.DateTime> NgayBinhLuan { get; set; }
        public Nullable<bool> hienthi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reply> Replies { get; set; }
        public virtual SanPham SanPham { get; set; }
        public virtual ThanhVien ThanhVien { get; set; }
    }
}