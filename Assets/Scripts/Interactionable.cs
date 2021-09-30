using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class Interactionable : MonoBehaviour, IInteraction
{

    [SerializeField]
    GameObject interactionTextUI;

    [SerializeField]
    InteractionType _interType;
    public InteractionType InterType { get => _interType; }
    public KeyCode InteractionKeyCode = KeyCode.X;
    public float interactionRadius = 3f;

    SphereCollider collider;
    Outline outline;

    bool _enable;
    public bool enable { get => _enable; }


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

        _enable = true;
    }

    public void Action()
    {
        if (_enable == false)
            return;

        if ((Input.GetKeyDown(InteractionKeyCode) || ButtonEventManager.instance.activeActionButton == true))
        {
            ButtonEventManager.instance.activeActionButton = false;

            // TODO Action
            print("Action : " + gameObject.name);
            #region Action


            #endregion
        }
    }

    public void EndInter()
    {
        interactionTextUI.SetActive(false);
        UnsetOutline();

        _enable = false;
    }

    public void NonShowInter()
    {

    }
    #endregion

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
