using UnityEngine;

public class Inventory : MonoBehaviour
{
    private void OnEnable()
    {
        LoadAllTokenID();
    }

    private void LoadAllTokenID()
    {
        EVM.AllErc721(Mock.chain, Mock.network, Mock.myAddress, Mock.contract);
    }
}
