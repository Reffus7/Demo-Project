using Cysharp.Threading.Tasks;
using Project.Config;
using Project.Data;
using Project.Factory;
using Project.Map;
using Project.Player;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Project.Init {

    public class GameInstaller : MonoInstaller {
        [SerializeField] private AssetReferenceContainer assetReferenceContainer;

        [SerializeField] private Projectile projectilePrefab;
        //[SerializeField] private PlayerController playerPrefab;
        //[SerializeField] private RectTransform playerUiPrefab;

        [SerializeField] private Canvas canvas;

        public override void InstallBindings() {
            //from instance
            Container.Bind<Projectile>().FromInstance(projectilePrefab).AsSingle();
            //Container.Bind<AssetReferenceContainer>().FromInstance(assetReferenceContainer).AsSingle();
            Container.Bind<Canvas>().FromInstance(canvas).AsSingle();

            //have zenject interfaces
            Container.BindInterfacesAndSelfTo<LevelController>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapGenerator>().AsSingle();
            Container.BindInterfacesAndSelfTo<RoomGenerator>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerExperience>().AsSingle().NonLazy();


            //simple bind
            Container.Bind<GameInput>().AsSingle();
            //Container.Bind<AssetProvider>().AsSingle();
            Container.Bind<ZenjectInstantiator>().AsSingle();


            //my interfaces
            Container.Bind<IInputHandler>().To<DesktopInputHandler>().AsSingle();
            Container.Bind<IDataSaver>().To<DataSaver>().AsSingle();

            //factories

            Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerFactory>().AsSingle();

            Container.Bind<EnemyProgressVarFactory>().AsSingle();


            Container.Bind<CancellationToken>().FromInstance(this.GetCancellationTokenOnDestroy()).AsSingle();


        }



    }
}