using UnityEngine;
using Pathfinding;
using System.Collections;
using System;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    // What to chase
    public Transform target;

    // How many times a second we will update our path
    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    // The calculated path
    public Path path;

    // The AI's speed per second
    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;

    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;

    private bool searchingForPlayer = false;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if(target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator UpdatePath()
    {
        if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield return null;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());

    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("We got a path. " + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if(sResult == null)
        {
            yield return new WaitForSeconds(1 / updateRate);
            StartCoroutine(SearchForPlayer());
        } else
        {
            target = sResult.transform;
            searchingForPlayer = false;
            StartCoroutine(UpdatePath());
            yield return false;
        }
    }

    private void FixedUpdate()
    {
        if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        //TODO: Always look at player
        if (path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            Debug.Log("End of path reached");
            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        //Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //Move the AI
        rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }


    }

}
