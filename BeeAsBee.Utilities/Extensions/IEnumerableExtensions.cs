using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable InconsistentNaming

namespace BeAsBee.Utilities.Extensions {
    public static class IEnumerableExtensions {
        public static IEnumerable<TEntity> Except<TEntity, TKey> (
            this IEnumerable<TEntity> left,
            IEnumerable<TEntity> right,
            Func<TEntity, TKey> keyRetrievalFunction ) {
            var leftSet = left.ToList();
            var rightSet = right.ToList();

            var leftSetKeys = leftSet.Select( keyRetrievalFunction );
            var rightSetKeys = rightSet.Select( keyRetrievalFunction );

            var deltaKeys = leftSetKeys.Except( rightSetKeys );
            var leftComplementRightSet = leftSet.Where( i => deltaKeys.Contains( keyRetrievalFunction.Invoke( i ) ) );
            return leftComplementRightSet;
        }

        public static IEnumerable<TEntity> Intersect<TEntity, TKey> (
            this IEnumerable<TEntity> left,
            IEnumerable<TEntity> right,
            Func<TEntity, TKey> keyRetrievalFunction ) {
            var leftSet = left.ToList();
            var rightSet = right.ToList();

            var leftSetKeys = leftSet.Select( keyRetrievalFunction );
            var rightSetKeys = rightSet.Select( keyRetrievalFunction );

            var intersectKeys = leftSetKeys.Intersect( rightSetKeys );
            var intersectionEntities = leftSet.Where( i => intersectKeys.Contains( keyRetrievalFunction.Invoke( i ) ) );
            return intersectionEntities;
        }
    }
}