using EF.Data;
using Microsoft.AspNetCore.Mvc;

namespace ERPNchr.Areas.Employee.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return View(new GlobalSearchVM());

            var model = new GlobalSearchVM
            {
                Jobs = _context.HrJobs
                    .Where(x => x.TitleAr.Contains(q) || x.TitleEn.Contains(q))
                    .Take(5)
                    .ToList(),

                Departments = _context.HrDepartments
                    .Where(x => x.NameAr.Contains(q) || x.NameEn.Contains(q))
                    .Take(5)
                    .ToList(),

                EmployeeAttendance = _context.HrEmployees
                    .Where(x => x.IsActive && (x.NameAr.Contains(q) || x.NameEn.Contains(q)))
                    .OrderByDescending(x => x.Id)
                    .Take(5)
                    .ToList()
            };

            ViewBag.Query = q;
            return View(model);
        }
    }

}
