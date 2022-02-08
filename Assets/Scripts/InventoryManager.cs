using UnityEngine;

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

    [SerializeField] private GameObject invenPanel;

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
        if (invenPanel.activeInHierarchy) return;

        GameManager.Instance.SetPlayerControllMode(true);
        invenPanel.SetActive(true);
    }

    public void OnClickClose()
    {
        GameManager.Instance.SetPlayerControllMode(false);
        invenPanel.SetActive(false);
    }
}
