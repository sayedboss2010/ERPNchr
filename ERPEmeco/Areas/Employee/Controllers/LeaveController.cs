using EF.Data;
using EF.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VM.ViewModels;

namespace YourProjectName.Areas.Employee.Controllers
{
    [Area("Employee")]

    public class LeaveController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context = new AppDbContext();
        public LeaveController(IWebHostEnvironment env)
        {
            _env = env;
        }
        // 🧾 عرض كل الإجازات
        public ActionResult Index()
        {

            int? userId = Request.Cookies.ContainsKey("UserId") ? int.Parse(Request.Cookies["UserId"]) : null;

            int? userType = Request.Cookies.ContainsKey("UserType") ? int.Parse(Request.Cookies["UserType"]) : null;

            int? branchId = Request.Cookies.ContainsKey("BranchID") ? int.Parse(Request.Cookies["BranchID"]) : null;

            int? departmentId = Request.Cookies.ContainsKey("DepartmentID") ? int.Parse(Request.Cookies["DepartmentID"]) : null;

            var query = from l in _context.HrEmployeeLeaves
                        join e in _context.HrEmployees on l.EmployeeId equals e.Id
                        join t in _context.HrLeaveTypes on l.LeaveTypeId equals t.Id
                        where l.IsActive == true
                        select new { l, e, t };

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
                .Select(x => new EmployeeLeaveVM
                {
                    Id = x.l.Id,
                    EmployeeId = x.e.Id,
                    EmployeeName = x.e.NameAr,
                    LeaveTypeId = x.t.Id,
                    LeaveTypeName = x.t.NameAr,
                    StartDate = x.l.StartDate,
                    EndDate = x.l.EndDate,
                    LeaveDays = x.l.LeaveDays,
                    Reason = x.l.Reason,
                    DirectManagerApproval = x.l.DirectManagerApproval,
                    DepartmentManagerApproval = x.l.DepartmentManagerApproval
                })
                .ToList();
            //var data = (from l in _context.HrEmployeeLeaves
            //            join e in _context.HrEmployees on l.EmployeeId equals e.Id
            //            join t in _context.HrLeaveTypes on l.LeaveTypeId equals t.Id
            //            where l.IsActive == true
            //            orderby l.Id descending
            //            select new EmployeeLeaveVM
            //            {
            //                Id = l.Id,
            //                EmployeeId = e.Id,
            //                EmployeeName = e.NameAr,
            //                LeaveTypeId = t.Id,
            //                LeaveTypeName = t.NameAr,
            //                StartDate = l.StartDate,
            //                EndDate = l.EndDate,
            //                LeaveDays = l.LeaveDays,
            //                Reason = l.Reason,
            //                DirectManagerApproval = l.DirectManagerApproval,
            //                DepartmentManagerApproval = l.DepartmentManagerApproval,

            //            }).ToList();



            return View(data);
        }

        // ➕ شاشة إضافة جديدة
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    int? UserId = null;
        //    int? UserType = null;
        //    int? BranchID = null;
        //    int? DepartmentID = null;
        //    if (Request.Cookies.ContainsKey("UserId"))
        //    {
        //        UserId = int.Parse(Request.Cookies["UserId"]);
        //    }
        //    if (Request.Cookies.ContainsKey("UserType"))
        //    {
        //        UserType = int.Parse(Request.Cookies["UserType"]);
        //    }
        //    if (Request.Cookies.ContainsKey("BranchID"))
        //    {
        //        BranchID = int.Parse(Request.Cookies["BranchID"]);
        //    }
        //    if (Request.Cookies.ContainsKey("DepartmentID"))
        //    {
        //        DepartmentID = int.Parse(Request.Cookies["DepartmentID"]);
        //    }
        //    if (UserType == 1)// موظف
        //    {
        //        var Emplist = (from e in _context.HrEmployees
        //                       where e.IsActive == true
        //                       && e.Id == UserId
        //                       select new
        //                       {
        //                           e.Id,
        //                           e.NameAr,
        //                           Display = e.NameAr + " (" + e.EmpCode + ")"  // نضيف النص المعروض بالاسم + الكود
        //                       }).ToList();

