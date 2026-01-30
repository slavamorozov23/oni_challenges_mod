using System;
using UnityEngine;

// Token: 0x02000838 RID: 2104
public class CargoDropperStorage : GameStateMachine<CargoDropperStorage, CargoDropperStorage.StatesInstance, IStateMachineTarget, CargoDropperStorage.Def>
{
	// Token: 0x06003963 RID: 14691 RVA: 0x0014069D File Offset: 0x0013E89D
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.JettisonCargo, delegate(CargoDropperStorage.StatesInstance smi, object data)
		{
			smi.JettisonCargo(data);
		});
	}

	// Token: 0x020017DF RID: 6111
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007901 RID: 30977
		public Vector3 dropOffset;
	}

	// Token: 0x020017E0 RID: 6112
	public class StatesInstance : GameStateMachine<CargoDropperStorage, CargoDropperStorage.StatesInstance, IStateMachineTarget, CargoDropperStorage.Def>.GameInstance
	{
		// Token: 0x06009CD3 RID: 40147 RVA: 0x0039B414 File Offset: 0x00399614
		public StatesInstance(IStateMachineTarget master, CargoDropperStorage.Def def) : base(master, def)
		{
		}

		// Token: 0x06009CD4 RID: 40148 RVA: 0x0039B420 File Offset: 0x00399620
		public void JettisonCargo(object data)
		{
			Vector3 position = base.master.transform.GetPosition() + base.def.dropOffset;
			Storage component = base.GetComponent<Storage>();
			if (component != null)
			{
				GameObject gameObject = component.FindFirst("ScoutRover");
				if (gameObject != null)
				{
					component.Drop(gameObject, true);
					Vector3 position2 = base.master.transform.GetPosition();
					position2.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
					gameObject.transform.SetPosition(position2);
					ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
					if (component2 != null)
					{
						KBatchedAnimController component3 = gameObject.GetComponent<KBatchedAnimController>();
						if (component3 != null)
						{
							component3.Play("enter", KAnim.PlayMode.Once, 1f, 0f);
						}
						new EmoteChore(component2, Db.Get().ChoreTypes.EmoteHighPriority, null, new HashedString[]
						{
							"enter"
						}, KAnim.PlayMode.Once, false);
					}
					gameObject.GetMyWorld().SetRoverLanded();
				}
				component.DropAll(position, false, false, default(Vector3), true, null);
			}
		}
	}
}
