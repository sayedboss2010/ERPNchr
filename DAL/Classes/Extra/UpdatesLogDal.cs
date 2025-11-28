using VM.ViewModels;

namespace DAL.Classes.Extra;

public static class UpdatesLogDal
{
    public static void LogUpdate(DataUpdatesLogVm logVm)
    {
        try
        {
            using var _dbcontext = new AppDbContext();
            // Assuming UpdatesLogTb is a model class representing the updates log table
            _dbcontext.DataUpdatesLogTbs.Add(new DataUpdatesLogTb
            {
                Operation = logVm.Operation,
                TableName = logVm.TableName,
                RawId = logVm.RawId,
                UserId = logVm.UserId,
                UpdateTime = logVm.UpdateTime,
                Description = logVm.Description
            });

            // Save changes to the database
            _dbcontext.SaveChanges();
        }
        catch (Exception ex)
        {
            ExceptionDal.LogException(new ExceptionLogVm
            {
                UserId = logVm.UserId, // Assuming 0 for system-level exceptions
                Exception = $"MethodName:LogUpdate\r\nException:\r\n{ex}",
                ExceptionTime = DateTime.Now,
                ExceptionType = ex.GetType().ToString(),
            });
        }
    }
}
