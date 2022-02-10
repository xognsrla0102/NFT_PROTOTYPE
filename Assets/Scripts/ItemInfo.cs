using UnityEngine;

public class ItemInfo
{
    [HideInInspector] public string itemName;
    [HideInInspector] public string description;

    [HideInInspector] public string tokenID;
    [HideInInspector] public string ipfsUri;
    [HideInInspector] public string balance;
    
    [HideInInspector] public Texture texture;

    // 월드 정보를 json으로 저장하기 위해 Serializable 해둠
    [HideInInspector] public WorldInfo worldInfo = new WorldInfo();

    [System.Serializable]
    public class WorldInfo
    {
        public string tokenID;
        public bool isPut;

        public float posX;
        public float posY;
        public float posZ;

        public float rotX;
        public float rotY;
        public float rotZ;
    }

    public ItemInfo(string tokenID, string ipfsUri, string balance, string itemName, string description, Texture texture)
    {
        this.tokenID = worldInfo.tokenID = tokenID;
        this.ipfsUri = ipfsUri;
        this.balance = balance;
        this.itemName = itemName;
        this.description = description;
        this.texture = texture;
    }
}
