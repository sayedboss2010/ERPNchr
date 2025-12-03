using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.ViewModels;

namespace DAL.Classes.Employee
{
    public class EmployeeDataDal
    {
        public static EmployeeVM Find(long EmployeeID)
        {
            using var dbContext = new AppDbContext();
            var obj = dbContext.HrEmployees.Where(a => a.Id == EmployeeID && a.IsActive == true)
                .Select(f => new EmployeeVM
                {
                    Id = f.Id,
                    EmpCode = f.EmpCode,
                    NameAr = f.NameAr,
                    NameEn = f.NameEn,
                    AddressAr = f.AddressAr,
                    AddressEn = f.AddressEn,
                    PhoneNumber = f.PhoneNumber,
                    Email = f.Email,
                    Birthdate = f.Birthdate,
                    HireDate = f.HireDate,
                    CurrentJobId = f.CurrentJobId,
                  
                    DepartmentId = f.DepartmentId,
                    
                    CurrentSalary = f.CurrentSalary,
                    CurrentFunctionalDegreeId = f.CurrentFunctionalDegreeId,
                    EmployeeTypeId = f.EmployeeTypeId,
                    IsActive = f.IsActive,
                    CreatedUserId = f.CreatedUserId,
                    //CreatedDate =f.CreatedDate,
                    UpdatedUserId = f.UpdatedUserId,
                    UpdatedDate = f.UpdatedDate,
                    DeletedUserId = f.DeletedUserId,
                    DeletedDate = f.DeletedDate,
                    InsuranceNumber = f.InsuranceNumber,
                    Nid = f.Nid,
                    DateIn = f.DateIn,
                    DateOut = f.DateOut,
                    NidPath = f.NidPath,
                    Mobile = f.Mobile,
                    EmpCodeNew = f.EmpCodeNew,
                    Isbank = f.Isbank,
                    AppointmentDate = f.AppointmentDate,
                }).FirstOrDefault();

            if (obj == null)
                return new EmployeeVM();

            return obj;
        }

        //public static IList<ColorVM> list(int userId)
        //{
        //    try
        //    {
        //        using var dbContext = new AppDbContext();

        //        var ColorList = dbContext.ColorTbs.Where(a => a.IsActive == true).Select(i => new ColorVM
        //        {
        //            Id = i.Id,
        //            ColorNo = i.ColorNo,
        //            ColorName = i.ColorName,
        //            CategoryId = i.CategoryId,
        //            CategoryName = i.Category.CategoryName,

        //        }).ToList();
        //        return ColorList;
        //    }
        //    catch (Exception ex)
        //    {

        //        ExceptionDal.LogException(new ExceptionLogVm
        //        {
        //            UserId = userId,
        //            Exception = $"MethodName:ColorDal-list\r\nException:\r\n{ex}",
        //            ExceptionTime = DateTime.Now,
        //            ExceptionType = ex.GetType().ToString(),
        //        });

        //        return [];
        //    }
        //}
        //public static long Add(ColorVM entity)
        //{
        //    try
        //    {
        //        using var dbContext = new AppDbContext();

        //        ColorTb com = new()
        //        {
        //            ColorNo = entity.ColorNo,
        //            ColorName = entity.ColorName,
        //            CategoryId = entity.CategoryId,
        //            IsActive = true,

        //        };

        //        dbContext.ColorTbs.Add(com);
        //        dbContext.SaveChanges();


        //        // Log the update asynchronously
        //        Task.Run(() =>
        //        {
        //            UpdatesLogDal.LogUpdate(new DataUpdatesLogVm
        //            {
        //                UserId = entity.UserId,
        //                Operation = "Add",
        //                TableName = "ColorTb",
        //                RawId = com.Id,
        //                UpdateTime = DateTime.Now,
        //                Description = $"Added new ColorTb: {com.ColorName} ({com.ColorNo})"
        //            });
        //        });

        //        return com.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionDal.LogException(new ExceptionLogVm
        //        {
        //            UserId = entity.UserId, // Assuming 0 for system-level exceptions
        //            Exception = $"MethodName:ColorTbDal-Add\r\nException:\r\n{ex}",
        //            ExceptionTime = DateTime.Now,
        //            ExceptionType = ex.GetType().ToString(),
        //        });

        //        return 0;
        //    }
        //}

        //public static bool CheckExist(ColorVM entity)
        //{
        //    using var dbContext = new AppDbContext();

        //    var exist = false;
        //    if (entity.Id == 0)
        //    {
        //        exist = dbContext.ColorTbs
        //            .Any(u => u.ColorName == entity.ColorName && u.ColorNo == entity.ColorNo && u.IsActive == true);
        //    }
        //    else
        //    {
        //        exist = dbContext.ColorTbs
        //            .Any(u => u.ColorName == entity.ColorName && u.ColorNo == entity.ColorNo && u.IsActive == true && u.Id != entity.Id);
        //    }

        //    return !exist;
        //}

        //public static bool Delete(int id, int UserId)
        //{
        //    using var dbContext = new AppDbContext();

        //    var obj = dbContext.ColorTbs.Find(id);

