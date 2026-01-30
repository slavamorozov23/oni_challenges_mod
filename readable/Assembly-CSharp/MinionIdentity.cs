using System;
using System.Collections.Generic;
using Klei.AI;
using Klei.CustomSettings;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000607 RID: 1543
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MinionIdentity")]
public class MinionIdentity : KMonoBehaviour, ISaveLoadable, IAssignableIdentity, IListableOption, ISim1000ms
{
	// Token: 0x17000178 RID: 376
	// (get) Token: 0x060023E4 RID: 9188 RVA: 0x000CF874 File Offset: 0x000CDA74
	// (set) Token: 0x060023E5 RID: 9189 RVA: 0x000CF87C File Offset: 0x000CDA7C
	[Serialize]
	public string genderStringKey { get; set; }

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x060023E6 RID: 9190 RVA: 0x000CF885 File Offset: 0x000CDA85
	// (set) Token: 0x060023E7 RID: 9191 RVA: 0x000CF88D File Offset: 0x000CDA8D
	[Serialize]
	public string nameStringKey { get; set; }

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x060023E8 RID: 9192 RVA: 0x000CF896 File Offset: 0x000CDA96
	// (set) Token: 0x060023E9 RID: 9193 RVA: 0x000CF89E File Offset: 0x000CDA9E
	[Serialize]
	public HashedString personalityResourceId { get; set; }

	// Token: 0x060023EA RID: 9194 RVA: 0x000CF8A7 File Offset: 0x000CDAA7
	public static void DestroyStatics()
	{
		MinionIdentity.maleNameList = null;
		MinionIdentity.femaleNameList = null;
	}

	// Token: 0x060023EB RID: 9195 RVA: 0x000CF8B8 File Offset: 0x000CDAB8
	protected override void OnPrefabInit()
	{
		if (this.name == null)
		{
			this.name = MinionIdentity.ChooseRandomName();
		}
		if (GameClock.Instance != null)
		{
			this.arrivalTime = (float)GameClock.Instance.GetCycle();
		}
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		if (component != null)
		{
			KAnimControllerBase kanimControllerBase = component;
			kanimControllerBase.OnUpdateBounds = (Action<Bounds>)Delegate.Combine(kanimControllerBase.OnUpdateBounds, new Action<Bounds>(this.OnUpdateBounds));
		}
		GameUtil.SubscribeToTags<MinionIdentity>(this, MinionIdentity.OnDeadTagAddedDelegate, true);
		base.Subscribe<MinionIdentity>(1502190696, MinionIdentity.OnQueueDestroyObjectDelegate);
	}

