using System;
using UnityEngine;

// Token: 0x02000BF4 RID: 3060
public class Thought : Resource
{
	// Token: 0x06005BDE RID: 23518 RVA: 0x002140AC File Offset: 0x002122AC
	public Thought(string id, ResourceSet parent, Sprite icon, string mode_icon, string sound_name, string bubble, string speech_prefix, LocString hover_text, bool show_immediately = false, float show_time = 4f) : base(id, parent, null)
	{
		this.sprite = icon;
		if (mode_icon != null)
		{
			this.modeSprite = Assets.GetSprite(mode_icon);
		}
		this.bubbleSprite = Assets.GetSprite(bubble);
		this.sound = sound_name;
		this.speechPrefix = speech_prefix;
		this.hoverText = hover_text;
		this.showImmediately = show_immediately;
		this.showTime = show_time;
	}

	// Token: 0x06005BDF RID: 23519 RVA: 0x0021411C File Offset: 0x0021231C
	public Thought(string id, ResourceSet parent, string icon, string mode_icon, string sound_name, string bubble, string speech_prefix, LocString hover_text, bool show_immediately = false, float show_time = 4f) : base(id, parent, null)
	{
		this.sprite = Assets.GetSprite(icon);
		if (mode_icon != null)
		{
			this.modeSprite = Assets.GetSprite(mode_icon);
		}
		this.bubbleSprite = Assets.GetSprite(bubble);
		this.sound = sound_name;
		this.speechPrefix = speech_prefix;
		this.hoverText = hover_text;
		this.showImmediately = show_immediately;
		this.showTime = show_time;
	}

	// Token: 0x06005BE0 RID: 23520 RVA: 0x00214193 File Offset: 0x00212393
	public void PlayAsSpeech(SpeechMonitor.Instance speechMonitorInstance)
	{
		speechMonitorInstance.PlaySpeech(this.speechPrefix, this.sound);
	}

	// Token: 0x04003D2E RID: 15662
	public int priority;

	// Token: 0x04003D2F RID: 15663
	public Sprite sprite;

	// Token: 0x04003D30 RID: 15664
	public Sprite modeSprite;

	// Token: 0x04003D31 RID: 15665
	public string sound;

	// Token: 0x04003D32 RID: 15666
	public Sprite bubbleSprite;

	// Token: 0x04003D33 RID: 15667
	public string speechPrefix;

	// Token: 0x04003D34 RID: 15668
	public LocString hoverText;

	// Token: 0x04003D35 RID: 15669
	public bool showImmediately;

	// Token: 0x04003D36 RID: 15670
	public float showTime;
}
