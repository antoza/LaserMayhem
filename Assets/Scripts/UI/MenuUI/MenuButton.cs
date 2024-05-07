#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator m_Animator;
    [SerializeField]
    private static float m_AnimationDelay = 0.2f; // TODO : Faire en sorte que le délai de l'animation soit cette valeur

    public void Awake()
    {
        m_Animator = GetComponent<Animator>();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (MenusManager.Instance.TryInteractWithUI())
        {
            StartCoroutine(OnClickCoroutine(0.2f));
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        m_Animator.SetTrigger("Highlighted");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       m_Animator.SetTrigger("Normal");
    }
    
    IEnumerator OnClickCoroutine(float duration)
    {
        m_Animator.SetTrigger("Pressed");
        yield return new WaitForSeconds(duration);
        m_Animator.SetTrigger("Normal");
        DoOnClick();
        MenusManager.Instance.StopChange();
    }

    public virtual void DoOnClick() { }
}
#endif