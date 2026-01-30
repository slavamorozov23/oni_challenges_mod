using System;
using UnityEngine;

// Token: 0x02000569 RID: 1385
[AddComponentMenu("KMonoBehaviour/scripts/AudioDebug")]
public class AudioDebug : KMonoBehaviour
{
	// Token: 0x06001EDF RID: 7903 RVA: 0x000A7F98 File Offset: 0x000A6198
	public static AudioDebug Get()
	{
		return AudioDebug.instance;
	}

	// Token: 0x06001EE0 RID: 7904 RVA: 0x000A7F9F File Offset: 0x000A619F
	protected override void OnPrefabInit()
	{
		AudioDebug.instance = this;
	}

	// Token: 0x06001EE1 RID: 7905 RVA: 0x000A7FA7 File Offset: 0x000A61A7
	public void ToggleMusic()
	{
		if (Game.Instance != null)
		{
			Game.Instance.SetMusicEnabled(this.musicEnabled);
		}
		this.musicEnabled = !this.musicEnabled;
	}

	// Token: 0x040011FC RID: 4604
	private static AudioDebug instance;

	// Token: 0x040011FD RID: 4605
	public bool musicEnabled;

	// Token: 0x040011FE RID: 4606
	public bool debugSoundEvents;

	// Token: 0x040011FF RID: 4607
	public bool debugFloorSounds;

	// Token: 0x04001200 RID: 4608
	public bool debugGameEventSounds;

	// Token: 0x04001201 RID: 4609
	public bool debugNotificationSounds;

	// Token: 0x04001202 RID: 4610
	public bool debugVoiceSounds;
}
