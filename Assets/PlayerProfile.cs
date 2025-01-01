using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerProfile : MonoBehaviour
{
    [System.Serializable]
    public class CounterText
    {
        public TextMeshProUGUI textComponent;
        public int targetValue;
    }

    [SerializeField] private List<CounterText> counters = new List<CounterText>();
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private float updateStep = 0.05f;

    void OnEnable()
    {
        StartAllCounters();
    }

    void StartAllCounters()
    {
        foreach (var counter in counters)
        {
            StartCoroutine(AnimateCounter(counter.textComponent, counter.targetValue));
        }
    }

    IEnumerator AnimateCounter(TextMeshProUGUI textComponent, int endValue)
    {
        float elapsedTime = 0;
        int startValue = 0;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += updateStep;
            float progress = elapsedTime / animationDuration;
            int currentValue = (int)Mathf.Lerp(startValue, endValue, progress);
            textComponent.text = currentValue.ToString();
            yield return new WaitForSeconds(updateStep);
        }

        // Ensure we end up with the exact target value
        textComponent.text = endValue.ToString();
    }
}