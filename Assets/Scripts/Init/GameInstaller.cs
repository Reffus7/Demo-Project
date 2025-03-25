using Cysharp.Threading.Tasks;
using Project.Data;
using Project.Factory;
using Project.Input;
using Project.Map;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Project.Init {

    public class GameInstaller : MonoInstaller {
        [SerializeField] private Canvas canvas;

        public override void InstallBindings() {
            Container.Bind<Canvas>().FromInstance(canvas).AsSingle();  


            Container.BindInterfacesAndSelfTo<LevelController>().AsSingle();
            Container.BindInterfacesAndSelfTo<MapGenerator>().AsSingle();
            Container.BindInterfacesAndSelfTo<RoomGenerator>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerExperience>().AsSingle().NonLazy();


            Container.Bind<GameInput>().AsSingle();
            Container.Bind<ZenjectInstantiator>().AsSingle();


#if UNITY_EDITOR
            Container.Bind<IInputHandler>().To<DesktopInputHandler>().AsSingle();
#elif UNITY_ANDROID
            Container.Bind<IInputHandler>().To<MobileInputHandler>().AsSingle();
#else
            Container.Bind<IInputHandler>().To<DesktopInputHandler>().AsSingle();

#endif
            
            Container.Bind<IDataSaver>().To<DataSaver>().AsSingle();

            Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileFactory>().AsSingle();

            Container.Bind<EnemyProgressVarFactory>().AsSingle();


            Container.Bind<CancellationToken>().FromInstance(this.GetCancellationTokenOnDestroy()).AsSingle();


        }



    }
}