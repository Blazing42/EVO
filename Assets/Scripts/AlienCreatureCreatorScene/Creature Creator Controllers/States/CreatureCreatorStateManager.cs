using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCreatorStateManager : MonoBehaviour
{
    //assign all of the references that all of the different states will use
    private Transform _grabbedObj;
    public Transform GrabbedObj  { get { return _grabbedObj; } set { _grabbedObj = value; } }
    private Transform _mirroredObj;
    public Transform MirroredObj { get { return _mirroredObj; } set { _mirroredObj = value; } }
    private BodyPartData _bodyPartData;
    public BodyPartData BodyPartData { get { return _bodyPartData; } set { _bodyPartData = value; } }

    [SerializeField] private GameObject _draggedIconPrefab;
    public GameObject DraggedIconPrefab{ get { return _draggedIconPrefab; }}

    [SerializeField] private GameObject _canvas;
    public GameObject Canvas { get { return _canvas; } }

    CreatureCreatorBaseState _currentState;
    public CreatureCreatorBaseState CurrentState { get { return _currentState; } }

    public string _currentStateName;
    public CreatureCreatorIdleState _idleState = new CreatureCreatorIdleState();
    public CreatureCreatorIconDragState _iconDragState = new CreatureCreatorIconDragState();
    public CreatureCreatorBodyPartDragState _partDragState = new CreatureCreatorBodyPartDragState();
    public CreatureCreatorBodyPartSnapState _partSnapState = new CreatureCreatorBodyPartSnapState();
    public CreatureCreatorBodyPartEditState _partEditState = new CreatureCreatorBodyPartEditState();

    //singleton 
    private static CreatureCreatorStateManager _instance;
    public static CreatureCreatorStateManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //set the creature creator to start in the idle state, start any processes that will begin then the idle state is first entered
        _currentState = _idleState;
        _currentStateName = _idleState.ToString();
        _currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        //call the update method in the current state every update frame
        _currentState.UpdateState(this);
    }

    public void SwitchState(CreatureCreatorBaseState newState)
    {
        //on state changed event here, using the current state and the new state
        _currentState.ExitState(this);
        _currentState = newState;
        _currentStateName = newState.ToString();
        _currentState.EnterState(this);
        
    }
    
}
