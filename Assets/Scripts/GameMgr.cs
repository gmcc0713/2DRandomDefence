using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public enum CLICK_TYPE {IDLE,PLACING_UNIT, COMPOSITION_UNIT,Sell_Unit,Show_Pannel}
public class GameMgr : MonoBehaviour
{
    [SerializeField] private UnitSpawner unitSpawner;
    [SerializeField] private PlayerInput input;
    [SerializeField] private Ground ground;
    [SerializeField] private Image fade;

    public static GameMgr Instance { get; private set; }
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private CLICK_TYPE click_Type = CLICK_TYPE.IDLE;

    private bool isDragging = false;
    private Vector3 startDragPos;

    private Unit[] unitCombination;

    private int playerGold;
    private int playerHealth;             

    private int playerBuyUnitGold;       //유닛 구매 가격
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
        mainCamera = Camera.main;
    }
    //===============================================================================

    void Start()
    {
        input.SwitchCurrentActionMap("Player");
        input.actions["MouseLeftBtnDown"].performed += MouseLeftBtnDown;
        input.actions["MouseLeftBtnUp"].canceled += MouseLeftBtnUp;
        input.actions["Drag"].performed += Drag;

        unitCombination = new Unit[2];
        playerHealth = 100;
        playerGold = 300;
        playerBuyUnitGold = 100;
        UIManager.Instance.SetGoldUI();
        UIManager.Instance.SetHealthUI();
    }
    
    void MouseLeftBtnDown(InputAction.CallbackContext context)                  //마우스 버튼을 눌렀을때
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 30)&& !isDragging)
        {
            switch (click_Type)
            {
                case CLICK_TYPE.IDLE:
                    break;
                case CLICK_TYPE.PLACING_UNIT:
                    if (hit.transform.CompareTag("GroundWithoutPlayer"))
                    {
                        if (unitSpawner.SetNewUnitInField(hit.transform))
                        {

                            UIManager.Instance.UnitSpawnWaitImageDelete();
                            hit.transform.gameObject.tag = "GroundWithPlayer";
                            click_Type = CLICK_TYPE.IDLE;
                            ground.ShowUnitGroundRemove();
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
        Debug.Log("마우스 버튼 뗐을때");
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
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

        if (click_Type != CLICK_TYPE.PLACING_UNIT)
        {
            click_Type = CLICK_TYPE.IDLE;
        }
        isDragging = false;
    }
   void Drag(InputAction.CallbackContext context)                   //마우스 버튼을 누르고 있을때
    {
        if (isDragging)
        {
            Vector2 delta = context.ReadValue<Vector2>();
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            worldPos.z = transform.position.z;
            transform.position = startDragPos + (worldPos - startDragPos) + (Vector3)delta;
            unitCombination[0].MoveImageWithMouse();
        }
    }


    public void GetDamagePlayer(int damage)
    {
        playerHealth -= damage;
        UIManager.Instance.SetHealthUI();
        if(playerHealth<=0)
        {
            StartCoroutine(GameOverAnimation());

        }
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
    }
    private IEnumerator GameOverAnimation()
    {
        Time.timeScale = 0;
        fade.gameObject.SetActive(true);
        fade.GetComponent<Fade>().CallFadeOut();

        yield return null;


    }

}
