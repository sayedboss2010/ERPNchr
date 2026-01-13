////using EF.Models;
////using Microsoft.AspNetCore.Mvc;
////using zkteco_attendance_api;

////namespace ERPNchr.Areas.Attendance.Controllers
////{
////    public class AttendanceDeviceLogAddController : Controller
////    {
////        public IActionResult Index()
////        {
////            return View();
////        }
////    }
////}



//using EF.Data;
//using EF.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using zkteco_attendance_api;


//namespace ERPNchr.BackgroundService
//{
//    public class AttendanceDeviceLogAddController 
//    {
//        //private readonly ApplicationDbContext _context;

//        //public AttendanceDeviceLogAddController(ApplicationDbContext context)
//        //{
//        //    _context = context;
//        //}
//        private readonly IServiceScopeFactory _scopeFactory;

//        public AttendanceDeviceLogAddController(IServiceScopeFactory scopeFactory)
//        {
//            _scopeFactory = scopeFactory;
//        }
//        public async Task SyncDeviceLogsAsync()
//        {
//            //var result = new AttendanceDeviceLogResult();
//            //var devices = await _context.BiometricDevices
//            //    .Where(d => d.IsActive && !d.IsDeleted)
//            //    .ToListAsync();

//            //var newLogs = new List<AttendanceLogDTO>();
//            //var failedDevices = new List<string>();
//            //var successfulDevices = new List<string>();

//            //foreach (var device in devices)
//            //{
//            try
//            {
//                var zk = new ZkTeco("62.240.102.84");
//                if (!zk.Connect())
//                {
//                    Console.WriteLine($"❌ Failed to connect to {"62.240.102.84"}");
//                    //failedDevices.Add(device.IpAddress);
//                    //continue;
//                }

//                //successfulDevices.Add(device.IpAddress);

//                //var latestScanTime = await _context.AttendanceDeviceLogs
//                //    .Where(l => l.DeviceId == device.Id)
//                //    .MaxAsync(l => (DateTime?)l.ScanTime) ?? DateTime.MinValue;

//                var attendances = zk.GetAttendance();

//                //foreach (var att in attendances)
//                //{
//                //    var scanTime = att.Timestamp.Kind == DateTimeKind.Unspecified
//                //        ? DateTime.SpecifyKind(att.Timestamp, DateTimeKind.Utc)
//                //        : att.Timestamp.ToUniversalTime();

//                //    if (scanTime <= latestScanTime)
//                //        continue;

//                //    bool alreadyExists = await _context.AttendanceDeviceLogs.AnyAsync(l =>
//                //        l.DeviceId == device.Id &&
//                //        l.EnrollNumber == att.UserId &&
//                //        l.ScanTime == scanTime);

//                //    if (alreadyExists)
//                //        continue;

//                //    var dto = new AttendanceLogDTO
//                //    {
//                //        DeviceId = device.Id,
//                //        Device = device,
//                //        EnrollNumber = att.UserId,
//                //        ScanTime = scanTime,
//                //        ScanType = att.Status.ToString(),
//                //        Verified = true,
//                //        Source = "Biometric",
//                //    };

//                //    newLogs.Add(dto);

//                //    _context.AttendanceDeviceLogs.Add(new AttendanceDeviceLog
//                //    {
//                //        DeviceId = dto.DeviceId,
//                //        EnrollNumber = dto.EnrollNumber,
//                //        ScanTime = scanTime,
//                //        ScanType = dto.ScanType,
//                //        Verified = dto.Verified,
//                //        Source = dto.Source,
//                //        CreatedAt = DateTime.UtcNow
//                //    });
//                //}

//                zk.Disconnect();
//            }
//            catch (Exception ex)
//            {
//                //failedDevices.Add(device.IpAddress);
//                //Console.WriteLine($"❌ Error while syncing device {device.IpAddress}: {ex.Message}");
//            }
//            //}

//            //if (newLogs.Count > 0)
//            //    await _context.SaveChangesAsync();

//            //result.Success = true;
//            //result.Log = newLogs.FirstOrDefault();
//            //result.SuccessMessage = $"✅ Synced {newLogs.Count} new logs from {successfulDevices.Count} device(s).";

