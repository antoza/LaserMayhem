#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Animator m_Animator;
    private float m_CurrentCooldown;

    public void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void OnClick()
    {
        m_Animator.SetTrigger("Pressed");
        if (MenusManager.Instance.StartChange())
        {
            //This value select the duration of the waiting time after a button is pressed
            StopCoroutine("DelayClick");
            StartCoroutine(DelayClick(0.2f));
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        m_Animator.SetTrigger("Highlighted");
    }

    // This method is called when the mouse pointer exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
       m_Animator.SetTrigger("Normal");
    }
    
    IEnumerator DelayClick(float duration)
    {
        Debug.Log("pas fini");
        m_CurrentCooldown = duration;
        while (m_CurrentCooldown > 0)
        {
            m_CurrentCooldown -= Time.deltaTime;
            yield return null;
        }
        //yield return new WaitForSeconds(duration);

        m_Animator.SetTrigger("Highlighted");
        DoOnClick();
        Debug.Log("fini");
    }

    public virtual void DoOnClick() { }
}
#endif