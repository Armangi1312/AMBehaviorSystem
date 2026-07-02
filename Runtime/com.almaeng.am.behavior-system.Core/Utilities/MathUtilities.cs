using System;
using UnityEngine;

namespace AMBehaviorSystem.Core.Utilities
{
    public static class MathUtilities
    {
        public static int Abs(int value) => Math.Abs(value);
        public static float Abs(float value) => MathF.Abs(value);
        public static double Abs(double value) => Math.Abs(value);

        public static float Sin(float value) => MathF.Sin(value);
        public static double Sin(double value) => Math.Sin(value);

        public static float Cos(float value) => MathF.Cos(value);
        public static double Cos(double value) => Math.Cos(value);

        public static float Tan(float value) => MathF.Tan(value);
        public static double Tan(double value) => Math.Tan(value);

        public static float Atan2(float y, float x) => MathF.Atan2(y, x);
        public static double Atan2(double y, double x) => Math.Atan2(y, x);

        public static int Ceil(float value) => (int)MathF.Ceiling(value);
        public static int Ceil(double value) => (int)Math.Ceiling(value);

        public static int Floor(float value) => (int)MathF.Floor(value);
        public static int Floor(double value) => (int)Math.Floor(value);

        public static int Round(float value) => (int)MathF.Round(value);
        public static int Round(double value) => (int)Math.Round(value);

        public static float Sqrt(float value) => MathF.Sqrt(value);
        public static double Sqrt(double value) => Math.Sqrt(value);

        public static float Pow(float a, float b) => MathF.Pow(a, b);
        public static double Pow(double a, double b) => Math.Pow(a, b);

        public static float Log(float value) => MathF.Log(value);
        public static double Log(double value) => Math.Log(value);

        public static float Log(float value, float base_) => MathF.Log(value, base_);
        public static double Log(double value, double base_) => Math.Log(value, base_);

        public static float Log10(float value) => MathF.Log10(value);
        public static double Log10(double value) => Math.Log10(value);

        public static float Exp(float value) => MathF.Exp(value);
        public static double Exp(double value) => Math.Exp(value);

        public static int Min(int a, int b) => Math.Min(a, b);
        public static float Min(float a, float b) => MathF.Min(a, b);
        public static double Min(double a, double b) => Math.Min(a, b);

        public static int Max(int a, int b) => Math.Max(a, b);
        public static float Max(float a, float b) => MathF.Max(a, b);
        public static double Max(double a, double b) => Math.Max(a, b);

        public static int Clamp(int value, int min, int max) => Math.Clamp(value, min, max);
        public static float Clamp(float value, float min, float max) => Math.Clamp(value, min, max);
        public static double Clamp(double value, double min, double max) => Math.Clamp(value, min, max);

        public static int Modulo(int a, int b) => a % b;
        public static float Modulo(float a, float b) => a % b;
        public static double Modulo(double a, double b) => a % b;

        public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
        }

        public static double Remap(double value, double fromMin, double fromMax, double toMin, double toMax)
        {
            return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
        }

        public static int Random(int min, int max) => UnityEngine.Random.Range(min, max);
        public static float Random(float min, float max) => UnityEngine.Random.Range(min, max);
        public static double Random(double min, double max) => (double)UnityEngine.Random.Range((float)min, (float)max);

        public static float Distance(float a, float b) => MathF.Abs(a - b);
        public static float Distance(Vector2 a, Vector2 b) => Vector2.Distance(a, b);
        public static float Distance(Vector3 a, Vector3 b) => Vector3.Distance(a, b);
        public static float Distance(Vector4 a, Vector4 b) => Vector4.Distance(a, b);

        public static float Distance(Vector2 a, Vector3 b) => Vector3.Distance(new Vector3(a.x, a.y, 0f), b);
        public static float Distance(Vector3 a, Vector2 b) => Vector3.Distance(a, new Vector3(b.x, b.y, 0f));
        public static float Distance(Vector3 a, Vector4 b) => Vector4.Distance(new Vector4(a.x, a.y, a.z, 0f), b);
        public static float Distance(Vector4 a, Vector3 b) => Vector4.Distance(a, new Vector4(b.x, b.y, b.z, 0f));
        public static float Distance(Vector2 a, Vector4 b) => Vector4.Distance(new Vector4(a.x, a.y, 0f, 0f), b);
        public static float Distance(Vector4 a, Vector2 b) => Vector4.Distance(a, new Vector4(b.x, b.y, 0f, 0f));

        public static float Dot(Vector2 a, Vector2 b) => Vector2.Dot(a, b);
        public static float Dot(Vector3 a, Vector3 b) => Vector3.Dot(a, b);
        public static float Dot(Vector4 a, Vector4 b) => Vector4.Dot(a, b);
        public static float Dot(Vector2 a, Vector3 b) => Vector3.Dot(new Vector3(a.x, a.y, 0f), b);
        public static float Dot(Vector3 a, Vector2 b) => Vector3.Dot(a, new Vector3(b.x, b.y, 0f));
        public static float Dot(Vector3 a, Vector4 b) => Vector4.Dot(new Vector4(a.x, a.y, a.z, 0f), b);
        public static float Dot(Vector4 a, Vector3 b) => Vector4.Dot(a, new Vector4(b.x, b.y, b.z, 0f));
        public static float Dot(Vector2 a, Vector4 b) => Vector4.Dot(new Vector4(a.x, a.y, 0f, 0f), b);
        public static float Dot(Vector4 a, Vector2 b) => Vector4.Dot(a, new Vector4(b.x, b.y, 0f, 0f));

        public static Vector2 Normalize(Vector2 value) => value.normalized;
        public static Vector3 Normalize(Vector3 value) => value.normalized;
        public static Vector4 Normalize(Vector4 value) => value.normalized;

        public static float Magnitude(Vector2 value) => value.magnitude;
        public static float Magnitude(Vector3 value) => value.magnitude;
        public static float Magnitude(Vector4 value) => value.magnitude;

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t) => Vector2.Lerp(a, b, t);
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => Vector3.Lerp(a, b, t);
        public static Vector4 Lerp(Vector4 a, Vector4 b, float t) => Vector4.Lerp(a, b, t);
        public static Vector3 Lerp(Vector2 a, Vector3 b, float t) => Vector3.Lerp(new Vector3(a.x, a.y, 0f), b, t);
        public static Vector3 Lerp(Vector3 a, Vector2 b, float t) => Vector3.Lerp(a, new Vector3(b.x, b.y, 0f), t);
        public static Vector4 Lerp(Vector3 a, Vector4 b, float t) => Vector4.Lerp(new Vector4(a.x, a.y, a.z, 0f), b, t);
        public static Vector4 Lerp(Vector4 a, Vector3 b, float t) => Vector4.Lerp(a, new Vector4(b.x, b.y, b.z, 0f), t);
        public static Vector4 Lerp(Vector2 a, Vector4 b, float t) => Vector4.Lerp(new Vector4(a.x, a.y, 0f, 0f), b, t);
        public static Vector4 Lerp(Vector4 a, Vector2 b, float t) => Vector4.Lerp(a, new Vector4(b.x, b.y, 0f, 0f), t);
    }
}