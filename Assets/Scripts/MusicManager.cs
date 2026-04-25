using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgm;
    public static MusicManager instance;

    private string sceneToDestroyOn = "MainMenu";
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }
    private void Start()
    {
        bgm.volume = 0.5f;
        bgm.pitch = 1.2f;
        bgm.volume = PlayerPrefs.GetFloat("musicVolume");

        if (!bgm.isPlaying)
        {
            bgm.Play();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneToDestroyOn && gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
