using EF.Data;
using EF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using zkteco_attendance_api;

namespace ERPNchr.BackgroundTasks
{
    public class AttendanceSyncService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AttendanceSyncService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {

        //        var runTime = DateTime.UtcNow;
        //        int totalNewLogs = 0;

        //        var result = new AttendanceDeviceLogResult();
        //        var devices = await _context.BiometricDevices
        //            .Where(d => d.IsActive && !d.IsDeleted)
        //            .ToListAsync();

        //        var newLogs = new List<AttendanceLogDTO>();
        //        var failedDevices = new List<string>();
        //        var successfulDevices = new List<string>();

        //        foreach (var device in devices)
        //        {
        //            try
        //            {
        //                var zk = new ZkTeco(device.IpAddress);
        //                if (!zk.Connect())
        //                {
        //                    Console.WriteLine($"❌ Failed to connect to {device.IpAddress}");
        //                    failedDevices.Add(device.IpAddress);
        //                    continue;
        //                }

        //                successfulDevices.Add(device.IpAddress);

        //                var latestScanTime = await _context.AttendanceDeviceLogs
        //                    .Where(l => l.DeviceId == device.Id)
        //                    .MaxAsync(l => (DateTime?)l.ScanTime) ?? DateTime.MinValue;

        //                var attendances = zk.GetAttendance();

        //                foreach (var att in attendances)
        //                {
        //                    var scanTime = att.Timestamp.Kind == DateTimeKind.Unspecified
        //                        ? DateTime.SpecifyKind(att.Timestamp, DateTimeKind.Utc)
        //                        : att.Timestamp.ToUniversalTime();

        //                    if (scanTime <= latestScanTime)
        //                        continue;

        //                    bool alreadyExists = await _context.AttendanceDeviceLogs.AnyAsync(l =>
        //                        l.DeviceId == device.Id &&
        //                        l.EnrollNumber == att.UserId &&
        //                        l.ScanTime == scanTime);

        //                    if (alreadyExists)
        //                        continue;

        //                    var dto = new AttendanceLogDTO
        //                    {
        //                        DeviceId = device.Id,
        //                        Device = device,
        //                        EnrollNumber = att.UserId,
        //                        ScanTime = scanTime,
        //                        ScanType = att.Status.ToString(),
        //                        Verified = true,
        //                        Source = "Biometric",
        //                    };

        //                    newLogs.Add(dto);

        //                    _context.AttendanceDeviceLogs.Add(new AttendanceDeviceLog
        //                    {
        //                        DeviceId = dto.DeviceId,
        //                        EnrollNumber = dto.EnrollNumber,
        //                        ScanTime = scanTime,
        //                        ScanType = dto.ScanType,
        //                        Verified = dto.Verified,
        //                        Source = dto.Source,
        //                        CreatedAt = DateTime.UtcNow
        //                    });
        //                }

        //                zk.Disconnect();
        //            }
        //            catch (Exception ex)
        //            {
        //                failedDevices.Add(device.IpAddress);
        //                Console.WriteLine($"❌ Error while syncing device {device.IpAddress}: {ex.Message}");
        //            }
        //        }

        //        if (newLogs.Count > 0)
        //            await _context.SaveChangesAsync();

        //        result.Success = true;
        //        result.Log = newLogs.FirstOrDefault();
        //        result.SuccessMessage = $"✅ Synced {newLogs.Count} new logs from {successfulDevices.Count} device(s).";

        //        if (failedDevices.Any())
        //        {
        //            result.ErrorMessage = $"⚠️ Failed to connect to {failedDevices.Count} device(s): {string.Join(", ", failedDevices)}";
        //        }

        //        await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
        //    }
        //}
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var runTime = DateTime.UtcNow;
                int totalNewLogs = 0;

                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        var devices = await _context.HrMachineIps.Where(d => d.IsActive).ToListAsync();

