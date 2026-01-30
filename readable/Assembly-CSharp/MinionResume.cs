using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B1B RID: 2843
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MinionResume")]
public class MinionResume : IExperienceRecipient, ISaveLoadable, ISim200ms
{
	// Token: 0x170005C5 RID: 1477
	// (get) Token: 0x060052C8 RID: 21192 RVA: 0x001E207A File Offset: 0x001E027A
	public MinionIdentity GetIdentity
	{
		get
		{
			return this.identity;
		}
	}

	// Token: 0x170005C6 RID: 1478
	// (get) Token: 0x060052C9 RID: 21193 RVA: 0x001E2082 File Offset: 0x001E0282
	public float TotalExperienceGained
	{
		get
		{
			return this.totalExperienceGained;
		}
	}

	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x060052CA RID: 21194 RVA: 0x001E208A File Offset: 0x001E028A
	public int TotalSkillPointsGained
	{
		get
		{
			return MinionResume.CalculateTotalSkillPointsGained(this.TotalExperienceGained);
		}
	}

	// Token: 0x060052CB RID: 21195 RVA: 0x001E2097 File Offset: 0x001E0297
	public static int CalculateTotalSkillPointsGained(float experience)
	{
		return Mathf.FloorToInt(Mathf.Pow(experience / (float)SKILLS.TARGET_SKILLS_CYCLE / 600f, 1f / SKILLS.EXPERIENCE_LEVEL_POWER) * (float)SKILLS.TARGET_SKILLS_EARNED);
	}

