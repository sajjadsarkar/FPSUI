using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PopWithGlitch : MonoBehaviour
{
    [Header("Pop Settings")]
    private float glitchDuration = 0.5f;
    private float glitchIntensity = 0.03f;
    private float glitchCount = 0.02f;
    [SerializeField] private float startDelay = 0.5f;
    private float slideInDuration = 0.1f;

    [Header("Visual Glitch Settings")]
    [SerializeField] private Material glitchMaterial;
    private float colorGlitchIntensity = 0.02f;
    private float scanLineJitter = 0.02f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Image image;
    private Material instanceMaterial;
    private Vector2 initialPosition;

    private Ease easeType = Ease.OutBack;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
        initialPosition = rectTransform.anchoredPosition;

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (image == null)
            image = gameObject.AddComponent<Image>();

        if (glitchMaterial != null)
        {
            instanceMaterial = new Material(glitchMaterial);
            image.material = instanceMaterial;
        }

        // Hide initially
        canvasGroup.alpha = 0f;
        rectTransform.anchoredPosition = initialPosition + Vector2.right * 50f;
    }

    private void OnEnable()
    {
        StartSequence();
    }

    public void StartSequence()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(startDelay);
        sequence.Append(rectTransform.DOAnchorPos(initialPosition, slideInDuration).SetEase(easeType));
        sequence.Join(canvasGroup.DOFade(1f, slideInDuration));
        sequence.Join(DOTween.Sequence().AppendCallback(() => PlayPopGlitchEffect()));

        sequence.Play();
    }

    private void PlayPopGlitchEffect()
    {
        rectTransform.localScale = Vector3.one;
        canvasGroup.alpha = 1f;

        Sequence glitchSequence = DOTween.Sequence();

        for (int i = 0; i < glitchCount; i++)
        {
            float glitchTime = Random.Range(0f, glitchDuration);

            glitchSequence.Insert(glitchTime, rectTransform.DOAnchorPos(
                initialPosition + Random.insideUnitCircle * glitchIntensity * 70f, 0.05f)
                .SetEase(Ease.InOutBack)
                .SetLoops(2, LoopType.Yoyo));

            if (instanceMaterial != null)
            {
                glitchSequence.Insert(glitchTime, DOTween.To(() => 0f,
                    x => instanceMaterial.SetFloat("_RGBSplit", x),
                    colorGlitchIntensity * 1.5f, 0.05f).SetLoops(2, LoopType.Yoyo));

                glitchSequence.Insert(glitchTime, DOTween.To(() => 0f,
                    x => instanceMaterial.SetFloat("_ScanLineJitter", x),
                    scanLineJitter * 1.5f, 0.05f).SetLoops(2, LoopType.Yoyo));
            }
        }

        glitchSequence.Play();
        Invoke(nameof(ResetGlitchEffect), glitchDuration);
    }

    private void ResetGlitchEffect()
    {
        if (instanceMaterial != null)
        {
            instanceMaterial.SetFloat("_RGBSplit", 0f);
            instanceMaterial.SetFloat("_ScanLineJitter", 0f);
        }
        rectTransform.anchoredPosition = initialPosition;
        rectTransform.localScale = Vector3.one;
    }

    private void OnDestroy()
    {
        if (instanceMaterial != null)
        {
            Destroy(instanceMaterial);
        }
    }
}
