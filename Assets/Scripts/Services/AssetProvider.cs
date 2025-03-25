using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class AssetProvider {
    private readonly List<AsyncOperationHandle> handleList = new();

    public async UniTask<TAsset> LoadAssetAsync<TAsset>(AssetReference reference) where TAsset : class {
        AsyncOperationHandle<TAsset> handle = Addressables.LoadAssetAsync<TAsset>(reference);
        handleList.Add(handle);

        TAsset asset = await handle.ToUniTask();

        return asset;

    }

    public void ClearHandles() {
        foreach (AsyncOperationHandle handle in handleList) {
            if (handle.IsValid()) {
                Addressables.Release(handle);
            }
        }

    }

}
