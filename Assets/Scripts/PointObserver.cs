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

    private void Thing_BoundaryEntered(int pointChange)
    {
        points += pointChange;


        if(points < 1)
        {
            spawner.ToggleSpawning(false);
            Manager.Instance.GameOver();
        } else if(points >= maxPoints)
        {
            spawner.ToggleSpawning(false);
            Manager.Instance.Win();
        }
        else
        {
            spawner.ChangeFrequency(points);
        }
        
        pointSlider.value = points;
    }

    public void ResetPoints()
    {
        pointSlider.maxValue = maxPoints;
        pointSlider.value = startPoints;
        points = startPoints;
        spawner.ChangeFrequency(points);
        spawner.ToggleSpawning(true);
    }
}
