using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public static class Mock
{
    public static readonly string chain = "ethereum";
    public static readonly string network = "ropsten";
    public static readonly string contract = "0x9468097D8c5f6898B628c43B5Fcf575DE2d5Dc87";
    public static readonly string myAddress;

    public static string ResourcePath
    {
        get
        {
#if UNITY_EDITOR
            return $"{Application.dataPath}/Resources";
#else
            return Application.persistentDataPath;
#endif
        }
    }

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
            TextAsset textAsset = Resources.Load<TextAsset>("DB/WorldObjectInfos");

            if (textAsset == null || textAsset.text.Length <= 0)
            {
                Debug.Log($"textAsset is null or Length is under 0");
                return;
            }

            worldObjectInfos = JsonConvert.DeserializeObject<List<ItemInfo.WorldInfo>>(textAsset.text);
        }
        catch (Exception e)
        {
            Debug.Log($"LoadWorldObjectInfos Exception : {e}");
        }

        // 아이템에 월드 정보 반영
        foreach (var worldInfo in worldObjectInfos)
        {
            items[worldInfo.tokenID].worldInfo = worldInfo;
        }

    LOAD_DONE:
        LoadingSceneController.isDone = true;
    }

    public static void SaveWorldObjectInfos()
    {
        // 기존 월드 정보 삭제
        worldObjectInfos.Clear();

        var items = InventoryManager.Instance.items;
        foreach (var item in items)
        {
            // 설치되어있다면
            if (item.Value.worldInfo.isPut)
            {
                worldObjectInfos.Add(item.Value.worldInfo);
            }
        }

        // 폴더가 없을 경우 생성
        if (Directory.Exists($"{ResourcePath}/DB") == false)
        {
            Directory.CreateDirectory($"{ResourcePath}/DB");
        }

        string path = $"{ResourcePath}/DB/WorldObjectInfos.json";
        string json = JsonConvert.SerializeObject(worldObjectInfos);
        File.WriteAllText(path, json);
    }
}
