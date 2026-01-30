using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000858 RID: 2136
public static class GeothermalFirstEmissionSequence
{
	// Token: 0x06003AC7 RID: 15047 RVA: 0x0014848E File Offset: 0x0014668E
	public static void Start(GeothermalController controller)
	{
		controller.StartCoroutine(GeothermalFirstEmissionSequence.Sequence(controller));
	}

	// Token: 0x06003AC8 RID: 15048 RVA: 0x0014849D File Offset: 0x0014669D
	private static IEnumerator Sequence(GeothermalController controller)
	{
		List<GeothermalVent> items = Components.GeothermalVents.GetItems(controller.GetMyWorldId());
		GeothermalVent vent = null;
		foreach (GeothermalVent geothermalVent in items)
		{
			if (geothermalVent != null && geothermalVent.IsVentConnected() && geothermalVent.HasMaterial())
			{
				vent = geothermalVent;
				break;
			}
		}
		if (vent != null)
		{
			if (!SpeedControlScreen.Instance.IsPaused)
			{
				SpeedControlScreen.Instance.Pause(false, false);
			}
			CameraController.Instance.SetWorldInteractive(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot, STOP_MODE.ALLOWFADEOUT);
			CameraController.Instance.FadeOut(1f, 1f, null);
			yield return SequenceUtil.WaitForSecondsRealtime(1f);
			CameraController.Instance.SetTargetPos(vent.transform.position + Vector3.up * 3f, 10f, false);
			CameraController.Instance.SetOverrideZoomSpeed(10f);
			yield return SequenceUtil.WaitForSecondsRealtime(1f);
			CameraController.Instance.FadeIn(0f, 1f, null);
			if (SpeedControlScreen.Instance.IsPaused)
			{
				SpeedControlScreen.Instance.Unpause(false);
			}
			SpeedControlScreen.Instance.SetSpeed(0);
		}
		yield return SequenceUtil.WaitForSecondsRealtime(1f);
		CameraController.Instance.SetWorldInteractive(true);
		yield break;
	}
}
