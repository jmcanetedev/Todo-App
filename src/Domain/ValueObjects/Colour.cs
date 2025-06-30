namespace Todo_App.Domain.ValueObjects;

public class Colour : ValueObject
{
    static Colour()
    {
    }

    private Colour()
    {
    }

    private Colour(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public static Colour From(string code)
    {
        if (string.IsNullOrEmpty(code))
            return White;

        var colour = new Colour { Code = code };

        if (!SupportedColours.Contains(colour))
        {
            throw new UnsupportedColourException(code);
        }

        return colour;
    }

    public static Colour White => new("#FFFFFF", "White");

    public static Colour Red => new("#FF5733","Red");

    public static Colour Orange => new("#FFC300", "Orange");

    public static Colour Yellow => new("#FFFF66", "Yellow");

    public static Colour Green => new("#CCFF99", "Green");

    public static Colour Blue => new("#6666FF", "Blue");

    public static Colour Purple => new("#9966CC", "Purple");

    public static Colour Grey => new("#999999", "Grey");

    public string Code { get; private set; } = "#000000";
    public string Name { get; private set; } = "Black";

    public static implicit operator string(Colour colour)
    {
        return colour.ToString();
    }

    public static explicit operator Colour(string code)
    {
        return From(code);
    }

    public override string ToString()
    {
        return Code;
    }

    protected static IEnumerable<Colour> SupportedColours
    {
        get
        {
            yield return White;
            yield return Red;
            yield return Orange;
            yield return Yellow;
            yield return Green;
            yield return Blue;
            yield return Purple;
            yield return Grey;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
    public static IEnumerable<Colour> GetSupportedColors()
    {
        return SupportedColours;
    }
}
