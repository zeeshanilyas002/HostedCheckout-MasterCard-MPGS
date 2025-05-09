using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using GlobalPay.HostedCheckouts.Mastercard.Models;
using Client.Models;

public class AwsSecretManagerService
{
    private readonly AwsSettings _awsSettings;

    public AwsSecretManagerService(IOptions<AwsSettings> awsOptions)
    {
        _awsSettings = awsOptions.Value;
    }

    public async Task<FabAwsServiceCreds> GetFABCredentialsAsync()
    {
        using var client = new AmazonSecretsManagerClient(
            _awsSettings.AccessKey,
            _awsSettings.SecretKey,
            RegionEndpoint.GetBySystemName(_awsSettings.Region));

        var request = new GetSecretValueRequest
        {
            SecretId = _awsSettings.SecretName,
            VersionStage = "AWSCURRENT"
        };

        try
        {
            var response = await client.GetSecretValueAsync(request);
            var secretJson = response.SecretString;

            // Deserialize into a Dictionary since ASP.NET Core config does not support colons
            var secretDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(secretJson);

            return new FabAwsServiceCreds
            {             
                BaseUrl = secretDictionary["apiBaseUrl"],
                APIKEY = secretDictionary["apiKey"],
                MerchantId = secretDictionary["merchantId"],
                Version = secretDictionary["apiVersion"],
                userName = secretDictionary["userName"],
                password = secretDictionary["Password"], 
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching AWS Secret: {ex.Message}");
            throw;
        }
    }

    public class FabAwsServiceCreds
    {
        [JsonProperty("HttpClient:MerchantId")]
        public string MerchantId { get; set; }

        [JsonProperty("HttpClient:BaseUrl")]
        public string BaseUrl { get; set; }

        [JsonProperty("HttpClient:APIKEY")]
        public string APIKEY { get; set; }

        [JsonProperty("BasicAuth:userName")]
        public string userName { get; set; }

        [JsonProperty("BasicAuth:password")]
        public string password { get; set; }
        [JsonProperty("BasicAuth:ApiVersion")]
        public string Version { get; set; }
    }
}
