namespace Sportal.Application.Utils;

public static class GuidBase64
{
    public static string Encode(Guid guid)
    {
        string encoded = Convert.ToBase64String(guid.ToByteArray());
        encoded = encoded.Replace("/", "_").Replace("+", "-");
        return encoded.Substring(0, 22);
    }

    /// <summary>
    ///     Decode Guid Base
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Guid</returns>
    public static Guid Decode(string value)
    {
        value = value.Replace("_", "/").Replace("-", "+");
        byte[] buffer = Convert.FromBase64String($"{value}==");
        return new Guid(buffer);
    }
}