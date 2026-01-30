using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200057A RID: 1402
public class GameAudioSheets : AudioSheets
{
	// Token: 0x06001F29 RID: 7977 RVA: 0x000A9C2A File Offset: 0x000A7E2A
	public static GameAudioSheets Get()
	{
		if (GameAudioSheets._Instance == null)
		{
			GameAudioSheets._Instance = Resources.Load<GameAudioSheets>("GameAudioSheets");
		}
		return GameAudioSheets._Instance;
	}

	// Token: 0x06001F2A RID: 7978 RVA: 0x000A9C50 File Offset: 0x000A7E50
	public override void Initialize()
	{
		this.validFileNames.Add("game_triggered");
		foreach (KAnimFile kanimFile in Assets.instance.AnimAssets)
		{
			if (!(kanimFile == null))
			{
				this.validFileNames.Add(kanimFile.name);
			}
		}
		base.Initialize();
		foreach (AudioSheet audioSheet in this.sheets)
		{
			foreach (AudioSheet.SoundInfo soundInfo in audioSheet.soundInfos)
			{
				if (soundInfo.Type == "MouthFlapSoundEvent" || soundInfo.Type == "VoiceSoundEvent")
				{
					HashSet<HashedString> hashSet = null;
					if (!this.animsNotAllowedToPlaySpeech.TryGetValue(soundInfo.File, out hashSet))
					{
						hashSet = new HashSet<HashedString>();
						this.animsNotAllowedToPlaySpeech[soundInfo.File] = hashSet;
					}
					hashSet.Add(soundInfo.Anim);
				}
			}
		}
	}

	// Token: 0x06001F2B RID: 7979 RVA: 0x000A9DB8 File Offset: 0x000A7FB8
	protected override AnimEvent CreateSoundOfType(string type, string file_name, string sound_name, int frame, float min_interval, string dlcId)
	{
		SoundEvent soundEvent = null;
		bool shouldCameraScalePosition = true;
		if (sound_name.Contains(":disable_camera_position_scaling"))
		{
			sound_name = sound_name.Replace(":disable_camera_position_scaling", "");
			shouldCameraScalePosition = false;
		}
		if (type == "FloorSoundEvent")
		{
			soundEvent = new FloorSoundEvent(file_name, sound_name, frame);
		}
		else if (type == "SoundEvent" || type == "LoopingSoundEvent")
		{
			bool is_looping = type == "LoopingSoundEvent";
			string[] array = sound_name.Split(':', StringSplitOptions.None);
			sound_name = array[0];
			soundEvent = new SoundEvent(file_name, sound_name, frame, true, is_looping, min_interval, false);
			for (int i = 1; i < array.Length; i++)
			{
				if (array[i] == "IGNORE_PAUSE")
				{
					soundEvent.ignorePause = true;
				}
				else
				{
					global::Debug.LogWarning(sound_name + " has unknown parameter " + array[i]);
				}
			}
		}
		else if (type == "LadderSoundEvent")
		{
			soundEvent = new LadderSoundEvent(file_name, sound_name, frame);
		}
		else if (type == "LaserSoundEvent")
		{
			soundEvent = new LaserSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "HatchDrillSoundEvent")
		{
			soundEvent = new HatchDrillSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "CreatureChewSoundEvent")
		{
			soundEvent = new CreatureChewSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "BuildingDamageSoundEvent")
		{
			soundEvent = new BuildingDamageSoundEvent(file_name, sound_name, frame);
		}
		else if (type == "WallDamageSoundEvent")
		{
			soundEvent = new WallDamageSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "RemoteSoundEvent")
		{
			soundEvent = new RemoteSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "VoiceSoundEvent" || type == "LoopingVoiceSoundEvent")
		{
			soundEvent = new VoiceSoundEvent(file_name, sound_name, frame, type == "LoopingVoiceSoundEvent");
		}
		else if (type == "MouthFlapSoundEvent")
		{
			soundEvent = new MouthFlapSoundEvent(file_name, sound_name, frame, false);
		}
		else if (type == "MainMenuSoundEvent")
		{
			soundEvent = new MainMenuSoundEvent(file_name, sound_name, frame);
		}
		else if (type == "ClusterMapSoundEvent")
		{
			soundEvent = new ClusterMapSoundEvent(file_name, sound_name, frame, false);
		}
		else if (type == "ClusterMapLoopingSoundEvent")
		{
			soundEvent = new ClusterMapSoundEvent(file_name, sound_name, frame, true);
		}
		else if (type == "UIAnimationSoundEvent")
		{
			soundEvent = new UIAnimationSoundEvent(file_name, sound_name, frame, false);
		}
		else if (type == "UIAnimationVoiceSoundEvent")
		{
			soundEvent = new UIAnimationVoiceSoundEvent(file_name, sound_name, frame, false);
		}
		else if (type == "UIAnimationLoopingSoundEvent")
		{
			soundEvent = new UIAnimationSoundEvent(file_name, sound_name, frame, true);
		}
		else if (type == "CreatureVariationSoundEvent" || type == "CreatureVariationLoopingSoundEvent")
		{
			soundEvent = new CreatureVariationSoundEvent(file_name, sound_name, frame, true, type == "CreatureVariationLoopingSoundEvent", min_interval, false);
		}
		else if (type == "CountedSoundEvent")
		{
			soundEvent = new CountedSoundEvent(file_name, sound_name, frame, true, false, min_interval, false);
		}
		else if (type == "SculptingSoundEvent")
		{
			soundEvent = new SculptingSoundEvent(file_name, sound_name, frame, true, false, min_interval, false);
		}
		else if (type == "PhonoboxSoundEvent")
		{
			soundEvent = new PhonoboxSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "PlantMutationSoundEvent")
		{
			soundEvent = new PlantMutationSoundEvent(file_name, sound_name, frame, min_interval);
		}
		if (soundEvent != null)
		{
			soundEvent.shouldCameraScalePosition = shouldCameraScalePosition;
		}
		return soundEvent;
	}

