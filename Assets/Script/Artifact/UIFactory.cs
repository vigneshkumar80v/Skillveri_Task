using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArtifactInspector
{
    /// <summary>Small helpers so content blocks can build UI without prefabs.</summary>
    public static class UIFactory
    {
        public static RectTransform CreateRect(string name, RectTransform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            var rect = (RectTransform)go.transform;
            rect.SetParent(parent, false);
            return rect;
        }

        public static TextMeshProUGUI CreateText(RectTransform parent, string name, string text,
            float fontSize, FontStyles style = FontStyles.Normal, Color? color = null)
        {
            var rect = CreateRect(name, parent);
            var label = rect.gameObject.AddComponent<TextMeshProUGUI>();
            label.text = text;
            label.fontSize = fontSize;
            label.fontStyle = style;
            label.color = color ?? Color.white;
            label.raycastTarget = false;
            return label;
        }

        public static Image CreateImage(RectTransform parent, string name, Sprite sprite, float preferredHeight)
        {
            var rect = CreateRect(name, parent);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = sprite;
            image.preserveAspect = true;
            image.raycastTarget = false;

            var layout = rect.gameObject.AddComponent<LayoutElement>();
            layout.preferredHeight = preferredHeight;
            layout.minHeight = preferredHeight;
            return image;
        }
    }
}
