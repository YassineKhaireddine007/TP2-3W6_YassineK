using JuliePro.Data;
using JuliePro.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JuliePro.Services
{
    public class RecordService
    {
        private readonly JulieProDbContext _context;

        public RecordService(JulieProDbContext context)
        {
            _context = context;
        }

        public async Task<List<Record>> GetAllAsync()
        {
            return await _context.Records
                .Include(r => r.Discipline)
                .Include(r => r.Trainer)
                .ToListAsync();
        }

        public async Task<Record?> GetByIdAsync(int id)
        {
            return await _context.Records
                .Include(r => r.Discipline)
                .Include(r => r.Trainer)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<RecordViewModel> BuildViewModelAsync(Record? record = null)
        {
            var disciplines = await _context.Disciplines
                .OrderBy(d => d.Name)
                .ToListAsync();

            var trainers = await _context.Trainers
                .OrderBy(t => t.LastName)
                .ThenBy(t => t.FirstName)
                .ToListAsync();

            return new RecordViewModel
            {
                Id = record?.Id,
                Date = record?.Date ?? DateTime.Now,
                Amount = record?.Amount ?? 0,
                Unit = record?.Unit ?? "",
                Discipline_Id = record?.Discipline_Id ?? 0,
                Trainer_Id = record?.Trainer_Id ?? 0,
                DisciplineName = record?.Discipline?.Name,
                TrainerFullName = record == null ? null : $"{record.Trainer.FirstName} {record.Trainer.LastName}",
                DisciplineList = new SelectList(disciplines, "Id", "Name", record?.Discipline_Id),
                TrainerList = new SelectList(trainers, "Id", "FullName", record?.Trainer_Id)
            };
        }

        public async Task CreateAsync(Record record)
        {
            _context.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Record record)
        {
            _context.Update(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _context.Records.FindAsync(id);
            if (record == null) return;
            _context.Records.Remove(record);
            await _context.SaveChangesAsync();
        }
    }
}
