﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1903966_Milestone2.Models
{
    public class ApplicationUser : IdentityUser

    {
        public string? Name { get; set; }

        public string? Address { get; set; }

        public Gender Gender { get; set; }

        public string? Image { get; set; }

    }
}
