namespace TableSystem
{
    /// <summary>Default provider: headers are "Column N", cells show their coordinates.</summary>
    public sealed class CoordinateContentProvider : ITableContentProvider
    {
        public string GetHeaderText(int column) => $"Column {column + 1}";
        public string GetCellText(int row, int column) => $"R{row + 1} C{column + 1}";
    }
}
