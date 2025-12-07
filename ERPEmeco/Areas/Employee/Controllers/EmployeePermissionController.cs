using EF.Data;
using EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VM.ViewModels.Employee;

namespace ERPNchr.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class EmployeePermissionController : Controller
    {
      
        private readonly AppDbContext _context = new AppDbContext();

        // 🧾 عرض كل الإجازات
        public ActionResult Index()
        
        {
            int? userId = Request.Cookies.ContainsKey("UserId") ? int.Parse(Request.Cookies["UserId"]) : null;

            int? userType = Request.Cookies.ContainsKey("UserType") ? int.Parse(Request.Cookies["UserType"]) : null;

            int? branchId = Request.Cookies.ContainsKey("BranchID") ? int.Parse(Request.Cookies["BranchID"]) : null;

            int? departmentId = Request.Cookies.ContainsKey("DepartmentID") ? int.Parse(Request.Cookies["DepartmentID"]) : null;

            var query = from l in _context.HrEmployeePermissions
                        join t in _context.PermissionsTypes on l.PermissionTypeId equals t.Id
                        join e in _context.HrEmployees on l.EmployeeId equals e.Id
                        //join t in _context.PermissionsTypes on l. equals t.Id
                        where l.IsActive == true
                        select new { l,t, e };

            switch (userType)
            {
                case 1: // موظف - يشوف بس الطلبات الخاصة به
                    query = query.Where(x => x.e.Id == userId);
                    break;

                case 2: // مدير إدارة - يشوف كل الموظفين في الإدارة
                    query = query.Where(x =>
                        x.e.DepartmentId == departmentId &&
                        x.e.BranchId == branchId
                    );
                    break;

                case 3: // رئيس قطاع - يشوف كل الموظفين
                        // لا نضيف أي فلاتر
                    break;
            }

            // ----- Final Select -----
            var data = query
                .OrderByDescending(x => x.l.Id)

                .Select(x => new EmployeePermissionVM
                {
                    Id = (int)x.l.Id,
                    EmployeeId = (int)x.e.Id,
                    EmplyeeName = x.e.NameAr,

                    //DepartmentId = d.Id,
                   // DepartmentName = x.e.Department.NameAr,        // ← يظهر في الفيو

                    PermissionTypeId = x.l.PermissionTypeId,
                    PermissionTypeName = x.l.PermissionType.NameAr,
                    DateOfPermission = x.l.DateOfPermission,
                   

                    DirectManagerApproval = x.l.DirectManagerApproval,
                    DepartmentManagerApproval = x.l.DepartmentManagerApproval
                }).ToList();


            return View(data);
        }


        // ➕ شاشة إضافة جديدة
        [HttpGet]

        public ActionResult Create()
        {
            int? userId = Request.Cookies.ContainsKey("UserId") ? int.Parse(Request.Cookies["UserId"]) : null;

            int? userType = Request.Cookies.ContainsKey("UserType") ? int.Parse(Request.Cookies["UserType"]) : null;

            int? branchId = Request.Cookies.ContainsKey("BranchID") ? int.Parse(Request.Cookies["BranchID"]) : null;

            int? departmentId = Request.Cookies.ContainsKey("DepartmentID") ? int.Parse(Request.Cookies["DepartmentID"]) : null;

            // ----- Base Query -----
            var employeesQuery = _context.HrEmployees.Where(e => e.IsActive);

            switch (userType)
            {
                case 1: // موظف
                    employeesQuery = employeesQuery.Where(e => e.Id == userId);
                    break;

                case 2: // مدير إدارة
                    employeesQuery = employeesQuery.Where(e => e.DepartmentId == departmentId && e.BranchId == branchId);
                    break;

                case 3: // رئيس قطاع
                        // يشوف الكل → لا نضيف أي فلاتر
                    break;
            }

            // ----- Build List -----
            var employeeOptions = employeesQuery
                .Select(e => new
                {
                    e.Id,
                    Display = e.NameAr + " (" + e.EmpCode + ")"
                })
                .ToList();

            ViewBag.EmployeeOptions = new SelectList(employeeOptions, "Id", "Display");
            ViewBag.PermissionType = new SelectList(_context.PermissionsTypes, "Id", "NameAr");

            return View();
        }


        [HttpPost]
        public IActionResult Create(EmployeePermissionVM model)
        {
            if (model.DateOfPermission.HasValue)
            {
                int permissionMonth = model.DateOfPermission.Value.Month;
                int permissionYear = model.DateOfPermission.Value.Year;

                bool exists = _context.HrEmployeePermissions
                                      .Any(p => p.EmployeeId == model.EmployeeId
                                             && p.PermissionTypeId == model.PermissionTypeId
                                             && p.DateOfPermission.HasValue
                                             && p.DateOfPermission.Value.Month == permissionMonth
                                             && p.DateOfPermission.Value.Year == permissionYear
                                             && p.IsActive);

                if (exists)
                {
                    var Emplist = (from e in _context.HrEmployees
                                   where e.IsActive == true
                                   select new
                                   {
                                       e.Id,
                                       e.NameAr,
                                       Display = e.NameAr + " (" + e.EmpCode + ")"
                                   }).ToList();

                    ViewBag.EmployeeOptions = new SelectList(Emplist, "Id", "Display");
                    ViewBag.PermissionType = new SelectList(_context.PermissionsTypes, "Id", "NameAr");

                    ModelState.AddModelError("", "⚠️ هذا الموظف لديه إذن من نفس النوع في نفس الشهر.");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "❌ يجب إدخال تاريخ الإذن.");
                return View(model);
            }


            // إنشاء المعرف الجديد
            long HrEmployeePermission_ID = _context.Database
                .SqlQueryRaw<long>("SELECT NEXT VALUE FOR dbo.HR_EmployeePermissions_SEQ")
                .AsEnumerable()
                .First();

            var entity = new HrEmployeePermission
            {
                Id = HrEmployeePermission_ID,
                EmployeeId = model.EmployeeId,
                PermissionTypeId = model.PermissionTypeId,
                DateOfPermission = model.DateOfPermission,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                CreatedUserId = 1, // TODO: استبدلها بالمستخدم الحالي
                IsActive = true,
            };

            _context.HrEmployeePermissions.Add(entity);
            _context.SaveChanges();

            return RedirectToAction("PrintPermissionNew", "EmployeePermission", new { area = "Employee", id = entity.Id });
        }


        public ActionResult PrintPermissionNew(long id)
        {
            var data = (from p in _context.HrEmployeePermissions
                        join e in _context.HrEmployees on p.EmployeeId equals e.Id
                        join dl in _context.HrDepartments on e.DepartmentId equals dl.Id into dept
                        from dl in dept.DefaultIfEmpty() // left join
                        join t in _context.PermissionsTypes on p.PermissionTypeId equals t.Id
                        where p.Id == id
                        select new EmployeePermissionVM
                        {
                            EmplyeeName = e.NameAr,
                            PermissionTypeName = t.NameAr,
                            DateOfPermission = p.DateOfPermission,
                            DepartmentName = dl.NameAr,

                        }).FirstOrDefault();

            return View(data);   // يفتح صفحة الطباعة
        }
        [HttpPost]
        public IActionResult DirectManagerAction(int id, bool isApproved, string type)
        {
            var permission = _context.HrEmployeePermissions.FirstOrDefault(x => x.Id == id);
            if (permission == null)
                return Json(new { success = false, message = "لم يتم العثور على الاذن" });

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            // منع الموافقة او الرفض لو اليوم > تاريخ الاجازة
            if (today > permission.DateOfPermission)
            {
                return Json(new
                {
                    success = false,
                    message = "لا يمكن الموافقة أو الرفض بعد موعد الاذن."
                });
            }

            // في حالة مسموح
            if (type == "direct")
            {
                permission.DirectManagerApproval = isApproved;

            }
            else if (type == "sector")
            {

                permission.DepartmentManagerApproval = isApproved;
            }
        
            permission.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

            _context.SaveChanges();

            return Json(new { success = true, message = "تم تحديث حالة الاذن بنجاح" });
        }



    }

}
