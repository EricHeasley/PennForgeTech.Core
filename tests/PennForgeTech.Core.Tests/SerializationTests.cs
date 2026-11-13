using PennForgeTech.Core.Serialization.Json;
using Xunit;

namespace PennForgeTech.Core.Tests
{
    public sealed class SerializationTests
    {
        private enum Level
        {
            Basic,
            Premium
        }

        private sealed class Account
        {
            public string DisplayName { get; set; } = string.Empty;

            public Level Level { get; set; }

            public string? Nickname { get; set; }
        }

        [Fact]
        public void Serialize_UsesCamelCaseStringEnumsAndSkipsNulls()
        {
            JsonTextSerializer serializer = new JsonTextSerializer();

            string text = serializer.Serialize(new Account { DisplayName = "Eric", Level = Level.Premium });

            Assert.Equal("{\"displayName\":\"Eric\",\"level\":\"Premium\"}", text);
        }

        [Fact]
        public void Deserialize_RoundTripsTheValue()
        {
            JsonTextSerializer serializer = new JsonTextSerializer();
            string text = serializer.Serialize(new Account { DisplayName = "Eric", Level = Level.Premium });

            Account account = serializer.Deserialize<Account>(text);

            Assert.Equal("Eric", account.DisplayName);
            Assert.Equal(Level.Premium, account.Level);
        }

        [Fact]
        public void Deserialize_ThrowsForInvalidJson()
        {
            JsonTextSerializer serializer = new JsonTextSerializer();

            Assert.Throws<SerializationFailedException>(() => serializer.Deserialize<Account>("{ not json"));
        }

        [Fact]
        public void Deserialize_ThrowsWhenTheJsonIsNull()
        {
            JsonTextSerializer serializer = new JsonTextSerializer();

            Assert.Throws<SerializationFailedException>(() => serializer.Deserialize<Account>("null"));
        }
    }
}
