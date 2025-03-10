using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.UI {
    public class CommonButtonAnimator : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

        [SerializeField]private float scaleMultiplier = 1.1f;
        [SerializeField]private float duration = 0.2f;

        private Vector3 originalScale;

        void Start() {
            originalScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (SceneLoader.canAnimateUI) {
                transform.DOScale(originalScale * scaleMultiplier, duration).SetEase(Ease.OutBack).SetUpdate(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (SceneLoader.canAnimateUI) {
                transform.DOScale(originalScale, duration).SetEase(Ease.InBack).SetUpdate(true);
            }

        }

        public void OnPointerClick(PointerEventData eventData) {
            if (SceneLoader.canAnimateUI) {
                transform.DOScale(originalScale, duration).SetEase(Ease.InBack).SetUpdate(true);
            }
        }
    }
}