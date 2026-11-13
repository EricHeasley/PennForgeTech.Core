namespace PennForgeTech.Core.Primitives
{
    public sealed record PageRequest
    {
        public const int MaximumPageSize = 500;

        public PageRequest(int pageNumber, int pageSize)
        {
            this.PageNumber = Guard.AgainstBelowMinimum(pageNumber, 1, nameof(pageNumber));
            this.PageSize = Guard.AgainstOutOfRange(pageSize, 1, MaximumPageSize, nameof(pageSize));
        }

        public int PageNumber { get; }

        public int PageSize { get; }

        public int ItemsToSkip
        {
            get { return (this.PageNumber - 1) * this.PageSize; }
        }
    }
}
