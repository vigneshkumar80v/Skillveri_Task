using System.Text;
using TMPro;
using UnityEngine;

namespace ArtifactInspector
{
    [CreateAssetMenu(menuName = "Artifact Inspector/Content/Bullet List Block", fileName = "BulletListBlock")]
    public sealed class BulletListContentBlock : ContentBlock
    {
        [SerializeField] private string heading;
        [SerializeField] private string[] items;

        public override void Build(RectTransform parent)
        {
            if (!string.IsNullOrEmpty(heading))
                UIFactory.CreateText(parent, "Heading", heading, 20f, FontStyles.Bold);

            if (items == null || items.Length == 0) return;

            var sb = new StringBuilder();
            foreach (var item in items)
                sb.Append("\u2022 ").AppendLine(item);

            UIFactory.CreateText(parent, "List", sb.ToString().TrimEnd(), 17f);
        }
    }
}
