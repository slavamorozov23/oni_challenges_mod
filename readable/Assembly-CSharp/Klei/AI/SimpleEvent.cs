using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x0200104A RID: 4170
	public class SimpleEvent : GameplayEvent<SimpleEvent.StatesInstance>
	{
		// Token: 0x0600812F RID: 33071 RVA: 0x0033E40F File Offset: 0x0033C60F
		public SimpleEvent(string id, string title, string description, string animFileName, string buttonText = null, string buttonTooltip = null) : base(id, 0, 0, null, null)
		{
			this.title = title;
			this.description = description;
			this.buttonText = buttonText;
			this.buttonTooltip = buttonTooltip;
			this.animFileName = animFileName;
		}

		// Token: 0x06008130 RID: 33072 RVA: 0x0033E447 File Offset: 0x0033C647
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new SimpleEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x040061E4 RID: 25060
		private string buttonText;

		// Token: 0x040061E5 RID: 25061
		private string buttonTooltip;

		// Token: 0x0200274B RID: 10059
		public class States : GameplayEventStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, SimpleEvent>
		{
			// Token: 0x0600C88E RID: 51342 RVA: 0x00428276 File Offset: 0x00426476
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.root;
				this.ending.ReturnSuccess();
			}

			// Token: 0x0600C88F RID: 51343 RVA: 0x0042828C File Offset: 0x0042648C
			public override EventInfoData GenerateEventPopupData(SimpleEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				eventInfoData.minions = smi.minions;
				eventInfoData.artifact = smi.artifact;
				EventInfoData.Option option = eventInfoData.AddOption(smi.gameplayEvent.buttonText, null);
				option.callback = delegate()
				{
					if (smi.callback != null)
					{
						smi.callback();
					}
					smi.StopSM("SimpleEvent Finished");
				};
				option.tooltip = smi.gameplayEvent.buttonTooltip;
				if (smi.textParameters != null)
				{
					foreach (global::Tuple<string, string> tuple in smi.textParameters)
					{
						eventInfoData.SetTextParameter(tuple.first, tuple.second);
					}
				}
				return eventInfoData;
			}

			// Token: 0x0400AEDA RID: 44762
			public GameStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, object>.State ending;
		}

		// Token: 0x0200274C RID: 10060
		public class StatesInstance : GameplayEventStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, SimpleEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600C891 RID: 51345 RVA: 0x004283A8 File Offset: 0x004265A8
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SimpleEvent simpleEvent) : base(master, eventInstance, simpleEvent)
			{
			}

			// Token: 0x0600C892 RID: 51346 RVA: 0x004283B3 File Offset: 0x004265B3
			public void SetTextParameter(string key, string value)
			{
				if (this.textParameters == null)
				{
					this.textParameters = new List<global::Tuple<string, string>>();
				}
				this.textParameters.Add(new global::Tuple<string, string>(key, value));
			}

			// Token: 0x0600C893 RID: 51347 RVA: 0x004283DA File Offset: 0x004265DA
			public void ShowEventPopup()
			{
				EventInfoScreen.ShowPopup(base.smi.sm.GenerateEventPopupData(base.smi));
			}

			// Token: 0x0400AEDB RID: 44763
			public GameObject[] minions;

			// Token: 0x0400AEDC RID: 44764
			public GameObject artifact;

			// Token: 0x0400AEDD RID: 44765
			public List<global::Tuple<string, string>> textParameters;

			// Token: 0x0400AEDE RID: 44766
			public System.Action callback;
		}
	}
}
