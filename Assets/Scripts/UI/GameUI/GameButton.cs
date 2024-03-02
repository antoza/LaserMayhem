using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class GameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected Animator _animator;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public virtual void OnClick()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _animator.SetBool("Selected", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       _animator.SetBool("Selected", false);
    }
}