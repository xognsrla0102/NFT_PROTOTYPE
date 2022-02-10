using System;
using UnityEngine;
using UnityEngine.UI;

public class WalletLogin: MonoBehaviour
{
    public Button connectBtn;
    public Toggle rememberMe;

    void Start()
    {
        rememberMe.isOn = false; //PlayerPrefs.GetInt("AutoConnectWallet") == 1 ? true : false;

        // if remember me is checked, set the account to the saved account
        if (rememberMe.isOn && PlayerPrefs.GetString("Account") != "")
        {
            // move to next scene
            LoadingSceneController.LoadScene("InGame");
        }
    }

    private void Update()
    {
        // 뒤로가기 버튼 누르면 앱 종료
        if (Input.GetKey(KeyCode.Escape))
        {
            PlayerPrefs.SetInt("AutoConnectWallet", rememberMe.isOn ? 1 : 0);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    async public void OnLogin()
    {
        // 연결 도중엔 다시 연결 못하게
        connectBtn.interactable = false;

        // get current timestamp
        int timestamp = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        // set expiration time
        int expirationTime = timestamp + 60;
        // set message
        string message = $"접속 시도 시각 : {System.DateTime.Now}";
        
        try
        {
            // sign message
            string signature = await Web3Wallet.Sign(message);

            // verify account
            string account = await EVM.Verify(message, signature);
            int now = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
            // validate
            if (account.Length == 42 && expirationTime >= now)
            {
                // save account
                PlayerPrefs.SetString("Account", account);

                // load next scene
                PlayerPrefs.SetInt("AutoConnectWallet", rememberMe.isOn ? 1 : 0);
                LoadingSceneController.LoadScene("InGame");
            }
        }
        catch (Exception e)
        {
            Debug.Log($"OnLogin Exception : {e}");
        }
        finally
        {
            // 연결 로직이 끝났으면 다시 연결할 수 있게 변경
            connectBtn.interactable = true;
        }
    }
}
