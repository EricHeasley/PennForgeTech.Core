using PennForgeTech.Core.Guards;

namespace PennForgeTech.Core.Paging;

/// <summary>Describes which page of a larger result set is being requested.</summary>
public sealed record PageRequest
{
    /// <summary>The largest page size that may be requested.</summary>
    public const int MaximumPageSize = 500;

    /// <summary>Initializes a new instance of the <see cref="PageRequest"/> record.</summary>
    /// <param name="pageNumber">The one-based page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    public PageRequest(int pageNumber, int pageSize)
    {
        this.PageNumber = Guard.AgainstBelowMinimum(pageNumber, 1, nameof(pageNumber));
        this.PageSize = Guard.AgainstOutOfRange(pageSize, 1, MaximumPageSize, nameof(pageSize));
    }

    /// <summary>Gets the one-based page number.</summary>
    public int PageNumber { get; }

    /// <summary>Gets the number of items per page.</summary>
    public int PageSize { get; }

    /// <summary>Gets the number of items to skip before the first item of this page.</summary>
    public int ItemsToSkip
    {
        get { return (this.PageNumber - 1) * this.PageSize; }
    }
}
