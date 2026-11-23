namespace Library.Tests
{
    // scip-dotnet marks test projects, not test attributes, so this fixture needs no test framework.
    public class CalculatorTests
    {
        public void AddsTwoNumbers()
        {
            var calculator = new Calculator();
            AssertEqual(TestData.Three, calculator.Add(1, 2));
        }

        public void AcceptsZero()
        {
            AssertEqual(0, Guard.NotNegative(0));
        }

        private static void AssertEqual(int expected, int actual)
        {
            if (expected != actual)
            {
                throw new System.Exception($"Expected {expected}, got {actual}");
            }
        }
    }
}
