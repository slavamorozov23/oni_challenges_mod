using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020008C6 RID: 2246
public class CryoTank : StateMachineComponent<CryoTank.StatesInstance>, ISidescreenButtonControl
{
	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x06003DF5 RID: 15861 RVA: 0x00159A7F File Offset: 0x00157C7F
	public string SidescreenButtonText
	{
		get
		{
			return BUILDINGS.PREFABS.CRYOTANK.DEFROSTBUTTON;
		}
	}

	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x06003DF6 RID: 15862 RVA: 0x00159A8B File Offset: 0x00157C8B
	public string SidescreenButtonTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.CRYOTANK.DEFROSTBUTTONTOOLTIP;
		}
	}

	// Token: 0x06003DF7 RID: 15863 RVA: 0x00159A97 File Offset: 0x00157C97
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06003DF8 RID: 15864 RVA: 0x00159A9A File Offset: 0x00157C9A
	public void OnSidescreenButtonPressed()
	{
		this.OnClickOpen();
	}

	// Token: 0x06003DF9 RID: 15865 RVA: 0x00159AA2 File Offset: 0x00157CA2
	public bool SidescreenButtonInteractable()
	{
		return this.HasDefrostedFriend();
	}

	// Token: 0x06003DFA RID: 15866 RVA: 0x00159AAA File Offset: 0x00157CAA
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x06003DFB RID: 15867 RVA: 0x00159AAE File Offset: 0x00157CAE
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003DFC RID: 15868 RVA: 0x00159AB5 File Offset: 0x00157CB5
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06003DFD RID: 15869 RVA: 0x00159AB8 File Offset: 0x00157CB8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		Demolishable component = base.GetComponent<Demolishable>();
		if (component != null)
		{
			component.allowDemolition = !this.HasDefrostedFriend();
		}
	}

	// Token: 0x06003DFE RID: 15870 RVA: 0x00159AF5 File Offset: 0x00157CF5
	public bool HasDefrostedFriend()
	{
		return base.smi.IsInsideState(base.smi.sm.closed) && this.chore == null;
	}

	// Token: 0x06003DFF RID: 15871 RVA: 0x00159B20 File Offset: 0x00157D20
	public void DropContents()
	{
		MinionStartingStats minionStartingStats = new MinionStartingStats(GameTags.Minions.Models.Standard, false, null, "AncientKnowledge", false);
		GameObject prefab = Assets.GetPrefab(BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
		GameObject gameObject = Util.KInstantiate(prefab, null, null);
		gameObject.name = prefab.name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(base.transform.position), this.dropOffset), Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position);
		gameObject.SetActive(true);
		minionStartingStats.Apply(gameObject);
		gameObject.GetComponent<MinionIdentity>().arrivalTime = (float)UnityEngine.Random.Range(-2000, -1000);
		MinionResume component = gameObject.GetComponent<MinionResume>();
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			component.ForceAddSkillPoint();
		}
		base.smi.sm.defrostedDuplicant.Set(gameObject, base.smi, false);
		gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
		ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
		if (component2 != null)
		{
			base.smi.defrostAnimChore = new EmoteChore(component2, Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_cryo_chamber_kanim", new HashedString[]
			{
				"defrost",
				"defrost_exit"
			}, KAnim.PlayMode.Once, false);
			Vector3 position2 = gameObject.transform.GetPosition();
			position2.z = Grid.GetLayerZ(Grid.SceneLayer.Gas);
			gameObject.transform.SetPosition(position2);
			gameObject.GetMyWorld().SetDupeVisited();
		}
		SaveGame.Instance.ColonyAchievementTracker.defrostedDuplicant = true;
	}

	// Token: 0x06003E00 RID: 15872 RVA: 0x00159CCC File Offset: 0x00157ECC
	public void ShowEventPopup()
	{
		GameObject gameObject = base.smi.sm.defrostedDuplicant.Get(base.smi);
		if (this.opener != null && gameObject != null)
		{
			SimpleEvent.StatesInstance statesInstance = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.CryoFriend, -1, null).smi as SimpleEvent.StatesInstance;
			statesInstance.minions = new GameObject[]
			{
				gameObject,
				this.opener
			};
			statesInstance.SetTextParameter("dupe", this.opener.GetProperName());
			statesInstance.SetTextParameter("friend", gameObject.GetProperName());
			statesInstance.ShowEventPopup();
		}
	}

	// Token: 0x06003E01 RID: 15873 RVA: 0x00159D78 File Offset: 0x00157F78
	public void Cheer()
	{
		GameObject gameObject = base.smi.sm.defrostedDuplicant.Get(base.smi);
		if (this.opener != null && gameObject != null)
		{
			Db db = Db.Get();
			this.opener.GetComponent<Effects>().Add(Db.Get().effects.Get("CryoFriend"), true);
			new EmoteChore(this.opener.GetComponent<Effects>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 1, null);
			gameObject.GetComponent<Effects>().Add(Db.Get().effects.Get("CryoFriend"), true);
			new EmoteChore(gameObject.GetComponent<Effects>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 1, null);
		}
	}

	// Token: 0x06003E02 RID: 15874 RVA: 0x00159E62 File Offset: 0x00158062
	private void OnClickOpen()
	{
		this.ActivateChore(null);
	}

	// Token: 0x06003E03 RID: 15875 RVA: 0x00159E6B File Offset: 0x0015806B
	private void OnClickCancel()
	{
		this.CancelActivateChore(null);
	}

	// Token: 0x06003E04 RID: 15876 RVA: 0x00159E74 File Offset: 0x00158074
	public void ActivateChore(object param = null)
	{
		if (this.chore != null)
		{
			return;
		}
		base.GetComponent<Workable>().SetWorkTime(1.5f);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, delegate(Chore o)
		{
			this.CompleteActivateChore();
		}, null, null, true, null, false, true, Assets.GetAnim(this.overrideAnim), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x06003E05 RID: 15877 RVA: 0x00159EE0 File Offset: 0x001580E0
	public void CancelActivateChore(object param = null)
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x06003E06 RID: 15878 RVA: 0x00159F04 File Offset: 0x00158104
	private void CompleteActivateChore()
	{
		this.opener = this.chore.driver.gameObject;
		base.smi.GoTo(base.smi.sm.open);
		this.chore = null;
		Demolishable component = base.smi.GetComponent<Demolishable>();
		if (component != null)
		{
			component.allowDemolition = true;
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x0400264F RID: 9807
	public string[][] possible_contents_ids;

	// Token: 0x04002650 RID: 9808
	public string machineSound;

	// Token: 0x04002651 RID: 9809
	public string overrideAnim;

	// Token: 0x04002652 RID: 9810
	public CellOffset dropOffset = CellOffset.none;

	// Token: 0x04002653 RID: 9811
	private GameObject opener;

	// Token: 0x04002654 RID: 9812
	private Chore chore;

	// Token: 0x020018E1 RID: 6369
	public class StatesInstance : GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.GameInstance
	{
		// Token: 0x0600A0A8 RID: 41128 RVA: 0x003AA2F7 File Offset: 0x003A84F7
		public StatesInstance(CryoTank master) : base(master)
		{
		}

		// Token: 0x04007C55 RID: 31829
		public Chore defrostAnimChore;
	}

	// Token: 0x020018E2 RID: 6370
	public class States : GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank>
	{
		// Token: 0x0600A0A9 RID: 41129 RVA: 0x003AA300 File Offset: 0x003A8500
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.closed;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.closed.PlayAnim("on").Enter(delegate(CryoTank.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StartSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
			this.open.GoTo(this.defrost).Exit(delegate(CryoTank.StatesInstance smi)
			{
				smi.master.DropContents();
			});
			this.defrost.PlayAnim("defrost").OnAnimQueueComplete(this.defrostExit).Update(delegate(CryoTank.StatesInstance smi, float dt)
			{
				smi.sm.defrostedDuplicant.Get(smi).GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(CryoTank.StatesInstance smi)
			{
				smi.master.ShowEventPopup();
			});
			this.defrostExit.PlayAnim("defrost_exit").Update(delegate(CryoTank.StatesInstance smi, float dt)
			{
				if (smi.defrostAnimChore == null || smi.defrostAnimChore.isComplete)
				{
					smi.GoTo(this.off);
				}
			}, UpdateRate.SIM_200ms, false).Exit(delegate(CryoTank.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.defrostedDuplicant.Get(smi);
				if (gameObject != null)
				{
					gameObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Move);
					smi.master.Cheer();
				}
			});
			this.off.PlayAnim("off").Enter(delegate(CryoTank.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StopSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
		}

		// Token: 0x04007C56 RID: 31830
		public StateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.TargetParameter defrostedDuplicant;

		// Token: 0x04007C57 RID: 31831
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State closed;

		// Token: 0x04007C58 RID: 31832
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State open;

		// Token: 0x04007C59 RID: 31833
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State defrost;

		// Token: 0x04007C5A RID: 31834
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State defrostExit;

		// Token: 0x04007C5B RID: 31835
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State off;
	}
}
