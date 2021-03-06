﻿using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    public Vector3 currentSpeed { get; private set; }
    private Vector2 clampX;
    private Vector2 clampY;
    private bool hasClampMinX;
    private bool hasClampMinY;
    private bool hasClampMaxX;
    private bool hasClampMaxY;

    private bool dead;
    public void Kill()
    {
        this.dead = true;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void Push(Vector3 position)
    {
        this.currentSpeed += position;
    }

    void LateUpdate()
    {
        if (dead) return;
        var desiredPosition = this.transform.position + currentSpeed;
        var x = desiredPosition.x;
        var y = desiredPosition.y;

        if (hasClampMinX || hasClampMaxX)
        {
            x = Mathf.Clamp(desiredPosition.x, hasClampMinX ? clampX.x : desiredPosition.x, hasClampMaxX ? clampX.y : desiredPosition.x);
        }

        if (hasClampMinY || hasClampMaxY)
        {
            y = Mathf.Clamp(desiredPosition.y, hasClampMinY ? clampY.x : desiredPosition.y, hasClampMaxY ? clampY.y : desiredPosition.y);
        }

        this.transform.position = new Vector3(x, y);

        Reset();
    }

    void Reset()
    {
        currentSpeed = Vector3.zero;
        clampX = Vector2.zero;
        clampY = Vector2.zero;
        hasClampMinX = false;
        hasClampMaxX = false;
        hasClampMinY = false;
        hasClampMaxY = false;
    }

    public void ClampYMin(float yMin)
    {
        this.hasClampMinY = true;
        clampY.x = yMin;
    }

    public void ClampYMax(float yMax)
    {
        this.hasClampMaxY = true;
        clampY.y = yMax;
    }

    public void ClampXMin(float xMin)
    {
        this.hasClampMinX = true;
        clampX.x = xMin;
    }

    public void ClampXMax(float xMax)
    {
        this.hasClampMaxX = true;
        clampX.y = xMax;
    }
}
