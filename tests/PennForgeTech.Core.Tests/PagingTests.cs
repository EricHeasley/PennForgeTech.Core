using System;
using System.Collections.Generic;
using System.Linq;
using PennForgeTech.Core.Primitives;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class PagingTests
    {
        [Fact]
        public void ToPagedResult_ReturnsTheRequestedSlice()
        {
            IEnumerable<int> numbers = Enumerable.Range(1, 25);
            PageRequest request = new PageRequest(pageNumber: 2, pageSize: 10);

            PagedResult<int> page = numbers.ToPagedResult(request);

            Assert.Equal(Enumerable.Range(11, 10), page.Items);
            Assert.Equal(25, page.TotalItemCount);
            Assert.Equal(3, page.TotalPageCount);
            Assert.True(page.HasNextPage);
            Assert.True(page.HasPreviousPage);
        }

        [Fact]
        public void ToPagedResult_LastPageHoldsTheRemainder()
        {
            PageRequest request = new PageRequest(pageNumber: 3, pageSize: 10);

            PagedResult<int> page = Enumerable.Range(1, 25).ToPagedResult(request);

            Assert.Equal(5, page.Items.Count);
            Assert.False(page.HasNextPage);
        }

        [Fact]
        public void PageRequest_RejectsPageNumberBelowOne()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PageRequest(0, 10));
        }

        [Fact]
        public void PageRequest_RejectsPageSizeAboveMaximum()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PageRequest(1, PageRequest.MaximumPageSize + 1));
        }
    }
}
