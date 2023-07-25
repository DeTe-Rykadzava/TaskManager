using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReactiveUI;
using TaskManager.ViewModels;

namespace TaskManager.Models;

public class TaskModel : INotifyPropertyChanged
{
    public Guid Id { get; set; }
    
    [JsonIgnore]
    private string _title;
    
    public string Title
    {
        get => _title;
        set => SetField(ref _title, value);
    }

    [JsonIgnore]
    private string? _description = null;
    public string? Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public DateOnly Date { get; set; }

    public TaskStatusModel Status { get; set; }
    

    public static async Task<IEnumerable<TaskModel>> GetTasks(Guid userId)
    {
        try
        {
            var result = await 
                RootWebClient.Client
                    .GetAsync($"https://localhost:7132/api/Task/{userId}");
            
            if(!result.IsSuccessStatusCode)
                return Array.Empty<TaskModel>();

            var tasks = JsonConvert.DeserializeObject<TaskModel[]>
                (await result.Content.ReadAsStringAsync());

            return tasks ?? Array.Empty<TaskModel>();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            // true - чтобы файл дополнялся, а не перезаписывался
            using (var sw = new StreamWriter("TasksGetErr.log",true))
            {
                sw.WriteLine($"[{DateTime.Now}] Error: {e.Message}\t InnerEX: {e.InnerException}");
            }
            return Array.Empty<TaskModel>();
        }
    }

    public static async Task<bool> UpdateTask(TaskModel task, Guid userId)
    {
        try
        {
            var requestBody = new StringContent(
                JsonConvert.SerializeObject(task));
            
            requestBody.Headers.Clear();
            requestBody.Headers.Add("Content-Type","application/json");

            var result = await RootWebClient.Client
                .PutAsync($"https://localhost:7132/api/Task?userId={userId}", requestBody);
            
            if(!result.IsSuccessStatusCode)
                return false;

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            // true - чтобы файл дополнялся, а не перезаписывался
            using (var sw = new StreamWriter("TasksUpdateErr.log",true))
            {
                sw.WriteLine($"[{DateTime.Now}] Error: {e.Message}\t InnerEX: {e.InnerException}");
            }

            return false;
        }
    }

    public static async Task<bool> DeleteTask(Guid taskId)
    {
        try
        {
            var result = await RootWebClient.Client
                .DeleteAsync($"https://localhost:7132/api/Task?id={taskId}");
            
            if(!result.IsSuccessStatusCode)
                return false;

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            // true - чтобы файл дополнялся, а не перезаписывался
            using (var sw = new StreamWriter("TasksDeleteErr.log",true))
            {
                sw.WriteLine($"[{DateTime.Now}] Error: {e.Message}\t InnerEX: {e.InnerException}");
            }

            return false;
        }
    }

    public static async Task<TaskModel?> CreateTask(TaskRequestModel task)
    {
        try
        {
            var requestBody = new StringContent(JsonConvert.SerializeObject(task));
            requestBody.Headers.Clear();
            requestBody.Headers.Add("Content-Type","application/json");

            var result = await RootWebClient.Client
                .PostAsync("https://localhost:7132/api/Task",requestBody);

            if (!result.IsSuccessStatusCode)
                return null;

            var taskModel = JsonConvert.DeserializeObject<TaskModel>(
                await result.Content.ReadAsStringAsync());

            return taskModel;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            // true - чтобы файл дополнялся, а не перезаписывался
            using (var sw = new StreamWriter("TasksCreateErr.log",true))
            {
                sw.WriteLine($"[{DateTime.Now}] Error: {e.Message}\t InnerEX: {e.InnerException}");
            }

            return null;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}