﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class RoleDto
    {
        public Guid RoleId { get; set; }
        public string? RoleName { get; set; }
        public ICollection<UserDto> Users { get; set; }
    }
}
