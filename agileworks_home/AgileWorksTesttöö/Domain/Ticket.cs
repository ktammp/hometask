
using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Ticket: BaseEntity
{


    [MaxLength(255), MinLength(5)]
    public string Description { get; set; } = default!;
    public DateTime CreationTime { get; set; } = DateTime.Now;
    public DateTime Deadline { get; set; } = default!;
    public bool Resolved { get; set; } = false;

   


}