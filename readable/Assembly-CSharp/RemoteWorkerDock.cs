using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000AD8 RID: 2776
[AddComponentMenu("KMonoBehaviour/Workable/RemoteWorkDock")]
public class RemoteWorkerDock : KMonoBehaviour
{
	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x060050B7 RID: 20663 RVA: 0x001D3BCD File Offset: 0x001D1DCD
	// (set) Token: 0x060050B8 RID: 20664 RVA: 0x001D3BD5 File Offset: 0x001D1DD5
	public RemoteWorkerSM RemoteWorker
	{
		get
		{
			return this.remoteWorker;
		}
		private set
		{
			this.remoteWorker = value;
			this.worker = ((value != null) ? new Ref<KSelectable>(value.GetComponent<KSelectable>()) : null);
		}
	}

	// Token: 0x060050B9 RID: 20665 RVA: 0x001D3BFB File Offset: 0x001D1DFB
	public WorkerBase GetActiveTerminalWorker()
	{
		if (this.terminal == null)
		{
			return null;
		}
		return this.terminal.worker;
	}

	// Token: 0x17000592 RID: 1426
	// (get) Token: 0x060050BA RID: 20666 RVA: 0x001D3C18 File Offset: 0x001D1E18
	public bool IsOperational
	{
		get
		{
			return this.operational.IsOperational;
		}
	}

