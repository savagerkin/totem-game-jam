using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CircularLoadingBar : MonoBehaviour
{
    [SerializeField][Range(0.0f, 1.0f)] private float value = 1.0f;
    [SerializeField] bool clockwise = true;
    [SerializeField] private int vertexCount = 100;

    [Header("Fill")]
    [SerializeField] CanvasRenderer fill;
    [SerializeField] private float radius = 100;
    [SerializeField] private float thickness = 40;
    [SerializeField] private Color fillColor = Color.white;

    [Header("Background")]
    [SerializeField] CanvasRenderer background;
    [SerializeField] private Color backgroundColor = Color.black;

    private void Awake()
    {
        if (background == null) {
            GameObject backgroundObject = Instantiate(new GameObject(), transform);
            backgroundObject.name = "Background";
            backgroundObject.AddComponent<RectTransform>();
            background = backgroundObject.AddComponent<CanvasRenderer>();
        }

        if (fill == null) {
            GameObject fillObject = Instantiate(new GameObject(), transform);
            fillObject.name = "Fill";
            fillObject.AddComponent<RectTransform>();
            fill = fillObject.AddComponent<CanvasRenderer>();
        }

        UpdateGraphics();
    }

    public void SetValue(float value) { 
        this.value = Mathf.Clamp(value, 0, 1);

        if (fill != null) {
            UpdateFill();
        }
    }

    public void SetFillColor(Color color)
    {
        this.fillColor = color;

        if (fill != null)
        {
            UpdateFill();
        }
    }

    private void OnValidate()
    {
        UpdateGraphics();
    }

    private void OnEnable()
    {
        UpdateGraphics();
    }

    private void OnDisable()
    {
        if (fill != null)
        {
            fill.Clear();
        }

        if (background != null)
        {
            background.Clear();
        }
    }

    private void UpdateGraphics() {
        value = Mathf.Clamp(value, 0, 1);

        if (vertexCount % 2 != 0)
        {
            vertexCount += 1;
        }
        vertexCount = Mathf.Max(vertexCount, 8);

        if (fill != null)
        {
            UpdateFill();
        }

        if (background != null)
        {
            UpdateBackground();
        }
    }

    private void UpdateFill() {
        float target_angle = 2 * Mathf.PI * value;

        if (clockwise) {
            target_angle *= -1;
        }

        Mesh mesh = CreateCircularMesh(vertexCount, target_angle, false, fillColor);
        Material defaultCanvasMaterial = Canvas.GetDefaultCanvasMaterial();

        fill.SetMaterial(defaultCanvasMaterial, null);
        fill.SetMesh(mesh);
    }

    private void UpdateBackground() {
        Mesh mesh = CreateCircularMesh(vertexCount - 2, 2 * Mathf.PI, true, backgroundColor);
        Material defaultCanvasMaterial = Canvas.GetDefaultCanvasMaterial();

        background.SetMaterial(defaultCanvasMaterial, null);
        background.SetMesh(mesh);
    }

    private Mesh CreateCircularMesh(int vertexCount, float angle, bool closed, Color color) {
        List<Vector3> vertices = new();
        List<int> triangles = new();
        List<Color> colors = new();

        int parts = vertexCount / 2;

        for (int i = 0; i < parts; i++)
        {
            float a;

            if (closed)
            {
                a = angle * i / parts;
            }
            else {
                a = angle * i / (parts - 1);
            }

            float cos_a = Mathf.Cos(a);
            float sin_a = Mathf.Sin(a);

            Vector3 v1 = new(radius * cos_a, radius * sin_a);
            Vector3 v2 = new((radius + thickness) * cos_a, (radius + thickness) * sin_a);

            vertices.Add(v1);
            vertices.Add(v2);

            colors.Add(color);
            colors.Add(color);

            int index = i * 2;

            if (i != parts - 1)
            {
                triangles.Add(index + 1);
                triangles.Add(index);
                triangles.Add(index + 3);

                triangles.Add(index);
                triangles.Add(index + 2);
                triangles.Add(index + 3);
            }
            else if (closed)
            {
                triangles.Add(index + 1);
                triangles.Add(index);
                triangles.Add(1);

                triangles.Add(index);
                triangles.Add(0);
                triangles.Add(1);
            }
        }

        Mesh mesh = new()
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            colors = colors.ToArray()
        };

        mesh.RecalculateBounds();

        return mesh;
    }
}
