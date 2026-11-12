namespace PennForgeTech.Core.Serialization
{
    public interface ISerializer
    {
        string Serialize<TValue>(TValue value);

        TValue Deserialize<TValue>(string text);
    }
}
