using UnityEngine;
using UnityEngine.UI;

public class Loadout : MonoBehaviour
{
    [SerializeField] private Image[] loadoutImages;
    [SerializeField] private Button[] loadoutButtons;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite unselectedSprite;

    private int currentSelectedIndex = -1;

    void Start()
    {
        // Setup buttons and initial visual states
        for (int i = 0; i < loadoutImages.Length; i++)
        {
            Color imageColor = loadoutImages[i].color;
            imageColor.a = 0f;
            loadoutImages[i].color = imageColor;
            int index = i;
            loadoutButtons[i].onClick.AddListener(() => SelectLoadout(index));
        }

        // Select first loadout by default
        SelectLoadout(0);
    }

    public void SelectLoadout(int index)
    {
        // Reset previous selection
        if (currentSelectedIndex >= 0)
        {
            Color imageColor = loadoutImages[currentSelectedIndex].color;
            imageColor.a = 0f;
            loadoutImages[currentSelectedIndex].color = imageColor;
            loadoutImages[currentSelectedIndex].sprite = unselectedSprite;
        }

        // Apply new selection
        currentSelectedIndex = index;
        Color newImageColor = loadoutImages[currentSelectedIndex].color;
        newImageColor.a = 1f;
        loadoutImages[currentSelectedIndex].color = newImageColor;
        loadoutImages[currentSelectedIndex].sprite = selectedSprite;
    }
}