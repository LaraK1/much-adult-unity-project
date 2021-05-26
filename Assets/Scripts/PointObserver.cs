using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointObserver : MonoBehaviour
{
    [SerializeField]
    private Slider pointSlider;
    [SerializeField]
    private int maxPoints;    
    [SerializeField]
    private int startPoints;

    private int points;

    [SerializeField]
    private Spawner spawner;

    void Start()
    {
        Thing.OnBoundaryEntered += Thing_BoundaryEntered;
    }

    private void OnDestroy()
    {
        Thing.OnBoundaryEntered -= Thing_BoundaryEntered;
    }

    /// <summary>
    /// Handles points. Adds points and monitors win and game over.
    /// </summary>
    /// <param name="pointChange">Points that get added. Can be negative.</param>
    private void Thing_BoundaryEntered(int pointChange)
    {
        points += pointChange;

        // game is lost
        if(points < 1)
        {
            spawner.ToggleSpawning(false);
            Manager.Instance.GameOver();
        } 
        // game is won
        else if(points >= maxPoints)
        {
            spawner.ToggleSpawning(false);
            Manager.Instance.Win();
        }
        // game is still on
        else
        {
            spawner.ChangeFrequency(points);
        }
        
        // change ui element
        pointSlider.value = points;
    }

    /// <summary>Resets all point values for restart and gives spawner the command to restart.</summary>
    public void ResetPoints()
    {
        pointSlider.maxValue = maxPoints;
        pointSlider.value = startPoints;
        points = startPoints;

        // resets and activates spawner
        spawner.ChangeFrequency(points);
        spawner.ToggleSpawning(true);
    }
}
