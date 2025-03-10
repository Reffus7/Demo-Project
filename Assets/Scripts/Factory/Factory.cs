using Zenject;

namespace Project.Factory {
    public abstract class Factory {

        protected ZenjectInstantiator zenjectInstantiator;

        [Inject]
        public void Construct(ZenjectInstantiator zenjectInstantiator) {
            this.zenjectInstantiator = zenjectInstantiator;
        }

    }
}