using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Animator backgroundAnimator;
    static public bool bAutonomous = true;

    static Animator _backgroundAnimator;

    static public void AnimateBackground()
    {
        _backgroundAnimator.SetTrigger("Play");
    }

    // Start is called before the first frame update
    void Start()
    {
        _backgroundAnimator = backgroundAnimator;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
