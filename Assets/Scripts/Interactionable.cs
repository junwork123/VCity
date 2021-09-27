using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class Interactionable : MonoBehaviour, IInteraction
{
    Collider collider;

    [SerializeField]
    GameObject interactionUI;

    [SerializeField]
    InteractionType _interType;
    public InteractionType InterType { get => _interType; }
    public KeyCode InteractionKeyCode = KeyCode.X;

    public Outline interOutline;

    bool _enable;
    public bool enable { get => _enable; }


    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();

        InitOutline();
    }

    // Update is called once per frame
    void Update()
    {
        ActionKey();
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
        interactionUI.SetActive(true);
        SetOutline();

        _enable = true;
    }

    public void ActionKey()
    {
        if (enable == true && (Input.GetKeyDown(InteractionKeyCode) || ButtonEvent.instance.activeActionButton == true))
        {
            EndInter();

            ButtonEvent.instance.activeActionButton = false;

            // TODO Action
            print("Action : " + gameObject.name);
        }
    }

    public void EndInter()
    {
        interactionUI.SetActive(false);
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
        interOutline.OutlineMode = Outline.Mode.OutlineVisible;
        interOutline.OutlineColor = Color.yellow;
    }

    void SetOutline()
    {
        interOutline.OutlineWidth = 5f;
    }

    void UnsetOutline()
    {
        interOutline.OutlineWidth = 0f;
    }
    #endregion
}
