﻿using System.ComponentModel.DataAnnotations;

namespace JwtRoleBased.Models
{
    public class RegistrationRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        public Role Role { get; set; }
    }                                                        

}
