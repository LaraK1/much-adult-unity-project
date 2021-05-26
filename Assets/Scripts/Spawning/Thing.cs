using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is an object that can be pooled
public class Thing : MonoBehaviour
{
    // time components
    private float lifeTime;

    [SerializeField]
    private float maxLifeTime = 10f;

    private float countDownPeriod = 3f;
    private float countDownTime;
    private float fastCountDownTime;

    // movement
    private Rigidbody2D _rb;

    [SerializeField]
    private float force = 4;
    [SerializeField]
    private float forceDuration = 0.2f;

    // visual
    private Color orangeRed = new Color(0.7f,0.33f,0.11f);
    private SpriteRenderer _spriteRenderer;

    public static event Action<bool, Vector3> PlayParticle;

    // game logic
    public static event Action<int> OnBoundaryEntered;

    private int penaltyForExpiring = -2;
    private int penaltyForLeftBoundary = -1;
    private int pointsForTopBoundary = 2;
    private int pointsForRightBoundary = 1;

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // setting individual live countdown times
        fastCountDownTime = maxLifeTime - countDownPeriod;
        countDownTime = fastCountDownTime - countDownPeriod;
    }
    void Start()
    {
        Manager.isOver += manager_isGameOver;
    }

    private void OnDestroy()
    {
        Manager.isOver -= manager_isGameOver;
    }

    private void OnEnable()
    {
        lifeTime = 0f;
        ResetColor();
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;

        // give boost
        if(lifeTime < forceDuration)
        {
            _rb.AddForce(Vector2.left * force);
        }

        // visual count down
        VisualCountDown();

        // expire
        Expire();
    }

    /// <summary> Checks if count down takes place.</summary>
    private void VisualCountDown()
    {
        // blink fast if near end
        if (lifeTime > fastCountDownTime)
        {
            Blink(10);
        }
        // blink slow if count down initiated
        else if (lifeTime > countDownTime)
        {
            Blink(5);
        }
    }

    /// <summary> Checks if this thing is over its lifetime.</summary>
    private void Expire()
    {
        if (lifeTime > maxLifeTime)
        {
            // point penalty
            if (OnBoundaryEntered != null)
                OnBoundaryEntered(penaltyForExpiring);

            // shoot particles
            if (PlayParticle != null)
                PlayParticle(false, this.transform.position);

            // returns itself to pool
            ThingPool.Instance.ReturnToPool(this);
        }
    }

    /// <summary>Monitors collisions with player.</summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // reset life time if colliding with player
        // prevents despawning
        if (collision.gameObject.layer == 13)
        {
            lifeTime = forceDuration;
            ResetColor();
        }

    }

    /// <summary>Monitors collisions with borders.</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (lifeTime > forceDuration)
        {
            // collision with left boundary
            if (collision.gameObject.layer == 10)
            {
                // shoot particles
                if (PlayParticle != null)
                    PlayParticle(false, this.transform.position);

                BoundaryCollision(penaltyForLeftBoundary);
            }
            // collision with top boundary
            if (collision.gameObject.layer == 11)
            {
                // shoot particles
                if (PlayParticle != null)
                    PlayParticle(true, this.transform.position);

                BoundaryCollision(pointsForTopBoundary);
            }
            // collision with right boundary
            if (collision.gameObject.layer == 12)
            {
                // shoot particles
                if (PlayParticle != null)
                    PlayParticle(true, this.transform.position);

                BoundaryCollision(pointsForRightBoundary);
            }
        }
    }

    /// <summary>Varies color dependend on speed.</summary>
    /// <param name="speed">Blink speed. The higher, the faster.</param>
    private void Blink(int speed)
    {
        int currentTime = (int)(lifeTime * speed);
        if (currentTime % 2 == 0)
            _spriteRenderer.color = orangeRed;
        else
            ResetColor();
    }

    /// <summary>Triggers point change and returns thing to pool.</summary>
    /// <param name="points">Point increase or penalty.</param>
    private void BoundaryCollision(int points)
    {
        // point event
        if (OnBoundaryEntered != null)
            OnBoundaryEntered(points);

        // return to pool
        ThingPool.Instance.ReturnToPool(this);
    }

    /// <summary>Resets its color to white.</summary>
    private void ResetColor()
    {
        _spriteRenderer.color = Color.white;
    }

    /// <summary>Returns itself when the game has ended.</summary>
    private void manager_isGameOver(bool gameOver)
    {
        if (gameOver)
        {
            ThingPool.Instance.ReturnToPool(this);
        }
    }
}
