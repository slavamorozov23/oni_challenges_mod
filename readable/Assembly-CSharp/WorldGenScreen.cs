using System;
using System.IO;
using ProcGenGame;
using UnityEngine;

// Token: 0x02000ED5 RID: 3797
public class WorldGenScreen : NewGameFlowScreen
{
	// Token: 0x0600797E RID: 31102 RVA: 0x002EB3AA File Offset: 0x002E95AA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		WorldGenScreen.Instance = this;
	}

	// Token: 0x0600797F RID: 31103 RVA: 0x002EB3B8 File Offset: 0x002E95B8
	protected override void OnForcedCleanUp()
	{
		WorldGenScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06007980 RID: 31104 RVA: 0x002EB3C8 File Offset: 0x002E95C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (MainMenu.Instance != null)
		{
			MainMenu.Instance.StopAmbience();
		}
		this.TriggerLoadingMusic();
		UnityEngine.Object.FindObjectOfType<FrontEndBackground>().gameObject.SetActive(false);
		SaveLoader.SetActiveSaveFilePath(null);
		try
		{
			if (File.Exists(WorldGen.WORLDGEN_SAVE_FILENAME))
			{
				File.Delete(WorldGen.WORLDGEN_SAVE_FILENAME);
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				ex.ToString()
			});
		}
		this.offlineWorldGen.Generate();
	}

	// Token: 0x06007981 RID: 31105 RVA: 0x002EB458 File Offset: 0x002E9658
	private void TriggerLoadingMusic()
	{
		if (AudioDebug.Get().musicEnabled && !MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MainMenu.Instance.StopMainMenuMusic();
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWorldGenerationSnapshot);
			MusicManager.instance.PlaySong("Music_FrontEnd", false);
			MusicManager.instance.SetSongParameter("Music_FrontEnd", "songSection", 1f, true);
		}
	}

	// Token: 0x06007982 RID: 31106 RVA: 0x002EB4CB File Offset: 0x002E96CB
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed)
		{
			e.TryConsume(global::Action.Escape);
		}
		if (!e.Consumed)
		{
			e.TryConsume(global::Action.MouseRight);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x040054FE RID: 21758
	[MyCmpReq]
	private OfflineWorldGen offlineWorldGen;

	// Token: 0x040054FF RID: 21759
	public static WorldGenScreen Instance;
}
