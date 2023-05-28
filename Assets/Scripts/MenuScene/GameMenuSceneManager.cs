
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameMenuSceneManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] Button[] stageButton;
    void Start()
    {
        for (int i = stageButton.Length - 1; i > PlayerPrefs.GetInt("MaxClearStage"); i--)
        {
            stageButton[i].interactable = false;
        }

    }
}
