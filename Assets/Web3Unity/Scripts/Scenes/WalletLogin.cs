using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WalletLogin: MonoBehaviour
{
    public Toggle rememberMe;

    void Start() {
        rememberMe.isOn = PlayerPrefs.GetInt("AutoConnectWallet") == 1 ? true : false;

        // if remember me is checked, set the account to the saved account
        if (rememberMe.isOn && PlayerPrefs.GetString("Account") != "")
        {
            // move to next scene
            SceneManager.LoadScene("InGame");
        }
    }

    private void Update()
    {
        // 뒤로가기 버튼 누르면 앱 종료
        if (Input.GetKey(KeyCode.Escape))
        {
            PlayerPrefs.SetInt("AutoConnectWallet", rememberMe.isOn ? 1 : 0);
            Application.Quit();
        }
    }

    async public void OnLogin()
    {
        // get current timestamp
        int timestamp = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        // set expiration time
        int expirationTime = timestamp + 60;
        // set message
        string message = $"{expirationTime}";
        // sign message
        string signature = await Web3Wallet.Sign(message);
        // verify account
        string account = await EVM.Verify(message, signature);
        int now = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        // validate
        if (account.Length == 42 && expirationTime >= now) {
            // save account
            PlayerPrefs.SetString("Account", account);

            // load next scene
            PlayerPrefs.SetInt("AutoConnectWallet", rememberMe.isOn ? 1 : 0);
            SceneManager.LoadScene("InGame");
        }
    }
}
