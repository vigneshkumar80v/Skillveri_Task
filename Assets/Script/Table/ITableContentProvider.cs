namespace TableSystem
{
    /// <summary>
    /// Supplies the text for the table. Implement this to show real data
    /// (e.g. a list of players) without changing DynamicTable.
    /// </summary>
    public interface ITableContentProvider
    {
        string GetHeaderText(int column);
        string GetCellText(int row, int column);
    }
}
