using UnityEngine;

namespace AdventurePuzzleKit.PadlockSystem
{
    public class PadlockItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private PadlockController _padlockController = null;

        public void StartLooking() { }

        public void StopInteraction() { }

        public void HandleInputClick()
        {
            _padlockController.ShowPadlock();
        }

        public void HandleInputHold() { }

        public void HandleInputStop() { }

        public void ObjectInteract()
        {
            _padlockController.ShowPadlock();
        }
    }
}