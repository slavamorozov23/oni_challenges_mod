using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000545 RID: 1349
public class ClusterMapSoundEvent : SoundEvent
{
	// Token: 0x06001D38 RID: 7480 RVA: 0x0009F167 File Offset: 0x0009D367
	public ClusterMapSoundEvent(string file_name, string sound_name, int frame, bool looping) : base(file_name, sound_name, frame, true, looping, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x06001D39 RID: 7481 RVA: 0x0009F17C File Offset: 0x0009D37C
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		if (ClusterMapScreen.Instance != null && ClusterMapScreen.Instance.IsActive())
		{
			this.PlaySound(behaviour);
		}
	}

	// Token: 0x06001D3A RID: 7482 RVA: 0x0009F1A0 File Offset: 0x0009D3A0
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				global::Debug.Log(behaviour.name + " (Cluster Map Object) is missing LoopingSounds component.");
				return;
			}
			if (!component.StartSound(base.sound, true, false, false))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", base.sound, behaviour.name)
				});
				return;
			}
		}
		else
		{
			EventInstance instance = KFMOD.BeginOneShot(base.sound, Vector3.zero, 1f);
			instance.setParameterByName(ClusterMapSoundEvent.X_POSITION_PARAMETER, behaviour.controller.transform.GetPosition().x / (float)Screen.width, false);
			instance.setParameterByName(ClusterMapSoundEvent.Y_POSITION_PARAMETER, behaviour.controller.transform.GetPosition().y / (float)Screen.height, false);
			instance.setParameterByName(ClusterMapSoundEvent.ZOOM_PARAMETER, ClusterMapScreen.Instance.CurrentZoomPercentage(), false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x06001D3B RID: 7483 RVA: 0x0009F2A0 File Offset: 0x0009D4A0
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(base.sound);
			}
		}
	}

	// Token: 0x04001128 RID: 4392
	private static string X_POSITION_PARAMETER = "Starmap_Position_X";

	// Token: 0x04001129 RID: 4393
	private static string Y_POSITION_PARAMETER = "Starmap_Position_Y";

	// Token: 0x0400112A RID: 4394
	private static string ZOOM_PARAMETER = "Starmap_Zoom_Percentage";
}
