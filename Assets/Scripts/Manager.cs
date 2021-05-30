using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static event Action<bool> isOver;

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
    }

    private void Start()
    {
        tutorialTextMesh.text = tutorialText;
    }

    /// <summary>Triggers the start of the game and all necessary resets.</summary>
    public void StartPlaying()
    {
        // play end animation for UI elements
        uiAnimator.SetTrigger("End");

        // reset points and time
        pointObserver.ResetPoints();
        startTime = Time.time;
    }

    /// <summary>Calculates duration of round and sets UI text. Triggers <c>StopPlaying</c> </summary>
    public void Win()
    {
        int timeNeed = (int)(Time.time - startTime);

        tutorialTextMesh.text = winText + " You needed " + timeNeed + "seconds to win.";

        StopPlaying(true);
    }

    /// <summary>Sets UI text. Triggers <c>StopPlaying</c>.</summary>
    public void GameOver()
    {
        tutorialTextMesh.text = returnText;

        StopPlaying(false);
    }

    /// <summary>Stops round.</summary>
    /// <param name="hasWon"> True: Game was won; False: Game was lost.</param>
    private void StopPlaying(bool hasWon) 
    {
        // play start animation
        uiAnimator.SetBool("HasWon", hasWon);
        uiAnimator.SetTrigger("Start");

        if (isOver != null)
            isOver(true);
    }
}
