using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TaskManager.Models;

public class UserModel
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    
    // login method
    public static async Task<UserModel?> GetUser(string login, string password)
    {
        var requestBody = new StringContent(JsonConvert.SerializeObject(new UserRequestModel()
        {
            Login = login,
            Password = password
        }));
        
        requestBody.Headers.Clear();
        requestBody.Headers.Add("Content-Type","application/json");

        try
        {
            // send request
            var result = await RootWebClient.Client
                .PostAsync("https://localhost:7132/api/User?action=0", requestBody);

            if (!result.IsSuccessStatusCode)
                return null;

            var user = JsonConvert.DeserializeObject<UserModel>(
                await result.Content.ReadAsStringAsync());

            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            // true - чтобы файл дополнялся, а не перезаписывался
            using (var sw = new StreamWriter("UserAuthErr.log",true))
            {
                sw.WriteLine($"[{DateTime.Now}] Error: {e.Message}\t InnerEX: {e.InnerException}");
            }
            return null;
        }
    }
    
    // registration method
    public static async Task<UserModel> CreateUser(string userName, string login, string password)
    {
        var requestBody = new StringContent(JsonConvert.SerializeObject(new UserRequestModel()
        {
            UserName = userName,
            Login = login,
            Password = password
        }));
        
        requestBody.Headers.Clear();
        requestBody.Headers.Add("Content-Type","application/json");
        
        try
        {
            // send request
            var result = await RootWebClient.Client
                .PostAsync("https://localhost:7132/api/User?action=1", requestBody);

            if (!result.IsSuccessStatusCode)
                return null;

            var user = JsonConvert.DeserializeObject<UserModel>(
                await result.Content.ReadAsStringAsync());

            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            // true - чтобы файл дополнялся, а не перезаписывался
            using (var sw = new StreamWriter("UserRegErr.log",true))
            {
                sw.WriteLine($"[{DateTime.Now}] Error: {e.Message}\t InnerEX: {e.InnerException}");
            }
            return null;
        }
    }
}