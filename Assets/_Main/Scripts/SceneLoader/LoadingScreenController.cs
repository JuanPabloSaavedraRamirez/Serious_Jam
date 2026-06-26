using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneLoading
{
    public class LoadingScreenController : MonoBehaviour
    {
        [Header("Animación (UI - fade con CanvasGroup)")]
        [Tooltip("Duración del fade de entrada (segundos).")]
        [SerializeField] private float showDuration = 0.4f;

        [Tooltip("Duración del fade de salida (segundos).")]
        [SerializeField] private float hideDuration = 0.4f;

        [Header("Barra de progreso (opcional)")]
        [SerializeField] private Slider progressBar;

        [Header("Texto 'Cargando...' animado")]
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private string baseText = "Loading";
        [SerializeField] private float dotInterval = 0.4f;
        [SerializeField] private int maxDots = 3;

        [Header("Engranaje girando")]
        [SerializeField] private RectTransform gearImage;
        [SerializeField] private float gearRotationSpeed = 90f;

        [Header("Referencias")]
        [SerializeField] private CanvasGroup canvasGroup;

        private bool _isActive;
        private float _dotTimer;
        private int _dotCount;

        private void Awake()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            SceneLoader.OnLoadProgress += UpdateProgress;
        }

        private void OnDestroy()
        {
            SceneLoader.OnLoadProgress -= UpdateProgress;
        }

        private void Update()
        {
            if (!_isActive) return;

            if (loadingText != null)
            {
                _dotTimer += Time.unscaledDeltaTime;
                if (_dotTimer >= dotInterval)
                {
                    _dotTimer = 0f;
                    _dotCount = (_dotCount + 1) % (maxDots + 1);
                    loadingText.text = baseText + new string('.', _dotCount);
                }
            }
            if (gearImage != null)
                gearImage.Rotate(0f, 0f, -gearRotationSpeed * Time.unscaledDeltaTime);
        }

        public IEnumerator PlayShowAnimation()
        {
            _isActive = true;
            _dotTimer = 0f;
            _dotCount = 0;
            if (loadingText != null) loadingText.text = baseText;

            canvasGroup.blocksRaycasts = true;
            yield return FadeCanvasGroup(0f, 1f, showDuration);
            canvasGroup.interactable = true;
        }

        public IEnumerator PlayHideAnimation()
        {
            canvasGroup.interactable = false;
            yield return FadeCanvasGroup(1f, 0f, hideDuration);
            canvasGroup.blocksRaycasts = false;

            _isActive = false;
        }

        private void UpdateProgress(float progress)
        {
            if (progressBar != null)
                progressBar.value = progress;
        }

        private IEnumerator FadeCanvasGroup(float from, float to, float duration)
        {
            float elapsed = 0f;
            canvasGroup.alpha = from;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
                yield return null;
            }

            canvasGroup.alpha = to;
        }
    }
}