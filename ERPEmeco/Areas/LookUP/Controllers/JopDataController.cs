using EF.Data;
using EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VM.ViewModels;


namespace ERPNchr.Areas.LookUP.Controllers
{
    [Area("LookUP")]
    public class JopDataController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        public IActionResult Index()
        {
            var data = (from l in _context.HrJobs
                        //where l.IsActive == true
                        orderby l.Id descending
                        select new JopDataVM
                        {
                            Id = l.Id,
                            TitleAr = l.TitleAr,
                            TitleEn = l.TitleEn,
                            IsActive = l.IsActive,
                        }).ToList();

            return View(data);
        }


        [HttpGet]

        public ActionResult CreateEdite(int TypePage, int? JobID)
        {
            JopDataVM model = new JopDataVM();

            // For Edit (TypePage = 2) or View (TypePage = 3)
            if ((TypePage == 2 || TypePage == 3) && JobID.HasValue)
            {
                model = _context.HrJobs
                    .Where(b => b.Id == JobID.Value)
                    .Select(b => new JopDataVM
                    {
                        Id = b.Id,
                        TitleAr = b.TitleAr,
                        TitleEn = b.TitleEn,
                        IsActive = b.IsActive,
                        
                    })
                    .FirstOrDefault();

                if (model == null)
                    return NotFound();
            }

            return View(model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateEdite(HrJob HRJobs)
        {
            if (!ModelState.IsValid)
            {
                return View(HRJobs);
            }

            if (HRJobs.Id == 0)   // CREATE
            {
                int newId = _context.Database
                    .SqlQueryRaw<int>("SELECT NEXT VALUE FOR dbo.HR_Jobs_SEQ")
                    .AsEnumerable()
                    .First();

                var entity = new HrJob
                {
                    Id = newId,
                    TitleAr = HRJobs.TitleAr,
                    TitleEn = HRJobs.TitleEn,
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                    CreatedUserId = 1,
                    IsActive = true
                };

                _context.HrJobs.Add(entity);
            }
            else                      // UPDATE
            {
                var entityold = _context.HrJobs.Find(HRJobs.Id);

                if (entityold == null)
                {
                    return NotFound(); ;
                }

                entityold.TitleAr = HRJobs.TitleAr;
                entityold.TitleEn = HRJobs.TitleEn;

                entityold.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
                entityold.UpdatedUserId = 1;

                _context.HrJobs.Update(entityold);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "JopData", new { area = "LookUP" });
        }


       

     
        [HttpPost]
        public async Task<IActionResult> ActivateBranch(int id, bool isActive)
        {
            var entity = await _context.HrJobs.FindAsync(id);

            if (entity == null)
                return Json(new { success = false, message = "Not found" });

            // عكس الحالة (Toggle)
            entity.IsActive = !isActive;

            entity.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
            entity.UpdatedUserId = 1;

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("Index", "JopData", new { area = "LookUP" })
            });
        }


    }
}
