using System.Collections;
using UnityEngine;

namespace BloxorzRemake
{
    public class Button : MonoBehaviour {
        [SerializeField] private bool requiresStanding;
        [SerializeField] private Bridge bridge;
        
        private IEnumerator OnTriggerEnter(Collider other) {
            if (other.GetComponent<Player>() == null) yield break;
            var player = other.GetComponent<Player>();
            while (player.IsMoving) {
                yield return null;
            }

            if (requiresStanding && !player.IsStanding) yield break;
            bridge.Trigger();
        }
    }
}
