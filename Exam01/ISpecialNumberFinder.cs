namespace Exam01;

public interface ISpecialNumberFinder
{
    double FindSpecialNumber();
}

public class SpecialNumberFinder : ISpecialNumberFinder
{
    private readonly INumberGenerator _numberGenerator;

    public SpecialNumberFinder(INumberGenerator numberGenerator)
    {
        _numberGenerator = numberGenerator ?? throw new ArgumentNullException(nameof(numberGenerator));
    }

    public SpecialNumberFinder() : this(NumberGenerator.Create())
    {
    }

    public double FindSpecialNumber()
    {
        IEnumerable<int> numbers = _numberGenerator.Random();

        // Mock Verify Times.Once
        // IEnumerable<int> numbers = new[] { 5, 1, 3 };

        // Preserve original numbers for display
        int[] originalNumbers = numbers.ToArray();
        Console.WriteLine("Original numbers: [" + string.Join(", ", originalNumbers) + "]");


        if (originalNumbers is null || originalNumbers.Length == 0)
        {
            Console.WriteLine("Original numbers is null or empty");
            return 0; // define as 0 for empty input to keep behavior predictable in tests
        }

        ISort sort = new Sort();
        sort.SortNumber(originalNumbers);

        int[] sortedNumbers = originalNumbers.ToArray();
        Console.WriteLine("Sorted numbers: [" + string.Join(", ", sortedNumbers) + "]");

        int length = sortedNumbers.Length;
        if (length % 2 == 1)
        {
            return sortedNumbers[length / 2];
        }

        int leftMid = sortedNumbers[(length / 2) - 1];
        int rightMid = sortedNumbers[length / 2];

        double result = (leftMid + rightMid) / 2.0;
        Console.WriteLine("Special number: " + result);
        return result;
    }
}


