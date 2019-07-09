using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAnCuoiKy.Models
{
    public class ResetPasswordModel
    {
        [Display(Name ="Mật Khẩu Mới")]
        [Required(ErrorMessage = "New password required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Nhập Lại Mật Khẩu Mới")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage ="New password and confirm password does not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ResetCode { get; set; }
    }
}