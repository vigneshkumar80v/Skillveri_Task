using System;

namespace ArtifactInspector
{
    /// <summary>
    /// Lightweight event bus (Observer pattern). Publishers and subscribers stay decoupled.
    /// </summary>
    public static class InspectionEvents
    {
        public static event Action<IInspectable> Focused;
        public static event Action Cleared;

        public static void RaiseFocused(IInspectable inspectable) => Focused?.Invoke(inspectable);
        public static void RaiseCleared() => Cleared?.Invoke();

        // Static events survive Play Mode when "Domain Reload" is disabled, so reset them.
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            Focused = null;
            Cleared = null;
        }
    }
}
