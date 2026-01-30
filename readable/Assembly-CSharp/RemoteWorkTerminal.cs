using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000ACC RID: 2764
[AddComponentMenu("KMonoBehaviour/Workable/RemoteWorkTerminal")]
public class RemoteWorkTerminal : Workable
{
	// Token: 0x0600506D RID: 20589 RVA: 0x001D3420 File Offset: 0x001D1620
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_remote_terminal_kanim")
		};
		this.InitializeWorkingInteracts();
		this.synchronizeAnims = true;
		this.showProgressBar = false;
		this.workLayer = Grid.SceneLayer.BuildingUse;
		this.surpressWorkerForceSync = true;
		this.kbac.onAnimComplete += this.PlayNextWorkingAnim;
	}

	// Token: 0x0600506E RID: 20590 RVA: 0x001D348C File Offset: 0x001D168C
	private void InitializeWorkingInteracts()
	{
		if (RemoteWorkTerminal.NUM_WORKING_INTERACTS != -1)
		{
			return;
		}
		KAnimFileData data = this.overrideAnims[0].GetData();
		RemoteWorkTerminal.NUM_WORKING_INTERACTS = 0;
		for (;;)
		{
			string anim_name = string.Format("working_loop_{0}", RemoteWorkTerminal.NUM_WORKING_INTERACTS + 1);
			if (data.GetAnim(anim_name) == null)
			{
				break;
			}
			RemoteWorkTerminal.NUM_WORKING_INTERACTS++;
		}
	}

	// Token: 0x0600506F RID: 20591 RVA: 0x001D34E4 File Offset: 0x001D16E4
	public override HashedString[] GetWorkAnims(WorkerBase worker)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return RemoteWorkTerminal.hatWorkAnims;
		}
		return RemoteWorkTerminal.normalWorkAnims;
	}

	// Token: 0x06005070 RID: 20592 RVA: 0x001D3524 File Offset: 0x001D1724
	public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return RemoteWorkTerminal.hatWorkPstAnim;
		}
		return RemoteWorkTerminal.normalWorkPstAnim;
	}

	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x06005071 RID: 20593 RVA: 0x001D3562 File Offset: 0x001D1762
	// (set) Token: 0x06005072 RID: 20594 RVA: 0x001D3575 File Offset: 0x001D1775
	public RemoteWorkerDock CurrentDock
	{
		get
		{
			Ref<RemoteWorkerDock> @ref = this.dock;
			if (@ref == null)
			{
				return null;
			}
			return @ref.Get();
		}
		set
		{
			Ref<RemoteWorkerDock> @ref = this.dock;
			if (((@ref != null) ? @ref.Get() : null) != null)
			{
				this.dock.Get().StopWorking(this);
			}
			this.dock = new Ref<RemoteWorkerDock>(value);
		}
	}

	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x06005073 RID: 20595 RVA: 0x001D35AE File Offset: 0x001D17AE
	// (set) Token: 0x06005074 RID: 20596 RVA: 0x001D35C0 File Offset: 0x001D17C0
	public RemoteWorkerDock FutureDock
	{
		get
		{
			return this.future_dock ?? this.CurrentDock;
		}
		set
		{
			this.CurrentDock = value;
		}
	}

	// Token: 0x06005075 RID: 20597 RVA: 0x001D35C9 File Offset: 0x001D17C9
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.kbac.Queue(this.GetWorkingLoop(), KAnim.PlayMode.Once, 1f, 0f);
		RemoteWorkerDock currentDock = this.CurrentDock;
		if (currentDock == null)
		{
			return;
		}
		currentDock.StartWorking(this);
	}

	// Token: 0x06005076 RID: 20598 RVA: 0x001D3600 File Offset: 0x001D1800
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		RemoteWorkerDock currentDock = this.CurrentDock;
		if (currentDock == null)
		{
			return;
		}
		currentDock.StopWorking(this);
	}

	// Token: 0x06005077 RID: 20599 RVA: 0x001D361A File Offset: 0x001D181A
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return this.CurrentDock == null || this.CurrentDock.OnRemoteWorkTick(dt);
	}

	// Token: 0x06005078 RID: 20600 RVA: 0x001D3638 File Offset: 0x001D1838
	private HashedString GetWorkingLoop()
	{
		return string.Format("working_loop_{0}", UnityEngine.Random.Range(1, RemoteWorkTerminal.NUM_WORKING_INTERACTS + 1));
	}

	// Token: 0x06005079 RID: 20601 RVA: 0x001D365B File Offset: 0x001D185B
	private void PlayNextWorkingAnim(HashedString anim)
	{
		if (base.worker == null)
		{
			return;
		}
		if (base.worker.GetState() == WorkerBase.State.Working)
		{
			this.kbac.Play(this.GetWorkingLoop(), KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x040035B0 RID: 13744
	[Serialize]
	private Ref<RemoteWorkerDock> dock;

	// Token: 0x040035B1 RID: 13745
	private static int NUM_WORKING_INTERACTS = -1;

	// Token: 0x040035B2 RID: 13746
	[MyCmpReq]
	private KBatchedAnimController kbac;

	// Token: 0x040035B3 RID: 13747
	private static readonly HashedString[] normalWorkAnims = new HashedString[]
	{
		"working_pre"
	};

	// Token: 0x040035B4 RID: 13748
	private static readonly HashedString[] hatWorkAnims = new HashedString[]
	{
		"hat_pre"
	};

	// Token: 0x040035B5 RID: 13749
	private static readonly HashedString[] normalWorkPstAnim = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x040035B6 RID: 13750
	private static readonly HashedString[] hatWorkPstAnim = new HashedString[]
	{
		"working_hat_pst"
	};

	// Token: 0x040035B7 RID: 13751
	public RemoteWorkerDock future_dock;
}
