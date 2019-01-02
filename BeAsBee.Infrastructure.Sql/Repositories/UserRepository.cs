using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BeAsBee.Domain.Entities;
using BeAsBee.Infrastructure.Repositories;
using BeAsBee.Infrastructure.Sql.Common;
using BeAsBee.Infrastructure.Sql.Models;
using BeAsBee.Infrastructure.Sql.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace BeAsBee.Infrastructure.Sql.Repositories {
    public class UserRepository : GenericRepository<UserEntity, User, Guid>, IUserRepository {
        private readonly ApplicationContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository ( ApplicationContext context ) : base( context ) {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        public async Task<UserEntity> GetByEmail ( string email ) {
            var result = await _dbSet.Where( user => user.Email == email ).FirstOrDefaultAsync();

            return Mapper.Map<UserEntity>( result );
        }

        public override async Task<UserEntity> GetByIdAsync ( Guid id ) {
            var result = await GetAsync( new List<Expression<Func<User, bool>>> {i => i.Id == id}, user => user.Include( u => u.UserChats ).ThenInclude( uChat => uChat.Chat ) ).FirstOrDefaultAsync();
            return Mapper.Map<UserEntity>( result );
        }

        public async Task<List<UserEntity>> GetPagedAsync ( int count = 10, int page = 0,
                                                            string infoToSearch = null ) {
            var result = await GetAsync( new List<Expression<Func<User, bool>>> {user => (user.FirstName + " " + user.SecondName).Contains( infoToSearch )} )
                .Skip( page * count ).Take( count )
                .ToListAsync();
            return Mapper.Map<List<UserEntity>>( result );
        }

        public async Task<int> CountAsync ( string infoToSearch ) {
            var result = await GetAsync( new List<Expression<Func<User, bool>>> {user => (user.FirstName + user.SecondName).Contains( infoToSearch )} ).CountAsync();
            return result;
        }

        //public override async Task<CaseStudyEntity> CreateWithSaveAsync ( CaseStudyEntity entity ) {
        //    var entitySql = Mapper.Map<CaseStudy>(entity);
        //    _context.AttachRange(entitySql.Images);
        //    foreach ( var entitySqlImage in entitySql.Images ) {
        //        _context.Entry(entitySqlImage).State = EntityState.Modified;
        //        _context.Entry(entitySqlImage).Property(i => i.SourceUrl).IsModified = false;
        //    }

        //    await _dbSet.AddAsync(entitySql);
        //    await _context.SaveChangesAsync();
        //    return await GetByIdAsync(entitySql.Id);
        //}

        //public override async Task UpdateAsync ( int id, CaseStudyEntity updatedEntity ) {
        //    var currentEntity = await GetAsync(new List<Expression<Func<CaseStudy, bool>>> { i => i.Id == id }, GetDefaultIncludes().ToArray()).FirstOrDefaultAsync();
        //    if ( currentEntity == null ) {
        //        throw new ItemNotFoundException(string.Format(Translations.ENTITY_WITH_ID_NOT_FOUND, Translations.CaseStudy, id));
        //    }

        //    var selectedServices = updatedEntity.ServiceEntities
        //        .Select(s => new CaseStudyService { CaseStudyId = currentEntity.Id, ServiceId = s.Id });
        //    _context.UpdateManyToMany(currentEntity.CaseStudyServices, selectedServices, c => c.ServiceId);

        //    var selectedExpertises = updatedEntity.ExpertiseEntities
        //        .Select(e => new CaseStudyExpertise { CaseStudyId = currentEntity.Id, ExpertiseId = e.Id });
        //    _context.UpdateManyToMany(currentEntity.CaseStudyExpertises, selectedExpertises, c => c.ExpertiseId);

        //    var selectedTechnologies = updatedEntity.TechnologyEntities
        //        .Select(e => new CaseStudyTechnology { CaseStudyId = currentEntity.Id, TechnologyId = e.Id });
        //    _context.UpdateManyToMany(currentEntity.CaseStudyTechnologies, selectedTechnologies, c => c.TechnologyId);

        //    var selectedIndustries = updatedEntity.IndustryEntities
        //        .Select(i => new CaseStudyIndustry { CaseStudyId = currentEntity.Id, IndustryId = i.Id });
        //    _context.UpdateManyToMany(currentEntity.CaseStudyIndustries, selectedIndustries, c => c.IndustryId);

        //    var updatedModel = Mapper.Map<CaseStudy>(updatedEntity);

        //    _context.Entry(currentEntity).CurrentValues.SetValues(updatedModel);
        //}

        //private static List<Expression<Func<CaseStudy, bool>>> GetFilters ( List<string> filterExpertises = null,
        //                                                                    List<string> filterServices = null,
        //                                                                    List<string> filterTechnologies = null,
        //                                                                    List<string> filterIndustries = null ) {
        //    var result = new List<Expression<Func<CaseStudy, bool>>>();

        //    if ( filterExpertises != null && filterExpertises.Any() ) {
        //        result.Add(filterExpertises
        //            .Select(expertise => (Expression<Func<CaseStudy, bool>>) (i => i.CaseStudyExpertises.Any(c => c.Expertise.Name == expertise)))
        //            .Aggregate(ExpressionExtensions.Or));
        //    }

        //    if ( filterTechnologies != null && filterTechnologies.Any() ) {
        //        result.Add(filterTechnologies
        //            .Select(technology => (Expression<Func<CaseStudy, bool>>) (i => i.CaseStudyTechnologies.Any(c => c.Technology.Name == technology)))
        //            .Aggregate(ExpressionExtensions.Or));
        //    }

        //    if ( filterIndustries != null && filterIndustries.Any() ) {
        //        result.Add(filterIndustries
        //            .Select(industry => (Expression<Func<CaseStudy, bool>>) (i => i.CaseStudyIndustries.Any(c => c.Industry.Name == industry)))
        //            .Aggregate(ExpressionExtensions.Or));
        //    }

        //    if ( filterServices != null && filterServices.Any() ) {
        //        result.Add(filterServices
        //            .Select(service => (Expression<Func<CaseStudy, bool>>) (i => i.CaseStudyServices.Any(c => c.Service.Name == service)))
        //            .Aggregate(ExpressionExtensions.Or));
        //    }

        //    return result;
        //}

        //private static List<Expression<Func<IQueryable<User>, IIncludableQueryable<User, object>>>> GetDefaultIncludes ( params Expression<Func<IQueryable<UserEntity>, IIncludableQueryable<UserEntity, object>>>[] includes ) {
        //    var result = new List<Expression<Func<IQueryable<User>, IIncludableQueryable<User, object>>>> {
        //        entity => entity.Include( c => c.CaseStudyExpertises ).ThenInclude( e => e.Expertise ),
        //        entity => entity.Include( c => c.CaseStudyIndustries ).ThenInclude( i => i.Industry ),
        //        entity => entity.Include( c => c.CaseStudyServices ).ThenInclude( s => s.Service ),
        //        entity => entity.Include( c => c.CaseStudyTechnologies ).ThenInclude( t => t.Technology ),
        //        entity => entity.Include( c => c.Images )
        //    };
        //    if ( !includes.IsNullOrEmpty() ) {
        //        result.AddRange(Mapper.Map<List<Expression<Func<IQueryable<User>, IIncludableQueryable<User, object>>>>>(includes));
        //    }

        //    return result;
        //}
    }
}