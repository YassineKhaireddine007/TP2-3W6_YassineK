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
            filter.VerifyProperties();

            var result = new TrainerSearchViewModel(filter);

            int pageIndex = filter.SelectedPageIndex;
            int pageSize = filter.SelectedPageSize;

            result.Items = await _dbContext.Trainers.ToPaginatedAsync(pageIndex, pageSize);

            result.AvailablePageSizes = new SelectList(new List<int>() { 9, 12, 18, 21 });
            result.Disciplines = new SelectList(new List<Discipline>(), "Id", "Name");
            result.Certifications = new SelectList(new List<Certification>(), "Id", "FullTitle");
            result.CertificationCenters = new SelectList(new List<string>());

            return result;
        }

    }
}
