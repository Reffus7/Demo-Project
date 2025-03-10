
using Project.Config;
using UnityEngine;
using Zenject;

public class BootScene : MonoBehaviour {

    private AssetReferenceContainer assetReferenceContainer;
    private SceneLoader sceneLoader;

    [Inject]
    public void Construct(
        AssetReferenceContainer assetReferenceContainer, 
        SceneLoader sceneLoader
        
    ) {
        this.assetReferenceContainer = assetReferenceContainer;
        this.sceneLoader = sceneLoader;


    }

    private void Start() {
#if UNITY_ANDROID 
        Application.targetFrameRate = 1000;
#endif
        sceneLoader.LoadSceneAsync(assetReferenceContainer.mainMenuScene).Forget();


    }
}