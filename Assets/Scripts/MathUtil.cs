using UnityEngine;


public class MathUtil
{

    public static Vector2 RandomPointUnitCircle()
    {
        float radians = Random.Range(0, 2*Mathf.PI);
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    public static float Dot(Vector2 a, Vector2 b)
    {
        return a.x * b.x + a.y * b.y;
    }
}

