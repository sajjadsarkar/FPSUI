using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private Image[] shopImages;
    [SerializeField] private Button[] shopButtons;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite unselectedSprite;

    private int currentSelectedIndex = -1;

    void Start()
    {
        // Initialize shop items with 0 opacity
        for (int i = 0; i < shopImages.Length; i++)
        {
            Color imageColor = shopImages[i].color;
            imageColor.a = 0f;
            shopImages[i].color = imageColor;
            int index = i;
            shopButtons[i].onClick.AddListener(() => SelectShopItem(index));
        }

        // Default selection
        SelectShopItem(0);
    }

    public void SelectShopItem(int index)
    {
        if (currentSelectedIndex >= 0)
        {
            Color imageColor = shopImages[currentSelectedIndex].color;
            imageColor.a = 0f;
            shopImages[currentSelectedIndex].color = imageColor;
        }

        currentSelectedIndex = index;
        Color newImageColor = shopImages[currentSelectedIndex].color;
        newImageColor.a = 1f;
        shopImages[currentSelectedIndex].color = newImageColor;
        shopImages[currentSelectedIndex].sprite = selectedSprite;
    }
}