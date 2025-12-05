using EF.Data;
using EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VM.ViewModels.Employee;


namespace ERPNchr.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class EmployeeMissionController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        // 🧾 عرض كل الإجازات
        public ActionResult Index()
        {
            var data = (from l in _context.HrEmployeeOfficialMissions
                        join e in _context.HrEmployees
                            on l.EmployeeId equals e.Id
                        join d in _context.HrDepartments
                            on l.DepartmentId equals d.Id      // ← الربط الصحيح مباشرة
                        where l.IsActive == true
                        orderby l.Id descending
                        select new EmployeeMissionsVM
                        {
                            Id = (int)l.Id,
                            EmployeeId = (int) e.Id,
                            EmplyeeName = e.NameAr,

                            DepartmentId = d.Id,
                            DepartmentName = d.NameAr,        // ← يظهر في الفيو

                            PurposeOfMission = l.PurposeOfMission,
                            AuthorityOfMission = l.AuthorityOfMission,
                            StartDate = l.StartDate,
                            EndDate = l.EndDate,

                            DirectManagerApproval = l.DirectManagerApproval,
                            DepartmentManagerApproval = l.DepartmentManagerApproval
                        }).ToList();

            return View(data);
        }


        // ➕ شاشة إضافة جديدة
        private void FillViewBags()
        {
            var Emplist = _context.HrEmployees
                .Where(e => e.IsActive)
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.NameAr + " (" + e.EmpCode + ")"
                }).ToList();

            ViewBag.EmployeeOptions = Emplist;

            //ViewBag.ListHrDepartment = _context.HrDepartments
            //    .Select(d => new SelectListItem
            //    {
            //        Value = d.Id.ToString(),
            //        Text = d.NameAr
            //    }).ToList();
        }

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
           
            return View();
        }



        [HttpPost]
        public IActionResult Create(EmployeeMissionsVM model)
        {
            //if (!ModelState.IsValid)
            //{
            //    FillViewBags();   // مهم جداً
            //    return View(model);
            //}
            // تحقق من وجود مأمورية لنفس الموظف تتداخل مع الفترة الجديدة
            bool exists = _context.HrEmployeeOfficialMissions
                .Any(p => p.EmployeeId == model.EmployeeId
                          && p.IsActive
                          && p.StartDate <= model.EndDate      // بداية القديمة <= نهاية الجديدة
                          && p.EndDate >= model.StartDate);   // نهاية القديمة >= بداية الجديدة

            if (exists)
            {
                FillViewBags(); // إعادة تعبئة القوائم في ViewBag

                ModelState.AddModelError("", "⚠️ هذا الموظف لديه مأمورية تتداخل مع نفس الفترة.");
                return View(model); // إعادة عرض الصفحة مع رسالة الخطأ
            }


            // إنشاء المعرف الجديد
            int HrEmployeeMission_ID = _context.Database
                .SqlQueryRaw<int>("SELECT NEXT VALUE FOR dbo.HR_EmployeeOfficialMission_SEQ")
                .AsEnumerable()
                .First();

            var entity = new HrEmployeeOfficialMission
            {
                Id = HrEmployeeMission_ID,
                EmployeeId = model.EmployeeId,
                PurposeOfMission = model.PurposeOfMission,
                AuthorityOfMission = model.AuthorityOfMission,
                DepartmentId = model.DepartmentId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                CreatedUserId = 1, // TODO: استبدلها بالمستخدم الحالي
                IsActive = true,
            };

            _context.HrEmployeeOfficialMissions.Add(entity);
            _context.SaveChanges();

            return RedirectToAction("PrintMissionNew", "EmployeeMission", new { area = "Employee", id = entity.Id });
        }

        [HttpGet]
        public ActionResult PrintMissionNew(long id)
        {
            var data = (from p in _context.HrEmployeeOfficialMissions
                        join e in _context.HrEmployees on p.EmployeeId equals e.Id
                        join dl in _context.HrDepartments on e.DepartmentId equals dl.Id into dept
                         from dl in dept.DefaultIfEmpty() // left join
                        where p.Id == id
                        select new EmployeeMissionsVM
                        {    Id=(int)e.Id,
                            EmplyeeName = e.NameAr,
                            EmployeeId=e.EmployeeTypeId,
                            DepartmentId=e.DepartmentId,
                            DepartmentName = dl.NameAr,
                            AuthorityOfMission = p.AuthorityOfMission,
                            PurposeOfMission = p.PurposeOfMission,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                        }).FirstOrDefault();

            return View(data);   // يفتح صفحة الطباعة
        }
    }
}
