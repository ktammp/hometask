using System.ComponentModel.DataAnnotations;
using Domain;

namespace BLL.DTO;

public class TicketDTO
{
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(255), MinLength(5)]
    public string Description { get; set; } = default!;
    public DateTime CreationTime { get; set; } = DateTime.Now;
    [Required]
    public DateTime Deadline { get; set; } = default!;
    public bool Resolved { get; set; } = false;
    
    public string IsTooLate()
    {
        return DateTime.Now.AddHours(1) > Deadline ? "tooLate" : "false";
    }
}