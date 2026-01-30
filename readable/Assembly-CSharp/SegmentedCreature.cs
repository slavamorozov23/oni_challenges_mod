using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000113 RID: 275
public class SegmentedCreature : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>
{
	// Token: 0x06000511 RID: 1297 RVA: 0x00028C78 File Offset: 0x00026E78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.freeMovement.idle;
		this.root.Enter(new StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State.Callback(this.SetRetractedPath));
		this.retracted.DefaultState(this.retracted.pre).Enter(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "idle_loop", KAnim.PlayMode.Loop, false, 0);
		}).Exit(new StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State.Callback(this.SetRetractedPath));
		this.retracted.pre.Update(new Action<SegmentedCreature.Instance, float>(this.UpdateRetractedPre), UpdateRate.SIM_EVERY_TICK, false);
		this.retracted.loop.ParamTransition<bool>(this.isRetracted, this.freeMovement, (SegmentedCreature.Instance smi, bool p) => !this.isRetracted.Get(smi)).Update(new Action<SegmentedCreature.Instance, float>(this.UpdateRetractedLoop), UpdateRate.SIM_EVERY_TICK, false);
		this.freeMovement.DefaultState(this.freeMovement.idle).ParamTransition<bool>(this.isRetracted, this.retracted, (SegmentedCreature.Instance smi, bool p) => this.isRetracted.Get(smi)).Update(new Action<SegmentedCreature.Instance, float>(this.UpdateFreeMovement), UpdateRate.SIM_EVERY_TICK, false);
		this.freeMovement.idle.Transition(this.freeMovement.moving, (SegmentedCreature.Instance smi) => smi.navigator.IsMoving(), UpdateRate.SIM_200ms).Enter(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "idle_loop", KAnim.PlayMode.Loop, true, 0);
		});
		this.freeMovement.moving.Transition(this.freeMovement.idle, (SegmentedCreature.Instance smi) => !smi.navigator.IsMoving(), UpdateRate.SIM_200ms).Enter(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "walking_pre", KAnim.PlayMode.Once, false, 0);
			this.PlayBodySegmentsAnim(smi, "walking_loop", KAnim.PlayMode.Loop, false, smi.def.animFrameOffset);
		}).Exit(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "walking_pst", KAnim.PlayMode.Once, true, 0);
		});
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x00028E30 File Offset: 0x00027030
	private void PlayBodySegmentsAnim(SegmentedCreature.Instance smi, string animName, KAnim.PlayMode playMode, bool queue = false, int frameOffset = 0)
	{
		LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode();
		int num = 0;
		while (linkedListNode != null)
		{
			if (queue)
			{
				linkedListNode.Value.animController.Queue(animName, playMode, 1f, 0f);
			}
			else
			{
				linkedListNode.Value.animController.Play(animName, playMode, 1f, 0f);
			}
			if (frameOffset > 0)
			{
				float num2 = (float)linkedListNode.Value.animController.GetCurrentNumFrames();
				float elapsedTime = (float)num * ((float)frameOffset / num2);
				linkedListNode.Value.animController.SetElapsedTime(elapsedTime);
			}
			num++;
			linkedListNode = linkedListNode.Next;
		}
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x00028ED8 File Offset: 0x000270D8
	private void UpdateRetractedPre(SegmentedCreature.Instance smi, float dt)
	{
		if (this.UpdateHeadPosition(smi) == 0f)
		{
			return;
		}
		bool flag = true;
		for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			linkedListNode.Value.distanceToPreviousSegment = Mathf.Max(smi.def.minSegmentSpacing, linkedListNode.Value.distanceToPreviousSegment - dt * smi.def.retractionSegmentSpeed);
			if (linkedListNode.Value.distanceToPreviousSegment > smi.def.minSegmentSpacing)
			{
				flag = false;
			}
		}
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		LinkedListNode<SegmentedCreature.PathNode> linkedListNode2 = smi.path.First;
		Vector3 forward = value.Forward;
		Quaternion rotation = value.Rotation;
		int num = 0;
		while (linkedListNode2 != null)
		{
			Vector3 b = value.Position - smi.def.pathSpacing * (float)num * forward;
			linkedListNode2.Value.position = Vector3.Lerp(linkedListNode2.Value.position, b, dt * smi.def.retractionPathSpeed);
			linkedListNode2.Value.rotation = Quaternion.Slerp(linkedListNode2.Value.rotation, rotation, dt * smi.def.retractionPathSpeed);
			num++;
			linkedListNode2 = linkedListNode2.Next;
		}
		this.UpdateBodyPosition(smi);
		if (flag)
		{
			smi.GoTo(this.retracted.loop);
		}
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x0002902C File Offset: 0x0002722C
	private void UpdateRetractedLoop(SegmentedCreature.Instance smi, float dt)
	{
		if (this.UpdateHeadPosition(smi) != 0f)
		{
			this.SetRetractedPath(smi);
			this.UpdateBodyPosition(smi);
		}
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x0002904C File Offset: 0x0002724C
	private void SetRetractedPath(SegmentedCreature.Instance smi)
	{
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		LinkedListNode<SegmentedCreature.PathNode> linkedListNode = smi.path.First;
		Vector3 position = value.Position;
		Quaternion rotation = value.Rotation;
		Vector3 forward = value.Forward;
		int num = 0;
		while (linkedListNode != null)
		{
			linkedListNode.Value.position = position - smi.def.pathSpacing * (float)num * forward;
			linkedListNode.Value.rotation = rotation;
			num++;
			linkedListNode = linkedListNode.Next;
		}
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x000290CC File Offset: 0x000272CC
	private void UpdateFreeMovement(SegmentedCreature.Instance smi, float dt)
	{
		float num = this.UpdateHeadPosition(smi);
		if (num != 0f)
		{
			this.AdjustBodySegmentsSpacing(smi, num);
			this.UpdateBodyPosition(smi);
		}
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x000290F8 File Offset: 0x000272F8
	private float UpdateHeadPosition(SegmentedCreature.Instance smi)
	{
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		if (value.Position == smi.previousHeadPosition)
		{
			return 0f;
		}
		SegmentedCreature.PathNode value2 = smi.path.First.Value;
		SegmentedCreature.PathNode value3 = smi.path.First.Next.Value;
		float magnitude = (value2.position - value3.position).magnitude;
		float magnitude2 = (value.Position - value3.position).magnitude;
		float result = magnitude2 - magnitude;
		value2.position = value.Position;
		value2.rotation = value.Rotation;
		smi.previousHeadPosition = value2.position;
		Vector3 normalized = (value2.position - value3.position).normalized;
		int num = Mathf.FloorToInt(magnitude2 / smi.def.pathSpacing);
		for (int i = 0; i < num; i++)
		{
			Vector3 position = value3.position + normalized * smi.def.pathSpacing;
			LinkedListNode<SegmentedCreature.PathNode> last = smi.path.Last;
			last.Value.position = position;
			last.Value.rotation = value2.rotation;
			float num2 = magnitude2 - (float)i * smi.def.pathSpacing;
			float t = num2 - smi.def.pathSpacing / num2;
			last.Value.rotation = Quaternion.Lerp(value2.rotation, value3.rotation, t);
			smi.path.RemoveLast();
			smi.path.AddAfter(smi.path.First, last);
			value3 = last.Value;
		}
		return result;
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x000292BC File Offset: 0x000274BC
	private void AdjustBodySegmentsSpacing(SegmentedCreature.Instance smi, float spacing)
	{
		for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			linkedListNode.Value.distanceToPreviousSegment += spacing;
			if (linkedListNode.Value.distanceToPreviousSegment < smi.def.minSegmentSpacing)
			{
				spacing = linkedListNode.Value.distanceToPreviousSegment - smi.def.minSegmentSpacing;
				linkedListNode.Value.distanceToPreviousSegment = smi.def.minSegmentSpacing;
			}
			else
			{
				if (linkedListNode.Value.distanceToPreviousSegment <= smi.def.maxSegmentSpacing)
				{
					break;
				}
				spacing = linkedListNode.Value.distanceToPreviousSegment - smi.def.maxSegmentSpacing;
				linkedListNode.Value.distanceToPreviousSegment = smi.def.maxSegmentSpacing;
			}
		}
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x00029388 File Offset: 0x00027588
	private void UpdateBodyPosition(SegmentedCreature.Instance smi)
	{
		LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode();
		LinkedListNode<SegmentedCreature.PathNode> linkedListNode2 = smi.path.First;
		float num = 0f;
		float num2 = smi.LengthPercentage();
		int num3 = 0;
		while (linkedListNode != null)
		{
			float num4 = linkedListNode.Value.distanceToPreviousSegment;
			float num5 = 0f;
			while (linkedListNode2.Next != null)
			{
				num5 = (linkedListNode2.Value.position - linkedListNode2.Next.Value.position).magnitude - num;
				if (num4 < num5)
				{
					break;
				}
				num4 -= num5;
				num = 0f;
				linkedListNode2 = linkedListNode2.Next;
			}
			if (linkedListNode2.Next == null)
			{
				linkedListNode.Value.SetPosition(linkedListNode2.Value.position);
				linkedListNode.Value.SetRotation(smi.path.Last.Value.rotation);
			}
			else
			{
				SegmentedCreature.PathNode value = linkedListNode2.Value;
				SegmentedCreature.PathNode value2 = linkedListNode2.Next.Value;
				linkedListNode.Value.SetPosition(linkedListNode2.Value.position + (linkedListNode2.Next.Value.position - linkedListNode2.Value.position).normalized * num4);
				linkedListNode.Value.SetRotation(Quaternion.Slerp(value.rotation, value2.rotation, num4 / num5));
				num = num4;
			}
			linkedListNode.Value.animController.FlipX = (linkedListNode.Previous.Value.Position.x < linkedListNode.Value.Position.x);
			linkedListNode.Value.animController.animScale = smi.baseAnimScale + smi.baseAnimScale * smi.def.compressedMaxScale * ((float)(smi.def.numBodySegments - num3) / (float)smi.def.numBodySegments) * (1f - num2);
			linkedListNode = linkedListNode.Next;
			num3++;
		}
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00029584 File Offset: 0x00027784
	private void DrawDebug(SegmentedCreature.Instance smi, float dt)
	{
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		DrawUtil.Arrow(value.Position, value.Position + value.Up, 0.05f, Color.red, 0f);
		DrawUtil.Arrow(value.Position, value.Position + value.Forward * 0.06f, 0.05f, Color.cyan, 0f);
		int num = 0;
		foreach (SegmentedCreature.PathNode pathNode in smi.path)
		{
			Color color = Color.HSVToRGB((float)num / (float)smi.def.numPathNodes, 1f, 1f);
			DrawUtil.Gnomon(pathNode.position, 0.05f, Color.cyan, 0f);
			DrawUtil.Arrow(pathNode.position, pathNode.position + pathNode.rotation * Vector3.up * 0.5f, 0.025f, color, 0f);
			num++;
		}
		for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.segments.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			DrawUtil.Circle(linkedListNode.Value.Position, 0.05f, Color.white, new Vector3?(Vector3.forward), 0f);
			DrawUtil.Gnomon(linkedListNode.Value.Position, 0.05f, Color.white, 0f);
		}
	}

	// Token: 0x040003AE RID: 942
	public SegmentedCreature.RectractStates retracted;

	// Token: 0x040003AF RID: 943
	public SegmentedCreature.FreeMovementStates freeMovement;

	// Token: 0x040003B0 RID: 944
	private StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.BoolParameter isRetracted;

	// Token: 0x02001198 RID: 4504
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006515 RID: 25877
		public HashedString segmentTrackerSymbol;

		// Token: 0x04006516 RID: 25878
		public Vector3 headOffset = Vector3.zero;

		// Token: 0x04006517 RID: 25879
		public Vector3 bodyPivot = Vector3.zero;

		// Token: 0x04006518 RID: 25880
		public Vector3 tailPivot = Vector3.zero;

		// Token: 0x04006519 RID: 25881
		public int numBodySegments;

		// Token: 0x0400651A RID: 25882
		public float minSegmentSpacing;

		// Token: 0x0400651B RID: 25883
		public float maxSegmentSpacing;

		// Token: 0x0400651C RID: 25884
		public int numPathNodes;

		// Token: 0x0400651D RID: 25885
		public float pathSpacing;

		// Token: 0x0400651E RID: 25886
		public KAnimFile midAnim;

		// Token: 0x0400651F RID: 25887
		public KAnimFile tailAnim;

		// Token: 0x04006520 RID: 25888
		public string movingAnimName;

		// Token: 0x04006521 RID: 25889
		public string idleAnimName;

		// Token: 0x04006522 RID: 25890
		public float retractionSegmentSpeed = 1f;

		// Token: 0x04006523 RID: 25891
		public float retractionPathSpeed = 1f;

		// Token: 0x04006524 RID: 25892
		public float compressedMaxScale = 1.2f;

		// Token: 0x04006525 RID: 25893
		public int animFrameOffset;

		// Token: 0x04006526 RID: 25894
		public HashSet<HashedString> hideBoddyWhenStartingAnimNames = new HashSet<HashedString>
		{
			"rocket_biological"
		};

		// Token: 0x04006527 RID: 25895
		public HashSet<HashedString> retractWhenStartingAnimNames = new HashSet<HashedString>
		{
			"trapped",
			"trussed",
			"escape",
			"drown_pre",
			"drown_loop",
			"drown_pst",
			"rocket_biological"
		};

		// Token: 0x04006528 RID: 25896
		public HashSet<HashedString> retractWhenEndingAnimNames = new HashSet<HashedString>
		{
			"floor_floor_2_0",
			"grooming_pst",
			"fall"
		};
	}

	// Token: 0x02001199 RID: 4505
	public class RectractStates : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State
	{
		// Token: 0x04006529 RID: 25897
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State pre;

		// Token: 0x0400652A RID: 25898
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State loop;
	}

	// Token: 0x0200119A RID: 4506
	public class FreeMovementStates : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State
	{
		// Token: 0x0400652B RID: 25899
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State idle;

		// Token: 0x0400652C RID: 25900
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State moving;

		// Token: 0x0400652D RID: 25901
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State layEgg;

		// Token: 0x0400652E RID: 25902
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State poop;

		// Token: 0x0400652F RID: 25903
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State dead;
	}

	// Token: 0x0200119B RID: 4507
	public new class Instance : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.GameInstance
	{
		// Token: 0x060084F8 RID: 34040 RVA: 0x0034633C File Offset: 0x0034453C
		public Instance(IStateMachineTarget master, SegmentedCreature.Def def) : base(master, def)
		{
			global::Debug.Assert((float)def.numBodySegments * def.maxSegmentSpacing < (float)def.numPathNodes * def.pathSpacing);
			this.CreateSegments();
			this.navigator = master.GetComponent<Navigator>();
		}

		// Token: 0x060084F9 RID: 34041 RVA: 0x0034639C File Offset: 0x0034459C
		private void CreateSegments()
		{
			float num = (float)SegmentedCreature.Instance.creatureBatchSlot * 0.01f;
			SegmentedCreature.Instance.creatureBatchSlot = (SegmentedCreature.Instance.creatureBatchSlot + 1) % 10;
			SegmentedCreature.CreatureSegment value = this.segments.AddFirst(new SegmentedCreature.CreatureSegment(base.GetComponent<KBatchedAnimController>(), base.gameObject, num, base.smi.def.headOffset, Vector3.zero)).Value;
			base.gameObject.SetActive(false);
			value.animController = base.GetComponent<KBatchedAnimController>();
			value.animController.SetSymbolVisiblity(base.smi.def.segmentTrackerSymbol, false);
			value.symbol = base.smi.def.segmentTrackerSymbol;
			value.SetPosition(base.transform.position);
			base.gameObject.SetActive(true);
			this.baseAnimScale = value.animController.animScale;
			value.animController.onAnimEnter += this.AnimEntered;
			value.animController.onAnimComplete += this.AnimComplete;
			for (int i = 0; i < base.def.numBodySegments; i++)
			{
				GameObject gameObject = new GameObject(base.gameObject.GetProperName() + string.Format(" Segment {0}", i));
				gameObject.SetActive(false);
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = value.Position;
				KAnimFile kanimFile = base.def.midAnim;
				Vector3 pivot = base.def.bodyPivot;
				if (i == base.def.numBodySegments - 1)
				{
					kanimFile = base.def.tailAnim;
					pivot = base.def.tailPivot;
				}
				KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
				kbatchedAnimController.AnimFiles = new KAnimFile[]
				{
					kanimFile
				};
				kbatchedAnimController.isMovable = true;
				kbatchedAnimController.SetSymbolVisiblity(base.smi.def.segmentTrackerSymbol, false);
				kbatchedAnimController.sceneLayer = value.animController.sceneLayer;
				SegmentedCreature.CreatureSegment creatureSegment = new SegmentedCreature.CreatureSegment(value.animController, gameObject, num + (float)(i + 1) * 0.0001f, Vector3.zero, pivot);
				creatureSegment.animController = kbatchedAnimController;
				creatureSegment.symbol = base.smi.def.segmentTrackerSymbol;
				creatureSegment.distanceToPreviousSegment = base.smi.def.minSegmentSpacing;
				creatureSegment.animLink = new KAnimLink(value.animController, kbatchedAnimController);
				this.segments.AddLast(creatureSegment);
				gameObject.SetActive(true);
			}
			for (int j = 0; j < base.def.numPathNodes; j++)
			{
				this.path.AddLast(new SegmentedCreature.PathNode(value.Position));
			}
		}

		// Token: 0x060084FA RID: 34042 RVA: 0x0034665C File Offset: 0x0034485C
		public void AnimEntered(HashedString name)
		{
			if (base.smi.def.retractWhenStartingAnimNames.Contains(name))
			{
				base.smi.sm.isRetracted.Set(true, base.smi, false);
			}
			else
			{
				base.smi.sm.isRetracted.Set(false, base.smi, false);
			}
			if (base.smi.def.hideBoddyWhenStartingAnimNames.Contains(name))
			{
				this.SetBodySegmentsVisibility(false);
				return;
			}
			this.SetBodySegmentsVisibility(true);
		}

		// Token: 0x060084FB RID: 34043 RVA: 0x003466E8 File Offset: 0x003448E8
		public void SetBodySegmentsVisibility(bool visible)
		{
			for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = base.smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value.animController.SetVisiblity(visible);
			}
		}

		// Token: 0x060084FC RID: 34044 RVA: 0x0034671E File Offset: 0x0034491E
		public void AnimComplete(HashedString name)
		{
			if (base.smi.def.retractWhenEndingAnimNames.Contains(name))
			{
				base.smi.sm.isRetracted.Set(true, base.smi, false);
			}
		}

		// Token: 0x060084FD RID: 34045 RVA: 0x00346756 File Offset: 0x00344956
		public LinkedListNode<SegmentedCreature.CreatureSegment> GetHeadSegmentNode()
		{
			return base.smi.segments.First;
		}

		// Token: 0x060084FE RID: 34046 RVA: 0x00346768 File Offset: 0x00344968
		public LinkedListNode<SegmentedCreature.CreatureSegment> GetFirstBodySegmentNode()
		{
			return base.smi.segments.First.Next;
		}

		// Token: 0x060084FF RID: 34047 RVA: 0x00346780 File Offset: 0x00344980
		public float LengthPercentage()
		{
			float num = 0f;
			for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = this.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				num += linkedListNode.Value.distanceToPreviousSegment;
			}
			float num2 = this.MinLength();
			float num3 = this.MaxLength();
			return Mathf.Clamp(num - num2, 0f, num3) / (num3 - num2);
		}

		// Token: 0x06008500 RID: 34048 RVA: 0x003467D4 File Offset: 0x003449D4
		public float MinLength()
		{
			return base.smi.def.minSegmentSpacing * (float)base.smi.def.numBodySegments;
		}

		// Token: 0x06008501 RID: 34049 RVA: 0x003467F8 File Offset: 0x003449F8
		public float MaxLength()
		{
			return base.smi.def.maxSegmentSpacing * (float)base.smi.def.numBodySegments;
		}

		// Token: 0x06008502 RID: 34050 RVA: 0x0034681C File Offset: 0x00344A1C
		protected override void OnCleanUp()
		{
			this.GetHeadSegmentNode().Value.animController.onAnimEnter -= this.AnimEntered;
			this.GetHeadSegmentNode().Value.animController.onAnimComplete -= this.AnimComplete;
			for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = this.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value.CleanUp();
			}
		}

		// Token: 0x04006530 RID: 25904
		private const int NUM_CREATURE_SLOTS = 10;

		// Token: 0x04006531 RID: 25905
		private static int creatureBatchSlot;

		// Token: 0x04006532 RID: 25906
		public float baseAnimScale;

		// Token: 0x04006533 RID: 25907
		public Vector3 previousHeadPosition;

		// Token: 0x04006534 RID: 25908
		public float previousDist;

		// Token: 0x04006535 RID: 25909
		public LinkedList<SegmentedCreature.PathNode> path = new LinkedList<SegmentedCreature.PathNode>();

		// Token: 0x04006536 RID: 25910
		public LinkedList<SegmentedCreature.CreatureSegment> segments = new LinkedList<SegmentedCreature.CreatureSegment>();

		// Token: 0x04006537 RID: 25911
		public Navigator navigator;
	}

	// Token: 0x0200119C RID: 4508
	public class PathNode
	{
		// Token: 0x06008503 RID: 34051 RVA: 0x00346889 File Offset: 0x00344A89
		public PathNode(Vector3 position)
		{
			this.position = position;
			this.rotation = Quaternion.identity;
		}

		// Token: 0x04006538 RID: 25912
		public Vector3 position;

		// Token: 0x04006539 RID: 25913
		public Quaternion rotation;
	}

	// Token: 0x0200119D RID: 4509
	public class CreatureSegment
	{
		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06008504 RID: 34052 RVA: 0x003468A3 File Offset: 0x00344AA3
		public float ZOffset
		{
			get
			{
				return Grid.GetLayerZ(this.head.sceneLayer) + this.zRelativeOffset;
			}
		}

		// Token: 0x06008505 RID: 34053 RVA: 0x003468BC File Offset: 0x00344ABC
		public CreatureSegment(KBatchedAnimController head, GameObject go, float zRelativeOffset, Vector3 offset, Vector3 pivot)
		{
			this.head = head;
			this.m_transform = go.transform;
			this.zRelativeOffset = zRelativeOffset;
			this.offset = offset;
			this.pivot = pivot;
			this.SetPosition(go.transform.position);
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06008506 RID: 34054 RVA: 0x0034690C File Offset: 0x00344B0C
		public Vector3 Position
		{
			get
			{
				Vector3 vector = this.offset;
				vector.x *= (float)(this.animController.FlipX ? -1 : 1);
				if (vector != Vector3.zero)
				{
					vector = this.Rotation * vector;
				}
				if (this.symbol.IsValid)
				{
					bool flag;
					Vector3 a = this.animController.GetSymbolTransform(this.symbol, out flag).GetColumn(3);
					a.z = this.ZOffset;
					return a + vector;
				}
				return this.m_transform.position + vector;
			}
		}

		// Token: 0x06008507 RID: 34055 RVA: 0x003469B0 File Offset: 0x00344BB0
		public void SetPosition(Vector3 value)
		{
			bool flag = false;
			if (this.animController != null && this.animController.sceneLayer != this.head.sceneLayer)
			{
				this.animController.SetSceneLayer(this.head.sceneLayer);
				flag = true;
			}
			value.z = this.ZOffset;
			this.m_transform.position = value;
			if (flag)
			{
				this.animController.enabled = false;
				this.animController.enabled = true;
			}
		}

		// Token: 0x06008508 RID: 34056 RVA: 0x00346A31 File Offset: 0x00344C31
		public void SetRotation(Quaternion rotation)
		{
			this.m_transform.rotation = rotation;
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06008509 RID: 34057 RVA: 0x00346A40 File Offset: 0x00344C40
		public Quaternion Rotation
		{
			get
			{
				if (this.symbol.IsValid)
				{
					bool flag;
					Vector3 toDirection = this.animController.GetSymbolLocalTransform(this.symbol, out flag).MultiplyVector(Vector3.right);
					if (!this.animController.FlipX)
					{
						toDirection.y *= -1f;
					}
					return Quaternion.FromToRotation(Vector3.right, toDirection);
				}
				return this.m_transform.rotation;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x0600850A RID: 34058 RVA: 0x00346AAF File Offset: 0x00344CAF
		public Vector3 Forward
		{
			get
			{
				return this.Rotation * (this.animController.FlipX ? Vector3.left : Vector3.right);
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x0600850B RID: 34059 RVA: 0x00346AD5 File Offset: 0x00344CD5
		public Vector3 Up
		{
			get
			{
				return this.Rotation * Vector3.up;
			}
		}

		// Token: 0x0600850C RID: 34060 RVA: 0x00346AE7 File Offset: 0x00344CE7
		public void CleanUp()
		{
			UnityEngine.Object.Destroy(this.m_transform.gameObject);
		}

		// Token: 0x0400653A RID: 25914
		public KBatchedAnimController animController;

		// Token: 0x0400653B RID: 25915
		public KAnimLink animLink;

		// Token: 0x0400653C RID: 25916
		public float distanceToPreviousSegment;

		// Token: 0x0400653D RID: 25917
		public HashedString symbol;

		// Token: 0x0400653E RID: 25918
		public Vector3 offset;

		// Token: 0x0400653F RID: 25919
		public Vector3 pivot;

		// Token: 0x04006540 RID: 25920
		public KBatchedAnimController head;

		// Token: 0x04006541 RID: 25921
		private float zRelativeOffset;

		// Token: 0x04006542 RID: 25922
		private Transform m_transform;
	}
}
