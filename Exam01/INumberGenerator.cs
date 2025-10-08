namespace Exam01;

public interface INumberGenerator
{
    /// <summary>
    /// Generates a sequence of random integers.
    /// </summary>
    /// <returns>Sequence of random integers.</returns>
    IEnumerable<int> Random();
}

public class NumberGenerator : INumberGenerator
{
    private readonly Random _random;
    public NumberGenerator()
        => _random = new Random();

    public static INumberGenerator Create()
        => new NumberGenerator();

    /// <summary>
    /// Generates a sequence of random integers.
    /// </summary>
    /// <returns>Sequence of random integers.</returns>
    public IEnumerable<int> Random()
    {
        var range = _random.Next(1, 7);
        while (range-- > 0)
        {
            yield return _random.Next(1, 100);
        }
    }
}