using System.ComponentModel.DataAnnotations;
using MrMohamedHassan.Models;

namespace MrMohamedHassan.ViewModels;

public class AttendanceTakeViewModel
{
    [Required]
    [Display(Name = "المجموعة")]
    public int GroupId { get; set; }

    [Required]
    [Display(Name = "التاريخ")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    public List<AttendanceItemViewModel> Students { get; set; } = new();
}

public class AttendanceItemViewModel
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentCode { get; set; } = string.Empty;
    public AttendanceStatus Status { get; set; }
    public string? Notes { get; set; }
}

public class AttendanceReportViewModel
{
    public int GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; } = DateTime.Today;
    public List<AttendanceReportItemViewModel> Records { get; set; } = new();
}

public class StudentAttendanceInfo
{
    public int StudentId { get; set; }
    public string StudentCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? GroupName { get; set; }
    public string? TodayAttendance { get; set; }
    public bool MonthlyPaid { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public decimal SubscriptionFee { get; set; }
    public int GroupId { get; set; }
}

public class RecordAttendanceRequest
{
    public int StudentId { get; set; }
    public int GroupId { get; set; }
    public string Status { get; set; } = "Absent";
}

public class AttendanceReportItemViewModel
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentCode { get; set; } = string.Empty;
    public int PresentDays { get; set; }
    public int AbsentDays { get; set; }
    public int LateDays { get; set; }
    public int TotalDays => PresentDays + AbsentDays + LateDays;
    public double AttendancePercentage => TotalDays > 0 ? Math.Round((double)(PresentDays + LateDays) / TotalDays * 100, 1) : 0;
}
