using UnityEngine;

namespace Util {   
    public static class Gizmos2D {
        public static void DrawCircle(Vector2 center, float radius, float duration = 0.0F) {
            DrawCircle(center, radius, Color.white, duration);
        }
        
        public static void DrawCircle(Vector2 center, float radius, Color color, float duration = 0.0F) {
            DrawPolygon(center, radius, color, segments: 16, duration: duration);
        }

        public static void DrawPolygon(Vector2 center, float radius, Color color, int segments, float duration = 0.0F) {
            float deltaTheta = 2 * Mathf.PI / segments;
            float theta = 0.0F;
            float x0 = Mathf.Cos(theta) * radius;
            float y0 = Mathf.Sin(theta) * radius;
            for (int i = 0; i <= segments; i++) {
                Vector2 start = new Vector2(x0, y0) + center;

                theta += deltaTheta;

                float x1 = Mathf.Cos(theta) * radius;
                float y1 = Mathf.Sin(theta) * radius;
                Vector2 end = new Vector2(x1, y1) + center;

                Debug.DrawLine(start, end, color, duration);

                x0 = x1;
                y0 = y1;
            }
        }

        public static void DrawRectangle(Vector2 p, Vector2 q, float duration = 0.0F) {
            DrawRectangle(p, q, Color.white, duration);
        }
        public static void DrawRectangle(Vector2 a, Vector2 c, Color color, float duration = 0.0F) {
            Vector2 b = new(a.x, c.y);
            Vector2 d = new(c.x, a.y);
            DrawQuad(a, b, c, d, color, duration);
        }

        // public static void DrawBox(Vector2 center, Vector2 size, float angle, float duration = 0.0F) {
        //     DrawBox(center, size, angle, Color.white, duration);
        // }

        // public static void DrawBox(Vector2 center, Vector2 size, float angle, Color color, float duration = 0.0F) {
        //     float theta = angle * Mathf.Deg2Rad;
        //     float cos = Mathf.Cos(theta);
        //     float sin = Mathf.Sin(theta);

        //     Vector2 forward = Vector2.up * size.y;
        //     forward.x = forward.x * cos - forward.y * sin;
        //     forward.y = forward.x * sin + forward.y * cos;

        //     Vector2 leftward = Vector2.Perpendicular(forward);

        //     Vector2 top = center + forward;
        //     Vector2 bottom = center - forward;
        //     Vector2 left = center + leftward;
        //     Vector2 right = center - leftward;

        //     Vector2 a = center + forward + leftward;
        //     Vector2 b = center - forward + leftward;
        //     Vector2 c = center - forward - leftward;
        //     Vector2 d = center + forward - leftward;

        //     Debug.DrawLine(top, bottom, Color.red, 1.0F);
        //     Debug.DrawLine(left, right, Color.red, 1.0F);
        //     DrawQuad(a, b, c, d, duration);
        // }

        public static void DrawQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float duration = 0.0F) {
            DrawQuad(a, b, c, d, Color.white, duration);
        }

        public static void DrawQuad(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color color, float duration = 0.0F) {
            Debug.DrawLine(a, b, color, duration);
            Debug.DrawLine(b, c, color, duration);
            Debug.DrawLine(c, d, color, duration);
            Debug.DrawLine(d, a, color, duration);
        }

        public static void DrawTriangle(Vector2 a, Vector2 b, Vector2 c, float duration = 0.0F) {
            DrawTriangle(a, b, c, Color.white, duration);
        }

        public static void DrawTriangle(Vector2 a, Vector2 b, Vector2 c, Color color, float duration = 0.0F) {
            Debug.DrawLine(a, b, color, duration);
            Debug.DrawLine(b, c, color, duration);
            Debug.DrawLine(c, a, color, duration);
        }
    }
}