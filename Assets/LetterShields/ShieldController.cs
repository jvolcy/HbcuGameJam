using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    BoxCollider2D boxCollider;
    Animator animator;
    [SerializeField] ParticleSystem ps;
    [SerializeField] float RestoreDelay = 5f; //how long to wait before we restore the destroyed letter
    [SerializeField] float explosionScale = 0.5f;
    [SerializeField] GameObject ExplosionPrefab;
    [SerializeField] Vector3 explosionOffest;

    float RestoreTime = 0;  //the future time when we will restore the destroyed letter
    public bool bShieldDown = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyLetter();
        }
        */

        if (bShieldDown && Time.time > RestoreTime)
        {
            RestoreLetter();
        }
    }


    void DestroyLetter()
    {
        ps.Play();
        animator.SetBool("Fade", true);
        bShieldDown = true;
        RestoreTime = Time.time + RestoreDelay;
        boxCollider.enabled = false;    //disable the collider

        var exp = Instantiate(ExplosionPrefab, transform.position, transform.rotation);
        exp.transform.localScale = explosionScale * Vector3.one;  //reduce the size of the explosion
        exp.transform.Translate(explosionOffest);
        exp.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 200);
    }

    void RestoreLetter()
    {
        animator.SetBool("Fade", false);
        bShieldDown = false;
        boxCollider.enabled = true; //enable the collider
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            DestroyLetter();        //destroy the letter
            //Destroy(collision.gameObject);  //destroy the bullet
        }
    }
}
