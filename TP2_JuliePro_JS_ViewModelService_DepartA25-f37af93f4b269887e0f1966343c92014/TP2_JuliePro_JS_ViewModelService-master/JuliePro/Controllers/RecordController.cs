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

        public RecordController(JulieProDbContext context, RecordService service)
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
        public async Task<IActionResult> Details(int id)
        {
            var vm = await _service.GetByIdAsync(id);
            if (vm == null)
                return NotFound();

            return View(vm);
        }


        // GET: Record/Create
        public async Task<IActionResult> Create()
        {
            var vm = await _service.BuildViewModelAsync();
            return View(vm);
        }


        // POST: Record/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecordViewModel recordVM)
        {
            if (ModelState.IsValid)
            {
                var record = new Record
                {
                    Date = recordVM.Date,
                    Discipline_Id = recordVM.Discipline_Id,
                    Amount = recordVM.Amount ,
                    Unit = recordVM.Unit ,
                    Trainer_Id = recordVM.Trainer_Id,

                };

                _context.Add(record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            recordVM.DisciplineList = new SelectList(_context.Disciplines, "Id", "Name", recordVM.Discipline_Id);
            recordVM.TrainerList = new SelectList(_context.Trainers, "Id", "TrainerFullName", recordVM.Trainer_Id);
            return View(recordVM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var record = await _context.Records.FindAsync(id.Value);
            if (record == null) return NotFound();

            var vm = await _service.BuildViewModelAsync(record);
            return View(vm);
        }

        // POST: Record/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Record @record)
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _context.Records
                .Include(x => x.Discipline)
                .Include(x => x.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (record == null)
            {
                return NotFound();
            }


            var viewModel = new RecordViewModel
            {

                Id = record.Id,
                Date = record.Date,
                Discipline_Id = record.Discipline.Id,
                DisciplineName = record.Discipline.Name,
                Amount = record.Amount,
                Unit = record.Unit,
                Trainer_Id = record.Trainer.Id,
                TrainerFullName = record.Trainer.FullName

            };

            return View(viewModel);
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

        public async Task<IActionResult> TrainerIndex(int trainerId)
        {
            var trainer = await _context.Trainers
                .FirstOrDefaultAsync(t => t.Id == trainerId);

            if (trainer == null)
                return NotFound();

            var records = await _context.Records
                .Where(r => r.Trainer_Id == trainerId)
                .Include(r => r.Discipline)
                .Include(r => r.Trainer)
                .OrderByDescending(r => r.Date)
                .ToListAsync();

            
            var recordsVm = records.Select(r => new RecordViewModel
            {
                Id = r.Id,
                Date = r.Date,
                Discipline_Id = r.Discipline.Id,
                DisciplineName = r.Discipline.Name,
                Amount = r.Amount,
                Unit = r.Unit,
                Trainer_Id = r.Trainer.Id,
                TrainerFullName = r.Trainer.FullName

            }).ToList();

            return View(recordsVm);
        }

    }
}
