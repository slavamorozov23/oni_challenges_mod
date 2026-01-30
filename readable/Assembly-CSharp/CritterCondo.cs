using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000739 RID: 1849
public class CritterCondo : GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>
{
	// Token: 0x06002E95 RID: 11925 RVA: 0x0010D188 File Offset: 0x0010B388
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.PlayAnim("off").EventTransition(GameHashes.UpdateRoom, this.operational, new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational)).EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational));
		this.operational.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.UpdateRoom, this.inoperational, GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Not(new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational))).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Not(new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational)));
	}

	// Token: 0x06002E96 RID: 11926 RVA: 0x0010D23A File Offset: 0x0010B43A
	private static bool IsOperational(CritterCondo.Instance smi)
	{
		return smi.def.IsCritterCondoOperationalCb(smi);
	}

	// Token: 0x04001B9A RID: 7066
	public GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.State inoperational;

	// Token: 0x04001B9B RID: 7067
	public GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.State operational;

	// Token: 0x0200160E RID: 5646
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x060095D5 RID: 38357 RVA: 0x0037D741 File Offset: 0x0037B941
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>();
		}

		// Token: 0x0400739F RID: 29599
		public Func<CritterCondo.Instance, bool> IsCritterCondoOperationalCb;

		// Token: 0x040073A0 RID: 29600
		public Action<KBatchedAnimController, bool> UpdateForegroundVisibilitySymbols;

		// Token: 0x040073A1 RID: 29601
		public StatusItem moveToStatusItem;

		// Token: 0x040073A2 RID: 29602
		public StatusItem interactStatusItem;

		// Token: 0x040073A3 RID: 29603
		public Tag condoTag = "CritterCondo";

		// Token: 0x040073A4 RID: 29604
		public string effectId;
	}

	// Token: 0x0200160F RID: 5647
	public new class Instance : GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.GameInstance
	{
		// Token: 0x060095D7 RID: 38359 RVA: 0x0037D760 File Offset: 0x0037B960
		public Instance(IStateMachineTarget master, CritterCondo.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
			KBatchedAnimController[] componentsInChildren = this.animController.GetComponentsInChildren<KBatchedAnimController>();
			this.foregroundController = componentsInChildren.First((KBatchedAnimController kbac) => kbac != this.animController);
		}

		// Token: 0x060095D8 RID: 38360 RVA: 0x0037D7A5 File Offset: 0x0037B9A5
		public override void StartSM()
		{
			base.StartSM();
			Components.CritterCondos.Add(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x060095D9 RID: 38361 RVA: 0x0037D7C3 File Offset: 0x0037B9C3
		protected override void OnCleanUp()
		{
			Components.CritterCondos.Remove(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x060095DA RID: 38362 RVA: 0x0037D7DB File Offset: 0x0037B9DB
		public bool IsReserved()
		{
			return base.HasTag(GameTags.Creatures.ReservedByCreature);
		}

		// Token: 0x060095DB RID: 38363 RVA: 0x0037D7E8 File Offset: 0x0037B9E8
		public void SetReserved(bool isReserved)
		{
			if (isReserved)
			{
				base.GetComponent<KPrefabID>().SetTag(GameTags.Creatures.ReservedByCreature, true);
				return;
			}
			if (base.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.ReservedByCreature);
				return;
			}
			global::Debug.LogWarningFormat(base.smi.gameObject, "Tried to unreserve a condo that wasn't reserved", Array.Empty<object>());
		}

		// Token: 0x060095DC RID: 38364 RVA: 0x0037D842 File Offset: 0x0037BA42
		public int GetInteractStartCell()
		{
			return Grid.PosToCell(this);
		}

		// Token: 0x060095DD RID: 38365 RVA: 0x0037D84A File Offset: 0x0037BA4A
		public bool CanBeReserved()
		{
			return !this.IsReserved() && CritterCondo.IsOperational(this);
		}

		// Token: 0x060095DE RID: 38366 RVA: 0x0037D85C File Offset: 0x0037BA5C
		public void UpdateCritterAnims(string anim_name, bool enters, bool is_large_critter)
		{
			if (enters)
			{
				this.animController.Play(anim_name, KAnim.PlayMode.Once, 1f, 0f);
			}
			if (base.def.UpdateForegroundVisibilitySymbols != null)
			{
				base.def.UpdateForegroundVisibilitySymbols(this.foregroundController, is_large_critter);
			}
		}

		// Token: 0x040073A5 RID: 29605
		private KBatchedAnimController foregroundController;

		// Token: 0x040073A6 RID: 29606
		private KBatchedAnimController animController;
	}
}
