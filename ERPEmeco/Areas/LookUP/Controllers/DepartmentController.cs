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
                        where l.IsActive == true
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
                         }).FirstOrDefault();

                if (model == null)
                    return NotFound();
            }

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateEdite(DepartmentVM Departments)
        {
            int _hR_Departments = _context.Database.SqlQueryRaw<int>("SELECT NEXT VALUE FOR dbo.HR_Departments_SEQ").AsEnumerable().First();
            var entity = new HrDepartment
            {
                Id = _hR_Departments,
                NameAr = Departments.NameAr,               
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                CreatedUserId = 1, 
                IsActive = true
            };
            _context.HrDepartments.Add(entity);
            return RedirectToAction("Index", "Department", new { area = "LookUP"});
        }
    }
}