        //        ViewBag.EmployeeOptions = new SelectList(Emplist, "Id", "Display");
        //    }
        //    else if (UserType == 2)// مدير ادارة
        //    {
        //        // هيشوف كل موظفين الادارة
        //        var Emplist = (from e in _context.HrEmployees
        //                       where e.IsActive == true
        //                       && e.DepartmentId == DepartmentID
        //                       && e.BranchId == BranchID
        //                       select new
        //                       {
        //                           e.Id,
        //                           e.NameAr,
        //                           Display = e.NameAr + " (" + e.EmpCode + ")"  // نضيف النص المعروض بالاسم + الكود
        //                       }).ToList();

        //        ViewBag.EmployeeOptions = new SelectList(Emplist, "Id", "Display");
        //    }
        //    else if (UserType == 3)// رئيس قطاع
        //    {
        //        // يشوف كل الموظفين
        //        var Emplist = (from e in _context.HrEmployees
        //                       where e.IsActive == true
        //                      select new
        //                       {
        //                           e.Id,
        //                           e.NameAr,
        //                           Display = e.NameAr + " (" + e.EmpCode + ")"  // نضيف النص المعروض بالاسم + الكود
        //                       }).ToList();

        //        ViewBag.EmployeeOptions = new SelectList(Emplist, "Id", "Display");
        //    }


        //    ViewBag.LeaveTypeId = new SelectList(_context.HrLeaveTypes.Where(a => a.IsActive == true), "Id", "NameAr");



        //    return View();

        //}

