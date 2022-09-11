using UnityEngine;

namespace RPG
{
    public class AIVisualization : MonoBehaviour
    {
        public Vector2[] vectors;
        public int vectorCount = 10;

        private void Start()
        {
            vectors = GetCircleVectors(vectorCount);
        }

        public Vector2[] GetCircleVectors(int count)
        {
            Vector2[] vector2s = new Vector2[count];
            for (int i = 0; i < count; i++)
            {
                float angle = 2 * i * Mathf.PI / count;
                vector2s[i].x = Mathf.Cos(angle);
                vector2s[i].y = Mathf.Sin(angle);
            }
            return vector2s;
        }

        public float[] GetVectorValue(Vector2[] vectors)
        {
            Vector2 mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            mouseDir = mouseDir.normalized;
            float[] values = new float[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
            {
                values[i] = mouseDir.x * vectors[i].x + mouseDir.y * vectors[i].y;
            }
            return values;
        }

        private void DrawVectorGizmos(Vector2 origin, Vector2[] vectors)
        {
            if (vectors != null)
            {
                float[] values = GetVectorValue(vectors);

                for (int i = 0; i < vectors.Length; i++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(origin, origin + vectors[i] * values[i]);
                }
            }
        }

        private void OnDrawGizmos()
        {
            DrawVectorGizmos(transform.position, vectors);
        }
    }
}