using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E28 RID: 3624
public class ClusterLocationFilterSideScreen : SideScreenContent
{
	// Token: 0x06007303 RID: 29443 RVA: 0x002BE64D File Offset: 0x002BC84D
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicClusterLocationSensor>() != null;
	}

	// Token: 0x06007304 RID: 29444 RVA: 0x002BE65B File Offset: 0x002BC85B
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.sensor = target.GetComponent<LogicClusterLocationSensor>();
		this.Build();
	}

	// Token: 0x06007305 RID: 29445 RVA: 0x002BE678 File Offset: 0x002BC878
	private void ClearRows()
	{
		if (this.emptySpaceRow != null)
		{
			Util.KDestroyGameObject(this.emptySpaceRow);
		}
		foreach (KeyValuePair<AxialI, GameObject> keyValuePair in this.worldRows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.worldRows.Clear();
	}

	// Token: 0x06007306 RID: 29446 RVA: 0x002BE6F4 File Offset: 0x002BC8F4
	private void Build()
	{
		this.headerLabel.SetText(UI.UISIDESCREENS.CLUSTERLOCATIONFILTERSIDESCREEN.HEADER);
		this.ClearRows();
		this.emptySpaceRow = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
		this.emptySpaceRow.SetActive(true);
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (!worldContainer.IsModuleInterior)
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
				gameObject.gameObject.name = worldContainer.GetProperName();
				AxialI myWorldLocation = worldContainer.GetMyWorldLocation();
				global::Debug.Assert(!this.worldRows.ContainsKey(myWorldLocation), "Adding two worlds/POI with the same cluster location to ClusterLocationFilterSideScreen UI: " + worldContainer.GetProperName());
				this.worldRows.Add(myWorldLocation, gameObject);
			}
		}
		this.Refresh();
	}

	// Token: 0x06007307 RID: 29447 RVA: 0x002BE7F0 File Offset: 0x002BC9F0
	private void Refresh()
	{
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(UI.UISIDESCREENS.CLUSTERLOCATIONFILTERSIDESCREEN.EMPTY_SPACE_ROW);
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite("hex_soft", "ui", false).first;
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Color.black;
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
		{
			this.sensor.SetSpaceEnabled(!this.sensor.ActiveInSpace);
			this.Refresh();
		};
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.sensor.ActiveInSpace ? 1 : 0);
		using (Dictionary<AxialI, GameObject>.Enumerator enumerator = this.worldRows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<AxialI, GameObject> kvp = enumerator.Current;
				ClusterGridEntity clusterGridEntity = ClusterGrid.Instance.cellContents[kvp.Key][0];
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(clusterGridEntity.GetProperName());
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite(clusterGridEntity, "ui", false).first;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite(clusterGridEntity, "ui", false).second;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.sensor.SetLocationEnabled(kvp.Key, !this.sensor.CheckLocationSelected(kvp.Key));
					this.Refresh();
				};
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.sensor.CheckLocationSelected(kvp.Key) ? 1 : 0);
				kvp.Value.SetActive(ClusterGrid.Instance.GetCellRevealLevel(kvp.Key) == ClusterRevealLevel.Visible);
			}
		}
	}

	// Token: 0x04004F7E RID: 20350
	private LogicClusterLocationSensor sensor;

	// Token: 0x04004F7F RID: 20351
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x04004F80 RID: 20352
	[SerializeField]
	private GameObject listContainer;

	// Token: 0x04004F81 RID: 20353
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x04004F82 RID: 20354
	private Dictionary<AxialI, GameObject> worldRows = new Dictionary<AxialI, GameObject>();

	// Token: 0x04004F83 RID: 20355
	private GameObject emptySpaceRow;
}
