using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPro.Models
{
    public class Admin
    {
        [Key]
        [MinLength(3)]
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا نام کاربری را وارد نمایید")]
        public string Username { get; set; }

        [MinLength(6)]
        [MaxLength(20)]
        [Display(Name = "رمزعبور")]
        [Required(ErrorMessage = "لطفا رمز عبور را وارد نمایید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [MinLength(3)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [MinLength(8)]
        [MaxLength(14)]
        [Display(Name = "موبایل")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "مدیر منطقه")]
        public string Access { get; set; }
    }
}