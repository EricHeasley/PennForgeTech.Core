using System.Collections.Generic;
using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Paging;

/// <summary>One page of items together with the information needed to navigate the full result set.</summary>
/// <typeparam name="TItem">The type of item on the page.</typeparam>
public sealed class PagedResult<TItem>
{
    /// <summary>Initializes a new instance of the <see cref="PagedResult{TItem}"/> class.</summary>
    /// <param name="items">The items on this page.</param>
    /// <param name="request">The request that produced this page.</param>
    /// <param name="totalItemCount">The number of items across all pages.</param>
    public PagedResult(IReadOnlyList<TItem> items, PageRequest request, int totalItemCount)
    {
        this.Items = Guard.AgainstNull(items, nameof(items));
        this.Request = Guard.AgainstNull(request, nameof(request));
        this.TotalItemCount = Guard.AgainstBelowMinimum(totalItemCount, 0, nameof(totalItemCount));
    }

    /// <summary>Gets the items on this page.</summary>
    public IReadOnlyList<TItem> Items { get; }

    /// <summary>Gets the request that produced this page.</summary>
    public PageRequest Request { get; }

    /// <summary>Gets the number of items across all pages.</summary>
    public int TotalItemCount { get; }

    /// <summary>Gets the number of pages needed to show every item.</summary>
    public int TotalPageCount
    {
        get
        {
            int fullPages = this.TotalItemCount / this.Request.PageSize;
            bool hasPartialPage = this.TotalItemCount % this.Request.PageSize != 0;
            return hasPartialPage ? fullPages + 1 : fullPages;
        }
    }

    /// <summary>Gets a value indicating whether a page exists after this one.</summary>
    public bool HasNextPage
    {
        get { return this.Request.PageNumber < this.TotalPageCount; }
    }

    /// <summary>Gets a value indicating whether a page exists before this one.</summary>
    public bool HasPreviousPage
    {
        get { return this.Request.PageNumber > 1; }
    }
}
