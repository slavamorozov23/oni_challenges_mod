using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Klei.CustomSettings;
using KSerialization;
using Newtonsoft.Json;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02000B27 RID: 2855
[SerializationConfig(KSerialization.MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SaveGame")]
public class SaveGame : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x06005369 RID: 21353 RVA: 0x001E619F File Offset: 0x001E439F
	// (set) Token: 0x0600536A RID: 21354 RVA: 0x001E61A7 File Offset: 0x001E43A7
	public int AutoSaveCycleInterval
	{
		get
		{
			return this.autoSaveCycleInterval;
		}
		set
		{
			this.autoSaveCycleInterval = value;
		}
	}

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x0600536B RID: 21355 RVA: 0x001E61B0 File Offset: 0x001E43B0
	// (set) Token: 0x0600536C RID: 21356 RVA: 0x001E61B8 File Offset: 0x001E43B8
	public Vector2I TimelapseResolution
	{
		get
		{
			return this.timelapseResolution;
		}
		set
		{
			this.timelapseResolution = value;
		}
	}

	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x0600536D RID: 21357 RVA: 0x001E61C1 File Offset: 0x001E43C1
	public string BaseName
	{
		get
		{
			return this.baseName;
		}
	}

	// Token: 0x0600536E RID: 21358 RVA: 0x001E61C9 File Offset: 0x001E43C9
	public static void DestroyInstance()
	{
		SaveGame.Instance = null;
	}

	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x0600536F RID: 21359 RVA: 0x001E61D1 File Offset: 0x001E43D1
	public ColonyAchievementTracker ColonyAchievementTracker
	{
		get
		{
			if (this.colonyAchievementTracker == null)
			{
				this.colonyAchievementTracker = base.GetComponent<ColonyAchievementTracker>();
			}
			return this.colonyAchievementTracker;
		}
	}

	// Token: 0x06005370 RID: 21360 RVA: 0x001E61F4 File Offset: 0x001E43F4
	protected override void OnPrefabInit()
	{
		SaveGame.Instance = this;
		new ColonyRationMonitor.Instance(this).StartSM();
		this.entombedItemManager = base.gameObject.AddComponent<EntombedItemManager>();
		this.worldGenSpawner = base.gameObject.AddComponent<WorldGenSpawner>();
		base.gameObject.AddOrGetDef<GameplaySeasonManager.Def>();
		base.gameObject.AddOrGetDef<ClusterFogOfWarManager.Def>();
	}

	// Token: 0x06005371 RID: 21361 RVA: 0x001E624C File Offset: 0x001E444C
	[OnSerializing]
	private void OnSerialize()
	{
		this.speed = SpeedControlScreen.Instance.GetSpeed();
	}

	// Token: 0x06005372 RID: 21362 RVA: 0x001E625E File Offset: 0x001E445E
	[OnDeserializing]
	private void OnDeserialize()
	{
		this.baseName = SaveLoader.Instance.GameInfo.baseName;
	}

	// Token: 0x06005373 RID: 21363 RVA: 0x001E6275 File Offset: 0x001E4475
	public int GetSpeed()
	{
		return this.speed;
	}

	// Token: 0x06005374 RID: 21364 RVA: 0x001E6280 File Offset: 0x001E4480
	public byte[] GetSaveHeader(bool isAutoSave, bool isCompressed, out SaveGame.Header header)
	{
		string originalSaveFileName = SaveLoader.GetOriginalSaveFileName(SaveLoader.GetActiveSaveFilePath());
		string s = JsonConvert.SerializeObject(new SaveGame.GameInfo(GameClock.Instance.GetCycle(), Components.LiveMinionIdentities.Count, this.baseName, isAutoSave, originalSaveFileName, SaveLoader.Instance.GameInfo.clusterId, SaveLoader.Instance.GameInfo.worldTraits, SaveLoader.Instance.GameInfo.colonyGuid, SaveLoader.Instance.GameInfo.dlcIds, this.sandboxEnabled));
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		header = default(SaveGame.Header);
		header.buildVersion = 706793U;
		header.headerSize = bytes.Length;
		header.headerVersion = 1U;
		header.compression = (isCompressed ? 1 : 0);
		return bytes;
	}

	// Token: 0x06005375 RID: 21365 RVA: 0x001E6344 File Offset: 0x001E4544
	public static string GetSaveUniqueID(SaveGame.GameInfo info)
	{
		if (!(info.colonyGuid != Guid.Empty))
		{
			return info.baseName + "/" + info.clusterId;
		}
		return info.colonyGuid.ToString();
	}

	// Token: 0x06005376 RID: 21366 RVA: 0x001E6384 File Offset: 0x001E4584
	public static global::Tuple<SaveGame.Header, SaveGame.GameInfo> GetFileInfo(string filename)
	{
		try
		{
			SaveGame.Header a;
			SaveGame.GameInfo gameInfo = SaveLoader.LoadHeader(filename, out a);
			if (gameInfo.saveMajorVersion >= 7)
			{
				return new global::Tuple<SaveGame.Header, SaveGame.GameInfo>(a, gameInfo);
			}
		}
		catch (Exception obj)
		{
			global::Debug.LogWarning("Exception while loading " + filename);
			global::Debug.LogWarning(obj);
		}
		return null;
	}

	// Token: 0x06005377 RID: 21367 RVA: 0x001E63DC File Offset: 0x001E45DC
	public static SaveGame.GameInfo GetHeader(IReader br, out SaveGame.Header header, string debugFileName)
	{
		header = default(SaveGame.Header);
		header.buildVersion = br.ReadUInt32();
		header.headerSize = br.ReadInt32();
		header.headerVersion = br.ReadUInt32();
		if (1U <= header.headerVersion)
		{
			header.compression = br.ReadInt32();
		}
		byte[] data = br.ReadBytes(header.headerSize);
		if (header.headerSize == 0 && !SaveGame.debug_SaveFileHeaderBlank_sent)
		{
			SaveGame.debug_SaveFileHeaderBlank_sent = true;
			global::Debug.LogWarning("SaveFileHeaderBlank - " + debugFileName);
		}
		SaveGame.GameInfo gameInfo = SaveGame.GetGameInfo(data);
		if (gameInfo.IsVersionOlderThan(7, 14) && gameInfo.worldTraits != null)
		{
			string[] worldTraits = gameInfo.worldTraits;
			for (int i = 0; i < worldTraits.Length; i++)
			{
				worldTraits[i] = worldTraits[i].Replace('\\', '/');
			}
		}
		if (gameInfo.IsVersionOlderThan(7, 20))
		{
			gameInfo.dlcId = "";
		}
		if (gameInfo.IsVersionOlderThan(7, 34))
		{
			gameInfo.dlcIds = new List<string>
			{
				gameInfo.dlcId
			};
		}
		return gameInfo;
	}

	// Token: 0x06005378 RID: 21368 RVA: 0x001E64D5 File Offset: 0x001E46D5
	public static SaveGame.GameInfo GetGameInfo(byte[] data)
	{
		return JsonConvert.DeserializeObject<SaveGame.GameInfo>(Encoding.UTF8.GetString(data));
	}

	// Token: 0x06005379 RID: 21369 RVA: 0x001E64E7 File Offset: 0x001E46E7
	public void SetBaseName(string newBaseName)
	{
		if (string.IsNullOrEmpty(newBaseName))
		{
			global::Debug.LogWarning("Cannot give the base an empty name");
			return;
		}
		this.baseName = newBaseName;
	}

	// Token: 0x0600537A RID: 21370 RVA: 0x001E6503 File Offset: 0x001E4703
	protected override void OnSpawn()
	{
		ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
		Game.Instance.Trigger(-1917495436, null);
	}

	// Token: 0x0600537B RID: 21371 RVA: 0x001E6520 File Offset: 0x001E4720
	public List<global::Tuple<string, TextStyleSetting>> GetColonyToolTip()
	{
		List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		ClusterLayout clusterLayout;
		SettingsCache.clusterLayouts.clusterCache.TryGetValue(currentQualitySetting.id, out clusterLayout);
		list.Add(new global::Tuple<string, TextStyleSetting>(this.baseName, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		if (DlcManager.IsExpansion1Active())
		{
			StringEntry entry = Strings.Get(clusterLayout.name);
			list.Add(new global::Tuple<string, TextStyleSetting>(entry, ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		if (GameClock.Instance != null)
		{
			list.Add(new global::Tuple<string, TextStyleSetting>(" ", null));
			list.Add(new global::Tuple<string, TextStyleSetting>(string.Format(UI.ASTEROIDCLOCK.CYCLES_OLD, GameUtil.GetCurrentCycle()), ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			list.Add(new global::Tuple<string, TextStyleSetting>(string.Format(UI.ASTEROIDCLOCK.TIME_PLAYED, (GameClock.Instance.GetTimePlayedInSeconds() / 3600f).ToString("0.00")), ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		int cameraActiveCluster = CameraController.Instance.cameraActiveCluster;
		WorldContainer world = ClusterManager.Instance.GetWorld(cameraActiveCluster);
		list.Add(new global::Tuple<string, TextStyleSetting>(" ", null));
		if (DlcManager.IsExpansion1Active())
		{
			list.Add(new global::Tuple<string, TextStyleSetting>(world.GetComponent<ClusterGridEntity>().Name, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		}
		else
		{
			StringEntry entry2 = Strings.Get(clusterLayout.name);
			list.Add(new global::Tuple<string, TextStyleSetting>(entry2, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
		}
		if (SaveLoader.Instance.GameInfo.worldTraits != null && SaveLoader.Instance.GameInfo.worldTraits.Length != 0)
		{
			string[] worldTraits = SaveLoader.Instance.GameInfo.worldTraits;
			for (int i = 0; i < worldTraits.Length; i++)
			{
				WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(worldTraits[i], false);
				if (cachedWorldTrait != null)
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(Strings.Get(cachedWorldTrait.name), ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
				else
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.MISSING_TRAIT, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
			}
		}
		else if (world.WorldTraitIds != null)
		{
			foreach (string name in world.WorldTraitIds)
			{
				WorldTrait cachedWorldTrait2 = SettingsCache.GetCachedWorldTrait(name, false);
				if (cachedWorldTrait2 != null)
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(Strings.Get(cachedWorldTrait2.name), ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
				else
				{
					list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.MISSING_TRAIT, ToolTipScreen.Instance.defaultTooltipBodyStyle));
				}
			}
			if (world.WorldTraitIds.Count == 0)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(WORLD_TRAITS.NO_TRAITS.NAME_SHORTHAND, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			}
		}
		return list;
	}

	// Token: 0x04003876 RID: 14454
	[Serialize]
	private int speed;

	// Token: 0x04003877 RID: 14455
	[Serialize]
	public List<Tag> expandedResourceTags = new List<Tag>();

	// Token: 0x04003878 RID: 14456
	[Serialize]
	public int minGermCountForDisinfect = 10000;

	// Token: 0x04003879 RID: 14457
	[Serialize]
	public bool enableAutoDisinfect = true;

	// Token: 0x0400387A RID: 14458
	[Serialize]
	public bool sandboxEnabled;

	// Token: 0x0400387B RID: 14459
	[Serialize]
	public float relativeTemperatureOverlaySliderValue = 294.15f;

	// Token: 0x0400387C RID: 14460
	[Serialize]
	private int autoSaveCycleInterval = 1;

	// Token: 0x0400387D RID: 14461
	[Serialize]
	private Vector2I timelapseResolution = new Vector2I(512, 768);

	// Token: 0x0400387E RID: 14462
	private string baseName;

	// Token: 0x0400387F RID: 14463
	public static SaveGame Instance;

	// Token: 0x04003880 RID: 14464
	private ColonyAchievementTracker colonyAchievementTracker;

	// Token: 0x04003881 RID: 14465
	public EntombedItemManager entombedItemManager;

	// Token: 0x04003882 RID: 14466
	public WorldGenSpawner worldGenSpawner;

	// Token: 0x04003883 RID: 14467
	[MyCmpReq]
	public MaterialSelectorSerializer materialSelectorSerializer;

	// Token: 0x04003884 RID: 14468
	private static bool debug_SaveFileHeaderBlank_sent;

	// Token: 0x02001C75 RID: 7285
	public struct Header
	{
		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x0600ADB8 RID: 44472 RVA: 0x003D1248 File Offset: 0x003CF448
		public bool IsCompressed
		{
			get
			{
				return this.compression != 0;
			}
		}

		// Token: 0x04008829 RID: 34857
		public uint buildVersion;

		// Token: 0x0400882A RID: 34858
		public int headerSize;

		// Token: 0x0400882B RID: 34859
		public uint headerVersion;

		// Token: 0x0400882C RID: 34860
		public int compression;
	}

	// Token: 0x02001C76 RID: 7286
	public struct GameInfo
	{
		// Token: 0x0600ADB9 RID: 44473 RVA: 0x003D1254 File Offset: 0x003CF454
		public GameInfo(int numberOfCycles, int numberOfDuplicants, string baseName, bool isAutoSave, string originalSaveName, string clusterId, string[] worldTraits, Guid colonyGuid, List<string> dlcIds, bool sandboxEnabled = false)
		{
			this.numberOfCycles = numberOfCycles;
			this.numberOfDuplicants = numberOfDuplicants;
			this.baseName = baseName;
			this.isAutoSave = isAutoSave;
			this.originalSaveName = originalSaveName;
			this.clusterId = clusterId;
			this.worldTraits = worldTraits;
			this.colonyGuid = colonyGuid;
			this.sandboxEnabled = sandboxEnabled;
			this.dlcIds = dlcIds;
			this.dlcId = null;
			this.saveMajorVersion = 7;
			this.saveMinorVersion = 37;
		}

		// Token: 0x0600ADBA RID: 44474 RVA: 0x003D12C4 File Offset: 0x003CF4C4
		public bool IsVersionOlderThan(int major, int minor)
		{
			return this.saveMajorVersion < major || (this.saveMajorVersion == major && this.saveMinorVersion < minor);
		}

		// Token: 0x0600ADBB RID: 44475 RVA: 0x003D12E5 File Offset: 0x003CF4E5
		public bool IsVersionExactly(int major, int minor)
		{
			return this.saveMajorVersion == major && this.saveMinorVersion == minor;
		}

		// Token: 0x0600ADBC RID: 44476 RVA: 0x003D12FC File Offset: 0x003CF4FC
		public bool IsCompatableWithCurrentDlcConfiguration(out HashSet<string> dlcIdsToEnable, out HashSet<string> dlcIdToDisable)
		{
			dlcIdsToEnable = new HashSet<string>();
			foreach (string item in this.dlcIds)
			{
				if (!DlcManager.IsContentSubscribed(item))
				{
					dlcIdsToEnable.Add(item);
				}
			}
			dlcIdToDisable = new HashSet<string>();
			if (!this.dlcIds.Contains("EXPANSION1_ID") && DlcManager.IsExpansion1Active())
			{
				dlcIdToDisable.Add("EXPANSION1_ID");
			}
			return dlcIdsToEnable.Count == 0 && dlcIdToDisable.Count == 0;
		}

		// Token: 0x0400882D RID: 34861
		public int numberOfCycles;

		// Token: 0x0400882E RID: 34862
		public int numberOfDuplicants;

		// Token: 0x0400882F RID: 34863
		public string baseName;

		// Token: 0x04008830 RID: 34864
		public bool isAutoSave;

		// Token: 0x04008831 RID: 34865
		public string originalSaveName;

		// Token: 0x04008832 RID: 34866
		public int saveMajorVersion;

		// Token: 0x04008833 RID: 34867
		public int saveMinorVersion;

		// Token: 0x04008834 RID: 34868
		public string clusterId;

		// Token: 0x04008835 RID: 34869
		public string[] worldTraits;

		// Token: 0x04008836 RID: 34870
		public bool sandboxEnabled;

		// Token: 0x04008837 RID: 34871
		public Guid colonyGuid;

		// Token: 0x04008838 RID: 34872
		[Obsolete("Please use dlcIds instead.")]
		public string dlcId;

		// Token: 0x04008839 RID: 34873
		public List<string> dlcIds;
	}
}
