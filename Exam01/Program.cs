namespace Exam01;

internal class Program
{
    static void Main(string[] args)
    {
        ISpecialNumberFinder specialNumberFinder = new SpecialNumberFinder();
        double specialNumber = specialNumberFinder.FindSpecialNumber();

        Console.WriteLine("Special number: " + specialNumber);
    }
}