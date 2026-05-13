namespace TextNormalizer.Tests;

public class TextNormalizerTests
{
    public static IEnumerable<object[]> NormalizerData =>
    [
        ["Como é???????",            "Como é?"],
        ["Não!!!!!!!!",              "Não!"],
        ["O que???!!!!! Não acredito!!!", "O que?! Não acredito!"],
        ["Incrível!!!",              "Incrível!"],
        ["Sério??????",              "Sério?"],
        ["Como assim!!!!???",        "Como assim?!"],
        ["Tudo bem?",                "Tudo bem?"],
        ["Vai!",                     "Vai!"],
        ["Por quê??? Assim!!!",      "Por quê? Assim!"],
        ["",                         ""],
    ];

    [Theory]
    [MemberData(nameof(NormalizerData))]
    public void Iterative_ShouldNormalizeText(string input, string expected)
    {
        string result = TextNormalizerFunctions.IterativeNormalizer(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(NormalizerData))]
    public void UsingForeach_ShouldNormalizeText(string input, string expected)
    {
        string result = TextNormalizerFunctions.ForeachNormalize(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void BothAlgorithms_ShouldHandleNullOrEmpty(string input)
    {
        Assert.Equal(input, TextNormalizerFunctions.IterativeNormalizer(input));
        Assert.Equal(input, TextNormalizerFunctions.ForeachNormalize(input));
    }

    [Fact]
    public void BothAlgorithms_ShouldNotChangeTextWithoutPunctuation()
    {
        string input = "Olá, tudo bem";
        Assert.Equal(input, TextNormalizerFunctions.IterativeNormalizer(input));
        Assert.Equal(input, TextNormalizerFunctions.ForeachNormalize(input));
    }
}