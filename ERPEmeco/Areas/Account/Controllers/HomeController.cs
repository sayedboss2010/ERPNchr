using DAL.Classes.DroupDowen;
using DAL.Classes.Employee;
using EF.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
//using WEB.Models;

namespace WEB.Areas.Account.Controllers;

//[AreaAuthentication]
[Area("Account")]
public class HomeController : Controller
{
    private readonly AppDbContext db = new AppDbContext();
    public IActionResult Index(long EmployeeID = 0)
    {
        var objlst = EmployeeDataDal.Find(EmployeeID);
        objlst.EmolyeetypeName = db.EmployeeTypes
                          .Where(x => x.Id == objlst.EmployeeTypeId)
                          .Select(x => x.EmployeeTypeNameAr)
                          .FirstOrDefault();

        objlst.DepartmentName = db.HrDepartments
                           .Where(x => x.Id == objlst.DepartmentId)
                           .Select(x => x.NameAr)
                           .FirstOrDefault();

        objlst.BranchName = db.HrBranches
.Where(x => x.Id == objlst.BranchId)
                                   .Select(x => x.NameAr)
                                   .FirstOrDefault();

        objlst.JobName = db.HrJobs
                                .Where(x => x.Id == objlst.CurrentJobId)
                                .Select(x => x.TitleAr)
                                .FirstOrDefault();

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
                   AppointmentDate=e.AppointmentDate,
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
               .FirstOrDefault();
            employees.DepartmentName = db.HrDepartments
                      .Where(x => x.Id == employees.DepartmentId)
                      .Select(x => x.NameAr)
                      .FirstOrDefault();

            employees.BranchName = db.HrBranches
    .Where(x => x.Id == employees.BranchId)
                                       .Select(x => x.NameAr)
                                       .FirstOrDefault();

            employees.JobName = db.HrJobs
                                    .Where(x => x.Id == employees.CurrentJobId)
                                    .Select(x => x.TitleAr)
                                    .FirstOrDefault();
            employees.EmolyeetypeName = db.EmployeeTypes
                        .Where(x => x.Id == objlst.EmployeeTypeId)
                        .Select(x => x.EmployeeTypeNameAr)
                        .FirstOrDefault();
            return View(employees);

        }
        var ListHrEmployeesTypes = new SelectList(DroupDowenDal.ListHrEmployeesTypes(), "Id", "EmployeeTypeNameAr");
        ViewBag.ListHrEmployeesTypes = ListHrEmployeesTypes;
        return View(objlst);

    }
    public IActionResult TestTable()
    {
        return View();
    }
}