using System.Collections;
using UnityEngine;

namespace BloxorzRemake
{
    public class BridgeTile : MonoBehaviour {

        private bool _isMoving;
        public bool IsMoving => _isMoving;
        
        public void Flip(float openingSpeed, int plusMinus) {
            StartCoroutine(FlipCoroutine());
            
            IEnumerator FlipCoroutine() {
                _isMoving = true;
                var direction = new Vector3((transform.parent.position.x - transform.position.x), 0, (transform.parent.position.z - transform.position.z));
                var anchor = transform.position + new Vector3(0, 0.1f * plusMinus, 0) + plusMinus * direction.normalized/2;
                var axis = Vector3.Cross(Vector3.up, direction);
            
                for (var i = 0; i < 180 / openingSpeed; i++) {
                    transform.RotateAround(anchor, axis, -openingSpeed*plusMinus);
                    yield return new WaitForSeconds(0.01f);
                }
                
                var position = transform.position;
                var eulerAngles = transform.eulerAngles;
                var targetPos = new Vector3(RoundToNearestTenth(position.x),RoundToNearestTenth(position.y), RoundToNearestTenth(position.z));
                var targetAngle = Quaternion.Euler(RoundToNearest90(eulerAngles.x), RoundToNearest90(eulerAngles.y), RoundToNearest90(eulerAngles.z));
                transform.position = targetPos;
                transform.rotation = targetAngle;
            
                float RoundToNearestTenth(float a) { return Mathf.Round(a / .1f) * .1f; }
                float RoundToNearest90(float a) { return Mathf.Round(a / 90) * 90; }
                _isMoving = false;
            }
        }
        
    }
}
