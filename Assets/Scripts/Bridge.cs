using UnityEngine;

namespace BloxorzRemake
{
    public class Bridge : MonoBehaviour {
        [SerializeField] private BridgeTile bridgeTile1;
        [SerializeField] private BridgeTile bridgeTile2;
        [SerializeField] private float bridgeOpeningSpeed;
        private int _plusMinus;

        private void Start() {
            _plusMinus = 1;
        }

        public void Trigger() {
            if (bridgeTile1.IsMoving) return;
            bridgeTile1.Flip(bridgeOpeningSpeed, _plusMinus);
            bridgeTile2.Flip(bridgeOpeningSpeed, _plusMinus);
            _plusMinus = -_plusMinus;
        }

    }
}
