using UnityEngine;

public class Interactionable : MonoBehaviour, IInteraction
{
    [SerializeField]
    NPCType _objectType;
    public NPCType objectType { get => _objectType; set => _objectType = value; }
    public bool enable { get; set; }
    [SerializeField]
    string _interString;
    public string interString { get => _interString; set => _interString = value; }

    GameObject player;

    bool isOpenInteractionMenu;

    [SerializeField]
    GameObject interactionMenuUI;

    [HideInInspector]
    public KeyCode InteractionKeyCode;
    public float interactionRadius = 3f;
    SphereCollider collider;
    public Outline outline;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = string.Format("{0} NPC", objectType.ToString());
        InteractionKeyCode = GameManager.instance.InteractionKeyCode;

        collider = GetComponent<SphereCollider>();
        collider.radius = interactionRadius;

        InitOutline();
    }

    // Update is called once per frame
    void Update()
    {
        Action();

        NonShowInter();
    }

    #region Trigger Event
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            enable = true;
            player = other.gameObject;

            ShowInter();

            UIManager.instance.AtciveActionButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            enable = false;
            player = null;

            EndInter();
            NonShowInter();

            UIManager.instance.InactiveActionButton();
        }
    }
    #endregion

    #region Interaciton
    public void ShowInter()
    {
        UIManager.instance.OpenDialog(interString);

        // SetOutline();
    }

    public void EndInter()
    {
        UIManager.instance.CloseDialog();

        // UnsetOutline();
        HideInteractionMenu();
    }

    public void NonShowInter()
    {
        if (enable == true)
            return;
    }

    public void Action()
    {
        if (enable == false)
            return;

        if (UIButtonEventManager.instance.activeActionButton == false)
            return;
        UIButtonEventManager.instance.activeActionButton = false;




        /// InteractionMenuCanvas?????? Panel??? ?????? ????????? ??????
        // #region Close Task Menu
        // /// ?????? ???????????? Task ????????? ??????
        // if (isOpenInteractionMenu == true)
        // {
        //     ShowInter();
        //     HideInteractionMenu();

        //     // ???????????? ?????? ??? ???????????? ?????????
        //     GameManager.instance.joystickRange.SetActive(true);
        // }
        // #endregion
        #region Open Task Menu
        // else
        // {
            // ??????????????? ??? ??????????????? ??????????????? ?????????
            player.transform.forward = transform.position - player.transform.position;

            EndInter();
            ShowInteractionMenu();

            // ?????????????????? ?????? ???????????? ????????????
            GameManager.instance.joystickRange.SetActive(false);
        // }
        #endregion

    }
    #endregion

    /// <summary>
    /// ??????????????? ???????????? ????????? ??????
    /// </summary>
    public void ShowInteractionMenu()
    {
        // interactionMenuUI.SetActive(true);
        isOpenInteractionMenu = true;

        UIManager.instance.OpenApplyPanel(objectType);
    }

    public void HideInteractionMenu()
    {
        // interactionMenuUI.SetActive(false);
        // isOpenInteractionMenu = false;

        // if (UIManager.instance.applyPanel[(int)objectType].activeSelf == true)
        //     UIManager.instance.HideApplyPanel(objectType);

        UIManager.instance.CloseApplyPanel(objectType);
    }

    #region Outline
    void InitOutline()
    {
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = Color.yellow;
        SetOutline();
    }

    void SetOutline()
    {
        outline.OutlineWidth = 5f;
    }

    void UnsetOutline()
    {
        outline.OutlineWidth = 0f;
    }
    #endregion

    // #region Debug
    // private void OnDrawGizmos()
    // {
    //     Color color = Color.cyan;
    //     color.a = 0.25f;
    //     Gizmos.color = color;
    //     Gizmos.DrawSphere(transform.position, interactionRadius);
    // }
    // #endregion
}
