 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoPageManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> Pages;

    [SerializeField]
    private GameObject LeftArrow;

    [SerializeField]
    private GameObject RightArrow;

    private int CurrentPage = 0;

    public void MoveToRight()
    {
        LeftArrow.SetActive(true);
        if(CurrentPage+1 < Pages.Count)
        {
            Pages[CurrentPage].SetActive(false);
            CurrentPage++;
            Pages[CurrentPage].SetActive(true);

            if (CurrentPage == Pages.Count - 1)
            {
                RightArrow.SetActive(false);
            }
        }
    }

    public void MoveToLeft()
    {
        RightArrow.SetActive(true);
        if (CurrentPage - 1 >= 0)
        {
            Pages[CurrentPage].SetActive(false);
            CurrentPage--;
            Pages[CurrentPage].SetActive(true);

            if (CurrentPage == 0)
            {
                LeftArrow.SetActive(false);
            }
        }
    }
}
