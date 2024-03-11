using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] Animator backgroundAnimator;
    [SerializeField] GameObject WinObj;
    [SerializeField] GameObject LoseObj;
    [SerializeField] GameObject RestartButton;
    [SerializeField] SpriteRenderer[] Lives;
    [SerializeField] TMP_Text Score;

    static public bool bAutonomous = true;
    static public bool bShieldsDown = false;
    static public bool bGameOver = false;
    //static GameObject mWin;
    //static GameObject mLose;
    //static GameObject mRestartButton;
    ShieldController[] mShields;

    int numLives;
    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        mShields = FindObjectsOfType<ShieldController>();
        Debug.Log("Found " + mShields.Length + " shields.");
        //mWin = WinObj;
        //mLose = LoseObj;
        //mRestartButton = RestartButton;

        Restart();
    }

    // Update is called once per frame
    void Update()
    {

        foreach (var shield in mShields)
        {
            if (shield.bShieldDown == false)
            {
                bShieldsDown = false;
                backgroundAnimator.SetBool("Play", false);
                return;
            }
        }

        bShieldsDown = true;
        backgroundAnimator.SetBool("Play", true);
    }

    void ShowShields(bool val)
    {
        foreach (var shield in mShields)
        {
            shield.gameObject.SetActive(val);
        }
    }

    public void Win()
    {
        ShowShields(false);
        WinObj.SetActive(true);
        RestartButton.SetActive(true);
        bGameOver = true;
    }


    public void Lose()
    {
        ShowShields(false);
        LoseObj.SetActive(true);
        RestartButton.SetActive(true);
        bGameOver = true;
    }

    public void Restart()
    {
        numLives = 3;
        score = 0;
        UpdateUI();
        ShowShields(true);
        WinObj.SetActive(false);
        LoseObj.SetActive(false);
        RestartButton.SetActive(false);
        bGameOver = false;

        var pcs = FindObjectsOfType<PlaneController>();
        foreach (var pc in pcs)
        {
            pc.Reset();
        }

    }

    void UpdateUI()
    {
        Lives[2].enabled = false;
        Lives[1].enabled = false;
        Lives[0].enabled = false;

        if (numLives >= 1)
        {
            Lives[0].enabled = true;
        }
        if (numLives >= 2)
        {
            Lives[1].enabled = true;
        }
        if (numLives >= 3)
        {
            Lives[2].enabled = true;
        }

        Score.text = "Score: " + score;
    }

    public void PlayerHit()
    {
        numLives--;
        UpdateUI();

        if (numLives == 0) Lose();
    }

    public void BlimpHit()
    {
        Win();
    }

    public void PlaneHit()
    {
        score += 100;
        UpdateUI();
    }
}
