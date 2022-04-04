using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;

namespace Dominisoft.Nokates.Common.Infrastructure.Client
{
    public class AuthenticationClient
    {
        private readonly string _authenticationUrl;

        public AuthenticationClient(string authenticationUrl)
        {
            _authenticationUrl = authenticationUrl;
        }

        public async Task<string> GetToken(string user, string pass)
        {

            var values = new Dictionary<string, string>
  {
      { "Username", user },
      { "EncryptedPassword", pass}
  };

            var content = new StringContent(values.Serialize(), Encoding.UTF8, "application/json");

            var client = new HttpClient();

            var response = await client.PostAsync(_authenticationUrl, content);

            var responseString = await response.Content.ReadAsStringAsync();
            return responseString.Trim('"');
        }
    }
}
