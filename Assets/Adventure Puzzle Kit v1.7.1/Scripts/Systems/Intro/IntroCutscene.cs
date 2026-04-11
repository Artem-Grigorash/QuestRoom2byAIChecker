using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace AdventurePuzzleKit
{
    public class IntroCutscene : MonoBehaviour
    {
        [Header("Video Settings")]
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private bool playOnStart = true;

        [Header("Skip Settings")]
        [SerializeField] private bool canSkip = true;
        [SerializeField] private KeyCode skipKey = KeyCode.Space;

        [Header("Post-Cutscene Actions")]
        [SerializeField] private UnityEvent onCutsceneFinished;
        [SerializeField] private string nextSceneName = ""; // Leave empty if not loading a new scene

        private bool hasFinished = false;

        private void Start()
        {
            if (videoPlayer == null)
            {
                videoPlayer = GetComponent<VideoPlayer>();
            }

            if (videoPlayer != null)
            {
                // Subscribe to the loopPointReached event which triggers when the video ends
                videoPlayer.loopPointReached += EndReached;

                if (playOnStart)
                {
                    videoPlayer.Play();
                }
            }
            else
            {
                Debug.LogError("IntroCutscene: VideoPlayer component not assigned!");
            }
        }

        private void Update()
        {
            if (canSkip && !hasFinished)
            {
                if (Input.GetKeyDown(skipKey) || Input.GetKeyDown(KeyCode.Escape))
                {
                    FinishCutscene();
                }
            }
        }

        private void EndReached(VideoPlayer vp)
        {
            FinishCutscene();
        }

        public void FinishCutscene()
        {
            if (hasFinished) return;
            hasFinished = true;

            if (videoPlayer != null)
            {
                videoPlayer.Stop();
            }

            onCutsceneFinished.Invoke();

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                // If not loading a scene, maybe we should disable this object
                gameObject.SetActive(false);
            }
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
