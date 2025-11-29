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
        private readonly AppDbContext _context = new AppDbContext();

        // 🧾 عرض كل الإجازات
        public ActionResult Index()
        {
            var data = (from l in _context.HrEmployeeLeaves
                        join e in _context.HrEmployees on l.EmployeeId equals e.Id
                        join t in _context.HrLeaveTypes on l.LeaveTypeId equals t.Id
                        where l.IsActive == true
                        orderby l.Id descending
                        select new EmployeeLeaveVM
                        {
                            Id = l.Id,
                            EmployeeId = e.Id,
                            EmployeeName = e.NameAr,
                            LeaveTypeId = t.Id,
                            LeaveTypeName = t.NameAr,
                            StartDate = l.StartDate,
                            EndDate = l.EndDate,
                            LeaveDays = l.LeaveDays,
                            Reason = l.Reason,
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
            ViewBag.LeaveTypeId = new SelectList(_context.HrLeaveTypes.Where(a => a.IsActive == true), "Id", "NameAr");

            return View();
        }

        // 💾 حفظ الإجازة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeLeaveVM model)
        {
            if (!ModelState.IsValid)
            {
                var detailedErrors = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .Select(ms => new
                    {
                        Field = ms.Key,                                      // اسم الحقل
                        Errors = ms.Value.Errors.Select(e => e.ErrorMessage), // رسالة الخطأ
                        AttemptedValue = ms.Value.AttemptedValue              // القيمة اللي دخلت وعملت مشكلة
                    })
                    .ToList();

                foreach (var error in detailedErrors)
                {
                    Console.WriteLine($"Field: {error.Field}");
                    Console.WriteLine($"Attempted Value: {error.AttemptedValue}");
                    Console.WriteLine($"Errors: {string.Join(", ", error.Errors)}");
                    Console.WriteLine("----------------------------");
                }

                // إعادة تحميل القوائم
                var Emplist = (from e in _context.HrEmployees
                               where e.IsActive == true
                               select new
                               {
                                   e.Id,
                                   e.NameAr,
                                   Display = e.NameAr + " (" + e.EmpCode + ")"
                               }).ToList();

                ViewBag.EmployeeOptions = new SelectList(Emplist, "Id", "Display");
                ViewBag.LeaveTypeId = new SelectList(_context.HrLeaveTypes.Where(a => a.IsActive == true), "Id", "NameAr", model.LeaveTypeId);

                return View(model);
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
                LeaveDays = model.ActualDays,
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                CreatedUserId = 1, // TODO: استبدلها بالمستخدم الحالي
                IsActive = true
            };
            _context.HrEmployeeLeaves.Add(entity);
            // HR_Employee_LeaveBalance اجمالى الاجازة للموظف
            var leaveBalance = _context.HrEmployeeLeaveBalances.FirstOrDefault(e => e.Id == model.LeaveBalanceID);

            if (leaveBalance != null)
            {
                leaveBalance.UsedDays =decimal.Parse(model.ActualDays.ToString()) ;  // new value
                leaveBalance.UpdatedDate = DateTime.Now;

                _context.SaveChanges(); // commits changes to the database
            }
            //HR_Employee_Monthly_effects تأثيرات المرتب
            if(model.DeductedDays > 0)
            {

            }
            _context.SaveChanges();
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
            ViewBag.Date = DateTime.Now;
            var data = (from l in _context.HrEmployeeLeaves
                        join e in _context.HrEmployees on l.EmployeeId equals e.Id
                        join t in _context.HrLeaveTypes on l.LeaveTypeId equals t.Id
                        join tb in _context.HrEmployeeLeaveBalances
                            on new { EmpId = e.Id, Year = l.StartDate.Value.Year }
                            equals new { EmpId = tb.EmployeeId, Year = tb.Year } into balanceJoin
                        from tb in balanceJoin.DefaultIfEmpty()
                        where l.Id == id
                        select new EmployeeLeaveVM
                        {
                            Id = l.Id,
                            EmployeeId = e.Id,
                            EmployeeCode = e.EmpCode,
                            EmployeeName = e.NameAr,
                            DepartmentName=e.CurrentBranchDept.Department.NameAr,
                            LeaveTypeId = t.Id,
                            LeaveTypeName = t.NameAr,
                            StartDate = l.StartDate,
                            EndDate = l.EndDate,
                            Reason = l.Reason,
                            TotalDays = tb.TotalDays,

                            // ✅ استخدم 0 لو مفيش سجل رصيد
                            RemainingBefore = (t.NameAr ?? "").Contains("عرض")
                                ? (tb != null ? (int)tb.CasualRemainingDays : 0)
                                : (tb != null ? (int)tb.RemainingDays : 0),

                            UsedDays = (t.NameAr ?? "").Contains("عرض")
                                ? (tb != null ? (int)tb.CasualUsedDays : 0)
                                : (tb != null ? (int)tb.UsedDays : 0)
                        }).FirstOrDefault();

            if (data == null)
                return Content("لم يتم العثور على الإجازة المطلوبة");

            if (data.StartDate == null || data.EndDate == null)
                return Content("بيانات التواريخ غير صحيحة.");

            //// أيام الاجازة المطلوبة
            ///
            DateTime startDate = DateTime.Parse( data.StartDate.Value.ToString());
            DateTime endDate = DateTime.Parse(data.EndDate.Value.ToString());
            // حساب الفرق بين التاريخين
            int RequiredVacationDays = (endDate - startDate).Days + 1;

            //int RequiredVacationDays = (data.EndDate.Value.Day - data.StartDate.Value.Day) + 1;
            ////  أيام الاجازة المطلوبة من غير جمع

            RequiredVacationDays = Enumerable.Range(0, RequiredVacationDays)
                            .Select(i => data.StartDate.Value.AddDays(i))
                            .Count(d => d.DayOfWeek != DayOfWeek.Friday);

            ViewBag.RequiredVacationDays = RequiredVacationDays;
            
            //// أول يومين بدون خصم
            //int deductedDays = actualDays <= 2 ? 0 : actualDays - 2;

            //// الرصيد بعد الخصم
            //int remainingAfter = data.RemainingBefore - actualDays;
            //if (remainingAfter < 0) remainingAfter = 0;

            //// تعيين القيم في الموديل
            ////data.TotalDays = totalDays;
            //data.ActualDays = actualDays;
            //data.DeductedDays = deductedDays;
            //data.RemainingAfter = remainingAfter;


            return View(data);
        }
        [HttpPost]
        public JsonResult CheckLeaveDate(long id, string startDate, string endDate)
        {
            DateOnly start = DateOnly.Parse(startDate);
            DateOnly end = DateOnly.Parse(endDate);

            // البحث عن أي إجازة متداخلة لنفس الموظف
            var overlappingLeaves = _context.HrEmployeeLeaves
                .Where(l => l.EmployeeId == id &&
                            l.IsActive &&
                            ((l.StartDate <= end) && (l.EndDate >= start)))
                .Select(l => new
                {
                    l.Id,
                    l.StartDate,
                    l.EndDate,
                    l.Reason
                })
                .ToList();

            if (overlappingLeaves.Any())
            {
                // إنشاء رسالة تحتوي على الأيام المتداخلة
                string message = "هناك إجازة موجودة بالفعل في الفترة المحددة:\n";
                foreach (var leave in overlappingLeaves)
                {
                    message += $"- من {leave.StartDate?.ToString("yyyy-MM-dd")} إلى {leave.EndDate?.ToString("yyyy-MM-dd")} الاسباب:  ({leave.Reason})\n";
                }

                return Json(new
                {
                    hasConflict = true,
                    message = message
                });
            }

            return Json(new { hasConflict = false });
        }

        [HttpGet]
        public IActionResult GetLeaveBalance(long employeeId)
        {
            int currentYear = DateTime.Now.Year;

            //var balance = _context.HrEmployeeLeaveBalances
            //                .Where(b => b.EmployeeId == employeeId 
            //                && b.Year == currentYear)
            //                .Select(b => new
            //                {
            //                    b.Id,
            //                    b.TotalDays,
            //                    b.UsedDays,
            //                    b.RemainingDays
            //                }).FirstOrDefault();
            // تعريف DTO مؤقت للتعامل مع النتائج

            // جلب الرصيد والإجازات
            var balance = _context.HrEmployeeLeaveBalances
                .Where(b => b.EmployeeId == employeeId && b.Year == currentYear)
                .Select(b => new EmployeeLeaveBalanceDto
                {
                    Id = b.Id,
                    TotalDays = b.TotalDays,
                    UsedDays = b.UsedDays,
                    RemainingDays = b.RemainingDays,
                    Leaves = _context.HrEmployeeLeaves
                                .Where(l => l.HrEmployeeLeaveBalanceId == b.Id
                                            && l.StartDate.HasValue
                                            && l.EndDate.HasValue)
                                .ToList()
                })
                .FirstOrDefault();

            if (balance != null)
            {
                // حساب الأيام المستخدمة لكل شهر حتى لو الإجازة تمتد بين شهور
                var usedDaysPerMonth = balance.Leaves
                    .SelectMany(l =>
                    {
                        var daysList = new List<(int Month, int Days)>();
                        var start = l.StartDate.Value;
                        var end = l.EndDate.Value;

                        while (start <= end)
                        {
                            // آخر يوم في الشهر الحالي
                            var endOfMonth = new DateOnly(start.Year, start.Month, DateTime.DaysInMonth(start.Year, start.Month));
                            var currentEnd = end < endOfMonth ? end : endOfMonth;

                            // عدد الأيام في هذا الشهر
                            int daysInMonth = currentEnd.DayNumber - start.DayNumber + 1;
                            daysList.Add((start.Month, daysInMonth));

                            // الانتقال للشهر التالي
                            start = currentEnd.AddDays(1);
                        }

                        return daysList;
                    })
                    .GroupBy(x => x.Month)
                    .Select(g => new
                    {
                        Month = g.Key,
                        UsedDays = g.Sum(x => x.Days)
                    })
                    .OrderBy(x => x.Month)
                    .Select(x => $"{x.Month}/{x.UsedDays}");

                // تحويل النتيجة إلى نص
                balance.UsedDaysMonth = string.Join(" : ", usedDaysPerMonth);

            }
            return Json(balance);
        }

        public class EmployeeLeaveBalanceDto
        {
            public long Id { get; set; }
            public decimal TotalDays { get; set; }
            public decimal UsedDays { get; set; }
            public decimal? RemainingDays { get; set; }
            public List<HrEmployeeLeaf> Leaves { get; set; }
            public string UsedDaysMonth { get; set; }
        }

    }
}
