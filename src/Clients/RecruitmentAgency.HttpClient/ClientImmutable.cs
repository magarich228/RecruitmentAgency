using Newtonsoft.Json;

namespace RecruitmentAgency.HttpClient;

public partial class Client
{
    static partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings)
    {
        
    }

    public static string? Token { get; set; } 
    
    partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request,
        string url)
    {
        if (Token != null)
        {
            request.Headers.Add("Authorization", "Bearer " + Token);
        }
    }
}