using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AdventurePuzzleKit.NoteSystem
{
    public class NoteInventorySlot : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Button button;

        private NoteCollectable note;

        public void Init(NoteCollectable noteCollectable)
        {
            note = noteCollectable;

            Debug.Log("NoteInventorySlot Init: " + note.NoteTitle);

            if (iconImage != null)
            {
                iconImage.sprite = note.InventoryIcon;
                iconImage.enabled = note.InventoryIcon != null;
            }
            else
            {
                Debug.LogWarning("Icon Image is missing in NoteInventorySlot");
            }

            if (titleText != null)
                titleText.text = note.NoteTitle;

            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(OpenNote);
            }
            else
            {
                Debug.LogWarning("Button is missing in NoteInventorySlot");
            }
        }

        private void OpenNote()
        {
            Debug.Log("Clicked note slot");

            if (note != null)
            {
                note.OpenFromInventory(transform);
            }
            else
            {
                Debug.LogWarning("Note reference is null");
            }
        }

    }
}