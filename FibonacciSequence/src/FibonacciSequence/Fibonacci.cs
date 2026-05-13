namespace FibonacciSequence;

public static class Fibonacci
{
    public static List<long> IterativeFibonacci(int x)
    {
        if (x <= 0) return [];

        List<long> sequence = new List<long>();

        long previous = 0;
        long current = 1;

        for (int i = 0; i < x; i++)
        {
            sequence.Add(previous);

            long next = previous + current;
            previous = current;
            current = next;
        }

        return sequence;
    }

    public static List<long> YieldFibonacci(int x)
    {
        if (x <= 0) return [];

        return GenerateFibonacci().Take(x).ToList();
    }

    private static IEnumerable<long> GenerateFibonacci()
    {
        long previous = 0;
        long current = 1;

        while (true)
        {
            yield return previous;

            long next = previous + current;
            previous = current;
            current = next;
        }
    }
}
