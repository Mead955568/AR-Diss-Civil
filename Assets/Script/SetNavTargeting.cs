using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SetNavTargeting : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown _navTargetDropDown;
    [SerializeField]
    private List<Target> _navTargetObjects = new List<Target>();
    [SerializeField]
    private Slider _navYOffset;

    private NavMeshPath _path; // Current Calculated Path
    private LineRenderer _lineRenderer; // Linerenderer To Display Path
    private Vector3 _targetPosition = Vector3.zero; // Current Target Position

    private bool _lineToggle = false;

    private void Start() // Create New Path
    {
        _path = new NavMeshPath();
        _lineRenderer = transform.GetComponent<LineRenderer>();
        _lineRenderer.enabled = _lineToggle;
    }

    private void Update() // Calculate Line Position
    {
        if (_lineToggle && _targetPosition != Vector3.zero) 
        {
            NavMesh.CalculatePath(transform.position, _targetPosition, NavMesh.AllAreas, _path);
            _lineRenderer.positionCount = _path.corners.Length;
            Vector3[] calculatedPathAndOffset = AddLineOffset();
            _lineRenderer.SetPositions(calculatedPathAndOffset);
        }
    }
    public void SetCurrentNavTarget(int selectedValue)
    {
        _targetPosition = Vector3.zero; // Resets Target Position
        string selectedText = _navTargetDropDown.options[selectedValue].text;
        Target currentTarget = _navTargetObjects.Find(x => x.Name.Equals(selectedText));
        if (currentTarget != null)
        {
            _targetPosition = currentTarget.PositionObject.transform.position;
        }
    }
    public void ToggleVisibility()
    {
        _lineToggle = !_lineToggle;
        _lineRenderer.enabled = _lineToggle;
        Debug.Log("Toggle Line Vis");
    }
    public Vector3[] AddLineOffset() 
    {
        if (_navYOffset.value == 0)
        {
            return _path.corners;
        }

        Vector3[] calculatedLine = new Vector3[_path.corners.Length];
        for (int i = 0; i < _path.corners.Length; i++)
        {
            calculatedLine[i] = _path.corners[i] + new Vector3(0, _navYOffset.value, 0);
        }
        return calculatedLine;
    }

}
