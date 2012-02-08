using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransvoxelXna.VolumeData.CompactOctree;
using TransvoxelXna.Geometry;
using TransvoxelXna.VolumeData;
using TransvoxelXna.Math;
using TransvoxelXna.Lengyel;

namespace TransvoxelXna.SurfaceExtractor
{
	public interface SurfaceExtractor
	{
		Mesh GenLodCell(int lod);
	}

	public class TransvoxelExtract : SurfaceExtractor
	{
		IVolumeData volume;

		public TransvoxelExtract(IVolumeData data)
		{
			volume = data;
		}

		public Mesh GenLodCell(int lod)
		{
			Mesh mesh = new Mesh();

			for (int x = 0; x < VolumeChunk.CHUNKSIZE - 1; x++)
				for (int y = 0; y < VolumeChunk.CHUNKSIZE - 1; y++)
					for (int z = 0; z < VolumeChunk.CHUNKSIZE - 1; z++)
					{ 
						//PolygonizeCell
						PolygonizeCell(new Vector3i(x,y,z),ref mesh,lod);
					}

			return mesh;
		}

		internal void PolygonizeCell(Vector3i pos, ref Mesh mesh,int lod)
		{
			sbyte[] density = new sbyte[8];
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

				Vector3i iP0 = (pos + Tables.CornerIndex[v0] * lod);
				Vector3f P0 = new Vector3f(iP0.X, iP0.Y, iP0.Z);
				Vector3i iP1 = (pos + Tables.CornerIndex[v1] * lod);
				Vector3f P1 = new Vector3f(iP1.X, iP1.Y, iP1.Z);
				Vector3f Q = InterpolateVoxelVector(t, P0, P1);
				mesh.AddVertex(Q);
				mapIndizes2Vertice(i, (ushort)(mesh.VertexCount() - 1), mappedIndizes, indexOffset);
			}

			for (int i = 0; i < triangleCount; i++)
			{
				ushort i1 = mappedIndizes[i * 3 + 0];
				ushort i2 = mappedIndizes[i * 3 + 1];
				ushort i3 = mappedIndizes[i * 3 + 2];

				mesh.AddIndex(i1);
				mesh.AddIndex(i2);
				mesh.AddIndex(i3);
			}
		}

		private void EliminateLodPositionShift(int lod, ref sbyte d0, ref sbyte d1, ref long t, ref Vector3i iP0, ref Vector3f P0, ref Vector3i iP1, ref Vector3f P1)
		{
			for (int k = 0; k < lod - 1; k++)
			{
				Vector3f vm = (P0 + P1) / 2.0f;
				Vector3i pm = (iP0 + iP1) / 2;
				sbyte sm = volume[pm];

				if ((d0 & 0x8F) != (d1 & 0x8F))
				{
					P1 = vm;
					iP1 = pm;
					d1 = sm;
				}
				else
				{
					P0 = vm;
					iP0 = pm;
					d0 = sm;
				}
			}

			t = (d1 << 8) / (d1 - d0); // recalc
		}

		private static Cell getReuseCell(Cell[, ,] cells, int lod, byte rDir, Vector3i pos)
		{
			int rx = rDir & 0x01;
			int rz = (rDir >> 1) & 0x01;
			int ry = (rDir >> 2) & 0x01;

			int dx = pos.X / lod - rx;
			int dy = pos.Y / lod - ry;
			int dz = pos.Z / lod - rz;

			Cell ccc = cells[dx, dy, dz];
			return ccc;
		}

		internal static Vector3f InterpolateVoxelVector(long t, Vector3f P0, Vector3f P1)
		{
			long u = 0x0100 - t; //256 - t
			float s = 1.0f / 256.0f;
			Vector3f Q = P0 * t + P1 * u; //Density Interpolation
			Q *= s; // shift to shader ! 
			return Q;
		}

		internal void mapIndizes2Vertice(int vertexNr, ushort index, ushort[] mappedIndizes, byte[] indexOffset)
		{
			for (int j = 0; j < mappedIndizes.Length; j++)
			{
				if (vertexNr == indexOffset[j])
				{
					mappedIndizes[j] = index;
				}
			}
		}

		internal static byte getCaseCode(sbyte[] density)
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
	}
}
