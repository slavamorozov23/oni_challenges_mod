using System;
using System.Collections.Generic;
using Delaunay.Geo;
using KSerialization;
using ProcGen;
using ProcGenGame;

namespace Klei
{
	// Token: 0x0200100C RID: 4108
	public class WorldDetailSave
	{
		// Token: 0x06007F89 RID: 32649 RVA: 0x0033534A File Offset: 0x0033354A
		public WorldDetailSave()
		{
			this.overworldCells = new List<WorldDetailSave.OverworldCell>();
		}

		// Token: 0x040060B0 RID: 24752
		public List<WorldDetailSave.OverworldCell> overworldCells;

		// Token: 0x040060B1 RID: 24753
		public int globalWorldSeed;

		// Token: 0x040060B2 RID: 24754
		public int globalWorldLayoutSeed;

		// Token: 0x040060B3 RID: 24755
		public int globalTerrainSeed;

		// Token: 0x040060B4 RID: 24756
		public int globalNoiseSeed;

		// Token: 0x0200271C RID: 10012
		[SerializationConfig(MemberSerialization.OptOut)]
		public class OverworldCell
		{
			// Token: 0x0600C7F4 RID: 51188 RVA: 0x00424CFA File Offset: 0x00422EFA
			public OverworldCell()
			{
			}

			// Token: 0x0600C7F5 RID: 51189 RVA: 0x00424D02 File Offset: 0x00422F02
			public OverworldCell(SubWorld.ZoneType zoneType, TerrainCell tc)
			{
				this.poly = tc.poly;
				this.tags = tc.node.tags;
				this.zoneType = zoneType;
			}

			// Token: 0x0400AE46 RID: 44614
			public Polygon poly;

			// Token: 0x0400AE47 RID: 44615
			public TagSet tags;

			// Token: 0x0400AE48 RID: 44616
			public SubWorld.ZoneType zoneType;
		}
	}
}
