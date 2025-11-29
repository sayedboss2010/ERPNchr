using EF.Data;
using EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VM.ViewModels;


namespace ERPNchr.Areas.LookUP.Controllers
{
    [Area("LookUP")]
    public class BranchesController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        public IActionResult Index()
        {
            var data = (from l in _context.HrBranches
                        //where l.IsActive == true
                        orderby l.Id descending
                        select new BranchVM
                        {
                            Id = l.Id,
                            NameAr = l.NameAr,
                            NameEn = l.NameEn,
                            IsActive = l.IsActive,
                        }).ToList();

            return View(data);
        }


        [HttpGet]

        public ActionResult CreateEdite(int TypePage, int? BranchID)
        {
            BranchVM model = new BranchVM();

            // For Edit (TypePage = 2) or View (TypePage = 3)
            if ((TypePage == 2 || TypePage == 3) && BranchID.HasValue)
            {
                model = _context.HrBranches
                    .Where(b => b.Id == BranchID.Value)
                    .Select(b => new BranchVM
                    {
                        Id = b.Id,
                        NameAr = b.NameAr,
                        NameEn = b.NameEn,
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
        public async Task<ActionResult> CreateEdite(HrBranch HrBranchs)
        {
            if (!ModelState.IsValid)
            {
                return View(HrBranchs);
            }

            if (HrBranchs.Id == 0)   // CREATE
            {
                int newId = _context.Database
                    .SqlQueryRaw<int>("SELECT NEXT VALUE FOR dbo.HR_Branches_SEQ")
                    .AsEnumerable()
                    .First();

                var entity = new HrBranch
                {
                    Id = newId,
                    NameAr = HrBranchs.NameAr,
                    NameEn = HrBranchs.NameEn,
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                    CreatedUserId = 1,
                    IsActive = false
                };

                _context.HrBranches.Add(entity);
            }
            else                      // UPDATE
            {
                var entityold = _context.HrBranches.Find(HrBranchs.Id);

                if (entityold == null)
                {
                    return NotFound(); ;
                }

                entityold.NameAr = HrBranchs.NameAr;
                entityold.NameEn = HrBranchs.NameEn;

                entityold.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
                entityold.UpdatedUserId = 1;

                _context.HrBranches.Update(entityold);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Branches", new { area = "LookUP" });
        }


       

     
        [HttpPost]
        public async Task<IActionResult> ActivateBranch(int id, bool isActive)
        {
            var entity = await _context.HrBranches.FindAsync(id);

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
                redirectUrl = Url.Action("Index", "Branches", new { area = "LookUP" })
            });
        }


    }
}
