using System.Text;
using TMPro;
using UnityEngine;

namespace EnemyAI
{
    /// <summary>Shows the current mode and a key legend. Built from the enemy's state list, so it needs no edits for new states.</summary>
    public sealed class EnemyStateHud : MonoBehaviour
    {
        [SerializeField] private EnemyController enemy;
        [SerializeField] private TextMeshProUGUI label;

        private void OnEnable() => enemy.StateChanged += Refresh;
        private void OnDisable() => enemy.StateChanged -= Refresh;
        private void Start() => Refresh(enemy.CurrentState);

        private void Refresh(EnemyState current)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Enemy mode: <b>{(current != null ? current.DisplayName : "-")}</b>");
            sb.AppendLine();

            foreach (var state in enemy.States)
            {
                string key = state.ActivationKey.ToString().Replace("Alpha", string.Empty);
                sb.AppendLine($"[{key}]  {state.DisplayName}");
            }

            label.text = sb.ToString();
        }
    }
}
