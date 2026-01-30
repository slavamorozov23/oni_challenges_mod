using System;

// Token: 0x02000A65 RID: 2661
[SkipSaveFileSerialization]
public class Fashionable : StateMachineComponent<Fashionable.StatesInstance>
{
	// Token: 0x06004D71 RID: 19825 RVA: 0x001C2A10 File Offset: 0x001C0C10
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06004D72 RID: 19826 RVA: 0x001C2A20 File Offset: 0x001C0C20
	protected bool IsUncomfortable()
	{
		ClothingWearer component = base.GetComponent<ClothingWearer>();
		return component != null && component.currentClothing.decorMod <= 0;
	}

	// Token: 0x02001B85 RID: 7045
	public class StatesInstance : GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.GameInstance
	{
		// Token: 0x0600AA43 RID: 43587 RVA: 0x003C3B72 File Offset: 0x003C1D72
		public StatesInstance(Fashionable master) : base(master)
		{
		}
	}

	// Token: 0x02001B86 RID: 7046
	public class States : GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable>
	{
		// Token: 0x0600AA44 RID: 43588 RVA: 0x003C3B7C File Offset: 0x003C1D7C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.EventHandler(GameHashes.EquippedItemEquipper, delegate(Fashionable.StatesInstance smi)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}).EventHandler(GameHashes.UnequippedItemEquipper, delegate(Fashionable.StatesInstance smi)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			});
			this.suffering.AddEffect("UnfashionableClothing").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x0400853D RID: 34109
		public GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State satisfied;

		// Token: 0x0400853E RID: 34110
		public GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State suffering;
	}
}
