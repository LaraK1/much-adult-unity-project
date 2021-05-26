using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private bool isPlaying;
    public static event Action<bool> isGameover;
    private bool hasWon;

    public static Manager Instance { get; private set; }

    [SerializeField]
    private PointObserver pointObserver;

    [SerializeField]
    private Animator uiAnimator;

    [SerializeField]
    private TextMeshProUGUI tutorialTextMesh;

    [SerializeField]
    private string tutorialText;
    [SerializeField]
    private string returnText;
    [SerializeField]
    private string winText;

    private float startTime;

    private void Awake()
    {
        Instance = this;
        isPlaying = false;
    }

    private void Start()
    {
        tutorialTextMesh.text = tutorialText;
    }

    private void Update()
    {
        if(!isPlaying && Input.GetMouseButtonDown(0))
        {
            StartPlaying();
        }

    }

    public void StartPlaying()
    {
        // play end animation
        uiAnimator.SetTrigger("End");

        isPlaying = true;
        pointObserver.ResetPoints();

        startTime = Time.time;
    }

    public void Win()
    {
        int timeNeed = (int)(Time.time - startTime);

        tutorialTextMesh.text = winText + " You needed " + timeNeed + "seconds to win.";
        hasWon = true;

        StopPlaying();
    }

    public void GameOver()
    {
        tutorialTextMesh.text = returnText;
        hasWon = false;

        StopPlaying();
    }

    public void StopPlaying() 
    {
        // play start animation
        uiAnimator.SetBool("HasWon", hasWon);
        uiAnimator.SetTrigger("Start");

        isPlaying = false;
        if (isGameover != null)
            isGameover(true);
    }
}
