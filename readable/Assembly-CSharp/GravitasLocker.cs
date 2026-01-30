using System;
using UnityEngine;

// Token: 0x02000976 RID: 2422
public class GravitasLocker : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>
{
	// Token: 0x06004518 RID: 17688 RVA: 0x00190630 File Offset: 0x0018E830
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.close;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.close.ParamTransition<bool>(this.IsOpen, this.open, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue).DefaultState(this.close.idle);
		this.close.idle.PlayAnim("on").ParamTransition<bool>(this.WorkOrderGiven, this.close.work, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue);
		this.close.work.DefaultState(this.close.work.waitingForDupe);
		this.close.work.waitingForDupe.Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StartlWorkChore_OpenLocker)).Exit(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StopWorkChore)).WorkableCompleteTransition((GravitasLocker.Instance smi) => smi.GetWorkable(), this.close.work.complete).ParamTransition<bool>(this.WorkOrderGiven, this.close, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse);
		this.close.work.complete.Enter(delegate(GravitasLocker.Instance smi)
		{
			this.WorkOrderGiven.Set(false, smi, false);
		}).Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.Open)).TriggerOnEnter(GameHashes.UIRefresh, null);
		this.open.ParamTransition<bool>(this.IsOpen, this.close, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse).DefaultState(this.open.opening);
		this.open.opening.PlayAnim("working").OnAnimQueueComplete(this.open.idle);
		this.open.idle.PlayAnim("empty").Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.SpawnLoot)).ParamTransition<bool>(this.WorkOrderGiven, this.open.work, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsTrue);
		this.open.work.DefaultState(this.open.work.waitingForDupe);
		this.open.work.waitingForDupe.Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StartWorkChore_CloseLocker)).Exit(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.StopWorkChore)).WorkableCompleteTransition((GravitasLocker.Instance smi) => smi.GetWorkable(), this.open.work.complete).ParamTransition<bool>(this.WorkOrderGiven, this.open.idle, GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.IsFalse);
		this.open.work.complete.Enter(delegate(GravitasLocker.Instance smi)
		{
			this.WorkOrderGiven.Set(false, smi, false);
		}).Enter(new StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State.Callback(GravitasLocker.Close)).TriggerOnEnter(GameHashes.UIRefresh, null);
	}

	// Token: 0x06004519 RID: 17689 RVA: 0x001908FC File Offset: 0x0018EAFC
	public static void Open(GravitasLocker.Instance smi)
	{
		smi.Open();
	}

	// Token: 0x0600451A RID: 17690 RVA: 0x00190904 File Offset: 0x0018EB04
	public static void Close(GravitasLocker.Instance smi)
	{
		smi.Close();
	}

	// Token: 0x0600451B RID: 17691 RVA: 0x0019090C File Offset: 0x0018EB0C
	public static void SpawnLoot(GravitasLocker.Instance smi)
	{
		smi.SpawnLoot();
	}

	// Token: 0x0600451C RID: 17692 RVA: 0x00190914 File Offset: 0x0018EB14
	public static void StartWorkChore_CloseLocker(GravitasLocker.Instance smi)
	{
		smi.CreateWorkChore_CloseLocker();
	}

	// Token: 0x0600451D RID: 17693 RVA: 0x0019091C File Offset: 0x0018EB1C
	public static void StartlWorkChore_OpenLocker(GravitasLocker.Instance smi)
	{
		smi.CreateWorkChore_OpenLocker();
	}

	// Token: 0x0600451E RID: 17694 RVA: 0x00190924 File Offset: 0x0018EB24
	public static void StopWorkChore(GravitasLocker.Instance smi)
	{
		smi.StopWorkChore();
	}

	// Token: 0x04002E50 RID: 11856
	public const float CLOSE_WORKTIME = 1f;

	// Token: 0x04002E51 RID: 11857
	public const float OPEN_WORKTIME = 1.5f;

	// Token: 0x04002E52 RID: 11858
	public const string CLOSED_ANIM_NAME = "on";

	// Token: 0x04002E53 RID: 11859
	public const string OPENING_ANIM_NAME = "working";

	// Token: 0x04002E54 RID: 11860
	public const string OPENED = "empty";

	// Token: 0x04002E55 RID: 11861
	private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter IsOpen;

	// Token: 0x04002E56 RID: 11862
	private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter WasEmptied;

	// Token: 0x04002E57 RID: 11863
	private StateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.BoolParameter WorkOrderGiven;

	// Token: 0x04002E58 RID: 11864
	public GravitasLocker.CloseStates close;

	// Token: 0x04002E59 RID: 11865
	public GravitasLocker.OpenStates open;

	// Token: 0x020019B4 RID: 6580
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007F1F RID: 32543
		public bool CanBeClosed;

		// Token: 0x04007F20 RID: 32544
		public string SideScreen_OpenButtonText;

		// Token: 0x04007F21 RID: 32545
		public string SideScreen_OpenButtonTooltip;

		// Token: 0x04007F22 RID: 32546
		public string SideScreen_CancelOpenButtonText;

		// Token: 0x04007F23 RID: 32547
		public string SideScreen_CancelOpenButtonTooltip;

		// Token: 0x04007F24 RID: 32548
		public string SideScreen_CloseButtonText;

		// Token: 0x04007F25 RID: 32549
		public string SideScreen_CloseButtonTooltip;

		// Token: 0x04007F26 RID: 32550
		public string SideScreen_CancelCloseButtonText;

		// Token: 0x04007F27 RID: 32551
		public string SideScreen_CancelCloseButtonTooltip;

		// Token: 0x04007F28 RID: 32552
		public string OPEN_INTERACT_ANIM_NAME = "anim_interacts_clothingfactory_kanim";

		// Token: 0x04007F29 RID: 32553
		public string CLOSE_INTERACT_ANIM_NAME = "anim_interacts_clothingfactory_kanim";

		// Token: 0x04007F2A RID: 32554
		public string[] ObjectsToSpawn = new string[0];

		// Token: 0x04007F2B RID: 32555
		public string[] LootSymbols = new string[0];
	}

	// Token: 0x020019B5 RID: 6581
	public class WorkStates : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
	{
		// Token: 0x04007F2C RID: 32556
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State waitingForDupe;

		// Token: 0x04007F2D RID: 32557
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State complete;
	}

	// Token: 0x020019B6 RID: 6582
	public class CloseStates : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
	{
		// Token: 0x04007F2E RID: 32558
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State idle;

		// Token: 0x04007F2F RID: 32559
		public GravitasLocker.WorkStates work;
	}

	// Token: 0x020019B7 RID: 6583
	public class OpenStates : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State
	{
		// Token: 0x04007F30 RID: 32560
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State opening;

		// Token: 0x04007F31 RID: 32561
		public GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.State idle;

		// Token: 0x04007F32 RID: 32562
		public GravitasLocker.WorkStates work;
	}

	// Token: 0x020019B8 RID: 6584
	public new class Instance : GameStateMachine<GravitasLocker, GravitasLocker.Instance, IStateMachineTarget, GravitasLocker.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x0600A2EE RID: 41710 RVA: 0x003B0CF0 File Offset: 0x003AEEF0
		public bool WorkOrderGiven
		{
			get
			{
				return base.smi.sm.WorkOrderGiven.Get(base.smi);
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x0600A2EF RID: 41711 RVA: 0x003B0D0D File Offset: 0x003AEF0D
		public bool IsOpen
		{
			get
			{
				return base.smi.sm.IsOpen.Get(base.smi);
			}
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x0600A2F0 RID: 41712 RVA: 0x003B0D2A File Offset: 0x003AEF2A
		public bool HasContents
		{
			get
			{
				return !base.smi.sm.WasEmptied.Get(base.smi) && base.def.ObjectsToSpawn.Length != 0;
			}
		}

		// Token: 0x0600A2F1 RID: 41713 RVA: 0x003B0D5A File Offset: 0x003AEF5A
		public Workable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x0600A2F2 RID: 41714 RVA: 0x003B0D62 File Offset: 0x003AEF62
		public void Open()
		{
			base.smi.sm.IsOpen.Set(true, base.smi, false);
		}

		// Token: 0x0600A2F3 RID: 41715 RVA: 0x003B0D82 File Offset: 0x003AEF82
		public void Close()
		{
			base.smi.sm.IsOpen.Set(false, base.smi, false);
		}

		// Token: 0x0600A2F4 RID: 41716 RVA: 0x003B0DA2 File Offset: 0x003AEFA2
		public Instance(IStateMachineTarget master, GravitasLocker.Def def) : base(master, def)
		{
		}

		// Token: 0x0600A2F5 RID: 41717 RVA: 0x003B0DAC File Offset: 0x003AEFAC
		public override void StartSM()
		{
			this.DefineDropSpawnPositions();
			base.StartSM();
			this.UpdateContentPreviewSymbols();
		}

		// Token: 0x0600A2F6 RID: 41718 RVA: 0x003B0DC0 File Offset: 0x003AEFC0
		public void DefineDropSpawnPositions()
		{
			if (this.dropSpawnPositions == null && base.def.LootSymbols.Length != 0)
			{
				this.dropSpawnPositions = new Vector3[base.def.LootSymbols.Length];
				for (int i = 0; i < this.dropSpawnPositions.Length; i++)
				{
					bool flag;
					Vector3 vector = this.animController.GetSymbolTransform(base.def.LootSymbols[i], out flag).GetColumn(3);
					vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
					this.dropSpawnPositions[i] = (flag ? vector : base.gameObject.transform.GetPosition());
				}
			}
		}

		// Token: 0x0600A2F7 RID: 41719 RVA: 0x003B0E74 File Offset: 0x003AF074
		public void CreateWorkChore_CloseLocker()
		{
			if (this.chore == null)
			{
				this.workable.SetWorkTime(1f);
				this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.Repair, this.workable, null, true, null, null, null, true, null, false, true, Assets.GetAnim(base.def.CLOSE_INTERACT_ANIM_NAME), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
			}
		}

		// Token: 0x0600A2F8 RID: 41720 RVA: 0x003B0EE0 File Offset: 0x003AF0E0
		public void CreateWorkChore_OpenLocker()
		{
			if (this.chore == null)
			{
				this.workable.SetWorkTime(1.5f);
				this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, this.workable, null, true, null, null, null, true, null, false, true, Assets.GetAnim(base.def.OPEN_INTERACT_ANIM_NAME), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
			}
		}

		// Token: 0x0600A2F9 RID: 41721 RVA: 0x003B0F4A File Offset: 0x003AF14A
		public void StopWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Canceled by user");
				this.chore = null;
			}
		}

		// Token: 0x0600A2FA RID: 41722 RVA: 0x003B0F6C File Offset: 0x003AF16C
		public void SpawnLoot()
		{
			if (this.HasContents)
			{
				for (int i = 0; i < base.def.ObjectsToSpawn.Length; i++)
				{
					string name = base.def.ObjectsToSpawn[i];
					GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), 0, 0, name, Grid.SceneLayer.Ore);
					gameObject.SetActive(true);
					if (this.dropSpawnPositions != null && i < this.dropSpawnPositions.Length)
					{
						gameObject.transform.position = this.dropSpawnPositions[i];
					}
				}
				base.smi.sm.WasEmptied.Set(true, base.smi, false);
				this.UpdateContentPreviewSymbols();
			}
		}

		// Token: 0x0600A2FB RID: 41723 RVA: 0x003B1018 File Offset: 0x003AF218
		public void UpdateContentPreviewSymbols()
		{
			for (int i = 0; i < base.def.LootSymbols.Length; i++)
			{
				this.animController.SetSymbolVisiblity(base.def.LootSymbols[i], false);
			}
			if (this.HasContents)
			{
				for (int j = 0; j < Mathf.Min(base.def.LootSymbols.Length, base.def.ObjectsToSpawn.Length); j++)
				{
					KAnim.Build.Symbol symbolByIndex = Assets.GetPrefab(base.def.ObjectsToSpawn[j]).GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbolByIndex(0U);
					SymbolOverrideController component = base.gameObject.GetComponent<SymbolOverrideController>();
					string text = base.def.LootSymbols[j];
					component.AddSymbolOverride(text, symbolByIndex, 0);
					this.animController.SetSymbolVisiblity(text, true);
				}
			}
		}

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x0600A2FC RID: 41724 RVA: 0x003B1100 File Offset: 0x003AF300
		public string SidescreenButtonText
		{
			get
			{
				if (!this.IsOpen)
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_OpenButtonText;
					}
					return base.def.SideScreen_CancelOpenButtonText;
				}
				else
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_CloseButtonText;
					}
					return base.def.SideScreen_CancelCloseButtonText;
				}
			}
		}

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x0600A2FD RID: 41725 RVA: 0x003B1154 File Offset: 0x003AF354
		public string SidescreenButtonTooltip
		{
			get
			{
				if (!this.IsOpen)
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_OpenButtonTooltip;
					}
					return base.def.SideScreen_CancelOpenButtonTooltip;
				}
				else
				{
					if (!this.WorkOrderGiven)
					{
						return base.def.SideScreen_CloseButtonTooltip;
					}
					return base.def.SideScreen_CancelCloseButtonTooltip;
				}
			}
		}

		// Token: 0x0600A2FE RID: 41726 RVA: 0x003B11A8 File Offset: 0x003AF3A8
		public bool SidescreenEnabled()
		{
			return !this.IsOpen || base.def.CanBeClosed;
		}

		// Token: 0x0600A2FF RID: 41727 RVA: 0x003B11BF File Offset: 0x003AF3BF
		public bool SidescreenButtonInteractable()
		{
			return !this.IsOpen || base.def.CanBeClosed;
		}

		// Token: 0x0600A300 RID: 41728 RVA: 0x003B11D6 File Offset: 0x003AF3D6
		public int HorizontalGroupID()
		{
			return 0;
		}

		// Token: 0x0600A301 RID: 41729 RVA: 0x003B11D9 File Offset: 0x003AF3D9
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x0600A302 RID: 41730 RVA: 0x003B11DD File Offset: 0x003AF3DD
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600A303 RID: 41731 RVA: 0x003B11E4 File Offset: 0x003AF3E4
		public void OnSidescreenButtonPressed()
		{
			base.smi.sm.WorkOrderGiven.Set(!base.smi.sm.WorkOrderGiven.Get(base.smi), base.smi, false);
		}

		// Token: 0x04007F33 RID: 32563
		[MyCmpGet]
		private Workable workable;

		// Token: 0x04007F34 RID: 32564
		[MyCmpGet]
		private KBatchedAnimController animController;

		// Token: 0x04007F35 RID: 32565
		private Chore chore;

		// Token: 0x04007F36 RID: 32566
		private Vector3[] dropSpawnPositions;
	}
}
