using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TransvoxelXnaStudio.TransvoxelHelpers
{
    [Serializable]
    public struct VertexPositionTextureNormalColor : IVertexType
    {
        private Vector3 _position;
        private Vector2 _textureCoord1;
        private Vector3 _normal;
        private Vector3 _binormal;
        private Vector3 _tangent;
        private Vector4 _color;

        public static readonly VertexElement[] VertexElements = new VertexElement[]
        { 
            new VertexElement(0,VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float)*3,VertexElementFormat.Vector2,  VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(float)*5,VertexElementFormat.Vector3,  VertexElementUsage.Normal, 0),
            new VertexElement(sizeof(float)*8,VertexElementFormat.Vector3,  VertexElementUsage.Binormal, 0),
            new VertexElement(sizeof(float)*11,VertexElementFormat.Vector3,  VertexElementUsage.Tangent, 0),
            new VertexElement(sizeof(float)*14,VertexElementFormat.Vector4, VertexElementUsage.Color,0)   
        };

        public VertexPositionTextureNormalColor(Vector3 position, Vector2 uv, Vector4 color)
        {
            _position = position;
            _textureCoord1 = uv;
            _color = color;
            _normal = Vector3.Zero;
            _binormal = Vector3.Zero;
            _tangent = Vector3.Zero;
        }
        public VertexPositionTextureNormalColor(Vector3 position, Vector2 uv, Vector3 normal, Vector3 binormal, Vector3 tangent, Vector4 color)
        {
            _position = position;
            _textureCoord1 = uv;
            _normal = normal;
            _binormal = binormal;
            _tangent = tangent;
            _color = color;
        }

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(VertexElements);
        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }


        public override String ToString()
        {
            return "(" + _position + "),(" + _textureCoord1 + ")";
        }

        public Vector3 Position { get { return _position; } set { _position = value; } }
        public Vector2 TextureCoordinate1 { get { return _textureCoord1; } set { _textureCoord1 = value; } }
        public Vector3 Normal { get { return _normal; } set { _normal = value; } }
        public Vector3 Binormal { get { return _binormal; } set { _binormal = value; } }
        public Vector3 Tangent { get { return _tangent; } set { _tangent = value; } }
        public Vector4 Color { get { return _color; } set { _color = value; } }
        public static int SizeInBytes { get { return sizeof(float) * 18; } }
    }
}