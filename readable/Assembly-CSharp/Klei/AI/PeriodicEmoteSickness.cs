using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x0200103B RID: 4155
	public class PeriodicEmoteSickness : Sickness.SicknessComponent
	{
		// Token: 0x060080CA RID: 32970 RVA: 0x0033C1EC File Offset: 0x0033A3EC
		public PeriodicEmoteSickness(Emote emote, float cooldown)
		{
			this.emote = emote;
			this.cooldown = cooldown;
		}

		// Token: 0x060080CB RID: 32971 RVA: 0x0033C202 File Offset: 0x0033A402
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			PeriodicEmoteSickness.StatesInstance statesInstance = new PeriodicEmoteSickness.StatesInstance(diseaseInstance, this);
			statesInstance.StartSM();
			return statesInstance;
		}

		// Token: 0x060080CC RID: 32972 RVA: 0x0033C211 File Offset: 0x0033A411
		public override void OnCure(GameObject go, object instance_data)
		{
			((PeriodicEmoteSickness.StatesInstance)instance_data).StopSM("Cured");
		}

		// Token: 0x0400618C RID: 24972
		private Emote emote;

		// Token: 0x0400618D RID: 24973
		private float cooldown;

		// Token: 0x02002730 RID: 10032
		public class StatesInstance : GameStateMachine<PeriodicEmoteSickness.States, PeriodicEmoteSickness.StatesInstance, SicknessInstance, object>.GameInstance
		{
			// Token: 0x0600C81A RID: 51226 RVA: 0x00425544 File Offset: 0x00423744
			public StatesInstance(SicknessInstance master, PeriodicEmoteSickness periodicEmoteSickness) : base(master)
			{
				this.periodicEmoteSickness = periodicEmoteSickness;
			}

			// Token: 0x0600C81B RID: 51227 RVA: 0x00425554 File Offset: 0x00423754
			public Reactable GetReactable()
			{
				return new SelfEmoteReactable(base.master.gameObject, "PeriodicEmoteSickness", Db.Get().ChoreTypes.Emote, 0f, this.periodicEmoteSickness.cooldown, float.PositiveInfinity, 0f).SetEmote(this.periodicEmoteSickness.emote).SetOverideAnimSet("anim_sneeze_kanim");
			}

			// Token: 0x0400AE83 RID: 44675
			public PeriodicEmoteSickness periodicEmoteSickness;
		}

		// Token: 0x02002731 RID: 10033
		public class States : GameStateMachine<PeriodicEmoteSickness.States, PeriodicEmoteSickness.StatesInstance, SicknessInstance>
		{
			// Token: 0x0600C81C RID: 51228 RVA: 0x004255BE File Offset: 0x004237BE
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.root;
				this.root.ToggleReactable((PeriodicEmoteSickness.StatesInstance smi) => smi.GetReactable());
			}
		}
	}
}
