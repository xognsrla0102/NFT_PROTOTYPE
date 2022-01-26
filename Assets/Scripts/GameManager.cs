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

    public GameObject inputPanel;

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

        inputPanel.SetActive(isAndroidMode);
    }

    private void Start()
    {
        Mock.Initialize();
    }
}
