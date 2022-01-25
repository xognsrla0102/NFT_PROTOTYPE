using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public static class Mock
{
    private static readonly string chain = "ethereum";
    private static readonly string network = "ropsten";
    private static readonly string contract = "0x9468097D8c5f6898B628c43B5Fcf575DE2d5Dc87";
    private static readonly string myAddress;

    public static Dictionary<string, GroundInfo> DB = new Dictionary<string, GroundInfo>();

    // Mock 생성자 호출하기 위한 함수
    public static void Initialize() { Debug.Log("Mock Initialized"); }

    static Mock()
    {
#if UNITY_EDITOR
        myAddress = "0xdC1D265eC0B57d8599bbAd4B5962A1e6CCE20700";
#else
        myAddress = PlayerPrefs.GetString("Account");
#endif

        string json;

        #region 임시로 json 작성
        // 파일 입출력으로 클라에서 DB 정보 임시로 가져옴
        // 토큰ID를 통해 URI 얻어오고, 나머지 정보는 DB에서 그냥 들고있음
        //DB.Add("1", new GroundInfo("1", "House1", new Vector3(-35f, 4.5f, 40f), new Vector3(0f, 180f, 0f)));
        //DB.Add("2", new GroundInfo("2", "House2", new Vector3(-13f, 4.5f, 40f), new Vector3(0f, 180f, 0f)));
        //DB.Add("3", new GroundInfo("3", "House3", new Vector3(15f, 8f, 40f), new Vector3(0f, 180f, 0f)));
        //DB.Add("4", new GroundInfo("4", "House4", new Vector3(41f, 7f, 13f), new Vector3(0f, 270f, 0f)));
        //DB.Add("6", new GroundInfo("6", "House5", new Vector3(40f, 7f, -30f), new Vector3(0f, 270f, 0f)));
        //                                
        //json = JsonUtility.ToJson(new Serialization<string, GroundInfo>(DB));
        //Debug.Log(json);
        //File.WriteAllText("Assets/Resources/DB/GroundInfo.json", json);
        #endregion

        //DB 데이터 얻어오기
        json = File.ReadAllText("Assets/Resources/DB/GroundInfo.json");
        DB = JsonUtility.FromJson<Serialization<string, GroundInfo>>(json).ToDictionary();
        
        foreach (var data in DB)
        {
            Ground ground = Resources.Load<Ground>("Prefabs/Ground");
            ground.tokenID = data.Key;
        
            Ground obj = Object.Instantiate(ground);
            obj.name = obj.name.Replace("(Clone)", $"{data.Key}");
        }
    }

    public static async Task<bool> CheckMine(string tokenID)
    {
        string ownerAddress = await ERC721.OwnerOf(chain, network, contract, tokenID);
        return myAddress == ownerAddress;
    }

    private struct Response
    {
        public string image;
    }

    public static async Task<Texture> GetTexture(string tokenID)
    {
        string uri = await ERC721.URI(chain, network, contract, tokenID);
        Debug.Log("uri: " + uri);

        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        await webRequest.SendWebRequest();

        Response data = JsonUtility.FromJson<Response>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(data.image);
        await textureRequest.SendWebRequest();

        return ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
    }
}
