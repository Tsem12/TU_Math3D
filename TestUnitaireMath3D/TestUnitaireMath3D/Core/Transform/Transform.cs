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
            _localPosition = value;
            float angleX = _localPosition.x * (float)Math.PI / 180;
            LocalRotationXMatrix[1, 1] = (float)Math.Cos(angleX);
            LocalRotationXMatrix[1, 2] = -(float)Math.Sin(angleX);
            LocalRotationXMatrix[2, 1] = (float)Math.Sin(angleX);
            LocalRotationXMatrix[2, 2] = (float)Math.Cos(angleX);
            
            float angleY = _localPosition.y * (float)Math.PI / 180;
            LocalRotationYMatrix[0, 0] = (float)Math.Cos(angleY);
            LocalRotationYMatrix[0, 2] = (float)Math.Sin(angleY);
            LocalRotationYMatrix[2, 0] = -(float)Math.Sin(angleY);
            LocalRotationYMatrix[2, 2] = (float)Math.Cos(angleY);
            
            float angleZ = _localPosition.z * (float)Math.PI / 180;
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

    public Transform Parent { get; private set; }
    
    public Transform(Vector3 localPosition = null, Vector3 localRotation = null, Vector3 localScale = null)
    {
        if(localPosition != null) {LocalPosition = localPosition;}
        if(localRotation != null) {LocalRotation = localRotation;}
        if(localScale != null) {LocalScale = localScale;}
    }

    public MatrixFloat LocalToWorldMatrix => LocalTranslationMatrix * LocalRotationMatrix * LocalScaleMatrix;
    public MatrixFloat WorldToLocalMatrix => MatrixFloat.InvertByRowReduction(LocalToWorldMatrix);

    public Vector3 WorldPosition
    {
        get
        {
            MatrixFloat TRS = LocalToWorldMatrix;
            if (Parent == null)
                return new Vector3(TRS[0, 3], TRS[1, 3], TRS[2, 3]);
                
            Transform currentParent = Parent;
            TRS = currentParent.WorldToLocalMatrix;
            while (currentParent.Parent != null)
            {
                TRS *= currentParent.Parent.WorldToLocalMatrix;
                currentParent = Parent.Parent;
            }

            TRS *= LocalToWorldMatrix;
            return new Vector3(TRS[0, 3], TRS[1, 3], TRS[2, 3]);
        }
    }

    public void SetParent(Transform tParent) => Parent = tParent;
}
