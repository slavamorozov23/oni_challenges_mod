using System;
using System.Collections;
using FMOD.Studio;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02000C64 RID: 3172
public class MinionSelectScreen : CharacterSelectionController
{
	// Token: 0x060060B6 RID: 24758 RVA: 0x00238B98 File Offset: 0x00236D98
	protected override void OnPrefabInit()
	{
		base.IsStarterMinion = true;
		base.OnPrefabInit();
		if (MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MusicManager.instance.SetSongParameter("Music_FrontEnd", "songSection", 2f, true);
		}
		GameObject parent = GameObject.Find("ScreenSpaceOverlayCanvas");
		GameObject gameObject = global::Util.KInstantiateUI(this.wattsonMessagePrefab.gameObject, parent, false);
		gameObject.name = "WattsonMessage";
		gameObject.SetActive(false);
		Game.Instance.Subscribe(-1992507039, new Action<object>(this.OnBaseAlreadyCreated));
		this.backButton.onClick += delegate()
		{
			LoadScreen.ForceStopGame();
			App.LoadScene("frontend");
		};
		this.InitializeContainers();
		base.StartCoroutine(this.SetDefaultMinionsRoutine());
	}

	// Token: 0x060060B7 RID: 24759 RVA: 0x00238C64 File Offset: 0x00236E64
	private IEnumerator SetDefaultMinionsRoutine()
	{
		yield return SequenceUtil.WaitForNextFrame;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id);
		bool flag = clusterData.clusterTags.Contains("CeresCluster");
		bool flag2 = clusterData.clusterTags.Contains("PrehistoricCluster");
		if (flag)
		{
			((CharacterContainer)this.containers[2]).SetMinion(new MinionStartingStats(Db.Get().Personalities.Get("FREYJA"), null, null, false));
			((CharacterContainer)this.containers[1]).GenerateCharacter(true, null);
			((CharacterContainer)this.containers[0]).GenerateCharacter(true, null);
		}
		else if (flag2)
		{
			((CharacterContainer)this.containers[2]).SetMinion(new MinionStartingStats(Db.Get().Personalities.Get("MAYA"), null, null, false));
			((CharacterContainer)this.containers[1]).SetMinion(new MinionStartingStats(Db.Get().Personalities.Get("HIGBY"), null, null, false));
			((CharacterContainer)this.containers[0]).GenerateCharacter(true, null);
		}
		yield break;
	}

	// Token: 0x060060B8 RID: 24760 RVA: 0x00238C74 File Offset: 0x00236E74
	public void SetProceedButtonActive(bool state, string tooltip = null)
	{
		if (state)
		{
			base.EnableProceedButton();
		}
		else
		{
			base.DisableProceedButton();
		}
		ToolTip component = this.proceedButton.GetComponent<ToolTip>();
		if (component != null)
		{
			if (tooltip != null)
			{
				component.toolTip = tooltip;
				return;
			}
			component.ClearMultiStringTooltip();
		}
	}

	// Token: 0x060060B9 RID: 24761 RVA: 0x00238CB8 File Offset: 0x00236EB8
	protected override void OnSpawn()
	{
		this.OnDeliverableAdded();
		base.EnableProceedButton();
		this.proceedButton.GetComponentInChildren<LocText>().text = UI.IMMIGRANTSCREEN.EMBARK;
		this.containers.ForEach(delegate(ITelepadDeliverableContainer container)
		{
			CharacterContainer characterContainer = container as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.DisableSelectButton();
			}
		});
	}

	// Token: 0x060060BA RID: 24762 RVA: 0x00238D18 File Offset: 0x00236F18
	protected override void OnProceed()
	{
		global::Util.KInstantiateUI(this.newBasePrefab.gameObject, GameScreenManager.Instance.ssOverlayCanvas, false);
		MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().NewBaseSetupSnapshot);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWorldGenerationSnapshot, STOP_MODE.ALLOWFADEOUT);
		int num = 0;
		this.selectedDeliverables.Clear();
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = (CharacterContainer)telepadDeliverableContainer;
			this.selectedDeliverables.Add(characterContainer.Stats);
			if (characterContainer.Stats.personality.model == BionicMinionConfig.MODEL)
			{
				num++;
			}
		}
		NewBaseScreen.Instance.Init(SaveLoader.Instance.Cluster, this.selectedDeliverables.ToArray());
		if (this.OnProceedEvent != null)
		{
			this.OnProceedEvent();
		}
		if (Game.IsDlcActiveForCurrentSave("DLC3_ID") && Components.RoleStations.Count > 0)
		{
			BuildingFacade component = Components.RoleStations[0].GetComponent<BuildingFacade>();
			bool flag = !component.IsOriginal;
			if (num == 3 || (!flag && num > 0))
			{
				component.ApplyBuildingFacade(Db.GetBuildingFacades().Get("permit_hqbase_cyberpunk"), false);
			}
		}
		Game.Instance.Trigger(-838649377, null);
		BuildWatermark.Instance.gameObject.SetActive(false);
		this.Deactivate();
	}

	// Token: 0x060060BB RID: 24763 RVA: 0x00238EB0 File Offset: 0x002370B0
	private void OnBaseAlreadyCreated(object data)
	{
		Game.Instance.StopFE();
		Game.Instance.StartBE();
		Game.Instance.SetGameStarted();
		this.Deactivate();
	}

	// Token: 0x060060BC RID: 24764 RVA: 0x00238ED6 File Offset: 0x002370D6
	private void ReshuffleAll()
	{
		if (this.OnReshuffleEvent != null)
		{
			this.OnReshuffleEvent(base.IsStarterMinion);
		}
	}

	// Token: 0x060060BD RID: 24765 RVA: 0x00238EF4 File Offset: 0x002370F4
	public override void OnPressBack()
	{
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.ForceStopEditingTitle();
			}
		}
	}

	// Token: 0x0400409F RID: 16543
	[SerializeField]
	private NewBaseScreen newBasePrefab;

	// Token: 0x040040A0 RID: 16544
	[SerializeField]
	private WattsonMessage wattsonMessagePrefab;

	// Token: 0x040040A1 RID: 16545
	public const string WattsonGameObjName = "WattsonMessage";

	// Token: 0x040040A2 RID: 16546
	public KButton backButton;
}
