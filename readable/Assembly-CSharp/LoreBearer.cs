using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009F4 RID: 2548
[AddComponentMenu("KMonoBehaviour/scripts/LoreBearer")]
public class LoreBearer : KMonoBehaviour
{
	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x06004A5E RID: 19038 RVA: 0x001AEF69 File Offset: 0x001AD169
	public string content
	{
		get
		{
			return Strings.Get("STRINGS.LORE.BUILDINGS." + base.gameObject.name + ".ENTRY");
		}
	}

	// Token: 0x06004A5F RID: 19039 RVA: 0x001AEF8F File Offset: 0x001AD18F
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06004A60 RID: 19040 RVA: 0x001AEF97 File Offset: 0x001AD197
	public LoreBearer Internal_SetContent(LoreBearerAction action)
	{
		this.displayContentAction = action;
		return this;
	}

	// Token: 0x06004A61 RID: 19041 RVA: 0x001AEFA1 File Offset: 0x001AD1A1
	public LoreBearer Internal_SetContent(LoreBearerAction action, string[] collectionsToUnlockFrom)
	{
		this.displayContentAction = action;
		this.collectionsToUnlockFrom = collectionsToUnlockFrom;
		return this;
	}

	// Token: 0x06004A62 RID: 19042 RVA: 0x001AEFB2 File Offset: 0x001AD1B2
	public static InfoDialogScreen ShowPopupDialog()
	{
		return (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
	}

	// Token: 0x06004A63 RID: 19043 RVA: 0x001AEFE4 File Offset: 0x001AD1E4
	private void OnClickRead()
	{
		InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(base.gameObject.GetComponent<KSelectable>().GetProperName()).AddDefaultOK(true);
		if (this.BeenClicked)
		{
			infoDialogScreen.AddPlainText(this.BeenSearched);
			return;
		}
		this.BeenClicked = true;
		if (DlcManager.IsExpansion1Active())
		{
			Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), 0, 1, "OrbitalResearchDatabank", Grid.SceneLayer.Front).SetActive(true);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab("OrbitalResearchDatabank".ToTag()).GetProperName(), base.gameObject.transform, 1.5f, false);
		}
		if (this.displayContentAction != null)
		{
			this.displayContentAction(infoDialogScreen);
			return;
		}
		LoreBearerUtil.UnlockNextJournalEntry(infoDialogScreen);
	}

	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x06004A64 RID: 19044 RVA: 0x001AF0AA File Offset: 0x001AD2AA
	public string SidescreenButtonText
	{
		get
		{
			return this.BeenClicked ? UI.USERMENUACTIONS.READLORE.ALREADYINSPECTED : UI.USERMENUACTIONS.READLORE.NAME;
		}
	}

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x06004A65 RID: 19045 RVA: 0x001AF0C5 File Offset: 0x001AD2C5
	public string SidescreenButtonTooltip
	{
		get
		{
			return this.BeenClicked ? UI.USERMENUACTIONS.READLORE.TOOLTIP_ALREADYINSPECTED : UI.USERMENUACTIONS.READLORE.TOOLTIP;
		}
	}

	// Token: 0x06004A66 RID: 19046 RVA: 0x001AF0E0 File Offset: 0x001AD2E0
	public void OnSidescreenButtonPressed()
	{
		this.OnClickRead();
	}

	// Token: 0x06004A67 RID: 19047 RVA: 0x001AF0E8 File Offset: 0x001AD2E8
	public bool SidescreenButtonInteractable()
	{
		return !this.BeenClicked;
	}

	// Token: 0x06004A68 RID: 19048 RVA: 0x001AF0F3 File Offset: 0x001AD2F3
	public int GetSideScreenSortOrder()
	{
		if (this.GetSidescreenSortOrder != null)
		{
			return this.GetSidescreenSortOrder();
		}
		return -100;
	}

	// Token: 0x04003153 RID: 12627
	[Serialize]
	private bool BeenClicked;

	// Token: 0x04003154 RID: 12628
	public string BeenSearched = UI.USERMENUACTIONS.READLORE.ALREADY_SEARCHED;

	// Token: 0x04003155 RID: 12629
	private string[] collectionsToUnlockFrom;

	// Token: 0x04003156 RID: 12630
	public Func<int> GetSidescreenSortOrder;

	// Token: 0x04003157 RID: 12631
	private LoreBearerAction displayContentAction;
}
