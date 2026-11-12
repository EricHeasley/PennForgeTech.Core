using System;
using PennForgeTech.Core.Guards;
using Xunit;

namespace PennForgeTech.Core.Tests;

public sealed class GuardTests
{
    [Fact]
    public void AgainstNull_ThrowsWhenValueIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Guard.AgainstNull<string>(null, "value"));
    }

    [Fact]
    public void AgainstNull_ReturnsValueWhenNotNull()
    {
        string result = Guard.AgainstNull("text", "value");

        Assert.Equal("text", result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AgainstNullOrWhiteSpace_ThrowsForBlankText(string? text)
    {
        Assert.Throws<ArgumentException>(() => Guard.AgainstNullOrWhiteSpace(text, "text"));
    }

    [Fact]
    public void AgainstOutOfRange_ThrowsWhenAboveMaximum()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.AgainstOutOfRange(11, 1, 10, "number"));
    }

    [Fact]
    public void AgainstOutOfRange_AllowsBoundaryValues()
    {
        Assert.Equal(1, Guard.AgainstOutOfRange(1, 1, 10, "number"));
        Assert.Equal(10, Guard.AgainstOutOfRange(10, 1, 10, "number"));
    }

    [Fact]
    public void AgainstBelowMinimum_ThrowsWhenBelowMinimum()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.AgainstBelowMinimum(0, 1, "number"));
    }
}
