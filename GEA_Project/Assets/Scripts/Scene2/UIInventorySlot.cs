using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventorySlot : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text countText;

    public void SetItem(Sprite icon, int count)
    {
        iconImage.enabled = (icon != null);
        iconImage.sprite = icon;
        countText.text = count > 1 ? count.ToString() : "";
    }

    public void Clear()
    {
        iconImage.enabled = false;
        countText.text = "";
    }
}