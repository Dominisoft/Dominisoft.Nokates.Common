using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Dominisoft.Nokates.Common.Infrastructure.Extensions
{
    public static class DynamicExtensions
    {
        public static dynamic ToDynamic(this Dictionary<string, object> values)
        {
            //var eo = new ExpandoObject();
            //var eoColl = (ICollection<KeyValuePair<string, object>>)eo;

            //foreach (var valuePair in values)
            //{
            //    eo.TryAdd()
            //    eoColl.Add(valuePair);
            //}

            dynamic eoDynamic = values;
            return eoDynamic;
        }
    }
}
