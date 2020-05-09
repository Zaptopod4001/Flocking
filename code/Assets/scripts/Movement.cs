using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Copyright Sami S.

// use of any kind without a written permission 
// from the author is not allowed.

// DO NOT:
// Fork, clone, copy or use in any shape or form.

public class Movement : MonoBehaviour
{

    // Boids rules
    // Alignment - Try to align direction to direction of neighbors
    // Cohesion - Seek towards average position of neigbors
    // Separation - Try to keep distance to nearby agents

    [Header("Collision")]
    [SerializeField] LayerMask wallMask;
    [SerializeField] LayerMask agentMask;
    [SerializeField] Rigidbody2D rb;

    [Header("Debug")]
    [SerializeField] Vector2 velocity;
    [SerializeField] int stuck;

    // local
    Vector3 lastPos;
    float targetAngle;
    Transform goal;


    // Init

    public void Init(Transform goal)
    {
        this.goal = goal;
        rb.velocity = rb.transform.up * Spawn.Settings.speed;
    }




    // Update

    void FixedUpdate()
    {
        Move();
    }


    void Move()
    {
        // Boids
        velocity = rb.velocity;

        var v1 = Steer((Vector2)GetAlignment()) * Spawn.Settings.alignWeight;
        var v2 = Steer((Vector2)GetCohesion()) * Spawn.Settings.cohesionWeight;
        var v3 = Steer((Vector2)GetSeparation()) * Spawn.Settings.separationWeight;
        var v4 = Steer((Vector2)GetWallAvoidDir()) * Spawn.Settings.wallAvoidWeight;
        var v5 = Steer((Vector2)GetGoalDirection()) * Spawn.Settings.goalWeight;

        // accelaration this frame
        Vector2 accel = Vector3.zero;
        accel += v1 + v2 + v3 + v4 + v5;


        // set speed from acceleration
        velocity += accel * Time.deltaTime;

        // limit max speed
        var dir = velocity.normalized;
        rb.velocity = dir * Mathf.Clamp(velocity.magnitude, Spawn.Settings.speed, Spawn.Settings.speedMax);


        // Visual rotation
        if (velocity.magnitude > 0.02f)
        {
            targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            var rot = Quaternion.Euler(0, 0, targetAngle);
            rb.MoveRotation(rot.eulerAngles.z);
        }

        lastPos = rb.position;
    }


    Vector2 Steer(Vector2 vec)
    {
        // new direction, counter existing velo
        Vector2 v = vec.normalized * Spawn.Settings.speedMax - velocity;

        // clamp speed
        return Vector3.ClampMagnitude(v, Spawn.Settings.steerSpeedMax);
    }





    // Collision

    // Wall avoid
    Vector3 GetWallAvoidDir()
    {
        Vector3 distantDir = Vector3.zero;
        float dist = 0;
        int rayCount = Spawn.Settings.viewRays;


        // Get free directions
        var freeDirs = new List<Vector3>();

        for (int i = 0; i < rayCount; i++)
        {
            var angle = Mathf.Lerp(-Spawn.Settings.viewAngle / 2f, Spawn.Settings.viewAngle / 2f, i / ((float)rayCount - 1));
            var v = Quaternion.Euler(0, 0, angle) * rb.transform.up;
            var hit = Physics2D.Raycast(rb.position, v, Spawn.Settings.viewDist, wallMask);

            // Store free directions
            if (hit.collider == null)
                freeDirs.Add(v);

            // Store furthest hit and its dir
            if (hit.collider != null)
            {
                if (hit.distance > dist)
                {
                    dist = hit.distance;
                    distantDir = v.normalized;
                }
            }
        }


        // No wall hits, return current dir
        if (freeDirs.Count == rayCount)
        {
            return Vector3.zero;
        }

        // Some wall hits, return average of free dirs
        if (freeDirs.Count > 0 && freeDirs.Count < rayCount)
        {
            var a = Vector3.zero;

            foreach (var f in freeDirs)
            {
                a += f;
            }

            a = a.normalized;
            // Debug.DrawRay(rb.position, a * 2f, Color.cyan);

            return a;
        }


        // Wall ahead
        stuck++;

        return Vector2.zero;
    }


    // Goal seek
    Vector3 GetGoalDirection()
    {
        return (goal.position - rb.transform.position).normalized;
    }




    // Boids

    // Alignment
    Vector3 GetAlignment()
    {
        var cols = Physics2D.OverlapCircleAll(rb.position, Spawn.Settings.alignDist, agentMask);

        if (cols.Length <= 1)
            return Vector3.zero;


        // Get average dir of closeby agents
        var vec = Vector3.zero;

        foreach (var c in cols)
        {
            if (c.gameObject != this.gameObject)
            {
                vec += (Vector3)c.transform.up;
            }
        }

        vec = (vec / cols.Length).normalized;
        // Debug.DrawRay(rb.position, vec, Color.white);

        return vec;
    }


    // Cohesion (flock)
    Vector3 GetCohesion()
    {
        var cols = Physics2D.OverlapCircleAll(rb.position, Spawn.Settings.cohesionDist, agentMask);

        if (cols.Length <= 1)
            return Vector2.zero;

        // Flock center
        var vec = Vector3.zero;

        foreach (var c in cols)
        {
            vec += c.transform.position;
        }

        // Center
        vec = vec / cols.Length;
        // Debug.DrawRay(vec, Vector3.up * 0.2f, Color.white);


        // Direction towards center        
        var dir = (vec - (Vector3)rb.position);

        dir = dir.normalized;
        // Debug.DrawRay(rb.position, dir, Color.cyan);

        return dir;
    }


    // Separation
    Vector3 GetSeparation()
    {
        var cols = Physics2D.OverlapCircleAll(rb.position, Spawn.Settings.separationDist, agentMask);

        if (cols.Length <= 1)
            return Vector2.zero;

        // Repel dir
        var vec = Vector3.zero;

        foreach (var c in cols)
        {
            if (IsInView(c.transform))
                vec += (c.transform.position - rb.transform.position);
        }

        vec = vec / cols.Length;
        vec = -vec;
        vec = vec.normalized;
        // Debug.DrawRay(rb.position, vec, Color.magenta);

        return vec;
    }




    // Other

    bool IsInView(Transform agent)
    {
        var otherAgent = rb.transform.InverseTransformPoint(agent.position);
        var angle = Vector2.Angle(otherAgent, Vector2.up);
        return angle < Spawn.Settings.perceptionAngle / 2f;
    }

}
