using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000715 RID: 1813
[AddComponentMenu("KMonoBehaviour/scripts/BuildingConfigManager")]
public class BuildingConfigManager : KMonoBehaviour
{
	// Token: 0x06002D36 RID: 11574 RVA: 0x00106048 File Offset: 0x00104248
	protected override void OnPrefabInit()
	{
		BuildingConfigManager.Instance = this;
		this.baseTemplate = new GameObject("BuildingTemplate");
		this.baseTemplate.SetActive(false);
		this.baseTemplate.AddComponent<KPrefabID>();
		this.baseTemplate.AddComponent<KSelectable>();
		this.baseTemplate.AddComponent<Modifiers>();
		this.baseTemplate.AddComponent<PrimaryElement>();
		this.baseTemplate.AddComponent<BuildingComplete>();
		this.baseTemplate.AddComponent<StateMachineController>();
		this.baseTemplate.AddComponent<Deconstructable>();
		this.baseTemplate.AddComponent<Reconstructable>();
		this.baseTemplate.AddComponent<SaveLoadRoot>();
		this.baseTemplate.AddComponent<OccupyArea>();
		this.baseTemplate.AddComponent<DecorProvider>();
		this.baseTemplate.AddComponent<Operational>();
		this.baseTemplate.AddComponent<BuildingEnabledButton>();
		this.baseTemplate.AddComponent<Prioritizable>();
		this.baseTemplate.AddComponent<BuildingHP>();
		this.baseTemplate.AddComponent<LoopingSounds>();
		this.baseTemplate.AddComponent<InvalidPortReporter>();
		this.defaultBuildingCompleteKComponents.Add(typeof(RequiresFoundation));
	}

	// Token: 0x06002D37 RID: 11575 RVA: 0x00106159 File Offset: 0x00104359
	public static string GetUnderConstructionName(string name)
	{
		return name + "UnderConstruction";
	}

