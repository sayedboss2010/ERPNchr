using EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Classes.DroupDowen
{
    public class DroupDowenDal
    {
        public static IList<HrDepartment> ListHrDepartment()
        {
            using var dbContext = new AppDbContext();

            var obj = (from emp in dbContext.HrDepartments
                       //join bd in dbContext.HrBranchDepartments on emp.Id equals bd.DepartmentId
                       where emp.IsActive == true
                       && emp.DeletedUserId == null
                       select new HrDepartment
                       {
                           NameAr = emp.NameAr,
                           Id=emp.Id
                           //Id = bd.Id
                       }).Distinct().OrderBy(a => a.NameAr).ToList();

            return obj;
        }
        public static IList<HrBranch> ListHrBranch()
        {
            using var dbContext = new AppDbContext();

            var obj = (from emp in dbContext.HrBranches
                           //join bd in dbContext.HrBranchDepartments on emp.Id equals bd.DepartmentId
                       where emp.IsActive == true
                       && emp.DeletedUserId == null
                       select new HrBranch
                       {
                           NameAr = emp.NameAr,
                           Id=emp.Id
                           //Id = bd.Id
                       }).Distinct().OrderBy(a => a.NameAr).ToList();

            return obj;
        }
        public static IList<EmployeeType> ListHrEmployeesTypes()
        {
            using var dbContext = new AppDbContext();

            var obj = (from emp in dbContext.EmployeeTypes
                           //join bd in dbContext.HrBranchDepartments on emp.Id equals bd.DepartmentId
                      
                       select new EmployeeType
                       {
                           EmployeeTypeNameAr = emp.EmployeeTypeNameAr,
                           Id=emp.Id
                           //Id = bd.Id
                       }).Distinct().OrderBy(a => a.EmployeeTypeNameAr).ToList();

            return obj;
        }
        //public static IList<HrFunctionalDegree> ListHrFunctionalDegrees()
        //{
        //    using var dbContext = new AppDbContext();

        //    var obj = (from emp in dbContext.HrFunctionalDegrees
        //               where emp.IsActive == true
        //               && emp.DeletedUserId == null
        //               select new HrFunctionalDegree
        //               {
        //                   NameAr = emp.NameAr,
        //                   Id = emp.Id
        //               }).Distinct().OrderBy(a => a.NameAr).ToList();

        //    return obj;
        //}
        public static IList<HrJob> ListHrJobs()
        {
            using var dbContext = new AppDbContext();

            var obj = (from emp in dbContext.HrJobs
                       where emp.IsActive == true
                       && emp.DeletedUserId == null
                       select new HrJob
                       {
                           TitleAr = emp.TitleAr,
                           Id = emp.Id
                       }).Distinct().OrderBy(a => a.TitleAr).ToList();

            return obj;
        }
    }
}
