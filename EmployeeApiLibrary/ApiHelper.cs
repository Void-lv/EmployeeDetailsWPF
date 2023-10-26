using System.Net.Http.Headers;
using System.Configuration;

namespace EmployeeApiLibrary
{
    public class ApiHelper
    {
        public static HttpClient ApiClient { get; set; } = new HttpClient();

        public static void InitializeClient()
        {
            ApiClient = new HttpClient();

            ApiClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["BaseAddress"]);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["AuthentificationtokenToken"]);
        }

    }
}