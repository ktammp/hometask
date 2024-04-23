
using AutoMapper;
using BLL.Contracts;
using BLL.DTO;
using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class TicketService : ITicketService
{
    
  
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TicketService( IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }


    public async Task<TicketDTO> CreateTicketAsync(TicketDTO ticketDTO)
    {
        if (ticketDTO == null)
        {
            throw new ArgumentNullException(nameof(ticketDTO), "Ticket DTO cannot be null");
        }
        var ticket = _mapper.Map<Domain.Ticket>(ticketDTO);
        _context.Tickets.Add(ticket);
        var result = await _context.SaveChangesAsync();
        return _mapper.Map<TicketDTO>(ticket);
        
    }

    public async Task<IEnumerable<TicketDTO>> GetAllResolvedTicketsAsync()
    {
        var tickets = await _context.Tickets
            .Where(t => t.Resolved)
            .OrderBy(t => t.Deadline)
            .ToListAsync();
        return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
    }

    public async Task<IEnumerable<TicketDTO>> GetAllNotResolvedTicketsAsync()
    {
        var tickets = await _context.Tickets
            .Where(t => !t.Resolved)
            .OrderBy(t => t.Deadline)
            .ToListAsync();
        return _mapper.Map<IEnumerable<TicketDTO>>(tickets);
    }



    public async Task<TicketDTO> GetTicketByIdAsync(Guid? id)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id), "Ticket ID cannot be null");
        }
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(m => m.Id == id);
        

        return _mapper.Map<TicketDTO>(ticket);
       
    }

    public async Task<TicketDTO> MarkTicketResolvedAsync(Guid? id)
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id), "Ticket ID cannot be null");
        }
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(m => m.Id == id);
        if (ticket == null)
        {
            throw new ArgumentException("Ticket not found", nameof(id));
        }
        ticket!.Resolved = true;
        _context.Tickets.Update(ticket);
        var result = await _context.SaveChangesAsync();
        return _mapper.Map<TicketDTO>(ticket);
        
    }

  

    public async Task<TicketDTO> UpdateTicketAsync(TicketDTO ticketDTO)
    {
        if (ticketDTO == null)
        {
            throw new ArgumentNullException(nameof(ticketDTO), "Ticket DTO cannot be null");
        }
        var ticket = _mapper.Map<Domain.Ticket>(ticketDTO);
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
        return _mapper.Map<TicketDTO>(ticket);
    }

    public async Task<bool> DeleteTicketAsync(Guid id)
    {
        
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
        {
            return false;
        }
        _context.Tickets.Remove(ticket);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}