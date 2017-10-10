using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPro.ViewModels
{
    public class AddUser
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "")]
        [MinLength(4)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
        [MinLength(8)]


        public string Phone { get; set; }

        public string Picture { get; set; }

        [Required]
        public string Access { get; set; }

        public string Confirm { get; set; }
    }
}