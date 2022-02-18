using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public static class Mock
{
    public static readonly string chain = "ethereum";
    public static readonly string network = "rinkeby";
    public static readonly string contract = "0xB1A8546F7c1ea062337383aFcE2257B51483f0A2";
    // 이전에 썼던 테스트넷과 컨트랙트
    //public static readonly string network = "ropsten";
    //public static readonly string contract = "0x9468097D8c5f6898B628c43B5Fcf575DE2d5Dc87";
    public static readonly string myAddress;

    public static string ResourcePath => Application.persistentDataPath;

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

    // 원래라면 서버가 가지고 있어야 할 정보들
    public static List<ItemInfo.WorldInfo> worldObjectInfos = new List<ItemInfo.WorldInfo>();
    public static Dictionary<string, ItemInfo> items = new Dictionary<string, ItemInfo>();

    static Mock()
    {
#if UNITY_EDITOR
        myAddress = "0xdC1D265eC0B57d8599bbAd4B5962A1e6CCE20700";
#else
        myAddress = PlayerPrefs.GetString("Account");
#endif
    }

    public static async void LoadItems()
    {
        string resJson = await EVM.AllErc721(chain, network, myAddress, contract);

        try
        {
            Debug.Log(resJson);
            var items = JsonConvert.DeserializeObject<NFT[]>(resJson);

            Debug.Log($"NFT 갯수 : {items.Length}");

            for (int i = 0; i < items.Length; i++)
            {
                UnityWebRequest webRequest = UnityWebRequest.Get(items[i].uri);
                await webRequest.SendWebRequest();

                IPFS_Info response = JsonUtility.FromJson<IPFS_Info>(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));

                Debug.Log($"tokenID : {items[i].tokenId}");
                Debug.Log($"ipfsUri : {items[i].uri}");
                Debug.Log($"balance : {items[i].balance}");
                
                Debug.Log($"itemName : {response.name}");
                Debug.Log($"itemDescription : {response.description}");
                Debug.Log($"itemImageUri : {response.image}");

                UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(response.image);
                await textureRequest.SendWebRequest();
                Texture texture = ((DownloadHandlerTexture)textureRequest.downloadHandler).texture;

                Mock.items.Add(items[i].tokenId,
                    new ItemInfo(items[i].tokenId, items[i].uri, items[i].balance,
                    response.name, response.description, texture)
                );

                LoadingSceneController.progress = (i + 1) / (float)items.Length;
            }
        }
        catch (Exception e)
        {
            Debug.Log($"LoadAllTokenID Exception : {e}");
        }

        LoadWorldObjectInfos();
    }

    private static void LoadWorldObjectInfos()
    {
        string path = $"{ResourcePath}/DB/WorldObjectInfos.json";
        Debug.Log($"LoadWorldObjectInfos path : {path}");

        if (File.Exists(path) == false)
        {
            goto LOAD_DONE;
        }

        try
        {
            string json = File.ReadAllText(path);

            if (json.Length <= 0)
            {
                Debug.Log($"Text length is under 0");
                return;
            }

            worldObjectInfos = JsonConvert.DeserializeObject<List<ItemInfo.WorldInfo>>(json);

            foreach (var worldInfo in worldObjectInfos)
            {
                Debug.Log($"tokenID : {worldInfo.tokenID}");
                Debug.Log($"isPut : {worldInfo.isPut}");

                Debug.Log($"posX : {worldInfo.posX}");
                Debug.Log($"posY: {worldInfo.posY}");
                Debug.Log($"posZ : {worldInfo.posZ}");

                Debug.Log($"rotX : {worldInfo.rotX}");
                Debug.Log($"rotY : {worldInfo.rotY}");
                Debug.Log($"rotZ : {worldInfo.rotZ}");
            }
        }
        catch (Exception e)
        {
            Debug.Log($"LoadWorldObjectInfos Exception : {e}");
        }

        // 아이템에 월드 정보 반영
        foreach (var worldInfo in worldObjectInfos)
        {
            if (items.ContainsKey(worldInfo.tokenID) == false)
            {
                Debug.Log($"Failed to Find Token ID : {worldInfo.tokenID}");
                continue;
            }
            items[worldInfo.tokenID].worldInfo = worldInfo;
        }

    LOAD_DONE:
        LoadingSceneController.isDone = true;
    }

    public static void SaveWorldObjectInfos()
    {
        Debug.Log("월드 오브젝트 정보 저장");

        // 기존 월드 정보 삭제
        worldObjectInfos.Clear();

        var items = InventoryManager.Instance.items;
        foreach (var item in items)
        {
            // NFT가 설치되어 있는 것만 추가
            if (item.Value.worldInfo.isPut)
            {
                worldObjectInfos.Add(item.Value.worldInfo);
            }
        }

        foreach (var worldInfo in worldObjectInfos)
        {
            Debug.Log($"tokenID : {worldInfo.tokenID}");
            Debug.Log($"isPut : {worldInfo.isPut}");

            Debug.Log($"posX : {worldInfo.posX}");
            Debug.Log($"posY: {worldInfo.posY}");
            Debug.Log($"posZ : {worldInfo.posZ}");

            Debug.Log($"rotX : {worldInfo.rotX}");
            Debug.Log($"rotY : {worldInfo.rotY}");
            Debug.Log($"rotZ : {worldInfo.rotZ}");
        }

        // 폴더가 없을 경우 생성
        if (Directory.Exists($"{ResourcePath}/DB") == false)
        {
            Directory.CreateDirectory($"{ResourcePath}/DB");
        }

        string path = $"{ResourcePath}/DB/WorldObjectInfos.json";
        string json = JsonConvert.SerializeObject(worldObjectInfos);
        Debug.Log($"Write File Json : {json}");
        File.WriteAllText(path, json);
    }
}