        [HttpGet]
        public ActionResult Create()
        {
            int? userId = Request.Cookies.ContainsKey("UserId")? int.Parse(Request.Cookies["UserId"]): null;

            int? userType = Request.Cookies.ContainsKey("UserType")? int.Parse(Request.Cookies["UserType"]): null;

            int? branchId = Request.Cookies.ContainsKey("BranchID")? int.Parse(Request.Cookies["BranchID"]): null;

            int? departmentId = Request.Cookies.ContainsKey("DepartmentID")? int.Parse(Request.Cookies["DepartmentID"]): null;

            // ----- Base Query -----
            var employeesQuery = _context.HrEmployees.Where(e => e.IsActive);

            switch (userType)
            {
                case 1: // موظف
                    employeesQuery = employeesQuery.Where(e => e.Id == userId);
                    break;

                case 2: // مدير إدارة
                    employeesQuery = employeesQuery.Where(e => e.DepartmentId == departmentId&& e.BranchId == branchId);
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

            // ----- Leave Types -----
            ViewBag.LeaveTypeId = new SelectList(_context.HrLeaveTypes.Where(a => a.IsActive),"Id", "NameAr");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeLeaveVM model, IFormFile AttachmentFile)
        {
            //if (!ModelState.IsValid)
            //{
            //    var detailedErrors = ModelState
            //        .Where(ms => ms.Value.Errors.Count > 0)
            //        .Select(ms => new
            //        {
            //            Field = ms.Key,                                      // اسم الحقل
            //            Errors = ms.Value.Errors.Select(e => e.ErrorMessage), // رسالة الخطأ
            //            AttemptedValue = ms.Value.AttemptedValue              // القيمة اللي دخلت وعملت مشكلة
            //        })
            //        .ToList();

            //    foreach (var error in detailedErrors)
            //    {
            //        Console.WriteLine($"Field: {error.Field}");
            //        Console.WriteLine($"Attempted Value: {error.AttemptedValue}");
            //        Console.WriteLine($"Errors: {string.Join(", ", error.Errors)}");
            //        Console.WriteLine("----------------------------");
            //    }

            //    // إعادة تحميل القوائم
            //    var Emplist = (from e in _context.HrEmployees
            //                   where e.IsActive == true
            //                   select new
            //                   {
            //                       e.Id,
            //                       e.NameAr,
            //                       Display = e.NameAr + " (" + e.EmpCode + ")"
            //                   }).ToList();

            //    ViewBag.EmployeeOptions = new SelectList(Emplist, "Id", "Display");
            //    ViewBag.LeaveTypeId = new SelectList(_context.HrLeaveTypes.Where(a => a.IsActive == true), "Id", "NameAr", model.LeaveTypeId);

            //    return View(model);
            //}
            string attachmentPath = null;

            if (AttachmentFile != null && AttachmentFile.Length > 0)
            {
                var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string uploadFolder = Path.Combine(webRoot, "uploads", "MedicalLeaves");

                Directory.CreateDirectory(uploadFolder);


                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                // اسم ملف فريد
                string fileName = $"{Guid.NewGuid()}_{AttachmentFile.FileName}";
                string filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    AttachmentFile.CopyTo(stream);
                }

                // المسار الذي يُخزن في الداتابيز
                attachmentPath = $"/uploads/MedicalLeaves/{fileName}";
            }

            long HrEmployeeLeaves_ID = _context.Database
                .SqlQueryRaw<long>("SELECT NEXT VALUE FOR dbo.HR_Employee_Leaves_SEQ")
                .AsEnumerable()
                .First();

            var entity = new HrEmployeeLeaf
            {
                Id = HrEmployeeLeaves_ID,
                EmployeeId = model.EmployeeId,
                LeaveTypeId = model.LeaveTypeId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Reason = model.Reason,
                HrEmployeeLeaveBalanceId = model.LeaveBalanceID,
                LeaveDays = model.ActualDays,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                CreatedUserId = 1, // TODO: استبدلها بالمستخدم الحالي
                IsActive = true,
                AttachmentPath = attachmentPath  // ← هنا الحفظ
            };

            _context.HrEmployeeLeaves.Add(entity);
            // HR_Employee_LeaveBalance اجمالى الاجازة للموظف
            var leaveBalance = _context.HrEmployeeLeaveBalances.FirstOrDefault(e => e.Id == model.LeaveBalanceID);

            if (leaveBalance != null)
            {
                // زيادة مجموع المستخدم من كل الأنواع
                leaveBalance.UsedDays += (int)model.ActualDays;

                // =============================
                // 1) إجازة عارضة (ID = 1)
                // =============================
                if (model.LeaveTypeId == 1)
                {
                    leaveBalance.CasualUsedDays = (int)model.ActualDays;
                    leaveBalance.CasualRemainingDays =
                        (int)(leaveBalance.CasualTotalDays - leaveBalance.CasualUsedDays);
                }

                // =============================
                // 2) إجازة اعتيادي (ID = 2)
                // =============================
                if (model.LeaveTypeId == 2)
                {
                    leaveBalance.UsedDays = (int)model.ActualDays;
                    leaveBalance.TotalDaysReminig =
                        (int)(leaveBalance.TotalDays - leaveBalance.UsedDays);
                }

                //// =============================
                //// 3) إجازة سنوي (ID = 5)
                //// = نفس حساب الاعتيادي
                //// =============================
                if (model.LeaveTypeId == 5)
                {
                    leaveBalance.AnnualUsedDays = (int)model.ActualDays;
                    leaveBalance.AnnualRemainingDays =
                        (int)(leaveBalance.AnnualTotalDays - leaveBalance.AnnualUsedDays);
                }

                //// =============================
                //// تحديث الإجمالي العام
                //// =============================
                //leaveBalance.TotalDaysReminig =
                //    (int)(leaveBalance.TotalDays - leaveBalance.UsedDays);

                leaveBalance.UpdatedDate = DateTime.Now;
                leaveBalance.UpdatedUserId = 1;


            }
            _context.SaveChanges();

            //  return RedirectToAction("index", "Leave", new { area = "Employee" });
            return RedirectToAction("PrintNew", "Leave", new { area = "Employee", id = entity.Id });
        }

