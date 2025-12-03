using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JuliePro.Data;
using JuliePro.Models;
using JuliePro.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JuliePro.Controllers
{
    public class RecordController : Controller
    {
        private readonly JulieProDbContext _context;
        private readonly RecordService _service;

        public RecordController(JulieProDbContext context , RecordService service)
        {
            _context = context;
            _service = service;
        }




        // GET: Record
        public async Task<IActionResult> Index()
        {
            var records = await _service.GetAllAsync();
            return View(records);
        }


        // GET: Record/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @record = await _context.Records
                .Include(x => x.Discipline)
                .Include(x => x.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@record == null)
            {
                return NotFound();
            }

            return View(@record);
        }

        // GET: Record/Create
        public IActionResult Create()
        {
            ViewData["Discipline_Id"] = new SelectList(_context.Disciplines, "Id", "Id");
            ViewData["Trainer_Id"] = new SelectList(_context.Trainers, "Id", "Email");
            return View();
        }

        // POST: Record/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecordViewModel @record)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Discipline_Id"] = new SelectList(_context.Disciplines, "Id", "Id", @record.Discipline_Id);
            ViewData["Trainer_Id"] = new SelectList(_context.Trainers, "Id", "Email", @record.Trainer_Id);
            return View(@record);
        }

        // GET: Record/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @record = await _context.Records.FindAsync(id);
            if (@record == null)
            {
                return NotFound();
            }
            ViewData["Discipline_Id"] = new SelectList(_context.Disciplines, "Id", "Id", @record.Discipline_Id);
            ViewData["Trainer_Id"] = new SelectList(_context.Trainers, "Id", "Email", @record.Trainer_Id);
            return View(@record);
        }

        // POST: Record/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Discipline_Id,Amount,Unit,Trainer_Id")] Record @record)
        {
            if (id != @record.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@record);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordExists(@record.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Discipline_Id"] = new SelectList(_context.Disciplines, "Id", "Id", @record.Discipline_Id);
            ViewData["Trainer_Id"] = new SelectList(_context.Trainers, "Id", "Email", @record.Trainer_Id);
            return View(@record);
        }

        // GET: Record/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @record = await _context.Records
                .Include(x => x.Discipline)
                .Include(x => x.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@record == null)
            {
                return NotFound();
            }

            return View(@record);
        }

        // POST: Record/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @record = await _context.Records.FindAsync(id);
            if (@record != null)
            {
                _context.Records.Remove(@record);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordExists(int id)
        {
            return _context.Records.Any(e => e.Id == id);
        }
    }
}
