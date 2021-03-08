using System;
using System.Collections.Generic;
using System.Text;

namespace PayFair.DTO.Users
{
    public class AuthTokenDTO
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }

        public AuthTokenDTO(string token, string message = null)
        {
            Token = token;
            Message = message;
        }
    }
}
