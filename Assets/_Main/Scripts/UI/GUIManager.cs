using UnityEngine;
using TMPro;
using Scripts.Score;

namespace UI
{
    public class GUIManager : MonoBehaviour
    {
        private ScoreSystemCore _scoreSystemCore;
        private GUIEvents _guiEvents;

        [Header("Score Texts")]
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _maxScoreText;

        [Header("Balls Texts")]
        [SerializeField] private TMP_Text _amountBallsText;
        [SerializeField] private TMP_Text _amountBallsMaxText;

        [Header("Game Screens")]
        [SerializeField] private GameObject _gameplayScreen;
        [SerializeField] private GameObject _gameCompleteScreen;
        [SerializeField] private GameObject _gameFailedScreen;

        private void Awake()
        {
            _scoreSystemCore = ScoreSystemCore.Instance;
            _guiEvents = GUIEvents.Instance;

            _gameplayScreen.SetActive(true);
        }

        private void OnEnable()
        {
            //Screens
            _scoreSystemCore.OnGoalCompleted += ShowGameCompleteScreen;
            _guiEvents.OnGameFailed += ShowGameFailedScreen;

            //Score
            _scoreSystemCore.OnScoreChanged += UpdateScoreText;
            _scoreSystemCore.OnMaxScore += UpdateMaxScoreText;

            //Balls

        }

        private void OnDisable()
        {
            //Screens
            _scoreSystemCore.OnGoalCompleted -= ShowGameCompleteScreen;
            _guiEvents.OnGameFailed -= ShowGameFailedScreen;

            //Score
            _scoreSystemCore.OnScoreChanged -= UpdateScoreText;
            _scoreSystemCore.OnMaxScore -= UpdateMaxScoreText;

            //Balls

        }

        private void ShowGameCompleteScreen() 
        {
            _gameCompleteScreen.SetActive(true);
            _gameplayScreen.SetActive(false);
        }

        private void ShowGameFailedScreen()
        {
            _gameFailedScreen.SetActive(true);
            _gameplayScreen.SetActive(false);
        }

        private void UpdateScoreText(int score) => _scoreText.text = $"Score: {score}";
        private void UpdateMaxScoreText(int maxScore) => _maxScoreText.text = $"Goal: {maxScore}";

        private void UpdateAmountBallsText(int amountBalls) => _amountBallsText.text = $"Balls: {amountBalls}";
        private void UpdateAmountBallsMaxText(int amountBallsMax) => _amountBallsMaxText.text = $"Max Balls: {amountBallsMax}";
    }
}