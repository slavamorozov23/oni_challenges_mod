using System;
using UnityEngine;

// Token: 0x02000C6D RID: 3181
public class PatchNotesScreen : KModalScreen
{
	// Token: 0x060060F9 RID: 24825 RVA: 0x0023AC2C File Offset: 0x00238E2C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.changesLabel.text = PatchNotesScreen.m_patchNotesText;
		this.closeButton.onClick += this.MarkAsReadAndClose;
		this.closeButton.soundPlayer.widget_sound_events()[0].OverrideAssetName = "HUD_Click_Close";
		this.okButton.onClick += this.MarkAsReadAndClose;
		this.previousVersion.onClick += delegate()
		{
			App.OpenWebURL("http://support.kleientertainment.com/customer/portal/articles/2776550");
		};
		this.fullPatchNotes.onClick += this.OnPatchNotesClick;
		PatchNotesScreen.instance = this;
	}

	// Token: 0x060060FA RID: 24826 RVA: 0x0023ACE4 File Offset: 0x00238EE4
	protected override void OnCleanUp()
	{
		PatchNotesScreen.instance = null;
	}

	// Token: 0x060060FB RID: 24827 RVA: 0x0023ACEC File Offset: 0x00238EEC
	public static bool ShouldShowScreen()
	{
		return false;
	}

	// Token: 0x060060FC RID: 24828 RVA: 0x0023ACEF File Offset: 0x00238EEF
	private void MarkAsReadAndClose()
	{
		KPlayerPrefs.SetInt("PatchNotesVersion", PatchNotesScreen.PatchNotesVersion);
		this.Deactivate();
	}

	// Token: 0x060060FD RID: 24829 RVA: 0x0023AD06 File Offset: 0x00238F06
	public static void UpdatePatchNotes(string patchNotesSummary, string url)
	{
		PatchNotesScreen.m_patchNotesUrl = url;
		PatchNotesScreen.m_patchNotesText = patchNotesSummary;
		if (PatchNotesScreen.instance != null)
		{
			PatchNotesScreen.instance.changesLabel.text = PatchNotesScreen.m_patchNotesText;
		}
	}

	// Token: 0x060060FE RID: 24830 RVA: 0x0023AD35 File Offset: 0x00238F35
	private void OnPatchNotesClick()
	{
		App.OpenWebURL(PatchNotesScreen.m_patchNotesUrl);
	}

	// Token: 0x060060FF RID: 24831 RVA: 0x0023AD41 File Offset: 0x00238F41
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.MarkAsReadAndClose();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x040040E8 RID: 16616
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040040E9 RID: 16617
	[SerializeField]
	private KButton okButton;

	// Token: 0x040040EA RID: 16618
	[SerializeField]
	private KButton fullPatchNotes;

	// Token: 0x040040EB RID: 16619
	[SerializeField]
	private KButton previousVersion;

	// Token: 0x040040EC RID: 16620
	[SerializeField]
	private LocText changesLabel;

	// Token: 0x040040ED RID: 16621
	private static string m_patchNotesUrl;

	// Token: 0x040040EE RID: 16622
	private static string m_patchNotesText;

	// Token: 0x040040EF RID: 16623
	private static int PatchNotesVersion = 9;

	// Token: 0x040040F0 RID: 16624
	private static PatchNotesScreen instance;
}
