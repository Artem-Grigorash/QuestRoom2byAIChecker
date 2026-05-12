using UnityEngine;

namespace AdventurePuzzleKit.NoteSystem
{
    public class NoteTypeSelector : MonoBehaviour, IInteractable
    {
        [Space(5)] [SerializeField] private UIType _NoteType = UIType.None;
        private enum UIType { None, Basic, BasicReverse, NormalCustom, ReverseCustom }

        private BasicNoteController basicNoteController;
        private BasicReverseNoteController basicReverseNoteController;
        private CustomNoteController normalCustomNoteController;
        private CustomReverseNoteController reverseCustomController;

        private void Awake()
        {
            // Use TryGetComponent only for the relevant controller based on the selected type.
            switch (_NoteType)
            {
                case UIType.Basic:
                    TryGetComponent(out basicNoteController);
                    break;
                case UIType.BasicReverse:
                    TryGetComponent(out basicReverseNoteController);
                    break;
                case UIType.NormalCustom:
                    TryGetComponent(out normalCustomNoteController);
                    break;
                case UIType.ReverseCustom:
                    TryGetComponent(out reverseCustomController);
                    break;
            }
        }

        public void StartLooking() { } //Started looking at the Note object

        public void StopInteraction() { } //Stopped interacting with the Note object

        public void HandleInputClick() //Started interaction with the Note object
        {
            DisplayNotes();
        }

        public void HandleInputHold() { } //Holding interaction with the Note object

        public void HandleInputStop() { } //Stopped interaction with the Note object

        public void DisplayNotes()
        {
            bool noteWasOpened = false;

            switch (_NoteType)
            {
                case UIType.Basic:
                    if (basicNoteController.isReadable)
                    {
                        basicNoteController.enabled = true;
                        basicNoteController.ShowNote();
                        noteWasOpened = true;
                    }
                    break;

                case UIType.BasicReverse:
                    if (basicReverseNoteController.isReadable)
                    {
                        basicReverseNoteController.enabled = true;
                        basicReverseNoteController.ShowNote();
                        noteWasOpened = true;
                    }
                    break;

                case UIType.NormalCustom:
                    if (normalCustomNoteController.isReadable)
                    {
                        normalCustomNoteController.enabled = true;
                        normalCustomNoteController.ShowNote();
                        noteWasOpened = true;
                    }
                    break;

                case UIType.ReverseCustom:
                    if (reverseCustomController.isReadable)
                    {
                        reverseCustomController.enabled = true;
                        reverseCustomController.ShowNote();
                        noteWasOpened = true;
                    }
                    break;
            }

            if (noteWasOpened)
            {
                NoteCollectable collectable = GetComponent<NoteCollectable>();

                if (collectable != null)
                {
                    collectable.CollectIfNeeded();
                }
            }
        }
    }
}
