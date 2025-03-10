﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsTask.Domain.Entities
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public IEnumerable<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
}
