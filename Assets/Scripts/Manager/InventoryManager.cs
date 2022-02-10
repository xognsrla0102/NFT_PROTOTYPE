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

    public GameObject mainUIBtn;

    public Dictionary<string, ItemInfo> items;

    private void Awake()
    {
        var inventoryManager = FindObjectsOfType<InventoryManager>();
        if (inventoryManager.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        items = Mock.items;

        #region 월드 오브젝트 생성
        foreach (var worldInfo in Mock.worldObjectInfos)
        {
            ItemInfo itemInfo = items[worldInfo.tokenID];
            GameObject house = Resources.Load<GameObject>($"Prefabs/Houses/{itemInfo.description}");

            GameObject obj = Instantiate(house, GameObject.Find("Environment").transform);
            obj.name = itemInfo.itemName;
            obj.transform.SetPositionAndRotation(
                new Vector3(itemInfo.worldInfo.posX, itemInfo.worldInfo.posY, itemInfo.worldInfo.posZ),
                Quaternion.Euler(new Vector3(itemInfo.worldInfo.rotX, itemInfo.worldInfo.rotY, itemInfo.worldInfo.rotZ))
                );
        }
        #endregion

        mainUIBtn.SetActive(GameManager.Instance.isAndroidMode);
    }

    public void OnClickOpen()
    {
        if (inventory.gameObject.activeInHierarchy) return;

        GameManager.Instance.SetPlayerControllMode(true);

        inventory.gameObject.SetActive(true);
        inventory.invenPanel.SetActive(true);
        inventory.usePanel.SetActive(false);
        mainUIBtn.SetActive(false);
    }

    public void OnClickClose()
    {
        GameManager.Instance.SetPlayerControllMode(false);
        inventory.gameObject.SetActive(false);

        if (GameManager.Instance.isAndroidMode)
        {
            mainUIBtn.SetActive(true);
        }
    }
}
