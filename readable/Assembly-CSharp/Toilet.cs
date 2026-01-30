using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000819 RID: 2073
public class Toilet : StateMachineComponent<Toilet.StatesInstance>, ISaveLoadable, IUsable, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x0600383C RID: 14396 RVA: 0x0013ACDB File Offset: 0x00138EDB
	// (set) Token: 0x0600383D RID: 14397 RVA: 0x0013ACE3 File Offset: 0x00138EE3
	public int FlushesUsed
	{
		get
		{
			return this._flushesUsed;
		}
		set
		{
			this._flushesUsed = value;
			base.smi.sm.flushes.Set(value, base.smi, false);
		}
	}

	// Token: 0x0600383E RID: 14398 RVA: 0x0013AD0C File Offset: 0x00138F0C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Toilets.Add(this);
		Components.BasicBuildings.Add(this);
		base.smi.StartSM();
		base.GetComponent<ToiletWorkableUse>().trackUses = true;
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		this.meter.SetPositionPercent((float)this.FlushesUsed / (float)this.maxFlushes);
		this.FlushesUsed = this._flushesUsed;
		base.Subscribe<Toilet>(493375141, Toilet.OnRefreshUserMenuDelegate);
	}

	// Token: 0x0600383F RID: 14399 RVA: 0x0013ADBF File Offset: 0x00138FBF
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BasicBuildings.Remove(this);
		Components.Toilets.Remove(this);
	}

	// Token: 0x06003840 RID: 14400 RVA: 0x0013ADDD File Offset: 0x00138FDD
	public bool IsUsable()
	{
		return base.smi.HasTag(GameTags.Usable);
	}

	// Token: 0x06003841 RID: 14401 RVA: 0x0013ADEF File Offset: 0x00138FEF
	public void Flush(WorkerBase worker)
	{
		this.FlushMultiple(worker, 1);
	}

	// Token: 0x06003842 RID: 14402 RVA: 0x0013ADFC File Offset: 0x00138FFC
	public void FlushMultiple(WorkerBase worker, int flushCount)
	{
		int b = this.maxFlushes - this.FlushesUsed;
		int num = Mathf.Min(flushCount, b);
		this.FlushesUsed += num;
		this.meter.SetPositionPercent((float)this.FlushesUsed / (float)this.maxFlushes);
		float num2 = 0f;
		Tag tag = ElementLoader.FindElementByHash(SimHashes.Dirt).tag;
		float num3;
		SimUtil.DiseaseInfo diseaseInfo;
		this.storage.ConsumeAndGetDisease(tag, base.smi.DirtUsedPerFlush() * (float)num, out num3, out diseaseInfo, out num2);
		byte index = Db.Get().Diseases.GetIndex(this.diseaseId);
		int num4 = this.diseasePerFlush * num;
		float mass = base.smi.MassPerFlush() + num3;
		GameObject gameObject = ElementLoader.FindElementByHash(this.solidWastePerUse.elementID).substance.SpawnResource(base.transform.GetPosition(), mass, this.solidWasteTemperature, index, num4, true, false, false);
		gameObject.GetComponent<PrimaryElement>().AddDisease(diseaseInfo.idx, diseaseInfo.count, "Toilet.Flush");
		num4 += diseaseInfo.count;
		this.storage.Store(gameObject, false, false, true, false);
		int num5 = this.diseaseOnDupePerFlush * num;
		worker.GetComponent<PrimaryElement>().AddDisease(index, num5, "Toilet.Flush");
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, Db.Get().Diseases[(int)index].Name, num4 + num5), base.transform, Vector3.up, 1.5f, false, false);
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, true);
	}

	// Token: 0x06003843 RID: 14403 RVA: 0x0013AFA8 File Offset: 0x001391A8
	private void OnRefreshUserMenu(object data)
	{
		if (base.smi.GetCurrentState() == base.smi.sm.full || !base.smi.IsSoiled || base.smi.cleanChore != null)
		{
			return;
		}
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("status_item_toilet_needs_emptying", UI.USERMENUACTIONS.CLEANTOILET.NAME, delegate()
		{
			base.smi.GoTo(base.smi.sm.earlyclean);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEANTOILET.TOOLTIP, true), 1f);
	}

	// Token: 0x06003844 RID: 14404 RVA: 0x0013B03A File Offset: 0x0013923A
	private void SpawnMonster()
	{
		GameUtil.KInstantiate(Assets.GetPrefab(new Tag("Glom")), base.smi.transform.GetPosition(), Grid.SceneLayer.Creatures, null, 0).SetActive(true);
	}

	// Token: 0x06003845 RID: 14405 RVA: 0x0013B06C File Offset: 0x0013926C
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = base.GetComponent<ManualDeliveryKG>().RequestedItemTag.ProperName();
		float mass = base.smi.DirtUsedPerFlush();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		list.Add(item);
		return list;
	}

	// Token: 0x06003846 RID: 14406 RVA: 0x0013B0F0 File Offset: 0x001392F0
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(this.solidWastePerUse.elementID).tag.ProperName();
		float mass = base.smi.MassPerFlush() + base.smi.DirtUsedPerFlush();
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.solidWasteTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.solidWasteTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false));
		Disease disease = Db.Get().Diseases.Get(this.diseaseId);
		int units = this.diseasePerFlush + this.diseaseOnDupePerFlush;
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.DiseaseSource, false));
		return list;
	}

	// Token: 0x06003847 RID: 14407 RVA: 0x0013B205 File Offset: 0x00139405
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

	// Token: 0x04002222 RID: 8738
	private static readonly HashedString[] FULL_ANIMS = new HashedString[]
	{
		"full_pre",
		"full"
	};

	// Token: 0x04002223 RID: 8739
	private const string EXIT_FULL_ANIM_NAME = "full_pst";

	// Token: 0x04002224 RID: 8740
	private const string EXIT_FULL_GUNK_ANIM_NAME = "full_gunk_pst";

	// Token: 0x04002225 RID: 8741
	private static readonly HashedString[] GUNK_CLOGGED_ANIMS = new HashedString[]
	{
		"full_gunk_pre",
		"full_gunk"
	};

	// Token: 0x04002226 RID: 8742
	[SerializeField]
	public Toilet.SpawnInfo solidWastePerUse;

	// Token: 0x04002227 RID: 8743
	[SerializeField]
	public float solidWasteTemperature;

	// Token: 0x04002228 RID: 8744
	[SerializeField]
	public Toilet.SpawnInfo gasWasteWhenFull;

	// Token: 0x04002229 RID: 8745
	[SerializeField]
	public int maxFlushes = 15;

	// Token: 0x0400222A RID: 8746
	[SerializeField]
	public string diseaseId;

	// Token: 0x0400222B RID: 8747
	[SerializeField]
	public int diseasePerFlush;

	// Token: 0x0400222C RID: 8748
	[SerializeField]
	public int diseaseOnDupePerFlush;

	// Token: 0x0400222D RID: 8749
	[SerializeField]
	public float dirtUsedPerFlush = 13f;

	// Token: 0x0400222E RID: 8750
	[Serialize]
	public int _flushesUsed;

	// Token: 0x0400222F RID: 8751
	private MeterController meter;

	// Token: 0x04002230 RID: 8752
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04002231 RID: 8753
	[MyCmpReq]
	private ManualDeliveryKG manualdeliverykg;

	// Token: 0x04002232 RID: 8754
	private static readonly EventSystem.IntraObjectHandler<Toilet> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Toilet>(delegate(Toilet component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x020017B1 RID: 6065
	[Serializable]
	public struct SpawnInfo
	{
		// Token: 0x06009C24 RID: 39972 RVA: 0x003983BD File Offset: 0x003965BD
		public SpawnInfo(SimHashes element_id, float mass, float interval)
		{
			this.elementID = element_id;
			this.mass = mass;
			this.interval = interval;
		}

		// Token: 0x04007876 RID: 30838
		[HashedEnum]
		public SimHashes elementID;

		// Token: 0x04007877 RID: 30839
		public float mass;

		// Token: 0x04007878 RID: 30840
		public float interval;
	}

	// Token: 0x020017B2 RID: 6066
	public class StatesInstance : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.GameInstance
	{
		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06009C25 RID: 39973 RVA: 0x003983D4 File Offset: 0x003965D4
		public bool IsCloggedWithGunk
		{
			get
			{
				return base.sm.cloggedWithGunk.Get(this);
			}
		}

		// Token: 0x06009C26 RID: 39974 RVA: 0x003983E7 File Offset: 0x003965E7
		public StatesInstance(Toilet master) : base(master)
		{
			this.activeUseChores = new List<Chore>();
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06009C27 RID: 39975 RVA: 0x00398406 File Offset: 0x00396606
		public bool IsSoiled
		{
			get
			{
				return base.master.FlushesUsed > 0;
			}
		}

		// Token: 0x06009C28 RID: 39976 RVA: 0x00398416 File Offset: 0x00396616
		public int GetFlushesRemaining()
		{
			return base.master.maxFlushes - base.master.FlushesUsed;
		}

		// Token: 0x06009C29 RID: 39977 RVA: 0x00398430 File Offset: 0x00396630
		public bool RequiresDirtDelivery()
		{
			return base.master.storage.IsEmpty() || !base.master.storage.Has(GameTags.Dirt) || (base.master.storage.GetAmountAvailable(GameTags.Dirt) < base.master.manualdeliverykg.capacity && !this.IsSoiled);
		}

		// Token: 0x06009C2A RID: 39978 RVA: 0x003984A1 File Offset: 0x003966A1
		public float MassPerFlush()
		{
			return base.master.solidWastePerUse.mass;
		}

		// Token: 0x06009C2B RID: 39979 RVA: 0x003984B3 File Offset: 0x003966B3
		public float DirtUsedPerFlush()
		{
			return base.master.dirtUsedPerFlush;
		}

		// Token: 0x06009C2C RID: 39980 RVA: 0x003984C0 File Offset: 0x003966C0
		public bool IsToxicSandRemoved()
		{
			Tag tag = GameTagExtensions.Create(base.master.solidWastePerUse.elementID);
			return base.master.storage.FindFirst(tag) == null;
		}

		// Token: 0x06009C2D RID: 39981 RVA: 0x003984FC File Offset: 0x003966FC
		public void CreateCleanChore()
		{
			if (this.cleanChore != null)
			{
				this.cleanChore.Cancel("dupe");
			}
			ToiletWorkableClean component = base.master.GetComponent<ToiletWorkableClean>();
			component.SetIsCloggedByGunk(this.IsCloggedWithGunk);
			this.cleanChore = new WorkChore<ToiletWorkableClean>(Db.Get().ChoreTypes.CleanToilet, component, null, true, new Action<Chore>(this.OnCleanComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
		}

		// Token: 0x06009C2E RID: 39982 RVA: 0x00398570 File Offset: 0x00396770
		public void CancelCleanChore()
		{
			if (this.cleanChore != null)
			{
				this.cleanChore.Cancel("Cancelled");
				this.cleanChore = null;
			}
		}

		// Token: 0x06009C2F RID: 39983 RVA: 0x00398594 File Offset: 0x00396794
		private void DropFromStorage(Tag tag)
		{
			ListPool<GameObject, Toilet>.PooledList pooledList = ListPool<GameObject, Toilet>.Allocate();
			base.master.storage.Find(tag, pooledList);
			foreach (GameObject go in pooledList)
			{
				base.master.storage.Drop(go, true);
			}
			pooledList.Recycle();
		}

		// Token: 0x06009C30 RID: 39984 RVA: 0x00398610 File Offset: 0x00396810
		private void OnCleanComplete(Chore chore)
		{
			this.cleanChore = null;
			Tag tag = GameTagExtensions.Create(base.master.solidWastePerUse.elementID);
			Tag tag2 = ElementLoader.FindElementByHash(SimHashes.Dirt).tag;
			this.DropFromStorage(tag);
			this.DropFromStorage(tag2);
			base.sm.cloggedWithGunk.Set(false, this, false);
			base.master.meter.SetPositionPercent((float)base.master.FlushesUsed / (float)base.master.maxFlushes);
		}

		// Token: 0x06009C31 RID: 39985 RVA: 0x00398698 File Offset: 0x00396898
		public void Flush()
		{
			WorkerBase worker = base.master.GetComponent<ToiletWorkableUse>().worker;
			base.master.Flush(worker);
		}

		// Token: 0x06009C32 RID: 39986 RVA: 0x003986C4 File Offset: 0x003968C4
		public void FlushAll()
		{
			WorkerBase worker = base.master.GetComponent<ToiletWorkableUse>().worker;
			base.master.FlushMultiple(worker, base.master.maxFlushes - base.master.FlushesUsed);
		}

		// Token: 0x06009C33 RID: 39987 RVA: 0x00398705 File Offset: 0x00396905
		public void FlushGunk()
		{
			base.sm.cloggedWithGunk.Set(true, this, false);
			this.Flush();
		}

		// Token: 0x06009C34 RID: 39988 RVA: 0x00398721 File Offset: 0x00396921
		public HashedString[] GetCloggedAnimations()
		{
			if (this.IsCloggedWithGunk)
			{
				return Toilet.GUNK_CLOGGED_ANIMS;
			}
			return Toilet.FULL_ANIMS;
		}

		// Token: 0x04007879 RID: 30841
		public Chore cleanChore;

		// Token: 0x0400787A RID: 30842
		public List<Chore> activeUseChores;

		// Token: 0x0400787B RID: 30843
		public float monsterSpawnTime = 1200f;
	}

	// Token: 0x020017B3 RID: 6067
	public class States : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet>
	{
		// Token: 0x06009C35 RID: 39989 RVA: 0x00398738 File Offset: 0x00396938
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.needsdirt;
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			this.root.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.needsdirt, (Toilet.StatesInstance smi) => smi.RequiresDirtDelivery()).EventTransition(GameHashes.OperationalChanged, this.notoperational, (Toilet.StatesInstance smi) => !smi.Get<Operational>().IsOperational);
			this.needsdirt.Enter(delegate(Toilet.StatesInstance smi)
			{
				if (smi.RequiresDirtDelivery())
				{
					smi.master.manualdeliverykg.RequestDelivery();
				}
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null).EventTransition(GameHashes.OnStorageChange, this.ready, (Toilet.StatesInstance smi) => !smi.RequiresDirtDelivery());
			this.ready.ParamTransition<int>(this.flushes, this.full, (Toilet.StatesInstance smi, int p) => smi.GetFlushesRemaining() <= 0).ParamTransition<int>(this.flushes, this.earlyclean, (Toilet.StatesInstance smi, int p) => smi.IsCloggedWithGunk).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Toilet, null).ToggleRecurringChore(new Func<Toilet.StatesInstance, Chore>(this.CreateUrgentUseChore), null).ToggleRecurringChore(new Func<Toilet.StatesInstance, Chore>(this.CreateBreakUseChore), null).ToggleTag(GameTags.Usable).EventHandler(GameHashes.Flush, delegate(Toilet.StatesInstance smi, object data)
			{
				smi.Flush();
			}).EventHandler(GameHashes.FlushGunk, delegate(Toilet.StatesInstance smi, object data)
			{
				smi.FlushGunk();
			});
			this.earlyclean.PlayAnims(new Func<Toilet.StatesInstance, HashedString[]>(Toilet.States.GetCloggedAnimations), KAnim.PlayMode.Once).OnAnimQueueComplete(this.earlyWaitingForClean);
			this.earlyWaitingForClean.Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.CreateCleanChore();
			}).Exit(delegate(Toilet.StatesInstance smi)
			{
				smi.CancelCleanChore();
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying, null).ToggleMainStatusItem(delegate(Toilet.StatesInstance smi)
			{
				if (!smi.sm.cloggedWithGunk.Get(smi))
				{
					return Db.Get().BuildingStatusItems.Unusable;
				}
				return Db.Get().BuildingStatusItems.UnusableGunked;
			}, null).EventTransition(GameHashes.OnStorageChange, this.exit_full, (Toilet.StatesInstance smi) => smi.IsToxicSandRemoved());
			this.full.PlayAnims(new Func<Toilet.StatesInstance, HashedString[]>(Toilet.States.GetCloggedAnimations), KAnim.PlayMode.Once).OnAnimQueueComplete(this.fullWaitingForClean);
			this.fullWaitingForClean.Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.CreateCleanChore();
			}).Exit(delegate(Toilet.StatesInstance smi)
			{
				smi.CancelCleanChore();
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying, null).ToggleMainStatusItem(delegate(Toilet.StatesInstance smi)
			{
				if (!smi.sm.cloggedWithGunk.Get(smi))
				{
					return Db.Get().BuildingStatusItems.Unusable;
				}
				return Db.Get().BuildingStatusItems.UnusableGunked;
			}, null).EventTransition(GameHashes.OnStorageChange, this.exit_full, (Toilet.StatesInstance smi) => smi.IsToxicSandRemoved()).Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.Schedule(smi.monsterSpawnTime, delegate
				{
					smi.master.SpawnMonster();
				}, null);
			});
			this.exit_full.PlayAnim(new Func<Toilet.StatesInstance, string>(Toilet.States.GetUnclogedAnimation), KAnim.PlayMode.Once).OnAnimQueueComplete(this.empty).Exit(new StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State.Callback(Toilet.States.ClearCloggedByGunkFlag)).ScheduleGoTo(0.74f, this.empty);
			this.empty.PlayAnim("off").Enter("ClearFlushes", delegate(Toilet.StatesInstance smi)
			{
				smi.master.FlushesUsed = 0;
			}).GoTo(this.needsdirt);
			this.notoperational.EventTransition(GameHashes.OperationalChanged, this.needsdirt, (Toilet.StatesInstance smi) => smi.Get<Operational>().IsOperational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null);
		}

		// Token: 0x06009C36 RID: 39990 RVA: 0x00398BDF File Offset: 0x00396DDF
		private static void ClearCloggedByGunkFlag(Toilet.StatesInstance smi)
		{
			smi.sm.cloggedWithGunk.Set(false, smi, false);
		}

		// Token: 0x06009C37 RID: 39991 RVA: 0x00398BF5 File Offset: 0x00396DF5
		public static string GetUnclogedAnimation(Toilet.StatesInstance smi)
		{
			if (!smi.sm.cloggedWithGunk.Get(smi))
			{
				return "full_pst";
			}
			return "full_gunk_pst";
		}

		// Token: 0x06009C38 RID: 39992 RVA: 0x00398C15 File Offset: 0x00396E15
		public static HashedString[] GetCloggedAnimations(Toilet.StatesInstance smi)
		{
			return smi.GetCloggedAnimations();
		}

		// Token: 0x06009C39 RID: 39993 RVA: 0x00398C1D File Offset: 0x00396E1D
		private Chore CreateUrgentUseChore(Toilet.StatesInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.Pee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderFull, null);
			chore.AddPrecondition(ChorePreconditions.instance.NotCurrentlyPeeing, null);
			return chore;
		}

		// Token: 0x06009C3A RID: 39994 RVA: 0x00398C58 File Offset: 0x00396E58
		private Chore CreateBreakUseChore(Toilet.StatesInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.BreakPee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderNotFull, null);
			chore.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Hygiene);
			return chore;
		}

		// Token: 0x06009C3B RID: 39995 RVA: 0x00398CAC File Offset: 0x00396EAC
		private Chore CreateUseChore(Toilet.StatesInstance smi, ChoreType choreType)
		{
			WorkChore<ToiletWorkableUse> workChore = new WorkChore<ToiletWorkableUse>(choreType, smi.master, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
			smi.activeUseChores.Add(workChore);
			WorkChore<ToiletWorkableUse> workChore2 = workChore;
			workChore2.onExit = (Action<Chore>)Delegate.Combine(workChore2.onExit, new Action<Chore>(delegate(Chore exiting_chore)
			{
				smi.activeUseChores.Remove(exiting_chore);
			}));
			workChore.AddPrecondition(ChorePreconditions.instance.IsPreferredAssignableOrUrgentBladder, smi.master.GetComponent<Assignable>());
			workChore.AddPrecondition(ChorePreconditions.instance.IsExclusivelyAvailableWithOtherChores, smi.activeUseChores);
			return workChore;
		}

		// Token: 0x0400787C RID: 30844
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State needsdirt;

		// Token: 0x0400787D RID: 30845
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State empty;

		// Token: 0x0400787E RID: 30846
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State notoperational;

		// Token: 0x0400787F RID: 30847
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State ready;

		// Token: 0x04007880 RID: 30848
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State earlyclean;

		// Token: 0x04007881 RID: 30849
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State earlyWaitingForClean;

		// Token: 0x04007882 RID: 30850
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State full;

		// Token: 0x04007883 RID: 30851
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State fullWaitingForClean;

		// Token: 0x04007884 RID: 30852
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State exit_full;

		// Token: 0x04007885 RID: 30853
		public StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.BoolParameter cloggedWithGunk;

		// Token: 0x04007886 RID: 30854
		public StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.IntParameter flushes = new StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.IntParameter(0);

		// Token: 0x02002958 RID: 10584
		public class ReadyStates : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State
		{
			// Token: 0x0400B6F8 RID: 46840
			public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State idle;

			// Token: 0x0400B6F9 RID: 46841
			public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State inuse;

			// Token: 0x0400B6FA RID: 46842
			public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State flush;
		}
	}
}
