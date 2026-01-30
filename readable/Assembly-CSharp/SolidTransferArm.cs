using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using FMODUnity;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000800 RID: 2048
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidTransferArm : StateMachineComponent<SolidTransferArm.SMInstance>, ISim1000ms, IRenderEveryTick
{
	// Token: 0x060036EE RID: 14062 RVA: 0x001354B0 File Offset: 0x001336B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreConsumer.AddProvider(GlobalChoreProvider.Instance);
		this.choreConsumer.SetReach(this.pickupRange);
		Klei.AI.Attributes attributes = this.GetAttributes();
		if (attributes.Get(Db.Get().Attributes.CarryAmount) == null)
		{
			attributes.Add(Db.Get().Attributes.CarryAmount);
		}
		AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, this.max_carry_weight, base.gameObject.GetProperName(), false, false, true);
		this.GetAttributes().Add(modifier);
		this.worker.usesMultiTool = false;
		this.storage.fxPrefix = Storage.FXPrefix.PickedUp;
		this.simRenderLoadBalance = false;
	}

	// Token: 0x060036EF RID: 14063 RVA: 0x00135574 File Offset: 0x00133774
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		string name = component.name + ".arm";
		this.arm_go = new GameObject(name);
		this.arm_go.SetActive(false);
		this.arm_go.transform.parent = component.transform;
		this.looping_sounds = this.arm_go.AddComponent<LoopingSounds>();
		string sound = GlobalAssets.GetSound(this.rotateSoundName, false);
		this.rotateSound = RuntimeManager.PathToEventReference(sound);
		this.arm_go.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
		this.arm_anim_ctrl = this.arm_go.AddComponent<KBatchedAnimController>();
		this.arm_anim_ctrl.AnimFiles = new KAnimFile[]
		{
			component.AnimFiles[0]
		};
		this.arm_anim_ctrl.initialAnim = "arm";
		this.arm_anim_ctrl.isMovable = true;
		this.arm_anim_ctrl.sceneLayer = Grid.SceneLayer.TransferArm;
		component.SetSymbolVisiblity("arm_target", false);
		bool flag;
		Vector3 position = component.GetSymbolTransform(new HashedString("arm_target"), out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
		this.arm_go.transform.SetPosition(position);
		this.arm_go.SetActive(true);
		this.gameCell = Grid.PosToCell(this.arm_go);
		this.link = new KAnimLink(component, this.arm_anim_ctrl);
		ChoreGroups choreGroups = Db.Get().ChoreGroups;
		for (int i = 0; i < choreGroups.Count; i++)
		{
			this.choreConsumer.SetPermittedByUser(choreGroups[i], true);
		}
		base.Subscribe<SolidTransferArm>(-592767678, SolidTransferArm.OnOperationalChangedDelegate);
		base.Subscribe<SolidTransferArm>(1745615042, SolidTransferArm.OnEndChoreDelegate);
		this.RotateArm(this.rotatable.GetRotatedOffset(Vector3.up), true, 0f);
		this.DropLeftovers();
		component.enabled = false;
		component.enabled = true;
		base.smi.StartSM();
	}

	// Token: 0x060036F0 RID: 14064 RVA: 0x0013577B File Offset: 0x0013397B
	protected override void OnCleanUp()
	{
		MinionGroupProber.Get().Vacate(this.reachableCells.ToList<int>());
		base.OnCleanUp();
	}

	// Token: 0x060036F1 RID: 14065 RVA: 0x00135798 File Offset: 0x00133998
	public static void BatchUpdate(List<UpdateBucketWithUpdater<ISim1000ms>.Entry> solid_transfer_arms, float time_delta)
	{
		SolidTransferArm.SolidTransferArmBatchUpdater.Instance.Reset(solid_transfer_arms);
		GlobalJobManager.Run(SolidTransferArm.SolidTransferArmBatchUpdater.Instance);
		SolidTransferArm.SolidTransferArmBatchUpdater.Instance.Finish();
	}

	// Token: 0x060036F2 RID: 14066 RVA: 0x001357BC File Offset: 0x001339BC
	private void Sim()
	{
		Chore.Precondition.Context context = default(Chore.Precondition.Context);
		if (this.choreConsumer.FindNextChore(ref context))
		{
			if (context.chore is FetchChore)
			{
				this.choreDriver.SetChore(context);
				FetchChore chore = context.chore as FetchChore;
				this.storage.DropUnlessMatching(chore);
				this.arm_anim_ctrl.enabled = false;
				this.arm_anim_ctrl.enabled = true;
			}
			else
			{
				bool condition = false;
				string str = "I am but a lowly transfer arm. I should only acquire FetchChores: ";
				Chore chore2 = context.chore;
				global::Debug.Assert(condition, str + ((chore2 != null) ? chore2.ToString() : null));
			}
		}
		this.operational.SetActive(this.choreDriver.HasChore(), false);
	}

	// Token: 0x060036F3 RID: 14067 RVA: 0x00135864 File Offset: 0x00133A64
	public void Sim1000ms(float dt)
	{
	}

	// Token: 0x060036F4 RID: 14068 RVA: 0x00135868 File Offset: 0x00133A68
	private void UpdateArmAnim()
	{
		FetchAreaChore fetchAreaChore = this.choreDriver.GetCurrentChore() as FetchAreaChore;
		if (this.worker.GetWorkable() && fetchAreaChore != null && this.rotation_complete)
		{
			this.StopRotateSound();
			this.SetArmAnim(fetchAreaChore.IsDelivering ? SolidTransferArm.ArmAnim.Drop : SolidTransferArm.ArmAnim.Pickup);
			return;
		}
		this.SetArmAnim(SolidTransferArm.ArmAnim.Idle);
	}

	// Token: 0x060036F5 RID: 14069 RVA: 0x001358C4 File Offset: 0x00133AC4
	private bool AsyncUpdate()
	{
		int num;
		int num2;
		Grid.CellToXY(this.gameCell, out num, out num2);
		bool flag = false;
		for (int i = num2 - this.pickupRange; i < num2 + this.pickupRange + 1; i++)
		{
			for (int j = num - this.pickupRange; j < num + this.pickupRange + 1; j++)
			{
				int num3 = Grid.XYToCell(j, i);
				if (Grid.IsValidCell(num3) && Grid.IsPhysicallyAccessible(num, num2, j, i, true) != this.reachableCells.Contains(num3))
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			ListPool<int, SolidTransferArm>.PooledList pooledList = ListPool<int, SolidTransferArm>.Allocate();
			ListPool<int, SolidTransferArm>.PooledList pooledList2 = ListPool<int, SolidTransferArm>.Allocate();
			pooledList.AddRange(this.reachableCells);
			this.reachableCells.Clear();
			for (int k = num2 - this.pickupRange; k < num2 + this.pickupRange + 1; k++)
			{
				for (int l = num - this.pickupRange; l < num + this.pickupRange + 1; l++)
				{
					int num4 = Grid.XYToCell(l, k);
					if (Grid.IsValidCell(num4) && Grid.IsPhysicallyAccessible(num, num2, l, k, true))
					{
						this.reachableCells.Add(num4);
						pooledList2.Add(num4);
					}
				}
			}
			MinionGroupProber.Get().Occupy(pooledList2);
			MinionGroupProber.Get().Vacate(pooledList);
			pooledList2.Recycle();
			pooledList.Recycle();
		}
		this.pickupables.Clear();
		GameScenePartitioner.Instance.ReadonlyVisitEntries<SolidTransferArm>(num - this.pickupRange, num2 - this.pickupRange, 2 * this.pickupRange + 1, 2 * this.pickupRange + 1, GameScenePartitioner.Instance.pickupablesLayer, SolidTransferArm.AsyncUpdateVisitor, this);
		GameScenePartitioner.Instance.ReadonlyVisitEntries<SolidTransferArm>(num - this.pickupRange, num2 - this.pickupRange, 2 * this.pickupRange + 1, 2 * this.pickupRange + 1, GameScenePartitioner.Instance.storedPickupablesLayer, SolidTransferArm.AsyncUpdateVisitor, this);
		return flag;
	}

	// Token: 0x060036F6 RID: 14070 RVA: 0x00135AA0 File Offset: 0x00133CA0
	public bool IsCellReachable(int cell)
	{
		return this.reachableCells.Contains(cell);
	}

	// Token: 0x060036F7 RID: 14071 RVA: 0x00135AAE File Offset: 0x00133CAE
	private bool IsPickupableRelevantToMyInterests(KPrefabID prefabID, int storage_cell)
	{
		return Assets.IsTagSolidTransferArmConveyable(prefabID.PrefabTag) && this.IsCellReachable(storage_cell);
	}

	// Token: 0x060036F8 RID: 14072 RVA: 0x00135AC6 File Offset: 0x00133CC6
	public Pickupable FindFetchTarget(Storage destination, FetchChore chore)
	{
		return FetchManager.FindFetchTarget(this.pickupables, destination, chore);
	}

	// Token: 0x060036F9 RID: 14073 RVA: 0x00135AD8 File Offset: 0x00133CD8
	public void RenderEveryTick(float dt)
	{
		if (this.worker.GetWorkable())
		{
			Vector3 targetPoint = this.worker.GetWorkable().GetTargetPoint();
			targetPoint.z = 0f;
			Vector3 position = base.transform.GetPosition();
			position.z = 0f;
			Vector3 target_dir = Vector3.Normalize(targetPoint - position);
			this.RotateArm(target_dir, false, dt);
		}
		this.UpdateArmAnim();
	}

	// Token: 0x060036FA RID: 14074 RVA: 0x00135B48 File Offset: 0x00133D48
	private void OnEndChore(object data)
	{
		this.DropLeftovers();
	}

	// Token: 0x060036FB RID: 14075 RVA: 0x00135B50 File Offset: 0x00133D50
	private void DropLeftovers()
	{
		if (!this.storage.IsEmpty() && !this.choreDriver.HasChore())
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x060036FC RID: 14076 RVA: 0x00135B90 File Offset: 0x00133D90
	private void SetArmAnim(SolidTransferArm.ArmAnim new_anim)
	{
		if (new_anim == this.arm_anim)
		{
			return;
		}
		this.arm_anim = new_anim;
		switch (this.arm_anim)
		{
		case SolidTransferArm.ArmAnim.Idle:
			this.arm_anim_ctrl.Play("arm", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		case SolidTransferArm.ArmAnim.Pickup:
			this.arm_anim_ctrl.Play("arm_pickup", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		case SolidTransferArm.ArmAnim.Drop:
			this.arm_anim_ctrl.Play("arm_drop", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		default:
			return;
		}
	}

	// Token: 0x060036FD RID: 14077 RVA: 0x00135C2A File Offset: 0x00133E2A
	private void OnOperationalChanged(object data)
	{
		if (!((Boxed<bool>)data).value)
		{
			if (this.choreDriver.HasChore())
			{
				this.choreDriver.StopChore();
			}
			this.UpdateArmAnim();
		}
	}

	// Token: 0x060036FE RID: 14078 RVA: 0x00135C57 File Offset: 0x00133E57
	private void SetArmRotation(float rot)
	{
		this.arm_rot = rot;
		this.arm_go.transform.rotation = Quaternion.Euler(0f, 0f, this.arm_rot);
	}

	// Token: 0x060036FF RID: 14079 RVA: 0x00135C88 File Offset: 0x00133E88
	private void RotateArm(Vector3 target_dir, bool warp, float dt)
	{
		float num = MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - this.arm_rot;
		if (num < -180f)
		{
			num += 360f;
		}
		if (num > 180f)
		{
			num -= 360f;
		}
		if (!warp)
		{
			num = Mathf.Clamp(num, -this.turn_rate * dt, this.turn_rate * dt);
		}
		this.arm_rot += num;
		this.SetArmRotation(this.arm_rot);
		this.rotation_complete = Mathf.Approximately(num, 0f);
		if (!warp && !this.rotation_complete)
		{
			if (!this.rotateSoundPlaying)
			{
				this.StartRotateSound();
			}
			this.SetRotateSoundParameter(this.arm_rot);
			return;
		}
		this.StopRotateSound();
	}

	// Token: 0x06003700 RID: 14080 RVA: 0x00135D3F File Offset: 0x00133F3F
	private void StartRotateSound()
	{
		if (!this.rotateSoundPlaying)
		{
			this.looping_sounds.StartSound(this.rotateSound);
			this.rotateSoundPlaying = true;
		}
	}

	// Token: 0x06003701 RID: 14081 RVA: 0x00135D62 File Offset: 0x00133F62
	private void SetRotateSoundParameter(float arm_rot)
	{
		if (this.rotateSoundPlaying)
		{
			this.looping_sounds.SetParameter(this.rotateSound, SolidTransferArm.HASH_ROTATION, arm_rot);
		}
	}

	// Token: 0x06003702 RID: 14082 RVA: 0x00135D83 File Offset: 0x00133F83
	private void StopRotateSound()
	{
		if (this.rotateSoundPlaying)
		{
			this.looping_sounds.StopSound(this.rotateSound);
			this.rotateSoundPlaying = false;
		}
	}

	// Token: 0x04002157 RID: 8535
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002158 RID: 8536
	[MyCmpReq]
	private KPrefabID kPrefabID;

	// Token: 0x04002159 RID: 8537
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x0400215A RID: 8538
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x0400215B RID: 8539
	[MyCmpAdd]
	private StandardWorker worker;

	// Token: 0x0400215C RID: 8540
	[MyCmpAdd]
	private ChoreConsumer choreConsumer;

	// Token: 0x0400215D RID: 8541
	[MyCmpAdd]
	private ChoreDriver choreDriver;

	// Token: 0x0400215E RID: 8542
	public int pickupRange = 4;

	// Token: 0x0400215F RID: 8543
	private float max_carry_weight = 1000f;

	// Token: 0x04002160 RID: 8544
	private List<Pickupable> pickupables = new List<Pickupable>();

	// Token: 0x04002161 RID: 8545
	private KBatchedAnimController arm_anim_ctrl;

	// Token: 0x04002162 RID: 8546
	private GameObject arm_go;

	// Token: 0x04002163 RID: 8547
	private LoopingSounds looping_sounds;

	// Token: 0x04002164 RID: 8548
	private bool rotateSoundPlaying;

	// Token: 0x04002165 RID: 8549
	private string rotateSoundName = "TransferArm_rotate";

	// Token: 0x04002166 RID: 8550
	private EventReference rotateSound;

	// Token: 0x04002167 RID: 8551
	private KAnimLink link;

	// Token: 0x04002168 RID: 8552
	private float arm_rot = 45f;

	// Token: 0x04002169 RID: 8553
	private float turn_rate = 360f;

	// Token: 0x0400216A RID: 8554
	private bool rotation_complete;

	// Token: 0x0400216B RID: 8555
	private int gameCell;

	// Token: 0x0400216C RID: 8556
	private SolidTransferArm.ArmAnim arm_anim;

	// Token: 0x0400216D RID: 8557
	private HashSet<int> reachableCells = new HashSet<int>();

	// Token: 0x0400216E RID: 8558
	private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SolidTransferArm>(delegate(SolidTransferArm component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x0400216F RID: 8559
	private static readonly EventSystem.IntraObjectHandler<SolidTransferArm> OnEndChoreDelegate = new EventSystem.IntraObjectHandler<SolidTransferArm>(delegate(SolidTransferArm component, object data)
	{
		component.OnEndChore(data);
	});

	// Token: 0x04002170 RID: 8560
	private static Func<object, SolidTransferArm, Util.IterationInstruction> AsyncUpdateVisitor = delegate(object obj, SolidTransferArm arm)
	{
		Pickupable pickupable = obj as Pickupable;
		if (Grid.GetCellRange(arm.gameCell, pickupable.cachedCell) <= arm.pickupRange && arm.IsPickupableRelevantToMyInterests(pickupable.KPrefabID, pickupable.cachedCell) && pickupable.CouldBePickedUpByTransferArm(arm.kPrefabID.InstanceID))
		{
			arm.pickupables.Add(pickupable);
		}
		return Util.IterationInstruction.Continue;
	};

	// Token: 0x04002171 RID: 8561
	private static HashedString HASH_ROTATION = "rotation";

	// Token: 0x0200177F RID: 6015
	private enum ArmAnim
	{
		// Token: 0x040077DA RID: 30682
		Idle,
		// Token: 0x040077DB RID: 30683
		Pickup,
		// Token: 0x040077DC RID: 30684
		Drop
	}

	// Token: 0x02001780 RID: 6016
	public class SMInstance : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.GameInstance
	{
		// Token: 0x06009B59 RID: 39769 RVA: 0x003948BB File Offset: 0x00392ABB
		public SMInstance(SolidTransferArm master) : base(master)
		{
		}
	}

	// Token: 0x02001781 RID: 6017
	public class States : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm>
	{
		// Token: 0x06009B5A RID: 39770 RVA: 0x003948C4 File Offset: 0x00392AC4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidTransferArm.SMInstance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidTransferArm.SMInstance smi)
			{
				smi.master.StopRotateSound();
			});
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidTransferArm.SMInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.working, (SolidTransferArm.SMInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.working.PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (SolidTransferArm.SMInstance smi) => !smi.GetComponent<Operational>().IsActive);
		}

		// Token: 0x040077DD RID: 30685
		public StateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.BoolParameter transferring;

		// Token: 0x040077DE RID: 30686
		public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State off;

		// Token: 0x040077DF RID: 30687
		public SolidTransferArm.States.ReadyStates on;

		// Token: 0x02002944 RID: 10564
		public class ReadyStates : GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State
		{
			// Token: 0x0400B68B RID: 46731
			public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State idle;

			// Token: 0x0400B68C RID: 46732
			public GameStateMachine<SolidTransferArm.States, SolidTransferArm.SMInstance, SolidTransferArm, object>.State working;
		}
	}

	// Token: 0x02001782 RID: 6018
	private class SolidTransferArmBatchUpdater : WorkItemCollection<List<UpdateBucketWithUpdater<ISim1000ms>.Entry>>
	{
		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06009B5C RID: 39772 RVA: 0x00394A2B File Offset: 0x00392C2B
		public static SolidTransferArm.SolidTransferArmBatchUpdater Instance
		{
			get
			{
				if (SolidTransferArm.SolidTransferArmBatchUpdater.instance == null)
				{
					SolidTransferArm.SolidTransferArmBatchUpdater.instance = new SolidTransferArm.SolidTransferArmBatchUpdater();
				}
				return SolidTransferArm.SolidTransferArmBatchUpdater.instance;
			}
		}

		// Token: 0x06009B5D RID: 39773 RVA: 0x00394A43 File Offset: 0x00392C43
		public void Reset(List<UpdateBucketWithUpdater<ISim1000ms>.Entry> entries)
		{
			this.sharedData = entries;
			this.count = (entries.Count + 8 - 1) / 8;
		}

		// Token: 0x06009B5E RID: 39774 RVA: 0x00394A60 File Offset: 0x00392C60
		public override void RunItem(int item, ref List<UpdateBucketWithUpdater<ISim1000ms>.Entry> shared_data, int threadIndex)
		{
			int num = item * 8;
			int num2 = Math.Min(shared_data.Count, num + 8);
			for (int i = num; i < num2; i++)
			{
				SolidTransferArm solidTransferArm = (SolidTransferArm)shared_data[i].data;
				if (solidTransferArm.operational.IsOperational)
				{
					solidTransferArm.AsyncUpdate();
				}
			}
		}

		// Token: 0x06009B5F RID: 39775 RVA: 0x00394AB4 File Offset: 0x00392CB4
		public void Finish()
		{
			foreach (UpdateBucketWithUpdater<ISim1000ms>.Entry entry in this.sharedData)
			{
				SolidTransferArm solidTransferArm = (SolidTransferArm)entry.data;
				if (solidTransferArm.operational.IsOperational)
				{
					solidTransferArm.Sim();
				}
			}
			this.Reset(SolidTransferArm.SolidTransferArmBatchUpdater.EmptyList);
		}

		// Token: 0x040077E0 RID: 30688
		private static readonly List<UpdateBucketWithUpdater<ISim1000ms>.Entry> EmptyList = new List<UpdateBucketWithUpdater<ISim1000ms>.Entry>();

		// Token: 0x040077E1 RID: 30689
		private const int kBatchSize = 8;

		// Token: 0x040077E2 RID: 30690
		private static SolidTransferArm.SolidTransferArmBatchUpdater instance;
	}

	// Token: 0x02001783 RID: 6019
	public struct CachedPickupable
	{
		// Token: 0x040077E3 RID: 30691
		public Pickupable pickupable;

		// Token: 0x040077E4 RID: 30692
		public int storage_cell;
	}
}
