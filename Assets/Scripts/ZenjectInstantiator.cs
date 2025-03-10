using UnityEngine;
using Zenject;

public class ZenjectInstantiator {

    private DiContainer diContainer;

    [Inject]
    public void Construct(DiContainer diContainer) {
        this.diContainer = diContainer;
    }

    public GameObject Instantiate(Object prefab, Transform parent = null) {
        return diContainer.InstantiatePrefab(prefab, parent);
    }

    public void Inject(object injectable) {
        diContainer.Inject(injectable);
    }

}