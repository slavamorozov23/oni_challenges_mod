using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000A90 RID: 2704
public class PedestalArtifactSpawner : KMonoBehaviour
{
	// Token: 0x06004E89 RID: 20105 RVA: 0x001C8F6C File Offset: 0x001C716C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (GameObject gameObject in this.storage.items)
		{
			if (ArtifactSelector.Instance.GetArtifactType(gameObject.name) == ArtifactType.Terrestrial)
			{
				gameObject.GetComponent<KPrefabID>().AddTag(GameTags.TerrestrialArtifact, true);
			}
		}
		if (this.artifactSpawned)
		{
			return;
		}
		GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab(ArtifactSelector.Instance.GetUniqueArtifactID(ArtifactType.Terrestrial)), base.transform.position);
		gameObject2.SetActive(true);
		gameObject2.GetComponent<KPrefabID>().AddTag(GameTags.TerrestrialArtifact, true);
		this.storage.Store(gameObject2, false, false, true, false);
		this.receptacle.ForceDeposit(gameObject2);
		this.artifactSpawned = true;
	}

	// Token: 0x04003460 RID: 13408
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04003461 RID: 13409
	[MyCmpReq]
	private SingleEntityReceptacle receptacle;

	// Token: 0x04003462 RID: 13410
	[Serialize]
	private bool artifactSpawned;
}
