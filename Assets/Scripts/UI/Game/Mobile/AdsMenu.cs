using Project.HealthSpace;
using Project.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.UI.Mobile {
    public class AdsMenu : MonoBehaviour {
        [SerializeField] private Button adsButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private GameObject root;

        private UnityAdsManager unityAdsManager;
        private PlayerController playerController;

        [Inject]
        public void Construct(UnityAdsManager unityAdsManager, PlayerController playerController) {
            this.unityAdsManager = unityAdsManager;
            this.playerController = playerController;
        }

        private void Start() {
            adsButton.onClick.AddListener(unityAdsManager.ShowAd);
            cancelButton.onClick.AddListener(Hide);
            playerController.GetComponent<PlayerHealth>().OnDie += Show;

        }

        private void Show() {
            root.SetActive(true);
        }

        private void Hide() {
            root.SetActive(false);

        }


    }
}