
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameDataController : MonoBehaviour
{
    public static GameDataController Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private int maxStage;
    public int _maxStage => maxStage;
    void Start()
    {
        PlayerPrefs.SetInt("MaxClearStage", 0);
        PlayerPrefs.SetInt("SelectStage", -1);
        Debug.Log("씬데이터컨트롤러");
    }

}
