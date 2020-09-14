﻿using System;

namespace IdentityServer.Infrastructure.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Application { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
