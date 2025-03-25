using UnityEngine.Advertisements;
using Zenject;
using UnityEngine;
using Project.Player;
using Project.HealthSpace;
using Project.Input;

public class UnityAdsManager : IUnityAdsShowListener, IUnityAdsInitializationListener, IUnityAdsLoadListener {

    private bool testMode = true;

    private const string androidGameId = "5815180";
    private string gameId;

    private const string androidAdId = "Rewarded_Android";
    string adId = null;

    private PlayerController player;
    private IInputHandler inputHandler;

    [Inject]
    public void Construct(PlayerController player, IInputHandler inputHandler) {
        this.player = player;
        this.inputHandler = inputHandler;

        gameId = androidGameId;
        adId = androidAdId;

        Advertisement.Initialize(gameId, testMode, this);

    }

    public void LoadAd() {
        Advertisement.Load(adId, this);
    }

    public void ShowAd() {
        Advertisement.Show(adId, this);
        LoadAd();
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) {
        if (adUnitId.Equals(adId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) {
            GiveReward();
        }
    }

    private void GiveReward() {
        player.GetComponent<PlayerHealth>().RestoreHealth();
        player.GetComponent<PlayerAnimator>().enabled = true;
        player.GetComponent<Animator>().SetTrigger("Attack");
        inputHandler.EnableInput();

    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { Debug.Log("OnUnityAdsShowFailure"); }

    public void OnUnityAdsShowStart(string placementId) { Debug.Log("OnUnityAdsShowStart"); }

    public void OnUnityAdsShowClick(string placementId) { Debug.Log("OnUnityAdsShowClick"); }

    public void OnInitializationComplete() {
        LoadAd();

        Debug.Log("OnInitializationComplete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) { Debug.Log("OnInitializationFailed"); }

    public void OnUnityAdsAdLoaded(string placementId) { Debug.Log("OnUnityAdsAdLoaded"); }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) { Debug.Log("OnUnityAdsFailedToLoad"); }
}
