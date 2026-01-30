using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E2A RID: 3626
public class CommandModuleSideScreen : SideScreenContent
{
	// Token: 0x06007312 RID: 29458 RVA: 0x002BEE8C File Offset: 0x002BD08C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ScheduleUpdate();
		MultiToggle multiToggle = this.debugVictoryButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			SpaceDestination destination = SpacecraftManager.instance.destinations.Find((SpaceDestination match) => match.GetDestinationType() == Db.Get().SpaceDestinationTypes.Wormhole);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Clothe8Dupes.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Build4NatureReserves.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.ReachedSpace.Id);
			this.target.Launch(destination);
		}));
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.CheckHydrogenRocket());
	}

	// Token: 0x06007313 RID: 29459 RVA: 0x002BEEEC File Offset: 0x002BD0EC
	private bool CheckHydrogenRocket()
	{
		RocketModule rocketModule = this.target.rocketModules.Find((RocketModule match) => match.GetComponent<RocketEngine>());
		return rocketModule != null && rocketModule.GetComponent<RocketEngine>().fuelTag == ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
	}

	// Token: 0x06007314 RID: 29460 RVA: 0x002BEF53 File Offset: 0x002BD153
	private void ScheduleUpdate()
	{
		this.updateHandle = UIScheduler.Instance.Schedule("RefreshCommandModuleSideScreen", 1f, delegate(object o)
		{
			this.RefreshConditions();
			this.ScheduleUpdate();
		}, null, null);
	}

	// Token: 0x06007315 RID: 29461 RVA: 0x002BEF7D File Offset: 0x002BD17D
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LaunchConditionManager>() != null;
	}

	// Token: 0x06007316 RID: 29462 RVA: 0x002BEF8C File Offset: 0x002BD18C
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<LaunchConditionManager>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a LaunchConditionManager component");
			return;
		}
		this.ClearConditions();
		this.ConfigureConditions();
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.CheckHydrogenRocket());
	}

	// Token: 0x06007317 RID: 29463 RVA: 0x002BF000 File Offset: 0x002BD200
	private void ClearConditions()
	{
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.conditionTable)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.conditionTable.Clear();
	}

	// Token: 0x06007318 RID: 29464 RVA: 0x002BF064 File Offset: 0x002BD264
	private void ConfigureConditions()
	{
		foreach (ProcessCondition key in this.target.GetLaunchConditionList())
		{
			GameObject value = Util.KInstantiateUI(this.prefabConditionLineItem, this.conditionListContainer, true);
			this.conditionTable.Add(key, value);
		}
		this.RefreshConditions();
	}

	// Token: 0x06007319 RID: 29465 RVA: 0x002BF0DC File Offset: 0x002BD2DC
	public void RefreshConditions()
	{
		bool flag = false;
		List<ProcessCondition> launchConditionList = this.target.GetLaunchConditionList();
		foreach (ProcessCondition processCondition in launchConditionList)
		{
			if (!this.conditionTable.ContainsKey(processCondition))
			{
				flag = true;
				break;
			}
			GameObject gameObject = this.conditionTable[processCondition];
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			if (processCondition.GetParentCondition() != null && processCondition.GetParentCondition().EvaluateCondition() == ProcessCondition.Status.Failure)
			{
				gameObject.SetActive(false);
			}
			else if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
			}
			ProcessCondition.Status status = processCondition.EvaluateCondition();
			bool flag2 = status == ProcessCondition.Status.Ready;
			component.GetReference<LocText>("Label").text = processCondition.GetStatusMessage(status);
			component.GetReference<LocText>("Label").color = (flag2 ? Color.black : Color.red);
			component.GetReference<Image>("Box").color = (flag2 ? Color.black : Color.red);
			component.GetReference<Image>("Check").gameObject.SetActive(flag2);
			gameObject.GetComponent<ToolTip>().SetSimpleTooltip(processCondition.GetStatusTooltip(status));
		}
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.conditionTable)
		{
			if (!launchConditionList.Contains(keyValuePair.Key))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.ClearConditions();
			this.ConfigureConditions();
		}
		this.destinationButton.gameObject.SetActive(ManagementMenu.StarmapAvailable());
		this.destinationButton.onClick = delegate()
		{
			ManagementMenu.Instance.ToggleStarmap();
		};
	}

	// Token: 0x0600731A RID: 29466 RVA: 0x002BF2DC File Offset: 0x002BD4DC
	protected override void OnCleanUp()
	{
		this.updateHandle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x04004F89 RID: 20361
	private LaunchConditionManager target;

	// Token: 0x04004F8A RID: 20362
	public GameObject conditionListContainer;

	// Token: 0x04004F8B RID: 20363
	public GameObject prefabConditionLineItem;

	// Token: 0x04004F8C RID: 20364
	public MultiToggle destinationButton;

	// Token: 0x04004F8D RID: 20365
	public MultiToggle debugVictoryButton;

	// Token: 0x04004F8E RID: 20366
	[Tooltip("This list is indexed by the ProcessCondition.Status enum")]
	public List<Color> statusColors;

	// Token: 0x04004F8F RID: 20367
	private Dictionary<ProcessCondition, GameObject> conditionTable = new Dictionary<ProcessCondition, GameObject>();

	// Token: 0x04004F90 RID: 20368
	private SchedulerHandle updateHandle;
}
