using JuliePro.Data;
using JuliePro.Models;
using JuliePro.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JuliePro.Services
{
    public class TrainerService : ServiceBaseEF<Trainer>
    {
        private IImageFileManager fileManager;

        public TrainerService(JulieProDbContext dbContext, IImageFileManager fileManager) : base(dbContext)
        {
            this.fileManager = fileManager;
        }

        public async Task<Trainer> CreateAsync(Trainer model, IFormCollection form)
        {
            model.Photo = await fileManager.UploadImage(form, false, null);

            return await base.CreateAsync(model);
        }

        public async Task EditAsync(Trainer model, IFormCollection form)
        {
            var old = await _dbContext.Trainers.Where(x => x.Id == model.Id).Select(x => x.Photo).FirstOrDefaultAsync();
            model.Photo = await fileManager.UploadImage(form, true, old);
            await EditAsync(model);
        }

        public async Task<TrainerSearchViewModel> GetAllAsync(TrainerSearchViewModelFilter filter)
        {
            filter.VerifyProperties(); //mets à null les éléments qui sont vides. 

            var result = new TrainerSearchViewModel(filter);



            // TODO: Remplacer par ce code une fois que vous commencez à implémenter la pagination
            int pageIndex = filter.SelectedPageIndex;
            int pageSize = filter.SelectedPageSize;

            //TODO: Ajouter les filtres
            result.Items = await _dbContext.Trainers
                .Where(x => filter.SearchNameText == null || x.FirstName.ToLower() == filter.SearchNameText || x.LastName.ToLower() == filter.SearchNameText)
                .Where(x => filter.SelectedGender == null || x.Genre == filter.SelectedGender)
                .Where(x => filter.SelectedDisciplineId == null || x.Discipline_Id == filter.SelectedDisciplineId)
                .Where(x => filter.SelectedCertificationId == null || x.Id == filter.SelectedCertificationId)
                .Where(x => filter.SelectedCertificationCenter == null || x.TrainerCertifications.Any(d => d.Certification.CertificationCenter == filter.SelectedCertificationCenter))
                .ToPaginatedAsync(pageIndex, pageSize) ;

            



            result.AvailablePageSizes = new SelectList(new List<int>() { 9, 12, 18, 21 });

            result.Disciplines = new SelectList(
                _dbContext.Disciplines.ToList(),
                "Id",
                "Name"
            );

            result.Certifications = new SelectList(
                _dbContext.Certifications.ToList(),
                "Id",
                "FullTitle"
            );

            result.CertificationCenters = new SelectList(
                _dbContext.Certifications
                    .Select(c => c.CertificationCenter)
                    .Distinct()
                    .ToList()
            );

            return result;
        }
    }
}
