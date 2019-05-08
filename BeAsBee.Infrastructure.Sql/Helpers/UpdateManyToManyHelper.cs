using System;
using System.Collections.Generic;
using System.Linq;
using BeAsBee.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BeAsBee.Infrastructure.Sql.Helpers {
    public static class UpdateManyToManyHelper {
        public static void UpdateManyToMany<TDependentEntity, TKey> (
            this DbContext db,
            IEnumerable<TDependentEntity> dbEntries,
            IEnumerable<TDependentEntity> updatedEntries,
            Func<TDependentEntity, TKey> keyRetrievalFunction )
            where TDependentEntity : class {
            var oldItems = dbEntries.ToList();
            var newItems = updatedEntries.ToList();
            var toBeRemoved = oldItems.Except( newItems, keyRetrievalFunction );
            var toBeAdded = newItems.Except( oldItems, keyRetrievalFunction );

            db.Set<TDependentEntity>().RemoveRange( toBeRemoved );
            db.Set<TDependentEntity>().AddRange( toBeAdded );
        }
    }
}