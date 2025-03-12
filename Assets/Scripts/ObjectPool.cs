using System.Collections.Generic;
using UnityEngine;

public class ObjectPool {
    // карта, снаряды, враги, иконки мини карты
    private GameObject prefab;

    private Stack<GameObject> disabledObjectsStack = new();
    private List<GameObject> enabledObjectsList = new();


    private ZenjectInstantiator zenjectInstantiator;
    public ObjectPool(ZenjectInstantiator zenjectInstantiator, GameObject prefab) {
        this.prefab = prefab;
        this.zenjectInstantiator = zenjectInstantiator;
    }

    public GameObject Get() {
        //Debug.Log(GetHashCode() + " " + disabledObjectsStack.Count);
        //Debug.Log(GetHashCode() + " " + enabledObjectsList.Count);
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