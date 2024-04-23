
using BLL.Contracts;
using BLL.DTO;
using Microsoft.AspNetCore.Mvc;


namespace WebApp.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _service;

        public TicketsController(ITicketService service)
        {
            _service = service;
        }


        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllResolvedTicketsAsync());
        }


        

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _service.GetTicketByIdAsync(id);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Description,CreationTime,Deadline,Resolved,Id")] TicketDTO ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.UpdateTicketAsync(ticket);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ticket = await _service.GetTicketByIdAsync(id);
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await   _service.DeleteTicketAsync(id);
            return RedirectToAction(nameof(Index));
        }

      
    }
}
