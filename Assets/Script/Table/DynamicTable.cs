using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TableSystem
{
    /// <summary>
    /// Manager for the dynamic table. Reads the row/column input fields and builds a grid of
    /// TableCell prefabs. Cells are pooled: regenerating reuses existing cells instead of
    /// destroying and re-instantiating them.
    ///
    /// Header: when enabled, it is an EXTRA row on top of the requested number of rows.
    /// </summary>
    public sealed class DynamicTable : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private TMP_InputField rowsInput;
        [SerializeField] private TMP_InputField columnsInput;
        [SerializeField] private Button generateButton;
        [SerializeField] private Toggle headerToggle;
        [SerializeField] private TextMeshProUGUI messageLabel;

        [Header("Table")]
        [SerializeField] private RectTransform tableRoot;
        [SerializeField] private TableCellView cellPrefab;
        [SerializeField] private TableStyle style;

        [Header("Options")]
        [SerializeField] private bool showHeader;
        [SerializeField, Min(1)] private int maxRows = 30;
        [SerializeField, Min(1)] private int maxColumns = 30;

        private readonly List<TableCellView> _cellPool = new List<TableCellView>();
        private GridLayoutGroup _grid;

        /// <summary>Replace to feed real data into the table. Defaults to coordinates.</summary>
        public ITableContentProvider ContentProvider { get; set; } = new CoordinateContentProvider();

        private void Awake()
        {
            if (style == null) style = ScriptableObject.CreateInstance<TableStyle>();

            SetUpTableRoot();
            SetUpInput(rowsInput, "3");
            SetUpInput(columnsInput, "4");

            if (headerToggle != null) headerToggle.SetIsOnWithoutNotify(showHeader);
        }

        private void OnEnable()
        {
            if (generateButton != null) generateButton.onClick.AddListener(Generate);
            if (headerToggle != null) headerToggle.onValueChanged.AddListener(SetHeaderVisible);
        }

        private void OnDisable()
        {
            if (generateButton != null) generateButton.onClick.RemoveListener(Generate);
            if (headerToggle != null) headerToggle.onValueChanged.RemoveListener(SetHeaderVisible);
        }

        private void Start() => Generate();

        public void SetHeaderVisible(bool visible)
        {
            showHeader = visible;
            Generate();
        }

        /// <summary>Validates the inputs and (re)builds the table.</summary>
        public void Generate()
        {
            if (!TryReadSize(rowsInput, "Rows", maxRows, out int rows)) return;
            if (!TryReadSize(columnsInput, "Columns", maxColumns, out int columns)) return;

            ShowMessage(string.Empty);
            Build(rows, columns);
        }

        private void Build(int rows, int columns)
        {
            ApplyGridSettings(columns);

            int totalRows = rows + (showHeader ? 1 : 0);
            int needed = totalRows * columns;
            EnsurePoolSize(needed);

            int index = 0;
            for (int r = 0; r < totalRows; r++)
            {
                bool isHeader = showHeader && r == 0;
                int dataRow = showHeader ? r - 1 : r;

                for (int c = 0; c < columns; c++)
                {
                    var cell = _cellPool[index++];
                    cell.gameObject.SetActive(true);

                    if (isHeader)
                    {
                        cell.Bind(ContentProvider.GetHeaderText(c), style.HeaderColor,
                            style.HeaderTextColor, style.HeaderFontSize, FontStyles.Bold);
                    }
                    else
                    {
                        Color background = dataRow % 2 == 0 ? style.CellColor : style.AlternateRowColor;
                        cell.Bind(ContentProvider.GetCellText(dataRow, c), background,
                            style.TextColor, style.FontSize, FontStyles.Normal);
                    }
                }
            }

            // Hide (not destroy) any cells left over from a bigger previous table.
            for (int i = needed; i < _cellPool.Count; i++)
                _cellPool[i].gameObject.SetActive(false);
        }

        private void EnsurePoolSize(int count)
        {
            while (_cellPool.Count < count)
                _cellPool.Add(Instantiate(cellPrefab, tableRoot));
        }

        private void ApplyGridSettings(int columns)
        {
            _grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _grid.constraintCount = columns;
            _grid.cellSize = style.CellSize;
            _grid.spacing = style.Spacing;
            _grid.padding = new RectOffset(style.Padding, style.Padding, style.Padding, style.Padding);
            _grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
            _grid.startAxis = GridLayoutGroup.Axis.Horizontal;
            _grid.childAlignment = TextAnchor.UpperLeft;
        }

        private void SetUpTableRoot()
        {
            _grid = EnsureComponent<GridLayoutGroup>(tableRoot.gameObject);

            var fitter = EnsureComponent<ContentSizeFitter>(tableRoot.gameObject);
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Top-left anchored so the table grows right/down inside a Scroll View.
            tableRoot.anchorMin = new Vector2(0f, 1f);
            tableRoot.anchorMax = new Vector2(0f, 1f);
            tableRoot.pivot = new Vector2(0f, 1f);
        }

        private static void SetUpInput(TMP_InputField field, string defaultText)
        {
            if (field == null) return;
            field.contentType = TMP_InputField.ContentType.IntegerNumber;
            if (string.IsNullOrEmpty(field.text)) field.text = defaultText;
        }

        private bool TryReadSize(TMP_InputField field, string label, int max, out int value)
        {
            value = 0;
            if (field == null) return false;

            if (!int.TryParse(field.text, out value) || value < 1 || value > max)
            {
                ShowMessage($"{label} must be a whole number from 1 to {max}.");
                return false;
            }
            return true;
        }

        private void ShowMessage(string message)
        {
            if (messageLabel != null) messageLabel.text = message;
        }

        private static T EnsureComponent<T>(GameObject target) where T : Component
        {
            return target.TryGetComponent(out T existing) ? existing : target.AddComponent<T>();
        }
    }
}
