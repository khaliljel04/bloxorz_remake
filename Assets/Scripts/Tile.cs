using System;
using UnityEngine;

namespace BloxorzRemake {
    [ExecuteInEditMode]
    public class Tile : MonoBehaviour {
        private void Update() {
            if (GetComponent<BridgeTile>() != null) transform.name = "Bridge Tile (" + transform.position.x + "," + transform.position.z + ")";
            else if (GetComponent<WinningTile>() != null) transform.name = "Winning Tile (" + transform.position.x + "," + transform.position.z + ")";
            else if (GetComponent<UnstableTile>() != null) transform.name = "Unstable Tile (" + transform.position.x + "," + transform.position.z + ")";
            else transform.name = "Tile (" + transform.position.x + "," + transform.position.z + ")";
        }
    }
}
