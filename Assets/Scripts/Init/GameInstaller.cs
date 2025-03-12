using Cysharp.Threading.Tasks;
using Project.Data;
using Project.Factory;
using Project.Map;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Project.Init {

    public class GameInstaller : MonoInstaller {

        [SerializeField] private Projectile projectilePrefab;

        [SerializeField] private Canvas canvas;

        public override void InstallBindings() {
            //from instance
            Container.Bind<Projectile>().FromInstance(projectilePrefab).AsSingle();
            Container.Bind<Canvas>().FromInstance(canvas).AsSingle();  

            //have zenject interfaces
            Container.BindInterfacesAndSelfTo<LevelController>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapGenerator>().AsSingle();
            Container.BindInterfacesAndSelfTo<RoomGenerator>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerExperience>().AsSingle().NonLazy();


            //simple bind
            Container.Bind<GameInput>().AsSingle();
            Container.Bind<ZenjectInstantiator>().AsSingle();


            //my interfaces
#if UNITY_ANDROID
            Container.Bind<IInputHandler>().To<MobileInputHandler>().AsSingle();
            //Container.Bind<IInputHandler>().To<DesktopInputHandler>().AsSingle();

#else
            Container.Bind<IInputHandler>().To<DesktopInputHandler>().AsSingle();

#endif
            Container.Bind<IDataSaver>().To<DataSaver>().AsSingle();

            //factories

            Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileFactory>().AsSingle();

            Container.Bind<EnemyProgressVarFactory>().AsSingle();


            Container.Bind<CancellationToken>().FromInstance(this.GetCancellationTokenOnDestroy()).AsSingle();


        }



    }
}