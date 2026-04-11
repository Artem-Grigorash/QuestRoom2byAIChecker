using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdventurePuzzleKit
{
    public class SceneTrigger : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string sceneToLoad = "EndingScene";
        [SerializeField] private string targetTag = "Player";
        [SerializeField] private bool once = true;

        private bool triggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (triggered && once) return;

            if (other.CompareTag(targetTag))
            {
                triggered = true;
                if (!string.IsNullOrEmpty(sceneToLoad))
                {
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
        }
    }
}