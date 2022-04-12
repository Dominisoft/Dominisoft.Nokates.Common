using System;
using System.Collections.Generic;
using System.Text;

namespace Dominisoft.Nokates.Common.Infrastructure.Attributes
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class JsonColumnAttribute : Attribute
    {
        public JsonColumnAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
