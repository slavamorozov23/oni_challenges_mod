using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000D8F RID: 3471
public class DiscoveredSpaceMessage : Message
{
	// Token: 0x06006C16 RID: 27670 RVA: 0x00290C62 File Offset: 0x0028EE62
	public DiscoveredSpaceMessage()
	{
	}

	// Token: 0x06006C17 RID: 27671 RVA: 0x00290C6A File Offset: 0x0028EE6A
	public DiscoveredSpaceMessage(Vector3 pos)
	{
		this.cameraFocusPos = pos;
		this.cameraFocusPos.z = -40f;
	}

	// Token: 0x06006C18 RID: 27672 RVA: 0x00290C89 File Offset: 0x0028EE89
	public override string GetSound()
	{
		return "Discover_Space";
	}

	// Token: 0x06006C19 RID: 27673 RVA: 0x00290C90 File Offset: 0x0028EE90
	public override string GetMessageBody()
	{
		return MISC.NOTIFICATIONS.DISCOVERED_SPACE.TOOLTIP;
	}

	// Token: 0x06006C1A RID: 27674 RVA: 0x00290C9C File Offset: 0x0028EE9C
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DISCOVERED_SPACE.NAME;
	}

	// Token: 0x06006C1B RID: 27675 RVA: 0x00290CA8 File Offset: 0x0028EEA8
	public override string GetTooltip()
	{
		return null;
	}

	// Token: 0x06006C1C RID: 27676 RVA: 0x00290CAB File Offset: 0x0028EEAB
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x06006C1D RID: 27677 RVA: 0x00290CAE File Offset: 0x0028EEAE
	public override void OnClick()
	{
		this.OnDiscoveredSpaceClicked();
	}

	// Token: 0x06006C1E RID: 27678 RVA: 0x00290CB6 File Offset: 0x0028EEB6
	private void OnDiscoveredSpaceClicked()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound(this.GetSound(), false));
		MusicManager.instance.PlaySong("Stinger_Surface", false);
		CameraController.Instance.SetTargetPos(this.cameraFocusPos, 8f, true);
	}

	// Token: 0x04004A23 RID: 18979
	[Serialize]
	private Vector3 cameraFocusPos;

	// Token: 0x04004A24 RID: 18980
	private const string MUSIC_STINGER = "Stinger_Surface";
}
