using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020005C7 RID: 1479
public class PollinationVFXMonitor : GameStateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>
{
	// Token: 0x060021ED RID: 8685 RVA: 0x000C4FF8 File Offset: 0x000C31F8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventTransition(GameHashes.EffectAdded, this.pollinated, new StateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.Transition.ConditionCallback(PollinationVFXMonitor.IsPollinated));
		this.pollinated.EventTransition(GameHashes.EffectRemoved, this.idle, GameStateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.Not(new StateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.Transition.ConditionCallback(PollinationVFXMonitor.IsPollinated))).Toggle("Toggle Pollination VFX", new StateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.State.Callback(PollinationVFXMonitor.CreatePollinationEffect), new StateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.State.Callback(PollinationVFXMonitor.DestroyPollinationEffect));
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x000C5081 File Offset: 0x000C3281
	private static bool IsPollinated(PollinationVFXMonitor.Instance smi)
	{
		return smi.IsPollinated();
	}

	// Token: 0x060021EF RID: 8687 RVA: 0x000C5089 File Offset: 0x000C3289
	private static void DestroyPollinationEffect(PollinationVFXMonitor.Instance smi)
	{
		smi.DestroyPollinationEffect();
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x000C5091 File Offset: 0x000C3291
	private static void CreatePollinationEffect(PollinationVFXMonitor.Instance smi)
	{
		smi.CreatePollinationEffect();
	}

	// Token: 0x040013C9 RID: 5065
	private GameStateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.State idle;

	// Token: 0x040013CA RID: 5066
	private GameStateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.State pollinated;

	// Token: 0x02001489 RID: 5257
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200148A RID: 5258
	public new class Instance : GameStateMachine<PollinationVFXMonitor, PollinationVFXMonitor.Instance, IStateMachineTarget, PollinationVFXMonitor.Def>.GameInstance
	{
		// Token: 0x0600902A RID: 36906 RVA: 0x0036DBB9 File Offset: 0x0036BDB9
		public Instance(IStateMachineTarget master, PollinationVFXMonitor.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
			this.occupyArea = base.GetComponent<OccupyArea>();
		}

		// Token: 0x0600902B RID: 36907 RVA: 0x0036DBDB File Offset: 0x0036BDDB
		public override void StartSM()
		{
			this.isHangingPlant = base.gameObject.HasTag(GameTags.Hanging);
			base.StartSM();
		}

		// Token: 0x0600902C RID: 36908 RVA: 0x0036DBFC File Offset: 0x0036BDFC
		public bool IsPollinated()
		{
			if (this.effects == null)
			{
				return false;
			}
			foreach (HashedString effect_id in PollinationMonitor.PollinationEffects)
			{
				if (this.effects.HasEffect(effect_id))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600902D RID: 36909 RVA: 0x0036DC48 File Offset: 0x0036BE48
		public void CreatePollinationEffect()
		{
			this.DestroyPollinationEffect();
			Vector4 vector = new Vector4(float.MaxValue, float.MinValue, float.MaxValue, float.MinValue);
			foreach (CellOffset cellOffset in this.occupyArea.OccupiedCellsOffsets)
			{
				if ((float)cellOffset.x < vector.x)
				{
					vector.x = (float)cellOffset.x;
				}
				if ((float)cellOffset.x > vector.y)
				{
					vector.y = (float)cellOffset.x;
				}
				if ((float)cellOffset.y < vector.z)
				{
					vector.z = (float)cellOffset.y;
				}
				if ((float)cellOffset.y > vector.w)
				{
					vector.w = (float)cellOffset.y;
				}
			}
			int num = 1 + (int)Mathf.Clamp(vector.y - vector.x, 0f, 2.1474836E+09f);
			int num2 = 1 + (int)Mathf.Clamp(vector.w - vector.z, 0f, 2.1474836E+09f);
			Vector3 position = Grid.CellToPosCBC(this.occupyArea.GetOffsetCellWithRotation(new CellOffset(0, this.isHangingPlant ? (-num2 + 1) : 0)), Grid.SceneLayer.BuildingFront);
			GameObject gameObject = Util.KInstantiate(EffectPrefabs.Instance.PlantPollinated, position, Quaternion.identity, base.gameObject, "PollinationVFX", true, 0);
			this.pollinationEffect = gameObject.GetComponent<ParticleSystem>();
			ParticleSystem.ShapeModule shape = this.pollinationEffect.shape;
			Vector3 scale = shape.scale;
			Vector3 position2 = shape.position;
			scale.x = (float)num;
			scale.y = (float)num2;
			position2.y = (float)num2 * 0.5f;
			shape.scale = scale;
			shape.position = position2;
		}

		// Token: 0x0600902E RID: 36910 RVA: 0x0036DE0E File Offset: 0x0036C00E
		public void DestroyPollinationEffect()
		{
			if (this.pollinationEffect != null)
			{
				this.pollinationEffect.DeleteObject();
				this.pollinationEffect = null;
			}
		}

		// Token: 0x04006EE6 RID: 28390
		private Effects effects;

		// Token: 0x04006EE7 RID: 28391
		private ParticleSystem pollinationEffect;

		// Token: 0x04006EE8 RID: 28392
		private OccupyArea occupyArea;

		// Token: 0x04006EE9 RID: 28393
		private bool isHangingPlant;
	}
}
