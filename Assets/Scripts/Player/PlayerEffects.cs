using Cysharp.Threading.Tasks;
using Project.HealthSpace;
using UnityEngine;

namespace Project.Player {

    public class PlayerEffects : MonoBehaviour {

        PlayerHealth playerHealth;
        SkinnedMeshRenderer[] skins;

        public Color glowColor = Color.yellow;
        public float minGlow = 0.2f;
        public float maxGlow = 0.5f;
        public float glowSpeed = 2f;

        private bool isGlowActive = false;

        private void Start() {
            playerHealth = GetComponent<PlayerHealth>();
            playerHealth.OnInvincibilityChanged += GlowEffect;

            skins = GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        private void OnDestroy() {
            playerHealth.OnInvincibilityChanged -= GlowEffect;
        }

        private void GlowEffect(bool isInvincible) {
            if (isInvincible) {
                isGlowActive = true;
                foreach (SkinnedMeshRenderer skin in skins) {
                    GlowEffectAsync(skin.material).Forget();
                }

            }
            else {
                isGlowActive = false;

            }
        }

        private async UniTaskVoid GlowEffectAsync(Material mat) {
            mat.EnableKeyword("_EMISSION");

            while (isGlowActive) {
                float emissionStrength = Mathf.Lerp(minGlow, maxGlow, (Mathf.Sin(Time.time * glowSpeed) + 1) / 2);
                mat.SetColor("_EmissionColor", glowColor * emissionStrength);

                await UniTask.Yield();
            }

            mat.SetColor("_EmissionColor", Color.black);
        }

    }
}