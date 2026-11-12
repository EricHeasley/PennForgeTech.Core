using System.Collections.Generic;
using System.Linq;
using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Paging
{
    public static class PagingExtensions
    {
        public static PagedResult<TItem> ToPagedResult<TItem>(this IEnumerable<TItem> source, PageRequest request)
        {
            Guard.AgainstNull(source, nameof(source));
            Guard.AgainstNull(request, nameof(request));

            IReadOnlyList<TItem> allItems = source.ToList();
            IReadOnlyList<TItem> itemsOnPage = allItems
                .Skip(request.ItemsToSkip)
                .Take(request.PageSize)
                .ToList();

            return new PagedResult<TItem>(itemsOnPage, request, allItems.Count);
        }
    }
}
