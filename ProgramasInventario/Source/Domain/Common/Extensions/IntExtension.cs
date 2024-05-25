namespace Domain.Common.Extensions;

public static class IntExtension
{
    public static int GetMin(params int[] values) => values.Min();
}