	// Token: 0x060050BB RID: 20667 RVA: 0x001D3C28 File Offset: 0x001D1E28
	private bool canWork(IRemoteDockWorkTarget provider)
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(provider.Approachable.GetCell(), out num3, out num4);
		return num2 == num4 && Math.Abs(num - num3) <= 12;
	}

	// Token: 0x060050BC RID: 20668 RVA: 0x001D3C6D File Offset: 0x001D1E6D
	private void considerProvider(IRemoteDockWorkTarget provider)
	{
		if (this.canWork(provider))
		{
			this.providers.Add(provider);
		}
	}

	// Token: 0x060050BD RID: 20669 RVA: 0x001D3C84 File Offset: 0x001D1E84
	private void forgetProvider(IRemoteDockWorkTarget provider)
	{
		this.providers.Remove(provider);
	}

	// Token: 0x060050BE RID: 20670 RVA: 0x001D3C94 File Offset: 0x001D1E94
	private static string GenerateName()
	{
		string text = "";
		for (int i = 0; i < 3; i++)
		{
			text += "011223345789"[UnityEngine.Random.Range(0, "011223345789".Length)].ToString();
		}
		return BUILDINGS.PREFABS.REMOTEWORKERDOCK.NAME_FMT.Replace("{ID}", text);
	}

	// Token: 0x060050BF RID: 20671 RVA: 0x001D3CEC File Offset: 0x001D1EEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		UserNameable component = base.GetComponent<UserNameable>();
		if (component.savedName == "" || component.savedName == BUILDINGS.PREFABS.REMOTEWORKERDOCK.NAME)
		{
			component.SetName(RemoteWorkerDock.GenerateName());
		}
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		Components.RemoteWorkerDocks.Add(this.GetMyWorldId(), this);
		this.add_provider_binding = new Action<IRemoteDockWorkTarget>(this.considerProvider);
		this.remove_provider_binding = new Action<IRemoteDockWorkTarget>(this.forgetProvider);
		Components.RemoteDockWorkTargets.Register(this.GetMyWorldId(), this.add_provider_binding, this.remove_provider_binding);
		Ref<KSelectable> @ref = this.worker;
		RemoteWorkerSM remoteWorkerSM;
		if (@ref == null)
		{
			remoteWorkerSM = null;
		}
		else
		{
			KSelectable kselectable = @ref.Get();
			remoteWorkerSM = ((kselectable != null) ? kselectable.GetComponent<RemoteWorkerSM>() : null);
		}
		this.remoteWorker = remoteWorkerSM;
		if (this.remoteWorker == null)
		{
			this.RequestNewWorker(null);
			return;
		}
		this.remoteWorkerDestroyedEventId = this.remoteWorker.Subscribe(1969584890, new Action<object>(this.RequestNewWorker));
	}

	// Token: 0x060050C0 RID: 20672 RVA: 0x001D3E00 File Offset: 0x001D2000
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RemoteWorkerDocks.Remove(this.GetMyWorldId(), this);
		Components.RemoteDockWorkTargets.Unregister(this.GetMyWorldId(), this.add_provider_binding, this.remove_provider_binding);
		if (this.remoteWorker != null)
		{
			this.remoteWorker.Unsubscribe(this.remoteWorkerDestroyedEventId);
		}
		if (this.newRemoteWorkerHandle.IsValid)
		{
			this.newRemoteWorkerHandle.ClearScheduler();
		}
	}

	// Token: 0x060050C1 RID: 20673 RVA: 0x001D3E78 File Offset: 0x001D2078
	public void CollectChores(ChoreConsumerState duplicant_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		if (this.remoteWorker == null)
		{
			return;
		}
		ChoreConsumerState consumerState = this.remoteWorker.ConsumerState;
		consumerState.resume = duplicant_state.resume;
		foreach (IRemoteDockWorkTarget remoteDockWorkTarget in this.providers)
		{
			Chore remoteDockChore = remoteDockWorkTarget.RemoteDockChore;
			if (remoteDockChore != null)
			{
				remoteDockChore.CollectChores(consumerState, succeeded_contexts, incomplete_contexts, failed_contexts, false);
			}
		}
	}

	// Token: 0x060050C2 RID: 20674 RVA: 0x001D3F00 File Offset: 0x001D2100
	public bool AvailableForWorkBy(RemoteWorkTerminal terminal)
	{
		return this.terminal == null || this.terminal == terminal;
	}

	// Token: 0x060050C3 RID: 20675 RVA: 0x001D3F1E File Offset: 0x001D211E
	public bool HasWorker()
	{
		return this.remoteWorker != null;
	}

	// Token: 0x060050C4 RID: 20676 RVA: 0x001D3F2C File Offset: 0x001D212C
	public void SetNextChore(RemoteWorkTerminal terminal, Chore.Precondition.Context chore_context)
	{
		global::Debug.Assert(this.worker != null);
		global::Debug.Assert(this.terminal == null || this.terminal == terminal);
		this.terminal = terminal;
		if (this.remoteWorker != null)
		{
			this.remoteWorker.SetNextChore(chore_context);
		}
	}

	// Token: 0x060050C5 RID: 20677 RVA: 0x001D3F8C File Offset: 0x001D218C
	public bool StartWorking(RemoteWorkTerminal terminal)
	{
		if (this.terminal == null)
		{
			this.terminal = terminal;
		}
		if (this.terminal == terminal && this.remoteWorker != null)
		{
			this.remoteWorker.ActivelyControlled = true;
			return true;
		}
		return false;
	}

	// Token: 0x060050C6 RID: 20678 RVA: 0x001D3FD9 File Offset: 0x001D21D9
	public void StopWorking(RemoteWorkTerminal terminal)
	{
		if (terminal == this.terminal)
		{
			this.terminal = null;
			if (this.remoteWorker != null)
			{
				this.remoteWorker.ActivelyControlled = false;
			}
		}
	}

	// Token: 0x060050C7 RID: 20679 RVA: 0x001D400A File Offset: 0x001D220A
	public bool OnRemoteWorkTick(float dt)
	{
		return this.remoteWorker == null || (!this.remoteWorker.ActivelyWorking && !this.remoteWorker.HasChoreQueued());
	}

	// Token: 0x060050C8 RID: 20680 RVA: 0x001D4039 File Offset: 0x001D2239
	private void OnStorageChanged(object _)
	{
		if (this.remoteWorker == null || this.worker.Get() == null)
		{
			this.RequestNewWorker(null);
		}
	}

	// Token: 0x060050C9 RID: 20681 RVA: 0x001D4064 File Offset: 0x001D2264
	private void RequestNewWorker(object _ = null)
	{
		if (this.newRemoteWorkerHandle.IsValid)
		{
			return;
		}
		Tag build_MATERIAL_TAG = RemoteWorkerConfig.BUILD_MATERIAL_TAG;
		if (this.storage.FindFirstWithMass(build_MATERIAL_TAG, 200f) == null)
		{
			if (!this.activeFetch)
			{
				this.activeFetch = true;
				FetchList2 fetchList = new FetchList2(this.storage, Db.Get().ChoreTypes.Fetch);
				fetchList.Add(build_MATERIAL_TAG, null, 200f, Operational.State.None);
				fetchList.Submit(delegate
				{
					this.activeFetch = false;
					this.RequestNewWorker(null);
				}, true);
				return;
			}
		}
		else
		{
			this.MakeNewWorker(null);
		}
	}

	// Token: 0x060050CA RID: 20682 RVA: 0x001D40F0 File Offset: 0x001D22F0
	private void MakeNewWorker(object _ = null)
	{
		if (this.newRemoteWorkerHandle.IsValid)
		{
			return;
		}
		if (this.storage.GetAmountAvailable(RemoteWorkerConfig.BUILD_MATERIAL_TAG) < 200f)
		{
			return;
		}
		PrimaryElement elem = this.storage.FindFirstWithMass(RemoteWorkerConfig.BUILD_MATERIAL_TAG, 200f);
		if (elem == null)
		{
			return;
		}
		float temperature;
		SimUtil.DiseaseInfo disease;
		float num;
		this.storage.ConsumeAndGetDisease(elem.ElementID.CreateTag(), 200f, out num, out disease, out temperature);
		this.status_item_handle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.RemoteWorkDockMakingWorker, null);
		this.newRemoteWorkerHandle = GameScheduler.Instance.Schedule("MakeRemoteWorker", 2f, delegate(object _)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(RemoteWorkerConfig.ID), this.transform.position, Grid.SceneLayer.Creatures, null, 0);
			if (this.remoteWorkerDestroyedEventId != -1 && this.remoteWorker != null)
			{
				this.remoteWorker.Unsubscribe(this.remoteWorkerDestroyedEventId);
			}
			this.RemoteWorker = gameObject.GetComponent<RemoteWorkerSM>();
			this.remoteWorker.HomeDepot = this;
			this.remoteWorker.playNewWorker = true;
			gameObject.SetActive(true);
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ElementID = elem.ElementID;
			component.Temperature = temperature;
			if (disease.idx != 255)
			{
				component.AddDisease(disease.idx, disease.count, "Inherited from construction material");
			}
			this.remoteWorkerDestroyedEventId = gameObject.Subscribe(1969584890, new Action<object>(this.RequestNewWorker));
			this.newRemoteWorkerHandle.ClearScheduler();
			this.GetComponent<KSelectable>().RemoveStatusItem(this.status_item_handle, false);
		}, null, null);
	}

	// Token: 0x040035DB RID: 13787
	[Serialize]
	protected Ref<KSelectable> worker;

	// Token: 0x040035DC RID: 13788
	protected RemoteWorkerSM remoteWorker;

	// Token: 0x040035DD RID: 13789
	private int remoteWorkerDestroyedEventId = -1;

	// Token: 0x040035DE RID: 13790
	protected RemoteWorkTerminal terminal;

	// Token: 0x040035DF RID: 13791
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040035E0 RID: 13792
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040035E1 RID: 13793
	[MyCmpAdd]
	private UserNameable nameable;

	// Token: 0x040035E2 RID: 13794
	[MyCmpAdd]
	private RemoteWorkerDock.NewWorker new_worker_;

	// Token: 0x040035E3 RID: 13795
	[MyCmpAdd]
	private RemoteWorkerDock.EnterableDock enter_;

	// Token: 0x040035E4 RID: 13796
	[MyCmpAdd]
	private RemoteWorkerDock.ExitableDock exit_;

	// Token: 0x040035E5 RID: 13797
	[MyCmpAdd]
	private RemoteWorkerDock.WorkerRecharger recharger_;

	// Token: 0x040035E6 RID: 13798
	[MyCmpAdd]
	private RemoteWorkerDock.WorkerGunkRemover gunk_remover_;

	// Token: 0x040035E7 RID: 13799
	[MyCmpAdd]
	private RemoteWorkerDock.WorkerOilRefiller oil_refiller_;

	// Token: 0x040035E8 RID: 13800
	private Guid status_item_handle;

	// Token: 0x040035E9 RID: 13801
	private SchedulerHandle newRemoteWorkerHandle;

	// Token: 0x040035EA RID: 13802
	private List<IRemoteDockWorkTarget> providers = new List<IRemoteDockWorkTarget>();

	// Token: 0x040035EB RID: 13803
	private Action<IRemoteDockWorkTarget> add_provider_binding;

	// Token: 0x040035EC RID: 13804
	private Action<IRemoteDockWorkTarget> remove_provider_binding;

	// Token: 0x040035ED RID: 13805
	private bool activeFetch;

	// Token: 0x02001C1B RID: 7195
	public class NewWorker : Workable
	{
		// Token: 0x0600AC87 RID: 44167 RVA: 0x003CCE20 File Offset: 0x003CB020
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.workAnims = RemoteWorkerDock.NewWorker.WORK_ANIMS;
			this.workingPstComplete = null;
			this.workingPstFailed = null;
			this.workAnimPlayMode = KAnim.PlayMode.Once;
			this.synchronizeAnims = true;
			this.triggerWorkReactions = false;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.resetProgressOnStop = true;
			KAnim.Anim anim = Assets.GetAnim(RemoteWorkerConfig.DOCK_ANIM_OVERRIDES).GetData().GetAnim("new_worker");
			base.SetWorkTime((float)anim.numFrames / anim.frameRate);
		}

		// Token: 0x0600AC88 RID: 44168 RVA: 0x003CCEA3 File Offset: 0x003CB0A3
		protected override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
		}

		// Token: 0x0600AC89 RID: 44169 RVA: 0x003CCEAC File Offset: 0x003CB0AC
		protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
			worker.GetComponent<RemoteWorkerSM>().Docked = true;
		}

		// Token: 0x040086E9 RID: 34537
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"new_worker"
		};
	}

	// Token: 0x02001C1C RID: 7196
	public class EnterableDock : Workable
	{
		// Token: 0x0600AC8C RID: 44172 RVA: 0x003CCEE8 File Offset: 0x003CB0E8
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.workerStatusItem = Db.Get().DuplicantStatusItems.EnteringDock;
			this.workAnims = RemoteWorkerDock.EnterableDock.WORK_ANIMS;
			this.workingPstComplete = null;
			this.workingPstFailed = null;
			this.workAnimPlayMode = KAnim.PlayMode.Once;
			this.synchronizeAnims = true;
			this.triggerWorkReactions = false;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.resetProgressOnStop = true;
			KAnim.Anim anim = Assets.GetAnim(RemoteWorkerConfig.DOCK_ANIM_OVERRIDES).GetData().GetAnim("enter_dock");
			base.SetWorkTime((float)anim.numFrames / anim.frameRate);
		}

		// Token: 0x0600AC8D RID: 44173 RVA: 0x003CCF80 File Offset: 0x003CB180
		protected override void OnCompleteWork(WorkerBase worker)
		{
			worker.GetComponent<RemoteWorkerSM>().Docked = true;
			base.OnCompleteWork(worker);
		}

		// Token: 0x040086EA RID: 34538
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"enter_dock"
		};
	}

	// Token: 0x02001C1D RID: 7197
	public class ExitableDock : Workable
	{
		// Token: 0x0600AC90 RID: 44176 RVA: 0x003CCFBC File Offset: 0x003CB1BC
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.workAnims = RemoteWorkerDock.ExitableDock.WORK_ANIMS;
			this.workingPstComplete = null;
			this.workingPstFailed = null;
			this.workAnimPlayMode = KAnim.PlayMode.Once;
			this.synchronizeAnims = true;
			this.triggerWorkReactions = false;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.resetProgressOnStop = true;
			KAnim.Anim anim = Assets.GetAnim(RemoteWorkerConfig.DOCK_ANIM_OVERRIDES).GetData().GetAnim("exit_dock");
			base.SetWorkTime((float)anim.numFrames / anim.frameRate);
		}

		// Token: 0x0600AC91 RID: 44177 RVA: 0x003CD03F File Offset: 0x003CB23F
		protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
			worker.GetComponent<RemoteWorkerSM>().Docked = false;
		}

		// Token: 0x040086EB RID: 34539
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"exit_dock"
		};
	}

	// Token: 0x02001C1E RID: 7198
	public class WorkerRecharger : Workable
	{
		// Token: 0x0600AC94 RID: 44180 RVA: 0x003CD07C File Offset: 0x003CB27C
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.workAnims = RemoteWorkerDock.WorkerRecharger.WORK_ANIMS;
			this.workingPstComplete = RemoteWorkerDock.WorkerRecharger.WORK_PST_ANIM;
			this.workingPstFailed = RemoteWorkerDock.WorkerRecharger.WORK_PST_ANIM;
			this.synchronizeAnims = true;
			this.triggerWorkReactions = false;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerRecharging;
			base.SetWorkTime(float.PositiveInfinity);
		}

		// Token: 0x0600AC95 RID: 44181 RVA: 0x003CD0E8 File Offset: 0x003CB2E8
		protected override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
			RemoteWorkerCapacitor component = worker.GetComponent<RemoteWorkerCapacitor>();
			this.progress = ((component != null) ? component.ChargeRatio : 0f);
			if (this.progressBar != null)
			{
				this.progressBar.SetUpdateFunc(() => this.progress);
			}
		}

		// Token: 0x0600AC96 RID: 44182 RVA: 0x003CD144 File Offset: 0x003CB344
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			base.OnWorkTick(worker, dt);
			RemoteWorkerCapacitor component = worker.GetComponent<RemoteWorkerCapacitor>();
			if (component != null)
			{
				this.progress = component.ChargeRatio;
				return component.ApplyDeltaEnergy(7.5f * dt) == 0f;
			}
			return true;
		}

		// Token: 0x040086EC RID: 34540
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"recharge_pre",
			"recharge_loop"
		};

		// Token: 0x040086ED RID: 34541
		private static readonly HashedString[] WORK_PST_ANIM = new HashedString[]
		{
			"recharge_pst"
		};

		// Token: 0x040086EE RID: 34542
		private float progress;
	}

	// Token: 0x02001C1F RID: 7199
	public class WorkerGunkRemover : Workable
	{
		// Token: 0x0600AC9A RID: 44186 RVA: 0x003CD1F4 File Offset: 0x003CB3F4
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_remote_work_dock_kanim")
			};
			this.workAnims = RemoteWorkerDock.WorkerGunkRemover.WORK_ANIMS;
			this.workingPstComplete = RemoteWorkerDock.WorkerGunkRemover.WORK_PST_ANIM;
			this.workingPstFailed = RemoteWorkerDock.WorkerGunkRemover.WORK_PST_ANIM;
			this.synchronizeAnims = true;
			this.triggerWorkReactions = false;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerDraining;
			base.SetWorkTime(float.PositiveInfinity);
		}

		// Token: 0x0600AC9B RID: 44187 RVA: 0x003CD27C File Offset: 0x003CB47C
		protected override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
			Storage component = worker.GetComponent<Storage>();
			if (component != null)
			{
				this.progress = 1f - component.GetMassAvailable(SimHashes.LiquidGunk) / 20.000002f;
			}
			if (this.progressBar != null)
			{
				this.progressBar.SetUpdateFunc(() => this.progress);
			}
		}

		// Token: 0x0600AC9C RID: 44188 RVA: 0x003CD2E4 File Offset: 0x003CB4E4
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			base.OnWorkTick(worker, dt);
			Storage component = worker.GetComponent<Storage>();
			if (component != null)
			{
				float massAvailable = component.GetMassAvailable(SimHashes.LiquidGunk);
				float num = Math.Min(massAvailable, 3.3333337f * dt);
				this.progress = 1f - massAvailable / 20.000002f;
				if (num > 0f)
				{
					component.TransferMass(this.storage, SimHashes.LiquidGunk.CreateTag(), num, false, false, true);
					return false;
				}
			}
			return true;
		}

		// Token: 0x040086EF RID: 34543
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"drain_gunk_pre",
			"drain_gunk_loop"
		};

		// Token: 0x040086F0 RID: 34544
		private static readonly HashedString[] WORK_PST_ANIM = new HashedString[]
		{
			"drain_gunk_pst"
		};

		// Token: 0x040086F1 RID: 34545
		[MyCmpGet]
		private Storage storage;

		// Token: 0x040086F2 RID: 34546
		private float progress;
	}

	// Token: 0x02001C20 RID: 7200
	public class WorkerOilRefiller : Workable
	{
		// Token: 0x0600ACA0 RID: 44192 RVA: 0x003CD3C8 File Offset: 0x003CB5C8
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_remote_work_dock_kanim")
			};
			this.workAnims = RemoteWorkerDock.WorkerOilRefiller.WORK_ANIMS;
			this.workingPstComplete = RemoteWorkerDock.WorkerOilRefiller.WORK_PST_ANIM;
			this.workingPstFailed = RemoteWorkerDock.WorkerOilRefiller.WORK_PST_ANIM;
			this.synchronizeAnims = true;
			this.triggerWorkReactions = false;
			this.workLayer = Grid.SceneLayer.BuildingUse;
			this.workerStatusItem = Db.Get().DuplicantStatusItems.RemoteWorkerOiling;
			base.SetWorkTime(float.PositiveInfinity);
		}

		// Token: 0x0600ACA1 RID: 44193 RVA: 0x003CD450 File Offset: 0x003CB650
		protected override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
			Storage component = worker.GetComponent<Storage>();
			if (component != null)
			{
				float massAvailable = component.GetMassAvailable(GameTags.LubricatingOil);
				this.progress = massAvailable / 20.000002f;
			}
			if (this.progressBar != null)
			{
				this.progressBar.SetUpdateFunc(() => this.progress);
			}
		}

		// Token: 0x0600ACA2 RID: 44194 RVA: 0x003CD4B4 File Offset: 0x003CB6B4
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			base.OnWorkTick(worker, dt);
			Storage component = worker.GetComponent<Storage>();
			if (component != null)
			{
				float massAvailable = component.GetMassAvailable(GameTags.LubricatingOil);
				float num = Math.Min(20.000002f - massAvailable, 2.5000002f * dt);
				this.progress = massAvailable / 20.000002f;
				if (num > 0f)
				{
					this.storage.TransferMass(component, GameTags.LubricatingOil, num, false, false, true);
					return false;
				}
			}
			return true;
		}

		// Token: 0x040086F3 RID: 34547
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"oil_pre",
			"oil_loop"
		};

		// Token: 0x040086F4 RID: 34548
		private static readonly HashedString[] WORK_PST_ANIM = new HashedString[]
		{
			"oil_pst"
		};

		// Token: 0x040086F5 RID: 34549
		[MyCmpGet]
		private Storage storage;

		// Token: 0x040086F6 RID: 34550
		private float progress;
	}
}
