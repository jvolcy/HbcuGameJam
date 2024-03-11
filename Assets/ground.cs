using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ground : MonoBehaviour
{
    [SerializeField] float explosionScale = 0.5f;
    [SerializeField] GameObject ExplosionPrefab;
    [SerializeField] Vector3 explosionOffest;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bomb"))
        {
            var exp = Instantiate(ExplosionPrefab, collision.transform.position, transform.rotation);
            exp.transform.localScale = explosionScale * Vector3.one;  //reduce the size of the explosion
            exp.transform.Translate(explosionOffest);
            exp.GetComponent<AudioSource>().volume = 0;
        }
    }
}
