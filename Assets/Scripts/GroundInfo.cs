using UnityEngine;

// DB에서 받아오는 정보
[System.Serializable]
public struct GroundInfo
{
    public Texture texture;

    public string tokenID;
    public string houseName;

    public Vector3 pos;
    public Vector3 rot;    

    public GroundInfo(string tokenID, string houseName, Vector3 pos, Vector3 rot)
    {
        texture = null;

        this.tokenID = tokenID;
        this.houseName = houseName;

        this.pos = pos;
        this.rot = rot;
    }
}