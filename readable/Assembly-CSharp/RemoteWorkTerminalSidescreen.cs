using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E6B RID: 3691
public class RemoteWorkTerminalSidescreen : SideScreenContent
{
	// Token: 0x06007542 RID: 30018 RVA: 0x002CC0EA File Offset: 0x002CA2EA
	public override string GetTitle()
	{
		return UI.UISIDESCREENS.REMOTE_WORK_TERMINAL_SIDE_SCREEN.TITLE;
	}

	// Token: 0x06007543 RID: 30019 RVA: 0x002CC0F6 File Offset: 0x002CA2F6
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.rowPrefab.SetActive(false);
		if (show)
		{
			this.RefreshOptions(null);
		}
	}

	// Token: 0x06007544 RID: 30020 RVA: 0x002CC115 File Offset: 0x002CA315
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<RemoteWorkTerminal>() != null;
	}

	// Token: 0x06007545 RID: 30021 RVA: 0x002CC123 File Offset: 0x002CA323
	public override void SetTarget(GameObject target)
	{
		this.targetTerminal = target.GetComponent<RemoteWorkTerminal>();
		this.RefreshOptions(null);
		this.uiRefreshSubHandle = target.Subscribe(1980521255, new Action<object>(this.RefreshOptions));
	}

	// Token: 0x06007546 RID: 30022 RVA: 0x002CC155 File Offset: 0x002CA355
	public override void ClearTarget()
	{
		if (this.uiRefreshSubHandle != -1 && this.targetTerminal != null)
		{
			this.targetTerminal.gameObject.Unsubscribe(this.uiRefreshSubHandle);
			this.uiRefreshSubHandle = -1;
		}
	}

	// Token: 0x06007547 RID: 30023 RVA: 0x002CC18C File Offset: 0x002CA38C
	private void RefreshOptions(object data = null)
	{
		int num = 0;
		this.SetRow(num++, UI.UISIDESCREENS.REMOTE_WORK_TERMINAL_SIDE_SCREEN.NOTHING_SELECTED, Assets.GetSprite("action_building_disabled"), null);
		foreach (RemoteWorkerDock remoteWorkerDock in Components.RemoteWorkerDocks.GetItems(this.targetTerminal.GetMyWorldId()))
		{
			remoteWorkerDock.GetProperName();
			Sprite first = Def.GetUISprite(remoteWorkerDock.gameObject, "ui", false).first;
			int idx = num++;
			string name = UI.StripLinkFormatting(remoteWorkerDock.GetProperName());
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(remoteWorkerDock.gameObject, "ui", false);
			this.SetRow(idx, name, (uisprite != null) ? uisprite.first : null, remoteWorkerDock);
		}
		for (int i = num; i < this.rowContainer.childCount; i++)
		{
			this.rowContainer.GetChild(i).gameObject.SetActive(false);
		}
	}

	// Token: 0x06007548 RID: 30024 RVA: 0x002CC290 File Offset: 0x002CA490
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x06007549 RID: 30025 RVA: 0x002CC2D4 File Offset: 0x002CA4D4
	private void SetRow(int idx, string name, Sprite icon, RemoteWorkerDock dock)
	{
		dock == null;
		GameObject gameObject;
		if (idx < this.rowContainer.childCount)
		{
			gameObject = this.rowContainer.GetChild(idx).gameObject;
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
		}
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		LocText reference = component.GetReference<LocText>("label");
		reference.text = name;
		reference.ApplySettings();
		Image reference2 = component.GetReference<Image>("icon");
		reference2.sprite = icon;
		reference2.color = Color.white;
		ToolTip toolTip = gameObject.GetComponentsInChildren<ToolTip>().First<ToolTip>();
		toolTip.SetSimpleTooltip(UI.UISIDESCREENS.REMOTE_WORK_TERMINAL_SIDE_SCREEN.DOCK_TOOLTIP);
		toolTip.enabled = (dock != null);
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		component2.ChangeState((this.targetTerminal.FutureDock == dock) ? 1 : 0);
		component2.onClick = delegate()
		{
			this.targetTerminal.FutureDock = dock;
			this.RefreshOptions(null);
		};
		component2.onDoubleClick = delegate()
		{
			GameUtil.FocusCamera((dock == null) ? this.targetTerminal.transform.GetPosition() : dock.transform.GetPosition(), 2f, true, true);
			return true;
		};
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
	}

	// Token: 0x04005122 RID: 20770
	private RemoteWorkTerminal targetTerminal;

	// Token: 0x04005123 RID: 20771
	public GameObject rowPrefab;

	// Token: 0x04005124 RID: 20772
	public RectTransform rowContainer;

	// Token: 0x04005125 RID: 20773
	public Dictionary<object, GameObject> rows = new Dictionary<object, GameObject>();

	// Token: 0x04005126 RID: 20774
	private int uiRefreshSubHandle = -1;
}
