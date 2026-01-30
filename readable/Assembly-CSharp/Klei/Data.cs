using System;
using System.Collections.Generic;
using ProcGen;
using ProcGenGame;
using VoronoiTree;

namespace Klei
{
	// Token: 0x0200100A RID: 4106
	public class Data
	{
		// Token: 0x06007F87 RID: 32647 RVA: 0x003352CC File Offset: 0x003334CC
		public Data()
		{
			this.worldLayout = new WorldLayout(null, 0);
			this.terrainCells = new List<TerrainCell>();
			this.overworldCells = new List<TerrainCell>();
			this.rivers = new List<ProcGen.River>();
			this.gameSpawnData = new GameSpawnData();
			this.world = new Chunk();
			this.voronoiTree = new Tree(0);
		}

		// Token: 0x0400609E RID: 24734
		public int globalWorldSeed;

		// Token: 0x0400609F RID: 24735
		public int globalWorldLayoutSeed;

		// Token: 0x040060A0 RID: 24736
		public int globalTerrainSeed;

		// Token: 0x040060A1 RID: 24737
		public int globalNoiseSeed;

		// Token: 0x040060A2 RID: 24738
		public int chunkEdgeSize = 32;

		// Token: 0x040060A3 RID: 24739
		public WorldLayout worldLayout;

		// Token: 0x040060A4 RID: 24740
		public List<TerrainCell> terrainCells;

		// Token: 0x040060A5 RID: 24741
		public List<TerrainCell> overworldCells;

		// Token: 0x040060A6 RID: 24742
		public List<ProcGen.River> rivers;

		// Token: 0x040060A7 RID: 24743
		public GameSpawnData gameSpawnData;

		// Token: 0x040060A8 RID: 24744
		public Chunk world;

		// Token: 0x040060A9 RID: 24745
		public Tree voronoiTree;

		// Token: 0x040060AA RID: 24746
		public AxialI clusterLocation;
	}
}
