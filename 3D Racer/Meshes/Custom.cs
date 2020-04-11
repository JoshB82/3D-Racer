using System.Diagnostics;

namespace _3D_Racer
{
    public sealed class Custom
    {
        public Custom(Vector3D position)
        {
            Debug.WriteLine($"Custom mesh created at ({position.X}, {position.Y}, {position.Z})");
        }
    }
}
