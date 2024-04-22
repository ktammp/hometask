using BLL.DTO;

namespace Tests.DTOTests;

public class TicketDTOTests
{
    [Test]
    public void IsToLate_should_return_tooLate()
    {
        var time = DateTime.Now;

        var ticket = new TicketDTO()
        {
            Description = "Test description",
            Deadline = time
        };
        
        Assert.That(ticket.IsTooLate(), Is.EqualTo("tooLate"));
    }
    
    [Test]
    public void IsToLate_should_return_false()
    {
        var time = DateTime.Now;

        var ticket = new TicketDTO()
        {
            Description = "Test description",
            Deadline = time.AddHours(20)
        };
        
        Assert.That(ticket.IsTooLate(), Is.EqualTo("false"));
    }
}