	// Token: 0x060023EC RID: 9196 RVA: 0x000CF948 File Offset: 0x000CDB48
	protected override void OnSpawn()
	{
		if (this.addToIdentityList)
		{
			this.ValidateProxy();
			this.CleanupLimboMinions();
		}
		Navigator component = base.GetComponent<Navigator>();
		if (component != null)
		{
			component.reportOccupation = true;
		}
		this.SetName(this.name);
		if (this.nameStringKey == null)
		{
			this.nameStringKey = this.name;
		}
		this.SetGender(this.gender);
		if (this.genderStringKey == null)
		{
			this.genderStringKey = "NB";
		}
		if (this.personalityResourceId == HashedString.Invalid)
		{
			Personality personalityFromNameStringKey = Db.Get().Personalities.GetPersonalityFromNameStringKey(this.nameStringKey);
			if (personalityFromNameStringKey != null)
			{
				this.personalityResourceId = personalityFromNameStringKey.Id;
			}
		}
		if (!this.model.IsValid)
		{
			Personality personality = Db.Get().Personalities.Get(this.personalityResourceId);
			if (personality != null)
			{
				this.model = personality.model;
			}
		}
		if (this.addToIdentityList)
		{
			Components.MinionIdentities.Add(this);
			if (!Components.MinionIdentitiesByModel.ContainsKey(this.model))
			{
				Components.MinionIdentitiesByModel[this.model] = new Components.Cmps<MinionIdentity>();
			}
			Components.MinionIdentitiesByModel[this.model].Add(this);
			if (!base.gameObject.HasTag(GameTags.Dead))
			{
				Components.LiveMinionIdentities.Add(this);
				if (!Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
				{
					Components.LiveMinionIdentitiesByModel[this.model] = new Components.Cmps<MinionIdentity>();
				}
				Components.LiveMinionIdentitiesByModel[this.model].Add(this);
				Game.Instance.Trigger(2144209314, this);
			}
		}
		SymbolOverrideController component2 = base.GetComponent<SymbolOverrideController>();
		if (component2 != null)
		{
			Accessorizer component3 = base.gameObject.GetComponent<Accessorizer>();
			if (component3 != null)
			{
				string str = HashCache.Get().Get(component3.GetAccessory(Db.Get().AccessorySlots.Mouth).symbol.hash).Replace("mouth", "cheek");
				component2.AddSymbolOverride("snapto_cheek", Assets.GetAnim("head_swap_kanim").GetData().build.GetSymbol(str), 1);
				component2.AddSymbolOverride("snapto_hair_always", component3.GetAccessory(Db.Get().AccessorySlots.Hair).symbol, 1);
				component2.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component3.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			}
		}
		this.voiceId = (this.voiceIdx + 1).ToString("D2");
		Prioritizable component4 = base.GetComponent<Prioritizable>();
		if (component4 != null)
		{
			component4.showIcon = false;
		}
		Pickupable component5 = base.GetComponent<Pickupable>();
		if (component5 != null)
		{
			component5.carryAnimOverride = Assets.GetAnim("anim_incapacitated_carrier_kanim");
		}
		this.ApplyCustomGameSettings();
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x000CFC88 File Offset: 0x000CDE88
	public void ValidateProxy()
	{
		this.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(this.assignableProxy, this);
	}

	// Token: 0x060023EE RID: 9198 RVA: 0x000CFC9C File Offset: 0x000CDE9C
	private void CleanupLimboMinions()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (component.InstanceID == -1)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with an invalid kpid! Attempting to recover...",
				this.name
			});
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		if (component.conflicted)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Minion with a conflicted kpid! Attempting to recover... ",
				component.InstanceID,
				this.name
			});
			if (KPrefabIDTracker.Get().GetInstance(component.InstanceID) != null)
			{
				KPrefabIDTracker.Get().Unregister(component);
			}
			component.InstanceID = KPrefabID.GetUniqueID();
			KPrefabIDTracker.Get().Register(component);
			DebugUtil.LogWarningArgs(new object[]
			{
				"Restored as:",
				component.InstanceID
			});
		}
		this.assignableProxy.Get().SetTarget(this, base.gameObject);
	}

	// Token: 0x060023EF RID: 9199 RVA: 0x000CFDD5 File Offset: 0x000CDFD5
	public string GetProperName()
	{
		return this.selectable.GetProperName();
	}

	// Token: 0x060023F0 RID: 9200 RVA: 0x000CFDE2 File Offset: 0x000CDFE2
	public string GetVoiceId()
	{
		return this.voiceId;
	}

	// Token: 0x060023F1 RID: 9201 RVA: 0x000CFDEA File Offset: 0x000CDFEA
	public void SetName(string name)
	{
		this.name = name;
		if (this.selectable != null)
		{
			this.selectable.SetName(name);
		}
		base.gameObject.name = name;
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
	}

	// Token: 0x060023F2 RID: 9202 RVA: 0x000CFE29 File Offset: 0x000CE029
	public void SetStickerType(string stickerType)
	{
		this.stickerType = stickerType;
	}

	// Token: 0x060023F3 RID: 9203 RVA: 0x000CFE32 File Offset: 0x000CE032
	public bool IsNull()
	{
		return this == null;
	}

	// Token: 0x060023F4 RID: 9204 RVA: 0x000CFE3B File Offset: 0x000CE03B
	public void SetGender(string gender)
	{
		this.gender = gender;
		this.selectable.SetGender(gender);
	}

	// Token: 0x060023F5 RID: 9205 RVA: 0x000CFE50 File Offset: 0x000CE050
	public static string ChooseRandomName()
	{
		if (MinionIdentity.femaleNameList == null)
		{
			MinionIdentity.maleNameList = new MinionIdentity.NameList(Game.Instance.maleNamesFile);
			MinionIdentity.femaleNameList = new MinionIdentity.NameList(Game.Instance.femaleNamesFile);
		}
		if (UnityEngine.Random.value > 0.5f)
		{
			return MinionIdentity.maleNameList.Next();
		}
		return MinionIdentity.femaleNameList.Next();
	}

	// Token: 0x060023F6 RID: 9206 RVA: 0x000CFEAD File Offset: 0x000CE0AD
	private void OnQueueDestroyObject()
	{
		this.RemoveFromComponentsLists();
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x000CFEB8 File Offset: 0x000CE0B8
	private void RemoveFromComponentsLists()
	{
		Components.MinionIdentities.Remove(this);
		if (Components.MinionIdentitiesByModel.ContainsKey(this.model))
		{
			Components.MinionIdentitiesByModel[this.model].Remove(this);
		}
		Components.LiveMinionIdentities.Remove(this);
		if (Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
		{
			Components.LiveMinionIdentitiesByModel[this.model].Remove(this);
		}
	}

	// Token: 0x060023F8 RID: 9208 RVA: 0x000CFF2C File Offset: 0x000CE12C
	protected override void OnCleanUp()
	{
		if (this.assignableProxy != null)
		{
			MinionAssignablesProxy minionAssignablesProxy = this.assignableProxy.Get();
			if (minionAssignablesProxy && minionAssignablesProxy.target == this)
			{
				Util.KDestroyGameObject(minionAssignablesProxy.gameObject);
			}
		}
		this.RemoveFromComponentsLists();
		Game.Instance.Trigger(2144209314, this);
	}

	// Token: 0x060023F9 RID: 9209 RVA: 0x000CFF7F File Offset: 0x000CE17F
	private void OnUpdateBounds(Bounds bounds)
	{
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		component.offset = bounds.center;
		component.size = bounds.extents;
	}

	// Token: 0x060023FA RID: 9210 RVA: 0x000CFFAC File Offset: 0x000CE1AC
	private void OnDied(object data)
	{
		this.GetSoleOwner().UnassignAll();
		this.GetEquipment().UnequipAll();
		Components.LiveMinionIdentities.Remove(this);
		if (Components.LiveMinionIdentitiesByModel.ContainsKey(this.model))
		{
			Components.LiveMinionIdentitiesByModel[this.model].Remove(this);
		}
		Game.Instance.Trigger(-1523247426, this);
		Game.Instance.Trigger(2144209314, this);
	}

	// Token: 0x060023FB RID: 9211 RVA: 0x000D0022 File Offset: 0x000CE222
	public List<Ownables> GetOwners()
	{
		return this.assignableProxy.Get().ownables;
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x000D0034 File Offset: 0x000CE234
	public Ownables GetSoleOwner()
	{
		return this.assignableProxy.Get().GetComponent<Ownables>();
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x000D0046 File Offset: 0x000CE246
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Contains(owner as Ownables);
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x000D0059 File Offset: 0x000CE259
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x000D0066 File Offset: 0x000CE266
	public Equipment GetEquipment()
	{
		return this.assignableProxy.Get().GetComponent<Equipment>();
	}

	// Token: 0x06002400 RID: 9216 RVA: 0x000D0078 File Offset: 0x000CE278
	public void Sim1000ms(float dt)
	{
		if (this == null)
		{
			return;
		}
		if (this.navigator == null)
		{
			this.navigator = base.GetComponent<Navigator>();
		}
		if (this.navigator != null && !this.navigator.IsMoving())
		{
			return;
		}
		if (this.choreDriver == null)
		{
			this.choreDriver = base.GetComponent<ChoreDriver>();
		}
		if (this.choreDriver != null)
		{
			Chore currentChore = this.choreDriver.GetCurrentChore();
			if (currentChore != null && currentChore is FetchAreaChore)
			{
				MinionResume component = base.GetComponent<MinionResume>();
				if (component != null)
				{
					component.AddExperienceWithAptitude(Db.Get().SkillGroups.Hauling.Id, dt, SKILLS.ALL_DAY_EXPERIENCE);
				}
			}
		}
	}

	// Token: 0x06002401 RID: 9217 RVA: 0x000D0134 File Offset: 0x000CE334
	private void ApplyCustomGameSettings()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ImmuneSystem);
		if (currentQualitySetting.id == "Compromised")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, -0.3333f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, -2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Weak")
		{
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, -1f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.WEAK.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Strong")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, 2f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting.id == "Invincible")
		{
			Db.Get().Attributes.DiseaseCureSpeed.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 100000000f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			Db.Get().Attributes.GermResistance.Lookup(this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, 200f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		SettingLevel currentQualitySetting2 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Stress);
		if (currentQualitySetting2.id == "Doomed")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.033333335f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.DOOMED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Pessimistic")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.016666668f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.PESSIMISTIC.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Optimistic")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.016666668f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.OPTIMISTIC.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		else if (currentQualitySetting2.id == "Indomitable")
		{
			Db.Get().Amounts.Stress.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, float.NegativeInfinity, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.INDOMITABLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
		SettingLevel currentQualitySetting3 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CalorieBurn);
		if (currentQualitySetting3.id == "VeryHard")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * 1f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.VERYHARD.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Hard")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * 0.5f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.HARD.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Easy")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_SECOND * -0.5f, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.EASY.ATTRIBUTE_MODIFIER_NAME, false, false, true));
			return;
		}
		if (currentQualitySetting3.id == "Disabled")
		{
			Db.Get().Amounts.Calories.deltaAttribute.Lookup(this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, float.PositiveInfinity, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.DISABLED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
		}
	}

	// Token: 0x06002402 RID: 9218 RVA: 0x000D06FC File Offset: 0x000CE8FC
	public static float GetCalorieBurnMultiplier()
	{
		float result = 1f;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CalorieBurn);
		if (currentQualitySetting.id == "VeryHard")
		{
			result = 2f;
		}
		else if (currentQualitySetting.id == "Hard")
		{
			result = 1.5f;
		}
		else if (currentQualitySetting.id == "Easy")
		{
			result = 0.5f;
		}
		else if (currentQualitySetting.id == "Disabled")
		{
			result = 0f;
		}
		return result;
	}

	// Token: 0x040014E7 RID: 5351
	public const string HairAlwaysSymbol = "snapto_hair_always";

	// Token: 0x040014E8 RID: 5352
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040014E9 RID: 5353
	[MyCmpReq]
	public Modifiers modifiers;

	// Token: 0x040014EA RID: 5354
	public int femaleVoiceCount;

	// Token: 0x040014EB RID: 5355
	public int maleVoiceCount;

	// Token: 0x040014EC RID: 5356
	[Serialize]
	public Tag model;

	// Token: 0x040014ED RID: 5357
	[Serialize]
	private new string name;

	// Token: 0x040014EE RID: 5358
	[Serialize]
	public string gender;

	// Token: 0x040014F2 RID: 5362
	[Serialize]
	public string stickerType;

	// Token: 0x040014F3 RID: 5363
	[Serialize]
	[ReadOnly]
	public float arrivalTime;

	// Token: 0x040014F4 RID: 5364
	[Serialize]
	public int voiceIdx;

	// Token: 0x040014F5 RID: 5365
	[Serialize]
	public Ref<MinionAssignablesProxy> assignableProxy;

	// Token: 0x040014F6 RID: 5366
	private Navigator navigator;

	// Token: 0x040014F7 RID: 5367
	private ChoreDriver choreDriver;

	// Token: 0x040014F8 RID: 5368
	public float timeLastSpoke;

	// Token: 0x040014F9 RID: 5369
	private string voiceId;

	// Token: 0x040014FA RID: 5370
	private KAnimHashedString overrideExpression;

	// Token: 0x040014FB RID: 5371
	private KAnimHashedString expression;

	// Token: 0x040014FC RID: 5372
	public bool addToIdentityList = true;

	// Token: 0x040014FD RID: 5373
	private static MinionIdentity.NameList maleNameList;

	// Token: 0x040014FE RID: 5374
	private static MinionIdentity.NameList femaleNameList;

	// Token: 0x040014FF RID: 5375
	private static readonly EventSystem.IntraObjectHandler<MinionIdentity> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<MinionIdentity>(GameTags.Dead, delegate(MinionIdentity component, object data)
	{
		component.OnDied(data);
	});

	// Token: 0x04001500 RID: 5376
	private static readonly EventSystem.IntraObjectHandler<MinionIdentity> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<MinionIdentity>(delegate(MinionIdentity component, object data)
	{
		component.OnQueueDestroyObject();
	});

	// Token: 0x020014DA RID: 5338
	private class NameList
	{
		// Token: 0x0600914C RID: 37196 RVA: 0x00370D58 File Offset: 0x0036EF58
		public NameList(TextAsset file)
		{
			string[] array = file.text.Replace("  ", " ").Replace("\r\n", "\n").Split('\n', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(' ', StringSplitOptions.None);
				if (array2[array2.Length - 1] != "" && array2[array2.Length - 1] != null)
				{
					this.names.Add(array2[array2.Length - 1]);
				}
			}
			this.names.Shuffle<string>();
		}

		// Token: 0x0600914D RID: 37197 RVA: 0x00370DF8 File Offset: 0x0036EFF8
		public string Next()
		{
			List<string> list = this.names;
			int num = this.idx;
			this.idx = num + 1;
			return list[num % this.names.Count];
		}

		// Token: 0x04006FCF RID: 28623
		private List<string> names = new List<string>();

		// Token: 0x04006FD0 RID: 28624
		private int idx;
	}
}
