namespace Project.Scripts.Other
{
    public static class GridSortOrderCalculator
    {
        private const int Multiplier = 10;

        public static int Calculate(int x, int y)
        {
            return y * Multiplier + x;
        }
    }
}