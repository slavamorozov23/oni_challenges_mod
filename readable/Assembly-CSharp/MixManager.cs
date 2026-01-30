using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200057B RID: 1403
public class MixManager : MonoBehaviour
{
	// Token: 0x06001F2E RID: 7982 RVA: 0x000AA169 File Offset: 0x000A8369
	private void Update()
	{
		if (AudioMixer.instance != null && AudioMixer.instance.persistentSnapshotsActive)
		{
			AudioMixer.instance.UpdatePersistentSnapshotParameters();
		}
	}

	// Token: 0x06001F2F RID: 7983 RVA: 0x000AA188 File Offset: 0x000A8388
	private void OnApplicationFocus(bool hasFocus)
	{
		if (AudioMixer.instance == null || AudioMixerSnapshots.Get() == null)
		{
			return;
		}
		if (!hasFocus && KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().GameNotFocusedSnapshot);
			return;
		}
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().GameNotFocusedSnapshot, STOP_MODE.ALLOWFADEOUT);
	}
}
