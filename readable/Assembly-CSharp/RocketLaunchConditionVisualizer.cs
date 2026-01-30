using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000629 RID: 1577
public class RocketLaunchConditionVisualizer : KMonoBehaviour
{
	// Token: 0x0600259E RID: 9630 RVA: 0x000D809C File Offset: 0x000D629C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			this.clusterModule = base.GetComponent<RocketModuleCluster>();
		}
		else
		{
			this.launchConditionManager = base.GetComponent<LaunchConditionManager>();
		}
		this.UpdateAllModuleData();
		base.Subscribe(1512695988, new Action<object>(this.OnAnyRocketModuleChanged));
	}

	// Token: 0x0600259F RID: 9631 RVA: 0x000D80EE File Offset: 0x000D62EE
	protected override void OnCleanUp()
	{
		base.Unsubscribe(1512695988, new Action<object>(this.OnAnyRocketModuleChanged));
	}

	// Token: 0x060025A0 RID: 9632 RVA: 0x000D8107 File Offset: 0x000D6307
	private void OnAnyRocketModuleChanged(object obj)
	{
		this.UpdateAllModuleData();
	}

	// Token: 0x060025A1 RID: 9633 RVA: 0x000D8110 File Offset: 0x000D6310
	private void UpdateAllModuleData()
	{
		if (this.moduleVisualizeData != null)
		{
			this.moduleVisualizeData = null;
		}
		bool flag = this.clusterModule != null;
		List<Ref<RocketModuleCluster>> list = null;
		List<RocketModule> list2 = null;
		if (flag)
		{
			list = new List<Ref<RocketModuleCluster>>(this.clusterModule.CraftInterface.ClusterModules);
			this.moduleVisualizeData = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData[list.Count];
			list.Sort(delegate(Ref<RocketModuleCluster> a, Ref<RocketModuleCluster> b)
			{
				int y = Grid.PosToXY(a.Get().transform.GetPosition()).y;
				int y2 = Grid.PosToXY(b.Get().transform.GetPosition()).y;
				return y.CompareTo(y2);
			});
		}
		else
		{
			list2 = new List<RocketModule>(this.launchConditionManager.rocketModules);
			list2.Sort(delegate(RocketModule a, RocketModule b)
			{
				int y = Grid.PosToXY(a.transform.GetPosition()).y;
				int y2 = Grid.PosToXY(b.transform.GetPosition()).y;
				return y.CompareTo(y2);
			});
			this.moduleVisualizeData = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData[list2.Count];
		}
		for (int i = 0; i < this.moduleVisualizeData.Length; i++)
		{
			RocketModule rocketModule = flag ? list[i].Get() : list2[i];
			Building component = rocketModule.GetComponent<Building>();
			this.moduleVisualizeData[i] = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData
			{
				Module = rocketModule,
				RangeMax = Mathf.FloorToInt((float)component.Def.WidthInCells / 2f),
				RangeMin = -Mathf.FloorToInt((float)(component.Def.WidthInCells - 1) / 2f)
			};
		}
	}

	// Token: 0x04001609 RID: 5641
	public RocketLaunchConditionVisualizer.RocketModuleVisualizeData[] moduleVisualizeData;

	// Token: 0x0400160A RID: 5642
	private LaunchConditionManager launchConditionManager;

	// Token: 0x0400160B RID: 5643
	private RocketModuleCluster clusterModule;

	// Token: 0x02001506 RID: 5382
	public struct RocketModuleVisualizeData
	{
		// Token: 0x0400706A RID: 28778
		public RocketModule Module;

		// Token: 0x0400706B RID: 28779
		public Vector2I OriginOffset;

		// Token: 0x0400706C RID: 28780
		public int RangeMin;

		// Token: 0x0400706D RID: 28781
		public int RangeMax;
	}
}
