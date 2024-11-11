using System;
using Maths_Matrices.Tests;

public class Vector3
{
    private float _x;
    private float _y;
    private float _z;
    
    public float x => _x;
    public float y => _y;
    public float z => _z;

    public Vector3 Normalized
    {
        get
        {
            float magnitude = (float)Math.Sqrt(Math.Pow(_x, 2) + Math.Pow(_y, 2) + Math.Pow(_z, 2));
            return new Vector3(_x / magnitude, _y / magnitude, _z / magnitude);
        }
    }

    public static Vector3 Zero => new Vector3(0, 0, 0);
    public static Vector3 One => new Vector3(1, 1, 1);
    public static Vector3 Forward => new Vector3(1, 0, 0);
    public static Vector3 Right => new Vector3(0, 1, 0);
    public static Vector3 Up => new Vector3(0, 0, 1);
    
    public Vector3(float x = 0f, float y = 0f, float z = 0f)
    {
        _x = x;
        _y = y;
        _z = z;
    }
    
    public static Vector3 operator +(Vector3 v1, Vector3 v2) => new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    public static Vector3 operator -(Vector3 v1, Vector3 v2) => new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    public static Vector3 operator *(Vector3 v1, Vector3 v2) => new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    public static Vector3 operator /(Vector3 v1, Vector3 v2) => new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
    
    public static Vector3 operator +(Vector3 v1, float value) => new Vector3(v1.x + value, v1.y + value, v1.z + value);
    public static Vector3 operator +(float value, Vector3 v1) => v1 + value;
    public static Vector3 operator -(Vector3 v1, float value) => new Vector3(v1.x - value, v1.y - value, v1.z - value);
    public static Vector3 operator -(float value, Vector3 v1) => v1 - value;
    public static Vector3 operator *(Vector3 v1, float value) => new Vector3(v1.x * value, v1.y * value, v1.z * value);
    public static Vector3 operator *(float value, Vector3 v1) => v1 * value;
    public static Vector3 operator /(Vector3 v1, float value) => new Vector3(v1.x / value, v1.y / value, v1.z / value);
    public static Vector3 operator /(float value, Vector3 v1) => v1 * value;

    public static Vector3 operator *(MatrixFloat matrix, Vector3 v1) => (matrix * MatrixFloat.Transpose(v1.ToMatrix())).ToVector3();
    

    public MatrixFloat ToMatrix() => new MatrixFloat(new float[,]
    {
        {
            _x, _y, _z
        }
    });
}