using System;
using System.Collections.Generic;
using Database;
using Klei.CustomSettings;
using KSerialization;
using UnityEngine;

// Token: 0x02000BE3 RID: 3043
[SerializationConfig(MemberSerialization.OptIn)]
public class StoryManager : KMonoBehaviour
{
	// Token: 0x1700069E RID: 1694
	// (get) Token: 0x06005B15 RID: 23317 RVA: 0x0020FA98 File Offset: 0x0020DC98
	// (set) Token: 0x06005B16 RID: 23318 RVA: 0x0020FA9F File Offset: 0x0020DC9F
	public static StoryManager Instance { get; private set; }

	// Token: 0x06005B17 RID: 23319 RVA: 0x0020FAA7 File Offset: 0x0020DCA7
	public static IReadOnlyList<StoryManager.StoryTelemetry> GetTelemetry()
	{
		return StoryManager.storyTelemetry;
	}

	// Token: 0x06005B18 RID: 23320 RVA: 0x0020FAB0 File Offset: 0x0020DCB0
	protected override void OnPrefabInit()
	{
		StoryManager.Instance = this;
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDayStarted));
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Combine(instance.OnLoad, new Action<Game.GameSaveData>(this.OnGameLoaded));
	}

	// Token: 0x06005B19 RID: 23321 RVA: 0x0020FB08 File Offset: 0x0020DD08
	protected override void OnCleanUp()
	{
		GameClock.Instance.Unsubscribe(631075836, new Action<object>(this.OnNewDayStarted));
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Remove(instance.OnLoad, new Action<Game.GameSaveData>(this.OnGameLoaded));
	}

	// Token: 0x06005B1A RID: 23322 RVA: 0x0020FB58 File Offset: 0x0020DD58
	public void InitialSaveSetup()
	{
		this.highestStoryCoordinateWhenGenerated = Db.Get().Stories.GetHighestCoordinate();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			foreach (string storyTraitTemplate in worldContainer.StoryTraitIds)
			{
				Story storyFromStoryTrait = Db.Get().Stories.GetStoryFromStoryTrait(storyTraitTemplate);
				this.CreateStory(storyFromStoryTrait, worldContainer.id);
			}
		}
		this.LogInitialSaveSetup();
	}

	// Token: 0x06005B1B RID: 23323 RVA: 0x0020FC20 File Offset: 0x0020DE20
	public StoryInstance CreateStory(string id, int worldId)
	{
		Story story = Db.Get().Stories.Get(id);
		return this.CreateStory(story, worldId);
	}

	// Token: 0x06005B1C RID: 23324 RVA: 0x0020FC48 File Offset: 0x0020DE48
	public StoryInstance CreateStory(Story story, int worldId)
	{
		StoryInstance storyInstance = new StoryInstance(story, worldId);
		this._stories.Add(story.HashId, storyInstance);
		StoryManager.InitTelemetry(storyInstance);
		if (story.autoStart)
		{
			this.BeginStoryEvent(story);
		}
		return storyInstance;
	}

	// Token: 0x06005B1D RID: 23325 RVA: 0x0020FC85 File Offset: 0x0020DE85
	public StoryInstance GetStoryInstance(Story story)
	{
		return this.GetStoryInstance(story.HashId);
	}

	// Token: 0x06005B1E RID: 23326 RVA: 0x0020FC94 File Offset: 0x0020DE94
	public StoryInstance GetStoryInstance(int hash)
	{
		StoryInstance result;
		this._stories.TryGetValue(hash, out result);
		return result;
	}

	// Token: 0x06005B1F RID: 23327 RVA: 0x0020FCB1 File Offset: 0x0020DEB1
	public Dictionary<int, StoryInstance> GetStoryInstances()
	{
		return this._stories;
	}

	// Token: 0x06005B20 RID: 23328 RVA: 0x0020FCB9 File Offset: 0x0020DEB9
	public int GetHighestCoordinate()
	{
		return this.highestStoryCoordinateWhenGenerated;
	}

	// Token: 0x06005B21 RID: 23329 RVA: 0x0020FCC1 File Offset: 0x0020DEC1
	private string GetCompleteUnlockId(string id)
	{
		return id + "_STORY_COMPLETE";
	}

	// Token: 0x06005B22 RID: 23330 RVA: 0x0020FCCE File Offset: 0x0020DECE
	public void ForceCreateStory(Story story, int worldId)
	{
		if (this.GetStoryInstance(story.HashId) == null)
		{
			this.CreateStory(story, worldId);
		}
	}

	// Token: 0x06005B23 RID: 23331 RVA: 0x0020FCE8 File Offset: 0x0020DEE8
	public void DiscoverStoryEvent(Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || this.CheckState(StoryInstance.State.DISCOVERED, story))
		{
			return;
		}
		storyInstance.CurrentState = StoryInstance.State.DISCOVERED;
	}

	// Token: 0x06005B24 RID: 23332 RVA: 0x0020FD18 File Offset: 0x0020DF18
	public void BeginStoryEvent(Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || this.CheckState(StoryInstance.State.IN_PROGRESS, story))
		{
			return;
		}
		storyInstance.CurrentState = StoryInstance.State.IN_PROGRESS;
	}

	// Token: 0x06005B25 RID: 23333 RVA: 0x0020FD47 File Offset: 0x0020DF47
	public void CompleteStoryEvent(Story story, MonoBehaviour keepsakeSpawnTarget, FocusTargetSequence.Data sequenceData)
	{
		if (this.GetStoryInstance(story.HashId) == null || this.CheckState(StoryInstance.State.COMPLETE, story))
		{
			return;
		}
		FocusTargetSequence.Start(keepsakeSpawnTarget, sequenceData);
	}

	// Token: 0x06005B26 RID: 23334 RVA: 0x0020FD6C File Offset: 0x0020DF6C
	public void CompleteStoryEvent(Story story, Vector3 keepsakeSpawnPosition)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null)
		{
			return;
		}
		GameObject prefab = Assets.GetPrefab(storyInstance.GetStory().keepsakePrefabId);
		if (prefab != null)
		{
			keepsakeSpawnPosition.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			GameObject gameObject = Util.KInstantiate(prefab, keepsakeSpawnPosition);
			gameObject.SetActive(true);
			new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, -0.5f, -0.1f)).StartSM();
		}
		storyInstance.CurrentState = StoryInstance.State.COMPLETE;
		Game.Instance.unlocks.Unlock(this.GetCompleteUnlockId(story.Id), true);
	}

	// Token: 0x06005B27 RID: 23335 RVA: 0x0020FE0C File Offset: 0x0020E00C
	public bool CheckState(StoryInstance.State state, Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		return storyInstance != null && storyInstance.CurrentState >= state;
	}

	// Token: 0x06005B28 RID: 23336 RVA: 0x0020FE37 File Offset: 0x0020E037
	public bool IsStoryComplete(Story story)
	{
		return this.CheckState(StoryInstance.State.COMPLETE, story);
	}

	// Token: 0x06005B29 RID: 23337 RVA: 0x0020FE41 File Offset: 0x0020E041
	public bool IsStoryCompleteGlobal(Story story)
	{
		return Game.Instance.unlocks.IsUnlocked(this.GetCompleteUnlockId(story.Id));
	}

	// Token: 0x06005B2A RID: 23338 RVA: 0x0020FE60 File Offset: 0x0020E060
	public StoryInstance DisplayPopup(Story story, StoryManager.PopupInfo info, System.Action popupCB = null, Notification.ClickCallback notificationCB = null)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || storyInstance.HasDisplayedPopup(info.PopupType))
		{
			return null;
		}
		EventInfoData eventInfoData = EventInfoDataHelper.GenerateStoryTraitData(info.Title, info.Description, info.CloseButtonText, info.TextureName, info.PopupType, info.CloseButtonToolTip, info.Minions, popupCB);
		if (info.extraButtons != null && info.extraButtons.Length != 0)
		{
			foreach (StoryManager.ExtraButtonInfo extraButtonInfo in info.extraButtons)
			{
				eventInfoData.SimpleOption(extraButtonInfo.ButtonText, extraButtonInfo.OnButtonClick).tooltip = extraButtonInfo.ButtonToolTip;
			}
		}
		Notification notification = null;
		if (!info.DisplayImmediate)
		{
			notification = EventInfoScreen.CreateNotification(eventInfoData, notificationCB);
		}
		storyInstance.SetPopupData(info, eventInfoData, notification);
		return storyInstance;
	}

	// Token: 0x06005B2B RID: 23339 RVA: 0x0020FF30 File Offset: 0x0020E130
	public bool HasDisplayedPopup(Story story, EventInfoDataHelper.PopupType type)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		return storyInstance != null && storyInstance.HasDisplayedPopup(type);
	}

	// Token: 0x06005B2C RID: 23340 RVA: 0x0020FF58 File Offset: 0x0020E158
	private void LogInitialSaveSetup()
	{
		int num = 0;
		StoryManager.StoryCreationTelemetry[] array = new StoryManager.StoryCreationTelemetry[CustomGameSettings.Instance.CurrentStoryLevelsBySetting.Count];
		foreach (KeyValuePair<string, string> keyValuePair in CustomGameSettings.Instance.CurrentStoryLevelsBySetting)
		{
			array[num] = new StoryManager.StoryCreationTelemetry
			{
				StoryId = keyValuePair.Key,
				Enabled = CustomGameSettings.Instance.IsStoryActive(keyValuePair.Key, keyValuePair.Value)
			};
			num++;
		}
		OniMetrics.LogEvent(OniMetrics.Event.NewSave, "StoryTraitsCreation", array);
	}

	// Token: 0x06005B2D RID: 23341 RVA: 0x0020FFFC File Offset: 0x0020E1FC
	private void OnNewDayStarted(object _)
	{
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, "SavedHighestStoryCoordinate", this.highestStoryCoordinateWhenGenerated);
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, "StoryTraits", StoryManager.storyTelemetry);
	}

	// Token: 0x06005B2E RID: 23342 RVA: 0x00210024 File Offset: 0x0020E224
	private static void InitTelemetry(StoryInstance story)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(story.worldId);
		if (world == null)
		{
			return;
		}
		story.Telemetry.StoryId = story.storyId;
		story.Telemetry.WorldId = world.worldName;
		StoryManager.storyTelemetry.Add(story.Telemetry);
	}

	// Token: 0x06005B2F RID: 23343 RVA: 0x00210080 File Offset: 0x0020E280
	private void OnGameLoaded(object _)
	{
		StoryManager.storyTelemetry.Clear();
		foreach (KeyValuePair<int, StoryInstance> keyValuePair in this._stories)
		{
			StoryManager.InitTelemetry(keyValuePair.Value);
		}
		CustomGameSettings.Instance.DisableAllStories();
		foreach (KeyValuePair<int, StoryInstance> keyValuePair2 in this._stories)
		{
			SettingConfig config;
			if (keyValuePair2.Value.Telemetry.Retrofitted < 0f && CustomGameSettings.Instance.StorySettings.TryGetValue(keyValuePair2.Value.storyId, out config))
			{
				CustomGameSettings.Instance.SetStorySetting(config, true);
			}
		}
	}

	// Token: 0x06005B30 RID: 23344 RVA: 0x0021016C File Offset: 0x0020E36C
	public static void DestroyInstance()
	{
		StoryManager.storyTelemetry.Clear();
		StoryManager.Instance = null;
	}

	// Token: 0x04003CBF RID: 15551
	public const int BEFORE_STORIES = -2;

	// Token: 0x04003CC1 RID: 15553
	private static List<StoryManager.StoryTelemetry> storyTelemetry = new List<StoryManager.StoryTelemetry>();

	// Token: 0x04003CC2 RID: 15554
	[Serialize]
	private Dictionary<int, StoryInstance> _stories = new Dictionary<int, StoryInstance>();

	// Token: 0x04003CC3 RID: 15555
	[Serialize]
	private int highestStoryCoordinateWhenGenerated = -2;

	// Token: 0x04003CC4 RID: 15556
	private const string STORY_TRAIT_KEY = "StoryTraits";

	// Token: 0x04003CC5 RID: 15557
	private const string STORY_CREATION_KEY = "StoryTraitsCreation";

	// Token: 0x04003CC6 RID: 15558
	private const string STORY_COORDINATE_KEY = "SavedHighestStoryCoordinate";

	// Token: 0x02001D74 RID: 7540
	public struct ExtraButtonInfo
	{
		// Token: 0x04008B59 RID: 35673
		public string ButtonText;

		// Token: 0x04008B5A RID: 35674
		public string ButtonToolTip;

		// Token: 0x04008B5B RID: 35675
		public System.Action OnButtonClick;
	}

	// Token: 0x02001D75 RID: 7541
	public struct PopupInfo
	{
		// Token: 0x04008B5C RID: 35676
		public string Title;

		// Token: 0x04008B5D RID: 35677
		public string Description;

		// Token: 0x04008B5E RID: 35678
		public string CloseButtonText;

		// Token: 0x04008B5F RID: 35679
		public string CloseButtonToolTip;

		// Token: 0x04008B60 RID: 35680
		public StoryManager.ExtraButtonInfo[] extraButtons;

		// Token: 0x04008B61 RID: 35681
		public string TextureName;

		// Token: 0x04008B62 RID: 35682
		public GameObject[] Minions;

		// Token: 0x04008B63 RID: 35683
		public bool DisplayImmediate;

		// Token: 0x04008B64 RID: 35684
		public EventInfoDataHelper.PopupType PopupType;
	}

	// Token: 0x02001D76 RID: 7542
	[SerializationConfig(MemberSerialization.OptIn)]
	public class StoryTelemetry : ISaveLoadable
	{
		// Token: 0x0600B135 RID: 45365 RVA: 0x003DCA0C File Offset: 0x003DAC0C
		public void LogStateChange(StoryInstance.State state, float time)
		{
			switch (state)
			{
			case StoryInstance.State.RETROFITTED:
				this.Retrofitted = ((this.Retrofitted >= 0f) ? this.Retrofitted : time);
				return;
			case StoryInstance.State.NOT_STARTED:
				break;
			case StoryInstance.State.DISCOVERED:
				this.Discovered = ((this.Discovered >= 0f) ? this.Discovered : time);
				return;
			case StoryInstance.State.IN_PROGRESS:
				this.InProgress = ((this.InProgress >= 0f) ? this.InProgress : time);
				return;
			case StoryInstance.State.COMPLETE:
				this.Completed = ((this.Completed >= 0f) ? this.Completed : time);
				break;
			default:
				return;
			}
		}

		// Token: 0x04008B65 RID: 35685
		public string StoryId;

		// Token: 0x04008B66 RID: 35686
		public string WorldId;

		// Token: 0x04008B67 RID: 35687
		[Serialize]
		public float Retrofitted = -1f;

		// Token: 0x04008B68 RID: 35688
		[Serialize]
		public float Discovered = -1f;

		// Token: 0x04008B69 RID: 35689
		[Serialize]
		public float InProgress = -1f;

		// Token: 0x04008B6A RID: 35690
		[Serialize]
		public float Completed = -1f;
	}

	// Token: 0x02001D77 RID: 7543
	public class StoryCreationTelemetry
	{
		// Token: 0x04008B6B RID: 35691
		public string StoryId;

		// Token: 0x04008B6C RID: 35692
		public bool Enabled;
	}
}
