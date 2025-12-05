using BCrypt.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DAL.Classes.Account;

public static class LoginDal
{
    // This class can be used to implement data access logic for user login operations.
    // For example, you can add methods to validate user credentials, retrieve user information, etc.
    // Currently, it is empty and serves as a placeholder for future implementation.

    public static LogInUserVm VerifyUserCredentials(UserCredentialsVm credentialsVm)
    {
        // Placeholder for user credential verification logic
        // This could involve checking against a database or other data source
        try
        {
            using var _dbContext = new AppDbContext();

            //var data = _dbContext.PrUsers.FirstOrDefault(u => u.UserName == credentialsVm.UserName
            //&& u.Password == HelperClass.HashMd5(credentialsVm.Password));

            var checkedEmalil = _dbContext.HrEmployees.FirstOrDefault(u => u.Email == credentialsVm.UserName);
            if (checkedEmalil != null)
            {
                //CheckPass
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(credentialsVm.Password);

                bool isCorrect = BCrypt.Net.BCrypt.Verify(checkedEmalil.Password, hashedPassword);
                if (isCorrect)
                {
                    // Login success
                    var user = new LogInUserVm
                    {
                        Id = checkedEmalil.Id,
                        UserName = checkedEmalil.Email,
                        UserTypeId = checkedEmalil.EmployeeTypeId,
                        FullName = checkedEmalil.NameAr,
                        BranchId = checkedEmalil.BranchId,
                        DepartmentId = checkedEmalil.DepartmentId,
                        Email = checkedEmalil.Email,
                        Sts = 1,
                        AuthKey = HelperClass.HashMd5(checkedEmalil.Id.ToString() + checkedEmalil.EmployeeTypeId.ToString())
                    };

                    //run in thread                
                    Task.Run(() => AddLoginHistory(user));

                    return user;
                }
                else
                {
                    return new LogInUserVm
                    {
                        Sts = 0 // Indicating failure
                    };
                }

                
            }
            else
            {
                return new LogInUserVm
                {
                    Sts = 0 // Indicating failure
                };
            }

            
        }
        catch (Exception ex)
        {
            ExceptionDal.LogException(new ExceptionLogVm
            {
                UserId = 0, // Assuming 0 for system-level exceptions
                Exception = $"MethodName:VerifyUserCredentials\r\nException:\r\n{ex}",
                ExceptionTime = DateTime.Now,
                ExceptionType = ex.GetType().ToString(),
            });

            return new LogInUserVm
            {
                Sts = 0 // Indicating failure
            };
        }
    }

    //******************************************************************//
    private static void AddLoginHistory(LogInUserVm user)
    {
        try
        {
            using var _dbContext = new AppDbContext();
            var loginHistory = new LogInHistoryTb
            {
                UserId =(int) user.Id,
                LogInDate = DateTime.Now,
            };

            _dbContext.LogInHistoryTbs.Add(loginHistory);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            ExceptionDal.LogException(new ExceptionLogVm
            {
                UserId = (int)user.Id, // Assuming 0 for system-level exceptions
                Exception = $"MethodName:AddLoginHistory\r\nException:\r\n{ex}",
                ExceptionTime = DateTime.Now,
                ExceptionType = ex.GetType().ToString(),
            });
        }
    }
}