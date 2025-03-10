using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Config;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class SceneLoader {
    public static bool canAnimateUI = true;

    private GameObject loadingScreen;

    [Inject]
    public async void Construct(
        AssetReferenceContainer assetReferenceContainer,
        AssetProvider assetProvider
    ) {

        GameObject loadingScreenPrefab = await assetProvider.LoadAssetAsync<GameObject>(assetReferenceContainer.loadingScreen);
        loadingScreen = Object.Instantiate(loadingScreenPrefab);
        Object.DontDestroyOnLoad(loadingScreen);
        loadingScreen.SetActive(false);

    }

    public async UniTaskVoid LoadScene(AssetReference sceneReference) {
        loadingScreen.SetActive(true);

        canAnimateUI = false;
        DOTween.KillAll();

        await Addressables.LoadSceneAsync(sceneReference);

        canAnimateUI = true;
        Time.timeScale = 1;

        await UniTask.Delay(100, ignoreTimeScale:true);

        loadingScreen.SetActive(false);
    }

}