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

        public async Task<IActionResult> Index( string recordType)
        {
            // قراءة الكوكيز
            int? userId = Request.Cookies.ContainsKey("UserId") ? int.Parse(Request.Cookies["UserId"]) : null;
            int? userType = Request.Cookies.ContainsKey("UserType") ? int.Parse(Request.Cookies["UserType"]) : null;
            int? branchId = Request.Cookies.ContainsKey("BranchID") ? int.Parse(Request.Cookies["BranchID"]) : null;
            int? departmentId = Request.Cookies.ContainsKey("DepartmentID") ? int.Parse(Request.Cookies["DepartmentID"]) : null;

            var query = _context.VwEmployeeActivities.AsQueryable();

            // 🔹 تطبيق صلاحيات المستخدم
            switch (userType)
            {
                case 1: // موظف - يشوف بس الطلبات الخاصة به
                    if (userId.HasValue)
                        query = query.Where(x => x.EmployeeId == userId.Value);
                    break;

                case 2: // مدير إدارة - يشوف كل الموظفين في الإدارة والفرع
                    if (departmentId.HasValue)
                        query = query.Where(x => x.DepartmentId == departmentId.Value);

                    if (branchId.HasValue)
                        query = query.Where(x => x.BranchId == branchId.Value);
                    break;

                case 3: // رئيس قطاع - يشوف كل الموظفين
                        // لا نضيف أي فلاتر
                    break;
            }

           

            if (!string.IsNullOrEmpty(recordType))
                query = query.Where(x => x.RecordType == recordType);

            // ترتيب البيانات
            var activities = await query
                .OrderBy(e => e.DepartmentId)
                .ThenBy(e => e.EmployeeName)
                .ThenBy(e => e.StartDate)
                .ToListAsync();

            // إرسال البيانات للـ View
            return View(activities);
        }

        public async Task<IActionResult> IndexDepartment(int? employeeId, string recordType)
        {
            // قراءة الكوكيز
            int? userId = Request.Cookies.ContainsKey("UserId") ? int.Parse(Request.Cookies["UserId"]) : null;
            int? userType = Request.Cookies.ContainsKey("UserType") ? int.Parse(Request.Cookies["UserType"]) : null;
            int? branchId = Request.Cookies.ContainsKey("BranchID") ? int.Parse(Request.Cookies["BranchID"]) : null;
            int? departmentId = Request.Cookies.ContainsKey("DepartmentID") ? int.Parse(Request.Cookies["DepartmentID"]) : null;

            var query = _context.VwEmployeeActivities.AsQueryable();

            // 🔹 تطبيق صلاحيات المستخدم
            switch (userType)
            {
                case 1: // موظف - يشوف بس الطلبات الخاصة به
                    if (userId.HasValue)
                        query = query.Where(x => x.EmployeeId == userId.Value);
                    break;

                case 2: // مدير إدارة - يشوف كل الموظفين في الإدارة والفرع
                    if (departmentId.HasValue)
                        query = query.Where(x => x.DepartmentId == departmentId.Value);

                    if (branchId.HasValue)
                        query = query.Where(x => x.BranchId == branchId.Value);
                    break;

                case 3: // رئيس قطاع - يشوف كل الموظفين
                        // لا نضيف أي فلاتر
                    break;
            }

            // 🔹 فلترة إضافية حسب مدخلات المستخدم
            if (employeeId.HasValue)
                query = query.Where(x => x.EmployeeId == employeeId.Value);

            if (!string.IsNullOrEmpty(recordType))
                query = query.Where(x => x.RecordType == recordType);

            // ترتيب البيانات
            var activities = await query
                .OrderBy(e => e.DepartmentId)
                .ThenBy(e => e.EmployeeName)
                .ThenBy(e => e.StartDate)
                .ToListAsync();

            // حماية من null
            activities = activities ?? new List<VwEmployeeActivity>();

            // 🔹 بناء ViewModel
            var summary = activities
                .GroupBy(a => a.RecordType ?? "غير محدد")
                .Select(g => new RecordSummaryVM { RecordType = g.Key, Count = g.Count() })
                .ToList();

            var byDepartment = activities
                .GroupBy(a => a.DepartmentName ?? "غير محدد")
                .Select(g => new DepartmentRecordsVM { DepartmentName = g.Key, Records = g.ToList() })
                .ToList();

            var model = new EmployeeActivityReportVM
            {
                Activities = activities,
                Summary = summary,
                ByDepartment = byDepartment
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> IndexFilter(int? departmentId, int? branchId, int? employeeId, string recordType, DateTime? fromDate, DateTime? toDate)
        {
            // قراءة الكوكيز
            int? userId = Request.Cookies.ContainsKey("UserId") ? int.Parse(Request.Cookies["UserId"]) : null;
            int? userType = Request.Cookies.ContainsKey("UserType") ? int.Parse(Request.Cookies["UserType"]) : null;
            int? branchCookieId = Request.Cookies.ContainsKey("BranchID") ? int.Parse(Request.Cookies["BranchID"]) : null;
            int? departmentCookieId = Request.Cookies.ContainsKey("DepartmentID") ? int.Parse(Request.Cookies["DepartmentID"]) : null;

            // 🔹 جلب القوائم للـ Dropdowns حسب الصلاحيات
            switch (userType)
            {
                case 1: // موظف
                    ViewBag.Departments = await _context.HrDepartments
                        .Where(d => false) // الموظف العادي ممكن لا يحتاج اختيار الإدارة
                        .Select(d => new { d.Id, d.NameAr }).ToListAsync();

                    ViewBag.Branches = await _context.HrBranches
                        .Where(b => false) // الموظف العادي ممكن لا يحتاج اختيار الفرع
                        .Select(b => new { b.Id, b.NameAr }).ToListAsync();

                    ViewBag.Employees = await _context.HrEmployees
                        .Where(e => e.Id == userId.Value)
                        .Select(e => new { e.Id, e.NameAr }).ToListAsync();
                    break;

                case 2: // مدير إدارة
                    ViewBag.Departments = await _context.HrDepartments
                        .Where(d => d.Id == departmentCookieId.Value)
                        .Select(d => new { d.Id, d.NameAr }).ToListAsync();

                    ViewBag.Branches = await _context.HrBranches
                        .Where(b => b.Id == branchCookieId.Value)
                        .Select(b => new { b.Id, b.NameAr }).ToListAsync();

                    ViewBag.Employees = await _context.HrEmployees
                        .Where(e => e.DepartmentId == departmentCookieId.Value && e.BranchId == branchCookieId.Value)
                        .Select(e => new { e.Id, e.NameAr }).ToListAsync();
                    break;

                case 3: // رئيس قطاع
                    ViewBag.Departments = await _context.HrDepartments
                        .Select(d => new { d.Id, d.NameAr }).ToListAsync();

                    ViewBag.Branches = await _context.HrBranches
                        .Select(b => new { b.Id, b.NameAr }).ToListAsync();

                    ViewBag.Employees = await _context.HrEmployees
                        .Select(e => new { e.Id, e.NameAr }).ToListAsync();
                    break;
            }
            ViewBag.RecordTypes = new List<string> { "Mission", "Permission", "Leave" };

            // جلب البيانات من Stored Procedure
            var activities = await _context.VwEmployeeActivities
                .FromSqlRaw(
                    "EXEC dbo.sp_GetEmployeeActivities @DepartmentID={0}, @BranchID={1}, @EmployeeID={2}, @RecordType={3}, @FromDate={4}, @ToDate={5}",
                    departmentId, branchId, employeeId, recordType, fromDate, toDate
                )
                .ToListAsync();

            activities = activities ?? new List<VwEmployeeActivity>();

            

            // بناء ViewModel
            var summary = activities
                .GroupBy(a => a.RecordType ?? "غير محدد")
                .Select(g => new RecordSummaryVM { RecordType = g.Key, Count = g.Count() })
                .ToList();

            var byDepartment = activities
                .GroupBy(a => a.DepartmentName ?? "غير محدد")
                .Select(g => new DepartmentRecordsVM { DepartmentName = g.Key, Records = g.ToList() })
                .ToList();

            var model = new EmployeeActivityReportVM
            {
                Activities = activities,
                Summary = summary,
                ByDepartment = byDepartment
            };

            return View(model);
        }

        //public async Task<IActionResult> IndexFilter(int? departmentId,int? branchId,int? employeeId,string recordType,DateTime? fromDate,DateTime? toDate)
        //{
        //    // جلب القوائم للـ Dropdowns
        //    ViewBag.Departments = await _context.HrDepartments.Select(d => new { d.Id, d.NameAr }).ToListAsync();

        //    ViewBag.Branches = await _context.HrBranches.Select(b => new { b.Id, b.NameAr }).ToListAsync();

        //    ViewBag.Employees = await _context.HrEmployees.Select(e => new { e.Id, e.NameAr }).ToListAsync();

        //    ViewBag.RecordTypes = new List<string> { "Mission", "Permission", "Leave" };

        //    // جلب البيانات من Stored Procedure
        //    var activities = await _context.VwEmployeeActivities
        //        .FromSqlRaw(
        //            "EXEC dbo.sp_GetEmployeeActivities @DepartmentID={0}, @BranchID={1}, @EmployeeID={2}, @RecordType={3}, @FromDate={4}, @ToDate={5}",
        //            departmentId, branchId, employeeId, recordType, fromDate, toDate
        //        )
        //        .ToListAsync();

        //    activities = activities ?? new List<VwEmployeeActivity>();

        //    // بناء ViewModel
        //    var summary = activities
        //        .GroupBy(a => a.RecordType ?? "غير محدد")
        //        .Select(g => new RecordSummaryVM { RecordType = g.Key, Count = g.Count() })
        //        .ToList();

        //    var byDepartment = activities
        //        .GroupBy(a => a.DepartmentName ?? "غير محدد")
        //        .Select(g => new DepartmentRecordsVM { DepartmentName = g.Key, Records = g.ToList() })
        //        .ToList();

        //    var model = new EmployeeActivityReportVM
        //    {
        //        Activities = activities,
        //        Summary = summary,
        //        ByDepartment = byDepartment
        //    };

        //    return View(model);
        //}


    }
}
