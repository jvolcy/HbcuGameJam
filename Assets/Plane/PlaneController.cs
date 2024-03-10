using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    //The plane must travel and it must drop bombs.  It must also
    //be able to eplode.  Both functions (traveling and dropping) bombs
    //are affected by the state fo the plane: exploded or not exploded.
    //The logic below is organized in 3 blocks: travel logic, bomb dropping
    //logic and explosion logic.

    //bomb inspector parameters
    [SerializeField] float bombMinPeriod = 0.25f;
    [SerializeField] float bombMaxPeriod = 3f;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float bombSpeed = -4f;

    //travel inspector parameters
    [SerializeField] float travelLeftEndPoint = -11f;
    [SerializeField] float travelRightEndPoint = 11f;
    [SerializeField] float travelEndPointVariance = 10f;
    [SerializeField] float travelMinSpeed = 1f;
    [SerializeField] float travelMaxSpeed = 4f;

    //explosion inspector parameters
    [SerializeField] GameObject ExplosionPrefab;
    [SerializeField] bool animateBackgroiund = false;

    //travel members
    float traveSpeed;
    bool bTravelingRight = true;

    //bomb members
    float AutoBombTime;   //time to auto instantiate a new bomb

    //explosion members
    bool bExploded = false;

    //misc. membwer vars
    SpriteRenderer spriteRenderer;
    BoxCollider2D mCollider;
    SpriteRenderer[] childrenSprites;   //sprites of child objects
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        //misc
        spriteRenderer = GetComponent<SpriteRenderer>();
        mCollider = GetComponent<BoxCollider2D>();
        childrenSprites = GetComponentsInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        //travel
        StartRandomTravel();

        //bomb
        AutoBombTime = Time.time + Random.Range(bombMinPeriod, bombMaxPeriod);

        //explosion

    }


    // Update is called once per frame
    void Update()
    {
        //travel
        transform.Translate(traveSpeed * Time.deltaTime * Vector2.right);

        if (bTravelingRight) //traveling right
        {
            if (transform.position.x > travelRightEndPoint) { StartRandomTravel();}
        }
        else  //traveling left
        {
            if (transform.position.x < travelLeftEndPoint) { StartRandomTravel(); }
        }


        audioSource.enabled = !GameManager.bAutonomous;

        if (bExploded)
            audioSource.volume = 0.05f;
        else
            audioSource.volume = Mathf.Clamp((10f - Mathf.Abs(transform.position.x)) / 10f, 0.05f, 1f);

        //bomb
        //only drop bombs if we are not exploded
        if (!bExploded && Time.time > AutoBombTime)
        {
            if (GameManager.bAutonomous)
                AutoBombTime = Time.time + Random.Range(bombMinPeriod, bombMaxPeriod*2);
            else
                AutoBombTime = Time.time + Random.Range(bombMinPeriod, bombMaxPeriod);

            var bomb = Instantiate(bombPrefab, transform.position, transform.rotation);
            bomb.transform.Translate(0.5f * Vector3.down);
            //var bc = bomb.GetComponent<BulletController>();
            //bc.targetTags = new string[] { "Player", "Plane" };

            bomb.GetComponent<BulletController>().speed = bombSpeed;
            bomb.GetComponent<SpriteRenderer>().color = Color.red;
        }


        //explosion
        if (Input.GetKeyDown(KeyCode.X))
        {
            //for debugging: manually invoke the explode() function
            explode();
        }

    }


    void StartRandomTravel()
    {
        bTravelingRight = Random.Range(0, 2) == 0;
        float EndPointDelta = Random.Range(0f, travelEndPointVariance);

        spriteRenderer.flipX = !bTravelingRight;
        spriteRenderer.enabled = true;
        foreach (var sr in childrenSprites)
        {
            sr.enabled = true;
        }

        mCollider.enabled = true;
        bExploded = false;

        if (bTravelingRight)
        {
            transform.position = new Vector2(travelLeftEndPoint - EndPointDelta, transform.position.y);
            traveSpeed = Random.Range(travelMinSpeed, travelMaxSpeed);
        }
        else
        {
            transform.position = new Vector2(travelRightEndPoint + EndPointDelta, transform.position.y);
            traveSpeed = -Random.Range(travelMinSpeed, travelMaxSpeed);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            //Destroy(collision.gameObject);  //destroy the bullet
            explode();
        }
    }

    void explode()
    {
        bExploded = true;
        var exp = Instantiate(ExplosionPrefab, transform);
        exp.transform.localScale = 0.5f * Vector3.one;  //reduce the size of the explosion
        traveSpeed *= 0.75f; //reduce horizontal velocity
        spriteRenderer.enabled = false; //hide the plane
        foreach (var sr in childrenSprites)
        {
            sr.enabled = false;
        }

        mCollider.enabled = false; //disable the collider

        if (animateBackgroiund) GameManager.AnimateBackground();
    }

}