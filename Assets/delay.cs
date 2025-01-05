using UnityEngine;
using DG.Tweening;

public class delay : MonoBehaviour
{
    public float delayTime = 3f;
    public GameObject targetObject;
    public float slideDistance = 500f; // How far to slide up
    public float slideDuration = 1f;
    public Ease easeType = Ease.OutBack;

    private float timer = 0f;
    private bool isShown = false;
    private Vector3 originalPosition;

    void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
            originalPosition = targetObject.transform.localPosition;
            // Set initial position below screen
            targetObject.transform.localPosition = originalPosition + new Vector3(0, -slideDistance, 0);
        }
    }

    void Update()
    {
        if (!isShown)
        {
            timer += Time.deltaTime;

            if (timer >= delayTime)
            {
                if (targetObject != null)
                {
                    ShowWithAnimation();
                    isShown = true;
                }
            }
        }
    }

    void ShowWithAnimation()
    {
        targetObject.SetActive(true);
        targetObject.transform.DOLocalMove(originalPosition, slideDuration)
            .SetEase(easeType)
            .OnComplete(() =>
            {
                // Animation completed callback if needed
            });
    }

    private void OnDestroy()
    {
        DOTween.Kill(targetObject.transform);
    }
}
