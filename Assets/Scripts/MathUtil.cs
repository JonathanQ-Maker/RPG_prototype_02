using UnityEngine;


public class MathUtil
{

    public static Vector2 RandomPointUnitCircle()
    {
        float radians = Random.Range(0, 2*Mathf.PI);
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }
}

