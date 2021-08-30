using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    public class UserAccount
    {
        [Key]
        public int UserID { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime? DateOfBirth { get; set; } // Optional
    }

    public class RegisterViewModel
    { // Does not go in DB do not add DbSet for this!!
        [Required]
        [StringLength(20)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(200)]
        [Display(Name = "Confirm Email")]
        public string Email { get; set; }

        [Required]
        [Compare(nameof(Email))]
        [Display(Name = "Confirm Email")]
        public string ConfirmEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(120, MinimumLength = 6, ErrorMessage = "Password must be at least {2} characters.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Date)] // Time ignored
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; } // Optional
    }

    public class LogInViewModel
    { // Does not go in DB do not add DbSet for this!!
        [Required]
        [Display(Name = "Username or Email ")]
        public string UserNameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
