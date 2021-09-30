using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Interactionable : MonoBehaviour, IInteraction
{
    [SerializeField]
    InteractionType _interactionType;
    public InteractionType interactionType { get => _interactionType; set => _interactionType = value; }
    public bool enable { get; set; }

    [SerializeField]
    GameObject interactionTextUI;

    public KeyCode InteractionKeyCode = KeyCode.X;
    public float interactionRadius = 3f;
    new SphereCollider collider;
    Outline outline;


    // Start is called before the first frame update
    void Start()
    {
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

        enable = true;
    }

    public void EndInter()
    {
        interactionTextUI.SetActive(false);
        UnsetOutline();
        HideInteractionMenu();

        enable = false;
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

        if (ButtonEventManager.instance.activeActionButton == true)
        {
            ButtonEventManager.instance.activeActionButton = false;

            // TODO Action
            #region Action
            EndInter();
            ShowInteractionMenu();

            // switch (interactionType)
            // {
            //     case InteractionType.NPC:

            //         break;
            //     case InteractionType.UNMANNED:

            //         break;
            //     default:
            //         break;
            // }
            #endregion

            // EndAction();
        }
    }

    public void EndAction()
    {
        ShowInter();
    }
    #endregion

    /// <summary>
    /// 오브젝트가 지원하는 기능을 표시
    /// </summary>
    public void ShowInteractionMenu()
    {

    }

    public void HideInteractionMenu()
    {

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
