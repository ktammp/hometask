using BLL.DTO;

namespace BLL.Contracts;

public interface ITicketService
{
    Task<TicketDTO> CreateTicketAsync(TicketDTO ticketDto);

    Task<IEnumerable<TicketDTO>> GetAllResolvedTicketsAsync();
    Task<IEnumerable<TicketDTO>> GetAllNotResolvedTicketsAsync();
    Task<TicketDTO> MarkTicketResolvedAsync(Guid? id);
   

    Task<TicketDTO> GetTicketByIdAsync(Guid? id);

    Task<TicketDTO> UpdateTicketAsync(TicketDTO ticketDto);

    Task<bool> DeleteTicketAsync(Guid id);
}