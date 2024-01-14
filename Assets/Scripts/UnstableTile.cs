using UnityEngine;

namespace BloxorzRemake
{
    public class UnstableTile : MonoBehaviour
    {
        public void Fall() {
            GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
