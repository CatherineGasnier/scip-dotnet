namespace Library
{
    public class Calculator
    {
        public int Add(int a, int b) => Guard.NotNegative(a) + Guard.NotNegative(b);
    }
}
