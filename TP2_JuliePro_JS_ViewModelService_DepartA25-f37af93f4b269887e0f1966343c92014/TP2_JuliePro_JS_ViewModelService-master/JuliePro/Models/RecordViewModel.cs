using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

        [Display(Name = "Discipline")]
        public string? DisciplineName { get; set; }

        [Display(Name = "Trainer")]
        public string? TrainerFullName { get; set; }

        
        public SelectList? DisciplineList { get; set; }
        public SelectList? TrainerList { get; set; }

        [ValidateNever]
        public Record Record { get; set; }
    }
}
