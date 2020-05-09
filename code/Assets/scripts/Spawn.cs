using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Copyright Sami S.

// use of any kind without a written permission 
// from the author is not allowed.

// DO NOT:
// Fork, clone, copy or use in any shape or form.

public class Spawn : MonoBehaviour
{
    public static Spawn instance;

    [Header("Agent prefab")]
    public GameObject prefab;

    [Header("Goal prefab")]
    public Transform goalTra;

    [Header("Agent Spawning")]
    public int count = 50;
    public float distance = 20;

    [Header("Agent settings")]
    public Settings settings;


    // Properties 

    public static Settings Settings
    {
        get => instance.settings;
    }


    // Init 

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CloneAgents();
    }

    void Update()
    {
        SetGoalPosition();
    }




    // Create

    void CloneAgents()
    {
        for (int i = 0; i < count; i++)
        {
            var clone = Instantiate(prefab);

            // position
            var px = UnityEngine.Random.Range(-distance, distance);
            var py = UnityEngine.Random.Range(-distance, distance);
            clone.transform.position = new Vector3(px, py);

            // heading
            var rot = UnityEngine.Random.Range(0, 360);
            clone.transform.rotation = Quaternion.Euler(0, 0, rot);

            // set goal
            if (goalTra != null)
                clone.GetComponent<Movement>().Init(goalTra);

            // parent
            clone.transform.SetParent(transform);
        }
    }

    
    void SetGoalPosition()
    {
        if (Input.GetMouseButton(0))
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            goalTra.transform.position = (Vector2)pos;
        }
    }

}
