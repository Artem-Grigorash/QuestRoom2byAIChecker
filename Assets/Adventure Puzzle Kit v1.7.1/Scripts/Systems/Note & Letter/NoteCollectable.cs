using System.Collections;
using UnityEngine;

namespace AdventurePuzzleKit.NoteSystem
{
    public class NoteCollectable : MonoBehaviour
    {
        [Header("Inventory View")]
        [SerializeField] private string noteTitle = "Note";
        [SerializeField] private Sprite inventoryIcon;

        [Header("World Object")]
        [SerializeField] private bool hideObjectAfterCollect = true;

        private NoteTypeSelector noteTypeSelector;
        private Renderer[] renderers;
        private Collider[] colliders;

        private bool collected = false;

        private CanvasGroup hiddenInventoryCanvasGroup;
        private Coroutine restoreInventoryCoroutine;

        public string NoteTitle => noteTitle;
        public Sprite InventoryIcon => inventoryIcon;
        public bool IsCollected => collected;

        private void Awake()
        {
            noteTypeSelector = GetComponent<NoteTypeSelector>();
            renderers = GetComponentsInChildren<Renderer>(true);
            colliders = GetComponentsInChildren<Collider>(true);
        }

        public void CollectIfNeeded()
        {
            if (collected) return;

            collected = true;

            if (NoteInventory.instance != null)
            {
                NoteInventory.instance.AddNote(this);
            }
            else
            {
                Debug.LogWarning("NoteInventory instance was not found in the scene.");
            }

            if (hideObjectAfterCollect)
            {
                HideWorldObject();
            }
        }

        public void OpenFromInventory()
        {
            OpenFromInventory(null);
        }

        public void OpenFromInventory(Transform inventorySlotTransform)
        {
            Debug.Log("Open note from inventory: " + noteTitle);

            HideInventoryTemporarily(inventorySlotTransform);

            if (noteTypeSelector == null)
            {
                noteTypeSelector = GetComponent<NoteTypeSelector>();
            }

            if (noteTypeSelector == null)
            {
                Debug.LogWarning($"NoteTypeSelector is missing on {gameObject.name}");
                RestoreInventory();
                return;
            }

            noteTypeSelector.DisplayNotes();

            if (restoreInventoryCoroutine != null)
            {
                StopCoroutine(restoreInventoryCoroutine);
            }

            restoreInventoryCoroutine = StartCoroutine(RestoreInventoryAfterNoteClosed());
        }

        private void HideInventoryTemporarily(Transform inventorySlotTransform)
        {
            GameObject inventoryContainer = FindMainInventoryContainer(inventorySlotTransform);

            if (inventoryContainer == null)
            {
                Debug.LogWarning("Main Inventory Container was not found.");
                return;
            }

            hiddenInventoryCanvasGroup = inventoryContainer.GetComponent<CanvasGroup>();

            if (hiddenInventoryCanvasGroup == null)
            {
                hiddenInventoryCanvasGroup = inventoryContainer.AddComponent<CanvasGroup>();
            }

            hiddenInventoryCanvasGroup.alpha = 0f;
            hiddenInventoryCanvasGroup.interactable = false;
            hiddenInventoryCanvasGroup.blocksRaycasts = false;
        }

        private GameObject FindMainInventoryContainer(Transform startTransform)
        {
            Transform current = startTransform;

            while (current != null)
            {
                if (current.name.Contains("Main Inventory Container"))
                {
                    return current.gameObject;
                }

                current = current.parent;
            }

            return null;
        }

        private IEnumerator RestoreInventoryAfterNoteClosed()
        {
            // Ждём один кадр, чтобы BasicNoteController успел включиться
            yield return null;

            while (IsAnyNoteControllerEnabled())
            {
                yield return null;
            }

            RestoreInventory();
            restoreInventoryCoroutine = null;
        }

        private bool IsAnyNoteControllerEnabled()
        {
            BasicNoteController basic = GetComponent<BasicNoteController>();
            if (basic != null && basic.enabled) return true;

            BasicReverseNoteController basicReverse = GetComponent<BasicReverseNoteController>();
            if (basicReverse != null && basicReverse.enabled) return true;

            CustomNoteController custom = GetComponent<CustomNoteController>();
            if (custom != null && custom.enabled) return true;

            CustomReverseNoteController customReverse = GetComponent<CustomReverseNoteController>();
            if (customReverse != null && customReverse.enabled) return true;

            return false;
        }

        private void RestoreInventory()
        {
            if (hiddenInventoryCanvasGroup == null) return;

            hiddenInventoryCanvasGroup.alpha = 1f;
            hiddenInventoryCanvasGroup.interactable = true;
            hiddenInventoryCanvasGroup.blocksRaycasts = true;

            hiddenInventoryCanvasGroup = null;
        }

        private void HideWorldObject()
        {
            foreach (Renderer r in renderers)
            {
                if (r != null)
                    r.enabled = false;
            }

            foreach (Collider c in colliders)
            {
                if (c != null)
                    c.enabled = false;
            }
        }

        private void LateUpdate()
        {
            // BasicNoteController.CloseNote() может обратно включить BoxCollider.
            // Поэтому после сбора держим коллайдеры выключенными.
            if (!collected || !hideObjectAfterCollect) return;

            foreach (Collider c in colliders)
            {
                if (c != null && c.enabled)
                    c.enabled = false;
            }
        }
    }
}