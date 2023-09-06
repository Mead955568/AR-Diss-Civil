using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emergency : MonoBehaviour
{
    public Transform person; // The person who needs to find the exit
    public Transform[] exits; // List of emergency exit locations
    public Color lineColor = Color.red; // Color of the navigation path
    public float lineWidth = 0.2f; // Width of the line

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        FindClosestExit();
    }

    private void FindClosestExit()
    {
        if (exits.Length == 0)
        {
            Debug.LogError("No exit locations provided.");
            return;
        }

        Transform closestExit = null;
        float shortestDistance = float.MaxValue;

        foreach (Transform exit in exits)
        {
            float distance = Vector3.Distance(person.position, exit.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestExit = exit;
            }
        }

        if (closestExit != null)
        {
            DrawPathToExit(closestExit.position);
        }
    }

    private void DrawPathToExit(Vector3 exitPosition)
    {
        // Set the line renderer positions
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, person.position);
        lineRenderer.SetPosition(1, exitPosition);
    }
}
