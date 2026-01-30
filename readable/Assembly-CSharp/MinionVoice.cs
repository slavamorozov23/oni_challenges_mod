using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000F RID: 15
public readonly struct MinionVoice
{
	// Token: 0x06000041 RID: 65 RVA: 0x00003CA4 File Offset: 0x00001EA4
	public MinionVoice(int voiceIndex)
	{
		this.voiceIndex = voiceIndex;
		this.voiceId = (voiceIndex + 1).ToString("D2");
		this.isValid = true;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00003CD5 File Offset: 0x00001ED5
	public static MinionVoice ByPersonality(Personality personality)
	{
		return MinionVoice.ByPersonality(personality.Id);
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00003CE4 File Offset: 0x00001EE4
	public static MinionVoice ByPersonality(string personalityId)
	{
		if (personalityId == "JORGE")
		{
			return new MinionVoice(-2);
		}
		if (personalityId == "MEEP")
		{
			return new MinionVoice(2);
		}
		MinionVoice minionVoice;
		if (!MinionVoice.personalityVoiceMap.TryGetValue(personalityId, out minionVoice))
		{
			minionVoice = MinionVoice.Random();
			MinionVoice.personalityVoiceMap.Add(personalityId, minionVoice);
		}
		return minionVoice;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00003D3C File Offset: 0x00001F3C
	public static MinionVoice Random()
	{
		return new MinionVoice(UnityEngine.Random.Range(0, 4));
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00003D4C File Offset: 0x00001F4C
	public static Option<MinionVoice> ByObject(UnityEngine.Object unityObject)
	{
		GameObject gameObject = unityObject as GameObject;
		GameObject gameObject2;
		if (gameObject != null)
		{
			gameObject2 = gameObject;
		}
		else
		{
			Component component = unityObject as Component;
			if (component != null)
			{
				gameObject2 = component.gameObject;
			}
			else
			{
				gameObject2 = null;
			}
		}
		if (gameObject2.IsNullOrDestroyed())
		{
			return Option.None;
		}
		MinionVoiceProviderMB componentInParent = gameObject2.GetComponentInParent<MinionVoiceProviderMB>();
		if (componentInParent.IsNullOrDestroyed())
		{
			return Option.None;
		}
		return componentInParent.voice;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00003DB0 File Offset: 0x00001FB0
	public string GetSoundAssetName(string localName)
	{
		global::Debug.Assert(this.isValid);
		string d = localName;
		if (localName.Contains(":"))
		{
			d = localName.Split(':', StringSplitOptions.None)[0];
		}
		return StringFormatter.Combine("DupVoc_", this.voiceId, "_", d);
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00003DF9 File Offset: 0x00001FF9
	public string GetSoundPath(string localName)
	{
		return GlobalAssets.GetSound(this.GetSoundAssetName(localName), true);
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00003E08 File Offset: 0x00002008
	public void PlaySoundUI(string localName)
	{
		global::Debug.Assert(this.isValid);
		string soundPath = this.GetSoundPath(localName);
		try
		{
			if (SoundListenerController.Instance == null)
			{
				KFMOD.PlayUISound(soundPath);
			}
			else
			{
				KFMOD.PlayOneShot(soundPath, SoundListenerController.Instance.transform.GetPosition(), 1f);
			}
		}
		catch
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"AUDIOERROR: Missing [" + soundPath + "]"
			});
		}
	}

	// Token: 0x04000044 RID: 68
	public readonly int voiceIndex;

	// Token: 0x04000045 RID: 69
	public readonly string voiceId;

	// Token: 0x04000046 RID: 70
	public readonly bool isValid;

	// Token: 0x04000047 RID: 71
	private static Dictionary<string, MinionVoice> personalityVoiceMap = new Dictionary<string, MinionVoice>();
}
