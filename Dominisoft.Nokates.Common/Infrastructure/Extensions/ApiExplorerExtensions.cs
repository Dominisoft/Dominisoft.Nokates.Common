using System.Collections.Generic;
using System.Linq;
using Dominisoft.Nokates.Common.Infrastructure.Attributes;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Dominisoft.Nokates.Common.Infrastructure.Extensions
{
    public static class ApiExplorerExtensions
    {
        public static List<ActionModel> GetActions(this ApplicationModel application)
        {
            return application.Controllers.SelectMany(c => c.Actions).ToList();
        }

        public static List<ActionModel> FilterOutWebCommon(this List<ActionModel> actions)
        => actions.Where(a => !a.DisplayName.EndsWith("(Dominisoft.WebCommon)")).ToList();

        public static Dictionary<string,List<string>> GetEndpointGroups(this List<ActionModel>  actions)
{
            var appName = AppHelper.GetAppName();
            var endpointGroups = new Dictionary<string, List<string>>();
            foreach (var action in actions)
            {
                var groupAttribute = (EndpointGroup)action.Attributes.FirstOrDefault(a => a.GetType() == typeof(EndpointGroup));
                if (groupAttribute == null) continue;
                var groups = groupAttribute.Groups;
               foreach (var group in groups)
                {
                    var name = $"{appName}:{action.Controller.ControllerName}.{action.ActionName}";
                    if (!endpointGroups.ContainsKey(group))
                        endpointGroups.Add(group, new List<string> {name });
                    else
                        endpointGroups[group].Add(name);
                }
            }

            return endpointGroups;
        }        


    }
}