        //    if (obj != null)
        //    {
        //        obj.IsActive = false;
        //        //dbContext.ColorTbs.Remove(obj);
        //        dbContext.SaveChanges();

        //        // Log the update asynchronously
        //        Task.Run(() =>
        //        {
        //            UpdatesLogDal.LogUpdate(new DataUpdatesLogVm
        //            {
        //                UserId = UserId,
        //                Operation = "delete",
        //                TableName = "ColorTb",
        //                RawId = id,
        //                UpdateTime = DateTime.Now,
        //                Description = $"delete new ColorTb: {obj.ColorName} ({obj.ColorNo})"
        //            });
        //        });


        //        return true;
        //    }

        //    return false;
        //}


        //public static IList<ColorVM> List()
        //{
        //    using var dbContext = new AppDbContext();

        //    var obj = dbContext.ColorTbs.Where(a => a.IsActive == true).Select(f => new ColorVM
        //    {
        //        Id = f.Id,
        //        ColorName = f.ColorName,
        //        ColorNo = f.ColorNo,
        //        CategoryId = f.CategoryId,
        //        CategoryName = f.Category.CategoryName,

        //    }).ToList();

        //    return obj;
        //}

        //public static IList<ColorVM> Search(string term)
        //{


        //    using var dbContext = new AppDbContext();
        //    var obj = dbContext.ColorTbs.Where(f => (f.ColorName.Contains(term) || f.ColorNo.Contains(term)) && f.IsActive == true).Select(f => new ColorVM
        //    {
        //        Id = f.Id,
        //        ColorName = f.ColorName,
        //        ColorNo = f.ColorNo,
        //        CategoryId = f.CategoryId,
        //        CategoryName = f.Category.CategoryName,

        //    }).ToList();
        //    return obj;
        //}

        //public static bool Update(ColorVM entity)
        //{
        //    try
        //    {
        //        using var dbContext = new AppDbContext();
        //        var obj = dbContext.ColorTbs.Find(entity.Id);

        //        if (obj != null)
        //        {
        //            obj.ColorName = entity.ColorName;
        //            obj.ColorNo = entity.ColorNo;
        //            obj.CategoryId = entity.CategoryId;
        //            dbContext.SaveChanges();
        //        }
        //        {
        //            UpdatesLogDal.LogUpdate(new DataUpdatesLogVm
        //            {
        //                UserId = entity.UserId,
        //                Operation = "edit",
        //                TableName = "ColorTb",
        //                RawId = entity.Id,
        //                UpdateTime = DateTime.Now,
        //                Description = $"edit new ColorTb: {entity.ColorName} ({entity.ColorNo})"
        //            });

        //            return true;
        //        }

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionDal.LogException(new ExceptionLogVm
        //        {
        //            UserId = entity.UserId, // Assuming 0 for system-level exceptions
        //            Exception = $"MethodName:ColorTbDal-Update\r\nException:\r\n{ex}",
        //            ExceptionTime = DateTime.Now,
        //            ExceptionType = ex.GetType().ToString(),
        //        });

        //        return false;
        //    }
        //}

        //public static PaginatedList<ColorVM> GetListWithPage(int CurrentPage, int pageSize, string searchStr)
        //{
        //    using var dbContext = new AppDbContext();


        //    if (searchStr == "")
        //    {

        //        var obj = dbContext.ColorTbs.Where(a => a.IsActive == true).OrderBy(b => b.Id).Select(f => new ColorVM
        //        {
        //            Id = f.Id,
        //            ColorName = f.ColorName,
        //            ColorNo = f.ColorNo,
        //            CategoryId = f.CategoryId,
        //            CategoryName = f.Category.CategoryName,

        //        }).Skip((CurrentPage - 1) * pageSize)
        //      .Take(pageSize)
        //      .ToList();





        //        var TotalResults = dbContext.ColorTbs.Where(a => a.IsActive == true).Count();
        //        var TotalPages = (int)Math.Ceiling(TotalResults / (double)pageSize);

        //        return new PaginatedList<ColorVM>(obj, CurrentPage, TotalPages, TotalResults);

        //    }
        //    else
        //    {
        //        var obj = dbContext.ColorTbs.Where(f => (f.ColorName.Contains(searchStr) || f.ColorNo.Contains(searchStr)) && f.IsActive == true).OrderBy(b => b.Id).Select(f => new ColorVM
        //        {
        //            Id = f.Id,
        //            ColorName = f.ColorName,
        //            ColorNo = f.ColorNo,
        //            CategoryId = f.CategoryId,
        //            CategoryName = f.Category.CategoryName,

        //        }).Skip((CurrentPage - 1) * pageSize)
        //    .Take(pageSize)
        //    .ToList();





        //        var TotalResults = dbContext.ColorTbs.Where(f => (f.ColorName.Contains(searchStr) || f.ColorNo.Contains(searchStr)) && f.IsActive == true).Count();
        //        var TotalPages = (int)Math.Ceiling(TotalResults / (double)pageSize);

        //        return new PaginatedList<ColorVM>(obj, CurrentPage, TotalPages, TotalResults);
        //    }


        //}

    }
}
