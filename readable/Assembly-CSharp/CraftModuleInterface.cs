using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000B79 RID: 2937
[SerializationConfig(MemberSerialization.OptIn)]
public class CraftModuleInterface : KMonoBehaviour, ISim4000ms
{
	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x0600575A RID: 22362 RVA: 0x001FC7F0 File Offset: 0x001FA9F0
	public IList<Ref<RocketModuleCluster>> ClusterModules
	{
		get
		{
			return this.clusterModules;
		}
	}

	// Token: 0x0600575B RID: 22363 RVA: 0x001FC7F8 File Offset: 0x001FA9F8
	public LaunchPad GetPreferredLaunchPadForWorld(int world_id)
	{
		if (this.preferredLaunchPad.ContainsKey(world_id))
		{
			return this.preferredLaunchPad[world_id].Get();
		}
		return null;
	}

	// Token: 0x0600575C RID: 22364 RVA: 0x001FC81C File Offset: 0x001FAA1C
	private void SetPreferredLaunchPadForWorld(LaunchPad pad)
	{
		if (!this.preferredLaunchPad.ContainsKey(pad.GetMyWorldId()))
		{
			this.preferredLaunchPad.Add(this.CurrentPad.GetMyWorldId(), new Ref<LaunchPad>());
		}
		this.preferredLaunchPad[this.CurrentPad.GetMyWorldId()].Set(this.CurrentPad);
	}

	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x0600575D RID: 22365 RVA: 0x001FC878 File Offset: 0x001FAA78
	public LaunchPad CurrentPad
	{
		get
		{
			if (this.m_clustercraft != null && this.m_clustercraft.Status != Clustercraft.CraftStatus.InFlight && this.clusterModules.Count > 0)
			{
				if (this.bottomModule == null)
				{
					this.SetBottomModule();
				}
				global::Debug.Assert(this.bottomModule != null && this.bottomModule.Get() != null, "More than one cluster module but no bottom module found.");
				int num = Grid.CellBelow(Grid.PosToCell(this.bottomModule.Get().transform.position));
				if (Grid.IsValidCell(num))
				{
					GameObject gameObject = null;
					Grid.ObjectLayers[1].TryGetValue(num, out gameObject);
					if (gameObject != null)
					{
						return gameObject.GetComponent<LaunchPad>();
					}
				}
			}
			return null;
		}
	}

	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x0600575E RID: 22366 RVA: 0x001FC934 File Offset: 0x001FAB34
	public float Speed
	{
		get
		{
			return this.m_clustercraft.Speed;
		}
	}

	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x0600575F RID: 22367 RVA: 0x001FC944 File Offset: 0x001FAB44
	public float Range
	{
		get
		{
			float num = 0f;
			RocketEngineCluster engine = this.GetEngine();
			if (engine != null)
			{
				num = this.BurnableMassRemaining / engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
			}
			bool flag;
			RocketModuleCluster primaryPilotModule = this.GetPrimaryPilotModule(out flag);
			if (flag)
			{
				num = Mathf.Min(primaryPilotModule.GetComponent<RoboPilotModule>().GetDataBankRange(), num);
			}
			return num;
		}
	}

	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x06005760 RID: 22368 RVA: 0x001FC99E File Offset: 0x001FAB9E
	public int RangeInTiles
	{
		get
		{
			return (int)Mathf.Floor((this.Range + 0.001f) / 600f);
		}
	}

	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x06005761 RID: 22369 RVA: 0x001FC9B8 File Offset: 0x001FABB8
	public float FuelPerHex
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (engine != null)
			{
				return engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance * 600f;
			}
			return float.PositiveInfinity;
		}
	}

	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x06005762 RID: 22370 RVA: 0x001FC9F4 File Offset: 0x001FABF4
	public float BurnableMassRemaining
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (!(engine != null))
			{
				return 0f;
			}
			if (!engine.requireOxidizer)
			{
				return this.FuelRemaining;
			}
			return Mathf.Min(this.FuelRemaining, this.OxidizerPowerRemaining);
		}
	}

	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x06005763 RID: 22371 RVA: 0x001FCA38 File Offset: 0x001FAC38
	public float FuelRemaining
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (engine == null)
			{
				return 0f;
			}
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
			{
				IFuelTank component = @ref.Get().GetComponent<IFuelTank>();
				if (!component.IsNullOrDestroyed())
				{
					num += component.Storage.GetAmountAvailable(engine.fuelTag);
				}
			}
			return (float)Mathf.CeilToInt(num);
		}
	}

	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x06005764 RID: 22372 RVA: 0x001FCAD0 File Offset: 0x001FACD0
	public float OxidizerPowerRemaining
	{
		get
		{
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
			{
				OxidizerTank component = @ref.Get().GetComponent<OxidizerTank>();
				if (component != null)
				{
					num += component.TotalOxidizerPower;
				}
			}
			return (float)Mathf.CeilToInt(num);
		}
	}

	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x06005765 RID: 22373 RVA: 0x001FCB48 File Offset: 0x001FAD48
	public int MaxRange
	{
		get
		{
			float num = 0f;
			RocketEngineCluster engine = this.GetEngine();
			if (engine != null)
			{
				num = this.BurnableMassCapacity / engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
			}
			bool flag;
			RocketModuleCluster primaryPilotModule = this.GetPrimaryPilotModule(out flag);
			if (flag)
			{
				num = Mathf.Min(primaryPilotModule.GetComponent<RoboPilotModule>().GetMaxDataBankRange(), num);
			}
			return (int)Mathf.Floor((num + 0.001f) / 600f);
		}
	}

	// Token: 0x17000642 RID: 1602
	// (get) Token: 0x06005766 RID: 22374 RVA: 0x001FCBB4 File Offset: 0x001FADB4
	public float BurnableMassCapacity
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (!(engine != null))
			{
				return 0f;
			}
			if (!engine.requireOxidizer)
			{
				return this.FuelCapacity;
			}
			return Mathf.Min(this.FuelCapacity, this.OxidizerCapacity);
		}
	}

	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x06005767 RID: 22375 RVA: 0x001FCBF8 File Offset: 0x001FADF8
	public float FuelCapacity
	{
		get
		{
			if (this.GetEngine() == null)
			{
				return 0f;
			}
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
			{
				IFuelTank component = @ref.Get().GetComponent<IFuelTank>();
				if (!component.IsNullOrDestroyed())
				{
					num += component.Storage.Capacity();
				}
			}
			return (float)Mathf.CeilToInt(num);
		}
	}

	// Token: 0x17000644 RID: 1604
	// (get) Token: 0x06005768 RID: 22376 RVA: 0x001FCC88 File Offset: 0x001FAE88
	public float OxidizerCapacity
	{
		get
		{
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
			{
				OxidizerTank component = @ref.Get().GetComponent<OxidizerTank>();
				if (component != null)
				{
					num += component.UserMaxCapacity;
				}
			}
			return (float)Mathf.CeilToInt(num);
		}
	}

	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x06005769 RID: 22377 RVA: 0x001FCD00 File Offset: 0x001FAF00
	public int MaxHeight
	{
		get
		{
			RocketEngineCluster engine = this.GetEngine();
			if (engine != null)
			{
				return engine.maxHeight;
			}
			return -1;
		}
	}

	// Token: 0x17000646 RID: 1606
	// (get) Token: 0x0600576A RID: 22378 RVA: 0x001FCD25 File Offset: 0x001FAF25
	public float TotalBurden
	{
		get
		{
			return this.m_clustercraft.TotalBurden;
		}
	}

	// Token: 0x17000647 RID: 1607
	// (get) Token: 0x0600576B RID: 22379 RVA: 0x001FCD32 File Offset: 0x001FAF32
	public float EnginePower
	{
		get
		{
			return this.m_clustercraft.EnginePower;
		}
	}

	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x0600576C RID: 22380 RVA: 0x001FCD40 File Offset: 0x001FAF40
	public int RocketHeight
	{
		get
		{
			int num = 0;
			foreach (Ref<RocketModuleCluster> @ref in this.ClusterModules)
			{
				num += @ref.Get().GetComponent<Building>().Def.HeightInCells;
			}
			return num;
		}
	}

	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x0600576D RID: 22381 RVA: 0x001FCDA4 File Offset: 0x001FAFA4
	public bool HasCargoModule
	{
		get
		{
			using (IEnumerator<Ref<RocketModuleCluster>> enumerator = this.ClusterModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Get().GetComponent<CargoBayCluster>() != null)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x0600576E RID: 22382 RVA: 0x001FCDFC File Offset: 0x001FAFFC
	protected override void OnPrefabInit()
	{
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Combine(instance.OnLoad, new Action<Game.GameSaveData>(this.OnLoad));
	}

	// Token: 0x0600576F RID: 22383 RVA: 0x001FCE24 File Offset: 0x001FB024
	protected override void OnSpawn()
	{
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Remove(instance.OnLoad, new Action<Game.GameSaveData>(this.OnLoad));
		if (this.m_clustercraft.Status != Clustercraft.CraftStatus.Grounded)
		{
			this.ForceAttachmentNetwork();
		}
		this.SetBottomModule();
		base.Subscribe(-1311384361, new Action<object>(this.CompleteSelfDestruct));
	}

	// Token: 0x06005770 RID: 22384 RVA: 0x001FCE88 File Offset: 0x001FB088
	private void OnLoad(Game.GameSaveData data)
	{
		foreach (Ref<RocketModule> @ref in this.modules)
		{
			this.clusterModules.Add(new Ref<RocketModuleCluster>(@ref.Get().GetComponent<RocketModuleCluster>()));
		}
		this.modules.Clear();
		foreach (Ref<RocketModuleCluster> ref2 in this.clusterModules)
		{
			if (!(ref2.Get() == null))
			{
				ref2.Get().CraftInterface = this;
			}
		}
		bool flag = false;
		for (int i = this.clusterModules.Count - 1; i >= 0; i--)
		{
			if (this.clusterModules[i] == null || this.clusterModules[i].Get() == null)
			{
				global::Debug.LogWarning(string.Format("Rocket {0} had a null module at index {1} on load! Why????", base.name, i), this);
				this.clusterModules.RemoveAt(i);
				flag = true;
			}
		}
		this.SetBottomModule();
		if (flag && this.m_clustercraft.Status == Clustercraft.CraftStatus.Grounded)
		{
			global::Debug.LogWarning("The module stack was broken. Collapsing " + base.name + "...", this);
			this.SortModuleListByPosition();
			LaunchPad currentPad = this.CurrentPad;
			if (currentPad != null)
			{
				int num = currentPad.RocketBottomPosition;
				for (int j = 0; j < this.clusterModules.Count; j++)
				{
					RocketModuleCluster rocketModuleCluster = this.clusterModules[j].Get();
					if (num != Grid.PosToCell(rocketModuleCluster.transform.GetPosition()))
					{
						global::Debug.LogWarning(string.Format("Collapsing space under module {0}:{1}", j, rocketModuleCluster.name));
						rocketModuleCluster.transform.SetPosition(Grid.CellToPos(num, CellAlignment.Bottom, Grid.SceneLayer.Building));
					}
					num = Grid.OffsetCell(num, 0, this.clusterModules[j].Get().GetComponent<Building>().Def.HeightInCells);
				}
			}
			for (int k = 0; k < this.clusterModules.Count - 1; k++)
			{
				BuildingAttachPoint component = this.clusterModules[k].Get().GetComponent<BuildingAttachPoint>();
				if (component != null)
				{
					AttachableBuilding component2 = this.clusterModules[k + 1].Get().GetComponent<AttachableBuilding>();
					if (component.points[0].attachedBuilding != component2)
					{
						global::Debug.LogWarning("Reattaching " + component.name + " & " + component2.name);
						component.points[0].attachedBuilding = component2;
					}
				}
			}
		}
	}

	// Token: 0x06005771 RID: 22385 RVA: 0x001FD17C File Offset: 0x001FB37C
	public void AddModule(RocketModuleCluster newModule)
	{
		for (int i = 0; i < this.clusterModules.Count; i++)
		{
			if (this.clusterModules[i].Get() == newModule)
			{
				global::Debug.LogError(string.Concat(new string[]
				{
					"Adding module ",
					(newModule != null) ? newModule.ToString() : null,
					" to the same rocket (",
					this.m_clustercraft.Name,
					") twice"
				}));
			}
		}
		this.clusterModules.Add(new Ref<RocketModuleCluster>(newModule));
		newModule.CraftInterface = this;
		base.Trigger(1512695988, newModule);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			if (rocketModuleCluster != null && rocketModuleCluster != newModule)
			{
				rocketModuleCluster.Trigger(1512695988, newModule);
			}
		}
		newModule.Trigger(1512695988, newModule);
		this.SetBottomModule();
	}

	// Token: 0x06005772 RID: 22386 RVA: 0x001FD298 File Offset: 0x001FB498
	public void RemoveModule(RocketModuleCluster module)
	{
		for (int i = this.clusterModules.Count - 1; i >= 0; i--)
		{
			if (this.clusterModules[i].Get() == module)
			{
				this.clusterModules.RemoveAt(i);
				break;
			}
		}
		base.Trigger(1512695988, null);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger(1512695988, null);
		}
		this.SetBottomModule();
		if (this.clusterModules.Count == 0)
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x06005773 RID: 22387 RVA: 0x001FD35C File Offset: 0x001FB55C
	private void SortModuleListByPosition()
	{
		this.clusterModules.Sort(delegate(Ref<RocketModuleCluster> a, Ref<RocketModuleCluster> b)
		{
			if (Grid.CellToPos(Grid.PosToCell(a.Get())).y >= Grid.CellToPos(Grid.PosToCell(b.Get())).y)
			{
				return 1;
			}
			return -1;
		});
	}

	// Token: 0x06005774 RID: 22388 RVA: 0x001FD388 File Offset: 0x001FB588
	private void SetBottomModule()
	{
		if (this.clusterModules.Count > 0)
		{
			this.bottomModule = this.clusterModules[0];
			Vector3 vector = this.bottomModule.Get().transform.position;
			using (List<Ref<RocketModuleCluster>>.Enumerator enumerator = this.clusterModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ref<RocketModuleCluster> @ref = enumerator.Current;
					Vector3 position = @ref.Get().transform.position;
					if (position.y < vector.y)
					{
						this.bottomModule = @ref;
						vector = position;
					}
				}
				return;
			}
		}
		this.bottomModule = null;
	}

	// Token: 0x06005775 RID: 22389 RVA: 0x001FD43C File Offset: 0x001FB63C
	public int GetHeightOfModuleTop(GameObject module)
	{
		int num = 0;
		for (int i = 0; i < this.ClusterModules.Count; i++)
		{
			num += this.clusterModules[i].Get().GetComponent<Building>().Def.HeightInCells;
			if (this.clusterModules[i].Get().gameObject == module)
			{
				return num;
			}
		}
		global::Debug.LogError("Could not find module " + module.GetProperName() + " in CraftModuleInterface craft " + this.m_clustercraft.Name);
		return 0;
	}

	// Token: 0x06005776 RID: 22390 RVA: 0x001FD4CC File Offset: 0x001FB6CC
	public int GetModuleRelativeVerticalPosition(GameObject module)
	{
		int num = 0;
		for (int i = 0; i < this.ClusterModules.Count; i++)
		{
			if (this.clusterModules[i].Get().gameObject == module)
			{
				return num;
			}
			num += this.clusterModules[i].Get().GetComponent<Building>().Def.HeightInCells;
		}
		global::Debug.LogError("Could not find module " + module.GetProperName() + " in CraftModuleInterface craft " + this.m_clustercraft.Name);
		return 0;
	}

	// Token: 0x06005777 RID: 22391 RVA: 0x001FD55C File Offset: 0x001FB75C
	public void Sim4000ms(float dt)
	{
		int num = 0;
		foreach (ProcessCondition.ProcessConditionType conditionType in this.conditionsToCheck)
		{
			if (this.EvaluateConditionSet(conditionType) != ProcessCondition.Status.Failure)
			{
				num++;
			}
		}
		if (num != this.lastConditionTypeSucceeded)
		{
			this.lastConditionTypeSucceeded = num;
			this.TriggerEventOnCraftAndRocket(GameHashes.LaunchConditionChanged, null);
		}
	}

	// Token: 0x06005778 RID: 22392 RVA: 0x001FD5D4 File Offset: 0x001FB7D4
	public bool IsLaunchRequested()
	{
		return this.m_clustercraft.LaunchRequested;
	}

	// Token: 0x06005779 RID: 22393 RVA: 0x001FD5E1 File Offset: 0x001FB7E1
	public bool CheckPreppedForLaunch()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketFlight) > ProcessCondition.Status.Failure;
	}

	// Token: 0x0600577A RID: 22394 RVA: 0x001FD601 File Offset: 0x001FB801
	public bool CheckReadyToLaunch()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketFlight) != ProcessCondition.Status.Failure && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) > ProcessCondition.Status.Failure;
	}

	// Token: 0x0600577B RID: 22395 RVA: 0x001FD62A File Offset: 0x001FB82A
	public bool HasLaunchWarnings()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Warning || this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Warning || this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) == ProcessCondition.Status.Warning;
	}

	// Token: 0x0600577C RID: 22396 RVA: 0x001FD64C File Offset: 0x001FB84C
	public bool CheckReadyForAutomatedLaunchCommand()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Ready;
	}

	// Token: 0x0600577D RID: 22397 RVA: 0x001FD664 File Offset: 0x001FB864
	public bool CheckReadyForAutomatedLaunch()
	{
		return this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketPrep) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketStorage) == ProcessCondition.Status.Ready && this.EvaluateConditionSet(ProcessCondition.ProcessConditionType.RocketBoard) == ProcessCondition.Status.Ready;
	}

	// Token: 0x0600577E RID: 22398 RVA: 0x001FD688 File Offset: 0x001FB888
	public void TriggerEventOnCraftAndRocket(GameHashes evt, object data)
	{
		base.Trigger((int)evt, data);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger((int)evt, data);
		}
	}

	// Token: 0x0600577F RID: 22399 RVA: 0x001FD6E8 File Offset: 0x001FB8E8
	public void CancelLaunch()
	{
		this.m_clustercraft.CancelLaunch();
	}

	// Token: 0x06005780 RID: 22400 RVA: 0x001FD6F5 File Offset: 0x001FB8F5
	public void TriggerLaunch(bool automated = false)
	{
		this.m_clustercraft.RequestLaunch(automated);
	}

	// Token: 0x06005781 RID: 22401 RVA: 0x001FD704 File Offset: 0x001FB904
	public void DoLaunch()
	{
		this.SortModuleListByPosition();
		this.CurrentPad.Trigger(705820818, this);
		this.SetPreferredLaunchPadForWorld(this.CurrentPad);
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger(705820818, this);
		}
	}

	// Token: 0x06005782 RID: 22402 RVA: 0x001FD784 File Offset: 0x001FB984
	public void DoLand(LaunchPad pad)
	{
		int num = pad.RocketBottomPosition;
		for (int i = 0; i < this.clusterModules.Count; i++)
		{
			this.clusterModules[i].Get().MoveToPad(num);
			num = Grid.OffsetCell(num, 0, this.clusterModules[i].Get().GetComponent<Building>().Def.HeightInCells);
		}
		this.SetBottomModule();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			@ref.Get().Trigger(-1165815793, pad);
		}
		pad.Trigger(-1165815793, this);
	}

	// Token: 0x06005783 RID: 22403 RVA: 0x001FD850 File Offset: 0x001FBA50
	public LaunchConditionManager FindLaunchConditionManager()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			LaunchConditionManager component = @ref.Get().GetComponent<LaunchConditionManager>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06005784 RID: 22404 RVA: 0x001FD8B8 File Offset: 0x001FBAB8
	public LaunchableRocketCluster FindLaunchableRocket()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			LaunchableRocketCluster component = rocketModuleCluster.GetComponent<LaunchableRocketCluster>();
			if (component != null && rocketModuleCluster.CraftInterface != null && rocketModuleCluster.CraftInterface.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.Grounded)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06005785 RID: 22405 RVA: 0x001FD940 File Offset: 0x001FBB40
	public List<GameObject> GetParts()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			list.Add(@ref.Get().gameObject);
		}
		return list;
	}

	// Token: 0x06005786 RID: 22406 RVA: 0x001FD9A4 File Offset: 0x001FBBA4
	public RocketEngineCluster GetEngine()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketEngineCluster component = @ref.Get().GetComponent<RocketEngineCluster>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06005787 RID: 22407 RVA: 0x001FDA0C File Offset: 0x001FBC0C
	public RocketModuleCluster GetPrimaryPilotModule(out bool is_robo_pilot)
	{
		is_robo_pilot = false;
		RocketModuleCluster result = null;
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			if (rocketModuleCluster.GetComponent<PassengerRocketModule>() != null)
			{
				result = rocketModuleCluster;
				is_robo_pilot = false;
				break;
			}
			if (rocketModuleCluster.GetComponent<RoboPilotModule>())
			{
				is_robo_pilot = true;
				result = rocketModuleCluster;
			}
		}
		return result;
	}

	// Token: 0x06005788 RID: 22408 RVA: 0x001FDA8C File Offset: 0x001FBC8C
	public PassengerRocketModule GetPassengerModule()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			PassengerRocketModule component = @ref.Get().GetComponent<PassengerRocketModule>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06005789 RID: 22409 RVA: 0x001FDAF4 File Offset: 0x001FBCF4
	public WorldContainer GetInteriorWorld()
	{
		PassengerRocketModule passengerModule = this.GetPassengerModule();
		if (passengerModule == null)
		{
			return null;
		}
		ClustercraftInteriorDoor interiorDoor = passengerModule.GetComponent<ClustercraftExteriorDoor>().GetInteriorDoor();
		if (interiorDoor == null)
		{
			return null;
		}
		return interiorDoor.GetMyWorld();
	}

	// Token: 0x0600578A RID: 22410 RVA: 0x001FDB30 File Offset: 0x001FBD30
	public RoboPilotModule GetRobotPilotModule()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RoboPilotModule component = @ref.Get().GetComponent<RoboPilotModule>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x0600578B RID: 22411 RVA: 0x001FDB98 File Offset: 0x001FBD98
	public RocketClusterDestinationSelector GetClusterDestinationSelector()
	{
		return base.GetComponent<RocketClusterDestinationSelector>();
	}

	// Token: 0x0600578C RID: 22412 RVA: 0x001FDBA0 File Offset: 0x001FBDA0
	public bool HasClusterDestinationSelector()
	{
		return base.GetComponent<RocketClusterDestinationSelector>() != null;
	}

	// Token: 0x0600578D RID: 22413 RVA: 0x001FDBB0 File Offset: 0x001FBDB0
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		this.returnConditions.Clear();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			List<ProcessCondition> conditionSet = @ref.Get().GetConditionSet(conditionType);
			if (conditionSet != null)
			{
				this.returnConditions.AddRange(conditionSet);
			}
		}
		if (this.CurrentPad != null)
		{
			List<ProcessCondition> conditionSet2 = this.CurrentPad.GetComponent<LaunchPadConditions>().GetConditionSet(conditionType);
			if (conditionSet2 != null)
			{
				this.returnConditions.AddRange(conditionSet2);
			}
		}
		return this.returnConditions;
	}

	// Token: 0x0600578E RID: 22414 RVA: 0x001FDC58 File Offset: 0x001FBE58
	public int PopulateConditionSet(ProcessCondition.ProcessConditionType conditionType, List<ProcessCondition> conditions)
	{
		int num = 0;
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			num += @ref.Get().PopulateConditionSet(conditionType, conditions);
		}
		if (this.CurrentPad != null)
		{
			num += this.CurrentPad.GetComponent<LaunchPadConditions>().PopulateConditionSet(conditionType, conditions);
		}
		return num;
	}

	// Token: 0x0600578F RID: 22415 RVA: 0x001FDCDC File Offset: 0x001FBEDC
	private ProcessCondition.Status EvaluateConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		ProcessCondition.Status status = ProcessCondition.Status.Ready;
		List<ProcessCondition> list;
		using (ProcessCondition.ListPool.Get(out list))
		{
			this.PopulateConditionSet(conditionType, list);
			foreach (ProcessCondition processCondition in list)
			{
				ProcessCondition.Status status2 = processCondition.EvaluateCondition();
				if (status2 < status)
				{
					status = status2;
				}
				if (status == ProcessCondition.Status.Failure)
				{
					break;
				}
			}
		}
		return status;
	}

	// Token: 0x06005790 RID: 22416 RVA: 0x001FDD6C File Offset: 0x001FBF6C
	private void ForceAttachmentNetwork()
	{
		RocketModuleCluster rocketModuleCluster = null;
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			RocketModuleCluster rocketModuleCluster2 = @ref.Get();
			if (rocketModuleCluster != null)
			{
				BuildingAttachPoint component = rocketModuleCluster.GetComponent<BuildingAttachPoint>();
				AttachableBuilding component2 = rocketModuleCluster2.GetComponent<AttachableBuilding>();
				component.points[0].attachedBuilding = component2;
			}
			rocketModuleCluster = rocketModuleCluster2;
		}
	}

	// Token: 0x06005791 RID: 22417 RVA: 0x001FDDE8 File Offset: 0x001FBFE8
	public static Storage SpawnRocketDebris(string nameSuffix, SimHashes element)
	{
		Vector3 position = new Vector3(-1f, -1f, 0f);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("DebrisPayload"), position);
		gameObject.GetComponent<PrimaryElement>().SetElement(element, true);
		gameObject.name += nameSuffix;
		gameObject.SetActive(true);
		return gameObject.GetComponent<Storage>();
	}

	// Token: 0x06005792 RID: 22418 RVA: 0x001FDE4C File Offset: 0x001FC04C
	public void CompleteSelfDestruct(object data = null)
	{
		global::Debug.Assert(this.HasTag(GameTags.RocketInSpace), "Self Destruct is only valid for in-space rockets!");
		List<RocketModule> list = new List<RocketModule>();
		foreach (Ref<RocketModuleCluster> @ref in this.clusterModules)
		{
			list.Add(@ref.Get());
		}
		List<GameObject> list2 = new List<GameObject>();
		List<GameObject> list3 = new List<GameObject>();
		foreach (RocketModule rocketModule in list)
		{
			foreach (Storage storage in rocketModule.GetComponents<Storage>())
			{
				bool vent_gas = false;
				bool dump_liquid = false;
				List<GameObject> collect_dropped_items = list3;
				storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
				foreach (GameObject gameObject in list3)
				{
					if (gameObject.HasTag(GameTags.Creature))
					{
						Butcherable component = gameObject.GetComponent<Butcherable>();
						if (component != null && component.drops != null && component.drops.Count > 0)
						{
							GameObject[] collection = component.CreateDrops(1f);
							list2.AddRange(collection);
						}
						gameObject.DeleteObject();
					}
					else
					{
						list2.Add(gameObject);
					}
				}
				list3.Clear();
			}
			Deconstructable component2 = rocketModule.GetComponent<Deconstructable>();
			list2.AddRange(component2.ForceDestroyAndGetMaterials());
		}
		bool flag;
		SimHashes elementID = this.GetPrimaryPilotModule(out flag).GetComponent<PrimaryElement>().ElementID;
		List<Storage> list4 = new List<Storage>();
		foreach (GameObject gameObject2 in list2)
		{
			Pickupable component3 = gameObject2.GetComponent<Pickupable>();
			if (component3 != null)
			{
				component3.PrimaryElement.Units = (float)Mathf.Max(1, Mathf.RoundToInt(component3.PrimaryElement.Units * 0.5f));
				if ((list4.Count == 0 || list4[list4.Count - 1].RemainingCapacity() == 0f) && component3.PrimaryElement.Mass > 0f)
				{
					list4.Add(CraftModuleInterface.SpawnRocketDebris(" from CMI", elementID));
				}
				Storage storage2 = list4[list4.Count - 1];
				while (component3.PrimaryElement.Mass > storage2.RemainingCapacity())
				{
					Pickupable pickupable = component3.Take(storage2.RemainingCapacity());
					storage2.Store(pickupable.gameObject, false, false, true, false);
					storage2 = CraftModuleInterface.SpawnRocketDebris(" from CMI", elementID);
					list4.Add(storage2);
				}
				if (component3.PrimaryElement.Mass > 0f)
				{
					storage2.Store(component3.gameObject, false, false, true, false);
				}
			}
		}
		foreach (Storage cmp in list4)
		{
			RailGunPayload.StatesInstance smi = cmp.GetSMI<RailGunPayload.StatesInstance>();
			smi.StartSM();
			smi.Travel(this.m_clustercraft.Location, ClusterUtil.ClosestVisibleAsteroidToLocation(this.m_clustercraft.Location).Location);
		}
		this.m_clustercraft.SetExploding();
	}

	// Token: 0x04003ABE RID: 15038
	[Serialize]
	private List<Ref<RocketModule>> modules = new List<Ref<RocketModule>>();

	// Token: 0x04003ABF RID: 15039
	[Serialize]
	private List<Ref<RocketModuleCluster>> clusterModules = new List<Ref<RocketModuleCluster>>();

	// Token: 0x04003AC0 RID: 15040
	private Ref<RocketModuleCluster> bottomModule;

	// Token: 0x04003AC1 RID: 15041
	[Serialize]
	private Dictionary<int, Ref<LaunchPad>> preferredLaunchPad = new Dictionary<int, Ref<LaunchPad>>();

	// Token: 0x04003AC2 RID: 15042
	[MyCmpReq]
	private Clustercraft m_clustercraft;

	// Token: 0x04003AC3 RID: 15043
	private List<ProcessCondition.ProcessConditionType> conditionsToCheck = new List<ProcessCondition.ProcessConditionType>
	{
		ProcessCondition.ProcessConditionType.RocketPrep,
		ProcessCondition.ProcessConditionType.RocketStorage,
		ProcessCondition.ProcessConditionType.RocketBoard,
		ProcessCondition.ProcessConditionType.RocketFlight
	};

	// Token: 0x04003AC4 RID: 15044
	private int lastConditionTypeSucceeded = -1;

	// Token: 0x04003AC5 RID: 15045
	private List<ProcessCondition> returnConditions = new List<ProcessCondition>();
}
