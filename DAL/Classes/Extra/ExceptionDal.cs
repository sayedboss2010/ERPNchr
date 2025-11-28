using VM.ViewModels;

namespace DAL.Classes.Extra;

public static class ExceptionDal
{
    // This class can be used to implement data access logic for exception handling operations.
    // For example, you can add methods to log exceptions, retrieve exception details, etc.
    // Currently, it is empty and serves as a placeholder for future implementation.
    public static void LogException(ExceptionLogVm log)
    {
        try
        {
            using var _dbcontext = new AppDbContext();
            // Assuming ExceptionLogTb is a model class representing the exception log table
            _dbcontext.ExceptionLogTbs.Add(new ExceptionLogTb
            {
                UserId = log.UserId,
                Exception = log.Exception,
                ExceptionType = log.ExceptionType,
                ExceptionTime = log.ExceptionTime ?? DateTime.Now
            });

            _dbcontext.SaveChanges();
        }
        catch (Exception ex)
        {
            //ignore for now
            //insert in to log file
            var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(logDir);
            var logPath = Path.Combine(logDir, "exceptions.log");
            var logEntry = $@"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 
UserId: {log?.UserId}
Exception: {log?.Exception}
ExceptionType: {log?.ExceptionType}
ExceptionTime: {log?.ExceptionTime}
DbException: {ex}
---------------------------
";
            File.AppendAllText(logPath, logEntry);
        }
    }
}