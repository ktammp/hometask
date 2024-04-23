using BLL.DTO;

namespace UnitTests.DTOTest;

public class TicketDtoTests
{
    [Fact]
    public void IsToLate_should_return_tooLate()
    {
        var time = DateTime.Now;

        var ticket = new TicketDTO()
        {
            Description = "Test description",
            Deadline = time
        };
        
        Assert.Equal("tooLate", ticket.IsTooLate());
    }
    
    [Fact]
    public void IsToLate_should_return_false()
    {
        var time = DateTime.Now;

        var ticket = new TicketDTO()
        {
            Description = "Test description",
            Deadline = time.AddHours(20)
        };
        
        Assert.Equal("false", ticket.IsTooLate());

    }
}