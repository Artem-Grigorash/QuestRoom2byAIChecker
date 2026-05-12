using UnityEngine;

namespace AdventurePuzzleKit.NoteSystem
{
    public class NoteInventoryUI : MonoBehaviour
    {
        [SerializeField] private Transform slotsParent;
        [SerializeField] private NoteInventorySlot slotPrefab;

        private void OnEnable()
        {
            if (NoteInventory.instance != null)
            {
                NoteInventory.instance.OnInventoryChanged += Refresh;
            }

            Refresh();
        }

        private void OnDisable()
        {
            if (NoteInventory.instance != null)
            {
                NoteInventory.instance.OnInventoryChanged -= Refresh;
            }
        }

        public void Refresh()
        {
            if (slotsParent == null || slotPrefab == null) return;
            if (NoteInventory.instance == null) return;

            foreach (Transform child in slotsParent)
            {
                Destroy(child.gameObject);
            }

            foreach (NoteCollectable note in NoteInventory.instance.Notes)
            {
                NoteInventorySlot slot = Instantiate(slotPrefab, slotsParent);
                slot.Init(note);
            }
        }
    }
}