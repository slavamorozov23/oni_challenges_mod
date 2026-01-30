using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000AFC RID: 2812
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Research")]
public class Research : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x060051CC RID: 20940 RVA: 0x001DAF72 File Offset: 0x001D9172
	public static void DestroyInstance()
	{
		Research.Instance = null;
	}

	// Token: 0x060051CD RID: 20941 RVA: 0x001DAF7C File Offset: 0x001D917C
	public TechInstance GetTechInstance(string techID)
	{
		return this.techs.Find((TechInstance match) => match.tech.Id == techID);
	}

	// Token: 0x060051CE RID: 20942 RVA: 0x001DAFAD File Offset: 0x001D91AD
	public bool IsBeingResearched(Tech tech)
	{
		return this.activeResearch != null && tech != null && this.activeResearch.tech == tech;
	}

	// Token: 0x060051CF RID: 20943 RVA: 0x001DAFCA File Offset: 0x001D91CA
	protected override void OnPrefabInit()
	{
		Research.Instance = this;
		this.researchTypes = new ResearchTypes();
	}

	// Token: 0x060051D0 RID: 20944 RVA: 0x001DAFE0 File Offset: 0x001D91E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.globalPointInventory == null)
		{
			this.globalPointInventory = new ResearchPointInventory();
		}
		this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.OnRolesUpdated));
		this.OnRolesUpdated(null);
		Components.ResearchCenters.OnAdd += new Action<IResearchCenter>(this.CheckResearchBuildings);
		Components.ResearchCenters.OnRemove += new Action<IResearchCenter>(this.CheckResearchBuildings);
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			IResearchCenter component = kprefabID.GetComponent<IResearchCenter>();
			if (component != null)
			{
				this.researchCenterPrefabs.Add(component);
			}
		}
	}

	// Token: 0x060051D1 RID: 20945 RVA: 0x001DB0AC File Offset: 0x001D92AC
	public ResearchType GetResearchType(string id)
	{
		return this.researchTypes.GetResearchType(id);
	}

	// Token: 0x060051D2 RID: 20946 RVA: 0x001DB0BA File Offset: 0x001D92BA
	public TechInstance GetActiveResearch()
	{
		return this.activeResearch;
	}

	// Token: 0x060051D3 RID: 20947 RVA: 0x001DB0C2 File Offset: 0x001D92C2
	public TechInstance GetTargetResearch()
	{
		if (this.queuedTech != null && this.queuedTech.Count > 0)
		{
			return this.queuedTech[this.queuedTech.Count - 1];
		}
		return null;
	}

	// Token: 0x060051D4 RID: 20948 RVA: 0x001DB0F4 File Offset: 0x001D92F4
	public TechInstance Get(Tech tech)
	{
		foreach (TechInstance techInstance in this.techs)
		{
			if (techInstance.tech == tech)
			{
				return techInstance;
			}
		}
		return null;
	}

	// Token: 0x060051D5 RID: 20949 RVA: 0x001DB150 File Offset: 0x001D9350
	public TechInstance GetOrAdd(Tech tech)
	{
		TechInstance techInstance = this.techs.Find((TechInstance tc) => tc.tech == tech);
		if (techInstance != null)
		{
			return techInstance;
		}
		TechInstance techInstance2 = new TechInstance(tech);
		this.techs.Add(techInstance2);
		return techInstance2;
	}

	// Token: 0x060051D6 RID: 20950 RVA: 0x001DB1A0 File Offset: 0x001D93A0
	public void GetNextTech()
	{
		if (this.queuedTech.Count > 0)
		{
			this.queuedTech.RemoveAt(0);
		}
		if (this.queuedTech.Count > 0)
		{
			this.SetActiveResearch(this.queuedTech[this.queuedTech.Count - 1].tech, false);
			return;
		}
		this.SetActiveResearch(null, false);
	}

	// Token: 0x060051D7 RID: 20951 RVA: 0x001DB204 File Offset: 0x001D9404
	private void AddTechToQueue(Tech tech)
	{
		TechInstance orAdd = this.GetOrAdd(tech);
		if (!orAdd.IsComplete() && !this.queuedTech.Contains(orAdd))
		{
			this.queuedTech.Add(orAdd);
		}
		orAdd.tech.requiredTech.ForEach(delegate(Tech _tech)
		{
			this.AddTechToQueue(_tech);
		});
	}

	// Token: 0x060051D8 RID: 20952 RVA: 0x001DB258 File Offset: 0x001D9458
	public void CancelResearch(Tech tech, bool clickedEntry = true)
	{
		Research.<>c__DisplayClass26_0 CS$<>8__locals1 = new Research.<>c__DisplayClass26_0();
		CS$<>8__locals1.tech = tech;
		CS$<>8__locals1.ti = this.queuedTech.Find((TechInstance qt) => qt.tech == CS$<>8__locals1.tech);
		if (CS$<>8__locals1.ti == null)
		{
			return;
		}
		this.SetActiveResearch(null, false);
		int i;
		int j;
		for (i = CS$<>8__locals1.ti.tech.unlockedTech.Count - 1; i >= 0; i = j - 1)
		{
			if (this.queuedTech.Find((TechInstance qt) => qt.tech == CS$<>8__locals1.ti.tech.unlockedTech[i]) != null)
			{
				this.CancelResearch(CS$<>8__locals1.ti.tech.unlockedTech[i], false);
			}
			j = i;
		}
		this.queuedTech.Remove(CS$<>8__locals1.ti);
		if (clickedEntry)
		{
			this.NotifyResearchCenters(GameHashes.ActiveResearchChanged, this.queuedTech);
		}
	}

	// Token: 0x060051D9 RID: 20953 RVA: 0x001DB350 File Offset: 0x001D9550
	private void NotifyResearchCenters(GameHashes hash, object data)
	{
		foreach (object obj in Components.ResearchCenters)
		{
			((KMonoBehaviour)obj).Trigger(-1914338957, data);
		}
		base.Trigger((int)hash, data);
	}

	// Token: 0x060051DA RID: 20954 RVA: 0x001DB3B4 File Offset: 0x001D95B4
	public void SetActiveResearch(Tech tech, bool clearQueue = false)
	{
		if (clearQueue)
		{
			this.queuedTech.Clear();
		}
		this.activeResearch = null;
		if (tech != null)
		{
			if (this.queuedTech.Count == 0)
			{
				this.AddTechToQueue(tech);
			}
			if (this.queuedTech.Count > 0)
			{
				this.queuedTech.Sort((TechInstance x, TechInstance y) => x.tech.tier.CompareTo(y.tech.tier));
				this.activeResearch = this.queuedTech[0];
			}
		}
		else
		{
			this.queuedTech.Clear();
		}
		this.NotifyResearchCenters(GameHashes.ActiveResearchChanged, this.queuedTech);
		this.CheckBuyResearch();
		this.CheckResearchBuildings(null);
		this.UpdateResearcherRoleNotification();
	}

	// Token: 0x060051DB RID: 20955 RVA: 0x001DB468 File Offset: 0x001D9668
	private void UpdateResearcherRoleNotification()
	{
		if (this.NoResearcherRoleNotification != null)
		{
			this.notifier.Remove(this.NoResearcherRoleNotification);
			this.NoResearcherRoleNotification = null;
		}
		if (this.activeResearch != null)
		{
			Skill skill = null;
			if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("advanced") && this.activeResearch.tech.costsByResearchTypeID["advanced"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowAdvancedResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowAdvancedResearch)[0];
			}
			else if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("space") && this.activeResearch.tech.costsByResearchTypeID["space"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowInterstellarResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowInterstellarResearch)[0];
			}
			else if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("nuclear") && this.activeResearch.tech.costsByResearchTypeID["nuclear"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowNuclearResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowNuclearResearch)[0];
			}
			else if (this.activeResearch.tech.costsByResearchTypeID.ContainsKey("orbital") && this.activeResearch.tech.costsByResearchTypeID["orbital"] > 0f && !MinionResume.AnyMinionHasPerk(Db.Get().SkillPerks.AllowOrbitalResearch.Id, -1))
			{
				skill = Db.Get().Skills.GetSkillsWithPerk(Db.Get().SkillPerks.AllowOrbitalResearch)[0];
			}
			if (skill != null)
			{
				this.NoResearcherRoleNotification = new Notification(RESEARCH.MESSAGING.NO_RESEARCHER_SKILL, NotificationType.Bad, new Func<List<Notification>, object, string>(this.NoResearcherRoleTooltip), skill, false, 12f, null, null, null, true, false, false);
				this.notifier.Add(this.NoResearcherRoleNotification, "");
			}
		}
	}

	// Token: 0x060051DC RID: 20956 RVA: 0x001DB6F0 File Offset: 0x001D98F0
	private string NoResearcherRoleTooltip(List<Notification> list, object data)
	{
		Skill skill = (Skill)data;
		return RESEARCH.MESSAGING.NO_RESEARCHER_SKILL_TOOLTIP.Replace("{ResearchType}", skill.Name);
	}

	// Token: 0x060051DD RID: 20957 RVA: 0x001DB71C File Offset: 0x001D991C
	public void AddResearchPoints(string researchTypeID, float points)
	{
		if (!this.UseGlobalPointInventory && this.activeResearch == null)
		{
			global::Debug.LogWarning("No active research to add research points to. Global research inventory is disabled.");
			return;
		}
		(this.UseGlobalPointInventory ? this.globalPointInventory : this.activeResearch.progressInventory).AddResearchPoints(researchTypeID, points);
		this.CheckBuyResearch();
		this.NotifyResearchCenters(GameHashes.ResearchPointsChanged, null);
	}

	// Token: 0x060051DE RID: 20958 RVA: 0x001DB778 File Offset: 0x001D9978
	private void CheckBuyResearch()
	{
		if (this.activeResearch != null)
		{
			ResearchPointInventory researchPointInventory = this.UseGlobalPointInventory ? this.globalPointInventory : this.activeResearch.progressInventory;
			if (this.activeResearch.tech.CanAfford(researchPointInventory))
			{
				foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
				{
					researchPointInventory.RemoveResearchPoints(keyValuePair.Key, keyValuePair.Value);
				}
				this.activeResearch.Purchased();
				Game.Instance.Trigger(-107300940, this.activeResearch.tech);
				this.GetNextTech();
			}
		}
	}

	// Token: 0x060051DF RID: 20959 RVA: 0x001DB848 File Offset: 0x001D9A48
	protected override void OnCleanUp()
	{
		if (Game.Instance != null && this.skillsUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillsUpdateHandle);
		}
		Components.ResearchCenters.OnAdd -= new Action<IResearchCenter>(this.CheckResearchBuildings);
		Components.ResearchCenters.OnRemove -= new Action<IResearchCenter>(this.CheckResearchBuildings);
		base.OnCleanUp();
	}

	// Token: 0x060051E0 RID: 20960 RVA: 0x001DB8B0 File Offset: 0x001D9AB0
	public void CompleteQueue()
	{
		while (this.queuedTech.Count > 0)
		{
			foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
			{
				this.AddResearchPoints(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}

	// Token: 0x060051E1 RID: 20961 RVA: 0x001DB92C File Offset: 0x001D9B2C
	public List<TechInstance> GetResearchQueue()
	{
		return new List<TechInstance>(this.queuedTech);
	}

	// Token: 0x060051E2 RID: 20962 RVA: 0x001DB93C File Offset: 0x001D9B3C
	[OnSerializing]
	internal void OnSerializing()
	{
		this.saveData = default(Research.SaveData);
		if (this.activeResearch != null)
		{
			this.saveData.activeResearchId = this.activeResearch.tech.Id;
		}
		else
		{
			this.saveData.activeResearchId = "";
		}
		if (this.queuedTech != null && this.queuedTech.Count > 0)
		{
			this.saveData.targetResearchId = this.queuedTech[this.queuedTech.Count - 1].tech.Id;
		}
		else
		{
			this.saveData.targetResearchId = "";
		}
		this.saveData.techs = new TechInstance.SaveData[this.techs.Count];
		for (int i = 0; i < this.techs.Count; i++)
		{
			this.saveData.techs[i] = this.techs[i].Save();
		}
	}

	// Token: 0x060051E3 RID: 20963 RVA: 0x001DBA34 File Offset: 0x001D9C34
	[OnDeserialized]
	internal void OnDeserialized()
	{
		if (this.saveData.techs != null)
		{
			foreach (TechInstance.SaveData saveData in this.saveData.techs)
			{
				Tech tech = Db.Get().Techs.TryGet(saveData.techId);
				if (tech != null)
				{
					this.GetOrAdd(tech).Load(saveData);
				}
			}
		}
		foreach (TechInstance techInstance in this.techs)
		{
			if (this.saveData.targetResearchId == techInstance.tech.Id)
			{
				this.SetActiveResearch(techInstance.tech, false);
				break;
			}
		}
	}

	// Token: 0x060051E4 RID: 20964 RVA: 0x001DBB08 File Offset: 0x001D9D08
	private void OnRolesUpdated(object data)
	{
		this.UpdateResearcherRoleNotification();
	}

	// Token: 0x060051E5 RID: 20965 RVA: 0x001DBB10 File Offset: 0x001D9D10
	public string GetMissingResearchBuildingName()
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.activeResearch.tech.costsByResearchTypeID)
		{
			bool flag = true;
			if (keyValuePair.Value > 0f)
			{
				flag = false;
				using (List<IResearchCenter>.Enumerator enumerator2 = Components.ResearchCenters.Items.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.GetResearchType() == keyValuePair.Key)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (!flag)
			{
				foreach (IResearchCenter researchCenter in this.researchCenterPrefabs)
				{
					if (researchCenter.GetResearchType() == keyValuePair.Key)
					{
						return ((KMonoBehaviour)researchCenter).GetProperName();
					}
				}
				return null;
			}
		}
		return null;
	}

	// Token: 0x060051E6 RID: 20966 RVA: 0x001DBC40 File Offset: 0x001D9E40
	private void CheckResearchBuildings(object data)
	{
		if (this.activeResearch == null)
		{
			this.notifier.Remove(this.MissingResearchStation);
			return;
		}
		if (string.IsNullOrEmpty(this.GetMissingResearchBuildingName()))
		{
			this.notifier.Remove(this.MissingResearchStation);
			return;
		}
		this.notifier.Add(this.MissingResearchStation, "");
	}

	// Token: 0x04003762 RID: 14178
	public static Research Instance;

	// Token: 0x04003763 RID: 14179
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04003764 RID: 14180
	private List<TechInstance> techs = new List<TechInstance>();

	// Token: 0x04003765 RID: 14181
	private List<TechInstance> queuedTech = new List<TechInstance>();

	// Token: 0x04003766 RID: 14182
	private TechInstance activeResearch;

	// Token: 0x04003767 RID: 14183
	private Notification NoResearcherRoleNotification;

	// Token: 0x04003768 RID: 14184
	private Notification MissingResearchStation = new Notification(RESEARCH.MESSAGING.MISSING_RESEARCH_STATION, NotificationType.Bad, (List<Notification> list, object data) => RESEARCH.MESSAGING.MISSING_RESEARCH_STATION_TOOLTIP.ToString().Replace("{0}", Research.Instance.GetMissingResearchBuildingName()), null, false, 11f, null, null, null, true, false, false);

	// Token: 0x04003769 RID: 14185
	private List<IResearchCenter> researchCenterPrefabs = new List<IResearchCenter>();

	// Token: 0x0400376A RID: 14186
	protected int skillsUpdateHandle = -1;

	// Token: 0x0400376B RID: 14187
	public ResearchTypes researchTypes;

	// Token: 0x0400376C RID: 14188
	public bool UseGlobalPointInventory;

	// Token: 0x0400376D RID: 14189
	[Serialize]
	public ResearchPointInventory globalPointInventory;

	// Token: 0x0400376E RID: 14190
	[Serialize]
	private Research.SaveData saveData;

	// Token: 0x02001C34 RID: 7220
	private struct SaveData
	{
		// Token: 0x04008740 RID: 34624
		public string activeResearchId;

		// Token: 0x04008741 RID: 34625
		public string targetResearchId;

		// Token: 0x04008742 RID: 34626
		public TechInstance.SaveData[] techs;
	}
}
