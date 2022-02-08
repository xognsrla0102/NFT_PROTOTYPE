using UnityEngine;

public class Lobby : MonoBehaviour
{
    private readonly string metamaskStoreLink = "https://play.google.com/store/apps/details?id=io.metamask";

    public void OnClickLinkStore()
    {
        Application.OpenURL(metamaskStoreLink);
    }
}
