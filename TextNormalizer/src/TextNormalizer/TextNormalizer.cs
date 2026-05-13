namespace TextNormalizer;

public static class TextNormalizerFunctions
{
    public static string IterativeNormalizer(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        string result = "";
        int i = 0;

        while (i < input.Length)
        {
            char c = input[i];

            if (c == '!' || c == '?')
            {
                bool hasExclamation = false;
                bool hasQuestion = false;

                while (i < input.Length && (input[i] == '!' || input[i] == '?'))
                {
                    if (input[i] == '!') hasExclamation = true;
                    if (input[i] == '?') hasQuestion = true;
                    i++;
                }

                if (hasQuestion && hasExclamation)
                    result += "?!";
                else if (hasExclamation)
                    result += "!";
                else
                    result += "?";
            }
            else
            {
                result += c;
                i++;
            }
        }

        return result;
    }

    public static string ForeachNormalize(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        string result = "";
        bool hasExclamation = false;
        bool hasQuestion = false;
        bool inPunctuationGroup = false;

        foreach (char c in input)
        {
            if (c == '!' || c == '?')
            {
                inPunctuationGroup = true;
                if (c == '!') hasExclamation = true;
                if (c == '?') hasQuestion = true;
            }
            else
            {
                if (inPunctuationGroup)
                {
                    result += ResolvePunctuation(hasQuestion, hasExclamation);
                    hasExclamation = false;
                    hasQuestion = false;
                    inPunctuationGroup = false;
                }

                result += c;
            }
        }

        if (inPunctuationGroup)
        {
            result += ResolvePunctuation(hasQuestion, hasExclamation);
        }

        return result;
    }

    private static string ResolvePunctuation(bool hasQuestion, bool hasExclamation)
    {
        if (hasQuestion && hasExclamation) return "?!";
        if (hasQuestion) return "?";
        return "!";
    }
}
