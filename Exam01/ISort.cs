namespace Exam01;

public interface ISort
{
    void SortNumber(int[] values);
}

public class Sort : ISort
{
    public void SortNumber(int[] values)
    {
        if (values is null || values.Length <= 1)
        {
            return;
        }

        int length = values.Length;
        for (int i = 0; i < length - 1; i++)
        {
            for (int j = 0; j < length - i - 1; j++)
            {
                if (values[j] > values[j + 1])
                {
                    int temp = values[j];
                    values[j] = values[j + 1];
                    values[j + 1] = temp;
                }
            }
        }
    }
}


