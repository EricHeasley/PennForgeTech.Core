using System.Collections.Generic;

namespace PennForgeTech.Core.Primitives
{
    public sealed class PagedResult<TItem>
    {
        public PagedResult(IReadOnlyList<TItem> items, PageRequest request, int totalItemCount)
        {
            this.Items = Guard.AgainstNull(items, nameof(items));
            this.Request = Guard.AgainstNull(request, nameof(request));
            this.TotalItemCount = Guard.AgainstBelowMinimum(totalItemCount, 0, nameof(totalItemCount));
        }

        public IReadOnlyList<TItem> Items { get; }

        public PageRequest Request { get; }

        public int TotalItemCount { get; }

        public int TotalPageCount
        {
            get
            {
                int fullPages = this.TotalItemCount / this.Request.PageSize;
                bool hasPartialPage = this.TotalItemCount % this.Request.PageSize != 0;
                return hasPartialPage ? fullPages + 1 : fullPages;
            }
        }

        public bool HasNextPage
        {
            get { return this.Request.PageNumber < this.TotalPageCount; }
        }

        public bool HasPreviousPage
        {
            get { return this.Request.PageNumber > 1; }
        }
    }
}
