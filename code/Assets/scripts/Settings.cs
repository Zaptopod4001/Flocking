using UnityEngine;

// Copyright Sami S.

// use of any kind without a written permission 
// from the author is not allowed.

// DO NOT:
// Fork, clone, copy or use in any shape or form.

[System.Serializable]
public class Settings
{
    [Header("Movement")]
    public float speed = 6f;
    public float speedMax = 8f;
    public float steerSpeedMax = 12f;

    [Header("Perception")]
    [Range(30,360)]
    public float perceptionAngle = 270f;

    [Header("Walls avoid")]
    public float viewDist = 2f;
    public float viewAngle = 45f;

    [Range(3, 10f)]
    public int viewRays = 5;

    [Range(0, 20f)]
    public float wallAvoidWeight = 20f;

    [Header("Goal seek")]
    [Range(0, 3f)]
    public float goalWeight = 1f;


    [Header("Alignment")]
    public float alignDist = 4f;
    [Range(0, 3f)]
    public float alignWeight = 0.1f;

    [Header("Cohesion")]
    public float cohesionDist = 4f;
    [Range(0, 3f)]
    public float cohesionWeight = 1f;

    [Header("Separation")]
    public float separationDist = 1f;
    [Range(0, 3f)]
    public float separationWeight = 1.3f;
}