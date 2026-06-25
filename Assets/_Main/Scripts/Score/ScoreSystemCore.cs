using System;

namespace Scripts.Score
{
    public class ScoreSystemCore
    {
        #region Singleton
        private ScoreSystemCore() { }
        public static ScoreSystemCore Instance { get; } = new ScoreSystemCore();
        #endregion

        #region Score system variables
        public int score { get; private set; }
        public int goalScore;
        #endregion

        #region Events
        public event Action OnGoalCompleted;
        public event Action<int> OnScoreChanged;
        #endregion

        #region Logic
        public void SetScoreSystem(int goal)
        {
            ResetScore();
            goalScore = goal;
        }
        public int AddScore(int newScore)
        {
            score += newScore;
            OnScoreChanged?.Invoke(score);
            if (score >= goalScore) OnGoalCompleted?.Invoke();
            return score;
        }
        private void ResetScore() => score = 0;
        #endregion
    }
}