	// Token: 0x06002D38 RID: 11576 RVA: 0x00106168 File Offset: 0x00104368
	public void RegisterBuilding(IBuildingConfig config)
	{
		string[] requiredDlcIds = config.GetRequiredDlcIds();
		string[] forbiddenDlcIds = config.GetForbiddenDlcIds();
		if (config.GetDlcIds() != null)
		{
			DlcManager.ConvertAvailableToRequireAndForbidden(config.GetDlcIds(), out requiredDlcIds, out forbiddenDlcIds);
		}
		if (!DlcManager.IsCorrectDlcSubscribed(config))
		{
			return;
		}
		BuildingDef buildingDef = config.CreateBuildingDef();
		buildingDef.RequiredDlcIds = requiredDlcIds;
		buildingDef.ForbiddenDlcIds = forbiddenDlcIds;
		this.configTable[config] = buildingDef;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.baseTemplate);
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.PrefabTag = buildingDef.Tag;
		component.SetDlcRestrictions(buildingDef);
		gameObject.name = buildingDef.PrefabID + "Template";
		gameObject.GetComponent<Building>().Def = buildingDef;
		gameObject.GetComponent<OccupyArea>().SetCellOffsets(buildingDef.PlacementOffsets);
		gameObject.AddTag(GameTags.RoomProberBuilding);
		if (buildingDef.Deprecated)
		{
			gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		}
		config.ConfigureBuildingTemplate(gameObject, buildingDef.Tag);
		buildingDef.BuildingComplete = BuildingLoader.Instance.CreateBuildingComplete(gameObject, buildingDef);
		bool flag = true;
		for (int i = 0; i < this.NonBuildableBuildings.Length; i++)
		{
			if (buildingDef.PrefabID == this.NonBuildableBuildings[i])
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			buildingDef.BuildingUnderConstruction = BuildingLoader.Instance.CreateBuildingUnderConstruction(buildingDef);
			buildingDef.BuildingUnderConstruction.name = BuildingConfigManager.GetUnderConstructionName(buildingDef.BuildingUnderConstruction.name);
			buildingDef.BuildingPreview = BuildingLoader.Instance.CreateBuildingPreview(buildingDef);
			GameObject buildingPreview = buildingDef.BuildingPreview;
			buildingPreview.name += "Preview";
		}
		buildingDef.PostProcess();
		config.DoPostConfigureComplete(buildingDef.BuildingComplete);
		if (flag)
		{
			config.DoPostConfigurePreview(buildingDef, buildingDef.BuildingPreview);
			config.DoPostConfigureUnderConstruction(buildingDef.BuildingUnderConstruction);
		}
		Assets.AddBuildingDef(buildingDef);
	}

	// Token: 0x06002D39 RID: 11577 RVA: 0x0010632C File Offset: 0x0010452C
	public void ConfigurePost()
	{
		foreach (KeyValuePair<IBuildingConfig, BuildingDef> keyValuePair in this.configTable)
		{
			keyValuePair.Key.ConfigurePost(keyValuePair.Value);
		}
	}

	// Token: 0x06002D3A RID: 11578 RVA: 0x0010638C File Offset: 0x0010458C
	public void IgnoreDefaultKComponent(Type type_to_ignore, Tag building_tag)
	{
		HashSet<Tag> hashSet;
		if (!this.ignoredDefaultKComponents.TryGetValue(type_to_ignore, out hashSet))
		{
			hashSet = new HashSet<Tag>();
			this.ignoredDefaultKComponents[type_to_ignore] = hashSet;
		}
		hashSet.Add(building_tag);
	}

	// Token: 0x06002D3B RID: 11579 RVA: 0x001063C4 File Offset: 0x001045C4
	private bool IsIgnoredDefaultKComponent(Tag building_tag, Type type)
	{
		bool result = false;
		HashSet<Tag> hashSet;
		if (this.ignoredDefaultKComponents.TryGetValue(type, out hashSet) && hashSet.Contains(building_tag))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06002D3C RID: 11580 RVA: 0x001063F0 File Offset: 0x001045F0
	public void AddBuildingCompleteKComponents(GameObject go, Tag prefab_tag)
	{
		foreach (Type type in this.defaultBuildingCompleteKComponents)
		{
			if (!this.IsIgnoredDefaultKComponent(prefab_tag, type))
			{
				GameComps.GetKComponentManager(type).Add(go);
			}
		}
		HashSet<Type> hashSet;
		if (this.buildingCompleteKComponents.TryGetValue(prefab_tag, out hashSet))
		{
			foreach (Type kcomponent_type in hashSet)
			{
				GameComps.GetKComponentManager(kcomponent_type).Add(go);
			}
		}
	}

	// Token: 0x06002D3D RID: 11581 RVA: 0x001064A4 File Offset: 0x001046A4
	public void DestroyBuildingCompleteKComponents(GameObject go, Tag prefab_tag)
	{
		foreach (Type type in this.defaultBuildingCompleteKComponents)
		{
			if (!this.IsIgnoredDefaultKComponent(prefab_tag, type))
			{
				GameComps.GetKComponentManager(type).Remove(go);
			}
		}
		HashSet<Type> hashSet;
		if (this.buildingCompleteKComponents.TryGetValue(prefab_tag, out hashSet))
		{
			foreach (Type kcomponent_type in hashSet)
			{
				GameComps.GetKComponentManager(kcomponent_type).Remove(go);
			}
		}
	}

	// Token: 0x06002D3E RID: 11582 RVA: 0x00106558 File Offset: 0x00104758
	public void AddDefaultBuildingCompleteKComponent(Type kcomponent_type)
	{
		this.defaultKComponents.Add(kcomponent_type);
	}

	// Token: 0x06002D3F RID: 11583 RVA: 0x00106568 File Offset: 0x00104768
	public void AddBuildingCompleteKComponent(Tag prefab_tag, Type kcomponent_type)
	{
		HashSet<Type> hashSet;
		if (!this.buildingCompleteKComponents.TryGetValue(prefab_tag, out hashSet))
		{
			hashSet = new HashSet<Type>();
			this.buildingCompleteKComponents[prefab_tag] = hashSet;
		}
		hashSet.Add(kcomponent_type);
	}

	// Token: 0x04001AE1 RID: 6881
	public static BuildingConfigManager Instance;

	// Token: 0x04001AE2 RID: 6882
	private GameObject baseTemplate;

	// Token: 0x04001AE3 RID: 6883
	private Dictionary<IBuildingConfig, BuildingDef> configTable = new Dictionary<IBuildingConfig, BuildingDef>();

	// Token: 0x04001AE4 RID: 6884
	private string[] NonBuildableBuildings = new string[]
	{
		"Headquarters"
	};

	// Token: 0x04001AE5 RID: 6885
	private HashSet<Type> defaultKComponents = new HashSet<Type>();

	// Token: 0x04001AE6 RID: 6886
	private HashSet<Type> defaultBuildingCompleteKComponents = new HashSet<Type>();

	// Token: 0x04001AE7 RID: 6887
	private Dictionary<Type, HashSet<Tag>> ignoredDefaultKComponents = new Dictionary<Type, HashSet<Tag>>();

	// Token: 0x04001AE8 RID: 6888
	private Dictionary<Tag, HashSet<Type>> buildingCompleteKComponents = new Dictionary<Tag, HashSet<Type>>();
}