	// Token: 0x170005C8 RID: 1480
	// (get) Token: 0x060052CC RID: 21196 RVA: 0x001E20C4 File Offset: 0x001E02C4
	public int SkillsMastered
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
			{
				if (keyValuePair.Value)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x170005C9 RID: 1481
	// (get) Token: 0x060052CD RID: 21197 RVA: 0x001E2120 File Offset: 0x001E0320
	public int AvailableSkillpoints
	{
		get
		{
			return this.TotalSkillPointsGained - this.SkillsMastered + ((this.GrantedSkillIDs == null) ? 0 : this.GrantedSkillIDs.Count);
		}
	}

	// Token: 0x060052CE RID: 21198 RVA: 0x001E2148 File Offset: 0x001E0348
	[OnDeserialized]
	private void OnDeserializedMethod()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 7))
		{
			foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryByRoleID)
			{
				if (keyValuePair.Value && keyValuePair.Key != "NoRole")
				{
					this.ForceAddSkillPoint();
				}
			}
			foreach (KeyValuePair<HashedString, float> keyValuePair2 in this.AptitudeByRoleGroup)
			{
				this.AptitudeBySkillGroup[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}
		if (this.TotalSkillPointsGained > 1000 || this.TotalSkillPointsGained < 0)
		{
			this.ForceSetSkillPoints(100);
		}
	}

	// Token: 0x060052CF RID: 21199 RVA: 0x001E2244 File Offset: 0x001E0444
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.MinionResumes.Add(this);
	}

	// Token: 0x060052D0 RID: 21200 RVA: 0x001E2258 File Offset: 0x001E0458
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.GrantedSkillIDs.RemoveAll((string x) => Db.Get().Skills.TryGet(x) == null);
		List<string> list = new List<string>();
		foreach (string text in this.MasteryBySkillID.Keys)
		{
			if (Db.Get().Skills.TryGet(text) == null)
			{
				list.Add(text);
			}
		}
		foreach (string key in list)
		{
			this.MasteryBySkillID.Remove(key);
		}
		if (this.GrantedSkillIDs == null)
		{
			this.GrantedSkillIDs = new List<string>();
		}
		List<string> list2 = new List<string>();
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).deprecated)
			{
				list2.Add(keyValuePair.Key);
			}
		}
		foreach (string skillId in list2)
		{
			this.UnmasterSkill(skillId);
		}
		foreach (KeyValuePair<string, bool> keyValuePair2 in this.MasteryBySkillID)
		{
			if (keyValuePair2.Value)
			{
				Skill skill = Db.Get().Skills.Get(keyValuePair2.Key);
				foreach (SkillPerk skillPerk in skill.perks)
				{
					if (Game.IsCorrectDlcActiveForCurrentSave(skillPerk))
					{
						if (skillPerk.OnRemove != null)
						{
							skillPerk.OnRemove(this);
						}
						if (skillPerk.OnApply != null)
						{
							skillPerk.OnApply(this);
						}
					}
				}
				if (!this.ownedHats.ContainsKey(skill.hat))
				{
					this.ownedHats.Add(skill.hat, true);
				}
			}
		}
		this.UpdateExpectations();
		this.UpdateMorale();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		MinionResume.ApplyHat(this.currentHat, component);
		this.ShowNewSkillPointNotification();
	}

	// Token: 0x060052D1 RID: 21201 RVA: 0x001E2534 File Offset: 0x001E0734
	public void RestoreResume(Dictionary<string, bool> MasteryBySkillID, Dictionary<HashedString, float> AptitudeBySkillGroup, List<string> GrantedSkillIDs, float totalExperienceGained)
	{
		this.MasteryBySkillID = MasteryBySkillID;
		this.GrantedSkillIDs = ((GrantedSkillIDs != null) ? GrantedSkillIDs : new List<string>());
		this.AptitudeBySkillGroup = AptitudeBySkillGroup;
		this.totalExperienceGained = totalExperienceGained;
	}

	// Token: 0x060052D2 RID: 21202 RVA: 0x001E255D File Offset: 0x001E075D
	protected override void OnCleanUp()
	{
		Components.MinionResumes.Remove(this);
		if (this.lastSkillNotification != null)
		{
			Game.Instance.GetComponent<Notifier>().Remove(this.lastSkillNotification);
			this.lastSkillNotification = null;
		}
		base.OnCleanUp();
	}

	// Token: 0x060052D3 RID: 21203 RVA: 0x001E2594 File Offset: 0x001E0794
	public bool HasMasteredSkill(string skillId)
	{
		return this.MasteryBySkillID.ContainsKey(skillId) && this.MasteryBySkillID[skillId];
	}

	// Token: 0x060052D4 RID: 21204 RVA: 0x001E25B4 File Offset: 0x001E07B4
	public void UpdateUrge()
	{
		if (this.targetHat != this.currentHat)
		{
			if (!base.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
			{
				base.gameObject.GetComponent<ChoreConsumer>().AddUrge(Db.Get().Urges.LearnSkill);
				return;
			}
		}
		else
		{
			base.gameObject.GetComponent<ChoreConsumer>().RemoveUrge(Db.Get().Urges.LearnSkill);
		}
	}

	// Token: 0x170005CA RID: 1482
	// (get) Token: 0x060052D5 RID: 21205 RVA: 0x001E2634 File Offset: 0x001E0834
	public string CurrentRole
	{
		get
		{
			return this.currentRole;
		}
	}

	// Token: 0x170005CB RID: 1483
	// (get) Token: 0x060052D6 RID: 21206 RVA: 0x001E263C File Offset: 0x001E083C
	public string CurrentHat
	{
		get
		{
			return this.currentHat;
		}
	}

	// Token: 0x170005CC RID: 1484
	// (get) Token: 0x060052D7 RID: 21207 RVA: 0x001E2644 File Offset: 0x001E0844
	public string TargetHat
	{
		get
		{
			return this.targetHat;
		}
	}

	// Token: 0x060052D8 RID: 21208 RVA: 0x001E264C File Offset: 0x001E084C
	public void SetHats(string current, string target)
	{
		this.currentHat = current;
		this.targetHat = target;
	}

	// Token: 0x060052D9 RID: 21209 RVA: 0x001E265C File Offset: 0x001E085C
	public void ClearAdditionalHats()
	{
		this.AdditionalHats.Clear();
	}

	// Token: 0x060052DA RID: 21210 RVA: 0x001E266C File Offset: 0x001E086C
	public void AddAdditionalHat(string context, string hat)
	{
		MinionResume.HatInfo hatInfo = null;
		foreach (MinionResume.HatInfo hatInfo2 in this.AdditionalHats)
		{
			if (hatInfo2.Source == context && hatInfo2.Hat == hat)
			{
				hatInfo = hatInfo2;
				break;
			}
		}
		if (hatInfo != null)
		{
			hatInfo.count++;
			return;
		}
		this.AdditionalHats.Add(new MinionResume.HatInfo(context, hat));
	}

	// Token: 0x060052DB RID: 21211 RVA: 0x001E2700 File Offset: 0x001E0900
	public void RemoveAdditionalHat(string context, string hat)
	{
		MinionResume.HatInfo hatInfo = null;
		foreach (MinionResume.HatInfo hatInfo2 in this.AdditionalHats)
		{
			if (hatInfo2.Source == context && hatInfo2.Hat == hat)
			{
				hatInfo2.count--;
				hatInfo = hatInfo2;
				break;
			}
		}
		if (hatInfo != null && hatInfo.count <= 0)
		{
			this.AdditionalHats.Remove(hatInfo);
			if (this.currentHat == hat)
			{
				this.RemoveHat();
			}
		}
	}

	// Token: 0x060052DC RID: 21212 RVA: 0x001E27AC File Offset: 0x001E09AC
	public void SetCurrentRole(string role_id)
	{
		this.currentRole = role_id;
	}

	// Token: 0x170005CD RID: 1485
	// (get) Token: 0x060052DD RID: 21213 RVA: 0x001E27B5 File Offset: 0x001E09B5
	public string TargetRole
	{
		get
		{
			return this.targetRole;
		}
	}

	// Token: 0x060052DE RID: 21214 RVA: 0x001E27C0 File Offset: 0x001E09C0
	public void ApplyAdditionalSkillPerks(SkillPerk[] perks)
	{
		foreach (SkillPerk skillPerk in perks)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(skillPerk))
			{
				this.AdditionalGrantedSkillPerkIDs.Add(skillPerk.IdHash);
				if (skillPerk.OnApply != null)
				{
					skillPerk.OnApply(this);
				}
			}
		}
		Game.Instance.Trigger(-1523247426, null);
	}

	// Token: 0x060052DF RID: 21215 RVA: 0x001E2820 File Offset: 0x001E0A20
	public void RemoveAdditionalSkillPerks(SkillPerk[] perks)
	{
		foreach (SkillPerk skillPerk in perks)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(skillPerk))
			{
				this.AdditionalGrantedSkillPerkIDs.Remove(skillPerk.IdHash);
				if (skillPerk.OnRemove != null)
				{
					skillPerk.OnRemove(this);
				}
			}
		}
	}

	// Token: 0x060052E0 RID: 21216 RVA: 0x001E2870 File Offset: 0x001E0A70
	private void ApplySkillPerksForSkill(string skillId)
	{
		foreach (SkillPerk skillPerk in Db.Get().Skills.Get(skillId).perks)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(skillPerk) && skillPerk.OnApply != null)
			{
				skillPerk.OnApply(this);
			}
		}
	}

	// Token: 0x060052E1 RID: 21217 RVA: 0x001E28E8 File Offset: 0x001E0AE8
	private void RemoveSkillPerksForSkill(string skillId)
	{
		foreach (SkillPerk skillPerk in Db.Get().Skills.Get(skillId).perks)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(skillPerk) && skillPerk.OnRemove != null)
			{
				skillPerk.OnRemove(this);
			}
		}
	}

	// Token: 0x060052E2 RID: 21218 RVA: 0x001E2960 File Offset: 0x001E0B60
	public void Sim200ms(float dt)
	{
		this.DEBUG_SecondsAlive += dt;
		if (!base.GetComponent<KPrefabID>().HasTag(GameTags.Dead))
		{
			this.DEBUG_PassiveExperienceGained += dt * SKILLS.PASSIVE_EXPERIENCE_PORTION;
			this.AddExperience(dt * SKILLS.PASSIVE_EXPERIENCE_PORTION);
		}
	}

	// Token: 0x060052E3 RID: 21219 RVA: 0x001E29B0 File Offset: 0x001E0BB0
	public bool IsAbleToLearnSkill(string skillId)
	{
		Skill skill = Db.Get().Skills.Get(skillId);
		string choreGroupID = Db.Get().SkillGroups.Get(skill.skillGroup).choreGroupID;
		if (!string.IsNullOrEmpty(choreGroupID))
		{
			Traits component = base.GetComponent<Traits>();
			if (component != null && component.IsChoreGroupDisabled(choreGroupID))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060052E4 RID: 21220 RVA: 0x001E2A14 File Offset: 0x001E0C14
	public bool BelowMoraleExpectation(Skill skill)
	{
		float num = Db.Get().Attributes.QualityOfLife.Lookup(this).GetTotalValue();
		float totalValue = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(this).GetTotalValue();
		int moraleExpectation = skill.GetMoraleExpectation();
		if (this.AptitudeBySkillGroup.ContainsKey(skill.skillGroup) && this.AptitudeBySkillGroup[skill.skillGroup] > 0f)
		{
			num += 1f;
		}
		return totalValue + (float)moraleExpectation <= num;
	}

	// Token: 0x060052E5 RID: 21221 RVA: 0x001E2AA4 File Offset: 0x001E0CA4
	public bool HasMasteredDirectlyRequiredSkillsForSkill(Skill skill)
	{
		for (int i = 0; i < skill.priorSkills.Count; i++)
		{
			if (!this.HasMasteredSkill(skill.priorSkills[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060052E6 RID: 21222 RVA: 0x001E2ADE File Offset: 0x001E0CDE
	public bool HasSkillPointsRequiredForSkill(Skill skill)
	{
		return this.AvailableSkillpoints >= 1;
	}

	// Token: 0x060052E7 RID: 21223 RVA: 0x001E2AEC File Offset: 0x001E0CEC
	public bool HasSkillAptitude(Skill skill)
	{
		return this.AptitudeBySkillGroup.ContainsKey(skill.skillGroup) && this.AptitudeBySkillGroup[skill.skillGroup] > 0f;
	}

	// Token: 0x060052E8 RID: 21224 RVA: 0x001E2B26 File Offset: 0x001E0D26
	public bool HasBeenGrantedSkill(Skill skill)
	{
		return this.GrantedSkillIDs != null && this.GrantedSkillIDs.Contains(skill.Id);
	}

	// Token: 0x060052E9 RID: 21225 RVA: 0x001E2B48 File Offset: 0x001E0D48
	public bool HasBeenGrantedSkill(string id)
	{
		return this.GrantedSkillIDs != null && this.GrantedSkillIDs.Contains(id);
	}

	// Token: 0x060052EA RID: 21226 RVA: 0x001E2B68 File Offset: 0x001E0D68
	public MinionResume.SkillMasteryConditions[] GetSkillMasteryConditions(string skillId)
	{
		List<MinionResume.SkillMasteryConditions> list = new List<MinionResume.SkillMasteryConditions>();
		Skill skill = Db.Get().Skills.Get(skillId);
		if (this.HasSkillAptitude(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.SkillAptitude);
		}
		if (!this.BelowMoraleExpectation(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.StressWarning);
		}
		if (!this.IsAbleToLearnSkill(skillId))
		{
			list.Add(MinionResume.SkillMasteryConditions.UnableToLearn);
		}
		if (!this.HasSkillPointsRequiredForSkill(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.NeedsSkillPoints);
		}
		if (!this.HasMasteredDirectlyRequiredSkillsForSkill(skill))
		{
			list.Add(MinionResume.SkillMasteryConditions.MissingPreviousSkill);
		}
		return list.ToArray();
	}

	// Token: 0x060052EB RID: 21227 RVA: 0x001E2BE2 File Offset: 0x001E0DE2
	public bool CanMasterSkill(MinionResume.SkillMasteryConditions[] masteryConditions)
	{
		return !Array.Exists<MinionResume.SkillMasteryConditions>(masteryConditions, (MinionResume.SkillMasteryConditions element) => element == MinionResume.SkillMasteryConditions.UnableToLearn || element == MinionResume.SkillMasteryConditions.NeedsSkillPoints || element == MinionResume.SkillMasteryConditions.MissingPreviousSkill);
	}

	// Token: 0x060052EC RID: 21228 RVA: 0x001E2C10 File Offset: 0x001E0E10
	public bool OwnsHat(string hatId)
	{
		using (List<MinionResume.HatInfo>.Enumerator enumerator = this.AdditionalHats.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Hat == hatId)
				{
					return true;
				}
			}
		}
		return this.ownedHats.ContainsKey(hatId) && this.ownedHats[hatId];
	}

	// Token: 0x060052ED RID: 21229 RVA: 0x001E2C8C File Offset: 0x001E0E8C
	public List<MinionResume.HatInfo> GetAllHats()
	{
		List<MinionResume.HatInfo> list = new List<MinionResume.HatInfo>();
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value)
			{
				Skill skill = Db.Get().Skills.TryGet(keyValuePair.Key);
				if (!skill.hat.IsNullOrWhiteSpace())
				{
					list.Add(new MinionResume.HatInfo(skill.Name, skill.hat));
				}
			}
		}
		list.AddRange(this.AdditionalHats);
		return list;
	}

	// Token: 0x060052EE RID: 21230 RVA: 0x001E2D30 File Offset: 0x001E0F30
	public void SkillLearned()
	{
		if (base.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
		{
			base.gameObject.GetComponent<ChoreConsumer>().RemoveUrge(Db.Get().Urges.LearnSkill);
		}
		foreach (string key in this.ownedHats.Keys.ToList<string>())
		{
			this.ownedHats[key] = true;
		}
		if (this.targetHat != null && this.currentHat != this.targetHat)
		{
			this.CreateHatChangeChore();
		}
	}

	// Token: 0x060052EF RID: 21231 RVA: 0x001E2DF4 File Offset: 0x001E0FF4
	public void CreateHatChangeChore()
	{
		if (this.lastHatChore != null)
		{
			this.lastHatChore.Cancel("New Hat");
		}
		this.lastHatChore = new PutOnHatChore(this, Db.Get().ChoreTypes.SwitchHat);
	}

	// Token: 0x060052F0 RID: 21232 RVA: 0x001E2E2C File Offset: 0x001E102C
	public void MasterSkill(string skillId)
	{
		if (!base.gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
		{
			base.gameObject.GetComponent<ChoreConsumer>().AddUrge(Db.Get().Urges.LearnSkill);
		}
		this.MasteryBySkillID[skillId] = true;
		this.ApplySkillPerksForSkill(skillId);
		this.UpdateExpectations();
		this.UpdateMorale();
		this.TriggerMasterSkillEvents();
		GameScheduler.Instance.Schedule("Morale Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Morale, true);
		}, null, null);
		if (!this.ownedHats.ContainsKey(Db.Get().Skills.Get(skillId).hat))
		{
			this.ownedHats.Add(Db.Get().Skills.Get(skillId).hat, false);
		}
		if (this.AvailableSkillpoints == 0 && this.lastSkillNotification != null)
		{
			Game.Instance.GetComponent<Notifier>().Remove(this.lastSkillNotification);
			this.lastSkillNotification = null;
		}
	}

	// Token: 0x060052F1 RID: 21233 RVA: 0x001E2F44 File Offset: 0x001E1144
	public void UnmasterSkill(string skillId)
	{
		if (this.MasteryBySkillID.ContainsKey(skillId))
		{
			this.MasteryBySkillID.Remove(skillId);
			this.RemoveSkillPerksForSkill(skillId);
			this.UpdateExpectations();
			this.UpdateMorale();
			this.TriggerMasterSkillEvents();
		}
	}

	// Token: 0x060052F2 RID: 21234 RVA: 0x001E2F7C File Offset: 0x001E117C
	public void GrantSkill(string skillId)
	{
		if (this.GrantedSkillIDs == null)
		{
			this.GrantedSkillIDs = new List<string>();
		}
		if (!this.HasBeenGrantedSkill(skillId))
		{
			this.MasteryBySkillID[skillId] = true;
			this.ApplySkillPerksForSkill(skillId);
			this.GrantedSkillIDs.Add(skillId);
			this.UpdateExpectations();
			this.UpdateMorale();
			this.TriggerMasterSkillEvents();
			if (!this.ownedHats.ContainsKey(Db.Get().Skills.Get(skillId).hat))
			{
				this.ownedHats.Add(Db.Get().Skills.Get(skillId).hat, false);
			}
		}
	}

	// Token: 0x060052F3 RID: 21235 RVA: 0x001E301C File Offset: 0x001E121C
	public void UngrantSkill(string skillId)
	{
		if (this.GrantedSkillIDs != null)
		{
			this.GrantedSkillIDs.RemoveAll((string match) => match == skillId);
		}
		this.UnmasterSkill(skillId);
	}

	// Token: 0x060052F4 RID: 21236 RVA: 0x001E3064 File Offset: 0x001E1264
	public Sprite GetSkillGrantSourceIcon(string skillID)
	{
		if (!this.GrantedSkillIDs.Contains(skillID))
		{
			return null;
		}
		BionicUpgradesMonitor.Instance smi = base.gameObject.GetSMI<BionicUpgradesMonitor.Instance>();
		if (smi != null)
		{
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in smi.upgradeComponentSlots)
			{
				if (upgradeComponentSlot.HasUpgradeInstalled)
				{
					return Def.GetUISprite(upgradeComponentSlot.installedUpgradeComponent.gameObject, "ui", false).first;
				}
			}
		}
		return Assets.GetSprite("skill_granted_trait");
	}

	// Token: 0x060052F5 RID: 21237 RVA: 0x001E30DC File Offset: 0x001E12DC
	private void TriggerMasterSkillEvents()
	{
		base.Trigger(540773776, null);
		Game.Instance.Trigger(-1523247426, this);
	}

	// Token: 0x060052F6 RID: 21238 RVA: 0x001E30FA File Offset: 0x001E12FA
	public void ForceSetSkillPoints(int points)
	{
		this.totalExperienceGained = MinionResume.CalculatePreviousExperienceBar(points);
	}

	// Token: 0x060052F7 RID: 21239 RVA: 0x001E3108 File Offset: 0x001E1308
	public void ForceAddSkillPoint()
	{
		this.AddExperience(MinionResume.CalculateNextExperienceBar(this.TotalSkillPointsGained) - this.totalExperienceGained);
	}

	// Token: 0x060052F8 RID: 21240 RVA: 0x001E3122 File Offset: 0x001E1322
	public static float CalculateNextExperienceBar(int current_skill_points)
	{
		return Mathf.Pow((float)(current_skill_points + 1) / (float)SKILLS.TARGET_SKILLS_EARNED, SKILLS.EXPERIENCE_LEVEL_POWER) * (float)SKILLS.TARGET_SKILLS_CYCLE * 600f;
	}

	// Token: 0x060052F9 RID: 21241 RVA: 0x001E3146 File Offset: 0x001E1346
	public static float CalculatePreviousExperienceBar(int current_skill_points)
	{
		return Mathf.Pow((float)current_skill_points / (float)SKILLS.TARGET_SKILLS_EARNED, SKILLS.EXPERIENCE_LEVEL_POWER) * (float)SKILLS.TARGET_SKILLS_CYCLE * 600f;
	}

	// Token: 0x060052FA RID: 21242 RVA: 0x001E3168 File Offset: 0x001E1368
	private void UpdateExpectations()
	{
		int num = 0;
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && !this.HasBeenGrantedSkill(keyValuePair.Key))
			{
				Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
				num += skill.tier + 1;
			}
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(this);
		if (this.skillsMoraleExpectationModifier != null)
		{
			attributeInstance.Remove(this.skillsMoraleExpectationModifier);
			this.skillsMoraleExpectationModifier = null;
		}
		if (num > 0)
		{
			this.skillsMoraleExpectationModifier = new AttributeModifier(attributeInstance.Id, (float)num, DUPLICANTS.NEEDS.QUALITYOFLIFE.EXPECTATION_MOD_NAME, false, false, true);
			attributeInstance.Add(this.skillsMoraleExpectationModifier);
		}
	}

	// Token: 0x060052FB RID: 21243 RVA: 0x001E3254 File Offset: 0x001E1454
	private void UpdateMorale()
	{
		int num = 0;
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && !this.HasBeenGrantedSkill(keyValuePair.Key))
			{
				Skill skill = Db.Get().Skills.Get(keyValuePair.Key);
				float num2 = 0f;
				if (this.AptitudeBySkillGroup.TryGetValue(new HashedString(skill.skillGroup), out num2))
				{
					num += (int)num2;
				}
			}
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(this);
		if (this.skillsMoraleModifier != null)
		{
			attributeInstance.Remove(this.skillsMoraleModifier);
			this.skillsMoraleModifier = null;
		}
		if (num > 0)
		{
			this.skillsMoraleModifier = new AttributeModifier(attributeInstance.Id, (float)num, DUPLICANTS.NEEDS.QUALITYOFLIFE.APTITUDE_SKILLS_MOD_NAME, false, false, true);
			attributeInstance.Add(this.skillsMoraleModifier);
		}
	}

	// Token: 0x060052FC RID: 21244 RVA: 0x001E335C File Offset: 0x001E155C
	private void OnSkillPointGained()
	{
		Game.Instance.Trigger(1505456302, this);
		this.ShowNewSkillPointNotification();
		if (PopFXManager.Instance != null)
		{
			string text = MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.identity.GetProperName());
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, text, base.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
		}
		new UpgradeFX.Instance(base.gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, -0.1f)).StartSM();
	}

	// Token: 0x060052FD RID: 21245 RVA: 0x001E3408 File Offset: 0x001E1608
	private void ShowNewSkillPointNotification()
	{
		if (this.AvailableSkillpoints == 1)
		{
			this.lastSkillNotification = new ManagementMenuNotification(global::Action.ManageSkills, NotificationValence.Good, this.identity.GetSoleOwner().gameObject.GetInstanceID().ToString(), MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.identity.GetProperName()), NotificationType.Good, new Func<List<Notification>, object, string>(this.GetSkillPointGainedTooltip), this.identity, true, 0f, delegate(object d)
			{
				ManagementMenu.Instance.OpenSkills(this.identity);
			}, null, null, true);
			base.GetComponent<Notifier>().Add(this.lastSkillNotification, "");
		}
	}

	// Token: 0x060052FE RID: 21246 RVA: 0x001E34A4 File Offset: 0x001E16A4
	private string GetSkillPointGainedTooltip(List<Notification> notifications, object data)
	{
		return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP.Replace("{Duplicant}", ((MinionIdentity)data).GetProperName());
	}

	// Token: 0x060052FF RID: 21247 RVA: 0x001E34C0 File Offset: 0x001E16C0
	public void SetAptitude(HashedString skillGroupID, float amount)
	{
		this.AptitudeBySkillGroup[skillGroupID] = amount;
	}

	// Token: 0x06005300 RID: 21248 RVA: 0x001E34D0 File Offset: 0x001E16D0
	public float GetAptitudeExperienceMultiplier(HashedString skillGroupId, float buildingFrequencyMultiplier)
	{
		float num = 0f;
		this.AptitudeBySkillGroup.TryGetValue(skillGroupId, out num);
		return 1f + num * SKILLS.APTITUDE_EXPERIENCE_MULTIPLIER * buildingFrequencyMultiplier;
	}

	// Token: 0x06005301 RID: 21249 RVA: 0x001E3504 File Offset: 0x001E1704
	public void AddExperience(float amount)
	{
		float num = this.totalExperienceGained;
		float num2 = MinionResume.CalculateNextExperienceBar(this.TotalSkillPointsGained);
		this.totalExperienceGained += amount;
		if (base.isSpawned && this.totalExperienceGained >= num2 && num < num2)
		{
			this.OnSkillPointGained();
		}
	}

	// Token: 0x06005302 RID: 21250 RVA: 0x001E3550 File Offset: 0x001E1750
	public override void AddExperienceWithAptitude(string skillGroupId, float amount, float buildingMultiplier)
	{
		float num = amount * this.GetAptitudeExperienceMultiplier(skillGroupId, buildingMultiplier) * SKILLS.ACTIVE_EXPERIENCE_PORTION;
		this.DEBUG_ActiveExperienceGained += num;
		this.AddExperience(num);
	}

	// Token: 0x06005303 RID: 21251 RVA: 0x001E3588 File Offset: 0x001E1788
	public bool HasPerk(HashedString perkId)
	{
		using (List<HashedString>.Enumerator enumerator = this.AdditionalGrantedSkillPerkIDs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == perkId)
				{
					return true;
				}
			}
		}
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).GivesPerk(perkId))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005304 RID: 21252 RVA: 0x001E3648 File Offset: 0x001E1848
	public bool HasPerk(SkillPerk perk)
	{
		using (List<HashedString>.Enumerator enumerator = this.AdditionalGrantedSkillPerkIDs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == perk.IdHash)
				{
					return true;
				}
			}
		}
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).GivesPerk(perk))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005305 RID: 21253 RVA: 0x001E3710 File Offset: 0x001E1910
	public void RemoveHat()
	{
		MinionResume.RemoveHat(base.GetComponent<KBatchedAnimController>());
		this.currentHat = null;
		this.targetHat = null;
	}

	// Token: 0x06005306 RID: 21254 RVA: 0x001E372C File Offset: 0x001E192C
	public static void RemoveHat(KBatchedAnimController controller)
	{
		AccessorySlot hat = Db.Get().AccessorySlots.Hat;
		Accessorizer component = controller.GetComponent<Accessorizer>();
		if (component != null)
		{
			Accessory accessory = component.GetAccessory(hat);
			if (accessory != null)
			{
				component.RemoveAccessory(accessory);
			}
		}
		else
		{
			controller.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride(hat.targetSymbolId, 4);
		}
		controller.SetSymbolVisiblity(hat.targetSymbolId, false);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
	}

	// Token: 0x06005307 RID: 21255 RVA: 0x001E37C8 File Offset: 0x001E19C8
	public static void AddHat(string hat_id, KBatchedAnimController controller)
	{
		AccessorySlot hat = Db.Get().AccessorySlots.Hat;
		Accessory accessory = hat.Lookup(hat_id);
		if (accessory == null)
		{
			global::Debug.LogWarning("Missing hat: " + hat_id);
		}
		Accessorizer component = controller.GetComponent<Accessorizer>();
		if (component != null)
		{
			Accessory accessory2 = component.GetAccessory(Db.Get().AccessorySlots.Hat);
			if (accessory2 != null)
			{
				component.RemoveAccessory(accessory2);
			}
			if (accessory != null)
			{
				component.AddAccessory(accessory);
			}
		}
		else
		{
			SymbolOverrideController component2 = controller.GetComponent<SymbolOverrideController>();
			component2.TryRemoveSymbolOverride(hat.targetSymbolId, 4);
			component2.AddSymbolOverride(hat.targetSymbolId, accessory.symbol, 4);
		}
		controller.SetSymbolVisiblity(hat.targetSymbolId, true);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
		controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
	}

	// Token: 0x06005308 RID: 21256 RVA: 0x001E38B0 File Offset: 0x001E1AB0
	public void ApplyTargetHat()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		MinionResume.ApplyHat(this.targetHat, component);
		this.currentHat = this.targetHat;
		this.targetHat = null;
	}

	// Token: 0x06005309 RID: 21257 RVA: 0x001E38E3 File Offset: 0x001E1AE3
	public static void ApplyHat(string hat_id, KBatchedAnimController controller)
	{
		if (hat_id.IsNullOrWhiteSpace())
		{
			MinionResume.RemoveHat(controller);
			return;
		}
		MinionResume.AddHat(hat_id, controller);
	}

	// Token: 0x0600530A RID: 21258 RVA: 0x001E38FB File Offset: 0x001E1AFB
	public string GetSkillsSubtitle()
	{
		return string.Format(DUPLICANTS.NEEDS.QUALITYOFLIFE.TOTAL_SKILL_POINTS, this.TotalSkillPointsGained);
	}

	// Token: 0x0600530B RID: 21259 RVA: 0x001E3918 File Offset: 0x001E1B18
	public static bool AnyMinionHasPerk(string perk, int worldId = -1)
	{
		using (List<MinionResume>.Enumerator enumerator = (from minion in (worldId >= 0) ? Components.MinionResumes.GetWorldItems(worldId, true) : Components.MinionResumes.Items
		where !minion.HasTag(GameTags.Dead)
		select minion).ToList<MinionResume>().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.HasPerk(perk))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600530C RID: 21260 RVA: 0x001E39B8 File Offset: 0x001E1BB8
	public static bool AnyOtherMinionHasPerk(string perk, MinionResume me)
	{
		foreach (MinionResume minionResume in Components.MinionResumes.Items)
		{
			if (!(minionResume == me) && minionResume.HasPerk(perk))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600530D RID: 21261 RVA: 0x001E3A28 File Offset: 0x001E1C28
	public void ResetSkillLevels(bool returnSkillPoints = true)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, bool> keyValuePair in this.MasteryBySkillID)
		{
			if (keyValuePair.Value)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (string skillId in list)
		{
			this.UnmasterSkill(skillId);
		}
	}

	// Token: 0x040037F3 RID: 14323
	[MyCmpReq]
	private MinionIdentity identity;

	// Token: 0x040037F4 RID: 14324
	[Serialize]
	public Dictionary<string, bool> MasteryByRoleID = new Dictionary<string, bool>();

	// Token: 0x040037F5 RID: 14325
	[Serialize]
	public Dictionary<string, bool> MasteryBySkillID = new Dictionary<string, bool>();

	// Token: 0x040037F6 RID: 14326
	[Serialize]
	public List<string> GrantedSkillIDs = new List<string>();

	// Token: 0x040037F7 RID: 14327
	private List<HashedString> AdditionalGrantedSkillPerkIDs = new List<HashedString>();

	// Token: 0x040037F8 RID: 14328
	private List<MinionResume.HatInfo> AdditionalHats = new List<MinionResume.HatInfo>();

	// Token: 0x040037F9 RID: 14329
	[Serialize]
	public Dictionary<HashedString, float> AptitudeByRoleGroup = new Dictionary<HashedString, float>();

	// Token: 0x040037FA RID: 14330
	[Serialize]
	public Dictionary<HashedString, float> AptitudeBySkillGroup = new Dictionary<HashedString, float>();

	// Token: 0x040037FB RID: 14331
	[Serialize]
	private string currentRole = "NoRole";

	// Token: 0x040037FC RID: 14332
	[Serialize]
	private string targetRole = "NoRole";

	// Token: 0x040037FD RID: 14333
	[Serialize]
	private string currentHat;

	// Token: 0x040037FE RID: 14334
	[Serialize]
	private string targetHat;

	// Token: 0x040037FF RID: 14335
	private Dictionary<string, bool> ownedHats = new Dictionary<string, bool>();

	// Token: 0x04003800 RID: 14336
	[Serialize]
	private float totalExperienceGained;

	// Token: 0x04003801 RID: 14337
	private Notification lastSkillNotification;

	// Token: 0x04003802 RID: 14338
	private PutOnHatChore lastHatChore;

	// Token: 0x04003803 RID: 14339
	private AttributeModifier skillsMoraleExpectationModifier;

	// Token: 0x04003804 RID: 14340
	private AttributeModifier skillsMoraleModifier;

	// Token: 0x04003805 RID: 14341
	public float DEBUG_PassiveExperienceGained;

	// Token: 0x04003806 RID: 14342
	public float DEBUG_ActiveExperienceGained;

	// Token: 0x04003807 RID: 14343
	public float DEBUG_SecondsAlive;

	// Token: 0x02001C64 RID: 7268
	public class HatInfo
	{
		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x0600AD4B RID: 44363 RVA: 0x003CF1D8 File Offset: 0x003CD3D8
		public string Source { get; }

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x0600AD4C RID: 44364 RVA: 0x003CF1E0 File Offset: 0x003CD3E0
		public string Hat { get; }

		// Token: 0x0600AD4D RID: 44365 RVA: 0x003CF1E8 File Offset: 0x003CD3E8
		public HatInfo(string source, string hat)
		{
			this.Source = source;
			this.Hat = hat;
			this.count = 1;
		}

		// Token: 0x040087DA RID: 34778
		public int count;
	}

	// Token: 0x02001C65 RID: 7269
	public enum SkillMasteryConditions
	{
		// Token: 0x040087DC RID: 34780
		SkillAptitude,
		// Token: 0x040087DD RID: 34781
		StressWarning,
		// Token: 0x040087DE RID: 34782
		UnableToLearn,
		// Token: 0x040087DF RID: 34783
		NeedsSkillPoints,
		// Token: 0x040087E0 RID: 34784
		MissingPreviousSkill
	}
}
