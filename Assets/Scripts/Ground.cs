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

    private bool isMine;

    private async void Start()
    {
        // 캔버스 월드 스페이스 카메라를 메인 카메라로 설정
        billBoardCanvas.worldCamera = Camera.main;

        // DB를 통해서 땅 정보 받아옴
        //GroundInfo groundInfo = (GroundInfo)Mock.DB[tokenID];

        // 현재 땅의 정보 대입
        //transform.position = groundInfo.pos;
        //transform.rotation = Quaternion.Euler(groundInfo.rot);

        // 보유한 NFT인지 확인
        isMine = await Mock.CheckMine(tokenID);

        if (isMine)
        {
            // 하우스 모델 추가
            //GameObject obj = Resources.Load<GameObject>($"Prefabs/Houses/{groundInfo.itemName}");
            //obj.transform.position = Vector3.zero;
            //obj.transform.rotation = Quaternion.Euler(Vector3.zero);

            //Instantiate(obj, transform);
        }

        // 빌보드 UI 초기화
        //houseNameText.text = $"이름 : {groundInfo.itemName}({(isMine ? "보유" : "미보유")})";
        //houseImage.texture = await Mock.GetTexture(tokenID);
    }
}
