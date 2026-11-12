using System;
using System.Collections.Generic;
using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<TItem>(this IEnumerable<TItem>? source)
        {
            if (source is null)
            {
                return true;
            }

            using (IEnumerator<TItem> enumerator = source.GetEnumerator())
            {
                return !enumerator.MoveNext();
            }
        }

        public static IEnumerable<TItem> WhereNotNull<TItem>(this IEnumerable<TItem?> source)
            where TItem : class
        {
            Guard.AgainstNull(source, nameof(source));

            foreach (TItem? item in source)
            {
                if (item is not null)
                {
                    yield return item;
                }
            }
        }

        public static void ForEach<TItem>(this IEnumerable<TItem> source, Action<TItem> action)
        {
            Guard.AgainstNull(source, nameof(source));
            Guard.AgainstNull(action, nameof(action));

            foreach (TItem item in source)
            {
                action(item);
            }
        }

        public static IEnumerable<IReadOnlyList<TItem>> InBatchesOf<TItem>(this IEnumerable<TItem> source, int batchSize)
        {
            Guard.AgainstNull(source, nameof(source));
            Guard.AgainstBelowMinimum(batchSize, 1, nameof(batchSize));

            List<TItem> currentBatch = new List<TItem>(batchSize);

            foreach (TItem item in source)
            {
                currentBatch.Add(item);

                if (currentBatch.Count == batchSize)
                {
                    yield return currentBatch;
                    currentBatch = new List<TItem>(batchSize);
                }
            }

            if (currentBatch.Count > 0)
            {
                yield return currentBatch;
            }
        }
    }
}
