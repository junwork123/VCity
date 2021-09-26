using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactionable : MonoBehaviour, IInteraction
{
    Collider collider;



    [SerializeField]
    GameObject interactionUI;

    [SerializeField]
    InteractionType _interType;
    public InteractionType InterType { get => _interType; }
    
    public Outline interOutline;

    bool _enable;
    public bool enable { get => _enable; }


    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        
        interOutline.OutlineColor = Color.yellow;
        interOutline.OutlineWidth = 5f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ShowInter();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // if input action key
        // ActionKey();

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            EndInter();
            NonShowInter();
        }
    }

    public void ShowInter()
    {
        interactionUI.SetActive(true);
        SetOutline();
    }

    void SetOutline()
    {
        interOutline.OutlineMode = Outline.Mode.OutlineAll;
    }

    public void ActionKey()
    {
        print(gameObject.name + " Action");
    }

    public void EndInter()
    {
        interactionUI.SetActive(false);
        UnsetOutline();
    }

    void UnsetOutline()
    {
        interOutline.OutlineMode = Outline.Mode.SilhouetteOnly;
    }

    public void NonShowInter()
    {
        
    }
}
