﻿//------------------------------------------------------------------------------
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
    using System.ComponentModel.DataAnnotations;

    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            this.BinhLuans = new HashSet<BinhLuan>();
            this.ChiTietDonDatHangs = new HashSet<ChiTietDonDatHang>();
            this.ChiTietPhieuNhaps = new HashSet<ChiTietPhieuNhap>();
        }
    
        [Display(Name ="Mã Sản Phẩm")]
        public int MaSP { get; set; }
        [Display(Name = "Tên Sản Phẩm")]
        public string TenSP { get; set; }
        [Display(Name = "Đơn giá")]
        public Nullable<decimal> DonGia { get; set; }
        [Display(Name = "Ngày Cập Nhật")]
        public Nullable<System.DateTime> NgayCapNhap { get; set; }
        [Display(Name = "Cấu Hình")]
        public string CauHinh { get; set; }
        [Display(Name = "Mô Tả")]
        public string MoTa { get; set; }
        [Display(Name = "Hình Ảnh")]
        public string HinhAnh { get; set; }
        [Display(Name = "Số Lượng Tồn")]
        public Nullable<int> SoLuongTon { get; set; }
        [Display(Name = "Lượt Xem")]
        public Nullable<int> LuotXem { get; set; }
        [Display(Name = "Lượt Bình Chon")]
        public Nullable<int> LuotBinhChon { get; set; }
        [Display(Name = "   Lượt Bình Luận")]
        public Nullable<int> LuotBinhLuan { get; set; }
        [Display(Name = "Số Lần Mua")]
        public Nullable<int> SoLanMua { get; set; }
        [Display(Name = "Mới")]
        public Nullable<int> Moi { get; set; }
        [Display(Name = "Mã Nhà Cung Cấp")]
        public Nullable<int> MaNCC { get; set; }
        [Display(Name = "Mã Nhà Sản Xuất")]
        public Nullable<int> MaNSX { get; set; }
        [Display(Name = "Mã Loại Sản Phẩm")]
        public Nullable<int> MaLoaiSP { get; set; }
        [Display(Name = "Đã Xóa")]
        public Nullable<bool> DaXoa { get; set; }
        [Display(Name = "Hình Ảnh 1")]
        public string HinhAnh1 { get; set; }
        [Display(Name = "Hình Ảnh 2")]
        public string HinhAnh2 { get; set; }
        [Display(Name = "Hình Ảnh 3")]
        public string HinhAnh3 { get; set; }
        [Display(Name = "Hình Ảnh 4")]
        public string HinhAnh4 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BinhLuan> BinhLuans { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonDatHang> ChiTietDonDatHangs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        public virtual LoaiSanPham LoaiSanPham { get; set; }
        public virtual NhaCungCap NhaCungCap { get; set; }
        public virtual NhaSanXuat NhaSanXuat { get; set; }
    }
}