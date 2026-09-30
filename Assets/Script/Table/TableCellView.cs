using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TableSystem
{
    /// <summary>
    /// One cell of the table (lives on the TableCell prefab). It only knows how to display
    /// itself; it has no idea about rows, columns or headers.
    /// </summary>
    public sealed class TableCellView : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI label;

        public void Bind(string text, Color backgroundColor, Color textColor, float fontSize, FontStyles fontStyle)
        {
            label.text = text;
            label.color = textColor;
            label.fontSize = fontSize;
            label.fontStyle = fontStyle;
            background.color = backgroundColor;
        }

        // Auto-fills the references when the component is added in the Editor.
        private void Reset()
        {
            background = GetComponent<Image>();
            label = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}
