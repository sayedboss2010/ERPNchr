using EF.Data;
using EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VM.ViewModels;

namespace ERPNchr.Areas.LookUP.Controllers
{
    [Area("LookUP")]

    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        public IActionResult Index()
        {
            var data = (from l in _context.HrDepartments                     
                        //where l.IsActive == true
                        orderby l.Id descending
                        select new DepartmentVM
                        {
                            Id = l.Id,
                            NameAr=l.NameAr,
                            NameEn=l.NameEn,
                            IsActive=l.IsActive,
                        }).ToList();

            return View(data);
        }
        [HttpGet]

        public ActionResult CreateEdite(int TypePage, int? DepartmentID)
        {
            DepartmentVM model = new DepartmentVM();

            // Edit or View
            if ((TypePage == 2 || TypePage == 3) && DepartmentID.HasValue)
            {
                model = (from l in _context.HrDepartments
                         where l.Id == DepartmentID.Value
                         select new DepartmentVM
                         {
                             Id = l.Id,
                             NameAr = l.NameAr,
                             NameEn = l.NameEn,
                             IsActive = l.IsActive,
                         }).Where(a=>a.Id== DepartmentID).FirstOrDefault();

                if (model == null)
                    return NotFound();
            }

            return View(model);
        }

        [HttpPost]
      
        public async Task<IActionResult> ActivateDepartment(int id, bool isActive)
        {
            var entity = await _context.HrDepartments.FindAsync(id);

            if (entity == null)
                return Json(new { success = false, message = "Department not found" });

            // يمكنك استخدام isActive هنا إذا أردت
            // مثال: تفعيل إذا كان غير نشط، أو العكس
            entity.IsActive = isActive; // أو استخدم قيمة isActive

            entity.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
            entity.UpdatedUserId = 1;

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("Index", "Department", new { area = "LookUP" })
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateEdite(DepartmentVM Departments)
        {
            if (!ModelState.IsValid)
            {
                return View(Departments);
            }

            if (Departments.Id == 0)   // CREATE
            {
                int newId = _context.Database
                    .SqlQueryRaw<int>("SELECT NEXT VALUE FOR dbo.HR_Departments_SEQ")
                    .AsEnumerable()
                    .First();

                var entity = new HrDepartment
                {
                    Id = newId,
                    NameAr = Departments.NameAr,
                    NameEn = Departments.NameEn,
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                    CreatedUserId = 1,
                    IsActive = true
                };

                _context.HrDepartments.Add(entity);
            }
            else                      // UPDATE
            {
                var entityold = _context.HrDepartments.Find(Departments.Id);

                if (entityold == null)
                {
                     return NotFound(); ;
                }

                entityold.NameAr = Departments.NameAr;
                entityold.NameEn = Departments.NameEn;

                entityold.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
                entityold.UpdatedUserId = 1;

                _context.HrDepartments.Update(entityold);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Department", new { area = "LookUP" });
        }

    }
}
