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
    [SerializeField] private GameObject BottomPanel;
    [SerializeField] private float transitionDuration = 0.3f;
    [SerializeField] private Ease easeType = Ease.OutQuint;
    [SerializeField] private List<GameObject> particleEffects;
    public GameObject bottompanel2;
    public GameObject player;

    private int currentPanelIndex = 1;
    private RectTransform panel1Rect, panel2Rect, panel3Rect, panel4Rect,
                         panel5Rect, panel6Rect, panel7Rect;
    private CanvasGroup panel1Group, panel2Group, panel3Group, panel4Group,
                       panel5Group, panel6Group, panel7Group;
    private float slideDistance = 1000f;
    private float fadeDuration = 0.15f;

    void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
        InitializePanels();
        ShowPanel1();
    }

    private void InitializePanels()
    {
        // Initialize RectTransforms
        panel1Rect = GetOrAddComponent<RectTransform>(panel1);
        panel2Rect = GetOrAddComponent<RectTransform>(panel2);
        panel3Rect = GetOrAddComponent<RectTransform>(panel3);
        panel4Rect = GetOrAddComponent<RectTransform>(panel4);
        panel5Rect = GetOrAddComponent<RectTransform>(panel5);
        panel6Rect = GetOrAddComponent<RectTransform>(panel6);
        panel7Rect = GetOrAddComponent<RectTransform>(panel7);

        // Initialize CanvasGroups
        panel1Group = GetOrAddComponent<CanvasGroup>(panel1);
        panel2Group = GetOrAddComponent<CanvasGroup>(panel2);
        panel3Group = GetOrAddComponent<CanvasGroup>(panel3);
        panel4Group = GetOrAddComponent<CanvasGroup>(panel4);
        panel5Group = GetOrAddComponent<CanvasGroup>(panel5);
        panel6Group = GetOrAddComponent<CanvasGroup>(panel6);
        panel7Group = GetOrAddComponent<CanvasGroup>(panel7);
    }

    private T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        if (component == null)
            component = obj.AddComponent<T>();
        return component;
    }

    private void ShowPlayer()
    {
        player.SetActive(true);
    }

    private void HideAllPanels()
    {
        foreach (var particle in particleEffects)
            particle.SetActive(false);
        BottomPanel.SetActive(false);
        bottompanel2.SetActive(false);
    }

    private void AnimatePanel(GameObject inPanel, GameObject outPanel, bool slideFromRight)
    {
        // Get components
        var inRect = inPanel.GetComponent<RectTransform>();
        var outRect = outPanel.GetComponent<RectTransform>();
        var inGroup = inPanel.GetComponent<CanvasGroup>();
        var outGroup = outPanel.GetComponent<CanvasGroup>();

        // Setup initial state
        inPanel.SetActive(true);
        float startX = slideFromRight ? slideDistance : -slideDistance;
        inRect.anchoredPosition = new Vector2(startX, 0);
        inGroup.alpha = 0;

        // Create animation sequence
        Sequence sequence = DOTween.Sequence();

        // Animate in panel
        sequence.Append(inRect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(easeType));
        sequence.Join(inGroup.DOFade(1, fadeDuration));

        // Animate out panel
        float targetX = slideFromRight ? -slideDistance : slideDistance;
        sequence.Join(outRect.DOAnchorPos(new Vector2(targetX, 0), transitionDuration).SetEase(easeType));
        sequence.Join(outGroup.DOFade(0, fadeDuration / 2).OnComplete(() => outPanel.SetActive(false)));
    }

    public void ShowPanel1()
    {
        player.SetActive(false);
        Invoke("ShowPlayer", 0.2f);

        bottompanel2.SetActive(false);
        foreach (var particle in particleEffects)
            particle.SetActive(true);

        BottomPanel.SetActive(true);

        if (currentPanelIndex != 1)
        {
            AnimatePanel(panel1, GetCurrentPanel(), false);
        }
        currentPanelIndex = 1;
    }

    public void ShowPanel2()
    {
        player.SetActive(false);
        Invoke("ShowPlayer", 0.2f);

        HideAllPanels();
        bottompanel2.SetActive(true);

        AnimatePanel(panel2, GetCurrentPanel(), currentPanelIndex < 2);
        currentPanelIndex = 2;
    }

    public void ShowPanel3()
    {
        player.SetActive(false);
        HideAllPanels();

        AnimatePanel(panel3, GetCurrentPanel(), currentPanelIndex < 3);
        currentPanelIndex = 3;
    }

    public void ShowPanel4()
    {
        player.SetActive(false);
        HideAllPanels();

        AnimatePanel(panel4, GetCurrentPanel(), currentPanelIndex < 4);
        currentPanelIndex = 4;
    }

    public void ShowPanel5()
    {
        player.SetActive(false);
        HideAllPanels();

        AnimatePanel(panel5, GetCurrentPanel(), currentPanelIndex < 5);
        currentPanelIndex = 5;
    }

    public void ShowPanel6()
    {
        player.SetActive(false);
        HideAllPanels();

        AnimatePanel(panel6, GetCurrentPanel(), currentPanelIndex < 6);
        currentPanelIndex = 6;
    }

    public void ShowPanel7()
    {
        player.SetActive(false);
        Invoke("ShowPlayer", 0.2f);
        HideAllPanels();

        AnimatePanel(panel7, GetCurrentPanel(), currentPanelIndex < 7);
        currentPanelIndex = 7;
    }

    private GameObject GetCurrentPanel()
    {
        switch (currentPanelIndex)
        {
            case 1: return panel1;
            case 2: return panel2;
            case 3: return panel3;
            case 4: return panel4;
            case 5: return panel5;
            case 6: return panel6;
            case 7: return panel7;
            default: return panel1;
        }
    }

    public void GoBack()
    {
        switch (currentPanelIndex)
        {
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