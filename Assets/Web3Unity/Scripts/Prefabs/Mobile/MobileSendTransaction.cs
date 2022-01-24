using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using UnityEngine.UI;

public class MobileSendTransaction : MonoBehaviour
{
    async public void OnMobileSendTransaction() {
        string to = "0xdA064B1Cef52e19caFF22ae2Cc1A4e8873B8bAB0";
        string value = "12300000000000000";
        string gas = "21000";
        string data = "0x";
        string receipt = await Web3Mobile.SendTransaction(to, value, gas, data);
        print(receipt);
    }
}
