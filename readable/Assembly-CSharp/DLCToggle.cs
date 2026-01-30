using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000CEE RID: 3310
public class DLCToggle : KMonoBehaviour
{
	// Token: 0x0600662A RID: 26154 RVA: 0x0026702F File Offset: 0x0026522F
	protected override void OnPrefabInit()
	{
		this.expansion1Active = DlcManager.IsExpansion1Active();
	}

	// Token: 0x0600662B RID: 26155 RVA: 0x0026703C File Offset: 0x0026523C
	public void ToggleExpansion1Cicked()
	{
		Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.GetComponentInParent<Canvas>().gameObject, true).AddDefaultCancel().SetHeader(this.expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1 : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1).AddSprite(this.expansion1Active ? GlobalResources.Instance().baseGameLogoSmall : GlobalResources.Instance().expansion1LogoSmall).AddPlainText(this.expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1_DESC : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1_DESC).AddOption(UI.CONFIRMDIALOG.OK, delegate(InfoDialogScreen screen)
		{
			DlcManager.ToggleDLC("EXPANSION1_ID");
		}, true);
	}

	// Token: 0x040045B4 RID: 17844
	private bool expansion1Active;
}
