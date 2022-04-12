using System;

namespace Dominisoft.Nokates.Common.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultConnectionString:Attribute
    {
        public readonly string Name;

        public DefaultConnectionString(string name)
        {
            Name = name;
        }
    }
}