        // 🖨️ عرض نموذج الطباعة
        public ActionResult Print(long id)
        {
            var data = (from l in _context.HrEmployeeLeaves
                        join e in _context.HrEmployees on l.EmployeeId equals e.Id
                        join t in _context.HrLeaveTypes on l.LeaveTypeId equals t.Id
                        where l.Id == id
                        select new EmployeeLeaveVM
                        {
                            Id = l.Id,
                            EmployeeId = e.Id,
                            EmployeeName = e.NameAr,
                            LeaveTypeId = t.Id,
                            LeaveTypeName = t.NameAr,
                            StartDate = l.StartDate,
                            EndDate = l.EndDate,
                            Reason = l.Reason
                        }).FirstOrDefault();

            if (data == null)
                return Content("لم يتم العثور على الإجازة المطلوبة");

            return View(data);
        }
        public ActionResult PrintNew(long id)
        {
            // جلب بيانات الإجازة والموظف ونوع الإجازة
            var leave = (from l in _context.HrEmployeeLeaves
                         join e in _context.HrEmployees on l.EmployeeId equals e.Id
                         join t in _context.HrLeaveTypes on l.LeaveTypeId equals t.Id
                         join d in _context.HrDepartments on e.DepartmentId equals d.Id into dept
                         from d in dept.DefaultIfEmpty() // left join
                         where l.Id == id
                         select new
                         {
                             Leave = l,
                             Employee = e,
                             Department = d,
                             LeaveType = t
                         }).FirstOrDefault();


            if (leave == null)
                return Content("❌ لم يتم العثور على الإجازة");

            // جلب آخر رصيد مسجل للموظف للسنة المطلوبة (إن وجد)
            int year = leave.Leave.StartDate?.Year ?? DateTime.Now.Year;
            var lastBalance = _context.HrEmployeeLeaveBalances
                  .Where(b => b.EmployeeId == leave.Employee.Id && b.Year == year)
                  .OrderByDescending(b => b.Id)
                  .FirstOrDefault();

            DateTime startDate = leave.Leave.StartDate?.ToDateTime(TimeOnly.MinValue).Date ?? DateTime.Now.Date;
            DateTime endDate = leave.Leave.EndDate?.ToDateTime(TimeOnly.MinValue).Date ?? DateTime.Now.Date;

            int totalDays = (endDate - startDate).Days + 1;
            totalDays = Math.Max(1, totalDays);

            int actualDays = Enumerable.Range(0, totalDays)
                                       .Select(i => startDate.AddDays(i))
                                       .Count(d => d.DayOfWeek != DayOfWeek.Friday);

            var data = new EmployeeLeaveVM
            {
                Id = leave.Leave.Id,
                EmployeeName = leave.Employee.NameAr,
                DepartmentID = leave.Employee.DepartmentId,
                EmployeeCode = leave.Employee.EmpCode,
                DepartmentName = string.IsNullOrWhiteSpace(leave.Department?.NameAr) ? "-" : leave.Department.NameAr,

                LeaveTypeId = leave.LeaveType.Id,
                LeaveTypeName = leave.LeaveType.NameAr,
                StartDate = leave.Leave.StartDate,
                EndDate = leave.Leave.EndDate,
                Reason = leave.Leave.Reason,
                AttachmentPath = leave.Leave.AttachmentPath,
                ActualDays = actualDays,

                TotalDays = lastBalance?.TotalDays ?? 0,
                UsedDays = lastBalance?.UsedDays ?? 0,
                RemainingBefore = lastBalance?.TotalDaysReminig ?? 0
            };

            if (data.LeaveTypeId == 1 || data.LeaveTypeId == 2)
            { int remainingBefore = data.RemainingBefore.Value; data.RemainingAfter = (byte)Math.Max(remainingBefore - actualDays, 0); }

            else
            {
                data.RemainingAfter = 0;
            }

            return View("PrintNew", data);
        }





        //public ActionResult PrintNew(long id)
        //{
        //    ViewBag.Date = DateTime.Now;
        //    var data = (from l in _context.HrEmployeeLeaves
        //                join e in _context.HrEmployees on l.EmployeeId equals e.Id
        //                join t in _context.HrLeaveTypes on l.LeaveTypeId equals t.Id
        //                join tb in _context.HrEmployeeLeaveBalances
        //                    on new { EmpId = e.Id, Year = l.StartDate.Value.Year }
        //                    equals new { EmpId = tb.EmployeeId, Year = tb.Year } into balanceJoin
        //                from tb in balanceJoin.DefaultIfEmpty()
        //                where l.Id == id
        //                select new EmployeeLeaveVM
        //                {
        //                    Id = l.Id,
        //                    EmployeeId = e.Id,
        //                    EmployeeCode = e.EmpCode,
        //                    EmployeeName = e.NameAr,
        //                    DepartmentName=e.Department.NameAr,
        //                    LeaveTypeId = t.Id,
        //                    LeaveTypeName = t.NameAr,
        //                    StartDate = l.StartDate,
        //                    EndDate = l.EndDate,
        //                    Reason = l.Reason,
        //                    TotalDays = tb.TotalDays,

