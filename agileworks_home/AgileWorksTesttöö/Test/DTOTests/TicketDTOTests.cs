using BLL.DTO;
using Xunit;

namespace Tests.DTOTests;

public class TicketDTOTests


{
    public void IsToLate_should_return_tooLate()
    {
        var time = DateTime.Now;

        var ticket = new TicketDTO()
        {
            Description = "Test description",
            Deadline = time
        };
        
        Assert.Equal("isLate", ticket.IsTooLate());
    }
    
    public void IsToLate_should_return_false()
    {
    }
}