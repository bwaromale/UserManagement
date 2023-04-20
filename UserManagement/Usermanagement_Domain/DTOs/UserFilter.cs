using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usermanagement_Domain.DTOs
{
    public class UserFilter
    {
        public int? Age { get; set; }
        public string? Gender { get; set; } 
        public string? MaritalStatus { get; set; } 
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
    }
}