        //                    // ✅ استخدم 0 لو مفيش سجل رصيد
        //                    RemainingBefore = (t.NameAr ?? "").Contains("عرض")
        //                        ? (tb != null ? (int)tb.CasualRemainingDays : 0)
        //                        : (tb != null ? (int)tb.TotalDaysReminig : 0),

        //                    UsedDays = (t.NameAr ?? "").Contains("عرض")
        //                        ? (tb != null ? (int)tb.CasualUsedDays : 0)
        //                        : (tb != null ? (int)tb.UsedDays : 0)
        //                }).FirstOrDefault();

        //    if (data == null)
        //        return Content("لم يتم العثور على الإجازة المطلوبة");

        //    if (data.StartDate == null || data.EndDate == null)
        //        return Content("بيانات التواريخ غير صحيحة.");

        //    //// أيام الاجازة المطلوبة
        //    ///
        //    DateTime startDate = DateTime.Parse( data.StartDate.Value.ToString());
        //    DateTime endDate = DateTime.Parse(data.EndDate.Value.ToString());
        //    // حساب الفرق بين التاريخين
        //    int RequiredVacationDays = (endDate - startDate).Days + 1;

        //    //int RequiredVacationDays = (data.EndDate.Value.Day - data.StartDate.Value.Day) + 1;
        //    ////  أيام الاجازة المطلوبة من غير جمع

        //    RequiredVacationDays = Enumerable.Range(0, RequiredVacationDays)
        //                    .Select(i => data.StartDate.Value.AddDays(i))
        //                    .Count(d => d.DayOfWeek != DayOfWeek.Friday);

        //    ViewBag.RequiredVacationDays = RequiredVacationDays;

        //    //// أول يومين بدون خصم
        //    //int deductedDays = actualDays <= 2 ? 0 : actualDays - 2;

        //    //// الرصيد بعد الخصم
        //    //int remainingAfter = data.RemainingBefore - actualDays;
        //    //if (remainingAfter < 0) remainingAfter = 0;

        //    //// تعيين القيم في الموديل
        //    ////data.TotalDays = totalDays;
        //    //data.ActualDays = actualDays;
        //    //data.DeductedDays = deductedDays;
        //    //data.RemainingAfter = remainingAfter;


        //    return View(data);
        //}
        [HttpPost]
        public JsonResult CheckLeaveDate(long employeeId, string startDate, string endDate)
        {
            DateOnly start = DateOnly.Parse(startDate);
            DateOnly end = DateOnly.Parse(endDate);

            var overlappingLeaves = _context.HrEmployeeLeaves
                .Where(l => l.EmployeeId == employeeId &&
                            l.IsActive &&
                            ((l.StartDate <= end) && (l.EndDate >= start)))
                .ToList();

            if (overlappingLeaves.Any())
            {
                string message = "هناك إجازة موجودة بالفعل:\n";

                foreach (var leave in overlappingLeaves)
                {
                    message += $"- من {leave.StartDate} إلى {leave.EndDate}\n";
                }

                return Json(new { hasConflict = true, message });
            }

            return Json(new { hasConflict = false });
        }


