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
    public IActionResult IndexALL()
    {
        var employees = db.HrEmployees
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
    .ToList();

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
    public async Task<ActionResult> AddEdite(HrEmployee model, IFormFile NidPath)
    {
       
        try
        {
            long HR_Employees_ID;

            // ===== إنشاء موظف جديد =====
            if (model.Id == 0)
            {
                HR_Employees_ID = db.Database
                    .SqlQueryRaw<long>("SELECT NEXT VALUE FOR dbo.HR_Employees_SEQ")
                    .AsEnumerable()
                    .First();

                model.Id = HR_Employees_ID;

                // معالجة ملف NID
                if (NidPath != null && NidPath.Length > 0)
                {
                    var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string uploadFolder = Path.Combine(webRoot, "uploads", "NidPath");



                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);
                    // اسم ملف فريد
                    string fileName = $"{Guid.NewGuid()}_{NidPath.FileName}";
                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        NidPath.CopyTo(stream);
                    }

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                    model.Password = hashedPassword;
                    // المسار الذي يُخزن في الداتابيز
                    model.NidPath = $"/uploads/NidPath/{fileName}"; ;
                }
                
                model.IsActive = model.IsActive; // من الفيو
                //model.CreatedDate = DateTime.Now.Date;
                //model.CreatedUserId = int.Parse(Session["User_ID"].ToString());

                db.HrEmployees.Add(model);
                await db.SaveChangesAsync();


                #region احتساب الاجازة
                // التعديل فى الارصدة
                // لو تاريخ التعيين اقل من 10 سنوات له 21 يوم 
                // 30 يوم بعد 10 سنوات
                // 45 يوم اجازة بعد سن 50 سنه
                // 45 يوم للإعاقة
                // جميع المراحل 7 ايام عارضه
                // model.AppointmentDate تاريخ التعيين
                var Epmloyeebalance = db.HrEmployeeLeaveBalances.Where(a => a.EmployeeId == model.Id).FirstOrDefault();
                if (Epmloyeebalance==null)
                {
                    var age = NationalIdExtensions.GetAge(model.Nid);

                    // -------------------------
                    // حساب مدة الخدمة
                    // -------------------------
                    int yearsOfService = 0;

                    if (model.AppointmentDate != null)
                    {
                        var appointDate = model.AppointmentDate.Value.ToDateTime(new TimeOnly(0, 0));

                        yearsOfService = DateTime.Now.Year - appointDate.Year;

                        if (appointDate.Date > DateTime.Now.AddYears(-yearsOfService))
                            yearsOfService--;
                    }

                    // -------------------------
                    // حساب رصيد الإجازات للسنة الجديدة
                    // -------------------------
                    int annualLeaveDays = 5;   // رصيد السنة الجديدة الأساسي
                    int casualLeaveDays = 7;
                    int totalDays = 0;

                    if (model.Disability == true)
                    {
                        totalDays = 45;
                    }
                    else if (age >= 50)
                    {
                        totalDays = 45;
                    }
                    else if (yearsOfService >= 10)
                    {
                        totalDays = 30;
                    }
                    else
                    {
                        totalDays = 21;
                    }
                    // var Epmloyeebalance = db.HrEmployeeLeaveBalances.Where(a => a.EmployeeId == model.Id).FirstOrDefault();

                    // -------------------------
                    // إضافة المتبقي من السنة السابقة (حد أقصى 7)
                    // -------------------------
                    //int previousYearRemaining = Epmloyeebalance?.AnnualRemainingDays ?? 0;
                    //int carryOver = 0;

                    //// شرط بداية السنة الجديدة
                    //bool isNewYear = (DateTime.Now.Month == 1 && DateTime.Now.Day == 1);

                    //if (isNewYear)
                    //{
                    //    carryOver = Math.Min(previousYearRemaining, 7);

                    //    // إضافتهم لرصيد السنة الجديدة
                    //    totalDays += carryOver;
                    //}

                    // -------------------------
                    // حفظ رصيد السنة الجديدة
                    // -------------------------
                    HrEmployeeLeaveBalance ff = new HrEmployeeLeaveBalance
                    {
                        EmployeeId = model.Id,
                        TotalDaysReminig = totalDays,
                        AnnualTotalDays = annualLeaveDays,
                        AnnualUsedDays = 0,
                        AnnualRemainingDays = annualLeaveDays,    // ← رصيد السنة الجديدة بعد الإضافة
                        CasualTotalDays = casualLeaveDays,
                        CasualUsedDays = 0,
                        CasualRemainingDays = casualLeaveDays,
                        TotalDays = totalDays,
                        UsedDays = 0,
                        CreatedDate = DateTime.Now,
                        Year = DateTime.Now.Year,                // ← أضف تبويب السنة الجديدة
                        IsActive = true
                    };

                    db.HrEmployeeLeaveBalances.Add(ff);
                    await db.SaveChangesAsync();
                }
               

                #endregion

            }
            else
            {
                // ===== تعديل موظف موجود =====
                var existingEmployee = db.HrEmployees.Find(model.Id);
                if (existingEmployee == null)
                    return NotFound();
                // معالجة ملف NID
                if (NidPath != null && NidPath.Length > 0)
                {
                    var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string uploadFolder = Path.Combine(webRoot, "uploads", "NidPath");



                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);
                    // اسم ملف فريد
                    string fileName = $"{Guid.NewGuid()}_{NidPath.FileName}";
                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        NidPath.CopyTo(stream);
                    }


                    // المسار الذي يُخزن في الداتابيز
                    existingEmployee.NidPath = $"/uploads/NidPath/{fileName}"; ;
                }
                // تحديث البيانات الأساسية
                existingEmployee.EmpCode = model.EmpCode;
                existingEmployee.NameAr = model.NameAr;
                existingEmployee.Nid = model.Nid;
                existingEmployee.Email = model.Email;
                existingEmployee.PhoneNumber = model.PhoneNumber;
                existingEmployee.AddressAr = model.AddressAr;
                existingEmployee.IsActive = model.IsActive;
                existingEmployee.EmployeeTypeId = model.EmployeeTypeId; // نوع الموظف
                existingEmployee.BranchId = model.BranchId;
                existingEmployee.DepartmentId = model.DepartmentId;
                existingEmployee.CurrentJobId = model.CurrentJobId;
                existingEmployee.AppointmentDate = model.AppointmentDate;
                existingEmployee.CurrentSalary = model.CurrentSalary;
                existingEmployee.Disability = model.Disability;
                existingEmployee.Isbank = model.Isbank;
                existingEmployee.InsuranceNumber = model.InsuranceNumber;
                existingEmployee.DateIn = model.DateIn;
                existingEmployee.DateOut = model.DateOut;
               
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                //model.Password = hashedPassword;
                bool isCorrect = BCrypt.Net.BCrypt.Verify(existingEmployee.Password, hashedPassword);
                if (isCorrect)
                {
                    // Login success
                }
                else
                {
                    model.Password = hashedPassword;
                }
                // معالجة ملف NID جديد
                if (NidPath != null && NidPath.Length > 0)
                {
                    // مسار حفظ الملفات داخل wwwroot
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "Uploads", "NidPath");

                    // إنشاء المجلد إذا لم يكن موجوداً
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileName = Path.GetFileName(NidPath.FileName);
                    string fullPath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await NidPath.CopyToAsync(stream);
                    }

                    // حفظ المسار النسبي في قاعدة البيانات
                    existingEmployee.NidPath = Path.Combine("Uploads", "NidPath", fileName).Replace("\\", "/");
                }
                var Epmloyeebalance=db.HrEmployeeLeaveBalances.Where(a=>a.EmployeeId==model.Id).FirstOrDefault();
                if (Epmloyeebalance==null)
                {
                    var age = NationalIdExtensions.GetAge(model.Nid);

                    // -------------------------
                    // حساب مدة الخدمة
                    // -------------------------
                    int yearsOfService = 0;

                    if (model.AppointmentDate != null)
                    {
                        var appointDate = model.AppointmentDate.Value.ToDateTime(new TimeOnly(0, 0));

                        yearsOfService = DateTime.Now.Year - appointDate.Year;

                        if (appointDate.Date > DateTime.Now.AddYears(-yearsOfService))
                            yearsOfService--;
                    }

                    // -------------------------
                    // حساب رصيد الإجازات للسنة الجديدة
                    // -------------------------
                    int annualLeaveDays = 5;   // رصيد السنة الجديدة الأساسي
                    int casualLeaveDays = 7;
                    int totalDays = 0;

                    if (model.Disability == true)
                    {
                        totalDays = 45;
                    }
                    else if (age >= 50)
                    {
                        totalDays = 45;
                    }
                    else if (yearsOfService >= 10)
                    {
                        totalDays = 30;
                    }
                    else
                    {
                        totalDays = 21;
                    }

                    // -------------------------
                    // إضافة المتبقي من السنة السابقة (حد أقصى 7)
                    // -------------------------
                    //int previousYearRemaining = Epmloyeebalance?.AnnualRemainingDays ?? 0;
                    //int carryOver = 0;

                    //// شرط بداية السنة الجديدة
                    //bool isNewYear = (DateTime.Now.Month == 1 && DateTime.Now.Day == 1);

                    //if (isNewYear)
                    //{
                    //    carryOver = Math.Min(previousYearRemaining, 7);

                    //    // إضافتهم لرصيد السنة الجديدة
                    //    totalDays += carryOver;
                    //}

                    // -------------------------
                    // حفظ رصيد السنة الجديدة
                    // -------------------------
                    HrEmployeeLeaveBalance ff = new HrEmployeeLeaveBalance
                    {
                        EmployeeId = model.Id,
                        TotalDaysReminig = totalDays,
                        AnnualTotalDays= annualLeaveDays,
                        AnnualRemainingDays = annualLeaveDays,    // ← رصيد السنة الجديدة بعد الإضافة
                        CasualTotalDays = casualLeaveDays,
                        CasualRemainingDays = casualLeaveDays,
                        TotalDays = totalDays,
                        CreatedDate = DateTime.Now,
                        Year = DateTime.Now.Year,                // ← أضف تبويب السنة الجديدة
                        IsActive = true
                    };

                    db.HrEmployeeLeaveBalances.Add(ff);
                    await db.SaveChangesAsync();


                }

                // تحديث السجل في قاعدة البيانات باستخدام EF Core
                db.HrEmployees.Update(existingEmployee);
                await db.SaveChangesAsync();
            }

            if (model.Id != 0)
            {
                return RedirectToAction("IndexALL", "EmployeeData", new { area = "Employee" });


            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Account", EmployeeID = model.Id });

            }

        }
        catch (Exception ex)
        {
            ViewBag.Message = "حدث خطأ أثناء الحفظ: " + ex.Message;
            //FillDroup(model);
            return View(model);
        }
    }


}
