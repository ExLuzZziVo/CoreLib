namespace CoreLib.CORE.Helpers.ValidationHelpers.Attributes
{
    /// <summary>
    /// Comparison types for validation attributes
    /// </summary>
    public enum ComparisonType : sbyte
    {
        Equal = 1,
        NotEqual = -1,
        Less = -3,
        LessOrEqual = -2,
        Greater = 3,
        GreaterOrEqual = 2
    }
}