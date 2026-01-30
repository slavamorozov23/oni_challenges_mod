using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E86 RID: 3718
public class TelescopeSideScreen : SideScreenContent
{
	// Token: 0x0600765E RID: 30302 RVA: 0x002D2920 File Offset: 0x002D0B20
	public TelescopeSideScreen()
	{
		this.refreshDisplayStateDelegate = new Action<object>(this.RefreshDisplayState);
	}

	// Token: 0x0600765F RID: 30303 RVA: 0x002D293C File Offset: 0x002D0B3C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.selectStarmapScreen.onClick += delegate()
		{
			ManagementMenu.Instance.ToggleStarmap();
		};
		SpacecraftManager.instance.Subscribe(532901469, this.refreshDisplayStateDelegate);
		this.RefreshDisplayState(null);
	}

	// Token: 0x06007660 RID: 30304 RVA: 0x002D2996 File Offset: 0x002D0B96
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RefreshDisplayState(null);
		this.target = SelectTool.Instance.selected.GetComponent<KMonoBehaviour>().gameObject;
	}

	// Token: 0x06007661 RID: 30305 RVA: 0x002D29BF File Offset: 0x002D0BBF
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.target)
		{
			this.target = null;
		}
	}

	// Token: 0x06007662 RID: 30306 RVA: 0x002D29DB File Offset: 0x002D0BDB
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.target)
		{
			this.target = null;
		}
	}

	// Token: 0x06007663 RID: 30307 RVA: 0x002D29F7 File Offset: 0x002D0BF7
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Telescope>() != null;
	}

	// Token: 0x06007664 RID: 30308 RVA: 0x002D2A08 File Offset: 0x002D0C08
	private void RefreshDisplayState(object _ = null)
	{
		if (SelectTool.Instance.selected == null)
		{
			return;
		}
		if (SelectTool.Instance.selected.GetComponent<Telescope>() == null)
		{
			return;
		}
		if (!SpacecraftManager.instance.HasAnalysisTarget())
		{
			this.DescriptionText.text = "<b><color=#FF0000>" + UI.UISIDESCREENS.TELESCOPESIDESCREEN.NO_SELECTED_ANALYSIS_TARGET + "</color></b>";
			return;
		}
		string text = UI.UISIDESCREENS.TELESCOPESIDESCREEN.ANALYSIS_TARGET_SELECTED;
		this.DescriptionText.text = text;
	}

	// Token: 0x040051E9 RID: 20969
	public KButton selectStarmapScreen;

	// Token: 0x040051EA RID: 20970
	public Image researchButtonIcon;

	// Token: 0x040051EB RID: 20971
	public GameObject content;

	// Token: 0x040051EC RID: 20972
	private GameObject target;

	// Token: 0x040051ED RID: 20973
	private Action<object> refreshDisplayStateDelegate;

	// Token: 0x040051EE RID: 20974
	public LocText DescriptionText;
}
