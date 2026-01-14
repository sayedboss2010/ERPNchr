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
        public ActionResult Index(string search)
        {
            int? userId = Request.Cookies.ContainsKey("UserId") ? int.Parse(Request.Cookies["UserId"]) : null;
            int? userType = Request.Cookies.ContainsKey("UserType") ? int.Parse(Request.Cookies["UserType"]) : null;
            int? branchId = Request.Cookies.ContainsKey("BranchID") ? int.Parse(Request.Cookies["BranchID"]) : null;
            int? departmentId = Request.Cookies.ContainsKey("DepartmentID") ? int.Parse(Request.Cookies["DepartmentID"]) : null;

            var query = from l in _context.HrEmployeeOfficialMissions
                        join e in _context.HrEmployees on l.EmployeeId equals e.Id
                        where l.IsActive == true
                        select new { l, e };

            // 🔍 البحث
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    (x.e.NameAr != null && x.e.NameAr.Contains(search)) ||
                    (x.e.Department.NameAr != null && x.e.Department.NameAr.Contains(search)) ||
                    (x.l.AuthorityOfMission != null && x.l.AuthorityOfMission.Contains(search)) ||
                    (x.l.PurposeOfMission != null && x.l.PurposeOfMission.Contains(search))
                );
            }

            switch (userType)
            {
                case 1:
                    query = query.Where(x => x.e.Id == userId);
                    break;

                case 2:
                    query = query.Where(x =>
                        x.e.DepartmentId == departmentId &&
                        x.e.BranchId == branchId
                    );
                    break;

                case 3:
                    break;
            }

            var data = query
                .OrderByDescending(x => x.l.Id)
                .Select(x => new EmployeeMissionsVM
                {
                    Id = (int)x.l.Id,
                    EmployeeId = (int)x.e.Id,
                    EmployeeUserType = x.e.EmployeeTypeId.Value,
                    EmplyeeName = x.e.NameAr,
                    DepartmentName = x.e.Department.NameAr,
                    PurposeOfMission = x.l.PurposeOfMission,
                    AuthorityOfMission = x.l.AuthorityOfMission,
                    StartDate = x.l.StartDate,
                    EndDate = x.l.EndDate,
                    DirectManagerApproval = x.l.DirectManagerApproval,
                    DepartmentManagerApproval = x.l.DepartmentManagerApproval
                })
                .ToList();

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
           
           
            return View();
        }



        [HttpPost]
        public IActionResult Create(EmployeeMissionsVM model, IFormFile AttachmentPath)
        {
            if (AttachmentPath == null || AttachmentPath.Length == 0)
            {
                ModelState.AddModelError("MissionAttachment", "⚠️ مرفق المأمورية مطلوب.");
                FillViewBags();
                return View(model);
            }

            // تحقق من وجود مأمورية تتداخل مع نفس الفترة
            bool exists = _context.HrEmployeeOfficialMissions
                 .Any(p => p.EmployeeId == model.EmployeeId
                           && p.IsActive
                           && p.StartDate <= model.EndDate
                           && p.EndDate >= model.StartDate);

            if (exists)
            {
                FillViewBags();
                ModelState.AddModelError("", "⚠️ هذا الموظف لديه مأمورية تتداخل مع نفس الفترة.");
                return View(model);
            }

            // إنشاء المعرف الجديد
            long HrEmployeeMission_ID = _context.Database
                .SqlQueryRaw<long>("SELECT NEXT VALUE FOR dbo.HR_EmployeeOfficialMission_SEQ")
                .AsEnumerable()
                .First();

            var entity = new HrEmployeeOfficialMission
            {
                Id = HrEmployeeMission_ID,
                EmployeeId = model.EmployeeId,
                PurposeOfMission = model.PurposeOfMission,
                AuthorityOfMission = model.AuthorityOfMission,
                StartDate = model.StartDate,
                
                EndDate = model.EndDate,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                CreatedUserId = 1,
                IsActive = true,
            };

            // حفظ الملف على السيرفر
            // إنشاء اسم الملف
            var fileName = $"{HrEmployeeMission_ID}_{Path.GetFileName(AttachmentPath.FileName)}";

            // تحديد مسار المجلد
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MissionAttachments");

            // التأكد من وجود المجلد، إذا لم يكن موجودًا إنشئه
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // المسار الكامل للملف
            var path = Path.Combine(folderPath, fileName);

            // حفظ الملف
            using (var stream = new FileStream(path, FileMode.Create))
            {
                AttachmentPath.CopyTo(stream);
            }

            // حفظ المسار في الكيان (Entity) ليتم تخزينه في قاعدة البيانات
            entity.AttachmentPath = $"/MissionAttachments/{fileName}";

            entity.AttachmentPath = $"/MissionAttachments/{fileName}";

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
                            AttachmentPath = p.AttachmentPath
                        }).FirstOrDefault();

            return View(data);   // يفتح صفحة الطباعة
        }
        [HttpPost]
        public IActionResult DirectManagerAction(int id, bool isApproved, string type)
        {
            var mission = _context.HrEmployeeOfficialMissions.FirstOrDefault(x => x.Id == id);
            if (mission == null)
                return Json(new { success = false, message = "لم يتم العثور على المامورية" });

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            // منع الموافقة او الرفض لو اليوم > تاريخ الاجازة
            if (today > mission.StartDate)
            {
                return Json(new
                {
                    success = false,
                    message = "لا يمكن الموافقة أو الرفض بعد موعد بداية المامورية."
                });
            }

            // في حالة مسموح
            if (type == "direct")
            {
                mission.DirectManagerApproval = isApproved;

            }
            else if (type == "sector")
            {

                mission.DepartmentManagerApproval = isApproved;
            }
              ;
            mission.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

            _context.SaveChanges();

            return Json(new { success = true, message = "تم تحديث حالة المأمورية بنجاح" });
        }

    }
}
