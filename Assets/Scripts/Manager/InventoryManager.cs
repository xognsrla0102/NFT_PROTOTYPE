using UnityEngine;
using System.Collections.Generic;
public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    public static InventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                var inventoryManager = FindObjectOfType<InventoryManager>();
                Debug.Assert(inventoryManager != null);
                instance = inventoryManager;
            }
            return instance;
        }
    }

    public Inventory inventory;

    public Dictionary<string, ItemInfo> items = new Dictionary<string, ItemInfo>();

    private void Awake()
    {
        var inventoryManager = FindObjectsOfType<InventoryManager>();
        if (inventoryManager.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void OnClickOpen()
    {
        if (inventory.gameObject.activeInHierarchy) return;

        GameManager.Instance.SetPlayerControllMode(true);

        inventory.gameObject.SetActive(true);
        inventory.invenPanel.SetActive(true);
        inventory.usePanel.SetActive(false);
    }

    public void OnClickClose()
    {
        GameManager.Instance.SetPlayerControllMode(false);
        inventory.gameObject.SetActive(false);        
    }
}
