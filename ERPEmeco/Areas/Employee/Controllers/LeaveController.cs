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
        public ActionResult Index(string search)
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

            // 🔍 البحث
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    (x.e.NameAr != null && x.e.NameAr.Contains(search)) ||
                    (x.t.NameAr != null && x.t.NameAr.Contains(search)) ||
                    (x.l.Reason != null && x.l.Reason.Contains(search))
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
                  DepartmentManagerApproval = x.l.DepartmentManagerApproval,
                  EmployeeUserType = x.e.EmployeeTypeId.Value   // ⭐ مهم
              })

                .ToList();

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
                    employeesQuery = employeesQuery.Where(e => e.DepartmentId == departmentId);
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
            
            // ==================================================
            // ❌ منع أخذ اعتيادي في اليوم التالي للعارضة
            // ==================================================
            if (model.LeaveTypeId == 2) // اعتيادي
            {
                var lastCasualLeave = _context.HrEmployeeLeaves
                    .Where(x => x.EmployeeId == model.EmployeeId
                             && x.LeaveTypeId == 1   // عارضة
                             && x.IsActive)
                    .OrderByDescending(x => x.EndDate)
                    .FirstOrDefault();

                if (lastCasualLeave != null && lastCasualLeave.EndDate.HasValue)
                {
                    DateOnly blockedDate = lastCasualLeave.EndDate.Value.AddDays(1);

                    if (model.StartDate == blockedDate)
                    {
                        TempData["ErrorMessage"] = "❌ لا يمكن أخذ إجازة اعتيادي في اليوم التالي مباشرة لإجازة عارضة.";
                        ReloadViewBags(model);
                        return View(model);
                    }
                }
            }
            // ==================================================
            // ❌ منع عمل أكثر من إجازة في نفس اليوم
            // ==================================================
            var hasLeaveSameDay = _context.HrEmployeeLeaves
                .Where(x => x.EmployeeId == model.EmployeeId
                         && x.IsActive
                         // لو عندك حالة موافقة / رفض
                         && x.DepartmentManagerApproval != false) // عدليها حسب اسم الحقل عندك
                .Any(x =>
                    model.StartDate <= x.EndDate &&
                    model.EndDate >= x.StartDate
                );

            if (hasLeaveSameDay)
            {
                TempData["ErrorMessage"] = "❌ لا يمكن إنشاء أكثر من إجازة في نفس اليوم إلا إذا تم رفض السابقة.";
                ReloadViewBags(model);
                return View(model);
            }
            // تحقق من وجود مأمورية تتداخل مع الإجازة
            bool hasMissionConflict = _context.HrEmployeeOfficialMissions
                .Any(m => m.EmployeeId == model.EmployeeId
                       && m.IsActive
                       && m.StartDate <= model.EndDate
                       && m.EndDate >= model.StartDate);

            if (hasMissionConflict)
            {
                TempData["ErrorMessage"] = "❌ لا يمكن إنشاء الإجازة، الموظف لديه مأمورية تتداخل مع نفس الفترة.";
                ReloadViewBags(model);
                return View(model);
            }

            // تحقق من وجود إذن في نفس اليوم
            bool hasPermissionConflict = _context.HrEmployeePermissions
                .Any(p => p.EmployeeId == model.EmployeeId
                       && p.DateOfPermission.HasValue
                       && model.StartDate <= p.DateOfPermission.Value
                       && model.EndDate >= p.DateOfPermission.Value
                       && p.IsActive);

            if (hasPermissionConflict)
            {
                TempData["ErrorMessage"] = "❌ لا يمكن إنشاء الإجازة، الموظف لديه إذن في نفس اليوم.";
                ReloadViewBags(model);
                return View(model);
            }


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
            //// HR_Employee_LeaveBalance اجمالى الاجازة للموظف
            //var leaveBalance = _context.HrEmployeeLeaveBalances.FirstOrDefault(e => e.Id == model.LeaveBalanceID);
            //var totalold = leaveBalance.TotalDays;
            //if (leaveBalance != null)
            //{
                

            //    // =============================
            //    // 1) إجازة عارضة (ID = 1)
            //    // =============================
            //    if (model.LeaveTypeId == 1)
            //    {
            //        leaveBalance.CasualUsedDays += (int)model.ActualDays;
            //        leaveBalance.CasualRemainingDays =
            //            (int)(leaveBalance.CasualTotalDays - leaveBalance.CasualUsedDays);
            //    }

            //    // =============================
            //    // 2) إجازة اعتيادي (ID = 2)
            //    // =============================
            //    if (model.LeaveTypeId == 2)
            //    {
            //        leaveBalance.UsedDays += (int)model.ActualDays;
            //        leaveBalance.TotalDaysReminig =(int)(leaveBalance.TotalDays - leaveBalance.UsedDays);
            //    }

            //    //// =============================
            //    //// 3) إجازة سنوي (ID = 5)
            //    //// = نفس حساب الاعتيادي
            //    //// =============================
            //    if (model.LeaveTypeId == 5)
            //    {
            //        leaveBalance.AnnualUsedDays += (int)model.ActualDays;
            //        leaveBalance.AnnualRemainingDays =
            //            (int)(leaveBalance.AnnualTotalDays - leaveBalance.AnnualUsedDays);
            //        leaveBalance.UsedDays += (int)model.ActualDays;
            //        leaveBalance.TotalDaysReminig =(int)(leaveBalance.TotalDays - (int)model.ActualDays);

            //    }

            //    //// =============================
            //    //// تحديث الإجمالي العام
            //    //// =============================
            //    //leaveBalance.TotalDaysReminig =
            //    //    (int)(leaveBalance.TotalDays - leaveBalance.UsedDays);

            //    leaveBalance.UpdatedDate = DateTime.Now;
            //    leaveBalance.UpdatedUserId = 1;


            //}
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
            var leaveData = (from l in _context.HrEmployeeLeaves
                             join e in _context.HrEmployees on l.EmployeeId equals e.Id
                             join w in _context.EmployeeTypes on e.EmployeeTypeId equals w.Id into pt
                             from w in pt.DefaultIfEmpty()
                             join t in _context.HrLeaveTypes on l.LeaveTypeId equals t.Id
                             join d in _context.HrDepartments on e.DepartmentId equals d.Id into dept
                             from d in dept.DefaultIfEmpty()
                             where l.Id == id
                             select new
                             {
                                 Leave = l,
                                 Employee = e,
                                 Department = d,
                                 LeaveType = t,
                                 EmployeeType = w
                             }).FirstOrDefault();

            if (leaveData == null)
                return Content("❌ لم يتم العثور على الإجازة");

            // ===== تحديد السنة =====
            int year = leaveData.Leave.StartDate?.Year ?? DateTime.Now.Year;

            DateOnly startDate = leaveData.Leave.StartDate ?? DateOnly.FromDateTime(DateTime.Now);
            DateOnly endDate = leaveData.Leave.EndDate ?? startDate;

            // ===== حساب عدد أيام الإجازة الحالية (بدون الجمعة) =====
            int totalDays = endDate.DayNumber - startDate.DayNumber + 1;
            totalDays = Math.Max(1, totalDays);

            int actualDays = Enumerable.Range(0, totalDays)
                .Select(i => startDate.AddDays(i))
                .Count(d => d.DayOfWeek != DayOfWeek.Friday);

            // ===== جلب الرصيد السنوي =====
            var balance = _context.HrEmployeeLeaveBalances
                .Where(b => b.EmployeeId == leaveData.Employee.Id && b.Year == year)
                .OrderByDescending(b => b.Id)
                .FirstOrDefault();

            int regTotal = balance?.TotalDays ?? 0;
            int casTotal = balance?.CasualTotalDays ?? 0;

            // ===== جلب الإجازات السابقة فقط (قبل الإجازة الحالية) =====
            var previousLeaves = _context.HrEmployeeLeaves
                .Where(l => l.EmployeeId == leaveData.Employee.Id
                            && l.Id != leaveData.Leave.Id   // استبعاد الحالية
                            && l.StartDate.HasValue
                            && l.StartDate.Value.Year == year
                            && l.StartDate.Value < startDate
                            && l.IsActive == true
                            && l.DepartmentManagerApproval == true)
                .ToList();

            int regUsedBefore = 0;
            int casUsedBefore = 0;

            foreach (var lv in previousLeaves)
            {
                DateOnly s = lv.StartDate ?? DateOnly.FromDateTime(DateTime.Now);
                DateOnly e = lv.EndDate ?? s;

                int days = e.DayNumber - s.DayNumber + 1;
                days = Math.Max(1, days);

                int actual = Enumerable.Range(0, days)
                    .Select(i => s.AddDays(i))
                    .Count(d => d.DayOfWeek != DayOfWeek.Friday);

                // اعتيادي
                if (lv.LeaveTypeId == 2 || lv.LeaveTypeId == 5)
                    regUsedBefore += actual;

                // عارضة
                if (lv.LeaveTypeId == 1)
                    casUsedBefore += actual;
            }

            // ===== الرصيد وقت أخذ الإجازة =====
            int regRemainingAtThatTime = Math.Max(regTotal - regUsedBefore, 0);
            int casRemainingAtThatTime = Math.Max(casTotal - casUsedBefore, 0);

            var vm = new EmployeeLeaveVM
            {
                Id = leaveData.Leave.Id,
                EmployeeName = leaveData.Employee.NameAr,
                DepartmentID = leaveData.Employee.DepartmentId,
                EmployeeCode = leaveData.Employee.EmpCode,
                DepartmentName = leaveData.Department?.NameAr ?? "-",
                LeaveTypeId = leaveData.LeaveType.Id,
                LeaveTypeName = leaveData.LeaveType.NameAr,
                StartDate = leaveData.Leave.StartDate,
                EndDate = leaveData.Leave.EndDate,
                Reason = leaveData.Leave.Reason,
                AttachmentPath = leaveData.Leave.AttachmentPath,
                ActualDays = actualDays,
                EmployeeTypeName = leaveData.EmployeeType?.EmployeeTypeNameAr ?? "-",

                // بيانات الرصيد
                TotalDays = regTotal,
                UsedDays = regUsedBefore,
                CasualTotalDays = casTotal,
                CasualUsedDays = casUsedBefore,
                RegularRemainingAfter = regRemainingAtThatTime,
                CasualRemainingAfter = casRemainingAtThatTime
            };

            return View("PrintNew", vm);
        }
        private void ReloadViewBags(EmployeeLeaveVM model)
        {
            var employees = _context.HrEmployees
                .Where(e => e.IsActive)
                .Select(e => new
                {
                    e.Id,
                    Display = e.NameAr + " (" + e.EmpCode + ")"
                }).ToList();

            ViewBag.EmployeeOptions = new SelectList(employees, "Id", "Display");

            ViewBag.LeaveTypeId = new SelectList(
                _context.HrLeaveTypes.Where(a => a.IsActive),
                "Id",
                "NameAr",
                model.LeaveTypeId);
        }
        // [HttpPost]
        //public IActionResult DirectManagerAction(int id, bool isApproved,string type)
        //{
        //    var leave = _context.HrEmployeeLeaves.FirstOrDefault(x => x.Id == id);
        //    if (leave == null)
        //        return Json(new { success = false, message = "لم يتم العثور على الإجازة" });

        //    DateOnly today = DateOnly.FromDateTime(DateTime.Now);

        //    // منع الموافقة او الرفض لو اليوم > تاريخ الاجازة
        //    if (today > leave.StartDate)
        //    {
        //        return Json(new
        //        {
        //            success = false,
        //            message = "لا يمكن الموافقة أو الرفض بعد موعد بداية الإجازة."
        //        });
        //    }

        //    // في حالة مسموح
        //    if (type=="direct")
        //    {
        //        leave.DirectManagerApproval = isApproved;

        //    }
        //    else if (type == "sector")
        //    {

        //        leave.DepartmentManagerApproval = isApproved;
        //    }
        //       ;
        //    leave.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

        //    _context.SaveChanges();

        //    return Json(new { success = true, message = "تم تحديث حالة الإجازة بنجاح" });
        //}

        [HttpPost]
        public IActionResult DirectManagerAction(int id, bool isApproved, string type, int ActualDays,int EmployeeUsertype)
        {

            int? userId = Request.Cookies.ContainsKey("UserId") ? int.Parse(Request.Cookies["UserId"]) : null;

            var leave = _context.HrEmployeeLeaves.FirstOrDefault(x => x.Id == id);
            if (leave == null)
                return Json(new { success = false, message = "لم يتم العثور على الإجازة" });

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);


            if (!leave.StartDate.HasValue)
            {
                return Json(new { success = false, message = "❌ تاريخ بداية الإجازة غير محدد" });
            }

            DateOnly startDate = leave.StartDate.Value;
            DateOnly lastAllowedDate = startDate.AddDays(2); // اليومين التاليين بعد بداية الإجازة

            // ==================================================
            // السماح قبل الإجازة أو يومها واليومين التاليين
            // ==================================================
            if (today > lastAllowedDate) // بعد اليومين المسموحين
            {
                return Json(new
                {
                    success = false,
                    message = "❌ لا يمكن الموافقة على الإجازة بعد اليومين التاليين لبداية الإجازة."
                });
            }
            // في حالة مسموح
            if (type == "direct")
            {
                leave.DirectManagerApproval = isApproved;
                

            }
            else if (type == "sector")
            {

                leave.DepartmentManagerApproval = isApproved;
                if (isApproved == true)// فى حالة قبول الاجازة
                {
                    var leaveBalance = _context.HrEmployeeLeaveBalances.FirstOrDefault(e => e.EmployeeId == leave.EmployeeId
                    && e.Year == DateTime.Now.Year);
                    if (leaveBalance != null)
                    {
                        // =============================
                        // 1) إجازة عارضة (ID = 1)
                        // =============================
                        if (EmployeeUsertype == 1)
                        {
                            leaveBalance.CasualUsedDays = leaveBalance.CasualUsedDays + ActualDays;                         
                        }

                        // =============================
                        // 2) إجازة اعتيادي (ID = 2)
                        // =============================
                        if (EmployeeUsertype == 2)
                        {
                            leaveBalance.UsedDays = leaveBalance.UsedDays + ActualDays;
                        }

                        //// =============================
                        //// 3) إجازة سنوي (ID = 5)
                        //// = نفس حساب الاعتيادي
                        //// =============================
                        if (EmployeeUsertype == 5)
                        {
                            leaveBalance.AnnualUsedDays = leaveBalance.AnnualUsedDays + ActualDays;
                           
                            leaveBalance.UsedDays = leaveBalance.UsedDays + ActualDays; ;

                        }

                       
                        leaveBalance.UpdatedDate = DateTime.Now;
                        leaveBalance.UpdatedUserId = userId;

                        _context.SaveChanges();
                    }
                }
            }
        ;
            leave.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);

            _context.SaveChanges();

            return Json(new { success = true, message = "تم تحديث حالة الإجازة بنجاح" });
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