	// Token: 0x06001F2C RID: 7980 RVA: 0x000AA10C File Offset: 0x000A830C
	public bool IsAnimAllowedToPlaySpeech(KAnim.Anim anim)
	{
		HashSet<HashedString> hashSet = null;
		return !this.animsNotAllowedToPlaySpeech.TryGetValue(anim.animFile.name, out hashSet) || !hashSet.Contains(anim.hash);
	}

	// Token: 0x0400122E RID: 4654
	private static GameAudioSheets _Instance;

	// Token: 0x0400122F RID: 4655
	private HashSet<HashedString> validFileNames = new HashSet<HashedString>();

	// Token: 0x04001230 RID: 4656
	private Dictionary<HashedString, HashSet<HashedString>> animsNotAllowedToPlaySpeech = new Dictionary<HashedString, HashSet<HashedString>>();

	// Token: 0x02001407 RID: 5127
	private class SingleAudioSheetLoader : AsyncLoader
	{
		// Token: 0x06008E70 RID: 36464 RVA: 0x003690C8 File Offset: 0x003672C8
		public override void Run()
		{
			this.sheet.soundInfos = new ResourceLoader<AudioSheet.SoundInfo>(this.text, this.name).resources.ToArray();
		}

		// Token: 0x04006D43 RID: 27971
		public AudioSheet sheet;

		// Token: 0x04006D44 RID: 27972
		public string text;

		// Token: 0x04006D45 RID: 27973
		public string name;
	}

	// Token: 0x02001408 RID: 5128
	private class GameAudioSheetLoader : GlobalAsyncLoader<GameAudioSheets.GameAudioSheetLoader>
	{
		// Token: 0x06008E72 RID: 36466 RVA: 0x003690F8 File Offset: 0x003672F8
		public override void CollectLoaders(List<AsyncLoader> loaders)
		{
			foreach (AudioSheet audioSheet in GameAudioSheets.Get().sheets)
			{
				loaders.Add(new GameAudioSheets.SingleAudioSheetLoader
				{
					sheet = audioSheet,
					text = audioSheet.asset.text,
					name = audioSheet.asset.name
				});
			}
		}

		// Token: 0x06008E73 RID: 36467 RVA: 0x0036917C File Offset: 0x0036737C
		public override void Run()
		{
		}
	}
}
