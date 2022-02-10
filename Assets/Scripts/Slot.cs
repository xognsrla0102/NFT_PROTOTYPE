using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public string tokenID;
    public GameObject selectedImg;

    public Text itemName;
    public RawImage image;

    public void OnPointerClick(PointerEventData eventData)
    {
        // 해당 슬롯을 클릭하면
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        SettingItemInfo();

        // 슬롯의 선택 이미지 활성화
        InventoryManager.Instance.inventory.SelectSlot(this);
    }

    public void SettingItemInfo()
    {
        var inventory = InventoryManager.Instance.inventory;
        var itemInfoPanel = inventory.itemInfoRect;

        Text itemNameText = itemInfoPanel.Find("Name").GetComponent<Text>();
        Text descriptionText = itemInfoPanel.Find("Description").GetComponent<Text>();
        Text tokenIDText = itemInfoPanel.Find("TokenID").GetComponent<Text>();
        Text balanceText = itemInfoPanel.Find("Balance").GetComponent<Text>();
        Text contractText = itemInfoPanel.Find("Contract").GetComponent<Text>();
        Text putText = itemInfoPanel.Find("Put").GetComponent<Text>();

        ItemInfo itemInfo = InventoryManager.Instance.items[tokenID];

        // 아이템 정보 패널의 텍스트 갱신
        itemNameText.text = $"이름 : {itemName.text}";
        descriptionText.text = $"설명 : {itemInfo.description}";
        tokenIDText.text = $"토큰ID : {tokenID}";
        balanceText.text = $"밸런스(수량) : {itemInfo.balance}";
        contractText.text = $"컨트랙트 주소: {Mock.contract}";
        putText.text = $"사용 유무 : {(itemInfo.worldInfo.isPut ? "Y" : "N")}";

        inventory.useBtn.SetActive(itemInfo.worldInfo.isPut == false);
        inventory.takeBtn.SetActive(itemInfo.worldInfo.isPut);
    }
}
