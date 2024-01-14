using Unity.Mathematics;

namespace Assets.Scripts.Meshes.Streams
{
    public struct TriangleUInt16
    {
        public ushort a;
        public ushort b;
        public ushort c;

        public static implicit operator TriangleUInt16(int3 t) => new()
        {
            a = (ushort)t.x,
            b = (ushort)t.y,
            c = (ushort)t.z
        };
    }
}