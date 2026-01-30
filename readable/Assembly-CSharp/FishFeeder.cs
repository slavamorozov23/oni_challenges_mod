using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000759 RID: 1881
public class FishFeeder : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>
{
	// Token: 0x06002FA4 RID: 12196 RVA: 0x00113000 File Offset: 0x00111200
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notoperational;
		this.root.Enter(new StateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State.Callback(FishFeeder.SetupFishFeederTopAndBot)).Exit(new StateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State.Callback(FishFeeder.CleanupFishFeederTopAndBot)).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameEvent.Callback(FishFeeder.OnStorageChange)).EventHandler(GameHashes.RefreshUserMenu, new GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameEvent.Callback(FishFeeder.OnRefreshUserMenu));
		this.notoperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.DefaultState(this.operational.on).TagTransition(GameTags.Operational, this.notoperational, true);
		this.operational.on.DoNothing();
		int num = 19;
		FishFeeder.ballSymbols = new HashedString[num];
		for (int i = 0; i < num; i++)
		{
			FishFeeder.ballSymbols[i] = "ball" + i.ToString();
		}
	}

	// Token: 0x06002FA5 RID: 12197 RVA: 0x001130F8 File Offset: 0x001112F8
	private static void SetupFishFeederTopAndBot(FishFeeder.Instance smi)
	{
		Storage storage = smi.Get<Storage>();
		smi.fishFeederTop = new FishFeeder.FishFeederTop(smi, FishFeeder.ballSymbols, storage.Capacity());
		smi.fishFeederTop.RefreshStorage();
		smi.fishFeederBot = new FishFeeder.FishFeederBot(smi, 10f, FishFeeder.ballSymbols);
		smi.fishFeederBot.RefreshStorage();
		smi.fishFeederTop.ToggleMutantSeedFetches(smi.ForbidMutantSeeds);
		smi.UpdateMutantSeedStatusItem();
	}

	// Token: 0x06002FA6 RID: 12198 RVA: 0x00113166 File Offset: 0x00111366
	private static void CleanupFishFeederTopAndBot(FishFeeder.Instance smi)
	{
		smi.fishFeederTop.Cleanup();
	}

	// Token: 0x06002FA7 RID: 12199 RVA: 0x00113174 File Offset: 0x00111374
	private static void MoveStoredContentsToConsumeOffset(FishFeeder.Instance smi)
	{
		foreach (GameObject gameObject in smi.GetComponent<Storage>().items)
		{
			if (!(gameObject == null))
			{
				FishFeeder.OnStorageChange(smi, gameObject);
			}
		}
	}

	// Token: 0x06002FA8 RID: 12200 RVA: 0x001131D8 File Offset: 0x001113D8
	private static void OnStorageChange(FishFeeder.Instance smi, object data)
	{
		if ((GameObject)data == null)
		{
			return;
		}
		smi.fishFeederTop.RefreshStorage();
		smi.fishFeederBot.RefreshStorage();
	}

	// Token: 0x06002FA9 RID: 12201 RVA: 0x00113200 File Offset: 0x00111400
	private static void OnRefreshUserMenu(FishFeeder.Instance smi, object data)
	{
		if (DlcManager.FeatureRadiationEnabled())
		{
			Game.Instance.userMenu.AddButton(smi.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", smi.ForbidMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT, delegate()
			{
				smi.ForbidMutantSeeds = !smi.ForbidMutantSeeds;
				FishFeeder.OnRefreshUserMenu(smi, null);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.FISH_FEEDER_TOOLTIP, true), 1f);
		}
	}

	// Token: 0x04001C53 RID: 7251
	public GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State notoperational;

	// Token: 0x04001C54 RID: 7252
	public FishFeeder.OperationalState operational;

	// Token: 0x04001C55 RID: 7253
	public static HashedString[] ballSymbols;

	// Token: 0x0200163D RID: 5693
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200163E RID: 5694
	public class OperationalState : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State
	{
		// Token: 0x04007425 RID: 29733
		public GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State on;
	}

	// Token: 0x0200163F RID: 5695
	public new class Instance : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameInstance
	{
		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06009668 RID: 38504 RVA: 0x0037F606 File Offset: 0x0037D806
		// (set) Token: 0x06009669 RID: 38505 RVA: 0x0037F60E File Offset: 0x0037D80E
		public bool ForbidMutantSeeds
		{
			get
			{
				return this.forbidMutantSeeds;
			}
			set
			{
				this.forbidMutantSeeds = value;
				this.fishFeederTop.ToggleMutantSeedFetches(this.forbidMutantSeeds);
				this.UpdateMutantSeedStatusItem();
			}
		}

		// Token: 0x0600966A RID: 38506 RVA: 0x0037F630 File Offset: 0x0037D830
		public Instance(IStateMachineTarget master, FishFeeder.Def def) : base(master, def)
		{
			this.mutantSeedStatusItem = new StatusItem("FISHFEEDERACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettingsDelegate));
		}

		// Token: 0x0600966B RID: 38507 RVA: 0x0037F688 File Offset: 0x0037D888
		private void OnCopySettingsDelegate(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject == null)
			{
				return;
			}
			FishFeeder.Instance smi = gameObject.GetSMI<FishFeeder.Instance>();
			if (smi == null)
			{
				return;
			}
			this.ForbidMutantSeeds = smi.ForbidMutantSeeds;
		}

		// Token: 0x0600966C RID: 38508 RVA: 0x0037F6BD File Offset: 0x0037D8BD
		public void UpdateMutantSeedStatusItem()
		{
			base.gameObject.GetComponent<KSelectable>().ToggleStatusItem(this.mutantSeedStatusItem, Game.IsDlcActiveForCurrentSave("EXPANSION1_ID") && !this.forbidMutantSeeds, null);
		}

		// Token: 0x04007426 RID: 29734
		private StatusItem mutantSeedStatusItem;

		// Token: 0x04007427 RID: 29735
		public FishFeeder.FishFeederTop fishFeederTop;

		// Token: 0x04007428 RID: 29736
		public FishFeeder.FishFeederBot fishFeederBot;

		// Token: 0x04007429 RID: 29737
		[Serialize]
		private bool forbidMutantSeeds;
	}

	// Token: 0x02001640 RID: 5696
	public class FishFeederTop : IRenderEveryTick
	{
		// Token: 0x0600966D RID: 38509 RVA: 0x0037F6EF File Offset: 0x0037D8EF
		public FishFeederTop(FishFeeder.Instance smi, HashedString[] ball_symbols, float capacity)
		{
			this.smi = smi;
			this.ballSymbols = ball_symbols;
			this.massPerBall = capacity / (float)ball_symbols.Length;
			this.FillFeeder(this.mass);
			SimAndRenderScheduler.instance.Add(this, false);
		}

		// Token: 0x0600966E RID: 38510 RVA: 0x0037F72C File Offset: 0x0037D92C
		private void FillFeeder(float mass)
		{
			KBatchedAnimController component = this.smi.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < this.ballSymbols.Length; i++)
			{
				bool is_visible = mass > (float)(i + 1) * this.massPerBall;
				component.SetSymbolVisiblity(this.ballSymbols[i], is_visible);
			}
		}

		// Token: 0x0600966F RID: 38511 RVA: 0x0037F780 File Offset: 0x0037D980
		public void RefreshStorage()
		{
			float num = 0f;
			foreach (GameObject gameObject in this.smi.GetComponent<Storage>().items)
			{
				if (!(gameObject == null))
				{
					num += gameObject.GetComponent<PrimaryElement>().Mass;
				}
			}
			this.targetMass = num;
			this.timeSinceLastBallAppeared = 0f;
		}

		// Token: 0x06009670 RID: 38512 RVA: 0x0037F808 File Offset: 0x0037DA08
		public void RenderEveryTick(float dt)
		{
			this.timeSinceLastBallAppeared += dt;
			if (Mathf.Abs(this.targetMass - this.mass) > 1f && this.timeSinceLastBallAppeared > 0.025f)
			{
				float num = Mathf.Min(this.massPerBall, this.targetMass - this.mass);
				this.mass += num;
				this.FillFeeder(this.mass);
				this.timeSinceLastBallAppeared = 0f;
			}
		}

		// Token: 0x06009671 RID: 38513 RVA: 0x0037F887 File Offset: 0x0037DA87
		public void Cleanup()
		{
			SimAndRenderScheduler.instance.Remove(this);
		}

		// Token: 0x06009672 RID: 38514 RVA: 0x0037F894 File Offset: 0x0037DA94
		public void ToggleMutantSeedFetches(bool allow)
		{
			StorageLocker component = this.smi.GetComponent<StorageLocker>();
			if (component != null)
			{
				component.UpdateForbiddenTag(GameTags.MutatedSeed, !allow);
			}
		}

		// Token: 0x0400742A RID: 29738
		private FishFeeder.Instance smi;

		// Token: 0x0400742B RID: 29739
		private float mass;

		// Token: 0x0400742C RID: 29740
		private float targetMass;

		// Token: 0x0400742D RID: 29741
		private HashedString[] ballSymbols;

		// Token: 0x0400742E RID: 29742
		private float massPerBall;

		// Token: 0x0400742F RID: 29743
		private float timeSinceLastBallAppeared;
	}

	// Token: 0x02001641 RID: 5697
	public class FishFeederBot
	{
		// Token: 0x06009673 RID: 38515 RVA: 0x0037F8C8 File Offset: 0x0037DAC8
		public FishFeederBot(FishFeeder.Instance smi, float mass_per_ball, HashedString[] ball_symbols)
		{
			this.smi = smi;
			this.massPerBall = mass_per_ball;
			this.anim = GameUtil.KInstantiate(Assets.GetPrefab("FishFeederBot"), smi.transform.GetPosition(), Grid.SceneLayer.Front, null, 0).GetComponent<KBatchedAnimController>();
			this.anim.transform.SetParent(smi.transform);
			this.anim.gameObject.SetActive(true);
			this.anim.SetSceneLayer(Grid.SceneLayer.Building);
			this.anim.Play("ball", KAnim.PlayMode.Once, 1f, 0f);
			this.anim.Stop();
			foreach (HashedString hash in ball_symbols)
			{
				this.anim.SetSymbolVisiblity(hash, false);
			}
			foreach (Storage storage in smi.gameObject.GetComponents<Storage>())
			{
				if (storage.storageID == "FishFeederBot")
				{
					this.botStorage = storage;
				}
				else if (storage.storageID == "FishFeederTop")
				{
					this.topStorage = storage;
				}
			}
			if (!this.botStorage.IsEmpty())
			{
				this.SetBallSymbol(this.botStorage.items[0].gameObject);
				this.anim.Play("ball", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06009674 RID: 38516 RVA: 0x0037FA4C File Offset: 0x0037DC4C
		public void RefreshStorage()
		{
			if (this.refreshingStorage)
			{
				return;
			}
			this.refreshingStorage = true;
			foreach (GameObject gameObject in this.botStorage.items)
			{
				if (!(gameObject == null))
				{
					int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(this.smi.transform.GetPosition())));
					gameObject.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore));
				}
			}
			if (this.botStorage.IsEmpty())
			{
				float num = 0f;
				foreach (GameObject gameObject2 in this.topStorage.items)
				{
					if (!(gameObject2 == null))
					{
						num += gameObject2.GetComponent<PrimaryElement>().Mass;
					}
				}
				if (num > 0f)
				{
					Pickupable pickupable = this.topStorage.items[0].GetComponent<Pickupable>().Take(this.massPerBall);
					this.botStorage.Store(pickupable.gameObject, false, false, true, false);
					this.SetBallSymbol(pickupable.gameObject);
					this.anim.Play("ball", KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					this.anim.SetSymbolVisiblity(FishFeeder.FishFeederBot.HASH_FEEDBALL, false);
				}
			}
			this.refreshingStorage = false;
		}

		// Token: 0x06009675 RID: 38517 RVA: 0x0037FBE8 File Offset: 0x0037DDE8
		private void SetBallSymbol(GameObject stored_go)
		{
			if (stored_go == null)
			{
				return;
			}
			this.anim.SetSymbolVisiblity(FishFeeder.FishFeederBot.HASH_FEEDBALL, true);
			KAnim.Build build = stored_go.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
			KAnim.Build.Symbol symbol = (build.GetSymbol("algae") != null) ? build.GetSymbol("algae") : build.GetSymbol("object");
			if (symbol != null)
			{
				this.anim.GetComponent<SymbolOverrideController>().AddSymbolOverride(FishFeeder.FishFeederBot.HASH_FEEDBALL, symbol, 0);
			}
			HashedString batchGroupOverride = new HashedString("FishFeeder" + stored_go.GetComponent<KPrefabID>().PrefabTag.Name);
			this.anim.SetBatchGroupOverride(batchGroupOverride);
			int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(this.smi.transform.GetPosition())));
			stored_go.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingUse));
		}

		// Token: 0x04007430 RID: 29744
		private KBatchedAnimController anim;

		// Token: 0x04007431 RID: 29745
		private Storage topStorage;

		// Token: 0x04007432 RID: 29746
		private Storage botStorage;

		// Token: 0x04007433 RID: 29747
		private bool refreshingStorage;

		// Token: 0x04007434 RID: 29748
		private FishFeeder.Instance smi;

		// Token: 0x04007435 RID: 29749
		private float massPerBall;

		// Token: 0x04007436 RID: 29750
		private static readonly HashedString HASH_FEEDBALL = "feedball";
	}
}
