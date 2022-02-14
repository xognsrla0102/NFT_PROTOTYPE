using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public RectTransform itemInfoRect;
    public GameObject invenPanel;
    public GameObject itemInfoPanel;
    public GameObject usePanel;
    public GameObject useBtn;
    public GameObject takeBtn;
    [SerializeField] private RectTransform contents;

    [HideInInspector] public bool itemChangeDetected;
    private List<Slot> slots = new List<Slot>();
    private int selSlotIdx;

    private Transform holdItemPos;
    private GameObject holdingItem;

    private void Start()
    {
        holdItemPos = GameObject.Find("HoldItemPos").transform;
    }

    private void OnEnable()
    {
        //if (itemChangeDetected)
        //{
            LoadSlot();
        //}

        // 다시 들어오면 스크롤 위치 초기화
        contents.anchoredPosition = new Vector2(contents.anchoredPosition.x, 0);

        // 임시 조건.. 처음부터 로딩하는 중에 인벤토리 열어서 outofrange남
        if (slots.Count > 0)
        {
            // 아이템 정보 창이 비활성화 된 경우만 활성화
            if (itemInfoPanel.activeInHierarchy == false)
            {
                itemInfoPanel.SetActive(true);
            }

            // 선택 슬롯 초기화
            selSlotIdx = 0;
            slots[selSlotIdx].OnClick();
        }
        else
        {
            // 인벤토리 아무 것도 없을 경우
            itemInfoPanel.SetActive(false);
        }
    }

    private void LoadSlot()
    {
        // 기존 슬롯 다 지우고
        foreach (var slot in slots)
        {
            Destroy(slot.gameObject);
        }
        slots.Clear();

        // 보유한 item 슬롯 추가
        var items = InventoryManager.Instance.items;

        foreach (var item in items)
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/Slot");
            Slot slot = Instantiate(obj, contents).GetComponent<Slot>();
            slots.Add(slot);
            slot.tokenID = item.Value.tokenID;
            slot.itemName.text = item.Value.itemName;
            slot.image.texture = item.Value.texture;
            slot.selectedImg.SetActive(false);
        }
    }

    public void SelectSlot(Slot selectedSlot)
    {
        slots[selSlotIdx].selectedImg.SetActive(false);
        selSlotIdx = slots.FindIndex(slot => slot == selectedSlot);
        slots[selSlotIdx].selectedImg.SetActive(true);
    }

    public void OnClickUse()
    {
        invenPanel.SetActive(false);
        usePanel.SetActive(true);

        // 선택한 NFT의 정보로 오브젝트 소환.
        ItemInfo itemInfo = InventoryManager.Instance.items[slots[selSlotIdx].tokenID];

        GameObject cube = Resources.Load<GameObject>("Prefabs/Cube");
        holdingItem = Instantiate(cube, holdItemPos).gameObject;
        holdingItem.GetComponent<Renderer>().material.mainTexture = itemInfo.texture;
        holdingItem.name = "HoldingItem";
    }

    public void OnClickTake()
    {
        ItemInfo itemInfo = InventoryManager.Instance.items[slots[selSlotIdx].tokenID];
        itemInfo.worldInfo.isPut = false;

        itemInfo.worldInfo.posX = 0;
        itemInfo.worldInfo.posY = 0;
        itemInfo.worldInfo.posZ = 0;

        itemInfo.worldInfo.rotX = 0;
        itemInfo.worldInfo.rotY = 0;
        itemInfo.worldInfo.rotZ = 0;

        Destroy(GameObject.Find(itemInfo.itemName));

        slots[selSlotIdx].SettingItemInfo();
        
        Mock.SaveWorldObjectInfos();
    }

    public void OnClickPutInUseMode()
    {
        ItemInfo itemInfo = InventoryManager.Instance.items[slots[selSlotIdx].tokenID];

        holdingItem.transform.SetParent(GameObject.Find("Environment").transform);
        itemInfo.worldInfo.isPut = true;

        holdingItem.name = itemInfo.itemName;

        itemInfo.worldInfo.posX = holdItemPos.position.x;
        itemInfo.worldInfo.posY = holdItemPos.position.y;
        itemInfo.worldInfo.posZ = holdItemPos.position.z;

        itemInfo.worldInfo.rotX = holdItemPos.eulerAngles.x;
        itemInfo.worldInfo.rotY = holdItemPos.eulerAngles.y;
        itemInfo.worldInfo.rotZ = holdItemPos.eulerAngles.z;

        Mock.SaveWorldObjectInfos();

        InventoryManager.Instance.OnClickClose();
    }

    public void OnClickCancelInUseMode()
    {
        Destroy(holdingItem);
        holdingItem = null;
        invenPanel.SetActive(true);
        usePanel.SetActive(false);
    }
}
