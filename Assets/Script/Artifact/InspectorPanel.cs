using TMPro;
using UnityEngine;

namespace ArtifactInspector
{
    /// <summary>
    /// Runtime "Inspector" panel. Listens for focus events and asks each ContentBlock
    /// to build itself. It has no knowledge of concrete block or artifact types.
    /// </summary>
    public sealed class InspectorPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleLabel;
        [SerializeField] private RectTransform contentRoot;
        [SerializeField] private GameObject emptyState;

        private void OnEnable()
        {
            InspectionEvents.Focused += Show;
            InspectionEvents.Cleared += Clear;
            Clear();
        }

        private void OnDisable()
        {
            InspectionEvents.Focused -= Show;
            InspectionEvents.Cleared -= Clear;
        }

        private void Show(IInspectable inspectable)
        {
            ClearContent();

            if (emptyState != null) emptyState.SetActive(false);
            if (titleLabel != null) titleLabel.text = $"{inspectable.Title} is selected";

            foreach (var block in inspectable.Blocks)
            {
                if (block != null) block.Build(contentRoot);
            }
        }

        private void Clear()
        {
            ClearContent();

            if (titleLabel != null) titleLabel.text = string.Empty;
            if (emptyState != null) emptyState.SetActive(true);
        }

        private void ClearContent()
        {
            for (int i = contentRoot.childCount - 1; i >= 0; i--)
            {
                var child = contentRoot.GetChild(i).gameObject;
                child.SetActive(false); // hide immediately; Destroy is deferred to end of frame
                Destroy(child);
            }
        }
    }
}
