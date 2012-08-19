using System;

namespace Transvoxel.VolumeData.VolumeHash
{
    /// <summary>
    /// Contains a volume size calculations.
    /// </summary>
    public sealed class VolumeSize
    {
        /// <summary>
        /// The axis unit size.
        /// </summary>
        public readonly int SideLength = 8;

        /// <summary>
        /// The axis unit size^2.
        /// </summary>
        public readonly int SideLengthSquared = 64;

        /// <summary>
        /// The axis unit size^3.
        /// </summary>
        public readonly int SideLengthCubed = 512;

        /// <summary>
        /// Creates a new instance of VolumeSize.
        /// </summary>
        /// <param name="sideLength">The axis unit size of the volume. Must be a power of 2 and between 8 and 256.</param>
        public VolumeSize(int sideLength)
        {
            if (sideLength % 2 != 0)
                throw new ArgumentOutOfRangeException("sideLength", "Not a power of 2.");
            if (sideLength < 8)
                throw new ArgumentOutOfRangeException("sideLength", "Must be between 8 and 256.");
            if (sideLength > 256)
                throw new ArgumentOutOfRangeException("sideLength", "Must be between 8 and 256.");

            SideLength = sideLength;
            SideLengthSquared = sideLength * sideLength;
            SideLengthCubed = SideLength * SideLength * SideLength;
        }
    }
}