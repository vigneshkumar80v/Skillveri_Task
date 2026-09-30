using System.Collections.Generic;
using UnityEngine;

namespace ArtifactInspector
{
    /// <summary>
    /// Put this on any world object. Its panel content is defined in the Inspector
    /// as a list of ContentBlock assets, nothing is hardcoded.
    /// </summary>
    public sealed class Artifact : MonoBehaviour, IInspectable
    {
        [SerializeField] private string title = "Artifact";
        [SerializeField] private List<ContentBlock> blocks = new List<ContentBlock>();

        public string Title => title;
        public Vector3 Position => transform.position;
        public IReadOnlyList<ContentBlock> Blocks => blocks;
    }
}
