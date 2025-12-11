using EF.Data;
using EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VM.ViewModels;

namespace ERPNchr.Areas.Attendance.Controllers
{
    [Area("Attendance")]
    //Attendance/EmployeeActivities/IndexDepartment
    public class EmployeeActivitiesController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();


        public async Task<IActionResult> Index(int? departmentId, int? branchId, int? employeeId, string recordType)
        {
            var query = _context.VwEmployeeActivities.AsQueryable();

            if (departmentId.HasValue)
                query = query.Where(e => e.DepartmentId == departmentId.Value);

            if (branchId.HasValue)
                query = query.Where(e => e.BranchId == branchId.Value);

            if (employeeId.HasValue)
                query = query.Where(e => e.EmployeeId == employeeId.Value);

            if (!string.IsNullOrEmpty(recordType))
                query = query.Where(e => e.RecordType == recordType);

            var activities = await query.OrderBy(e => e.EmployeeId).ThenBy(e => e.StartDate).ToListAsync();

            return View(activities);
        }

        public async Task<IActionResult> IndexDepartment()
        {
            // جلب الأنشطة مع ترتيب
            var activities = await _context.VwEmployeeActivities
                .OrderBy(a => a.DepartmentId)
                .ThenBy(a => a.EmployeeName)
                .ThenBy(a => a.StartDate)
                .ToListAsync();

            // حماية من null
            activities = activities ?? new List<VwEmployeeActivity>();

            // إجماليات لكل نوع سجل
            var summary = activities
                .GroupBy(a => a.RecordType ?? "غير محدد")
                .Select(g => new
                {
                    RecordType = g.Key,
                    Count = g.Count()
                })
                .ToList();

            // بيانات كل إدارة
            var byDepartment = activities
                .GroupBy(a => a.DepartmentName ?? "غير محدد")
                .Select(g => new
                {
                    DepartmentName = g.Key,
                    Records = g.ToList()
                })
                .ToList();

            //ViewBag.Summary = summary;
            //ViewBag.ByDepartment = byDepartment;

            //return View(activities);

            var model = new EmployeeActivityReportVM
            {
                Activities = activities,
                Summary = summary.Select(s => new RecordSummaryVM { RecordType = s.RecordType, Count = s.Count }).ToList(),
                ByDepartment = byDepartment.Select(d => new DepartmentRecordsVM { DepartmentName = d.DepartmentName, Records = d.Records }).ToList()
            };

            return View(model);

        }



    }
}
