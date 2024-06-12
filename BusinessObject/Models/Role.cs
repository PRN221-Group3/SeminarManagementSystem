﻿using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Role
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}