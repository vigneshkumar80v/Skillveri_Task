using System.Collections.Generic;
using UnityEngine;

namespace ArtifactInspector
{
    /// <summary>
    /// Anything the player can "inspect". The sensor and the panel depend only on this
    /// abstraction, so new inspectable types never require changes to them.
    /// </summary>
    public interface IInspectable
    {
        string Title { get; }
        Vector3 Position { get; }
        IReadOnlyList<ContentBlock> Blocks { get; }
    }
}
