using System;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x0200070C RID: 1804
[SerializationConfig(MemberSerialization.OptIn)]
public class AutoMiner : StateMachineComponent<AutoMiner.Instance>, ISim1000ms
{
	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06002CB4 RID: 11444 RVA: 0x00103FFB File Offset: 0x001021FB
	private bool HasDigCell
	{
		get
		{
			return this.dig_cell != Grid.InvalidCell;
		}
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06002CB5 RID: 11445 RVA: 0x0010400D File Offset: 0x0010220D
	private bool RotationComplete
	{
		get
		{
			return this.HasDigCell && this.rotation_complete;
		}
	}

	// Token: 0x06002CB6 RID: 11446 RVA: 0x0010401F File Offset: 0x0010221F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.simRenderLoadBalance = true;
	}

	// Token: 0x06002CB7 RID: 11447 RVA: 0x00104030 File Offset: 0x00102230
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hitEffectPrefab = Assets.GetPrefab("fx_dig_splash");
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		string name = component.name + ".gun";
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
		this.arm_anim_ctrl.initialAnim = "gun";
		this.arm_anim_ctrl.isMovable = true;
		this.arm_anim_ctrl.sceneLayer = Grid.SceneLayer.TransferArm;
		component.SetSymbolVisiblity("gun_target", false);
		bool flag;
		Vector3 position = component.GetSymbolTransform(new HashedString("gun_target"), out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
		this.arm_go.transform.SetPosition(position);
		this.arm_go.SetActive(true);
		this.link = new KAnimLink(component, this.arm_anim_ctrl);
		base.Subscribe<AutoMiner>(-592767678, AutoMiner.OnOperationalChangedDelegate);
		this.RotateArm(this.rotatable.GetRotatedOffset(Quaternion.Euler(0f, 0f, -45f) * Vector3.up), true, 0f);
		this.StopDig();
		base.smi.StartSM();
	}

	// Token: 0x06002CB8 RID: 11448 RVA: 0x001041FE File Offset: 0x001023FE
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06002CB9 RID: 11449 RVA: 0x00104206 File Offset: 0x00102406
	public void Sim1000ms(float dt)
	{
		if (!this.operational.IsOperational)
		{
			return;
		}
		this.RefreshDiggableCell();
		this.operational.SetActive(this.HasDigCell, false);
	}

	// Token: 0x06002CBA RID: 11450 RVA: 0x0010422E File Offset: 0x0010242E
	private void OnOperationalChanged(object data)
	{
		if (!((Boxed<bool>)data).value)
		{
			this.dig_cell = Grid.InvalidCell;
			this.rotation_complete = false;
		}
	}

