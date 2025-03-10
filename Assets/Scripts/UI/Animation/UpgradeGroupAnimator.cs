using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI {

    public class UpgradeGroupAnimator : MonoBehaviour {

        [SerializeField] private Button minusButton;
        [SerializeField] private TextMeshProUGUI minusText;
        [SerializeField] private Button plusButton;
        [SerializeField] private TextMeshProUGUI plusText;

        [SerializeField] private float duration = .2f;
        [SerializeField] private float scaleMultiplier = 1.2f;

        private Sequence minusSequence;
        private Sequence plusSequence;

        private void Start() {
            minusButton.onClick.AddListener(AnimateMinus);
            plusButton.onClick.AddListener(AnimatePlus);
        }

        private void AnimateMinus() {
            minusSequence.Kill();

            minusSequence = ScaleAndColorTextSequence(minusText, Color.red);
        }

        private void AnimatePlus() {
            plusSequence.Kill();

            plusSequence = ScaleAndColorTextSequence(plusText, Color.green);
        }

        private Sequence ScaleAndColorTextSequence(TextMeshProUGUI text, Color color) {
            text.transform.localScale = Vector3.one;
            text.color = Color.black;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(text.transform.DOScale(scaleMultiplier, duration).SetEase(Ease.OutBack))
                    .Join(text.DOColor(color, duration))
                    .Append(text.transform.DOScale(1f, duration).SetEase(Ease.InBack))
                    .Append(text.DOColor(Color.black, duration));

            sequence.Play();

            return sequence;
        }

    }
}