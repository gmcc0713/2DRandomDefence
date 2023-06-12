using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum CLICK_TYPE {IDLE,PLACING_UNIT, COMPOSITION_UNIT,Sell_Unit,Show_Pannel}
public class GameMgr : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private Ground ground;
    [SerializeField] private GameObject gameResultImage;
    public GameObject _gameResultImage => gameResultImage;


    public static GameMgr Instance { get; private set; }
    private Ray ray;
    private RaycastHit hit;

    private CLICK_TYPE click_Type = CLICK_TYPE.IDLE;

    private bool isDragging = false;
    private Vector3 startDragPos;

    private Unit[] unitCombination;

    private int playerGold;
    private int playerHealth;             

    private int playerBuyUnitGold;       //유닛 구매 가격
    private bool isGameEnd;
    public bool _isGameEnd => isGameEnd;
    public CLICK_TYPE _click_Type
    {
        get { return click_Type; }
        set { click_Type = value; }
    }
    public int _playerGold
    {
        get { return playerGold; }
        set { playerGold = value; }
    }
    public int _playerHealth
    {
        get { return playerHealth; }
        set { playerHealth = value; }
    }
    public int _playerBuyUnitGold
    {
        get { return playerBuyUnitGold; }
        set { playerBuyUnitGold = value; }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Initialize();
    }
    //===============================================================================
    public void Initialize()
    {
        input.SwitchCurrentActionMap("Player");
       /* input.actions["MouseLeftBtnDown"].performed += MouseLeftBtnDown;
        input.actions["Drag"].performed += Drag;
        input.actions["MouseLeftBtnDown"].canceled += MouseLeftBtnUp;*/

        input.actions["TouchPress"].started += MouseLeftBtnDown;
        input.actions["Drag"].performed += Drag;
        input.actions["TouchPress"].canceled += MouseLeftBtnUp;


        unitCombination = new Unit[2];
        playerHealth = 100;
        playerGold = 300;
        playerBuyUnitGold = 100;

        UIManager.Instance.SetGoldUI();
        UIManager.Instance.SetHealthUI();
    }
    void MouseLeftBtnDown(InputAction.CallbackContext context)                  //마우스 버튼을 눌렀을때
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 30)&& !isDragging)
        {
            switch (click_Type)
            {
                case CLICK_TYPE.IDLE:
                    break;
                case CLICK_TYPE.PLACING_UNIT:
                    if (hit.transform.CompareTag("GroundWithoutPlayer"))
                    {
                        if (UnitSpawner.Instance.SetNewUnitInField(hit.transform))
                        {

                            UIManager.Instance.UnitSpawnWaitImageDelete();
                            hit.transform.gameObject.tag = "GroundWithPlayer";
                            click_Type = CLICK_TYPE.IDLE;
                            ground.ShowUnitGroundRemove();
                        }
                        else
                        {
                            click_Type = CLICK_TYPE.IDLE;
                        }
                    }
                    return;
                case CLICK_TYPE.COMPOSITION_UNIT:
                    if (hit.transform.CompareTag("Unit"))
                    {
                        
                        unitCombination[0] = hit.collider.GetComponent<Unit>();
                        unitCombination[0].SettingMoveImageWithMouse();
                        isDragging = true;
                        startDragPos = Input.mousePosition;
                        break;
                    }
                    else
                    {
                        click_Type = CLICK_TYPE.IDLE;
                    }
                    break;
                case CLICK_TYPE.Sell_Unit:
                    if (hit.transform.CompareTag("Unit"))
                    {
                        
                        UnitSpawner.Instance.SellUnit(hit.collider.GetComponent<Unit>());
                    }
                    else
                    {
                        click_Type = CLICK_TYPE.IDLE;
                    }
                    break;
            }

            ground.ShowUnitGroundRemove();
        }
    }
    void MouseLeftBtnUp(InputAction.CallbackContext context)//마우스 버튼을 눌렀다가 땠을때
    {
        Debug.Log("클릭 땜");
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 30))
        {
            if (isDragging && click_Type == CLICK_TYPE.COMPOSITION_UNIT)
            {
                unitCombination[0].BackToImagePosition();                       //드래그로 이동중이던 이미지 원래대로 되돌리기
                ground.ShowUnitGroundRemove();                                  //유닛을 놓을수 있는 표시 없애기
               
                if (hit.transform.CompareTag("Unit"))
                {
                    unitCombination[1] = hit.collider.GetComponent<Unit>();
                    UnitSpawner.Instance.CompositionUnit(unitCombination);
                    unitCombination[0] = null;
                    unitCombination[1] = null;
                }
            }
        }

        isDragging = false;
    }
   void Drag(InputAction.CallbackContext context)                   //마우스 버튼을 누르고 있을때
    {
        if (isDragging)
        {
            Vector2 delta = context.ReadValue<Vector2>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            worldPos.z = transform.position.z;
            transform.position = worldPos  + (Vector3)delta;
            unitCombination[0].MoveImageWithMouse();
        }
    }


    public void GetDamagePlayer(int damage)
    {
        playerHealth -= damage;
        

        if(playerHealth<=0)
        {
            playerHealth = 0;
            isGameEnd = true;
            SoundManager.Instance.PlayAudioClipOneShot(Sound_Type.Sound_SFX, (int)SFX_Num.Lose);
            SoundManager.Instance.StopBGM();
            gameResultImage.GetComponent<GameResultSetting>().CallFadeOut(GameResult_Type.GameOver);
        }
        UIManager.Instance.SetHealthUI();
    }
    public bool IsEnoughtGold(int gold)
    {
        if (playerGold >= gold)
        {
            playerGold -= gold;
            return true;
        }
        return false;
    }
    public void GetGold(int addGold)
    {
        playerGold += addGold;
        UIManager.Instance.SetGoldUI();
    }

    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;
        Initialize();
        WaveManager.Instance.SceneLoad();
    }

    void OnDisable()
    {
        input.actions["MouseLeftBtnDown"].performed -= MouseLeftBtnDown;
        input.actions["MouseLeftBtnUp"].canceled -= MouseLeftBtnUp;
        input.actions["Drag"].performed -= Drag;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void TouchStart(InputAction.CallbackContext context)
    {
        Debug.Log("TouchStart");
    }
    void TouchEnd(InputAction.CallbackContext context)
    {
        Debug.Log("TouchEnd");

    }
}
   
