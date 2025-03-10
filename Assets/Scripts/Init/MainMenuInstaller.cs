using Zenject;
using UnityEngine;
using Project.Config;
using Project.Data;

namespace Project.Init {
    public class MainMenuInstaller : MonoInstaller {
        [SerializeField] private PlayerConfig playerConfig;
        public override void InstallBindings() {
            Container.Bind<PlayerConfig>().FromInstance(playerConfig).AsSingle();

            Container.Bind<IDataSaver>().To<DataSaver>().AsSingle();
        }
    }
}