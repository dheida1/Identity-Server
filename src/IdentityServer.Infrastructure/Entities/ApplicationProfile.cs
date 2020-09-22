﻿using System;
using System.Collections.Generic;

namespace IdentityServer.Infrastructure.Entities
{
    public class ApplicationProfile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public bool CreatedInAD { get; set; }
        public IList<ApplicationProfileRole> ProfileRoles { get; set; }
    }
}
