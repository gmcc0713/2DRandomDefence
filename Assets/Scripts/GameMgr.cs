using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameMgr : MonoBehaviour
{
    public enum CLICK_TYPE {IDLE,PLACING_UNIT, COMPOSITION_UNIT}
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

    void MouseLeftBtnDown(InputAction.CallbackContext context)
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
                            ground.ShowPlaceAbleRemove();
                            hit.transform.gameObject.tag = "CantSetUnitField";
                        }
                    }
                    break;
                case CLICK_TYPE.COMPOSITION_UNIT:
                    if (hit.transform.CompareTag("Unit"))
                    {
                        Debug.Log("드래그 시작");
                        unitCombination[0] = hit.collider.GetComponent<Unit>();
                        isDragging = true;
                        startDragPos = Input.mousePosition;

                    }
                    break; 
            }
           
        }
    }
    void MouseLeftBtnUp(InputAction.CallbackContext context)
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 30))
        {
           if(isDragging) 
            {
                if (hit.transform.CompareTag("Unit"))
                {
                    unitCombination[1] = hit.collider.GetComponent<Unit>();
                    Debug.Log(unitCombination[0].type);
                    Debug.Log(unitCombination[1].type);
                    UnitSpawner.Instance.CompositionUnit(unitCombination);
                    Debug.Log("드래그 끝");
                }
                isDragging = false;
                click_Type = CLICK_TYPE.IDLE;
            }
        }

    }
   void Drag(InputAction.CallbackContext context)
    {
        if (isDragging)
        {
                Vector2 delta = context.ReadValue<Vector2>();
                Vector3 worldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                worldPos.z = transform.position.z;
                transform.position = startDragPos + (worldPos - startDragPos) + (Vector3)delta;
         }
    }

}
