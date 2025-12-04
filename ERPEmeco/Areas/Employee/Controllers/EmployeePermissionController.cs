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
            var data = (from l in _context.HrEmployeePermissions
                        join e in _context.HrEmployees on l.EmployeeId equals e.Id
                        join t in _context.PermissionsTypes on l.PermissionTypeId equals t.Id
                        where l.IsActive == true
                        orderby l.Id descending
                        select new EmployeePermissionVM
                        {
                            Id = l.Id,
                            EmployeeId = e.Id,
                            EmplyeeName = e.NameAr,
                            PermissionTypeName = t.NameAr,                           
                            DateOfPermission = l.DateOfPermission,                                              
                            DirectManagerApproval = l.DirectManagerApproval,
                            DepartmentManagerApproval = l.DepartmentManagerApproval,

                        }).ToList();

            return View(data);
        }

        // ➕ شاشة إضافة جديدة
        [HttpGet]

        public ActionResult Create()
        {
            var Emplist = (from e in _context.HrEmployees
                           where e.IsActive == true
                           //&& e.CurrentBranchDeptId == 5
                           select new
                           {
                               e.Id,
                               e.NameAr,
                               Display = e.NameAr + " (" + e.EmpCode + ")"  // نضيف النص المعروض بالاسم + الكود
                           }).ToList();
            // هنا نخزن النص المعروض في ViewBag
            ViewBag.EmployeeOptions = new SelectList(Emplist, "Id", "Display");
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



    }

}
