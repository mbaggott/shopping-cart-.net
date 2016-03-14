namespace ShoppingCart.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CustomerMetaClass
    {

        public int CustId { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters")]
        public string Suburb { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters")]
        public string State { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Range(0, 999999, ErrorMessage = "Postcode must be a number betwen 0000 and 999999")]
        public int Postcode { get; set; }

    }
}