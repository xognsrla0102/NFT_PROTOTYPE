using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    public static bool isDone;
    public static float progress;

    [SerializeField] private Text progressText;
    [SerializeField] private Image loadingBar;
    private static string loadSceneName;

    public static void LoadScene(string loadSceneName)
    {
        LoadingSceneController.loadSceneName = loadSceneName;
        Debug.Log($"loadSceneName : {loadSceneName}");

        // 로딩씬으로 화면 전환
        SceneManager.LoadScene("LoadingScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        isDone = false;
        progress = 0f;
        SetLoadingFillAmount(0f);

        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);

        // 자동으로 다음 씬 이동 안 하도록 설정
        op.allowSceneActivation = false;

        // 씬 로드
        while (op.isDone == false)
        {
            yield return null;

            SetLoadingFillAmount(Mathf.Lerp(0f, 0.1f, op.progress));

            if (op.progress >= 0.9f)
            {
                SetLoadingFillAmount(0.1f);
                break;
            }
        }

        // 각 씬에 필요한 데이터 로드
        switch (loadSceneName)
        {
            case "InGame":
                Debug.Log("인게임 데이터 로딩 시작");
                Mock.LoadItems();
                break;
            default:
                Debug.Assert(false);
                isDone = true;
                break;
        }

        while (isDone == false)
        {
            yield return null;
            SetLoadingFillAmount(Mathf.Lerp(0.1f, 0.7f, progress));
        }

        Debug.Log("페이크 로딩 시작");

        // 페이크 로딩.. 2초 정도 소요
        float time = 0f;
        while (true)
        {
            yield return null;

            time += Time.unscaledDeltaTime / 2f;
            if (time >= 1f)
            {
                SetLoadingFillAmount(1f);

                // 자동 씬 이동 활성화
                op.allowSceneActivation = true;
                yield break;
            }

            SetLoadingFillAmount(Mathf.Lerp(0.7f, 1f, time));
        }
    }

    private void SetLoadingFillAmount(float value)
    {
        loadingBar.fillAmount = value;
        progressText.text = $"{loadingBar.fillAmount * 100:f2}%";
    }
}
