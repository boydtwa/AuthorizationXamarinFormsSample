﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AuthXamSam.Models
{
    public class User
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string UserPrincipalName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
