using System.Net.Http;

namespace TaskManager;

public static class RootWebClient
{
    private static HttpClientHandler _clientHandler = new HttpClientHandler()
    {
        ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true 
    };

    public static HttpClient Client = new HttpClient(_clientHandler);
}