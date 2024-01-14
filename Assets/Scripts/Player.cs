using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BloxorzRemake {
    public class Player : MonoBehaviour {
        [SerializeField] private float rollSpeed;
        [SerializeField] private Transform point1;
        [SerializeField] private Transform point2;
        
        private Rigidbody _rigidbody;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            Move();
            HandlePlayerStability();
        }
        
        // Continue working on the stability system and fix the "input lag" feel

        #region Player Movement
        
        private bool _isMoving;
        public bool IsMoving => _isMoving;
        
        private void Move() {
            RefreshPlayerOrientation();
            if (_isMoving) return;
            var xVector = Vector3.right;
            var zVector = Vector3.forward;
            if (_isStanding) { xVector = Vector3.right / 2; zVector = Vector3.forward / 2;}
            else if (_isDirectionX) zVector = Vector3.forward / 2; 
            else if (_isDirectionZ) xVector = Vector3.right / 2;

            if (Input.GetKeyDown(KeyCode.W)) StartCoroutine(RollCoroutine( zVector));
            else if (Input.GetKeyDown(KeyCode.A)) StartCoroutine(RollCoroutine(-xVector));
            else if (Input.GetKeyDown(KeyCode.S)) StartCoroutine(RollCoroutine(-zVector));
            else if (Input.GetKeyDown(KeyCode.D)) StartCoroutine(RollCoroutine( xVector));
            
            SnapPosition();
        }

        private IEnumerator RollCoroutine(Vector3 direction) {
            _isMoving = true;
            
            var anchor = transform.position + Vector3.down * transform.position.y + direction;
            var axis = Vector3.Cross(Vector3.up, direction);

            for (int i = 1; i < 25; i++) { // Sum of an arithmetic progression. (Σ=90)
                transform.RotateAround(anchor, axis, 0.3f * i);
                yield return new WaitForSeconds(0.1f / rollSpeed);
            }

            _isMoving = false;
        }
        
        private void SnapPosition() {
            var position = transform.position;
            var eulerAngles = transform.eulerAngles;
            var targetPos = new Vector3(RoundAToB(position.x, 0.5f), RoundAToB(position.y, 0.5f), RoundAToB(position.z, 0.5f));
            var targetAngle = Quaternion.Euler(RoundAToB(eulerAngles.x, 90), RoundAToB(eulerAngles.y, 90), RoundAToB(eulerAngles.z, 90));
            transform.position = targetPos;
            transform.rotation = targetAngle;

            // Round a to the nearest b multiple
            float RoundAToB(float a, float b) { return Mathf.Round(a / b) * b; } 
        }
        
        #endregion
        
        #region Player Stability
        
        private List<Transform> _tilesBelow = new();

        private void HandlePlayerStability() {
            GetTilesBelow();
            if (_tilesBelow.Count == 2) { return; }
            if (_tilesBelow.Count == 0)
            {
                _rigidbody.useGravity = true;
                return;
            }
            if (!_isStanding || (_isStanding && _tilesBelow[0].GetComponent<UnstableTile>() != null))
            {
                _rigidbody.useGravity = true;
                return;
            }
        }
        
        private void GetTilesBelow() {
            RefreshPlayerOrientation();
            _tilesBelow.Clear();
            CheckTileBelow(point1);
            CheckTileBelow(point2);

            void CheckTileBelow(Transform point) { 
                var ray = new Ray(point.transform.position, Vector3.down);
                // Ignore Button layer (#10).
                int layerMask = 1 << 10;
                layerMask = ~layerMask;
                if (!Physics.Raycast(ray, out RaycastHit hit, 3, layerMask)) return;
                
                if (hit.transform.GetComponent<Tile>() == null || _tilesBelow.Contains(hit.transform)) return;
                _tilesBelow.Add(hit.transform);
            }
        }

        #endregion
        
        #region Player Orientation
        
        public bool IsStanding => _isStanding;
        private bool _isStanding;
        private bool _isDirectionX;
        private bool _isDirectionZ;

        private void RefreshPlayerOrientation() {
            if (_isMoving) {
                _isStanding= false;
                _isDirectionX= false;
                _isDirectionZ = false;
                return;
            }
            var directorVector = point1.transform.position - point2.transform.position;
            _isStanding = directorVector == Vector3.up || directorVector == Vector3.down;
            _isDirectionX = directorVector == Vector3.left || directorVector == Vector3.right;
            _isDirectionZ = directorVector == Vector3.forward || directorVector == Vector3.back;
        }

        #endregion
    }
}
