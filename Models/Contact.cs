using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace ContactManagerApp.Models
    {
    public class Contact
        {
        public int ContactId { get; set; }

        [Required(ErrorMessage ="Please enter a first name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a last name")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a phone number")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Please enter an email")]
        public string Email { get; set; } = null!;
        public int CategoryId { get; set;}

        [ValidateNever]
        public Category Category { get; set; } = null!;

        public DateTime DateAdded { get; set; }

        public string? ProfilePhoto { get; set; } = null!;
        }
    }
