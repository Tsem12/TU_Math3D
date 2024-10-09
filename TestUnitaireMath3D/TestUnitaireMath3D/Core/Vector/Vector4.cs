using Maths_Matrices.Tests;

public class Vector4
{
    private float _x;
    private float _y;
    private float _z;
    private float _w;
    
    public float x => _x;
    public float y => _y;
    public float z => _z;
    public float w => _w;
    
    public Vector4(float x = 0f, float y = 0f, float z = 0f, float w = 0f)
    {
        _x = x;
        _y = y;
        _z = z;
        _w = w;
    }
    
    public static Vector4 operator +(Vector4 v1, Vector4 v2) => new Vector4(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
    public static Vector4 operator -(Vector4 v1, Vector4 v2) => new Vector4(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
    public static Vector4 operator *(Vector4 v1, Vector4 v2) => new Vector4(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
    public static Vector4 operator /(Vector4 v1, Vector4 v2) => new Vector4(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z, v1.w / v2.w);
    
    public static Vector4 operator +(Vector4 v1, float value) => new Vector4(v1.x + value, v1.y + value, v1.z + value, v1.w + value);
    public static Vector4 operator -(Vector4 v1, float value) => new Vector4(v1.x - value, v1.y - value, v1.z - value, v1.w - value);
    public static Vector4 operator *(Vector4 v1, float value) => new Vector4(v1.x * value, v1.y * value, v1.z * value, v1.w * value);
    public static Vector4 operator /(Vector4 v1, float value) => new Vector4(v1.x / value, v1.y / value, v1.z / value, v1.w / value);

    public static Vector4 operator *(MatrixFloat matrix, Vector4 v1) => (matrix * MatrixFloat.Transpose(v1.ToMatrix())).ToVector4();

    public MatrixFloat ToMatrix() => new MatrixFloat(new float[,]
    {
        {
            _x, _y, _z, _w
        }
    });
}