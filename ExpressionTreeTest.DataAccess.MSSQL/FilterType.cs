namespace ExpressionTreeTest.DataAccess.MSSQL
{
    public enum FilterType
    {
        Null,
        NotNull,
        Equals,
        NotEquals,
        Blank,
        NotBlank,
        Contains,
        NotContains,
        StartsWith,
        NotStartWith,
        EndsWith,
        NotEndWith,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }

   /* public enum Stub 
    {
        Null = FilterType.Null,
        NotNull = FilterType.NotNull,
        Equals = FilterType.Equals,
        NotEquals = FilterType.NotEquals,
        Blank = FilterType.Blank,
        NotBlank = FilterType.NotBlank,
        Contains = FilterType.Contains,
        NotContains = FilterType.NotContains,
        StartsWith = FilterType.StartsWith,
        NotStartWith = FilterType.NotStartWith,
        EndsWith = FilterType.EndsWith,
        NotEndWith = FilterType.NotEndWith,
        GreaterThan = FilterType.GreaterThan,
        GreaterThanOrEqual = FilterType.GreaterThanOrEqual,
        LessThan = FilterType.LessThan,
        LessThanOrEqual = FilterType.LessThanOrEqual
    }*/

    public enum StringFilterType
    {
        Null = FilterType.Null,
        NotNull = FilterType.NotNull,
        Equals = FilterType.Equals,
        NotEquals = FilterType.NotEquals,
        Blank = FilterType.Blank,
        NotBlank = FilterType.NotBlank,
        Contains = FilterType.Contains,
        NotContains = FilterType.NotContains,
        StartsWith = FilterType.StartsWith,
        NotStartWith = FilterType.NotStartWith,
        EndsWith = FilterType.EndsWith,
        NotEndWith = FilterType.NotEndWith,
    }

    public enum DateFilterType
    {
        Null = FilterType.Null,
        NotNull = FilterType.NotNull,
        Equals = FilterType.Equals,
        NotEquals = FilterType.NotEquals,
        GreaterThan = FilterType.GreaterThan,
        GreaterThanOrEqual = FilterType.GreaterThanOrEqual,
        LessThan = FilterType.LessThan,
        LessThanOrEqual = FilterType.LessThanOrEqual
    }

    public enum NumberFilterType
    {
        Null = FilterType.Null,
        NotNull = FilterType.NotNull,
        Equals = FilterType.Equals,
        NotEquals = FilterType.NotEquals,
        GreaterThan = FilterType.GreaterThan,
        GreaterThanOrEqual = FilterType.GreaterThanOrEqual,
        LessThan = FilterType.LessThan,
        LessThanOrEqual = FilterType.LessThanOrEqual
    }
}
