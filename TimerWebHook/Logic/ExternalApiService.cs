namespace TimerWebHook.Logic
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ExternalApiService
    {
        private readonly HttpClient _httpClient;

        public ExternalApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> CallExternalApi(string apiUrl)
        {
            try
            {
                // Make the HTTP GET request
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                // Check if the request was successful (status code 200-299)
                if (response.IsSuccessStatusCode)
                {
                    // Read and return the content as a string
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Handle the error, throw an exception, or log the issue
                    throw new HttpRequestException($"Failed to call the API. Status Code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., network issues, timeouts, etc.)
                throw new Exception("An error occurred while calling the external API.", ex);
            }
        }
    }
}
