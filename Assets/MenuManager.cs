using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject panel1;
    [SerializeField] private GameObject panel2;
    [SerializeField] private GameObject panel3;

    [SerializeField] private GameObject BottomPanel;
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private Ease easeType = Ease.OutQuint;

    private int currentPanelIndex = 1;
    private RectTransform panel1Rect;
    private RectTransform panel2Rect;
    private RectTransform panel3Rect;


    private float slideDistance = 1000f; // Reduced slide distance

    [SerializeField] private List<GameObject> particleEffects;

    public GameObject bottompanel2;
    void Start()
    {
        panel1Rect = panel1.GetComponent<RectTransform>();
        panel2Rect = panel2.GetComponent<RectTransform>();
        panel3Rect = panel3.GetComponent<RectTransform>();
        ShowPanel1();
    }

    public void ShowPanel1()
    {
        bottompanel2.SetActive(false);
        foreach (var particle in particleEffects)
        {
            particle.SetActive(true);
        }
        panel1.SetActive(true);
        BottomPanel.SetActive(true);
        panel1Rect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(easeType);

        if (currentPanelIndex == 2)
        {
            panel2Rect.DOAnchorPos(new Vector2(slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel2.SetActive(false));
        }
        currentPanelIndex = 1;
    }

    public void ShowPanel2()
    {
        bottompanel2.SetActive(true);
        foreach (var particle in particleEffects)
        {
            particle.SetActive(false);
        }
        panel2.SetActive(true);
        BottomPanel.SetActive(false);
        if (currentPanelIndex == 1)
        {
            panel2Rect.anchoredPosition = new Vector2(slideDistance, 0);
            panel2Rect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(easeType);
            panel1Rect.DOAnchorPos(new Vector2(-slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel1.SetActive(false));
        }
        else if (currentPanelIndex == 3)
        {
            panel2Rect.anchoredPosition = new Vector2(-slideDistance, 0);
            panel2Rect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(easeType);
            panel3Rect.DOAnchorPos(new Vector2(slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel3.SetActive(false));
        }
        currentPanelIndex = 2;
    }

    public void ShowPanel3()
    {
        bottompanel2.SetActive(false);
        foreach (var particle in particleEffects)
        {
            particle.SetActive(false);
        }
        panel3.SetActive(true);
        BottomPanel.SetActive(false);
        panel3Rect.anchoredPosition = new Vector2(slideDistance, 0);
        panel3Rect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(easeType);
        panel2Rect.DOAnchorPos(new Vector2(-slideDistance, 0), transitionDuration).SetEase(easeType)
            .OnComplete(() => panel2.SetActive(false));
        currentPanelIndex = 3;
    }

    public void GoBack()
    {
        switch (currentPanelIndex)
        {
            case 3:
                bottompanel2.SetActive(true);
                foreach (var particle in particleEffects)
                {
                    particle.SetActive(false);
                }
                panel2.SetActive(true);
                panel2Rect.anchoredPosition = new Vector2(slideDistance, 0);
                panel2Rect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(easeType);
                panel3Rect.DOAnchorPos(new Vector2(-slideDistance, 0), 0.4f).SetEase(easeType)
                    .OnComplete(() => panel3.SetActive(false));
                currentPanelIndex = 2;
                break;

            case 2:
                panel1.SetActive(true);
                bottompanel2.SetActive(false);
                foreach (var particle in particleEffects)
                {
                    particle.SetActive(true);
                }
                panel1Rect.anchoredPosition = new Vector2(slideDistance, 0);
                panel1Rect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(easeType);
                panel2Rect.DOAnchorPos(new Vector2(-slideDistance, 0), 0.4f).SetEase(easeType)
                    .OnComplete(() => panel2.SetActive(false));
                currentPanelIndex = 1;
                BottomPanel.SetActive(true);

                break;
        }
    }

}
