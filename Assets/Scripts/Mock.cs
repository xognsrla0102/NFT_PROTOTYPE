using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Newtonsoft.Json;

public static class Mock
{
    public static readonly string chain = "ethereum";
    public static readonly string network = "ropsten";
    public static readonly string contract = "0x9468097D8c5f6898B628c43B5Fcf575DE2d5Dc87";
    public static readonly string myAddress;

    private class NFT
    {
        public string contract;
        public string tokenId;
        public string uri;
        public string balance;
    }
    public struct IPFS_Info
    {
        public string name;
        public string description;
        public string image;
    }

    // Mock 생성자 호출하기 위한 함수
    public static void Initialize() { Debug.Log("Mock Initialized"); }

    static Mock()
    {
#if UNITY_EDITOR
        myAddress = "0xdC1D265eC0B57d8599bbAd4B5962A1e6CCE20700";
#else
        myAddress = PlayerPrefs.GetString("Account");
#endif

        LoadAllTokenID();

        // 게임 오브젝트 추가되는 건 임시로 막음. 나중에 사용할 때 다시 꺼낼 것임
        //foreach (var data in DB)
        //{
        //    Ground ground = Resources.Load<Ground>("Prefabs/Ground");
        //    ground.tokenID = data.Key;
        //
        //    Ground obj = Object.Instantiate(ground);
        //    obj.name = obj.name.Replace("(Clone)", $"{data.Key}");
        //}
    }

    public static async Task<bool> CheckMine(string tokenID)
    {
        string ownerAddress = await ERC721.OwnerOf(chain, network, contract, tokenID);
        return myAddress == ownerAddress;
    }

    public static async Task<Texture> GetTexture(string tokenID)
    {
        string uri = await ERC721.URI(chain, network, contract, tokenID);

        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        await webRequest.SendWebRequest();

        string imageUri = JsonUtility.FromJson<string>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(imageUri);
        await textureRequest.SendWebRequest();

        return ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;
    }

    private static async void LoadAllTokenID()
    {
        string resJson = await EVM.AllErc721(chain, network, myAddress, contract);

        try
        {
            var items = JsonConvert.DeserializeObject<NFT[]>(resJson);

            for (int i = 0; i < items.Length; i++)
            {
                //Debug.Log($"tokenID : {items[i].tokenId}");
                //Debug.Log($"ipfsUri : {items[i].uri}");
                //Debug.Log($"balance : {items[i].balance}");

                UnityWebRequest webRequest = UnityWebRequest.Get(items[i].uri);
                await webRequest.SendWebRequest();

                IPFS_Info response = JsonUtility.FromJson<IPFS_Info>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));
                //Debug.Log($"itemName : {response.name}");
                //Debug.Log($"itemDescription : {response.description}");
                //Debug.Log($"itemImageUri : {response.image}");

                UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(response.image);
                await textureRequest.SendWebRequest();
                Texture texture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;

                InventoryManager.Instance.items.Add(items[i].tokenId,
                    new ItemInfo(items[i].tokenId, items[i].uri, items[i].balance,
                    response.name, response.description, texture)
                );
            }
        }
        catch (Exception e)
        {
            Debug.Log($"LoadAllTokenID error : {e}");
        }
    }
}
