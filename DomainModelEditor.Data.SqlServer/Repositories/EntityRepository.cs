﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModelEditor.Data.Abstractions;
using DomainModelEditor.Domain;
using DomainModelEditor.Shared.Dto;
using Microsoft.EntityFrameworkCore;

namespace DomainModelEditor.Data.SqlServer.Repositories
{
    public class EntityRepository :Repository<Entity>, IEntityRepository
    {
       public EntityContext EntityContext => Context as EntityContext;

       public EntityRepository(EntityContext entityContext):base(entityContext)
        {
        }
        public async Task<IEnumerable<Entity>> GetEntitiesWithAttributesAsync()
        {
            return await EntityContext.Entities.Include(x => x.Coordination).Include(x => x.Attributes)
                .ThenInclude(x => x.Attribute).ToListAsync();
        }

        public async Task<PagedList<Entity>> GetEntitiesAsync(EntitiesResourceParameters entitiesResourceParameters)
        {
            if (entitiesResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(entitiesResourceParameters));
            }

            var collection = EntityContext.Entities as IQueryable<Entity>;

            if (entitiesResourceParameters.IsPersistent != null)
            {
                collection = collection.Where(a => a.IsPersistentEntity == entitiesResourceParameters.IsPersistent);
            }

            if (!string.IsNullOrWhiteSpace(entitiesResourceParameters.SearchQuery))
            {

                var searchQuery = entitiesResourceParameters.SearchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery));
            }
            //if (!string.IsNullOrWhiteSpace(entitiesResourceParameters.OrderBy))
            //{
            //    // get property mapping dictionary
            //    var authorPropertyMappingDictionary =
            //        _propertyMappingService.GetPropertyMapping<Models.AuthorDto, Author>();

            //    collection = collection.ApplySort(authorsResourceParameters.OrderBy,
            //        authorPropertyMappingDictionary);
            //}

            return await PagedList<Entity>.CreateAsync(collection,
                entitiesResourceParameters.PageNumber,
                entitiesResourceParameters.PageSize);
        }
    }
}
