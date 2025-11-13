using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public UIInventorySlot[] slots;

    [Header("아이콘 스프라이트 직접 연결")]
    public Sprite dirtIcon;
    public Sprite grassIcon;
    public Sprite waterIcon;

    private Dictionary<BlockType, Sprite> icons = new();
    private Inventory _inventory;

    void Awake()
    {
        icons[BlockType.Dirt] = dirtIcon;
        icons[BlockType.Grass] = grassIcon;
        icons[BlockType.Water] = waterIcon;
    }

    void Start()
    {
        _inventory = FindObjectOfType<Inventory>();
        if (_inventory != null)
        {
            _inventory.OnChanged += RefreshUI;
            RefreshUI();
        }
        else
        {
            Debug.LogError("[UIInventory] Inventory not found in scene!");
        }
    }

    public void RefreshUI()
    {
        if (_inventory == null) return;

        int i = 0;
        foreach (var kvp in _inventory.items)
        {
            if (i >= slots.Length) break;

            if (kvp.Value <= 0)
            {
                slots[i].Clear();
            }
            else
            {
                Sprite icon = icons.ContainsKey(kvp.Key) ? icons[kvp.Key] : null;
                slots[i].SetItem(icon, kvp.Value);
            }
            i++;
        }

        for (; i < slots.Length; i++)
            slots[i].Clear();
    }
}