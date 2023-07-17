using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models;

public class UserModel
{
    public string? UserName { get; set; } = null;
    
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}