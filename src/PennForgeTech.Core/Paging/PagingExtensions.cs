using System.Collections.Generic;
using System.Linq;
using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Paging;

/// <summary>Extension methods for slicing sequences into pages.</summary>
public static class PagingExtensions
{
    /// <summary>Returns the requested page of an in-memory sequence.</summary>
    /// <param name="source">The full sequence of items.</param>
    /// <param name="request">Which page to return.</param>
    /// <typeparam name="TItem">The type of item in the sequence.</typeparam>
    /// <returns>The requested page, including the total item count of the full sequence.</returns>
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
