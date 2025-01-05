using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SocialPlatforms.Impl;
using DG.Tweening;

public class TargetManager : MonoBehaviour
{
    public Target[] allTargets;
    public float hitpoints = 100f;
    public GameObject VIctoryUI;
    public GameObject[] killFeedObjects = new GameObject[5]; // Array of UI text objects
    private int currentKillFeedIndex = 0; private float timer = 0.0f;
    public float duration = 30.0f;
    private int trainingScore = 0;
    private int kills = 0;
    private int headshots = 0;
    [HideInInspector] public int state = 0;

    public AudioSource aSource;
    public AudioClip countdownSound;
    Text timerText;
    public GameObject firstKillObject;
    public GameObject secondKillObject;
    public GameObject thirdKillObject;
    public GameObject fourthKillObject;
    void Start()
    {
        state = 0;
        timerText = CanvasManager.instance.timerText;

        for (int i = 0; i < allTargets.Length; i++)
        {
            allTargets[i].baseHitPoints = hitpoints;
            allTargets[i].trainingMode = false;
            StartCoroutine(allTargets[i].TargetUp());
        }
    }

    public void NextTarget()
    {
        // Filter out destroyed targets
        var remainingTargets = allTargets.Where(t => t != null).ToArray();

        if (remainingTargets.Length > 0)
        {
            StartCoroutine(remainingTargets[Random.Range(0, remainingTargets.Length)].TargetUp());
        }
        else
        {
            // All targets destroyed, end the game
            StartCoroutine(TrainingEnds());
        }
    }
    void StartTraining()
    {
        for (int i = 0; i < allTargets.Length; i++)
        {
            allTargets[i].baseHitPoints = hitpoints;
            allTargets[i].trainingMode = true;
            StartCoroutine(allTargets[i].TargetDown());
        }

        trainingScore = 0;
        headshots = 0;
        kills = 0;
        timer = duration;
        state = 1;
        StartCoroutine(Ready());
    }

    IEnumerator Ready()
    {
        aSource.PlayOneShot(countdownSound, 0.5f);
        yield return new WaitForSeconds(6.0f);
        state = 2;
        NextTarget();
    }

    void Update()
    {
        if (state == 2)
        {
            timer -= Time.deltaTime;

            if (timer <= 0.0f)
            {
                StartCoroutine(TrainingEnds());
            }
        }

        if (state == 1 || state == 2)
        {
            timerText.text = FormatSeconds(timer);
        }
    }

    public void SetScore(int s, bool hs)
    {
        trainingScore += s;
        kills++;
        if (hs) headshots++;
    }

    IEnumerator TrainingEnds()
    {
        state = 3;
        CanvasManager.instance.timerUI.SetActive(false);
        CanvasManager.instance.ShowResult("<color=#88FF6AFF>SCORE :  </color>" + trainingScore, "<color=#88FF6AFF>KILLS :  </color>" + kills, "<color=#88FF6AFF>HEADSHOTS :  </color>" + headshots);

        yield return new WaitForSeconds(10.0f);
        state = 0;
        for (int i = 0; i < allTargets.Length; i++)
        {
            allTargets[i].baseHitPoints = hitpoints;
            allTargets[i].trainingMode = false;
            StartCoroutine(allTargets[i].TargetUp());
        }
    }

    string FormatSeconds(float elapsed)
    {
        int d = (int)(elapsed * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void Action()
    {
        if (state == 0)
        {
            StartTraining();
            CanvasManager.instance.timerUI.SetActive(true);
        }
    }
    public void ShowKillFeed(bool isHeadshot)
    {
        kills++;

        // Hide all currently visible kill feeds
        for (int i = 0; i < killFeedObjects.Length; i++)
        {
            if (i != currentKillFeedIndex && killFeedObjects[i].activeSelf)
            {
                killFeedObjects[i].transform.DOKill();
                killFeedObjects[i].SetActive(false);
            }
        }

        GameObject currentFeed = killFeedObjects[currentKillFeedIndex];

        if (currentFeed != null)
        {
            RectTransform rect = currentFeed.GetComponent<RectTransform>();
            float width = rect.rect.width;

            // Start from current position - width (off-screen to the left)
            Vector3 startPos = currentFeed.transform.localPosition - new Vector3(width, 0f, 0f);
            Vector3 targetPos = currentFeed.transform.localPosition;

            currentFeed.transform.localPosition = startPos;
            Image img = currentFeed.GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
            currentFeed.SetActive(true);

            Sequence sequence = DOTween.Sequence();

            sequence.Append(currentFeed.transform.DOLocalMove(targetPos, 0.5f).SetEase(Ease.OutBack))
                    .Join(img.DOFade(1f, 0.3f))
                    .AppendInterval(2f)
                    .Append(img.DOFade(0f, 0.5f))
                    .OnComplete(() =>
                    {
                        currentFeed.SetActive(false);
                    });

            currentKillFeedIndex = (currentKillFeedIndex + 1) % killFeedObjects.Length;
        }

        switch (kills)
        {
            case 1:
                if (firstKillObject != null)
                    firstKillObject.SetActive(true);
                break;

            case 3:
                if (thirdKillObject != null)
                    secondKillObject.SetActive(true);

                break;
            case 4:
                if (fourthKillObject != null)
                    thirdKillObject.SetActive(true);
                Invoke("SHowWIn", 1.5f);
                Invoke("SHowVictorty", 3f);
                break;
        }
    }
    public void SHowWIn()
    {
        fourthKillObject.SetActive(true);
    }
    public void SHowVictorty()
    {
        VIctoryUI.SetActive(true);
        RectTransform rect = VIctoryUI.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = VIctoryUI.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = VIctoryUI.AddComponent<CanvasGroup>();

        // Initial setup
        rect.anchoredPosition = new Vector2(-Screen.width, 0);
        rect.localScale = Vector3.one;
        canvasGroup.alpha = 0;

        Sequence victorySequence = DOTween.Sequence();

        victorySequence
            // Clean slide in from left
            .Append(rect.DOAnchorPos(Vector2.zero, 0.3f)
                .SetEase(Ease.OutQuint))
            // Smooth fade
            .Join(canvasGroup.DOFade(1, 0.3f));

        // Quick camera shake
        Camera.main.DOShakePosition(0.2f, 0.3f, 20, 90, false);

        victorySequence.SetAutoKill(true);
    }
    private IEnumerator FadeKillFeed(GameObject feedObject)
    {
        // Wait before starting fade
        yield return new WaitForSeconds(3f);

        Image img = feedObject.GetComponent<Image>();
        if (img != null)
        {
            float elapsed = 0f;
            float duration = 1f;
            Color startColor = img.color;

            // Gradually fade out
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                img.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
        }

        // Ensure object is disabled after fade
        feedObject.SetActive(false);
        if (kills == 4)
        {
            VIctoryUI.SetActive(true);
        }
    }
}