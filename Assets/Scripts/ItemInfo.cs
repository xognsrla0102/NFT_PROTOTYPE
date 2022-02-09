using UnityEngine;

public class ItemInfo
{
    [HideInInspector] public string itemName;
    [HideInInspector] public string description;

    [HideInInspector] public string tokenID;
    [HideInInspector] public string ipfsUri;
    [HideInInspector] public string balance;
    
    [HideInInspector] public Texture texture;

    public ItemInfo(string tokenID, string ipfsUri, string balance, string itemName, string description, Texture texture)
    {
        this.tokenID = tokenID;
        this.ipfsUri = ipfsUri;
        this.balance = balance;
        this.itemName = itemName;
        this.description = description;
        this.texture = texture;
    }
}
