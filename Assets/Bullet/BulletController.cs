using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10;
    public float life  = 1f;
    public bool explodes = false;
    public float explosionScale = 0.5f;
    [SerializeField] GameObject ExplosionPrefab;
    public string[] targetTags;
    public Vector3 explosionOffest;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, life);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.up);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //if the length of the targetTags is 0, target all tags
        if (targetTags.Length != 0)
        {
            bool foundTag = false;
            foreach (string tag in targetTags)
            {
                if (collision.CompareTag(tag)) foundTag = true;
            }

            if (!foundTag) return;
        }

        if (explodes)
        {
            var exp = Instantiate(ExplosionPrefab, transform.position, transform.rotation);
            exp.transform.localScale = explosionScale * Vector3.one;  //reduce the size of the explosion
            exp.transform.Translate(explosionOffest);
        }

        Destroy(gameObject);    //destroy the bullet

    }
}
