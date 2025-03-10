
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour {
    private TextMeshProUGUI text;
    private float deltaTime = 0.0f;
    private float updateInterval = 0.25f; 
    private float timeSinceLastUpdate = 0.0f;

    void Start() {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        timeSinceLastUpdate += Time.unscaledDeltaTime;

        if (timeSinceLastUpdate >= updateInterval) {
            float fps = 1.0f / deltaTime;
            text.text = "FPS: " + Mathf.Ceil(fps).ToString();
            timeSinceLastUpdate = 0.0f;
        }
    }
}
