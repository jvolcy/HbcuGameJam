using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        //only play a sound if we are not in autonomous mode
        if (!GameManager.bAutonomous)
            GetComponent<AudioSource>().Play();

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //once the animation completes, destroy the explosion object
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Done"))
        {
            Destroy(gameObject);
        }
    }
}
