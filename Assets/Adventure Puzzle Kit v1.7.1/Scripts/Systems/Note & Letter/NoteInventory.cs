using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventurePuzzleKit.NoteSystem
{
    public class NoteInventory : MonoBehaviour
    {
        public static NoteInventory instance;

        public event Action OnInventoryChanged;

        private readonly List<NoteCollectable> notes = new List<NoteCollectable>();

        public IReadOnlyList<NoteCollectable> Notes => notes;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        public void AddNote(NoteCollectable note)
        {
            if (note == null) return;
            if (notes.Contains(note)) return;

            notes.Add(note);
            OnInventoryChanged?.Invoke();
        }
    }
}