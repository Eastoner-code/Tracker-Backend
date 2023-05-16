using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using TrackerApi.ApiModels;
using TrackerApi.Models;
using TrackerApi.Models.HelperModels;

namespace TrackerApi.Infrastructure
{
    public class ServicesMappingProfile : Profile
    {
        public ServicesMappingProfile()
        {
            #region Users

            CreateMap<IdentityAuthUser, UserApiModel>();
            CreateMap<UserApiModel, IdentityAuthUser>();

            #endregion

            #region Projects

            CreateMap<Project, ProjectApiModel>();
            CreateMap<ProjectApiModel, Project>();

            #endregion

            #region UserProjectApiModal

            CreateMap<UserProject, UserProjectApiModal>();
            CreateMap<UserProjectApiModal, UserProject>();

            #endregion

            

            #region Skill

            CreateMap<Skill, SkillApiModel>();
            CreateMap<SkillApiModel, Skill>();

            #endregion

            #region Activity

            CreateMap<Activity, ActivityApiModel>();
            CreateMap<ActivityApiModel, Activity>();

            #endregion


            #region Position

            CreateMap<Position, PositionApiModel>();
            CreateMap<PositionApiModel, Position>();

            #endregion

            #region RegisterUserModel

            CreateMap<IdentityAuthUser, RegisterUserModel>();
            CreateMap<RegisterUserModel, IdentityAuthUser>();

            #endregion

            #region Rates

            CreateMap<UserRate, UserRateApiModel>()
                .ForMember(dest => dest.Date, m => m.MapFrom(src => DateTime.SpecifyKind(src.Date, DateTimeKind.Utc)));
            CreateMap<UserRateApiModel, UserRate>();

            #endregion

            #region Holidays

            CreateMap<Holiday, HolidayApiModel>()
                .ForMember(dest => dest.Date, m => m.MapFrom(src => DateTime.SpecifyKind(src.Date, DateTimeKind.Utc)));
            CreateMap<HolidayApiModel, Holiday>()
                .ForMember(dest => dest.Date, m => m.MapFrom(src => src.Date.ToUniversalTime()));

            #endregion

            #region GeneralCustomerReport
            CreateMap<GeneralCustomerReport, GeneralCustomerReportApiModal>();
            CreateMap<GeneralCustomerReportApiModal, GeneralCustomerReport>();

            CreateMap<UserCustomerReportApiModal, UserCustomerReport>();
            CreateMap<UserCustomerReport, UserCustomerReportApiModal>();

            CreateMap<TimeCustomerReportApiModal, TimeCustomerReport>();
            CreateMap<TimeCustomerReport, TimeCustomerReportApiModal>();

            CreateMap<TimeCustomerReportForPeriodApiModal, TimeCustomerReportForPeriod>();
            CreateMap<TimeCustomerReportForPeriod, TimeCustomerReportForPeriodApiModal>();

            CreateMap<ActivityCustomerReport, ActivityCustomerReportApiModal>();
            #endregion

            #region Notifications

            CreateMap<Notification, NotificationApiModel>()
                .ForMember(dest => dest.CreatedOn, m => m.MapFrom(src => DateTime.SpecifyKind(src.CreatedOn, DateTimeKind.Utc)));
            CreateMap<NotificationApiModel, Notification>()
                .ForMember(dest => dest.CreatedOn, m => m.MapFrom(src => src.CreatedOn.ToUniversalTime()));

            #endregion

            #region Absences

            CreateMap<Absence, AbsenceApiModel>()
                .ForMember(dest => dest.StartDate, m => m.MapFrom(src => DateTime.SpecifyKind(src.StartDate, DateTimeKind.Utc)))
                .ForMember(dest => dest.EndDate, m => m.MapFrom(src => DateTime.SpecifyKind(src.EndDate, DateTimeKind.Utc)));
            CreateMap<AbsenceApiModel, Absence>()
                .ForMember(dest => dest.StartDate, m => m.MapFrom(src => src.StartDate.ToUniversalTime()))
                .ForMember(dest => dest.EndDate, m => m.MapFrom(src => src.EndDate.ToUniversalTime()));

            #endregion

            #region UserYearRanges

            CreateMap<UserYearRange, UserYearRangeApiModel>()
                .ForMember(dest => dest.From, m => m.MapFrom(src => DateTime.SpecifyKind(src.From, DateTimeKind.Utc)))
                .ForMember(dest => dest.To, m => m.MapFrom(src => DateTime.SpecifyKind(src.To, DateTimeKind.Utc)));
            CreateMap<UserYearRangeApiModel, UserYearRange>()
                .ForMember(dest => dest.From, m => m.MapFrom(src => src.From.ToUniversalTime()))
                .ForMember(dest => dest.To, m => m.MapFrom(src => src.To.ToUniversalTime()));

            #endregion

            #region Recruiting
            
            #region Vacancies

            CreateMap<Vacancy, VacancyApiModel>()
                .ForMember(dest => dest.DateOpened, m => m.MapFrom(src => DateTime.SpecifyKind(src.DateOpened, DateTimeKind.Utc)))
                .ForMember(dest => dest.DueDate, m => m.MapFrom(src => DateTime.SpecifyKind(src.DueDate, DateTimeKind.Utc)));
            CreateMap<VacancyApiModel, Vacancy>()
                .ForMember(dest => dest.DateOpened, m => m.MapFrom(src => src.DateOpened.ToUniversalTime()))
                .ForMember(dest => dest.DueDate, m => m.MapFrom(src => src.DueDate.ToUniversalTime()));

            #endregion

            #region Candidates

            CreateMap<Candidate, CandidateApiModel>()
                .ForMember(dest => dest.InterviewDate, m => m.MapFrom(src => DateTime.SpecifyKind(src.InterviewDate, DateTimeKind.Utc)));
            CreateMap<CandidateApiModel, Candidate>()
                .ForMember(dest => dest.InterviewDate, m => m.MapFrom(src => src.InterviewDate.ToUniversalTime()));

            #endregion

            #endregion

            #region InvoicePipeline

            CreateMap<InvoicePipeline, InvoicePipelineApiModel>();
            CreateMap<InvoicePipelineApiModel, InvoicePipeline>();

            #endregion

            #region PaymentDetails
            CreateMap<PaymentDetails, PaymentDetailsApiModel>();
            CreateMap<PaymentDetailsApiModel, PaymentDetails>();
            #endregion
        }
    }
}