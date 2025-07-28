using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShowTrajectory : MonoBehaviour
{
    public float minDistance = 0.1f;

    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private bool isTracking = false;
    private Vector3 lastPoint;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (!isTracking)
            return;

        Vector3 currentPosition = transform.position;
        if (points.Count == 0 || Vector3.Distance(lastPoint, currentPosition) >= minDistance)
        {
            AddPoint(currentPosition);
        }
    }

    public void Show()
    {
        isTracking = true;
        points.Clear();
        AddPoint(transform.position);
        lineRenderer.enabled = true;
    }

    public void Hide()
    {
        isTracking = false;
        lineRenderer.enabled = false;
    }

    private void AddPoint(Vector3 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
        lastPoint = point;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isTracking = false;
    }
}