//            //if (failedDevices.Any())
//            //{
//            //    result.ErrorMessage = $"⚠️ Failed to connect to {failedDevices.Count} device(s): {string.Join(", ", failedDevices)}";
//            //}
//            // return result;
//            //return null;
//            await Task.Delay(TimeSpan.FromMinutes(60));

//        }

//        protected override Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            throw new NotImplementedException();
//        }

//        //public enum AttendanceStatus
//        //{
//        //    CheckIn = 0,
//        //    CheckOut = 1,
//        //    BreakStart = 2,
//        //    BreakEnd = 3,
//        //    Overtime = 4,
//        //    Manual = 5,
//        //    System = 6
//        //}

//        //public async Task<AttendanceDeviceLogResult> SyncDeviceLogsByDeviceIdAsync(int deviceId)
//        //{
//        //    var result = new AttendanceDeviceLogResult();
//        //    var device = await _context.BiometricDevices
//        //        .FirstOrDefaultAsync(d => d.Id == deviceId && d.IsActive && !d.IsDeleted);

//        //    if (device == null)
//        //    {
//        //        result.Success = false;
//        //        result.ErrorMessage = $"❌ Device with ID {deviceId} not found or inactive.";
//        //        return result;
//        //    }

//        //    var newLogs = new List<AttendanceLogDTO>();

//        //    try
//        //    {
//        //        var zk = new ZkTeco(device.IpAddress);
//        //        if (!zk.Connect())
//        //        {
//        //            result.Success = false;
//        //            result.ErrorMessage = $"❌ Failed to connect to device {device.IpAddress}";
//        //            return result;
//        //        }

//        //        var latestScanTime = await _context.AttendanceDeviceLogs
//        //            .Where(l => l.DeviceId == device.Id)
//        //            .MaxAsync(l => (DateTime?)l.ScanTime) ?? DateTime.MinValue;

//        //        var attendances = zk.GetAttendance();

//        //        foreach (var att in attendances)
//        //        {
//        //            var scanTime = att.Timestamp.Kind == DateTimeKind.Unspecified
//        //                ? DateTime.SpecifyKind(att.Timestamp, DateTimeKind.Utc)
//        //                : att.Timestamp.ToUniversalTime();

//        //            if (scanTime <= latestScanTime)
//        //                continue;

//        //            bool alreadyExists = await _context.AttendanceDeviceLogs.AnyAsync(l =>
//        //                l.DeviceId == device.Id &&
//        //                l.EnrollNumber == att.UserId &&
//        //                l.ScanTime == scanTime);

//        //            if (alreadyExists)
//        //                continue;

//        //            var dto = new AttendanceLogDTO
//        //            {
//        //                DeviceId = device.Id,
//        //                Device = device,
//        //                EnrollNumber = att.UserId,
//        //                ScanTime = scanTime,
//        //                ScanType = att.Status.ToString(),
//        //                Verified = true,
//        //                Source = "Biometric",
//        //            };

//        //            newLogs.Add(dto);

//        //            _context.AttendanceDeviceLogs.Add(new AttendanceDeviceLog
//        //            {
//        //                DeviceId = dto.DeviceId,
//        //                EnrollNumber = dto.EnrollNumber,
//        //                ScanTime = scanTime,
//        //                ScanType = dto.ScanType,
//        //                Verified = dto.Verified,
//        //                Source = dto.Source,
//        //                CreatedAt = DateTime.UtcNow
//        //            });
//        //        }

//        //        zk.Disconnect();

//        //        if (newLogs.Count > 0)
//        //            await _context.SaveChangesAsync();

//        //        result.Success = true;
//        //        result.Log = newLogs.FirstOrDefault();
//        //        result.SuccessMessage = $"✅ Synced {newLogs.Count} new logs from device {device.IpAddress}";
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        result.Success = false;
//        //        result.ErrorMessage = $"❌ Error syncing device {device.IpAddress}: {ex.Message}";
//        //    }

//        //    return result;
//        //}

//    }
//}