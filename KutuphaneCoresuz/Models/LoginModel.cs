using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KutuphaneCoresuz.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Lütfen Kullanici adinizi giriniz.")]
        [Display(Name = "E-Mail")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi giriniz.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        public bool rememberMe { get; set; } 

    }
}
