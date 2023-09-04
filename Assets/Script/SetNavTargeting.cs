using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetNavTargeting : MonoBehaviour
{
    [SerializeField]
    private Camera _topDownCamera;
    [SerializeField]
    private GameObject _navTargetObject;

    private NavMeshPath _path; // Current Calculated Path
    private LineRenderer _lineRenderer; // Linerenderer To Display Path

    private bool _lineToggle = false;

    private void Start() // Create New Path
    {
        _path = new NavMeshPath();
        LineRenderer lineRenderer= transform.GetComponent<LineRenderer>();
    }

    private void Update() // When Touch Screen LineToggle Toggled
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            _lineToggle = !_lineToggle;
        }

        if (_lineToggle)
        {
            NavMesh.CalculatePath(transform.position, _navTargetObject.transform.position, NavMesh.AllAreas, _path);
            _lineRenderer.positionCount = _path.corners.Length;
            _lineRenderer.SetPositions(_path.corners);
            _lineRenderer.enabled = true;
        }
    }
}
