using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000577 RID: 1399
public abstract class AudioSheets : ScriptableObject
{
	// Token: 0x06001F1E RID: 7966 RVA: 0x000A91D0 File Offset: 0x000A73D0
	public virtual void Initialize()
	{
		foreach (AudioSheet audioSheet in this.sheets)
		{
			foreach (AudioSheet.SoundInfo soundInfo in audioSheet.soundInfos)
			{
				if (DlcManager.IsContentSubscribed(soundInfo.RequiredDlcId))
				{
					string text = soundInfo.Type;
					if (text == null || text == "")
					{
						text = audioSheet.defaultType;
					}
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name0, soundInfo.Frame0, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name1, soundInfo.Frame1, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name2, soundInfo.Frame2, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name3, soundInfo.Frame3, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name4, soundInfo.Frame4, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name5, soundInfo.Frame5, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name6, soundInfo.Frame6, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name7, soundInfo.Frame7, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name8, soundInfo.Frame8, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name9, soundInfo.Frame9, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name10, soundInfo.Frame10, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name11, soundInfo.Frame11, soundInfo.RequiredDlcId);
				}
			}
		}
	}

	// Token: 0x06001F1F RID: 7967 RVA: 0x000A94DC File Offset: 0x000A76DC
	private void CreateSound(string file_name, string anim_name, string type, float min_interval, string sound_name, int frame, string dlcId)
	{
		if (string.IsNullOrEmpty(sound_name))
		{
			return;
		}
		HashedString key = file_name + "." + anim_name;
		AnimEvent animEvent = this.CreateSoundOfType(type, file_name, sound_name, frame, min_interval, dlcId);
		if (animEvent == null)
		{
			global::Debug.LogError("Unknown sound type: " + type);
			return;
		}
		List<AnimEvent> list = null;
		if (!this.events.TryGetValue(key, out list))
		{
			list = new List<AnimEvent>();
			this.events[key] = list;
		}
		list.Add(animEvent);
	}

	// Token: 0x06001F20 RID: 7968
	protected abstract AnimEvent CreateSoundOfType(string type, string file_name, string sound_name, int frame, float min_interval, string dlcId);

	// Token: 0x06001F21 RID: 7969 RVA: 0x000A9558 File Offset: 0x000A7758
	public List<AnimEvent> GetEvents(HashedString anim_id)
	{
		List<AnimEvent> result = null;
		HashedString hashedString;
		if (this.skinToBaseAnim.TryGetValue(anim_id, out hashedString))
		{
			anim_id = hashedString;
		}
		this.events.TryGetValue(anim_id, out result);
		return result;
	}

	// Token: 0x04001229 RID: 4649
	public List<AudioSheet> sheets = new List<AudioSheet>();

	// Token: 0x0400122A RID: 4650
	public Dictionary<HashedString, HashedString> skinToBaseAnim = new Dictionary<HashedString, HashedString>();

	// Token: 0x0400122B RID: 4651
	public Dictionary<HashedString, List<AnimEvent>> events = new Dictionary<HashedString, List<AnimEvent>>();
}
