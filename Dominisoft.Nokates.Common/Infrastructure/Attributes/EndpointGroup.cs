﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominisoft.Nokates.Common.Infrastructure.Attributes
{
    public class EndpointGroup : Attribute
    {
        public EndpointGroup(params string[] groups)
        {
            Groups = groups.ToList();
        }

        public List<string> Groups { get; }
    }
}
