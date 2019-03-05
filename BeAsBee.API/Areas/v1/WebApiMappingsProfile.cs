using AutoMapper;
using BeAsBee.API.Areas.v1.Models.Chat;
using BeAsBee.API.Areas.v1.Models.Message;
using BeAsBee.API.Areas.v1.Models.User;
using BeAsBee.Domain.Entities;

namespace BeAsBee.API.Areas.v1 {
    public class WebApiMappingsProfile : Profile {
        public WebApiMappingsProfile () {
            #region View -> Domain

            #region CreateBinding -> Domain

            CreateMap<CreateMessageBindingModel, MessageEntity>();
            CreateMap<CreateUserBindingModel, UserEntity>();
            CreateMap<CreateChatBindingModel, ChatEntity>();

            #endregion

            #endregion

            #region Domain -> View

            #region Domain -> Model

            CreateMap<MessageEntity, MessageViewModel>();
            CreateMap<UserEntity, UserPageBindingModel>();
            CreateMap<ChatEntity, ChatListBindingModel>();
            CreateMap<ChatEntity, ChatViewModel>();

            #endregion

            #endregion

            // #region UpdateBinding -> Domain

            // CreateMap<UpdateCaseStudyBindingModel, CaseStudyEntity>()
            //     .ForMember( dto => dto.ImageEntities, opt => opt.MapFrom( src => src.Images ) )
            //     .ForMember( dto => dto.ExpertiseEntities, opt => opt.MapFrom( src => src.Expertises.Select( e => new ExpertiseEntity {Id = e} ).ToList() ) )
            //     .ForMember( dto => dto.IndustryEntities, opt => opt.MapFrom( src => src.Industries.Select( e => new IndustryEntity {Id = e} ).ToList() ) )
            //     .ForMember( dto => dto.ServiceEntities, opt => opt.MapFrom( src => src.Services.Select( e => new ServiceEntity {Id = e} ).ToList() ) )
            //     .ForMember( dto => dto.TechnologyEntities, opt => opt.MapFrom( src => src.Technologies.Select( e => new TechnologyEntity {Id = e} ).ToList() ) );
            // CreateMap<UpdateVacancyBindingModel, VacancyEntity>()
            //     .ForMember( dto => dto.JobAreaEntities, opt => opt.MapFrom( src => src.JobAreas.Select( jobAreaId => new JobAreaEntity {Id = jobAreaId} ).ToList() ) )
            //     .ForMember( dto => dto.LocationEntities, opt => opt.MapFrom( src => src.Locations.Select( locationId => new LocationEntity {Id = locationId} ).ToList() ) );
            // CreateMap<UpdateImageBindingModel<int>, ImageEntity<int>>();
            // CreateMap<UpdateLocationBindingModel, LocationEntity>();
            // CreateMap<UpdateJobAreaBindingModel, JobAreaEntity>();
            // CreateMap<UpdateExpertiseBindingModel, ExpertiseEntity>();
            // CreateMap<UpdateTechnologyBindingModel, TechnologyEntity>();
            // CreateMap<UpdateServiceBindingModel, ServiceEntity>();
            // CreateMap<UpdateIndustryBindingModel, IndustryEntity>();

            // CreateMap<CreateFileBindingModel, FileEntity>()
            //     .ForMember(f => f.File, o => o.MapFrom(s => s.AttachedFile))
            //     .ForMember(f => f.Name, o => o.MapFrom(s => s.AttachedFile.FileName));

            // #endregion

            // #region ViewModel -> Domain

            // CreateMap<ContactViewModel, ContactEntity>();
            // CreateMap<CaseStudyViewModel, CaseStudyEntity>();
            // CreateMap<ImageViewModel<int>, ImageEntity<int>>();
            // CreateMap<LocationViewModel, LocationEntity>();
            // CreateMap<JobAreaViewModel, JobAreaEntity>();
            // CreateMap<ExpertiseViewModel, ExpertiseEntity>();
            // CreateMap<TechnologyViewModel, TechnologyEntity>();
            // CreateMap<ServiceViewModel, ServiceEntity>();
            // CreateMap<IndustryViewModel, IndustryEntity>();

            // #endregion
            // .ForMember(dto => dto.UsersChats, opt => opt.MapFrom(src => src.UserChats));
            //.ForMember(dto => dto.ExpertiseEntities, opt => opt.MapFrom(src => src.Expertises.Select(e => new ExpertiseEntity { Id = e }).ToList()))
            //.ForMember(dto => dto.IndustryEntities, opt => opt.MapFrom(src => src.Industries.Select(e => new IndustryEntity { Id = e }).ToList()))
            //.ForMember(dto => dto.ServiceEntities, opt => opt.MapFrom(src => src.Services.Select(e => new ServiceEntity { Id = e }).ToList()))
            //.ForMember(dto => dto.TechnologyEntities, opt => opt.MapFrom(src => src.Technologies.Select(e => new TechnologyEntity { Id = e }).ToList()));

            // #region Domain -> InfoModel

            // CreateMap<ServiceEntity, InfoServiceBindingModel>();
            // CreateMap<IndustryEntity, InfoIndustryBindingModel>();
            // CreateMap<ExpertiseEntity, InfoExpertiseBindingModel>();
            // CreateMap<TechnologyEntity, InfoTechnologyBindingModel>();

            // #endregion

            // #region InfoModel -> Domain

            // CreateMap<InfoServiceBindingModel, ServiceEntity>();
            // CreateMap<InfoIndustryBindingModel, IndustryEntity>();
            // CreateMap<InfoExpertiseBindingModel, ExpertiseEntity>();
            // CreateMap<InfoTechnologyBindingModel, TechnologyEntity>();
            // CreateMap<InfoFileBindingModel, FileEntity>();

            // #endregion
        }
    }
}