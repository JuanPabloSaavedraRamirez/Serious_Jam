using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoading
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        public static event Action<float> OnLoadProgress;
        public static event Action OnSceneReady;

        [Header("Configuración")]
        [Tooltip("Tiempo mínimo que la pantalla de carga permanece visible (en segundos).")]
        [SerializeField] private float minimumLoadingTime = 0.5f;

        [Header("Referencias")]
        [Tooltip("Controlador de la pantalla de carga (UI), hijo de este mismo objeto persistente.")]
        [SerializeField] private LoadingScreenController loadingScreenController;

        private readonly List<Func<Task>> _mandatoryTasks = new();
        private bool _isLoading;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public static void RegisterMandatoryTask(Func<Task> task)
        {
            if (Instance == null)
            {
                Debug.LogError("[SceneLoader] No existe instancia activa.");
                return;
            }
            Instance._mandatoryTasks.Add(task);
        }

        public static void ClearMandatoryTasks() => Instance?._mandatoryTasks.Clear();

        public static void LoadScene(string sceneName)
        {
            if (Instance == null)
            {
                Debug.LogError("[SceneLoader] No existe instancia activa.");
                return;
            }

            if (Instance._isLoading)
            {
                Debug.LogWarning("[SceneLoader] Ya hay una carga en progreso.");
                return;
            }

            Instance.StartCoroutine(Instance.LoadSceneRoutine(sceneName));
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            _isLoading = true;

            yield return loadingScreenController.PlayShowAnimation();

            float startTime = Time.realtimeSinceStartup;

            AsyncOperation sceneOp = SceneManager.LoadSceneAsync(sceneName);
            sceneOp.allowSceneActivation = false;

            while (sceneOp.progress < 0.9f)
            {
                OnLoadProgress?.Invoke(sceneOp.progress / 0.9f);
                yield return null;
            }

            OnLoadProgress?.Invoke(1f);

            float elapsed = Time.realtimeSinceStartup - startTime;
            if (elapsed < minimumLoadingTime)
                yield return new WaitForSecondsRealtime(minimumLoadingTime - elapsed);

            sceneOp.allowSceneActivation = true;
            yield return new WaitUntil(() => sceneOp.isDone);

            OnSceneReady?.Invoke();

            if (_mandatoryTasks.Count > 0)
            {
                yield return StartCoroutine(RunMandatoryTasks());
                _mandatoryTasks.Clear();
            }

            yield return loadingScreenController.PlayHideAnimation();

            _isLoading = false;
        }

        private IEnumerator RunMandatoryTasks()
        {
            var pendingTasks = new List<Task>(_mandatoryTasks.Count);

            foreach (var taskFactory in _mandatoryTasks)
            {
                try
                {
                    pendingTasks.Add(taskFactory.Invoke());
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[SceneLoader] Error iniciando tarea obligatoria: {ex}");
                }
            }

            Task allTasks = Task.WhenAll(pendingTasks);
            yield return new WaitUntil(() => allTasks.IsCompleted);

            if (allTasks.IsFaulted)
                Debug.LogError($"[SceneLoader] Una o más tareas obligatorias fallaron: {allTasks.Exception}");
        }
    }
}