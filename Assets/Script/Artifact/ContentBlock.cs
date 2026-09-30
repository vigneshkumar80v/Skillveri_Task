using UnityEngine;

namespace ArtifactInspector
{
    /// <summary>
    /// Base class for one piece of panel content (text, image, list...).
    /// Each block is a ScriptableObject asset, so content is data-driven and reusable.
    /// To add a new UI type: subclass this and implement Build(). No existing code changes.
    /// </summary>
    public abstract class ContentBlock : ScriptableObject
    {
        /// <summary>Create this block's UI elements under <paramref name="parent"/>.</summary>
        public abstract void Build(RectTransform parent);
    }
}
