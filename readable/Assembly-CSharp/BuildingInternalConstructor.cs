using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006FE RID: 1790
public class BuildingInternalConstructor : GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>
{
	// Token: 0x06002C65 RID: 11365 RVA: 0x00102594 File Offset: 0x00100794
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (BuildingInternalConstructor.Instance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(false);
		});
		this.operational.DefaultState(this.operational.constructionRequired).EventTransition(GameHashes.OperationalChanged, this.inoperational, (BuildingInternalConstructor.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.operational.constructionRequired.EventTransition(GameHashes.OnStorageChange, this.operational.constructionHappening, (BuildingInternalConstructor.Instance smi) => smi.GetMassForConstruction() != null).EventTransition(GameHashes.OnStorageChange, this.operational.constructionSatisfied, (BuildingInternalConstructor.Instance smi) => smi.HasOutputInStorage()).ToggleFetch((BuildingInternalConstructor.Instance smi) => smi.CreateFetchList(), this.operational.constructionHappening).ParamTransition<bool>(this.constructionRequested, this.operational.constructionSatisfied, GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.IsFalse).Enter(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(true);
		}).Exit(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(false);
		});
		this.operational.constructionHappening.EventTransition(GameHashes.OnStorageChange, this.operational.constructionSatisfied, (BuildingInternalConstructor.Instance smi) => smi.HasOutputInStorage()).EventTransition(GameHashes.OnStorageChange, this.operational.constructionRequired, (BuildingInternalConstructor.Instance smi) => smi.GetMassForConstruction() == null).ToggleChore((BuildingInternalConstructor.Instance smi) => smi.CreateWorkChore(), this.operational.constructionHappening, this.operational.constructionHappening).ParamTransition<bool>(this.constructionRequested, this.operational.constructionSatisfied, GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.IsFalse).Enter(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(true);
		}).Exit(delegate(BuildingInternalConstructor.Instance smi)
		{
			smi.ShowConstructionSymbol(false);
		});
		this.operational.constructionSatisfied.EventTransition(GameHashes.OnStorageChange, this.operational.constructionRequired, (BuildingInternalConstructor.Instance smi) => !smi.HasOutputInStorage() && this.constructionRequested.Get(smi)).ParamTransition<bool>(this.constructionRequested, this.operational.constructionRequired, (BuildingInternalConstructor.Instance smi, bool p) => p && !smi.HasOutputInStorage());
	}

	// Token: 0x04001A55 RID: 6741
	public GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State inoperational;

	// Token: 0x04001A56 RID: 6742
	public BuildingInternalConstructor.OperationalStates operational;

	// Token: 0x04001A57 RID: 6743
	public StateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.BoolParameter constructionRequested = new StateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.BoolParameter(true);

	// Token: 0x020015C2 RID: 5570
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007290 RID: 29328
		public DefComponent<Storage> storage;

		// Token: 0x04007291 RID: 29329
		public float constructionMass;

		// Token: 0x04007292 RID: 29330
		public List<string> outputIDs;

		// Token: 0x04007293 RID: 29331
		public bool spawnIntoStorage;

		// Token: 0x04007294 RID: 29332
		public string constructionSymbol;
	}

	// Token: 0x020015C3 RID: 5571
	public class OperationalStates : GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State
	{
		// Token: 0x04007295 RID: 29333
		public GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State constructionRequired;

		// Token: 0x04007296 RID: 29334
		public GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State constructionHappening;

		// Token: 0x04007297 RID: 29335
		public GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.State constructionSatisfied;
	}

	// Token: 0x020015C4 RID: 5572
	public new class Instance : GameStateMachine<BuildingInternalConstructor, BuildingInternalConstructor.Instance, IStateMachineTarget, BuildingInternalConstructor.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x0600946D RID: 37997 RVA: 0x00378FAC File Offset: 0x003771AC
		public Instance(IStateMachineTarget master, BuildingInternalConstructor.Def def) : base(master, def)
		{
			this.storage = def.storage.Get(this);
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new InternalConstructionCompleteCondition(this));
		}

		// Token: 0x0600946E RID: 37998 RVA: 0x00378FDC File Offset: 0x003771DC
		protected override void OnCleanUp()
		{
			Element element = null;
			float num = 0f;
			float num2 = 0f;
			byte maxValue = byte.MaxValue;
			int disease_count = 0;
			foreach (string s in base.def.outputIDs)
			{
				GameObject gameObject = this.storage.FindFirst(s);
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					global::Debug.Assert(element == null || element == component.Element);
					element = component.Element;
					num2 = GameUtil.GetFinalTemperature(num, num2, component.Mass, component.Temperature);
					num += component.Mass;
					gameObject.DeleteObject();
				}
			}
			if (element != null)
			{
				element.substance.SpawnResource(base.transform.GetPosition(), num, num2, maxValue, disease_count, false, false, false);
			}
			base.OnCleanUp();
		}

		// Token: 0x0600946F RID: 37999 RVA: 0x003790DC File Offset: 0x003772DC
		public FetchList2 CreateFetchList()
		{
			FetchList2 fetchList = new FetchList2(this.storage, Db.Get().ChoreTypes.Fetch);
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			fetchList.Add(component.Element.tag, null, base.def.constructionMass, Operational.State.None);
			return fetchList;
		}

		// Token: 0x06009470 RID: 38000 RVA: 0x00379128 File Offset: 0x00377328
		public PrimaryElement GetMassForConstruction()
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			return this.storage.FindFirstWithMass(component.Element.tag, base.def.constructionMass);
		}

		// Token: 0x06009471 RID: 38001 RVA: 0x0037915D File Offset: 0x0037735D
		public bool HasOutputInStorage()
		{
			return this.storage.FindFirst(base.def.outputIDs[0].ToTag());
		}

		// Token: 0x06009472 RID: 38002 RVA: 0x00379185 File Offset: 0x00377385
		public bool IsRequestingConstruction()
		{
			base.sm.constructionRequested.Get(this);
			return base.smi.sm.constructionRequested.Get(base.smi);
		}

		// Token: 0x06009473 RID: 38003 RVA: 0x003791B4 File Offset: 0x003773B4
		public void ConstructionComplete(bool force = false)
		{
			SimHashes element_id;
			if (!force)
			{
				PrimaryElement massForConstruction = this.GetMassForConstruction();
				element_id = massForConstruction.ElementID;
				float mass = massForConstruction.Mass;
				float num = massForConstruction.Temperature * massForConstruction.Mass;
				massForConstruction.Mass -= base.def.constructionMass;
				Mathf.Clamp(num / mass, 0f, 318.15f);
			}
			else
			{
				element_id = SimHashes.Cuprite;
				float temperature = base.GetComponent<PrimaryElement>().Temperature;
			}
			foreach (string s in base.def.outputIDs)
			{
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(s), base.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0);
				gameObject.GetComponent<PrimaryElement>().SetElement(element_id, false);
				gameObject.SetActive(true);
				if (base.def.spawnIntoStorage)
				{
					this.storage.Store(gameObject, false, false, true, false);
				}
			}
		}

		// Token: 0x06009474 RID: 38004 RVA: 0x003792BC File Offset: 0x003774BC
		public WorkChore<BuildingInternalConstructorWorkable> CreateWorkChore()
		{
			return new WorkChore<BuildingInternalConstructorWorkable>(Db.Get().ChoreTypes.Build, base.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x06009475 RID: 38005 RVA: 0x003792F4 File Offset: 0x003774F4
		public void ShowConstructionSymbol(bool show)
		{
			KBatchedAnimController component = base.master.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.SetSymbolVisiblity(base.def.constructionSymbol, show);
			}
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06009476 RID: 38006 RVA: 0x00379330 File Offset: 0x00377530
		public string SidescreenButtonText
		{
			get
			{
				if (!base.smi.sm.constructionRequested.Get(base.smi))
				{
					return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.ALLOW_INTERNAL_CONSTRUCTOR.text, Assets.GetPrefab(base.def.outputIDs[0]).GetProperName());
				}
				return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.DISALLOW_INTERNAL_CONSTRUCTOR.text, Assets.GetPrefab(base.def.outputIDs[0]).GetProperName());
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06009477 RID: 38007 RVA: 0x003793BC File Offset: 0x003775BC
		public string SidescreenButtonTooltip
		{
			get
			{
				if (!base.smi.sm.constructionRequested.Get(base.smi))
				{
					return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.ALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP.text, Assets.GetPrefab(base.def.outputIDs[0]).GetProperName());
				}
				return string.Format(UI.UISIDESCREENS.BUTTONMENUSIDESCREEN.DISALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP.text, Assets.GetPrefab(base.def.outputIDs[0]).GetProperName());
			}
		}

		// Token: 0x06009478 RID: 38008 RVA: 0x00379448 File Offset: 0x00377648
		public void OnSidescreenButtonPressed()
		{
			base.smi.sm.constructionRequested.Set(!base.smi.sm.constructionRequested.Get(base.smi), base.smi, false);
			if (DebugHandler.InstantBuildMode && base.smi.sm.constructionRequested.Get(base.smi) && !this.HasOutputInStorage())
			{
				this.ConstructionComplete(true);
			}
		}

		// Token: 0x06009479 RID: 38009 RVA: 0x003794C3 File Offset: 0x003776C3
		public void SetButtonTextOverride(ButtonMenuTextOverride text)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600947A RID: 38010 RVA: 0x003794CA File Offset: 0x003776CA
		public bool SidescreenEnabled()
		{
			return true;
		}

		// Token: 0x0600947B RID: 38011 RVA: 0x003794CD File Offset: 0x003776CD
		public bool SidescreenButtonInteractable()
		{
			return true;
		}

		// Token: 0x0600947C RID: 38012 RVA: 0x003794D0 File Offset: 0x003776D0
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x0600947D RID: 38013 RVA: 0x003794D4 File Offset: 0x003776D4
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x04007298 RID: 29336
		private Storage storage;

		// Token: 0x04007299 RID: 29337
		[Serialize]
		private float constructionElapsed;

		// Token: 0x0400729A RID: 29338
		private ProgressBar progressBar;
	}
}
