using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private RectTransform contents;
    public RectTransform itemInfoRect;
    public GameObject invenPanel;
    public GameObject itemInfoPanel;
    public GameObject usePanel;

    [HideInInspector] public bool itemChangeDetected;
    private List<Slot> slots = new List<Slot>();
    private int selSlotIdx;

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
            slot.name = slot.name.Replace("(Clone)", $"{slots.Count}");
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
        invenPanel.gameObject.SetActive(false);
        usePanel.SetActive(true);
    }

    public void OnClickPutInUseMode()
    {
        InventoryManager.Instance.OnClickClose();
    }

    public void OnClickCancleInUseMode()
    {
        invenPanel.gameObject.SetActive(true);
        usePanel.SetActive(false);
    }
}
