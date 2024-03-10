using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float UserSpeed = 1;
    [SerializeField] float AutoSpeed = 1;
    [SerializeField] float LeftLimit = -8;
    [SerializeField] float RightLimit = 8;
    [SerializeField] float AutoBulletMinPeriod = 0.25f;
    [SerializeField] float AutoBulletMaxPeriod = 3f;
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] GameObject ExplosionPrefab;
    [SerializeField] float Timeout = 15f;

    int AutoDirection = 1;
    float AutoBulletTime;   //time to auto instantiate a new bullet
    float timeoutTime;  //time when we revert to auto-play
    bool bInitializing; //screen countdown when user play begins
    float initializingTime;
    Animator animator;
    BoxCollider2D playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        AutoBulletTime = Time.time + Random.Range(AutoBulletMinPeriod, AutoBulletMaxPeriod);
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.bAutonomous)
        {
            autonomousUpdate();
        }
        else
        {
            userControlsUpdate();
        }
    }


    void autonomousUpdate()
    {
        float x = AutoSpeed * Time.deltaTime * AutoDirection;
        transform.Translate(x * Vector2.right);

        //check if we hit the right limit
        if (transform.position.x > RightLimit)
        {
            transform.position = new Vector2(RightLimit, transform.position.y);
            AutoDirection = -AutoDirection; //change direction
        }

        //check if we hit the left limit
        if (transform.position.x < LeftLimit)
        {
            transform.position = new Vector2(LeftLimit, transform.position.y);
            AutoDirection = -AutoDirection; //change direction
        }

        AutoBullet();

        //exit autonomous mode if a key is pressed
        if (Input.anyKeyDown)
        {
            GameManager.bAutonomous = false;
            Initialize();
        }
    }

    private void Initialize()
    {
        bInitializing = true;
        initializingTime = Time.time + 3.1f;


        //center the player
        transform.position = new Vector2(0, transform.position.y);

        //play the countdown animation
        animator.SetTrigger("Start");

        //disable the collider
        playerCollider.enabled = false;
    }

    void userControlsUpdate()
    {
        if (bInitializing)
        {
            if (Time.time < initializingTime) return;

            bInitializing = false;
            timeoutTime = Time.time + Timeout;

            //enable the collider
            playerCollider.enabled = true;
        }

        float x = UserSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        transform.Translate(x * Vector2.right);

        //check if we hit the right limit
        if (transform.position.x > RightLimit)
        {
            transform.position = new Vector2(RightLimit, transform.position.y);
        }

        //check if we hit the left limit
        if (transform.position.x < LeftLimit)
        {
            transform.position = new Vector2(LeftLimit, transform.position.y);
        }

        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            InstantiateBullet();
        }

        if (Input.anyKey) timeoutTime = Time.time + Timeout;

        if (Time.time > timeoutTime)
        {
            GameManager.bAutonomous = true;
        }
    }

    void AutoBullet()
    {
        if (Time.time > AutoBulletTime)
        {
            AutoBulletTime = Time.time + Random.Range(AutoBulletMinPeriod, AutoBulletMaxPeriod);
            InstantiateBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bomb"))
        {
            var exp = Instantiate(ExplosionPrefab, transform.position, transform.rotation);
            exp.transform.localScale = 0.5f * Vector3.one;  //reduce the size of the explosion
            Initialize();
        }
    }

    void InstantiateBullet()
    {
        var bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
        bullet.transform.Translate(0.5f * Vector3.up);
        //var bc = bullet.GetComponent<BulletController>();
        //bc.targetTags = new string[] { "Untagged", "Player" };
        //bc.explodes = true;
        //bc.explosionScale = 0.5f;
    }
}
