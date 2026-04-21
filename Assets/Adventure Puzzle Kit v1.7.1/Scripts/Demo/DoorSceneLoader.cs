using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventurePuzzleKit
{
    public class DoorSceneLoader : MonoBehaviour
    {
        [Header("Scene Settings")]
        [SerializeField] private string nextSceneName = "";

        private bool hasLoaded = false;

        public void LoadNextScene()
        {
            if (hasLoaded) return;

            if (string.IsNullOrWhiteSpace(nextSceneName))
            {
                Debug.LogError("DoorSceneLoader: nextSceneName is empty on " + gameObject.name);
                return;
            }

            hasLoaded = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }
}