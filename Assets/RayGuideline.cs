using UnityEngine;

public class RayGuideline
{
    private LineRenderer lineRenderer;
    private float lineSize;

    public RayGuideline(float lineSize = 0.005f)
    {
        initLineRenderer();
        this.lineSize = lineSize;
    }

    void initLineRenderer()
    {
        GameObject lineObj = new GameObject("RayGuideline");

        lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));
    }

    public void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        if (lineRenderer == null)
        {
            initLineRenderer();
        }

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void Destroy()
    {
        if (lineRenderer != null)
        {
            Object.Destroy(lineRenderer.gameObject);
            lineRenderer = null;
        }
    }
}
