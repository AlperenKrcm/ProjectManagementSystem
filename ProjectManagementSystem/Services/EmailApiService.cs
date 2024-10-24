using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class EmailApiService
{
    private readonly HttpClient _httpClient;

    public EmailApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendEmailAsync(string email, string subject, string description)
    {
        var requestUri = "https://yourapiurl.com/api/Email"; // Buraya kendi API URL'nizi ekleyin

        var emailRequest = new
        {
            email = email,
            subject = subject,
            description = description
        };

        var jsonContent = JsonConvert.SerializeObject(emailRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(requestUri, content);

        
    }
    public async Task SendEmailMultiAsync(List<string> emailList, string subject, string description)
    {
        var requestUri = "https://yourapiurl.com/api/Email"; // Buraya kendi API URL'nizi ekleyin

        foreach (var email in emailList)
        {
            var emailRequest = new
            {
                email = email,
                subject = subject,
                description = description
            };

            var jsonContent = JsonConvert.SerializeObject(emailRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(requestUri, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"E-posta başarıyla gönderildi: {email}");
                }
                else
                {
                    Console.WriteLine($"E-posta gönderimi başarısız: {email} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"E-posta gönderim hatası: {email} - {ex.Message}");
            }
        }
    }


   
}
