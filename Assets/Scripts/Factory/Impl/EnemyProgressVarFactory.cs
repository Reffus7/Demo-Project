using Project.Progress;

namespace Project.Factory {

    public class EnemyProgressVarFactory : Factory {
        public EnemyProgressVar Create(EnemyProgressVar config) {
            EnemyProgressVar progressVar = config;
            zenjectInstantiator.Inject(progressVar);
            return progressVar;
        }
    }
}