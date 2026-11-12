using System.Collections.Generic;
using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Utilities
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
