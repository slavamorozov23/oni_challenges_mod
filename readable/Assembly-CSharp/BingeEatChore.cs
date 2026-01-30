using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000496 RID: 1174
public class BingeEatChore : Chore<BingeEatChore.StatesInstance>
{
	// Token: 0x060018E9 RID: 6377 RVA: 0x0008A48C File Offset: 0x0008868C
	public BingeEatChore(IStateMachineTarget target, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.BingeEat, target, target.GetComponent<ChoreProvider>(), false, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BingeEatChore.StatesInstance(this, target.gameObject);
		this.onEatHandlerID = base.Subscribe(1121894420, new Action<object>(this.OnEat));
	}

	// Token: 0x060018EA RID: 6378 RVA: 0x0008A4F0 File Offset: 0x000886F0
	private void OnEat(object data)
	{
		Edible edible = (Edible)data;
		if (edible != null)
		{
			base.smi.sm.bingeremaining.Set(Mathf.Max(0f, base.smi.sm.bingeremaining.Get(base.smi) - edible.unitsConsumed), base.smi, false);
		}
	}

	// Token: 0x060018EB RID: 6379 RVA: 0x0008A556 File Offset: 0x00088756
	public override void Cleanup()
	{
		base.Cleanup();
		base.Unsubscribe(ref this.onEatHandlerID);
	}

	// Token: 0x04000E63 RID: 3683
	private int onEatHandlerID;

	// Token: 0x020012C0 RID: 4800
	public class StatesInstance : GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.GameInstance
	{
		// Token: 0x0600894D RID: 35149 RVA: 0x00351691 File Offset: 0x0034F891
		public StatesInstance(BingeEatChore master, GameObject eater) : base(master)
		{
			base.sm.eater.Set(eater, base.smi, false);
			base.sm.bingeremaining.Set(2f, base.smi, false);
		}

		// Token: 0x0600894E RID: 35150 RVA: 0x003516D0 File Offset: 0x0034F8D0
		public void FindFood()
		{
			Navigator component = base.GetComponent<Navigator>();
			int num = int.MaxValue;
			Edible edible = null;
			if (base.sm.bingeremaining.Get(base.smi) <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
			{
				this.GoTo(base.sm.eat_pst);
				return;
			}
			foreach (Edible edible2 in Components.Edibles.Items)
			{
				if (!edible2.HasTag(GameTags.Dehydrated) && !(edible2 == null) && !(edible2 == base.sm.ediblesource.Get<Edible>(base.smi)) && !edible2.isBeingConsumed)
				{
					Pickupable component2 = edible2.GetComponent<Pickupable>();
					if (component2.UnreservedFetchAmount > 0f && component2.CouldBePickedUpByMinion(base.GetComponent<KPrefabID>().InstanceID) && !component2.HasTag(GameTags.StoredPrivate))
					{
						int navigationCost = component.GetNavigationCost(edible2);
						if (navigationCost != -1 && navigationCost < num)
						{
							num = navigationCost;
							edible = edible2;
						}
					}
				}
			}
			base.sm.ediblesource.Set(edible, base.smi);
			base.sm.requestedfoodunits.Set(base.sm.bingeremaining.Get(base.smi), base.smi, false);
			if (edible == null)
			{
				this.GoTo(base.sm.cantFindFood);
				return;
			}
			this.GoTo(base.sm.fetch);
		}

		// Token: 0x0600894F RID: 35151 RVA: 0x0035186C File Offset: 0x0034FA6C
		public bool IsBingeEating()
		{
			return base.sm.isBingeEating.Get(base.smi);
		}
	}

	// Token: 0x020012C1 RID: 4801
	public class States : GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore>
	{
		// Token: 0x06008950 RID: 35152 RVA: 0x00351884 File Offset: 0x0034FA84
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.findfood;
			base.Target(this.eater);
			this.bingeEatingEffect = new Effect("Binge_Eating", DUPLICANTS.MODIFIERS.BINGE_EATING.NAME, DUPLICANTS.MODIFIERS.BINGE_EATING.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.bingeEatingEffect.Add(new AttributeModifier(Db.Get().Attributes.Decor.Id, -30f, DUPLICANTS.MODIFIERS.BINGE_EATING.NAME, false, false, true));
			this.bingeEatingEffect.Add(new AttributeModifier("CaloriesDelta", -6666.6665f, DUPLICANTS.MODIFIERS.BINGE_EATING.NAME, false, false, true));
			Db.Get().effects.Add(this.bingeEatingEffect);
			this.root.ToggleEffect((BingeEatChore.StatesInstance smi) => this.bingeEatingEffect);
			this.noTarget.GoTo(this.finish);
			this.eat_pst.ToggleAnims("anim_eat_overeat_kanim", 0f).PlayAnim("working_pst").OnAnimQueueComplete(this.finish);
			this.finish.Enter(delegate(BingeEatChore.StatesInstance smi)
			{
				smi.StopSM("complete/no more food");
			});
			this.findfood.Enter("FindFood", delegate(BingeEatChore.StatesInstance smi)
			{
				smi.FindFood();
			});
			this.fetch.InitializeStates(this.eater, this.ediblesource, this.ediblechunk, this.requestedfoodunits, this.actualfoodunits, this.eat, this.cantFindFood);
			this.eat.ToggleAnims("anim_eat_overeat_kanim", 0f).QueueAnim("working_loop", true, null).Enter(delegate(BingeEatChore.StatesInstance smi)
			{
				this.isBingeEating.Set(true, smi, false);
			}).DoEat(this.ediblechunk, this.actualfoodunits, this.findfood, this.findfood).Exit("ClearIsBingeEating", delegate(BingeEatChore.StatesInstance smi)
			{
				this.isBingeEating.Set(false, smi, false);
			});
			this.cantFindFood.ToggleAnims("anim_interrupt_binge_eat_kanim", 0f).PlayAnim("interrupt_binge_eat").OnAnimQueueComplete(this.noTarget);
		}

		// Token: 0x040068C0 RID: 26816
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter eater;

		// Token: 0x040068C1 RID: 26817
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter ediblesource;

		// Token: 0x040068C2 RID: 26818
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter ediblechunk;

		// Token: 0x040068C3 RID: 26819
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.BoolParameter isBingeEating;

		// Token: 0x040068C4 RID: 26820
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter requestedfoodunits;

		// Token: 0x040068C5 RID: 26821
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter actualfoodunits;

		// Token: 0x040068C6 RID: 26822
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter bingeremaining;

		// Token: 0x040068C7 RID: 26823
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State noTarget;

		// Token: 0x040068C8 RID: 26824
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State findfood;

		// Token: 0x040068C9 RID: 26825
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State eat;

		// Token: 0x040068CA RID: 26826
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State eat_pst;

		// Token: 0x040068CB RID: 26827
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State cantFindFood;

		// Token: 0x040068CC RID: 26828
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State finish;

		// Token: 0x040068CD RID: 26829
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FetchSubState fetch;

		// Token: 0x040068CE RID: 26830
		private Effect bingeEatingEffect;
	}
}
