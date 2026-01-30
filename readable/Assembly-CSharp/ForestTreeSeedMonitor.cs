using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200019C RID: 412
public class ForestTreeSeedMonitor : KMonoBehaviour
{
	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000806 RID: 2054 RVA: 0x000369DF File Offset: 0x00034BDF
	public bool ExtraSeedAvailable
	{
		get
		{
			return this.hasExtraSeedAvailable;
		}
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x000369E8 File Offset: 0x00034BE8
	public void ExtractExtraSeed()
	{
		if (!this.hasExtraSeedAvailable)
		{
			return;
		}
		this.hasExtraSeedAvailable = false;
		Vector3 position = base.transform.position;
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		Util.KInstantiate(Assets.GetPrefab("ForestTreeSeed"), position).SetActive(true);
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x00036A3A File Offset: 0x00034C3A
	public void TryRollNewSeed()
	{
		if (!this.hasExtraSeedAvailable && UnityEngine.Random.Range(0, 100) < 5)
		{
			this.hasExtraSeedAvailable = true;
		}
	}

	// Token: 0x0400060E RID: 1550
	[Serialize]
	private bool hasExtraSeedAvailable;
}
