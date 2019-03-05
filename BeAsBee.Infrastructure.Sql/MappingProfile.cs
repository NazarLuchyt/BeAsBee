using System.Linq;
using AutoMapper;
using BeAsBee.Domain.Entities;
using BeAsBee.Infrastructure.Sql.Models;
using BeAsBee.Infrastructure.Sql.Models.Identity;

namespace BeAsBee.Infrastructure.Sql {
    public class MappingProfile : Profile {
        public MappingProfile () {
            #region Entity -> Domain

            CreateMap<Chat, ChatEntity>()
                .ForMember( dto => dto.UserChats, opt =>
                    opt.MapFrom( c => c.UserChats.Select( userChat => userChat.User ).Select( u => new UserEntity {
                        FirstName = u.FirstName,
                        SecondName = u.SecondName,
                        Id = u.Id
                    } ).ToList() ) ).MaxDepth( 1 );
            CreateMap<User, UserEntity>()
                .ForMember( dto => dto.UserChats, opt =>
                    opt.MapFrom( c => c.UserChats.Select( userChat => userChat.Chat ).ToList() ) ).MaxDepth( 1 );
            CreateMap<Message, MessageEntity>()
                .ForMember( dto => dto.UserName, opt => opt.MapFrom( msg => msg.User.FirstName + " " + msg.User.SecondName ) );

            //CreateMap<Industry, IndustryEntity>();
            //CreateMap<Expertise, ExpertiseEntity>();
            //CreateMap<Service, ServiceEntity>();
            //CreateMap<Technology, TechnologyEntity>();
            //CreateMap<JobArea, JobAreaEntity>();
            //CreateMap<CaseStudyImage, ImageEntity<int>>()
            //    .ForMember( dto => dto.ForeignKey, opt => opt.MapFrom( c => c.CaseStudyId ) );

            #endregion

            #region Domain -> Entity

            CreateMap<UserEntity, User>()
                .ForMember( c => c.Chats,
                    opt => opt.MapFrom( dto => dto.Chats.Select( s => new Chat {Id = s.Id} ) ) )
                .ForMember( c => c.UserChats,
                    opt => opt.MapFrom( dto => dto.UserChats.Select( s => new UserChat {ChatId = s.Id} ) ) );
            CreateMap<MessageEntity, Message>();
            CreateMap<ChatEntity, Chat>()
                .ForMember( c => c.UserChats,
                    opt => opt.MapFrom( dto => dto.UserChats.Select( user => new UserChat {UserId = user.Id} ) ) );

            //CreateMap<ContactEntity, Contact>()
            //     .ForMember(x => x.AttachedFiles, o => o.MapFrom(a => a.AttachedFiles));
            //CreateMap<FileEntity, File>();            
            //CreateMap<LocationEntity, Location>();
            //CreateMap<CaseStudyEntity, CaseStudy>()
            //    .ForMember( c => c.CaseStudyServices,
            //        opt => opt.MapFrom( dto => dto.ServiceEntities.Select( s => new CaseStudyService {ServiceId = s.Id} ) ) )
            //    .ForMember( c => c.CaseStudyExpertises,
            //        opt => opt.MapFrom( dto => dto.ExpertiseEntities.Select( e => new CaseStudyExpertise {ExpertiseId = e.Id} ) ) )
            //    .ForMember( c => c.CaseStudyIndustries,
            //        opt => opt.MapFrom( dto => dto.IndustryEntities.Select( i => new CaseStudyIndustry {IndustryId = i.Id} ) ) )
            //    .ForMember( c => c.CaseStudyTechnologies,
            //        opt => opt.MapFrom( dto => dto.TechnologyEntities.Select( t => new CaseStudyTechnology {TechnologyId = t.Id} ) ) )
            //    .ForMember( c => c.Images, opt => opt.MapFrom( dto => dto.ImageEntities ) );
            //CreateMap<VacancyEntity, Vacancy>()
            //    .ForMember( v => v.VacancyJobAreas,
            //        opt => opt.MapFrom( dto => dto.JobAreaEntities.Select( j => new VacancyJobArea {JobAreaId = j.Id} ) ) )
            //    .ForMember( v => v.VacancyLocations,
            //        opt => opt.MapFrom( dto => dto.LocationEntities.Select( l => new VacancyLocation {LocationId = l.Id} ) ) );
            //CreateMap<ExpertiseEntity, Expertise>();
            //CreateMap<JobAreaEntity, JobArea>();
            //CreateMap<IndustryEntity, Industry>();
            //CreateMap<ServiceEntity, Service>();
            //CreateMap<TechnologyEntity, Technology>();
            //CreateMap<ImageEntity<int>, CaseStudyImage>()
            //    .ForMember( i => i.CaseStudyId, opt => opt.MapFrom( i => i.ForeignKey ) );

            #endregion
        }
    }
}