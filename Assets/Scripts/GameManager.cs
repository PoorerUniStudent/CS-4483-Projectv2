using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float stageTime = 0f;
    private static float bestStageTime;

    string STAGE_NAME = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        STAGE_NAME = SceneManager.GetActiveScene().name;
        bestStageTime = PlayerPrefs.GetFloat(STAGE_NAME);
    }

    private void Update()
    {
        stageTime += Time.deltaTime;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log(bestStageTime);
        if (bestStageTime == 0f || stageTime < bestStageTime)
        {
            PlayerPrefs.SetFloat(STAGE_NAME, stageTime);
        }

        SceneManager.LoadScene(sceneName);
        stageTime = 0f;
        ResumeGame();
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(STAGE_NAME);
        ResumeGame();
    }

    public float GetBestStageTime()
    {
        return bestStageTime;
    }
}
