using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

// Token: 0x02000C3D RID: 3133
public class AudioMixerSnapshots : ScriptableObject
{
	// Token: 0x06005EAA RID: 24234 RVA: 0x0022A93C File Offset: 0x00228B3C
	[ContextMenu("Reload")]
	public void ReloadSnapshots()
	{
		this.snapshotMap.Clear();
		EventReference[] array = this.snapshots;
		for (int i = 0; i < array.Length; i++)
		{
			string eventReferencePath = KFMOD.GetEventReferencePath(array[i]);
			if (!eventReferencePath.IsNullOrWhiteSpace())
			{
				this.snapshotMap.Add(eventReferencePath);
			}
		}
	}

	// Token: 0x06005EAB RID: 24235 RVA: 0x0022A98A File Offset: 0x00228B8A
	public static AudioMixerSnapshots Get()
	{
		if (AudioMixerSnapshots.instance == null)
		{
			AudioMixerSnapshots.instance = Resources.Load<AudioMixerSnapshots>("AudioMixerSnapshots");
		}
		return AudioMixerSnapshots.instance;
	}

	// Token: 0x04003EEF RID: 16111
	public EventReference TechFilterOnMigrated;

	// Token: 0x04003EF0 RID: 16112
	public EventReference TechFilterLogicOn;

	// Token: 0x04003EF1 RID: 16113
	public EventReference NightStartedMigrated;

	// Token: 0x04003EF2 RID: 16114
	public EventReference MenuOpenMigrated;

	// Token: 0x04003EF3 RID: 16115
	public EventReference MenuOpenHalfEffect;

	// Token: 0x04003EF4 RID: 16116
	public EventReference SpeedPausedMigrated;

	// Token: 0x04003EF5 RID: 16117
	public EventReference DuplicantCountAttenuatorMigrated;

	// Token: 0x04003EF6 RID: 16118
	public EventReference NewBaseSetupSnapshot;

	// Token: 0x04003EF7 RID: 16119
	public EventReference FrontEndSnapshot;

	// Token: 0x04003EF8 RID: 16120
	public EventReference FrontEndWelcomeScreenSnapshot;

	// Token: 0x04003EF9 RID: 16121
	public EventReference FrontEndWorldGenerationSnapshot;

	// Token: 0x04003EFA RID: 16122
	public EventReference IntroNIS;

	// Token: 0x04003EFB RID: 16123
	public EventReference PulseSnapshot;

	// Token: 0x04003EFC RID: 16124
	public EventReference ESCPauseSnapshot;

	// Token: 0x04003EFD RID: 16125
	public EventReference MENUNewDuplicantSnapshot;

	// Token: 0x04003EFE RID: 16126
	public EventReference UserVolumeSettingsSnapshot;

	// Token: 0x04003EFF RID: 16127
	public EventReference DuplicantCountMovingSnapshot;

	// Token: 0x04003F00 RID: 16128
	public EventReference DuplicantCountSleepingSnapshot;

	// Token: 0x04003F01 RID: 16129
	public EventReference PortalLPDimmedSnapshot;

	// Token: 0x04003F02 RID: 16130
	public EventReference DynamicMusicPlayingSnapshot;

	// Token: 0x04003F03 RID: 16131
	public EventReference FabricatorSideScreenOpenSnapshot;

	// Token: 0x04003F04 RID: 16132
	public EventReference SpaceVisibleSnapshot;

	// Token: 0x04003F05 RID: 16133
	public EventReference MENUStarmapSnapshot;

	// Token: 0x04003F06 RID: 16134
	public EventReference MENUStarmapNotPausedSnapshot;

	// Token: 0x04003F07 RID: 16135
	public EventReference GameNotFocusedSnapshot;

	// Token: 0x04003F08 RID: 16136
	public EventReference FacilityVisibleSnapshot;

	// Token: 0x04003F09 RID: 16137
	public EventReference TutorialVideoPlayingSnapshot;

	// Token: 0x04003F0A RID: 16138
	public EventReference VictoryMessageSnapshot;

	// Token: 0x04003F0B RID: 16139
	public EventReference VictoryNISGenericSnapshot;

	// Token: 0x04003F0C RID: 16140
	public EventReference VictoryNISRocketSnapshot;

	// Token: 0x04003F0D RID: 16141
	public EventReference VictoryCinematicSnapshot;

	// Token: 0x04003F0E RID: 16142
	public EventReference VictoryFadeToBlackSnapshot;

	// Token: 0x04003F0F RID: 16143
	public EventReference MuteDynamicMusicSnapshot;

	// Token: 0x04003F10 RID: 16144
	public EventReference ActiveBaseChangeSnapshot;

	// Token: 0x04003F11 RID: 16145
	public EventReference EventPopupSnapshot;

	// Token: 0x04003F12 RID: 16146
	public EventReference SmallRocketInteriorReverbSnapshot;

	// Token: 0x04003F13 RID: 16147
	public EventReference MediumRocketInteriorReverbSnapshot;

	// Token: 0x04003F14 RID: 16148
	public EventReference MainMenuVideoPlayingSnapshot;

	// Token: 0x04003F15 RID: 16149
	public EventReference TechFilterRadiationOn;

	// Token: 0x04003F16 RID: 16150
	public EventReference FrontEndSupplyClosetSnapshot;

	// Token: 0x04003F17 RID: 16151
	public EventReference FrontEndItemDropScreenSnapshot;

	// Token: 0x04003F18 RID: 16152
	[SerializeField]
	private EventReference[] snapshots;

	// Token: 0x04003F19 RID: 16153
	[NonSerialized]
	public List<string> snapshotMap = new List<string>();

	// Token: 0x04003F1A RID: 16154
	private static AudioMixerSnapshots instance;
}
