using System;
using System.Collections.Generic;

namespace Klei
{
	// Token: 0x0200100E RID: 4110
	public class ClusterLayoutSave
	{
		// Token: 0x06007F8B RID: 32651 RVA: 0x00335370 File Offset: 0x00333570
		public ClusterLayoutSave()
		{
			this.worlds = new List<ClusterLayoutSave.World>();
		}

		// Token: 0x040060BB RID: 24763
		public string ID;

		// Token: 0x040060BC RID: 24764
		public Vector2I version;

		// Token: 0x040060BD RID: 24765
		public List<ClusterLayoutSave.World> worlds;

		// Token: 0x040060BE RID: 24766
		public Vector2I size;

		// Token: 0x040060BF RID: 24767
		public int currentWorldIdx;

		// Token: 0x040060C0 RID: 24768
		public int numRings;

		// Token: 0x040060C1 RID: 24769
		public Dictionary<ClusterLayoutSave.POIType, List<AxialI>> poiLocations = new Dictionary<ClusterLayoutSave.POIType, List<AxialI>>();

		// Token: 0x040060C2 RID: 24770
		public Dictionary<AxialI, string> poiPlacements = new Dictionary<AxialI, string>();

		// Token: 0x0200271D RID: 10013
		public class World
		{
			// Token: 0x0400AE49 RID: 44617
			public Data data = new Data();

			// Token: 0x0400AE4A RID: 44618
			public string name = string.Empty;

			// Token: 0x0400AE4B RID: 44619
			public bool isDiscovered;

			// Token: 0x0400AE4C RID: 44620
			public List<string> traits = new List<string>();

			// Token: 0x0400AE4D RID: 44621
			public List<string> storyTraits = new List<string>();

			// Token: 0x0400AE4E RID: 44622
			public List<string> seasons = new List<string>();

			// Token: 0x0400AE4F RID: 44623
			public List<string> generatedSubworlds = new List<string>();
		}

		// Token: 0x0200271E RID: 10014
		public enum POIType
		{
			// Token: 0x0400AE51 RID: 44625
			TemporalTear,
			// Token: 0x0400AE52 RID: 44626
			ResearchDestination
		}
	}
}
