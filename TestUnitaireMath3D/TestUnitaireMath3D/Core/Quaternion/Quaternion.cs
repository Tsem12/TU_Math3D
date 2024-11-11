using System;
using Maths_Matrices.Tests;

public struct Quaternion
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    public float w { get; set; }

    public Quaternion(float _x, float _y, float _z, float _w)
    {
        x = _x;
        y = _y;
        z = _z;
        w = _w;
    }

    public static Quaternion Identity = new Quaternion(0, 0, 0, 1);

    public MatrixFloat Matrix
    {
        get
        {
            return new MatrixFloat(new float[4, 4]
            {
                {1 - 2 * y * y - 2 * z * z,2 * x * y - 2 * w * z, 2 * x * z + 2 * w * y,0},
                {2 * x * y + 2 * w * z,1 - 2 * x * x - 2 * z * z,2 * y * z - 2 * w * x,0},
                {2 * x * z - 2 * w * y,2 * y * z + 2 * w * x, 1 - 2 * x * x - 2 * y * y,0},
                {0,0,0,1}
            });
        }
    }

    public Vector3 EulerAngles
    {
        get
        {
            MatrixFloat matrix = Matrix;
            float p = -(float)Math.Asin(matrix[1, 2]);
            float h = Math.Cos(p) != 0 ? (float)Math.Atan2(matrix[0,2], matrix[2,2]) : (float)Math.Atan2(-matrix[2,0], matrix[0,0]);
            float b = Math.Cos(p) != 0 ? (float)Math.Atan2(matrix[1, 0], matrix[1, 1]) : 0;
            return new Vector3(p, h, b) * (360 / (float)(Math.PI * 2));
        }
    }

    public static Quaternion AngleAxis(float angle, Vector3 axis)
    {
        float rad = angle * (float)(Math.PI / 180);
        Vector3 vect = axis.Normalized * (float)Math.Sin(rad / 2);
        return new Quaternion(vect.x, vect.y, vect.z, (float)Math.Cos(rad / 2));
    }

    public static Quaternion operator *(Quaternion q1, Quaternion q2) => new Quaternion(
        (q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y),
        (q1.w * q2.y - q1.x * q2.z + q1.y * q2.w + q1.z * q2.x),
        (q1.w * q2.z + q1.x * q2.y - q1.y * q2.x + q1.z * q2.w),
        (q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z));

    public static Vector3 operator *(Quaternion q1, Vector3 p1)
    {
        Vector4 point = new Vector4(p1.x, p1.y, p1.z, 0);
        MatrixFloat result = q1.Matrix * point.ToMatrix().Transpose();
        return new Vector3(result[0,0], result[1,0], result[2,0]);
    }

    public static Quaternion Euler(float x, float y, float z)
    {
        Quaternion qx = AngleAxis(x, Vector3.Forward);
        Quaternion qy = AngleAxis(y, Vector3.Right);
        Quaternion qz = AngleAxis(z, Vector3.Up);

        return qy * qx * qz;
    }
}   