using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactionable : MonoBehaviour, IInteraction
{
    bool isPlayerEnter = false;
    Collider collider;

    public GameObject interactionUI;

    [SerializeField]
    public InteractionType _interType;
    public InteractionType InterType { get { return _interType; } }

    public bool enable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerEnter = true;
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
            isPlayerEnter = false;
            EndInter();
            NonShowInter();
        }
    }

    public void ShowInter()
    {
        interactionUI.SetActive(true);
    }

    public void ActionKey()
    {
    }

    public void EndInter()
    {
        interactionUI.SetActive(false);
    }

    public void NonShowInter()
    {
    }
}
