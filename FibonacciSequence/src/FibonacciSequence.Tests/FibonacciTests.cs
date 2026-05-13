namespace FibonacciSequence.Tests;

public class FibonacciTests
{
    public static IEnumerable<object[]> FibonacciData =>
    [
        [1, new long[] { 0 }],
        [2, new long[] { 0, 1 }],
        [3, new long[] { 0, 1, 1 }],
        [5, new long[] { 0, 1, 1, 2, 3 }],
        [7, new long[] { 0, 1, 1, 2, 3, 5, 8 }],
        [10, new long[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34 }],
    ];

    [Theory]
    [MemberData(nameof(FibonacciData))]
    public void Iterative_ShouldGenerateSequence(int x, long[] expected)
    {
        List<long> result = Fibonacci.IterativeFibonacci(x);
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(FibonacciData))]
    public void UsingYield_ShouldGenerateSequence(int x, long[] expected)
    {
        List<long> result = Fibonacci.YieldFibonacci(x);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void AllAlgorithms_ShouldReturnEmpty_WhenXIsZeroOrNegative(int x)
    {
        Assert.Empty(Fibonacci.IterativeFibonacci(x));
        Assert.Empty(Fibonacci.YieldFibonacci(x));
    }

    [Fact]
    public void AllAlgorithms_ShouldReturnCorrectCount()
    {
        int x = 15;
        Assert.Equal(x, Fibonacci.IterativeFibonacci(x).Count);
        Assert.Equal(x, Fibonacci.YieldFibonacci(x).Count);
    }
}