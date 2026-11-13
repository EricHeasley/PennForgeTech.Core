using System;
using PennForgeTech.Core.Configuration;
using PennForgeTech.Core.Exceptions;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class ConfigurationAndExceptionTests
    {
        [Fact]
        public void Require_ReturnsTheValueWhenPresent()
        {
            Assert.Equal("server=local", RequiredSetting.Require("server=local", "ConnectionString"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("  ")]
        public void Require_ThrowsWithTheSettingNameWhenMissing(string? value)
        {
            PennForgeException exception = Assert.Throws<PennForgeException>(() => RequiredSetting.Require(value, "ConnectionString"));

            Assert.Equal(RequiredSetting.MissingCode, exception.Code);
            Assert.Contains("ConnectionString", exception.Message);
        }

        [Fact]
        public void PennForgeException_RequiresACode()
        {
            Assert.Throws<ArgumentException>(() => new PennForgeException(" ", "message"));
        }
    }
}
