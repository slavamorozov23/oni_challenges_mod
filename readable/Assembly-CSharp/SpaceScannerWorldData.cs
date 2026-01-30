using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;

// Token: 0x02000BD6 RID: 3030
[Serialize]
[SerializationConfig(MemberSerialization.OptIn)]
[Serializable]
public class SpaceScannerWorldData
{
	// Token: 0x06005ACC RID: 23244 RVA: 0x0020E066 File Offset: 0x0020C266
	[Serialize]
	public SpaceScannerWorldData(int worldId)
	{
		this.worldId = worldId;
	}

	// Token: 0x06005ACD RID: 23245 RVA: 0x0020E096 File Offset: 0x0020C296
	public WorldContainer GetWorld()
	{
		if (this.world == null)
		{
			this.world = ClusterManager.Instance.GetWorld(this.worldId);
		}
		return this.world;
	}

	// Token: 0x04003C88 RID: 15496
	[NonSerialized]
	private WorldContainer world;

	// Token: 0x04003C89 RID: 15497
	[Serialize]
	public int worldId;

	// Token: 0x04003C8A RID: 15498
	[Serialize]
	public float networkQuality01;

	// Token: 0x04003C8B RID: 15499
	[Serialize]
	public Dictionary<string, float> targetIdToRandomValue01Map = new Dictionary<string, float>();

	// Token: 0x04003C8C RID: 15500
	[Serialize]
	public HashSet<string> targetIdsDetected = new HashSet<string>();

	// Token: 0x04003C8D RID: 15501
	[NonSerialized]
	public SpaceScannerWorldData.Scratchpad scratchpad = new SpaceScannerWorldData.Scratchpad();

	// Token: 0x02001D65 RID: 7525
	public class Scratchpad
	{
		// Token: 0x04008B2C RID: 35628
		public List<ClusterTraveler> ballisticObjects = new List<ClusterTraveler>();

		// Token: 0x04008B2D RID: 35629
		public HashSet<MeteorShowerEvent.StatesInstance> lastDetectedMeteorShowers = new HashSet<MeteorShowerEvent.StatesInstance>();

		// Token: 0x04008B2E RID: 35630
		public HashSet<LaunchConditionManager> lastDetectedRocketsBaseGame = new HashSet<LaunchConditionManager>();

		// Token: 0x04008B2F RID: 35631
		public HashSet<Clustercraft> lastDetectedRocketsDLC1 = new HashSet<Clustercraft>();
	}
}
