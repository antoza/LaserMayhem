using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field:SerializeField]
    private Animator m_Animator;
    [field: SerializeField]
    protected MenuSelection m_MenuSelection;

    public void OnClick()
    {
        m_Animator.SetTrigger("Pressed");
        if (m_MenuSelection.StartChange())
        {
            //This value select the duration of the waiting time after a button is pressed
            StartCoroutine(Delay(0.2f));
        }
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

    IEnumerator Delay(float duration)
    {
        while(duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        ChangeMenu();
    }
        

    public virtual void ChangeMenu()
    {
        ;
    }
}

