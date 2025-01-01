using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    [SerializeField] private Image[] shopImages;
    [SerializeField] private Button[] shopButtons;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite unselectedSprite;
    [SerializeField] private Image[] itemImages;
    [SerializeField] private Sprite[] itemSprites;

    private int currentSelectedIndex = -1;
    private Dictionary<int, int[]> buttonItemMapping = new Dictionary<int, int[]>();

    void Start()
    {
        InitializeButtonItems();

        for (int i = 0; i < shopImages.Length; i++)
        {
            Color imageColor = shopImages[i].color;
            imageColor.a = 0f;
            shopImages[i].color = imageColor;
            int index = i;
            shopButtons[i].onClick.AddListener(() => SelectShopItem(index));
        }

        SelectShopItem(0);
    }

    private void InitializeButtonItems()
    {
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < itemSprites.Length; i++)
        {
            availableIndices.Add(i);
        }

        // Assign random items to each button
        for (int buttonIndex = 0; buttonIndex < shopButtons.Length; buttonIndex++)
        {
            int[] itemIndices = new int[itemImages.Length];

            // Get random items for this button
            for (int itemIndex = 0; itemIndex < itemImages.Length; itemIndex++)
            {
                if (availableIndices.Count == 0)
                {
                    // Refill available indices if exhausted
                    for (int i = 0; i < itemSprites.Length; i++)
                    {
                        availableIndices.Add(i);
                    }
                }

                int randomIndex = Random.Range(0, availableIndices.Count);
                itemIndices[itemIndex] = availableIndices[randomIndex];
                availableIndices.RemoveAt(randomIndex);
            }

            buttonItemMapping[buttonIndex] = itemIndices;
        }
    }

    public void SelectShopItem(int index)
    {
        // Reset previous selection
        if (currentSelectedIndex >= 0)
        {
            Color imageColor = shopImages[currentSelectedIndex].color;
            imageColor.a = 0f;
            shopImages[currentSelectedIndex].color = imageColor;
            shopImages[currentSelectedIndex].sprite = unselectedSprite;
        }

        // Set new selection
        currentSelectedIndex = index;
        Color newImageColor = shopImages[currentSelectedIndex].color;
        newImageColor.a = 1f;
        shopImages[currentSelectedIndex].color = newImageColor;
        shopImages[currentSelectedIndex].sprite = selectedSprite;

        // Show saved items for this button
        if (buttonItemMapping.ContainsKey(index))
        {
            int[] itemIndices = buttonItemMapping[index];
            for (int i = 0; i < itemImages.Length; i++)
            {
                itemImages[i].sprite = itemSprites[itemIndices[i]];
            }
        }
    }
}