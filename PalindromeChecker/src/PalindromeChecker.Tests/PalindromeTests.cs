namespace PalindromeChecker.Tests;

public class PalindromeTests
{
    public static IEnumerable<object[]> PalindromeData =>
    [
        ["Arara",              true],
        ["Ovo",                true],
        ["Roma me tem amor",   true],
        ["O lobo ama o bolo",  true],
        ["Girafarig",          true],
        ["Farigiraf",          true],
        ["A",                  true],
        ["",                   true],
        ["Hello",              false],
        ["Palindrome",         false],
        ["abcba",              true],
        ["abcde",              false],
    ];

    [Theory]
    [MemberData(nameof(PalindromeData))]
    public void TwoPointers_IdentifyPalindrome(string input, bool expected)
    {
        bool result = Palindrome.TwoPointers(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(PalindromeData))]
    public void UsingStack_IdentifyPalindrome(string input, bool expected)
    {
        bool result = Palindrome.UsingStack(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("ARARA")]
    [InlineData("  arara  ")]
    [InlineData("Amor Roma")]
    public void TwoPointers_SpecialCases(string input)
    {
        Assert.True(Palindrome.TwoPointers(input));
    }
}
