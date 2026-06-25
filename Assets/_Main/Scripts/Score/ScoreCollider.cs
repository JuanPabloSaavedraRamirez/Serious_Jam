using UnityEngine;

namespace Scripts.Score
{
    [RequireComponent(typeof(Collider))]
    public class ScoreCollider : MonoBehaviour
    {
        private ScoreSystemCore _scoreSystemCore;
        [SerializeField] private int _scoreCount;
        
        #region Unity Methods
        private void Awake()
        {
            _scoreSystemCore = ScoreSystemCore.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                if (_scoreSystemCore == null) Debug.LogError("[ScoreCollider] ScoreSystemCore instance is null");
                else _scoreSystemCore.AddScore(_scoreCount);
            }
        }
        #endregion
    }
}