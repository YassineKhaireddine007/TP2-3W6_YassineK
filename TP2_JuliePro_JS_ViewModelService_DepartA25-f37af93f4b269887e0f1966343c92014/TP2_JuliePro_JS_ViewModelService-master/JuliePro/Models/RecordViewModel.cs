using Microsoft.AspNetCore.Mvc.Rendering;

namespace JuliePro.Models
{
    public class RecordViewModel
    {
        public int? Id { get; set; }

        
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Unit { get; set; } = "";
        public int Discipline_Id { get; set; }
        public int Trainer_Id { get; set; }

        
        public string? DisciplineName { get; set; }
        public string? TrainerFullName { get; set; }

        
        public SelectList? DisciplineList { get; set; }
        public SelectList? TrainerList { get; set; }
    }
}
