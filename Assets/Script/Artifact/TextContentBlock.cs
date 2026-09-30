using TMPro;
using UnityEngine;

namespace ArtifactInspector
{
    [CreateAssetMenu(menuName = "Artifact Inspector/Content/Text Block", fileName = "TextBlock")]
    public sealed class TextContentBlock : ContentBlock
    {
        [SerializeField] private string heading;
        [SerializeField, TextArea(2, 8)] private string body;

        public override void Build(RectTransform parent)
        {
            if (!string.IsNullOrEmpty(heading))
                UIFactory.CreateText(parent, "Heading", heading, 22f, FontStyles.Bold);

            if (!string.IsNullOrEmpty(body))
                UIFactory.CreateText(parent, "Body", body, 18f);
        }
    }
}
