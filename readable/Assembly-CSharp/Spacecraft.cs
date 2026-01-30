using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000BAA RID: 2986
[SerializationConfig(MemberSerialization.OptIn)]
public class Spacecraft
{
	// Token: 0x0600594D RID: 22861 RVA: 0x00206CBD File Offset: 0x00204EBD
	public Spacecraft(LaunchConditionManager launchConditions)
	{
		this.launchConditions = launchConditions;
	}

	// Token: 0x0600594E RID: 22862 RVA: 0x00206CEE File Offset: 0x00204EEE
	public Spacecraft()
	{
	}

	// Token: 0x17000681 RID: 1665
	// (get) Token: 0x0600594F RID: 22863 RVA: 0x00206D18 File Offset: 0x00204F18
	// (set) Token: 0x06005950 RID: 22864 RVA: 0x00206D25 File Offset: 0x00204F25
	public LaunchConditionManager launchConditions
	{
		get
		{
			return this.refLaunchConditions.Get();
		}
		set
		{
			this.refLaunchConditions.Set(value);
		}
	}

	// Token: 0x06005951 RID: 22865 RVA: 0x00206D33 File Offset: 0x00204F33
	public void SetRocketName(string newName)
	{
		this.rocketName = newName;
		this.UpdateNameOnRocketModules();
	}

	// Token: 0x06005952 RID: 22866 RVA: 0x00206D42 File Offset: 0x00204F42
	public string GetRocketName()
	{
		return this.rocketName;
	}

