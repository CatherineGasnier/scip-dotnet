namespace Library
{
    // Library.Tests also compiles this file. It is still library code, not test code.
    internal static class Guard
    {
        public static int NotNegative(int value) =>
            value < 0 ? throw new System.ArgumentOutOfRangeException(nameof(value)) : value;
    }
}
