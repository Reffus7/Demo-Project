using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class LoadingScreen : MonoBehaviour {
    private CanvasGroup canvasGroup;

    public static LoadingScreen instance;

    private void Awake() {
        instance = this;
        canvasGroup = GetComponent<CanvasGroup>();

        DontDestroyOnLoad(gameObject);
    }

    private bool blockHide = false;

    public void Show(bool setBlock = false) {
        if (setBlock) blockHide = true;

        canvasGroup.alpha = 1;
    }

    public void Hide(bool resetBlock = false) {
        if (resetBlock) blockHide = false;

        if (blockHide) return;

        canvasGroup.alpha = 0;

    }
}