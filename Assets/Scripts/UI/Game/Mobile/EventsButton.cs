using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.UI.Mobile {

    public class EventsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

        public event Action onHolding;
        public event Action onClick;

        private bool isHolding = false;
        private void Update() {
            if (isHolding) {
                onHolding?.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            isHolding = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            isHolding = false;

        }

        public void OnPointerClick(PointerEventData eventData) {
            onClick?.Invoke();
        }


    }
}