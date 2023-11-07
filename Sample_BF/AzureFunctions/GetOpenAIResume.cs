using Sample_BF.AzureStorage;

namespace Sample_BF.AzureFunctions
{
    public class GetOpenAIResume
    {
        private static HttpClient sharedClient = new HttpClient();

        public async Task<string> GetAsync(Claim rk)
        {
            var response = await sharedClient.GetAsync("https://function-connect-openai-formacion-ia.azurewebsites.net/api/OpenAIConnect?query=" + rk.ClientComment);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            rk.OpenAIComment = jsonResponse;
            Console.WriteLine($"{jsonResponse}\n");
            AzureStorageHelper client = new AzureStorageHelper();
            try
            {
                var entity = client.insert(rk);
                return jsonResponse;
            }
            catch(Exception err)
            {
                return "False";
            }
            
        }
    }
}
