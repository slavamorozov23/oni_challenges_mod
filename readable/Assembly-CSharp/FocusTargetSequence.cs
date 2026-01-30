using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000472 RID: 1138
public static class FocusTargetSequence
{
	// Token: 0x060017D0 RID: 6096 RVA: 0x00086DB1 File Offset: 0x00084FB1
	public static void Start(MonoBehaviour coroutineRunner, FocusTargetSequence.Data sequenceData)
	{
		FocusTargetSequence.sequenceCoroutine = coroutineRunner.StartCoroutine(FocusTargetSequence.RunSequence(sequenceData));
	}

	// Token: 0x060017D1 RID: 6097 RVA: 0x00086DC4 File Offset: 0x00084FC4
	public static void Cancel(MonoBehaviour coroutineRunner)
	{
		if (FocusTargetSequence.sequenceCoroutine == null)
		{
			return;
		}
		coroutineRunner.StopCoroutine(FocusTargetSequence.sequenceCoroutine);
		FocusTargetSequence.sequenceCoroutine = null;
		if (FocusTargetSequence.prevSpeed >= 0)
		{
			SpeedControlScreen.Instance.SetSpeed(FocusTargetSequence.prevSpeed);
		}
		if (SpeedControlScreen.Instance.IsPaused && !FocusTargetSequence.wasPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		if (!SpeedControlScreen.Instance.IsPaused && FocusTargetSequence.wasPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		FocusTargetSequence.SetUIVisible(true);
		CameraController.Instance.SetWorldInteractive(true);
		SelectTool.Instance.Select(FocusTargetSequence.prevSelected, true);
		FocusTargetSequence.prevSelected = null;
		FocusTargetSequence.wasPaused = false;
		FocusTargetSequence.prevSpeed = -1;
	}

	// Token: 0x060017D2 RID: 6098 RVA: 0x00086E71 File Offset: 0x00085071
	public static IEnumerator RunSequence(FocusTargetSequence.Data sequenceData)
	{
		SaveGame.Instance.GetComponent<UserNavigation>();
		CameraController.Instance.FadeOut(1f, 1f, null);
		FocusTargetSequence.prevSpeed = SpeedControlScreen.Instance.GetSpeed();
		SpeedControlScreen.Instance.SetSpeed(0);
		FocusTargetSequence.wasPaused = SpeedControlScreen.Instance.IsPaused;
		if (!FocusTargetSequence.wasPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		PlayerController.Instance.CancelDragging();
		CameraController.Instance.SetWorldInteractive(false);
		yield return CameraController.Instance.activeFadeRoutine;
		FocusTargetSequence.prevSelected = SelectTool.Instance.selected;
		SelectTool.Instance.Select(null, true);
		FocusTargetSequence.SetUIVisible(false);
		ClusterManager.Instance.SetActiveWorld(sequenceData.WorldId);
		ManagementMenu.Instance.CloseAll();
		CameraController.Instance.SnapTo(sequenceData.Target, sequenceData.OrthographicSize);
		if (sequenceData.PopupData != null)
		{
			EventInfoScreen.ShowPopup(sequenceData.PopupData);
		}
		CameraController.Instance.FadeIn(0f, 2f, null);
		if (sequenceData.TargetSize - sequenceData.OrthographicSize > Mathf.Epsilon)
		{
			CameraController.Instance.StartCoroutine(CameraController.Instance.DoCinematicZoom(sequenceData.TargetSize));
		}
		if (sequenceData.CanCompleteCB != null)
		{
			SpeedControlScreen.Instance.Unpause(false);
			while (!sequenceData.CanCompleteCB())
			{
				yield return SequenceUtil.WaitForNextFrame;
			}
			SpeedControlScreen.Instance.Pause(false, false);
		}
		CameraController.Instance.SetWorldInteractive(true);
		SpeedControlScreen.Instance.SetSpeed(FocusTargetSequence.prevSpeed);
		if (SpeedControlScreen.Instance.IsPaused && !FocusTargetSequence.wasPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		if (sequenceData.CompleteCB != null)
		{
			sequenceData.CompleteCB();
		}
		FocusTargetSequence.SetUIVisible(true);
		SelectTool.Instance.Select(FocusTargetSequence.prevSelected, true);
		sequenceData.Clear();
		FocusTargetSequence.sequenceCoroutine = null;
		FocusTargetSequence.prevSpeed = -1;
		FocusTargetSequence.wasPaused = false;
		FocusTargetSequence.prevSelected = null;
		yield break;
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x00086E80 File Offset: 0x00085080
	private static void SetUIVisible(bool visible)
	{
		NotificationScreen.Instance.Show(visible);
		OverlayMenu.Instance.Show(visible);
		ManagementMenu.Instance.Show(visible);
		ToolMenu.Instance.Show(visible);
		ToolMenu.Instance.PriorityScreen.Show(visible);
		PinnedResourcesPanel.Instance.Show(visible);
		TopLeftControlScreen.Instance.Show(visible);
		global::DateTime.Instance.Show(visible);
		BuildWatermark.Instance.Show(visible);
		BuildWatermark.Instance.Show(visible);
		ColonyDiagnosticScreen.Instance.Show(visible);
		RootMenu.Instance.Show(visible);
		if (PlanScreen.Instance != null)
		{
			PlanScreen.Instance.Show(visible);
		}
		if (BuildMenu.Instance != null)
		{
			BuildMenu.Instance.Show(visible);
		}
		if (WorldSelector.Instance != null)
		{
			WorldSelector.Instance.Show(visible);
		}
	}

	// Token: 0x04000DF4 RID: 3572
	private static Coroutine sequenceCoroutine = null;

	// Token: 0x04000DF5 RID: 3573
	private static KSelectable prevSelected = null;

	// Token: 0x04000DF6 RID: 3574
	private static bool wasPaused = false;

	// Token: 0x04000DF7 RID: 3575
	private static int prevSpeed = -1;

	// Token: 0x02001282 RID: 4738
	public struct Data
	{
		// Token: 0x0600885E RID: 34910 RVA: 0x0034E831 File Offset: 0x0034CA31
		public void Clear()
		{
			this.PopupData = null;
			this.CompleteCB = null;
			this.CanCompleteCB = null;
		}

		// Token: 0x04006822 RID: 26658
		public int WorldId;

		// Token: 0x04006823 RID: 26659
		public float OrthographicSize;

		// Token: 0x04006824 RID: 26660
		public float TargetSize;

		// Token: 0x04006825 RID: 26661
		public Vector3 Target;

		// Token: 0x04006826 RID: 26662
		public EventInfoData PopupData;

		// Token: 0x04006827 RID: 26663
		public System.Action CompleteCB;

		// Token: 0x04006828 RID: 26664
		public Func<bool> CanCompleteCB;
	}
}
