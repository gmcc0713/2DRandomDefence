using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public enum CLICK_TYPE {IDLE,PLACING_UNIT, COMPOSITION_UNIT,Sell_Unit}
public class GameMgr : MonoBehaviour
{
    [SerializeField] private UnitSpawner unitSpawner;
    [SerializeField] private PlayerInput input;
    [SerializeField] private Ground ground;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private CLICK_TYPE click_Type = CLICK_TYPE.IDLE;
    public static GameMgr Instance { get; private set; }
    private bool isDragging = false;
    private Vector3 startDragPos;
    private Unit[] unitCombination;
    public CLICK_TYPE _click_Type
    {
        get { return click_Type; }
        set { click_Type = value; }
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
    }

    void MouseLeftBtnDown(InputAction.CallbackContext context)                  //마우스 버튼을 눌렀을때
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 30))
        {
            switch (click_Type)
            {
                case CLICK_TYPE.IDLE:
                    break;
                case CLICK_TYPE.PLACING_UNIT:
                    if (hit.transform.CompareTag("CanSetUnitField"))
                    {
                        if (unitSpawner.SetNewUnitInField(hit.transform))
                        {
                            UIManager.Instance.UnitSpawnWaitImageDelete();
                            hit.transform.gameObject.tag = "CantSetUnitField";
                        }
                    }
                    break;
                case CLICK_TYPE.COMPOSITION_UNIT:
                    if (hit.transform.CompareTag("Unit"))
                    {
                        Debug.Log("드래그 시작");
                        unitCombination[0] = hit.collider.GetComponent<Unit>();
                        unitCombination[0].SettingMoveImageWithMouse();
                        Debug.Log(unitCombination[0]);
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
                    break;
            }
            ground.ShowUnitGroundRemove();
        }
    }
    void MouseLeftBtnUp(InputAction.CallbackContext context)//마우스 버튼을 눌렀다가 땠을때
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 30))
        {
           if(isDragging) 
            {
                if (hit.transform.CompareTag("Unit"))
                {
                    unitCombination[1] = hit.collider.GetComponent<Unit>();
                    if (!UnitSpawner.Instance.CompositionUnit(unitCombination))
                    {
                        ground.ShowUnitGroundRemove();
                        unitCombination[0].BackToImagePosition();
                    }
                }
                else
                {
                    ground.ShowUnitGroundRemove();
                    unitCombination[0].BackToImagePosition();
                }
            }
        }

        if(click_Type != CLICK_TYPE.PLACING_UNIT)
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

}
