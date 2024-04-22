using System.Diagnostics;
using BLL.Contracts;
using BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ITicketService _service;


    public HomeController( ITicketService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _service.GetAllNotResolvedTicketsAsync());
    }

    public async Task<IActionResult> MarkAsResolved(Guid id)
    {
        await _service.MarkTicketResolvedAsync(id);
        return RedirectToAction(nameof(Index));
    }
    
    // GET: Home/CreateTicket
    public IActionResult CreateTicket()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTicket(TicketDTO ticket)
    {
        if (ModelState.IsValid)
        {
            await _service.CreateTicketAsync(ticket);
            return RedirectToAction(nameof(Index));;
           
        }
        return View(ticket);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}