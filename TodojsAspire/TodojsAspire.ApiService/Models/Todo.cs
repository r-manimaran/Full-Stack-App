using System.ComponentModel.DataAnnotations;

namespace TodojsAspire.ApiService.Models;

public class Todo
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = default!;
    public bool IsComplete { get; set; } = false;
    [Required]
    public int PriorityPosition { get; set; } = 0;
}
