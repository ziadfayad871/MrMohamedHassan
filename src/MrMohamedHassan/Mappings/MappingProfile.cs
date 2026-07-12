using AutoMapper;
using MrMohamedHassan.Models;
using MrMohamedHassan.ViewModels;

namespace MrMohamedHassan.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Student, StudentListViewModel>()
            .ForMember(d => d.GroupsCount, opt => opt.MapFrom(s => s.StudentGroups.Count(sg => sg.IsActive)));

        CreateMap<Student, StudentDetailsViewModel>()
            .ForMember(d => d.Groups, opt => opt.MapFrom(s => s.StudentGroups.Select(sg => sg.Group.Name).ToList()));

        CreateMap<StudentCreateViewModel, Student>();

        CreateMap<StudentEditViewModel, Student>();

        CreateMap<Group, GroupListViewModel>()
            .ForMember(d => d.TeacherName, opt => opt.MapFrom(g => g.Teacher.FullName))
            .ForMember(d => d.EnrolledCount, opt => opt.MapFrom(g => g.StudentGroups.Count(sg => sg.IsActive)));

        CreateMap<Group, GroupDetailsViewModel>()
            .ForMember(d => d.TeacherName, opt => opt.MapFrom(g => g.Teacher.FullName))
            .ForMember(d => d.Students, opt => opt.MapFrom(g => g.StudentGroups.Where(sg => sg.IsActive).Select(sg => sg.Student)));

        CreateMap<GroupCreateViewModel, Group>();
        CreateMap<GroupEditViewModel, Group>();

        CreateMap<Payment, PaymentListViewModel>()
            .ForMember(d => d.StudentName, opt => opt.MapFrom(p => p.Student.FullName))
            .ForMember(d => d.StudentCode, opt => opt.MapFrom(p => p.Student.StudentCode));

        CreateMap<PaymentCreateViewModel, Payment>();

        CreateMap<Expense, ExpenseListViewModel>()
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(e => e.Category.Name));

        CreateMap<ExpenseCreateViewModel, Expense>();

        CreateMap<Exam, ExamListViewModel>()
            .ForMember(d => d.GroupName, opt => opt.MapFrom(e => e.Group.Name))
            .ForMember(d => d.ResultsCount, opt => opt.MapFrom(e => e.Results.Count));

        CreateMap<ExamCreateViewModel, Exam>();

        CreateMap<Homework, HomeworkListViewModel>()
            .ForMember(d => d.GroupName, opt => opt.MapFrom(h => h.Group.Name))
            .ForMember(d => d.SubmissionsCount, opt => opt.MapFrom(h => h.Submissions.Count));

        CreateMap<HomeworkCreateViewModel, Homework>();

        CreateMap<Notification, NotificationListViewModel>();

        CreateMap<NotificationCreateViewModel, Notification>();
    }
}
