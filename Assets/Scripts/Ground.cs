using UnityEngine;
using UnityEngine.UI;

public class Ground : MonoBehaviour
{
    // NFT 토큰ID
    [HideInInspector] public string tokenID;

    // 빌보드 UI
    [SerializeField] private Text houseNameText;
    [SerializeField] private RawImage houseImage;
    [SerializeField] private Canvas billBoardCanvas;

    private void Start()
    {
        // 캔버스 월드 스페이스 카메라를 메인 카메라로 설정
        billBoardCanvas.worldCamera = Camera.main;

        // DB를 통해서 땅 정보 받아옴
        ItemInfo itemInfo = InventoryManager.Instance.items[tokenID];

        // 하우스 모델 추가
        //GameObject obj = Resources.Load<GameObject>($"Prefabs/Houses/{itemInfo.itemName}");
        // json이 설명에 이름이 들어가 있어서 임시로 이 코드 사용
        GameObject obj = Resources.Load<GameObject>($"Prefabs/Houses/{itemInfo.description}");
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.Euler(Vector3.zero);

        Instantiate(obj, transform);

        // 빌보드 UI 초기화
        houseNameText.text = $"이름 : {itemInfo.itemName}";
        houseImage.texture = itemInfo.texture;
    }
}
