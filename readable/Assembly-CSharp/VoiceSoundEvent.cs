using System;
using FMOD.Studio;
using Klei.AI;
using UnityEngine;

// Token: 0x02000567 RID: 1383
public class VoiceSoundEvent : SoundEvent
{
	// Token: 0x06001ED2 RID: 7890 RVA: 0x000A7875 File Offset: 0x000A5A75
	public VoiceSoundEvent(string file_name, string sound_name, int frame, bool is_looping) : base(file_name, sound_name, frame, false, is_looping, (float)SoundEvent.IGNORE_INTERVAL, true)
	{
		base.noiseValues = SoundEventVolumeCache.instance.GetVolume("VoiceSoundEvent", sound_name);
	}

	// Token: 0x06001ED3 RID: 7891 RVA: 0x000A78AB File Offset: 0x000A5AAB
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		VoiceSoundEvent.PlayVoice(base.name, behaviour.controller, this.intervalBetweenSpeaking, base.looping, false);
	}

	// Token: 0x06001ED4 RID: 7892 RVA: 0x000A78CC File Offset: 0x000A5ACC
	public static EventInstance PlayVoice(string name, KBatchedAnimController controller, float interval_between_speaking, bool looping, bool objectIsSelectedAndVisible = false)
	{
		EventInstance eventInstance = default(EventInstance);
		MinionIdentity component = controller.GetComponent<MinionIdentity>();
		if (component == null || (name.Contains("state") && Time.time - component.timeLastSpoke < interval_between_speaking))
		{
			return eventInstance;
		}
		bool flag = component.model == BionicMinionConfig.MODEL;
		if (name.Contains(":"))
		{
			float num = float.Parse(name.Split(':', StringSplitOptions.None)[1]);
			if ((float)UnityEngine.Random.Range(0, 100) > num)
			{
				return eventInstance;
			}
		}
		WorkerBase component2 = controller.GetComponent<WorkerBase>();
		string assetName = VoiceSoundEvent.GetAssetName(name, component2);
		StaminaMonitor.Instance smi = component2.GetSMI<StaminaMonitor.Instance>();
		if (!name.Contains("sleep_") && smi != null && smi.IsSleeping())
		{
			return eventInstance;
		}
		Vector3 vector = component2.transform.GetPosition();
		vector.z = 0f;
		if (SoundEvent.ObjectIsSelectedAndVisible(controller.gameObject))
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
		}
		string sound = GlobalAssets.GetSound(assetName, true);
		if (!SoundEvent.ShouldPlaySound(controller, sound, looping, false))
		{
			return eventInstance;
		}
		if (sound != null)
		{
			if (looping)
			{
				LoopingSounds component3 = controller.GetComponent<LoopingSounds>();
				if (component3 == null)
				{
					global::Debug.Log(controller.name + " is missing LoopingSounds component. ");
				}
				else if (!component3.StartSound(sound))
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", sound, controller.name)
					});
				}
				else
				{
					component3.UpdateFirstParameter(sound, "isBionic", (float)(flag ? 1 : 0));
				}
			}
			else
			{
				eventInstance = SoundEvent.BeginOneShot(sound, vector, 1f, false);
				eventInstance.setParameterByName("isBionic", (float)(flag ? 1 : 0), false);
				if (sound.Contains("sleep_") && controller.GetComponent<Traits>().HasTrait("Snorer"))
				{
					eventInstance.setParameterByName("snoring", 1f, false);
				}
				SoundEvent.EndOneShot(eventInstance);
				component.timeLastSpoke = Time.time;
			}
		}
		else if (AudioDebug.Get().debugVoiceSounds)
		{
			global::Debug.LogWarning("Missing voice sound: " + assetName);
		}
		return eventInstance;
	}

	// Token: 0x06001ED5 RID: 7893 RVA: 0x000A7ADC File Offset: 0x000A5CDC
	private static string GetAssetName(string name, Component cmp)
	{
		string b = "F01";
		if (cmp != null)
		{
			MinionIdentity component = cmp.GetComponent<MinionIdentity>();
			if (component != null)
			{
				b = component.GetVoiceId();
			}
		}
		string d = name;
		if (name.Contains(":"))
		{
			d = name.Split(':', StringSplitOptions.None)[0];
		}
		return StringFormatter.Combine("DupVoc_", b, "_", d);
	}

	// Token: 0x06001ED6 RID: 7894 RVA: 0x000A7B3C File Offset: 0x000A5D3C
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				string sound = GlobalAssets.GetSound(VoiceSoundEvent.GetAssetName(base.name, component), true);
				component.StopSound(sound);
			}
		}
	}

	// Token: 0x040011F4 RID: 4596
	public static float locomotionSoundProb = 50f;

	// Token: 0x040011F5 RID: 4597
	public float timeLastSpoke;

	// Token: 0x040011F6 RID: 4598
	public float intervalBetweenSpeaking = 10f;
}
