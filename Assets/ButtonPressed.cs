using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressed : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field:SerializeField]
    private Animator m_Animator;
    public void OnClick()
    {
        m_Animator.SetTrigger("Pressed");
        Debug.Log("Pressed");
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

