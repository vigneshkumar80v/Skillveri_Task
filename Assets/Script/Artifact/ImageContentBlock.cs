using TMPro;
using UnityEngine;

namespace ArtifactInspector
{
    [CreateAssetMenu(menuName = "Artifact Inspector/Content/Image Block", fileName = "ImageBlock")]
    public sealed class ImageContentBlock : ContentBlock
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private string caption;
        [SerializeField, Min(16f)] private float height = 160f;

        public override void Build(RectTransform parent)
        {
            if (sprite != null)
                UIFactory.CreateImage(parent, "Image", sprite, height);

            if (!string.IsNullOrEmpty(caption))
                UIFactory.CreateText(parent, "Caption", caption, 15f, FontStyles.Italic,
                    new Color(1f, 1f, 1f, 0.7f));
        }
    }
}