        [HttpGet]
        public JsonResult GetLeaveBalance(long employeeId, int leaveTypeId)
        {
            int year = DateTime.Now.Year;

            var balance = _context.HrEmployeeLeaveBalances
                .FirstOrDefault(b => b.EmployeeId == employeeId && b.Year == year);

            if (balance == null)
                return Json(null);

            var dto = new LeaveBalanceDto
            {
                Id = balance.Id,

                TotalDays = balance.TotalDays,
                UsedDays = balance.UsedDays,
                RemainingDays = balance.TotalDaysReminig,

                AnnualTotalDays = balance.AnnualTotalDays,
                AnnualUsedDays = balance.AnnualUsedDays,
                AnnualRemainingDays = balance.AnnualRemainingDays,

                CasualTotalDays = balance.CasualTotalDays,
                CasualUsedDays = balance.CasualUsedDays,
                CasualRemainingDays = balance.CasualRemainingDays
            };

            // ✨ تخصيص نوع الإجازة المختار
            switch (leaveTypeId)
            {
                case 1: // Casual
                    dto.TotalDays = balance.CasualTotalDays;
                    dto.UsedDays = balance.CasualUsedDays;
                    dto.RemainingDays = balance.CasualRemainingDays;
                    break;

                case 2: // Normal
                    dto.TotalDays = balance.TotalDays;
                    dto.UsedDays = balance.UsedDays;
                    dto.RemainingDays = balance.TotalDaysReminig;
                    break;

                case 5: // Annual
                    dto.TotalDays = balance.AnnualTotalDays;
                    dto.UsedDays = balance.AnnualUsedDays;
                    dto.RemainingDays = balance.AnnualRemainingDays;
                    break;
            }

            // ✨ Load Leaves (as DTOs—not Entities)
            dto.Leaves = _context.HrEmployeeLeaves
                .Where(l => l.HrEmployeeLeaveBalanceId == balance.Id &&
                            l.StartDate != null &&
                            l.EndDate != null)
                .Select(l => new LeaveItemDto
                {
                    StartDate = l.StartDate.Value,
                    EndDate = l.EndDate.Value,
                    LeaveDays = l.LeaveDays,
                    LeaveTypeId = l.LeaveTypeId
                })
                .ToList();

            // ✨ حساب الأيام لكل شهر
            if (dto.Leaves.Any())
            {
                var perMonth = dto.Leaves
                    .SelectMany(l =>
                    {
                        var list = new List<(int Month, int Days)>();
                        var start = l.StartDate;
                        var end = l.EndDate;

                        while (start <= end)
                        {
                            var endOfMonth = new DateOnly(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));
                            var currentEnd = end < endOfMonth ? end : endOfMonth;

                            int days = currentEnd.DayNumber - start.DayNumber + 1;
                            list.Add((start.Month, days));

                            start = currentEnd.AddDays(1);
                        }

                        return list;
                    })
                    .GroupBy(x => x.Month)
                    .Select(g => $"{g.Key}/{g.Sum(x => x.Days)}")
                    .ToList();

                dto.UsedDaysMonth = string.Join(" : ", perMonth);
            }

            return Json(dto);
        }

        public class LeaveBalanceDto
        {
            public long Id { get; set; }

            public decimal TotalDays { get; set; }
            public decimal UsedDays { get; set; }
            public decimal? RemainingDays { get; set; }

            public decimal AnnualTotalDays { get; set; }
            public decimal AnnualUsedDays { get; set; }
            public int? AnnualRemainingDays { get; set; }

            public decimal CasualTotalDays { get; set; }
            public decimal CasualUsedDays { get; set; }
            public int? CasualRemainingDays { get; set; }

            public int? TotalDaysReminig { get; set; }

            public string UsedDaysMonth { get; set; }

            public List<LeaveItemDto> Leaves { get; set; } = new();
        }

        public class LeaveItemDto
        {
            public DateOnly StartDate { get; set; }
            public DateOnly EndDate { get; set; }
            public int? LeaveDays { get; set; }
            public int? LeaveTypeId { get; set; }
        }


        //public class EmployeeLeaveBalanceDto
        //{
        //    public long Id { get; set; }
        //    public decimal TotalDays { get; set; }
        //    public decimal UsedDays { get; set; }
        //    public decimal? RemainingDays { get; set; }
        //    public List<HrEmployeeLeaf> Leaves { get; set; }
        //    public string UsedDaysMonth { get; set; }

        //    /// إجمالي الإجازات العارضة في السنة
        //    /// </summary>
        //    public decimal CasualTotalDays { get; set; }

        //    /// <summary>
        //    /// ايام العارضة المستخدمة
        //    /// </summary>
        //    public decimal CasualUsedDays { get; set; }
        //    public decimal AnnualTotalDays { get; set; }

        //    public decimal AnnualUsedDays { get; set; }
        //    public int? AnnualRemainingDays { get; set; }


        //    public int? CasualRemainingDays { get; set; }

        //    public int? TotalDaysReminig { get; set; }
        //}

    }
}
