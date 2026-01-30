using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020007B9 RID: 1977
[AddComponentMenu("KMonoBehaviour/Workable/MessStation")]
public class MessStation : Workable, IGameObjectEffectDescriptor, IDiningSeat
{
	// Token: 0x06003440 RID: 13376 RVA: 0x001284BA File Offset: 0x001266BA
	protected override void OnPrefabInit()
	{
		this.ownable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.HasCaloriesOwnablePrecondition));
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim(MessStation.eatAnim)
		};
	}

	// Token: 0x06003441 RID: 13377 RVA: 0x001284F4 File Offset: 0x001266F4
	public static bool CanBeAssignedTo(IAssignableIdentity assignee)
	{
		MinionAssignablesProxy minionAssignablesProxy = assignee as MinionAssignablesProxy;
		if (minionAssignablesProxy == null)
		{
			return false;
		}
		MinionIdentity minionIdentity = minionAssignablesProxy.target as MinionIdentity;
		return !(minionIdentity == null) && (Db.Get().Amounts.Calories.Lookup(minionIdentity) != null || (Game.IsDlcActiveForCurrentSave("DLC3_ID") && minionIdentity.model == BionicMinionConfig.MODEL));
	}

	// Token: 0x06003442 RID: 13378 RVA: 0x00128561 File Offset: 0x00126761
	private bool HasCaloriesOwnablePrecondition(MinionAssignablesProxy worker)
	{
		return MessStation.CanBeAssignedTo(worker);
	}

	// Token: 0x06003443 RID: 13379 RVA: 0x00128569 File Offset: 0x00126769
	protected override void OnCompleteWork(WorkerBase worker)
	{
		worker.GetWorkable().GetComponent<Edible>().CompleteWork(worker);
	}

	// Token: 0x06003444 RID: 13380 RVA: 0x0012857C File Offset: 0x0012677C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new MessStation.MessStationSM.Instance(this);
		this.smi.StartSM();
	}

	// Token: 0x06003445 RID: 13381 RVA: 0x0012859C File Offset: 0x0012679C
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (go.GetComponent<Storage>().Has(TableSaltConfig.ID.ToTag()))
		{
			list.Add(MessStation.TABLE_SALT_DESCRIPTOR);
		}
		return list;
	}

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x06003446 RID: 13382 RVA: 0x001285D2 File Offset: 0x001267D2
	public bool HasSalt
	{
		get
		{
			return this.smi.HasSalt;
		}
	}

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06003447 RID: 13383 RVA: 0x001285DF File Offset: 0x001267DF
	public HashedString EatAnim
	{
		get
		{
			return MessStation.eatAnim;
		}
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06003448 RID: 13384 RVA: 0x001285E6 File Offset: 0x001267E6
	public HashedString ReloadElectrobankAnim
	{
		get
		{
			return MessStation.reloadElectrobankAnim;
		}
	}

	// Token: 0x06003449 RID: 13385 RVA: 0x001285ED File Offset: 0x001267ED
	public Storage FindStorage()
	{
		return base.GetComponent<Storage>();
	}

	// Token: 0x0600344A RID: 13386 RVA: 0x001285F5 File Offset: 0x001267F5
	public Operational FindOperational()
	{
		return base.GetComponent<Operational>();
	}

	// Token: 0x17000344 RID: 836
	// (get) Token: 0x0600344B RID: 13387 RVA: 0x001285FD File Offset: 0x001267FD
	// (set) Token: 0x0600344C RID: 13388 RVA: 0x00128605 File Offset: 0x00126805
	public KPrefabID Diner { get; set; }

	// Token: 0x04001F8C RID: 8076
	public static readonly Descriptor TABLE_SALT_DESCRIPTOR = new Descriptor(string.Format(UI.BUILDINGEFFECTS.MESS_TABLE_SALT, TableSaltTuning.MORALE_MODIFIER), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MESS_TABLE_SALT, TableSaltTuning.MORALE_MODIFIER), Descriptor.DescriptorType.Effect, false);

	// Token: 0x04001F8D RID: 8077
	[MyCmpGet]
	private Ownable ownable;

	// Token: 0x04001F8E RID: 8078
	private MessStation.MessStationSM.Instance smi;

	// Token: 0x04001F8F RID: 8079
	public static readonly HashedString eatAnim = "anim_eat_table_kanim";

	// Token: 0x04001F90 RID: 8080
	public static readonly HashedString reloadElectrobankAnim = "anim_bionic_eat_table_kanim";

	// Token: 0x020016EF RID: 5871
	public class MessStationSM : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation>
	{
		// Token: 0x06009934 RID: 39220 RVA: 0x0038C050 File Offset: 0x0038A250
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.salt.none;
			this.salt.none.Transition(this.salt.salty, (MessStation.MessStationSM.Instance smi) => smi.HasSalt, UpdateRate.SIM_200ms).PlayAnim("off");
			this.salt.salty.Transition(this.salt.none, (MessStation.MessStationSM.Instance smi) => !smi.HasSalt, UpdateRate.SIM_200ms).PlayAnim("salt").EventTransition(GameHashes.EatStart, this.eating, null);
			this.eating.Transition(this.salt.salty, (MessStation.MessStationSM.Instance smi) => smi.HasSalt && !smi.IsEating(), UpdateRate.SIM_200ms).Transition(this.salt.none, (MessStation.MessStationSM.Instance smi) => !smi.HasSalt && !smi.IsEating(), UpdateRate.SIM_200ms).PlayAnim("off");
		}

		// Token: 0x04007639 RID: 30265
		public MessStation.MessStationSM.SaltState salt;

		// Token: 0x0400763A RID: 30266
		public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State eating;

		// Token: 0x02002919 RID: 10521
		public class SaltState : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State
		{
			// Token: 0x0400B5B0 RID: 46512
			public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State none;

			// Token: 0x0400B5B1 RID: 46513
			public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State salty;
		}

		// Token: 0x0200291A RID: 10522
		public new class Instance : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.GameInstance
		{
			// Token: 0x0600CF56 RID: 53078 RVA: 0x00433A2D File Offset: 0x00431C2D
			public Instance(MessStation master) : base(master)
			{
				this.saltStorage = master.GetComponent<Storage>();
				this.reservable = master.GetComponent<Reservable>();
			}

			// Token: 0x17000D6E RID: 3438
			// (get) Token: 0x0600CF57 RID: 53079 RVA: 0x00433A4E File Offset: 0x00431C4E
			public bool HasSalt
			{
				get
				{
					return this.saltStorage.Has(TableSaltConfig.ID.ToTag());
				}
			}

			// Token: 0x0600CF58 RID: 53080 RVA: 0x00433A68 File Offset: 0x00431C68
			public bool IsEating()
			{
				if (this.reservable == null)
				{
					return false;
				}
				if (this.reservable.ReservedBy == null)
				{
					return false;
				}
				ChoreDriver choreDriver;
				if (!this.reservable.ReservedBy.TryGetComponent<ChoreDriver>(out choreDriver))
				{
					return false;
				}
				if (!choreDriver.HasChore())
				{
					return false;
				}
				ReloadElectrobankChore reloadElectrobankChore = choreDriver.GetCurrentChore() as ReloadElectrobankChore;
				if (reloadElectrobankChore != null)
				{
					return reloadElectrobankChore.IsInstallingAtMessStation();
				}
				return choreDriver.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
			}

			// Token: 0x0400B5B2 RID: 46514
			private Storage saltStorage;

			// Token: 0x0400B5B3 RID: 46515
			private Reservable reservable;
		}
	}
}