	// Token: 0x06005953 RID: 22867 RVA: 0x00206D4C File Offset: 0x00204F4C
	public void UpdateNameOnRocketModules()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchConditions.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null)
			{
				component.SetParentRocketName(this.rocketName);
			}
		}
	}

	// Token: 0x06005954 RID: 22868 RVA: 0x00206DBC File Offset: 0x00204FBC
	public bool HasInvalidID()
	{
		return this.id == -1;
	}

	// Token: 0x06005955 RID: 22869 RVA: 0x00206DC7 File Offset: 0x00204FC7
	public void SetID(int id)
	{
		this.id = id;
	}

	// Token: 0x06005956 RID: 22870 RVA: 0x00206DD0 File Offset: 0x00204FD0
	public void SetState(Spacecraft.MissionState state)
	{
		this.state = state;
	}

	// Token: 0x06005957 RID: 22871 RVA: 0x00206DD9 File Offset: 0x00204FD9
	public void BeginMission(SpaceDestination destination)
	{
		this.missionElapsed = 0f;
		this.missionDuration = (float)destination.OneBasedDistance * ROCKETRY.MISSION_DURATION_SCALE / this.GetPilotNavigationEfficiency();
		this.SetState(Spacecraft.MissionState.Launching);
	}

	// Token: 0x06005958 RID: 22872 RVA: 0x00206E08 File Offset: 0x00205008
	private float GetPilotNavigationEfficiency()
	{
		float num = 1f;
		if (!this.launchConditions.GetComponent<CommandModule>().robotPilotControlled)
		{
			List<MinionStorage.Info> storedMinionInfo = this.launchConditions.GetComponent<MinionStorage>().GetStoredMinionInfo();
			if (storedMinionInfo.Count < 1)
			{
				return 1f;
			}
			StoredMinionIdentity component = storedMinionInfo[0].serializedMinion.Get().GetComponent<StoredMinionIdentity>();
			string b = Db.Get().Attributes.SpaceNavigation.Id;
			foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
			{
				foreach (SkillPerk skillPerk in Db.Get().Skills.Get(keyValuePair.Key).perks)
				{
					if (Game.IsCorrectDlcActiveForCurrentSave(skillPerk))
					{
						SkillAttributePerk skillAttributePerk = skillPerk as SkillAttributePerk;
						if (skillAttributePerk != null && skillAttributePerk.modifier.AttributeId == b)
						{
							num += skillAttributePerk.modifier.Value;
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06005959 RID: 22873 RVA: 0x00206F4C File Offset: 0x0020514C
	public void ForceComplete()
	{
		this.missionElapsed = this.missionDuration;
	}

	// Token: 0x0600595A RID: 22874 RVA: 0x00206F5C File Offset: 0x0020515C
	public void ProgressMission(float deltaTime)
	{
		if (this.state == Spacecraft.MissionState.Underway)
		{
			this.missionElapsed += deltaTime;
			if (this.controlStationBuffTimeRemaining > 0f)
			{
				this.missionElapsed += deltaTime * 0.20000005f;
				this.controlStationBuffTimeRemaining -= deltaTime;
			}
			else
			{
				this.controlStationBuffTimeRemaining = 0f;
			}
			if (this.missionElapsed > this.missionDuration)
			{
				this.CompleteMission();
			}
		}
	}

	// Token: 0x0600595B RID: 22875 RVA: 0x00206FD0 File Offset: 0x002051D0
	public float GetTimeLeft()
	{
		return this.missionDuration - this.missionElapsed;
	}

	// Token: 0x0600595C RID: 22876 RVA: 0x00206FDF File Offset: 0x002051DF
	public float GetDuration()
	{
		return this.missionDuration;
	}

	// Token: 0x0600595D RID: 22877 RVA: 0x00206FE7 File Offset: 0x002051E7
	public void CompleteMission()
	{
		SpacecraftManager.instance.PushReadyToLandNotification(this);
		this.SetState(Spacecraft.MissionState.WaitingToLand);
		this.Land();
	}

	// Token: 0x0600595E RID: 22878 RVA: 0x00207004 File Offset: 0x00205204
	private void Land()
	{
		this.launchConditions.Trigger(-1165815793, SpacecraftManager.instance.GetSpacecraftDestination(this.id));
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchConditions.GetComponent<AttachableBuilding>()))
		{
			if (gameObject != this.launchConditions.gameObject)
			{
				gameObject.Trigger(-1165815793, SpacecraftManager.instance.GetSpacecraftDestination(this.id));
			}
		}
	}

	// Token: 0x0600595F RID: 22879 RVA: 0x002070A8 File Offset: 0x002052A8
	public void TemporallyTear()
	{
		SpacecraftManager.instance.hasVisitedWormHole = true;
		LaunchConditionManager launchConditions = this.launchConditions;
		for (int i = launchConditions.rocketModules.Count - 1; i >= 0; i--)
		{
			Storage component = launchConditions.rocketModules[i].GetComponent<Storage>();
			if (component != null)
			{
				component.ConsumeAllIgnoringDisease();
			}
			MinionStorage component2 = launchConditions.rocketModules[i].GetComponent<MinionStorage>();
			if (component2 != null)
			{
				List<MinionStorage.Info> storedMinionInfo = component2.GetStoredMinionInfo();
				for (int j = storedMinionInfo.Count - 1; j >= 0; j--)
				{
					component2.DeleteStoredMinion(storedMinionInfo[j].id);
				}
			}
			Util.KDestroyGameObject(launchConditions.rocketModules[i].gameObject);
		}
	}

	// Token: 0x06005960 RID: 22880 RVA: 0x0020716B File Offset: 0x0020536B
	public void GenerateName()
	{
		this.SetRocketName(GameUtil.GenerateRandomRocketName());
	}

	// Token: 0x04003BF1 RID: 15345
	[Serialize]
	public int id = -1;

	// Token: 0x04003BF2 RID: 15346
	[Serialize]
	public string rocketName = UI.STARMAP.DEFAULT_NAME;

	// Token: 0x04003BF3 RID: 15347
	[Serialize]
	public float controlStationBuffTimeRemaining;

	// Token: 0x04003BF4 RID: 15348
	[Serialize]
	public Ref<LaunchConditionManager> refLaunchConditions = new Ref<LaunchConditionManager>();

	// Token: 0x04003BF5 RID: 15349
	[Serialize]
	public Spacecraft.MissionState state;

	// Token: 0x04003BF6 RID: 15350
	[Serialize]
	private float missionElapsed;

	// Token: 0x04003BF7 RID: 15351
	[Serialize]
	private float missionDuration;

	// Token: 0x02001D3C RID: 7484
	public enum MissionState
	{
		// Token: 0x04008AB9 RID: 35513
		Grounded,
		// Token: 0x04008ABA RID: 35514
		Launching,
		// Token: 0x04008ABB RID: 35515
		Underway,
		// Token: 0x04008ABC RID: 35516
		WaitingToLand,
		// Token: 0x04008ABD RID: 35517
		Landing,
		// Token: 0x04008ABE RID: 35518
		Destroyed
	}
}
