namespace ShoppingCart.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class UserMetaClass
    {

        public int UserId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Username")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Length must be between 4 and 30 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Password")]
        [StringLength(14, MinimumLength = 4, ErrorMessage = "Length must be between 4 and 15 characters")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "This field is required")]
        [DisplayName("Confirm Password")]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Password", ErrorMessage = "The passwords must match")]
        public string ConfirmPassword { get; set; }

    }
}