using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transvoxel.Math;
using Transvoxel.VolumeData;
using Transvoxel.Lengyel;

namespace Transvoxel.SurfaceExtractor
{
	public class Surface
	{
		private List<Vector3f> vertexList;
		private List<ushort> indexList;
		private IVolumeData volume;
		private int lod;

		public Surface(IVolumeData volumeData,int lod)
		{
			this.lod = lod;
			this.volume = volumeData;
		}

		// Local Variables
		sbyte[] density = new sbyte[8];

		public void BuildCell(Vector3i pos)
		{
			for (int i = 0; i < density.Length; i++)
			{
				density[i] = volume[pos + Tables.CornerIndex[i] * lod];
			}

			byte caseCode = getCaseCode(density);

			if ((caseCode ^ ((density[7] >> 7) & 0xFF)) == 0) //for this cases there is no triangulation
				return;

			byte regularCellClass = Tables.RegularCellClass[caseCode];
			ushort[] vertexLocations = Tables.RegularVertexData[caseCode];

			Tables.RegularCell c = Tables.RegularCellData[regularCellClass];
			long vertexCount = c.GetVertexCount();
			long triangleCount = c.GetTriangleCount();
			byte[] indexOffset = c.Indizes(); //index offsets for current cell
			ushort[] mappedIndizes = new ushort[indexOffset.Length]; //array with real indizes for current cell

			for (int i = 0; i < vertexCount; i++)
			{
				byte edge = (byte)(vertexLocations[i] >> 8);
				byte reuseIndex = (byte)(edge & 0xF); //Vertex id which should be created or reused 1,2 or 3

				byte v1 = (byte)((vertexLocations[i]) & 0x0F); //Second Corner Index
				byte v0 = (byte)((vertexLocations[i] >> 4) & 0x0F); //First Corner Index

				sbyte d0 = density[v0];
				sbyte d1 = density[v1];

				long t = (d1 << 8) / (d1 - d0);

				if (t != 0 && t != 256 && t != -1 && t != -2 && t != 257 && t != 258)
					Console.WriteLine(t);

				byte rDir = (byte)(edge >> 4); //the direction to go to reach a previous cell for reusing 

				Vector3i iP0 = (pos + Tables.CornerIndex[v0] * lod);
                Vector3f P0 = (Vector3f)iP0;
				Vector3i iP1 = (pos + Tables.CornerIndex[v1] * lod);
				Vector3f P1 = (Vector3f)iP1;

				Vector3f Q = InterpolateVoxelVector(t, P0, P1);
				vertexList.Add(Q);
				mapIndizes2Vertice(i, (ushort)(vertexList.Count - 1), mappedIndizes, indexOffset);
			}

			for (int i = 0; i < triangleCount; i++)
			{
				ushort i1 = mappedIndizes[i * 3 + 0];
				ushort i2 = mappedIndizes[i * 3 + 1];
				ushort i3 = mappedIndizes[i * 3 + 2];

				indexList.Add(i1);
				indexList.Add(i2);
				indexList.Add(i3);
			}
		}

		public static byte getCaseCode(sbyte[] density)
		{
			byte code = 0;
			byte konj = 0x01;
			for (int i = 0; i < density.Length; i++)
			{
				code |= (byte)((density[i] >> (density.Length - 1 - i)) & konj);
				konj <<= 1;
			}

			return code;
		}

		private static Vector3f InterpolateVoxelVector(long t, Vector3f P0, Vector3f P1)
		{
			long u = 0x0100 - t; //256 - t
			float s = 1.0f / 256.0f;
			Vector3f Q = P0 * t + P1 * u; //Density Interpolation
			Q *= s; // shift to shader ! 
			return Q;
		}

		private void mapIndizes2Vertice(int vertexNr, ushort index, ushort[] mappedIndizes, byte[] indexOffset)
		{
			for (int j = 0; j < mappedIndizes.Length; j++)
			{
				if (vertexNr == indexOffset[j])
				{
					mappedIndizes[j] = index;
				}
			}
		}
	}
}
