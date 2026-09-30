using UnityEngine;

namespace TableSystem
{
    /// <summary>Visual settings for a table. Swap the asset to re-skin the table without touching code.</summary>
    [CreateAssetMenu(menuName = "Dynamic Table/Table Style", fileName = "TableStyle")]
    public sealed class TableStyle : ScriptableObject
    {
        [Header("Layout")]
        [SerializeField] private Vector2 cellSize = new Vector2(120f, 48f);
        [SerializeField] private Vector2 spacing = new Vector2(4f, 4f);
        [SerializeField, Min(0)] private int padding = 8;

        [Header("Body")]
        [SerializeField] private Color cellColor = new Color(0.16f, 0.20f, 0.26f);
        [SerializeField] private Color alternateRowColor = new Color(0.20f, 0.25f, 0.32f);
        [SerializeField] private Color textColor = Color.white;
        [SerializeField, Min(8f)] private float fontSize = 20f;

        [Header("Header")]
        [SerializeField] private Color headerColor = new Color(0.10f, 0.45f, 0.75f);
        [SerializeField] private Color headerTextColor = Color.white;
        [SerializeField, Min(8f)] private float headerFontSize = 22f;

        public Vector2 CellSize => cellSize;
        public Vector2 Spacing => spacing;
        public int Padding => padding;
        public Color CellColor => cellColor;
        public Color AlternateRowColor => alternateRowColor;
        public Color TextColor => textColor;
        public float FontSize => fontSize;
        public Color HeaderColor => headerColor;
        public Color HeaderTextColor => headerTextColor;
        public float HeaderFontSize => headerFontSize;
    }
}
