using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPart : MonoBehaviour
{
    [field: SerializeField]
    private Animator Animator;
    public void StartLaser(bool prediction)
    {
        Animator.SetBool("Prediction", prediction);
    }
}
