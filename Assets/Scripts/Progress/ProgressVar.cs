using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Progress {
    [Serializable]
    public class ProgressVar {
        public enum CalculationType { Linear, Exponential, Manual }

        [SerializeField] protected CalculationType calculationMethod;
        [SerializeField] protected float baseValue;
        [SerializeField] protected float coeff;
        [SerializeField] protected float minValue = float.MinValue;
        [SerializeField] protected float maxValue = float.MaxValue;
        [SerializeField] protected bool isDecreasing;
        [SerializeField] protected List<float> manualValues;

        protected int level;

        public float Value {
            get {
                return calculationMethod switch {
                    CalculationType.Linear => CalculateLinear(),
                    CalculationType.Exponential => CalculateExponential(),
                    CalculationType.Manual => CalculateManual(),
                    _ => baseValue
                };
            }
        }

        private float CalculateLinear() {
            float result = isDecreasing
                ? baseValue - level * coeff
                : baseValue + level * coeff;

            return Mathf.Clamp(result, minValue, maxValue);
        }

        private float CalculateExponential() {
            if (coeff <= 1) {
                Debug.LogWarning("Coeff should be greater than 1 for exponential values.");
            }

            float result = isDecreasing
                ? baseValue / Mathf.Pow(coeff, level)
                : baseValue * Mathf.Pow(coeff, level);

            return Mathf.Clamp(result, minValue, maxValue);
        }

        private float CalculateManual() {
            if (manualValues == null || manualValues.Count == 0) {
                Debug.LogWarning("Manual values list is empty.");
                return baseValue;
            }
            return level >= manualValues.Count ? manualValues[^1] : manualValues[level];
        }
    }
}