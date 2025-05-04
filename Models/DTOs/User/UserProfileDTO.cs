using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.User
{
    public class UserProfileDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public int? ImageId { get; set; }
        public List<string> Allergies { get; set; }
        public List<string> Diseases { get; set; }
    }
}