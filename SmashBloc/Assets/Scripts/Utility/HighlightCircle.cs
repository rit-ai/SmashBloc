using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HighlightCircle : MonoBehaviour {

    private const int segments = 50;
    private const float width = 1f;

    private LineRenderer line;
    private float radius;

	public void Init (float radius, Color c = default(Color)) {
        this.radius = radius;

        line = gameObject.GetComponent<LineRenderer>();
        if (line == null) { line = gameObject.AddComponent<LineRenderer>(); }

        line.positionCount = segments;
        line.numCapVertices = segments;
        line.numCornerVertices = segments;
        line.useWorldSpace = false;

        CreateCircle();
        SetColor(c);
	}
	
	private void CreateCircle()
    {
        float x, z, angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.startWidth = width;
            line.endWidth = width;
            line.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / segments);
        }
    }

    private void SetColor(Color c)
    {
        Color color;

        if (c == default(Color))
        {
            color = gameObject.GetComponent<MeshRenderer>().material.color;
        }
        else
        {
            color = c;
        }

        line.startColor = color;
        line.endColor = color;
    }
}
