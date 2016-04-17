using UnityEngine;
using System.Collections;

public class Autoscrolling : MonoBehaviour
{
    public Transform PlayerReference;
    public AutoscrollingMark[] Scrollers;
    public Vector2 defaultScroll;

    private float dampTime = 0.15f;
    private Vector3 velocity;    
    private BoxCollider2D playerCollider;
    int activeScroll=-1;

    //test
    public Score score;

    void Start()
    {
        playerCollider = PlayerReference.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        //foreach (var scrPoint in ScrollPoints)
        //{
        //    var realPos = scrPoint.position - scrPoint.size / 2;
        //    Debug.DrawLine(realPos, realPos + new Vector2(scrPoint.size.x, 0), Color.yellow);
        //    Debug.DrawLine(realPos + new Vector2(0, -scrPoint.size.y), realPos + new Vector2(scrPoint.size.x, -scrPoint.size.y), Color.yellow);
        //    Debug.DrawLine(realPos + new Vector2(scrPoint.size.x, 0), realPos + new Vector2(scrPoint.size.x, -scrPoint.size.y), Color.yellow);
        //    Debug.DrawLine(realPos, realPos + new Vector2(0, -scrPoint.size.y), Color.yellow);
        //}
        for (int i = 0; i < Scrollers.Length; i++)
        {
            if (Scrollers[i].BoxCollider.bounds.Intersects(playerCollider.bounds))
            {
                activeScroll = i;
                score.Increase(10);
                break;
            }
        }
        transform.position = Vector3.SmoothDamp(transform.position, transform.position + GetCurrentScrollSpeed(), ref velocity, dampTime);
    }

    public float GetDampTime()
    {
        return this.dampTime;
    }

    public Vector3 GetCurrentScrollSpeed()
    {
        if (activeScroll==-1) return new Vector3(defaultScroll.x, defaultScroll.y);
        var scroll = Scrollers[activeScroll];
        if (scroll == null) return new Vector3(defaultScroll.x, defaultScroll.y);
        return new Vector3(scroll.ScrollSpeed.x, scroll.ScrollSpeed.y);
    }
}
