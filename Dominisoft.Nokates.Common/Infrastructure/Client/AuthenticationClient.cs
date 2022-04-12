using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;

namespace Dominisoft.Nokates.Common.Infrastructure.Client
{
    public class AuthenticationClient
    {
        private readonly string _authenticationUrl;

        public AuthenticationClient(string authenticationUrl)
        {
            _authenticationUrl = authenticationUrl;
        }

        public string GetToken(string user, string pass)
        {

            var values = new Dictionary<string, string>
  {
      { "Username", user },
      { "EncryptedPassword", pass}
  };
            var response = HttpHelper.Post(_authenticationUrl, values, string.Empty);

            return response?.Split('.').Length != 3 ? null : response.Trim('"');
        }
    }
}
