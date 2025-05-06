using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Circle : MonoBehaviour
{
    public Vector3 center;
    public float radius;
   //public int segment;
    public Color color;
    public float lineThickness;

    public Circle(Vector3 center, float radius, Color color, float lineThickness)
    {
        this.center = center;
        this.radius = radius;

        this.color = color;
        this.lineThickness = lineThickness;
    } /*   public Circle(Vector3 center, float radius, int segment, Color color, float linethickness)
    {
        this.center = center;
        this.radius = radius;
        this.segment = segment;
        this.color = color;
        this.linethickness = linethickness;
    }*/
}
