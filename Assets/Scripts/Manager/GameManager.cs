using UnityEngine;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                var gameManager = FindObjectOfType<GameManager>();
                Debug.Assert(gameManager != null);
                instance = gameManager;
            }
            return instance;
        }
    }

    [SerializeField] private GameObject mobileInputPanel;
    [SerializeField] private GameObject uiPanel;

    public bool isAndroidMode;

    private void Awake()
    {
        var gameManagers = FindObjectsOfType<GameManager>();
        if (gameManagers.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

#if !UNITY_EDITOR
        isAndroidMode = true;
#endif
        instance = this;

        mobileInputPanel.SetActive(isAndroidMode);
        uiPanel.SetActive(isAndroidMode);

        SetPlayerControllMode(false);
    }

    private void Update()
    {
        // 뒤로가기 버튼 누르면 앱 종료
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void SetPlayerControllMode(bool isUIControll)
    {
        // PC 모드일 때만 적용
        if (isAndroidMode) return;

        Cursor.lockState = isUIControll ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isUIControll;
    }
}
