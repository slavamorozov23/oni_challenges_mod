using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E4C RID: 3660
public class LogicBroadcastChannelSideScreen : SideScreenContent
{
	// Token: 0x0600740E RID: 29710 RVA: 0x002C4C5A File Offset: 0x002C2E5A
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicBroadcastReceiver>() != null;
	}

	// Token: 0x0600740F RID: 29711 RVA: 0x002C4C68 File Offset: 0x002C2E68
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.sensor = target.GetComponent<LogicBroadcastReceiver>();
		this.Build();
	}

	// Token: 0x06007410 RID: 29712 RVA: 0x002C4C84 File Offset: 0x002C2E84
	private void ClearRows()
	{
		if (this.emptySpaceRow != null)
		{
			Util.KDestroyGameObject(this.emptySpaceRow);
		}
		foreach (KeyValuePair<LogicBroadcaster, GameObject> keyValuePair in this.broadcasterRows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.broadcasterRows.Clear();
	}

	// Token: 0x06007411 RID: 29713 RVA: 0x002C4D00 File Offset: 0x002C2F00
	private void Build()
	{
		this.headerLabel.SetText(UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.HEADER);
		this.ClearRows();
		foreach (object obj in Components.LogicBroadcasters)
		{
			LogicBroadcaster logicBroadcaster = (LogicBroadcaster)obj;
			if (!logicBroadcaster.IsNullOrDestroyed())
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
				gameObject.gameObject.name = logicBroadcaster.gameObject.GetProperName();
				global::Debug.Assert(!this.broadcasterRows.ContainsKey(logicBroadcaster), "Adding two of the same broadcaster to LogicBroadcastChannelSideScreen UI: " + logicBroadcaster.gameObject.GetProperName());
				this.broadcasterRows.Add(logicBroadcaster, gameObject);
				gameObject.SetActive(true);
			}
		}
		this.noChannelRow.SetActive(Components.LogicBroadcasters.Count == 0);
		this.Refresh();
	}

	// Token: 0x06007412 RID: 29714 RVA: 0x002C4DFC File Offset: 0x002C2FFC
	private void Refresh()
	{
		using (Dictionary<LogicBroadcaster, GameObject>.Enumerator enumerator = this.broadcasterRows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<LogicBroadcaster, GameObject> kvp = enumerator.Current;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(kvp.Key.gameObject.GetProperName());
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("DistanceLabel").SetText(LogicBroadcastReceiver.CheckRange(this.sensor.gameObject, kvp.Key.gameObject) ? UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.IN_RANGE : UI.UISIDESCREENS.LOGICBROADCASTCHANNELSIDESCREEN.OUT_OF_RANGE);
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite(kvp.Key.gameObject, "ui", false).first;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite(kvp.Key.gameObject, "ui", false).second;
				WorldContainer myWorld = kvp.Key.GetMyWorld();
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("WorldIcon").sprite = (myWorld.IsModuleInterior ? Assets.GetSprite("icon_category_rocketry") : Def.GetUISprite(myWorld.GetComponent<ClusterGridEntity>(), "ui", false).first);
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("WorldIcon").color = (myWorld.IsModuleInterior ? Color.white : Def.GetUISprite(myWorld.GetComponent<ClusterGridEntity>(), "ui", false).second);
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.sensor.SetChannel(kvp.Key);
					this.Refresh();
				};
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState((this.sensor.GetChannel() == kvp.Key) ? 1 : 0);
			}
		}
	}

	// Token: 0x04005042 RID: 20546
	private LogicBroadcastReceiver sensor;

	// Token: 0x04005043 RID: 20547
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x04005044 RID: 20548
	[SerializeField]
	private GameObject listContainer;

	// Token: 0x04005045 RID: 20549
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x04005046 RID: 20550
	[SerializeField]
	private GameObject noChannelRow;

	// Token: 0x04005047 RID: 20551
	private Dictionary<LogicBroadcaster, GameObject> broadcasterRows = new Dictionary<LogicBroadcaster, GameObject>();

	// Token: 0x04005048 RID: 20552
	private GameObject emptySpaceRow;
}
