using System;
using TUNING;
using UnityEngine;

// Token: 0x020007EE RID: 2030
[AddComponentMenu("KMonoBehaviour/scripts/ResearchModule")]
public class ResearchModule : KMonoBehaviour
{
	// Token: 0x06003655 RID: 13909 RVA: 0x00132BF0 File Offset: 0x00130DF0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		base.Subscribe<ResearchModule>(-1277991738, ResearchModule.OnLaunchDelegate);
		base.Subscribe<ResearchModule>(-887025858, ResearchModule.OnLandDelegate);
	}

	// Token: 0x06003656 RID: 13910 RVA: 0x00132C45 File Offset: 0x00130E45
	public void OnLaunch(object data)
	{
	}

	// Token: 0x06003657 RID: 13911 RVA: 0x00132C48 File Offset: 0x00130E48
	public void OnLand(object data)
	{
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			SpaceDestination.ResearchOpportunity researchOpportunity = SpacecraftManager.instance.GetSpacecraftDestination(SpacecraftManager.instance.GetSpacecraftID(base.GetComponent<RocketModule>().conditionManager.GetComponent<ILaunchableRocket>())).TryCompleteResearchOpportunity();
			if (researchOpportunity != null)
			{
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("ResearchDatabank"), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0);
				gameObject.SetActive(true);
				gameObject.GetComponent<PrimaryElement>().Mass = (float)researchOpportunity.dataValue;
				if (!string.IsNullOrEmpty(researchOpportunity.discoveredRareItem))
				{
					GameObject prefab = Assets.GetPrefab(researchOpportunity.discoveredRareItem);
					if (prefab == null)
					{
						KCrashReporter.Assert(false, "Missing prefab: " + researchOpportunity.discoveredRareItem, null);
					}
					else
					{
						GameUtil.KInstantiate(prefab, base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0).SetActive(true);
					}
				}
			}
		}
		GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab("ResearchDatabank"), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0);
		gameObject2.SetActive(true);
		gameObject2.GetComponent<PrimaryElement>().Mass = (float)ROCKETRY.DESTINATION_RESEARCH.EVERGREEN;
	}

	// Token: 0x040020FF RID: 8447
	private static readonly EventSystem.IntraObjectHandler<ResearchModule> OnLaunchDelegate = new EventSystem.IntraObjectHandler<ResearchModule>(delegate(ResearchModule component, object data)
	{
		component.OnLaunch(data);
	});

	// Token: 0x04002100 RID: 8448
	private static readonly EventSystem.IntraObjectHandler<ResearchModule> OnLandDelegate = new EventSystem.IntraObjectHandler<ResearchModule>(delegate(ResearchModule component, object data)
	{
		component.OnLand(data);
	});
}
