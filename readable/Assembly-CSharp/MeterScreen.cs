using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x02000DA4 RID: 3492
public class MeterScreen : KScreen, IRender1000ms
{
	// Token: 0x170007B6 RID: 1974
	// (get) Token: 0x06006CAF RID: 27823 RVA: 0x00291E5D File Offset: 0x0029005D
	// (set) Token: 0x06006CB0 RID: 27824 RVA: 0x00291E64 File Offset: 0x00290064
	public static MeterScreen Instance { get; private set; }

	// Token: 0x06006CB1 RID: 27825 RVA: 0x00291E6C File Offset: 0x0029006C
	public static void DestroyInstance()
	{
		MeterScreen.Instance = null;
	}

	// Token: 0x170007B7 RID: 1975
	// (get) Token: 0x06006CB2 RID: 27826 RVA: 0x00291E74 File Offset: 0x00290074
	public bool StartValuesSet
	{
		get
		{
			return this.startValuesSet;
		}
	}

	// Token: 0x06006CB3 RID: 27827 RVA: 0x00291E7C File Offset: 0x0029007C
	protected override void OnPrefabInit()
	{
		MeterScreen.Instance = this;
	}

	// Token: 0x06006CB4 RID: 27828 RVA: 0x00291E84 File Offset: 0x00290084
	protected override void OnSpawn()
	{
		this.RedAlertTooltip.OnToolTip = new Func<string>(this.OnRedAlertTooltip);
		MultiToggle redAlertButton = this.RedAlertButton;
		redAlertButton.onClick = (System.Action)Delegate.Combine(redAlertButton.onClick, new System.Action(delegate()
		{
			this.OnRedAlertClick();
		}));
		Game.Instance.Subscribe(1983128072, delegate(object data)
		{
			this.Refresh();
		});
		Game.Instance.Subscribe(1585324898, delegate(object data)
		{
			this.RefreshRedAlertButtonState();
		});
		Game.Instance.Subscribe(-1393151672, delegate(object data)
		{
			this.RefreshRedAlertButtonState();
		});
	}

	// Token: 0x06006CB5 RID: 27829 RVA: 0x00291F24 File Offset: 0x00290124
	private void OnRedAlertClick()
	{
		bool flag = !ClusterManager.Instance.activeWorld.AlertManager.IsRedAlertToggledOn();
		ClusterManager.Instance.activeWorld.AlertManager.ToggleRedAlert(flag);
		if (flag)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
			return;
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
	}

	// Token: 0x06006CB6 RID: 27830 RVA: 0x00291F82 File Offset: 0x00290182
	private void RefreshRedAlertButtonState()
	{
		this.RedAlertButton.ChangeState(ClusterManager.Instance.activeWorld.IsRedAlert() ? 1 : 0);
	}

	// Token: 0x06006CB7 RID: 27831 RVA: 0x00291FA4 File Offset: 0x002901A4
	public void Render1000ms(float dt)
	{
		this.Refresh();
	}

	// Token: 0x06006CB8 RID: 27832 RVA: 0x00291FAC File Offset: 0x002901AC
	public void InitializeValues()
	{
		if (this.startValuesSet)
		{
			return;
		}
		this.startValuesSet = true;
		this.Refresh();
	}

	// Token: 0x06006CB9 RID: 27833 RVA: 0x00291FC4 File Offset: 0x002901C4
	private void Refresh()
	{
		this.RefreshWorldMinionIdentities();
		this.RefreshMinions();
		for (int i = 0; i < this.valueDisplayers.Length; i++)
		{
			this.valueDisplayers[i].Refresh();
		}
		this.RefreshRedAlertButtonState();
	}

	// Token: 0x06006CBA RID: 27834 RVA: 0x00292004 File Offset: 0x00290204
	private void RefreshWorldMinionIdentities()
	{
		this.worldLiveMinionIdentities = new List<MinionIdentity>(from x in Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId, false)
		where !x.IsNullOrDestroyed()
		select x);
	}

	// Token: 0x06006CBB RID: 27835 RVA: 0x00292055 File Offset: 0x00290255
	private List<MinionIdentity> GetWorldMinionIdentities()
	{
		if (this.worldLiveMinionIdentities == null)
		{
			this.RefreshWorldMinionIdentities();
		}
		return this.worldLiveMinionIdentities;
	}

	// Token: 0x06006CBC RID: 27836 RVA: 0x0029206C File Offset: 0x0029026C
	private void RefreshMinions()
	{
		int count = Components.LiveMinionIdentities.Count;
		int count2 = this.GetWorldMinionIdentities().Count;
		if (count2 == this.cachedMinionCount)
		{
			return;
		}
		this.cachedMinionCount = count2;
		string newString;
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			ClusterGridEntity component = ClusterManager.Instance.activeWorld.GetComponent<ClusterGridEntity>();
			newString = string.Format(UI.TOOLTIPS.METERSCREEN_POPULATION_CLUSTER, component.Name, count2, count);
			this.currentMinions.text = string.Format("{0}/{1}", count2, count);
		}
		else
		{
			this.currentMinions.text = string.Format("{0}", count);
			newString = string.Format(UI.TOOLTIPS.METERSCREEN_POPULATION, count.ToString("0"));
		}
		this.MinionsTooltip.ClearMultiStringTooltip();
		this.MinionsTooltip.AddMultiStringTooltip(newString, this.ToolTipStyle_Header);
	}

	// Token: 0x06006CBD RID: 27837 RVA: 0x00292158 File Offset: 0x00290358
	private string OnRedAlertTooltip()
	{
		this.RedAlertTooltip.ClearMultiStringTooltip();
		this.RedAlertTooltip.AddMultiStringTooltip(UI.TOOLTIPS.RED_ALERT_TITLE, this.ToolTipStyle_Header);
		this.RedAlertTooltip.AddMultiStringTooltip(UI.TOOLTIPS.RED_ALERT_CONTENT, this.ToolTipStyle_Property);
		return "";
	}

	// Token: 0x04004A5A RID: 19034
	[SerializeField]
	private LocText currentMinions;

	// Token: 0x04004A5C RID: 19036
	public ToolTip MinionsTooltip;

	// Token: 0x04004A5D RID: 19037
	public MeterScreen_ValueTrackerDisplayer[] valueDisplayers;

	// Token: 0x04004A5E RID: 19038
	public TextStyleSetting ToolTipStyle_Header;

	// Token: 0x04004A5F RID: 19039
	public TextStyleSetting ToolTipStyle_Property;

	// Token: 0x04004A60 RID: 19040
	private bool startValuesSet;

	// Token: 0x04004A61 RID: 19041
	public MultiToggle RedAlertButton;

	// Token: 0x04004A62 RID: 19042
	public ToolTip RedAlertTooltip;

	// Token: 0x04004A63 RID: 19043
	private MeterScreen.DisplayInfo immunityDisplayInfo = new MeterScreen.DisplayInfo
	{
		selectedIndex = -1
	};

	// Token: 0x04004A64 RID: 19044
	private List<MinionIdentity> worldLiveMinionIdentities;

	// Token: 0x04004A65 RID: 19045
	private int cachedMinionCount = -1;

	// Token: 0x02001FE2 RID: 8162
	private struct DisplayInfo
	{
		// Token: 0x04009416 RID: 37910
		public int selectedIndex;
	}
}
