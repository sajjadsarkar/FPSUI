using UnityEngine;
using UnityEngine.UI;

public class Loadout : MonoBehaviour
{
    [SerializeField] private Image[] loadoutImages;
    [SerializeField] private Button[] loadoutButtons;
    [SerializeField] private Image[] secondaryImages;
    [SerializeField] private Image[] tertiaryImages;  // Third set of images
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite unselectedSprite;
    [SerializeField] private Sprite[] secondarySprites;
    [SerializeField] private Sprite[] tertiarySprites;  // Third set of sprites

    private int currentSelectedIndex = -1;

    void Start()
    {
        for (int i = 0; i < loadoutImages.Length; i++)
        {
            Color imageColor = loadoutImages[i].color;
            imageColor.a = 0f;
            loadoutImages[i].color = imageColor;
            int index = i;
            loadoutButtons[i].onClick.AddListener(() => SelectLoadout(index));
        }

        SelectLoadout(0);
    }

    public void SelectLoadout(int index)
    {
        if (currentSelectedIndex >= 0)
        {
            Color imageColor = loadoutImages[currentSelectedIndex].color;
            imageColor.a = 0f;
            loadoutImages[currentSelectedIndex].color = imageColor;
            loadoutImages[currentSelectedIndex].sprite = unselectedSprite;

            if (secondaryImages != null && secondaryImages.Length > currentSelectedIndex)
            {
                secondaryImages[currentSelectedIndex].sprite = null;
            }

            if (tertiaryImages != null && tertiaryImages.Length > currentSelectedIndex)
            {
                tertiaryImages[currentSelectedIndex].sprite = null;
            }
        }

        currentSelectedIndex = index;
        Color newImageColor = loadoutImages[currentSelectedIndex].color;
        newImageColor.a = 1f;
        loadoutImages[currentSelectedIndex].color = newImageColor;
        loadoutImages[currentSelectedIndex].sprite = selectedSprite;

        if (secondaryImages != null && secondaryImages.Length > index && secondarySprites.Length > index)
        {
            secondaryImages[index].sprite = secondarySprites[index];
        }

        if (tertiaryImages != null && tertiaryImages.Length > index && tertiarySprites.Length > index)
        {
            tertiaryImages[index].sprite = tertiarySprites[index];
        }
    }
}
