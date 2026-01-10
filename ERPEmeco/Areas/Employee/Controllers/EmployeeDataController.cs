using BCrypt.Net;
using DAL.Classes.DroupDowen;
using DAL.Classes.Employee;
using EF.Data;
using EF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web;
using VM.ViewModels;


namespace ERPNchr.Areas.Employee.Controllers;

//[AreaAuthentication]
[Area("Employee")]
public class EmployeeDataController : Controller
{
    private readonly AppDbContext db = new AppDbContext();
    private readonly IWebHostEnvironment _env;
    public EmployeeDataController(IWebHostEnvironment env)
    {
        _env = env;
    }
    public IActionResult IndexALL(string search)
    {
        var query = db.HrEmployees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
      e.NameAr.Contains(search) ||
      e.Nid.Contains(search) ||
      e.Email.Contains(search) ||
      (e.EmpCode != null && e.EmpCode.ToString().Contains(search))
  );

        }

        var employees = query.Select(e => new EmployeeVM
        {
            Id = e.Id,
            EmpCode = e.EmpCode,
            NameAr = e.NameAr,
            NameEn = e.NameEn,
            Email = e.Email,
            AddressAr = e.AddressAr,
            AddressEn = e.AddressEn,
            PhoneNumber = e.PhoneNumber,
            CurrentJobId = e.CurrentJobId,
            CurrentSalary = e.CurrentSalary,
            CurrentFunctionalDegreeId = e.CurrentFunctionalDegreeId,
            IsMananger = e.IsMananger,
            IsActive = e.IsActive,
            CreatedUserId = e.CreatedUserId,
            UpdatedUserId = e.UpdatedUserId,
            DeletedUserId = e.DeletedUserId,
            InsuranceNumber = e.InsuranceNumber,
            Nid = e.Nid,
            NidPath = e.NidPath,
            Mobile = e.Mobile,
            EmpCodeNew = e.EmpCodeNew,
            Isbank = e.Isbank,
            HrJobGradesId = e.HrJobGradesId,
            EmployeeTypeId = e.EmployeeTypeId,
            BranchId = e.BranchId,
            DepartmentId = e.DepartmentId
        }).ToList();

        return View(employees);
    }

    public IActionResult AddEdite(long EmployeeID = 0)
    {
        //var userId = int.Parse(Request.Cookies.FirstOrDefault(c => c.Key == "UserId").Value);
        //var materials = new SelectList(MaterialsDal.GetAllMaterials(userId), "Id", "MaterialNameAr");

       // var colors = new SelectList(ColorDal.list(userId), "Id", "ColorName");

        //ViewBag.Materials = materials;
        //ViewBag.colors = colors;

        //int pageSize = 10;

        var objlst = EmployeeDataDal.Find(EmployeeID);
        var ListHrDepartment = new SelectList(DroupDowenDal.ListHrDepartment(), "Id", "NameAr");
        ViewBag.ListHrDepartment = ListHrDepartment;
        var ListHrBranch = new SelectList(DroupDowenDal.ListHrBranch(), "Id", "NameAr");

        ViewBag.ListHrBranch = ListHrBranch;
        var ListHrJobs = new SelectList(DroupDowenDal.ListHrJobs(), "Id", "TitleAr");
       
        ViewBag.ListHrJobs = ListHrJobs;
        var ListHrEmployeesTypes = new SelectList(DroupDowenDal.ListHrEmployeesTypes(), "Id", "EmployeeTypeNameAr");
        ViewBag.ListHrEmployeesTypes = ListHrEmployeesTypes;
        //ViewBag.TotalResults = objlst.TotalResults;
        //ViewBag.CurrentPage = objlst.CurrentPage;
        //ViewBag.TotalPages = objlst.TotalPages;
        //ViewBag.Search = searchStr;


        //bool isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        //if (isAjax)
        //{
        //    return PartialView("_PartialAllCompanys", objlst.Items);
        //}

        if (EmployeeID > 0)
        {
            var employees = db.HrEmployees
                .Where(a => a.Id == EmployeeID)
               .Select(e => new EmployeeVM
               {
                   Id = e.Id,
                   EmpCode = e.EmpCode,
                   NameAr = e.NameAr,
                   NameEn = e.NameEn,
                   Email = e.Email,
                   Password = e.Password,
                   AddressAr = e.AddressAr,
                   AddressEn = e.AddressEn,
                   PhoneNumber = e.PhoneNumber,
                   Disability=e.Disability.Value,
                   //Birthdate = e.Birthdate.HasValue
                   //    ? DateOnly.FromDateTime(e.Birthdate.Value)
                   //    : null,

                   //HireDate = e.HireDate.HasValue
                   //    ? DateOnly.FromDateTime(e.HireDate.Value)
                   //    : null,

                   CurrentJobId = e.CurrentJobId,
                   CurrentSalary = e.CurrentSalary,
                   CurrentFunctionalDegreeId = e.CurrentFunctionalDegreeId,
                   IsMananger = e.IsMananger,
                   IsActive = e.IsActive,
                   CreatedUserId = e.CreatedUserId,
                   //CreatedDate = e.CreatedDate,
                   UpdatedUserId = e.UpdatedUserId,
                   AppointmentDate=e.AppointmentDate,
                   //UpdatedDate = e.UpdatedDate.HasValue
                   //    ? DateOnly.FromDateTime(e.UpdatedDate.Value)
                   //    : null,

                   DeletedUserId = e.DeletedUserId,

                   //DeletedDate = e.DeletedDate.HasValue
                   //    ? DateOnly.FromDateTime(e.DeletedDate.Value)
                   //    : null,

                   InsuranceNumber = e.InsuranceNumber,
                   Nid = e.Nid,

                   //DateIn = e.DateIn.HasValue
                   //    ? DateOnly.FromDateTime(e.DateIn.Value)
                   //    : null,

                   //DateOut = e.DateOut.HasValue
                   //    ? DateOnly.FromDateTime(e.DateOut.Value)
                   //    : null,
                   
                   NidPath = e.NidPath,
                   Mobile = e.Mobile,
                   EmpCodeNew = e.EmpCodeNew,
                   Isbank = e.Isbank,

                   //AppointmentDate = e.AppointmentDate.HasValue
                   //    ? DateOnly.FromDateTime(e.AppointmentDate.Value)
                   //    : null,

                   HrJobGradesId = e.HrJobGradesId,
                   EmployeeTypeId = e.EmployeeTypeId,
                   BranchId = e.BranchId,
                   DepartmentId = e.DepartmentId
               })
               .FirstOrDefault();
            return View(employees);

        }

        return View(objlst);


    }

    [HttpPost]
    public async Task<IActionResult> AddEdite(VM.ViewModels.EmployeeVM model, IFormFile NidPath)
    {
        try
        {
            // =========================
            // تحقق من البريد والرقم القومي
            // =========================
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                bool emailExists = db.HrEmployees.Any(e => e.Email == model.Email && e.Id != model.Id);
                if (emailExists)
                    ModelState.AddModelError("Email", "هذا البريد الإلكتروني مستخدم من موظف آخر!");
            }

            if (!string.IsNullOrWhiteSpace(model.Nid))
            {
                bool nidExists = db.HrEmployees.Any(e => e.Nid == model.Nid && e.Id != model.Id);
                if (nidExists)
                    ModelState.AddModelError("Nid", "هذا الرقم القومي مستخدم من موظف آخر!");
            }

            // =========================
            // إذا كانت هناك أخطاء
            // =========================
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.ListHrDepartment = GetDepartments();
            //    ViewBag.ListHrBranch = GetBranches();
            //    ViewBag.ListHrJobs = GetJobs();
            //    ViewBag.ListHrEmployeesTypes = GetEmployeeTypes();
            //    return View(model); // ← نعيد الفيو مع الاحتفاظ بالقيم
            //}

            // =========================
            // إنشاء أو تعديل الموظف
            // =========================
            HrEmployee employee;

            if (model.Id == 0)
            {
                // إنشاء موظف جديد
                long HR_Employees_ID = db.Database
                    .SqlQueryRaw<long>("SELECT NEXT VALUE FOR dbo.HR_Employees_SEQ")
                    .AsEnumerable()
                    .First();

                employee = new HrEmployee { Id = HR_Employees_ID };
                db.HrEmployees.Add(employee);
            }
            else
            {
                // تعديل موظف موجود
                employee = db.HrEmployees.Find(model.Id);
                if (employee == null)
                    return NotFound();
            }

            // =========================
            // ربط البيانات من ViewModel
            // =========================
            employee.EmpCode = model.EmpCode;
            employee.NameAr = model.NameAr;
            employee.Nid = model.Nid;
            employee.Email = model.Email;
            employee.PhoneNumber = model.PhoneNumber;
            employee.AddressAr = model.AddressAr;
            employee.IsActive = model.IsActive;
            employee.Disability = model.Disability;
            employee.EmployeeTypeId = model.EmployeeTypeId;
            employee.BranchId = model.BranchId;
            employee.DepartmentId = model.DepartmentId;
            employee.CurrentJobId = model.CurrentJobId;
            employee.AppointmentDate = model.AppointmentDate;
            employee.CurrentSalary = model.CurrentSalary;

            // =========================
            // معالجة كلمة المرور
            // =========================

            employee.Password = model.Password;
            //if (!string.IsNullOrWhiteSpace(model.Password))
            //{
            //    employee.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            //}

            // =========================
            // رفع ملف Nid
            // =========================
            if (NidPath != null && NidPath.Length > 0)
            {
                string webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string uploadFolder = Path.Combine(webRoot, "uploads", "NidPath");

                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                string fileName = $"{Guid.NewGuid()}_{NidPath.FileName}";
                string filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await NidPath.CopyToAsync(stream);
                }

                employee.NidPath = $"/uploads/NidPath/{fileName}";
            }

            await db.SaveChangesAsync();

            // =========================
            // حساب رصيد الإجازات إذا لم يوجد
            // =========================
            var leaveBalance = db.HrEmployeeLeaveBalances.FirstOrDefault(a => a.EmployeeId == employee.Id);
            if (leaveBalance == null)
            {
                var age = NationalIdExtensions.GetAge(employee.Nid);
                int yearsOfService = 0;
                if (employee.AppointmentDate != null)
                {
                    var appointDate = employee.AppointmentDate.Value.ToDateTime(new TimeOnly(0, 0));
                    yearsOfService = DateTime.Now.Year - appointDate.Year;
                    if (appointDate.Date > DateTime.Now.AddYears(-yearsOfService))
                        yearsOfService--;
                }

                int totalDays = 21;
                if (employee.Disability.GetValueOrDefault() || age >= 50)
                    totalDays = 45;
                else if (yearsOfService >= 10)
                    totalDays = 30;

                HrEmployeeLeaveBalance ff = new HrEmployeeLeaveBalance
                {
                    EmployeeId = employee.Id,
                    TotalDaysReminig = totalDays,
                    AnnualTotalDays = 5,
                    AnnualUsedDays = 0,
                    AnnualRemainingDays = 5,
                    CasualTotalDays = 7,
                    CasualUsedDays = 0,
                    CasualRemainingDays = 7,
                    TotalDays = totalDays,
                    UsedDays = 0,
                    CreatedDate = DateTime.Now,
                    Year = DateTime.Now.Year,
                    IsActive = true
                };

                db.HrEmployeeLeaveBalances.Add(ff);
                await db.SaveChangesAsync();
            }

            // =========================
            // إعادة التوجيه بعد الحفظ
            // =========================
            if (model.Id != 0)
                return RedirectToAction("IndexALL", "EmployeeData", new { area = "Employee" });
            else
                return RedirectToAction("Index", "Home", new { area = "Account", EmployeeID = employee.Id });
        }
        catch (Exception ex)
        {
            ViewBag.Message = "حدث خطأ أثناء الحفظ: " + ex.Message;
            ViewBag.ListHrDepartment = GetDepartments();
            ViewBag.ListHrBranch = GetBranches();
            ViewBag.ListHrJobs = GetJobs();
            ViewBag.ListHrEmployeesTypes = GetEmployeeTypes();
            return View(model);
        }
    }

    // =========================
    // وظائف مساعدة لتحميل Dropdowns
    // =========================
    private IEnumerable<SelectListItem> GetDepartments()
    {
        return db.HrDepartments.Select(d => new SelectListItem { Text = d.NameAr, Value = d.Id.ToString() }).ToList();
    }

    private IEnumerable<SelectListItem> GetBranches()
    {
        return db.HrBranches.Select(b => new SelectListItem { Text = b.NameAr, Value = b.Id.ToString() }).ToList();
    }

    private IEnumerable<SelectListItem> GetJobs()
    {
        return db.HrJobs.Select(j => new SelectListItem { Text = j.TitleAr, Value = j.Id.ToString() }).ToList();
    }

    private IEnumerable<SelectListItem> GetEmployeeTypes()
    {
        return db.EmployeeTypes.Select(t => new SelectListItem { Text = t.EmployeeTypeNameAr, Value = t.Id.ToString() }).ToList();
    }


}
