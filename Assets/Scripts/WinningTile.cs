using System.Collections;
using UnityEngine;

namespace BloxorzRemake
{
    public class WinningTile : MonoBehaviour {
        private IEnumerator OnCollisionEnter(Collision collision) {
            if (collision.gameObject.GetComponent<Player>() == null) yield break;
            var player = collision.gameObject.GetComponent<Player>();
            while (player.IsMoving) {
                yield return null;
            }
            if (!player.IsStanding) yield break;
            WinLevel();
        }

        private void WinLevel() {
            GetComponent<Collider>().enabled = false;
        }
    }
}
