using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject panel1;
    [SerializeField] private GameObject panel2;
    [SerializeField] private GameObject panel3;
    [SerializeField] private GameObject panel4;
    [SerializeField] private GameObject panel5;
    [SerializeField] private GameObject panel6;
    [SerializeField] private GameObject panel7;
    [SerializeField] private GameObject panel8;
    [SerializeField] private GameObject BottomPanel;
    [SerializeField] private float transitionDuration = 0.5f;
    [SerializeField] private Ease easeType = Ease.OutQuint;

    private int currentPanelIndex = 1;
    private RectTransform panel1Rect;
    private RectTransform panel2Rect;
    private RectTransform panel3Rect;
    private RectTransform panel4Rect;
    private RectTransform panel5Rect;
    private RectTransform panel6Rect;
    private RectTransform panel7Rect;
    private RectTransform panel8Rect;

    private float slideDistance = 1000f;

    [SerializeField] private List<GameObject> particleEffects;
    public GameObject bottompanel2;
    public GameObject player;

    void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        panel1Rect = panel1.GetComponent<RectTransform>();
        panel2Rect = panel2.GetComponent<RectTransform>();
        panel3Rect = panel3.GetComponent<RectTransform>();
        panel4Rect = panel4.GetComponent<RectTransform>();
        panel5Rect = panel5.GetComponent<RectTransform>();
        panel6Rect = panel6.GetComponent<RectTransform>();
        panel7Rect = panel7.GetComponent<RectTransform>();
        panel8Rect = panel8.GetComponent<RectTransform>();
        ShowPanel1();
    }

    private void ShowPlayer()
    {
        player.SetActive(true);
    }

    private void HideAllPanels()
    {
        foreach (var particle in particleEffects)
        {
            particle.SetActive(false);
        }
        BottomPanel.SetActive(false);
        bottompanel2.SetActive(false);
    }

    public void ShowPanel1()
    {
        player.SetActive(false);
        Invoke("ShowPlayer", 0.3f);
        HideAllPanels();
        panel1.SetActive(true);
        panel1Rect.anchoredPosition = new Vector2(slideDistance, 0);
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
        player.SetActive(false);
        Invoke("ShowPlayer", 0.3f);
        HideAllPanels();
        panel2.SetActive(true);
        panel2Rect.anchoredPosition = new Vector2(slideDistance, 0);
        panel2Rect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(easeType);

        if (currentPanelIndex == 1)
        {
            panel1Rect.DOAnchorPos(new Vector2(-slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel1.SetActive(false));
        }
        else if (currentPanelIndex == 3)
        {
            panel3Rect.DOAnchorPos(new Vector2(slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel3.SetActive(false));
        }
        currentPanelIndex = 2;
    }

    public void ShowPanel3()
    {
        player.SetActive(false);
        Invoke("ShowPlayer", 0.3f);
        HideAllPanels();
        panel3.SetActive(true);
        panel3Rect.anchoredPosition = new Vector2(slideDistance, 0);
        panel3Rect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(easeType);

        if (currentPanelIndex == 2)
        {
            panel2Rect.DOAnchorPos(new Vector2(-slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel2.SetActive(false));
        }
        else if (currentPanelIndex == 4)
        {
            panel4Rect.DOAnchorPos(new Vector2(slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel4.SetActive(false));
        }
        currentPanelIndex = 3;
    }

    public void ShowPanel4()
    {
        player.SetActive(false);
        Invoke("ShowPlayer", 0.3f);
        HideAllPanels();
        panel4.SetActive(true);
        panel4Rect.anchoredPosition = new Vector2(slideDistance, 0);
        panel4Rect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(easeType);

        if (currentPanelIndex == 3)
        {
            panel3Rect.DOAnchorPos(new Vector2(-slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel3.SetActive(false));
        }
        else if (currentPanelIndex == 5)
        {
            panel5Rect.DOAnchorPos(new Vector2(slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel5.SetActive(false));
        }
        currentPanelIndex = 4;
    }

    public void ShowPanel5()
    {
        player.SetActive(false);
        Invoke("ShowPlayer", 0.3f);
        HideAllPanels();
        panel5.SetActive(true);
        panel5Rect.anchoredPosition = new Vector2(slideDistance, 0);
        panel5Rect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(easeType);

        if (currentPanelIndex == 4)
        {
            panel4Rect.DOAnchorPos(new Vector2(-slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel4.SetActive(false));
        }
        else if (currentPanelIndex == 6)
        {
            panel6Rect.DOAnchorPos(new Vector2(slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel6.SetActive(false));
        }
        currentPanelIndex = 5;
    }

    public void ShowPanel6()
    {
        player.SetActive(false);
        Invoke("ShowPlayer", 0.3f);
        HideAllPanels();
        panel6.SetActive(true);
        panel6Rect.anchoredPosition = new Vector2(slideDistance, 0);
        panel6Rect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(easeType);

        if (currentPanelIndex == 5)
        {
            panel5Rect.DOAnchorPos(new Vector2(-slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel5.SetActive(false));
        }
        else if (currentPanelIndex == 7)
        {
            panel7Rect.DOAnchorPos(new Vector2(slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel7.SetActive(false));
        }
        currentPanelIndex = 6;
    }

    public void ShowPanel7()
    {
        player.SetActive(false);
        Invoke("ShowPlayer", 0.3f);
        HideAllPanels();
        panel7.SetActive(true);
        panel7Rect.anchoredPosition = new Vector2(slideDistance, 0);
        panel7Rect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(easeType);

        if (currentPanelIndex == 6)
        {
            panel6Rect.DOAnchorPos(new Vector2(-slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel6.SetActive(false));
        }
        else if (currentPanelIndex == 8)
        {
            panel8Rect.DOAnchorPos(new Vector2(slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel8.SetActive(false));
        }
        currentPanelIndex = 7;
    }

    public void ShowPanel8()
    {
        player.SetActive(false);
        Invoke("ShowPlayer", 0.3f);
        HideAllPanels();
        panel8.SetActive(true);
        panel8Rect.anchoredPosition = new Vector2(slideDistance, 0);
        panel8Rect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(easeType);

        if (currentPanelIndex == 7)
        {
            panel7Rect.DOAnchorPos(new Vector2(-slideDistance, 0), transitionDuration).SetEase(easeType)
                .OnComplete(() => panel7.SetActive(false));
        }
        currentPanelIndex = 8;
    }

    public void GoBack()
    {
        switch (currentPanelIndex)
        {
            case 8:
            case 7:
            case 6:
            case 5:
            case 4:
                ShowPanel1();
                break;
            case 3:
                ShowPanel2();
                break;
            case 2:
                ShowPanel1();
                break;
        }
    }
}