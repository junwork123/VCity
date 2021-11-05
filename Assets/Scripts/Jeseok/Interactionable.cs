using UnityEngine;

public class Interactionable : MonoBehaviour, IInteraction
{
    [SerializeField]
    ObjectType _objectType;
    public ObjectType objectType { get => _objectType; set => _objectType = value; }
    public bool enable { get; set; }
    [SerializeField]
    string _interString;
    public string interString { get => _interString; set => _interString = value; }

    GameObject player;

    bool isOpenInteractionMenu;

    [SerializeField]
    GameObject interactionMenuUI;

    public KeyCode InteractionKeyCode;
    public float interactionRadius = 3f;
    SphereCollider collider;
    public Outline outline;


    // Start is called before the first frame update
    void Start()
    {
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
        UIManager.instance.SetDialogMessage(interString);
        UIManager.instance.ShowDialog();

        SetOutline();
    }

    public void EndInter()
    {
        UIManager.instance.HideDialog();

        UnsetOutline();
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


        // 상호작용하는 동안 조이스틱 비활성화
        GameManager.instance.joystickRange.SetActive(isOpenInteractionMenu);

        #region Close Task Menu
        /// 이미 열려있는 Task 메뉴를 닫음
        if (isOpenInteractionMenu == true)
        {
            ShowInter();
            HideInteractionMenu();
        }
        #endregion
        #region Open Task Menu
        else
        {
            // 상호작용할 때 플레이어가 오브젝트를 바라봄
            player.transform.forward = transform.position - player.transform.position;

            EndInter();
            ShowInteractionMenu();
        }
        #endregion

    }
    #endregion

    /// <summary>
    /// 오브젝트가 지원하는 기능을 표시
    /// </summary>
    public void ShowInteractionMenu()
    {
        interactionMenuUI.SetActive(true);
        isOpenInteractionMenu = true;
    }

    public void HideInteractionMenu()
    {
        interactionMenuUI.SetActive(false);
        isOpenInteractionMenu = false;

        if (UIManager.instance.applyPanel.activeSelf == true)
            UIManager.instance.HideApplyPanel();
    }

    #region Outline
    void InitOutline()
    {
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = Color.yellow;
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

    #region Debug
    private void OnDrawGizmos()
    {
        Color color = Color.cyan;
        color.a = 0.25f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, interactionRadius);
    }
    #endregion
}
