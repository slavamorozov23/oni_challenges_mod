using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000BA3 RID: 2979
[AddComponentMenu("KMonoBehaviour/scripts/RocketModule")]
public class RocketModule : KMonoBehaviour
{
	// Token: 0x060058FA RID: 22778 RVA: 0x002048F8 File Offset: 0x00202AF8
	public ProcessCondition AddModuleCondition(ProcessCondition.ProcessConditionType conditionType, ProcessCondition condition)
	{
		if (!this.moduleConditions.ContainsKey(conditionType))
		{
			this.moduleConditions.Add(conditionType, new List<ProcessCondition>());
		}
		if (!this.moduleConditions[conditionType].Contains(condition))
		{
			this.moduleConditions[conditionType].Add(condition);
		}
		return condition;
	}

	// Token: 0x060058FB RID: 22779 RVA: 0x0020494C File Offset: 0x00202B4C
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		List<ProcessCondition> list = new List<ProcessCondition>();
		if (conditionType == ProcessCondition.ProcessConditionType.All)
		{
			using (Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>>.Enumerator enumerator = this.moduleConditions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ProcessCondition.ProcessConditionType, List<ProcessCondition>> keyValuePair = enumerator.Current;
					list.AddRange(keyValuePair.Value);
				}
				return list;
			}
		}
		if (this.moduleConditions.ContainsKey(conditionType))
		{
			list = this.moduleConditions[conditionType];
		}
		return list;
	}

	// Token: 0x060058FC RID: 22780 RVA: 0x002049CC File Offset: 0x00202BCC
	public int PopulateConditionSet(ProcessCondition.ProcessConditionType conditionType, List<ProcessCondition> conditions)
	{
		int num = 0;
		if (conditionType == ProcessCondition.ProcessConditionType.All)
		{
			using (Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>>.Enumerator enumerator = this.moduleConditions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ProcessCondition.ProcessConditionType, List<ProcessCondition>> keyValuePair = enumerator.Current;
					conditions.AddRange(keyValuePair.Value);
					num += keyValuePair.Value.Count;
				}
				return num;
			}
		}
		List<ProcessCondition> list;
		if (this.moduleConditions.TryGetValue(conditionType, out list))
		{
			conditions.AddRange(list);
			num += list.Count;
		}
		return num;
	}

	// Token: 0x060058FD RID: 22781 RVA: 0x00204A5C File Offset: 0x00202C5C
	public void SetBGKAnim(KAnimFile anim_file)
	{
		this.bgAnimFile = anim_file;
	}

	// Token: 0x060058FE RID: 22782 RVA: 0x00204A65 File Offset: 0x00202C65
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameUtil.SubscribeToTags<RocketModule>(this, RocketModule.OnRocketOnGroundTagDelegate, false);
		GameUtil.SubscribeToTags<RocketModule>(this, RocketModule.OnRocketNotOnGroundTagDelegate, false);
	}

	// Token: 0x060058FF RID: 22783 RVA: 0x00204A88 File Offset: 0x00202C88
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			this.conditionManager = this.FindLaunchConditionManager();
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.conditionManager);
			if (spacecraftFromLaunchConditionManager != null)
			{
				this.SetParentRocketName(spacecraftFromLaunchConditionManager.GetRocketName());
			}
			this.RegisterWithConditionManager();
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.RocketName, this);
		}
		this.FixSorting();
		AttachableBuilding component2 = base.GetComponent<AttachableBuilding>();
		component2.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(component2.onAttachmentNetworkChanged, new Action<object>(this.OnAttachmentNetworkChanged));
		if (this.bgAnimFile != null)
		{
			this.AddBGGantry();
		}
	}

	// Token: 0x06005900 RID: 22784 RVA: 0x00204B3C File Offset: 0x00202D3C
	public void FixSorting()
	{
		int num = 0;
		AttachableBuilding component = base.GetComponent<AttachableBuilding>();
		while (component != null)
		{
			BuildingAttachPoint attachedTo = component.GetAttachedTo();
			if (!(attachedTo != null))
			{
				break;
			}
			component = attachedTo.GetComponent<AttachableBuilding>();
			num++;
		}
		Vector3 localPosition = base.transform.GetLocalPosition();
		localPosition.z = Grid.GetLayerZ(Grid.SceneLayer.Building) - (float)num * 0.01f;
		base.transform.SetLocalPosition(localPosition);
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		if (component2.enabled)
		{
			component2.enabled = false;
			component2.enabled = true;
		}
	}

	// Token: 0x06005901 RID: 22785 RVA: 0x00204BC8 File Offset: 0x00202DC8
	private void OnAttachmentNetworkChanged(object ab)
	{
		this.FixSorting();
	}

	// Token: 0x06005902 RID: 22786 RVA: 0x00204BD0 File Offset: 0x00202DD0
	private void AddBGGantry()
	{
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		GameObject gameObject = new GameObject();
		gameObject.name = string.Format(this.rocket_module_bg_base_string, base.name, this.rocket_module_bg_affix);
		gameObject.SetActive(false);
		Vector3 position = component.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.InteriorWall);
		gameObject.transform.SetPosition(position);
		gameObject.transform.parent = base.transform;
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			this.bgAnimFile
		};
		kbatchedAnimController.initialAnim = this.rocket_module_bg_anim;
		kbatchedAnimController.fgLayer = Grid.SceneLayer.NoLayer;
		kbatchedAnimController.initialMode = KAnim.PlayMode.Paused;
		kbatchedAnimController.FlipX = component.FlipX;
		kbatchedAnimController.FlipY = component.FlipY;
		gameObject.SetActive(true);
	}

	// Token: 0x06005903 RID: 22787 RVA: 0x00204C9C File Offset: 0x00202E9C
	private void OnRocketOnGroundTag(object data)
	{
		this.RegisterComponents();
		Operational component = base.GetComponent<Operational>();
		if (this.operationalLandedRequired && component != null)
		{
			component.SetFlag(RocketModule.landedFlag, true);
		}
	}

	// Token: 0x06005904 RID: 22788 RVA: 0x00204CD4 File Offset: 0x00202ED4
	private void OnRocketNotOnGroundTag(object data)
	{
		this.DeregisterComponents();
		Operational component = base.GetComponent<Operational>();
		if (this.operationalLandedRequired && component != null)
		{
			component.SetFlag(RocketModule.landedFlag, false);
		}
	}

	// Token: 0x06005905 RID: 22789 RVA: 0x00204D0C File Offset: 0x00202F0C
	public void DeregisterComponents()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		component.IsSelectable = false;
		BuildingComplete component2 = base.GetComponent<BuildingComplete>();
		if (component2 != null)
		{
			component2.UpdatePosition();
		}
		if (SelectTool.Instance.selected == component)
		{
			SelectTool.Instance.Select(null, false);
		}
		Deconstructable component3 = base.GetComponent<Deconstructable>();
		if (component3 != null)
		{
			component3.SetAllowDeconstruction(false);
		}
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		if (handle.IsValid())
		{
			GameComps.StructureTemperatures.Disable(handle);
		}
		FakeFloorAdder component4 = base.GetComponent<FakeFloorAdder>();
		if (component4 != null)
		{
			component4.SetFloor(false);
		}
		AccessControl component5 = base.GetComponent<AccessControl>();
		if (component5 != null)
		{
			component5.SetRegistered(false);
		}
		foreach (ManualDeliveryKG manualDeliveryKG in base.GetComponents<ManualDeliveryKG>())
		{
			DebugUtil.DevAssert(!manualDeliveryKG.IsPaused, "RocketModule ManualDeliver chore was already paused, when this rocket lands it will re-enable it.", null);
			manualDeliveryKG.Pause(true, "Rocket heading to space");
		}
		BuildingConduitEndpoints[] components2 = base.GetComponents<BuildingConduitEndpoints>();
		for (int i = 0; i < components2.Length; i++)
		{
			components2[i].RemoveEndPoint();
		}
		ReorderableBuilding component6 = base.GetComponent<ReorderableBuilding>();
		if (component6 != null)
		{
			component6.ShowReorderArm(false);
		}
		Workable component7 = base.GetComponent<Workable>();
		if (component7 != null)
		{
			component7.RefreshReachability();
		}
		Structure component8 = base.GetComponent<Structure>();
		if (component8 != null)
		{
			component8.UpdatePosition();
		}
		WireUtilitySemiVirtualNetworkLink component9 = base.GetComponent<WireUtilitySemiVirtualNetworkLink>();
		if (component9 != null)
		{
			component9.SetLinkConnected(false);
		}
		PartialLightBlocking component10 = base.GetComponent<PartialLightBlocking>();
		if (component10 != null)
		{
			component10.ClearLightBlocking();
		}
	}

	// Token: 0x06005906 RID: 22790 RVA: 0x00204EB0 File Offset: 0x002030B0
	public void RegisterComponents()
	{
		base.GetComponent<KSelectable>().IsSelectable = true;
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component != null)
		{
			component.UpdatePosition();
		}
		Deconstructable component2 = base.GetComponent<Deconstructable>();
		if (component2 != null)
		{
			component2.SetAllowDeconstruction(true);
		}
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		if (handle.IsValid())
		{
			GameComps.StructureTemperatures.Enable(handle);
		}
		Storage[] components = base.GetComponents<Storage>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].UpdateStoredItemCachedCells();
		}
		FakeFloorAdder component3 = base.GetComponent<FakeFloorAdder>();
		if (component3 != null)
		{
			component3.SetFloor(true);
		}
		AccessControl component4 = base.GetComponent<AccessControl>();
		if (component4 != null)
		{
			component4.SetRegistered(true);
		}
		ManualDeliveryKG[] components2 = base.GetComponents<ManualDeliveryKG>();
		for (int i = 0; i < components2.Length; i++)
		{
			components2[i].Pause(false, "Landing on world");
		}
		BuildingConduitEndpoints[] components3 = base.GetComponents<BuildingConduitEndpoints>();
		for (int i = 0; i < components3.Length; i++)
		{
			components3[i].AddEndpoint();
		}
		ReorderableBuilding component5 = base.GetComponent<ReorderableBuilding>();
		if (component5 != null)
		{
			component5.ShowReorderArm(true);
		}
		Workable component6 = base.GetComponent<Workable>();
		if (component6 != null)
		{
			component6.RefreshReachability();
		}
		Structure component7 = base.GetComponent<Structure>();
		if (component7 != null)
		{
			component7.UpdatePosition();
		}
		WireUtilitySemiVirtualNetworkLink component8 = base.GetComponent<WireUtilitySemiVirtualNetworkLink>();
		if (component8 != null)
		{
			component8.SetLinkConnected(true);
		}
		PartialLightBlocking component9 = base.GetComponent<PartialLightBlocking>();
		if (component9 != null)
		{
			component9.SetLightBlocking();
		}
	}

	// Token: 0x06005907 RID: 22791 RVA: 0x00205040 File Offset: 0x00203240
	private void ToggleComponent(Type cmpType, bool enabled)
	{
		MonoBehaviour monoBehaviour = (MonoBehaviour)base.GetComponent(cmpType);
		if (monoBehaviour != null)
		{
			monoBehaviour.enabled = enabled;
		}
	}

	// Token: 0x06005908 RID: 22792 RVA: 0x0020506A File Offset: 0x0020326A
	public void RegisterWithConditionManager()
	{
		global::Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		if (this.conditionManager != null)
		{
			this.conditionManager.RegisterRocketModule(this);
		}
	}

	// Token: 0x06005909 RID: 22793 RVA: 0x00205093 File Offset: 0x00203293
	protected override void OnCleanUp()
	{
		if (this.conditionManager != null)
		{
			this.conditionManager.UnregisterRocketModule(this);
		}
		base.OnCleanUp();
	}

	// Token: 0x0600590A RID: 22794 RVA: 0x002050B8 File Offset: 0x002032B8
	public virtual LaunchConditionManager FindLaunchConditionManager()
	{
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				LaunchConditionManager component = gameObject.GetComponent<LaunchConditionManager>();
				if (component != null)
				{
					return component;
				}
			}
		}
		return null;
	}

	// Token: 0x0600590B RID: 22795 RVA: 0x00205128 File Offset: 0x00203328
	public void SetParentRocketName(string newName)
	{
		this.parentRocketName = newName;
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
	}

	// Token: 0x0600590C RID: 22796 RVA: 0x00205141 File Offset: 0x00203341
	public virtual string GetParentRocketName()
	{
		return this.parentRocketName;
	}

	// Token: 0x0600590D RID: 22797 RVA: 0x0020514C File Offset: 0x0020334C
	public void MoveToSpace()
	{
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (component != null && component.GetMyWorld() != null)
		{
			component.GetMyWorld().RemoveTopPriorityPrioritizable(component);
		}
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Building component2 = base.GetComponent<Building>();
		component2.Def.UnmarkArea(cell, component2.Orientation, component2.Def.ObjectLayer, base.gameObject);
		Vector3 position = new Vector3(-1f, -1f, 0f);
		base.gameObject.transform.SetPosition(position);
		LogicPorts component3 = base.GetComponent<LogicPorts>();
		if (component3 != null)
		{
			component3.OnMove();
		}
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Entombed, false, this);
	}

	// Token: 0x0600590E RID: 22798 RVA: 0x0020521C File Offset: 0x0020341C
	public void MoveToPad(int newCell)
	{
		base.gameObject.transform.SetPosition(Grid.CellToPos(newCell, CellAlignment.Bottom, Grid.SceneLayer.Building));
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Building component = base.GetComponent<Building>();
		component.RefreshCells();
		component.Def.MarkArea(cell, component.Orientation, component.Def.ObjectLayer, base.gameObject);
		LogicPorts component2 = base.GetComponent<LogicPorts>();
		if (component2 != null)
		{
			component2.OnMove();
		}
		Prioritizable component3 = base.GetComponent<Prioritizable>();
		if (component3 != null && component3.IsTopPriority())
		{
			component3.GetMyWorld().AddTopPriorityPrioritizable(component3);
		}
	}

	// Token: 0x04003BC4 RID: 15300
	public LaunchConditionManager conditionManager;

	// Token: 0x04003BC5 RID: 15301
	public Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>> moduleConditions = new Dictionary<ProcessCondition.ProcessConditionType, List<ProcessCondition>>();

	// Token: 0x04003BC6 RID: 15302
	public static readonly Operational.Flag landedFlag = new Operational.Flag("landed", Operational.Flag.Type.Requirement);

	// Token: 0x04003BC7 RID: 15303
	public bool operationalLandedRequired = true;

	// Token: 0x04003BC8 RID: 15304
	private string rocket_module_bg_base_string = "{0}{1}";

	// Token: 0x04003BC9 RID: 15305
	private string rocket_module_bg_affix = "BG";

	// Token: 0x04003BCA RID: 15306
	private string rocket_module_bg_anim = "on";

	// Token: 0x04003BCB RID: 15307
	[SerializeField]
	private KAnimFile bgAnimFile;

	// Token: 0x04003BCC RID: 15308
	protected string parentRocketName = UI.STARMAP.DEFAULT_NAME;

	// Token: 0x04003BCD RID: 15309
	private static readonly EventSystem.IntraObjectHandler<RocketModule> OnRocketOnGroundTagDelegate = GameUtil.CreateHasTagHandler<RocketModule>(GameTags.RocketOnGround, delegate(RocketModule component, object data)
	{
		component.OnRocketOnGroundTag(data);
	});

	// Token: 0x04003BCE RID: 15310
	private static readonly EventSystem.IntraObjectHandler<RocketModule> OnRocketNotOnGroundTagDelegate = GameUtil.CreateHasTagHandler<RocketModule>(GameTags.RocketNotOnGround, delegate(RocketModule component, object data)
	{
		component.OnRocketNotOnGroundTag(data);
	});
}
