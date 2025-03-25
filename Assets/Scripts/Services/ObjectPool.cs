using System.Collections.Generic;
using UnityEngine;

public class ObjectPool {
    private GameObject prefab;

    private Stack<GameObject> disabledObjectsStack = new();
    private List<GameObject> enabledObjectsList = new();


    private ZenjectInstantiator zenjectInstantiator;
    public ObjectPool(ZenjectInstantiator zenjectInstantiator, GameObject prefab) {
        this.prefab = prefab;
        this.zenjectInstantiator = zenjectInstantiator;
    }

    public GameObject Get() {
        if (disabledObjectsStack.Count == 0) Expand();

        GameObject preparedObj = disabledObjectsStack.Pop();
        enabledObjectsList.Add(preparedObj);
        preparedObj.SetActive(true);

        return preparedObj;

    }

    public void Return(GameObject obj) {
        obj.SetActive(false);
        enabledObjectsList.Remove(obj);
        disabledObjectsStack.Push(obj);
    }

    public void ReturnAll() {
        foreach (GameObject obj in enabledObjectsList) {
            obj.SetActive(false);
            disabledObjectsStack.Push(obj);
        }
        enabledObjectsList.Clear();

    }

    private void Expand() {
        GameObject obj = zenjectInstantiator.Instantiate(prefab);
        obj.SetActive(false);
        disabledObjectsStack.Push(obj);

    }

}