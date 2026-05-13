namespace PalindromeChecker;

public static class Palindrome
{
    public static string Normalize(string input)
    {
        string result = "";
        foreach (char c in input)
        {
            if (c != ' ')
                result += char.ToLower(c);
        }

        return result;
    }

    public static bool TwoPointers(string input)
    {
        string normalized = Normalize(input);

        int left = 0;
        int right = normalized.Length - 1;

        while (left < right)
        {
            if (normalized[left] != normalized[right])
                return false;

            left++;
            right--;
        }

        return true;
    }

    public static bool UsingStack(string input)
    {
        string normalized = Normalize(input);
        int length = normalized.Length;

        Stack<char> stack = new Stack<char>();

        for (int i = 0; i < length / 2; i++)
            stack.Push(normalized[i]);

        int startSecondHalf = (length % 2 == 0) ? length / 2 : length / 2 + 1;

        for (int i = startSecondHalf; i < length; i++)
        {
            if (stack.Pop() != normalized[i])
                return false;
        }

        return true;
    }
}
