namespace PennForgeTech.Core.Abstractions
{
    public interface ISerializer
    {
        string Serialize<TValue>(TValue value);

        TValue Deserialize<TValue>(string text);
    }
}
