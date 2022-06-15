using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newspaper.DTO.User
{
    public class RegisterReaderDto
    {
        [Required(ErrorMessage = "Please enter full name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please enter address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please enter email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Phone")]
        //[RegularExpression("/^01[0125][0-9]{8}$", ErrorMessage = "Please enter correct phone number")]
        public string Phone { get; set; }
        public string ProfilePicture { get; set; }
        public string ProfilePhotoBase64 { get; set; }
    }
}
