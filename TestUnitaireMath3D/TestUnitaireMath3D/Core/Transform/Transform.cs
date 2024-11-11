using System;
using Maths_Matrices.Tests;

public class Transform
{
    #region Translation
    
    private Vector3 _localPosition = Vector3.Zero;
    private MatrixFloat _localTranslationMatrix = MatrixFloat.Identity(4);
    public Vector3 LocalPosition
    {
        get => _localPosition;
        set
        {
            _localPosition = value;
            _localTranslationMatrix[0, 3] = _localPosition.x;
            _localTranslationMatrix[1, 3] = _localPosition.y;
            _localTranslationMatrix[2, 3] = _localPosition.z;
        }
    }
    public MatrixFloat LocalTranslationMatrix => _localTranslationMatrix;

    #endregion

    #region Rotation

    private Vector3 _localRotation = Vector3.Zero;
    public Vector3 LocalRotation
    {
        get => _localRotation;
        set
        {
            _localRotation = value;
            float angleX = _localRotation.x * (float)Math.PI / 180;
            LocalRotationXMatrix[1, 1] = (float)Math.Cos(angleX);
            LocalRotationXMatrix[1, 2] = -(float)Math.Sin(angleX);
            LocalRotationXMatrix[2, 1] = (float)Math.Sin(angleX);
            LocalRotationXMatrix[2, 2] = (float)Math.Cos(angleX);
            
            float angleY = _localRotation.y * (float)Math.PI / 180;
            LocalRotationYMatrix[0, 0] = (float)Math.Cos(angleY);
            LocalRotationYMatrix[0, 2] = (float)Math.Sin(angleY);
            LocalRotationYMatrix[2, 0] = -(float)Math.Sin(angleY);
            LocalRotationYMatrix[2, 2] = (float)Math.Cos(angleY);
            
            float angleZ = _localRotation.z * (float)Math.PI / 180;
            LocalRotationZMatrix[0, 0] = (float)Math.Cos(angleZ);
            LocalRotationZMatrix[0, 1] = -(float)Math.Sin(angleZ);
            LocalRotationZMatrix[1, 0] = (float)Math.Sin(angleZ);
            LocalRotationZMatrix[1, 1] = (float)Math.Cos(angleZ);

            LocalRotationMatrix = LocalRotationYMatrix * LocalRotationXMatrix * LocalRotationZMatrix;
        }
    }
    public MatrixFloat LocalRotationXMatrix { get; private set; } = MatrixFloat.Identity(4);
    public MatrixFloat LocalRotationYMatrix { get; private set; } = MatrixFloat.Identity(4);
    public MatrixFloat LocalRotationZMatrix { get; private set; } = MatrixFloat.Identity(4);
    public MatrixFloat LocalRotationMatrix { get; private set; } = MatrixFloat.Identity(4);
    
    #endregion

    #region Scale

    private Vector3 _localScale = Vector3.One;
    public Vector3 LocalScale
    {
        get => _localScale;
        set
        {
            _localScale = value;
            LocalScaleMatrix[0, 0] = _localScale.x;
            LocalScaleMatrix[1, 1] = _localScale.y;
            LocalScaleMatrix[2, 2] = _localScale.z;
        }
    }
    public MatrixFloat LocalScaleMatrix { get; private set; } = MatrixFloat.Identity(4);

    #endregion

    #region TRSMatrixs
    public MatrixFloat LocalToWorldMatrix
    {
        get
        {
            if (Parent == null)
            {
                return LocalTranslationMatrix * LocalRotationMatrix * LocalScaleMatrix;
            }
            else
            {
                return Parent.LocalToWorldMatrix * (LocalTranslationMatrix * LocalRotationMatrix * LocalScaleMatrix);
            }
        }
    }
    public MatrixFloat WorldToLocalMatrix => MatrixFloat.InvertByRowReduction(LocalToWorldMatrix);

    #endregion
    public Transform Parent { get; private set; }
    public Vector3 WorldPosition
    {
        get
        {
            MatrixFloat matrix = LocalToWorldMatrix;
            return new Vector3(matrix[0, 3], matrix[1, 3], matrix[2, 3]);
        }
        set
        {
            LocalPosition = value;
            if (Parent != null)
            {
                MatrixFloat worldTRS = WorldToLocalMatrix;
                LocalPosition = new Vector3(worldTRS[0, 3], worldTRS[1, 3], worldTRS[2, 3]);
            }
        }
    }
    public Quaternion LocalRotationQuaternion
    {
        get
        {
            Vector3 rotation = LocalRotation;
            return Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        }
        set
        {
            LocalRotation = value.EulerAngles;
        }
    }

    public Transform(Vector3 localPosition = null)
    {
        if(localPosition != null) {LocalPosition = localPosition;}
    }
    
    public void SetParent(Transform tParent) => Parent = tParent;
}
