using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Interactionable : MonoBehaviour, IInteraction
{
    [SerializeField]
    InteractionType _interactionType;
    public InteractionType interactionType { get => _interactionType; set => _interactionType = value; }
    public bool enable { get; set; }
    bool isOpenTaskMenu;

    [SerializeField]
    GameObject interactionTextUI;
    [SerializeField]
    GameObject interactionMenuUI;

    public KeyCode InteractionKeyCode;
    public float interactionRadius = 3f;
    new SphereCollider collider;
    Outline outline;


    // Start is called before the first frame update
    void Start()
    {
        InteractionKeyCode = GameManager.instance.InteractionKeyCode;

        collider = GetComponent<SphereCollider>();
        collider.radius = interactionRadius;

        outline = GetComponent<Outline>();

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

            ShowInter();
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            enable = false;

            EndInter();
            NonShowInter();
        }
    }
    #endregion

    #region Interaciton
    public void ShowInter()
    {
        interactionTextUI.SetActive(true);

        SetOutline();

        // 액션 버튼 활성화
        // ButtonEventManager.instance.ActionButton
    }

    public void EndInter()
    {
        interactionTextUI.SetActive(false);

        UnsetOutline();
        HideInteractionMenu();

        // 액션 버튼 비활성화
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


        GameManager.instance.joystickRange.SetActive(isOpenTaskMenu);

        #region Close Task Menu, 현재 열려있는 Task 메뉴를 닫음
        if (isOpenTaskMenu == true)
        {
            ShowInter();
            HideInteractionMenu();
        }
        #endregion
        #region Open Task Menu
        else
        {
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
        isOpenTaskMenu = true;
    }

    public void HideInteractionMenu()
    {
        interactionMenuUI.SetActive(false);
        isOpenTaskMenu = false;

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