                        foreach (var device in devices)
                        {
                            int deviceNewLogs = 0;
                            string status = "Success";
                            string message = "";

                            try
                            {
                                ZkTeco zk = null;
                                try
                                {
                                    zk = new ZkTeco(device.MachineIp);
                                    if (!zk.Connect())
                                    {
                                        status = "Failed";
                                        message = "Could not connect";
                                        throw new Exception(message);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    status = "Failed";
                                    message = $"Connection error: {ex.Message}";

                                    _context.AttendanceSyncErrorLogs.Add(new AttendanceSyncErrorLog
                                    {
                                        RunTime = runTime,
                                        DeviceIp = device.MachineIp,
                                        ErrorMessage = message
                                    });

                                    throw;
                                }

                                var logs = await _context.AttendanceDeviceLogs
                                    .Where(l => l.DeviceId == device.Id)
                                    .Select(l => l.ScanTime)
                                    .ToListAsync();

                                var latestScanTime = logs.Any() ? logs.Max() : DateTime.MinValue;
                                var attendances = zk.GetAttendance();

                                foreach (var att in attendances)
                                {
                                    try
                                    {
                                        var scanTime = att.Timestamp.Kind == DateTimeKind.Unspecified
                                        ? DateTime.SpecifyKind(att.Timestamp, DateTimeKind.Utc)
                                        : att.Timestamp.ToUniversalTime();

                                        if (scanTime <= latestScanTime)
                                            continue;

                                        bool alreadyExists = await _context.AttendanceDeviceLogs.AnyAsync(l =>
                                            l.DeviceId == device.Id &&
                                            l.EnrollNumber == att.UserId &&
                                            l.ScanTime == scanTime);

                                        if (alreadyExists)
                                            continue;

                                        _context.AttendanceDeviceLogs.Add(new EF.Models.AttendanceDeviceLog
                                        {
                                            DeviceId = device.Id,
                                            EnrollNumber = att.UserId,
                                            ScanTime = scanTime,
                                            ScanType = att.Status.ToString(),
                                            Verified = true,
                                            Source = "Biometric",
                                            CreatedAt = DateTime.UtcNow
                                        });
                                        try
                                        {
                                            var attendance = new HrEmployeeAttendance
                                            {
                                                Id = await _context.Database.ExecuteSqlRawAsync("SELECT NEXT VALUE FOR dbo.HR_Employee_Attendance_SEQ"), // Sequence
                                                EmployeeId =long.Parse( att.UserId),
                                                MachineId = device.Id,
                                                MoveCodeId =byte.Parse( att.Status.ToString()),
                                                ModeDate = DateOnly.FromDateTime( scanTime.Date),
                                                MoveTime = TimeOnly.FromDateTime(scanTime)
                                            };

                                            _context.HrEmployeeAttendances.Add(attendance);
                                            await _context.SaveChangesAsync();
                                        }
                                        catch (Exception ex)
                                        {
                                            // تجاهل أي خطأ أو سجل اللوج
                                            Console.WriteLine($"Failed to insert HR_Employee_Attendance: {ex.Message}");
                                        }
                                        deviceNewLogs++;
                                    }
                                    catch (Exception ex)
                                    {
                                        deviceNewLogs = deviceNewLogs;
                                        if (string.IsNullOrEmpty(message))
                                        {
                                            message = ex.Message;

                                            _context.AttendanceSyncErrorLogs.Add(new AttendanceSyncErrorLog
                                            {
                                                RunTime = runTime,
                                                DeviceIp = device.MachineIp,
                                                ErrorMessage = message
                                            });
                                        }
                                        status = "Failed";
                                    }

                                }

                                totalNewLogs += deviceNewLogs;
                                zk.Disconnect();
                                message = $"Synced {deviceNewLogs} new logs";
                            }
                            catch (Exception ex)
                            {
                                if (string.IsNullOrEmpty(message))
                                {
                                    message = ex.Message;

                                    _context.AttendanceSyncErrorLogs.Add(new AttendanceSyncErrorLog
                                    {
                                        RunTime = runTime,
                                        DeviceIp = device.MachineIp,
                                        ErrorMessage = message
                                    });
                                }
                                status = "Failed";
                            }

                            // سجل كل جهاز
                            _context.AttendanceSyncDeviceLogs.Add(new AttendanceSyncDeviceLog
                            {
                                RunTime = runTime,
                                DeviceIp = device.MachineIp,
                                NewLogs = deviceNewLogs,
                                Status = status,
                                Message = message
                            });

                            Console.WriteLine($"[{DateTime.Now}] Device {device.MachineIp}: {status} - {message}");
                        }

                        await _context.SaveChangesAsync();

                        Console.WriteLine($"[{DateTime.Now}] Total new logs in this sync: {totalNewLogs}");
                    }
                }
                catch (Exception ex)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        _context.AttendanceSyncErrorLogs.Add(new AttendanceSyncErrorLog
                        {
                            RunTime = runTime,
                            DeviceIp = null,
                            ErrorMessage = ex.Message
                        });
                        await _context.SaveChangesAsync();
                    }

                    Console.WriteLine($"[{DateTime.Now}] General sync error: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
            }
        }
    }
}
