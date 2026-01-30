using System;

// Token: 0x02000559 RID: 1369
public class MouthFlapSoundEvent : SoundEvent
{
	// Token: 0x06001E73 RID: 7795 RVA: 0x000A5A80 File Offset: 0x000A3C80
	public MouthFlapSoundEvent(string file_name, string sound_name, int frame, bool is_looping) : base(file_name, sound_name, frame, false, is_looping, (float)SoundEvent.IGNORE_INTERVAL, true)
	{
	}

	// Token: 0x06001E74 RID: 7796 RVA: 0x000A5A95 File Offset: 0x000A3C95
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		behaviour.controller.GetSMI<SpeechMonitor.Instance>().PlaySpeech(base.name, null);
	}
}