	// Token: 0x06002CBB RID: 11451 RVA: 0x00104250 File Offset: 0x00102450
	public void UpdateRotation(float dt)
	{
		if (this.HasDigCell)
		{
			Vector3 a = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.TileMain);
			a.z = 0f;
			Vector3 position = this.arm_go.transform.GetPosition();
			position.z = 0f;
			Vector3 target_dir = Vector3.Normalize(a - position);
			this.RotateArm(target_dir, false, dt);
		}
	}

	// Token: 0x06002CBC RID: 11452 RVA: 0x001042B2 File Offset: 0x001024B2
	private Element GetTargetElement()
	{
		if (this.HasDigCell)
		{
			return Grid.Element[this.dig_cell];
		}
		return null;
	}

	// Token: 0x06002CBD RID: 11453 RVA: 0x001042CC File Offset: 0x001024CC
	public void StartDig()
	{
		Element targetElement = this.GetTargetElement();
		base.Trigger(-1762453998, targetElement);
		this.CreateHitEffect();
		this.arm_anim_ctrl.Play("gun_digging", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06002CBE RID: 11454 RVA: 0x00104312 File Offset: 0x00102512
	public void StopDig()
	{
		base.Trigger(939543986, null);
		this.DestroyHitEffect();
		this.arm_anim_ctrl.Play("gun", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06002CBF RID: 11455 RVA: 0x00104348 File Offset: 0x00102548
	public void UpdateDig(float dt)
	{
		if (!this.HasDigCell)
		{
			return;
		}
		if (!this.rotation_complete)
		{
			return;
		}
		Diggable.DoDigTick(this.dig_cell, dt, WorldDamage.DamageType.NoBuildingDamage);
		float percentComplete = Grid.Damage[this.dig_cell];
		this.mining_sounds.SetPercentComplete(percentComplete);
		Vector3 a = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.FXFront2);
		a.z = 0f;
		Vector3 position = this.arm_go.transform.GetPosition();
		position.z = 0f;
		float sqrMagnitude = (a - position).sqrMagnitude;
		this.arm_anim_ctrl.GetBatchInstanceData().SetClipRadius(position.x, position.y, sqrMagnitude, true);
		if (!AutoMiner.ValidDigCell(this.dig_cell))
		{
			this.dig_cell = Grid.InvalidCell;
			this.rotation_complete = false;
		}
	}

	// Token: 0x06002CC0 RID: 11456 RVA: 0x00104414 File Offset: 0x00102614
	private void CreateHitEffect()
	{
		if (this.hitEffectPrefab == null)
		{
			return;
		}
		if (this.hitEffect != null)
		{
			this.DestroyHitEffect();
		}
		Vector3 position = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.FXFront2);
		this.hitEffect = GameUtil.KInstantiate(this.hitEffectPrefab, position, Grid.SceneLayer.FXFront2, null, 0);
		this.hitEffect.SetActive(true);
		KBatchedAnimController component = this.hitEffect.GetComponent<KBatchedAnimController>();
		component.sceneLayer = Grid.SceneLayer.FXFront2;
		component.initialMode = KAnim.PlayMode.Loop;
		component.enabled = false;
		component.enabled = true;
	}

	// Token: 0x06002CC1 RID: 11457 RVA: 0x0010449B File Offset: 0x0010269B
	private void DestroyHitEffect()
	{
		if (this.hitEffectPrefab == null)
		{
			return;
		}
		if (this.hitEffect != null)
		{
			this.hitEffect.DeleteObject();
			this.hitEffect = null;
		}
	}

	// Token: 0x06002CC2 RID: 11458 RVA: 0x001044CC File Offset: 0x001026CC
	private void RefreshDiggableCell()
	{
		CellOffset rotatedCellOffset = this.vision_offset;
		if (this.rotatable)
		{
			rotatedCellOffset = this.rotatable.GetRotatedCellOffset(this.vision_offset);
		}
		int cell = Grid.PosToCell(base.transform.gameObject);
		int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
		int num;
		int num2;
		Grid.CellToXY(cell2, out num, out num2);
		float num3 = float.MaxValue;
		int num4 = Grid.InvalidCell;
		Vector3 a = Grid.CellToPos(cell2);
		bool flag = false;
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				CellOffset rotatedCellOffset2 = new CellOffset(this.x + j, this.y + i);
				if (this.rotatable)
				{
					rotatedCellOffset2 = this.rotatable.GetRotatedCellOffset(rotatedCellOffset2);
				}
				int num5 = Grid.OffsetCell(cell, rotatedCellOffset2);
				if (Grid.IsValidCell(num5))
				{
					int x;
					int y;
					Grid.CellToXY(num5, out x, out y);
					if (Grid.IsValidCell(num5) && AutoMiner.ValidDigCell(num5) && Grid.TestLineOfSight(num, num2, x, y, AutoMiner.DigBlockingCB, false, false))
					{
						if (num5 == this.dig_cell)
						{
							flag = true;
						}
						Vector3 b = Grid.CellToPos(num5);
						float num6 = Vector3.Distance(a, b);
						if (num6 < num3)
						{
							num3 = num6;
							num4 = num5;
						}
					}
				}
			}
		}
		if (!flag && this.dig_cell != num4)
		{
			this.dig_cell = num4;
			this.rotation_complete = false;
		}
	}

	// Token: 0x06002CC3 RID: 11459 RVA: 0x00104634 File Offset: 0x00102834
	private static bool ValidDigCell(int cell)
	{
		bool flag = Grid.HasDoor[cell] && Grid.Foundation[cell] && Grid.ObjectLayers[9].ContainsKey(cell);
		if (flag)
		{
			Door component = Grid.ObjectLayers[9][cell].GetComponent<Door>();
			flag = (component != null && component.IsOpen() && !component.IsPendingClose());
		}
		return Grid.Solid[cell] && (!Grid.Foundation[cell] || flag) && Grid.Element[cell].hardness < 150;
	}

	// Token: 0x06002CC4 RID: 11460 RVA: 0x001046DC File Offset: 0x001028DC
	private void RotateArm(Vector3 target_dir, bool warp, float dt)
	{
		if (this.rotation_complete)
		{
			return;
		}
		float num = MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - this.arm_rot;
		num = MathUtil.Wrap(-180f, 180f, num);
		this.rotation_complete = Mathf.Approximately(num, 0f);
		float num2 = num;
		if (warp)
		{
			this.rotation_complete = true;
		}
		else
		{
			num2 = Mathf.Clamp(num2, -this.turn_rate * dt, this.turn_rate * dt);
		}
		this.arm_rot += num2;
		this.arm_rot = MathUtil.Wrap(-180f, 180f, this.arm_rot);
		this.arm_go.transform.rotation = Quaternion.Euler(0f, 0f, this.arm_rot);
		if (!this.rotation_complete)
		{
			this.StartRotateSound();
			this.looping_sounds.SetParameter(this.rotateSound, AutoMiner.HASH_ROTATION, this.arm_rot);
			return;
		}
		this.StopRotateSound();
	}

	// Token: 0x06002CC5 RID: 11461 RVA: 0x001047D1 File Offset: 0x001029D1
	private void StartRotateSound()
	{
		if (!this.rotate_sound_playing)
		{
			this.looping_sounds.StartSound(this.rotateSound);
			this.rotate_sound_playing = true;
		}
	}

	// Token: 0x06002CC6 RID: 11462 RVA: 0x001047F4 File Offset: 0x001029F4
	private void StopRotateSound()
	{
		if (this.rotate_sound_playing)
		{
			this.looping_sounds.StopSound(this.rotateSound);
			this.rotate_sound_playing = false;
		}
	}

	// Token: 0x04001A92 RID: 6802
	private static HashedString HASH_ROTATION = "rotation";

	// Token: 0x04001A93 RID: 6803
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001A94 RID: 6804
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04001A95 RID: 6805
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001A96 RID: 6806
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001A97 RID: 6807
	[MyCmpReq]
	private MiningSounds mining_sounds;

	// Token: 0x04001A98 RID: 6808
	public int x;

	// Token: 0x04001A99 RID: 6809
	public int y;

	// Token: 0x04001A9A RID: 6810
	public int width;

	// Token: 0x04001A9B RID: 6811
	public int height;

	// Token: 0x04001A9C RID: 6812
	public CellOffset vision_offset;

	// Token: 0x04001A9D RID: 6813
	private KBatchedAnimController arm_anim_ctrl;

	// Token: 0x04001A9E RID: 6814
	private GameObject arm_go;

	// Token: 0x04001A9F RID: 6815
	private LoopingSounds looping_sounds;

	// Token: 0x04001AA0 RID: 6816
	private string rotateSoundName = "AutoMiner_rotate";

	// Token: 0x04001AA1 RID: 6817
	private EventReference rotateSound;

	// Token: 0x04001AA2 RID: 6818
	private KAnimLink link;

	// Token: 0x04001AA3 RID: 6819
	private float arm_rot = 45f;

	// Token: 0x04001AA4 RID: 6820
	private float turn_rate = 180f;

	// Token: 0x04001AA5 RID: 6821
	private bool rotation_complete;

	// Token: 0x04001AA6 RID: 6822
	private bool rotate_sound_playing;

	// Token: 0x04001AA7 RID: 6823
	private GameObject hitEffectPrefab;

	// Token: 0x04001AA8 RID: 6824
	private GameObject hitEffect;

	// Token: 0x04001AA9 RID: 6825
	private int dig_cell = Grid.InvalidCell;

	// Token: 0x04001AAA RID: 6826
	private static readonly EventSystem.IntraObjectHandler<AutoMiner> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<AutoMiner>(delegate(AutoMiner component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001AAB RID: 6827
	public static Func<int, bool> DigBlockingCB = delegate(int cell)
	{
		bool flag = Grid.HasDoor[cell] && Grid.Foundation[cell] && Grid.ObjectLayers[9].ContainsKey(cell);
		if (flag)
		{
			Door component = Grid.ObjectLayers[9][cell].GetComponent<Door>();
			flag = (component != null && component.IsOpen() && !component.IsPendingClose());
		}
		return (Grid.Foundation[cell] && Grid.Solid[cell] && !flag) || Grid.Element[cell].hardness >= 150;
	};

	// Token: 0x020015CF RID: 5583
	public class Instance : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.GameInstance
	{
		// Token: 0x060094A9 RID: 38057 RVA: 0x00379DC0 File Offset: 0x00377FC0
		public Instance(AutoMiner master) : base(master)
		{
		}
	}

	// Token: 0x020015D0 RID: 5584
	public class States : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner>
	{
		// Token: 0x060094AA RID: 38058 RVA: 0x00379DCC File Offset: 0x00377FCC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (AutoMiner.Instance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.moving, (AutoMiner.Instance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.moving.Exit(delegate(AutoMiner.Instance smi)
			{
				smi.master.StopRotateSound();
			}).PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsActive).Update(delegate(AutoMiner.Instance smi, float dt)
			{
				smi.master.UpdateRotation(dt);
			}, UpdateRate.SIM_33ms, false).Transition(this.on.digging, new StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback(AutoMiner.States.RotationComplete), UpdateRate.SIM_200ms);
			this.on.digging.Enter(delegate(AutoMiner.Instance smi)
			{
				smi.master.StartDig();
			}).Exit(delegate(AutoMiner.Instance smi)
			{
				smi.master.StopDig();
			}).PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsActive).Update(delegate(AutoMiner.Instance smi, float dt)
			{
				smi.master.UpdateDig(dt);
			}, UpdateRate.SIM_200ms, false).Transition(this.on.moving, GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Not(new StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback(AutoMiner.States.RotationComplete)), UpdateRate.SIM_200ms);
		}

		// Token: 0x060094AB RID: 38059 RVA: 0x0037A048 File Offset: 0x00378248
		public static bool RotationComplete(AutoMiner.Instance smi)
		{
			return smi.master.RotationComplete;
		}

		// Token: 0x040072C5 RID: 29381
		public StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.BoolParameter transferring;

		// Token: 0x040072C6 RID: 29382
		public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State off;

		// Token: 0x040072C7 RID: 29383
		public AutoMiner.States.ReadyStates on;

		// Token: 0x020028C8 RID: 10440
		public class ReadyStates : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State
		{
			// Token: 0x0400B3B1 RID: 46001
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State idle;

			// Token: 0x0400B3B2 RID: 46002
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State moving;

			// Token: 0x0400B3B3 RID: 46003
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State digging;
		}
	}
}
