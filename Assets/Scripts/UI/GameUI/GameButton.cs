using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected Animator m_Animator;

    protected virtual void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public virtual void OnClick()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_Animator.SetBool("Selected", true);
    }

    // This method is called when the mouse pointer exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
       m_Animator.SetBool("Selected", false);
    }
}