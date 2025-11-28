using VM.ViewModels;

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

            var data = _dbContext.PrUsers.FirstOrDefault(u => u.UserName == credentialsVm.UserName
            && u.Password == HelperClass.HashMd5(credentialsVm.Password));

            if (data != null && !string.IsNullOrEmpty(data.UserName))
            {
                var user = new LogInUserVm
                {
                    Id = data.Id,
                    UserName = data.UserName,
                    UserTypeId = data.UserTypeId,
                    FullName = data.FullName,
                    //Email = data.Email,
                    Sts = 1,
                    AuthKey = HelperClass.HashMd5(data.Id.ToString() + data.UserTypeId.ToString())
                };

                //run in thread                
                Task.Run(() => AddLoginHistory(user));

                return user;
            }

            return new LogInUserVm
            {
                Sts = 0 // Indicating failure
            };
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
                UserId = user.Id,
                LogInDate = DateTime.Now,
            };

            _dbContext.LogInHistoryTbs.Add(loginHistory);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            ExceptionDal.LogException(new ExceptionLogVm
            {
                UserId = user.Id, // Assuming 0 for system-level exceptions
                Exception = $"MethodName:AddLoginHistory\r\nException:\r\n{ex}",
                ExceptionTime = DateTime.Now,
                ExceptionType = ex.GetType().ToString(),
            });
        }
    }
}