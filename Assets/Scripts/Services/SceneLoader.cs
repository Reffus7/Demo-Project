using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneLoader {
    public static bool canAnimateUI = true;

    private AssetProvider assetProvider;

    [Inject]
    public void Construct(AssetProvider assetProvider) {
        this.assetProvider = assetProvider;
    }

    private SceneInstance prevScene;
    private SceneInstance currentScene;

    public async UniTaskVoid LoadSceneAsync(AssetReference sceneReference) {
        LoadingScreen.instance.Show();

        assetProvider.ClearHandles();

        canAnimateUI = false;
        DOTween.KillAll();

        if (prevScene.Scene.IsValid()) {
        currentScene = await Addressables.LoadSceneAsync(sceneReference, LoadSceneMode.Additive);

        }
        else {
            currentScene = await Addressables.LoadSceneAsync(sceneReference);

        }

        if (prevScene.Scene.IsValid()) {
            await Addressables.UnloadSceneAsync(prevScene);
        }

        prevScene = currentScene;

        canAnimateUI = true;
        Time.timeScale = 1;

        LoadingScreen.instance.Hide();

    }

}