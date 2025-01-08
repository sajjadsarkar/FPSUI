using UnityEngine;
using UnityEngine.UI;

public class SettingPanelGameplay : MonoBehaviour
{
    [SerializeField] private Image[] primaryImages;
    [SerializeField] private Image[] secondaryImages;
    [SerializeField] private Image[] tertiaryImages;
    [SerializeField] private Image[] quaternaryImages;

    [SerializeField] private Button[] primaryButtons;
    [SerializeField] private Button[] secondaryButtons;
    [SerializeField] private Button[] tertiaryButtons;
    [SerializeField] private Button[] quaternaryButtons;

    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite unselectedSprite;

    private int currentPrimaryIndex = -1;
    private int currentSecondaryIndex = -1;
    private int currentTertiaryIndex = -1;
    private int currentQuaternaryIndex = -1;

    void Start()
    {
        // Initialize primary buttons
        for (int i = 0; i < primaryImages.Length; i++)
        {
            primaryImages[i].sprite = unselectedSprite;
            int index = i;
            primaryButtons[i].onClick.AddListener(() => SelectPrimary(index));
        }

        // Initialize secondary buttons
        for (int i = 0; i < secondaryImages.Length; i++)
        {
            secondaryImages[i].sprite = unselectedSprite;
            int index = i;
            secondaryButtons[i].onClick.AddListener(() => SelectSecondary(index));
        }

        // Initialize tertiary buttons
        for (int i = 0; i < tertiaryImages.Length; i++)
        {
            tertiaryImages[i].sprite = unselectedSprite;
            int index = i;
            tertiaryButtons[i].onClick.AddListener(() => SelectTertiary(index));
        }

        // Initialize quaternary buttons
        for (int i = 0; i < quaternaryImages.Length; i++)
        {
            quaternaryImages[i].sprite = unselectedSprite;
            int index = i;
            quaternaryButtons[i].onClick.AddListener(() => SelectQuaternary(index));
        }

        // Set default selections
        SelectPrimary(0);
        SelectSecondary(0);
        SelectTertiary(0);
        SelectQuaternary(0);
    }

    public void SelectPrimary(int index)
    {
        if (currentPrimaryIndex >= 0)
        {
            primaryImages[currentPrimaryIndex].sprite = unselectedSprite;
        }
        currentPrimaryIndex = index;
        primaryImages[currentPrimaryIndex].sprite = selectedSprite;
    }

    public void SelectSecondary(int index)
    {
        if (currentSecondaryIndex >= 0)
        {
            secondaryImages[currentSecondaryIndex].sprite = unselectedSprite;
        }
        currentSecondaryIndex = index;
        secondaryImages[currentSecondaryIndex].sprite = selectedSprite;
    }

    public void SelectTertiary(int index)
    {
        if (currentTertiaryIndex >= 0)
        {
            tertiaryImages[currentTertiaryIndex].sprite = unselectedSprite;
        }
        currentTertiaryIndex = index;
        tertiaryImages[currentTertiaryIndex].sprite = selectedSprite;
    }

    public void SelectQuaternary(int index)
    {
        if (currentQuaternaryIndex >= 0)
        {
            quaternaryImages[currentQuaternaryIndex].sprite = unselectedSprite;
        }
        currentQuaternaryIndex = index;
        quaternaryImages[currentQuaternaryIndex].sprite = selectedSprite;
    }
}