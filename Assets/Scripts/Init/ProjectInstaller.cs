using Zenject;
using UnityEngine;
using Project.Config;

namespace Project.Init {
    public class ProjectInstaller : MonoInstaller {
        [SerializeField] private AssetReferenceContainer assetReferenceContainer;

        public override void InstallBindings() {
            Container.Bind<AssetReferenceContainer>().FromInstance(assetReferenceContainer).AsSingle();

            Container.Bind<AssetProvider>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
        }

    }
}