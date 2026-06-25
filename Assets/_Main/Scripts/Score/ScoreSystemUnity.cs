using UnityEngine;

namespace Scripts.Score
{
    public class ScoreSystemUnity : MonoBehaviour
    {
        private ScoreSystemCore _scoreSystemCore;

        [Header("Score System Settings")]
        [SerializeField] private int _goalScore;

        #region Initialization
        public void Awake()
        {
            _scoreSystemCore = ScoreSystemCore.Instance;
        }
        public void Start()
        {
            _scoreSystemCore.SetScoreSystem(_goalScore);
        }
        #endregion
    }
}