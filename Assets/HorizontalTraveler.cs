using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalTraveler : MonoBehaviour
{
    public float LeftEndPoint = -11f;
    public float RightEndPoint = 11f;
    public float EndPointVariance = 10f;
    public float MinSpeed = 1f;
    public float MaxSpeed = 4f;

    float speed;
    bool bTravelingRight = true;
    public bool bAutoRestartTravel = true;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartRandomTravel();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.right);

        if (bTravelingRight)
        {
            if (bAutoRestartTravel && transform.position.x > RightEndPoint)
            {
                StartRandomTravel();
            }
        }
        else
        {
            if (bAutoRestartTravel && transform.position.x < LeftEndPoint)
            {
                StartRandomTravel();
            }
        }

    }

    void StartRandomTravel()
    {
        bTravelingRight = Random.Range(0, 2) == 0;
        float EndPointDelta = Random.Range(0f, EndPointVariance);

        spriteRenderer.flipX = !bTravelingRight;
        spriteRenderer.enabled = true;

        if (bTravelingRight)
        {
            transform.position = new Vector2(LeftEndPoint - EndPointDelta, transform.position.y);
            speed = Random.Range(MinSpeed, MaxSpeed);
        }
        else
        {
            transform.position = new Vector2(RightEndPoint + EndPointDelta, transform.position.y);
            speed = -Random.Range(MinSpeed, MaxSpeed);
        }
    }

}
