using UnityEngine;
using UnityEngine.UI;

public class WeaponLoadout : MonoBehaviour
{
    [SerializeField] private Image[] weaponImages;
    [SerializeField] private Button[] weaponButtons;
    [SerializeField] private Sprite[] selectedSprites;
    [SerializeField] private Sprite[] unselectedSprites;
    [SerializeField] private Image weaponPreviewImage;
    [SerializeField] private Sprite[] previewSprites;

    private int currentSelectedIndex = -1;

    void Start()
    {
        // Initialize all weapons with unselected sprites
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            int index = i; // Capture the index for the lambda
            weaponImages[i].sprite = unselectedSprites[i];
            weaponImages[i].rectTransform.sizeDelta = new Vector2(499, 187); // Set initial size
            weaponButtons[i].onClick.AddListener(() => SelectWeapon(index));
        }

        // Set initial preview if needed
        if (previewSprites.Length > 0)
        {
            weaponPreviewImage.sprite = previewSprites[0];
        }
        SelectWeapon(1);

    }

    void SelectWeapon(int index)
    {
        // Deselect previous weapon if any
        if (currentSelectedIndex >= 0)
        {
            weaponImages[currentSelectedIndex].sprite = unselectedSprites[currentSelectedIndex];
            weaponImages[currentSelectedIndex].rectTransform.sizeDelta = new Vector2(499, 187);
        }

        // Select new weapon
        currentSelectedIndex = index;
        weaponImages[index].sprite = selectedSprites[index];
        weaponImages[index].rectTransform.sizeDelta = new Vector2(589, 277);

        // Update preview image
        weaponPreviewImage.sprite = previewSprites[index];
    }
}
