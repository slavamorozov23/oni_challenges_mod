using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000F90 RID: 3984
	public class AutomateABuilding : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DCC RID: 32204 RVA: 0x0031F788 File Offset: 0x0031D988
		public override bool Success()
		{
			foreach (UtilityNetwork utilityNetwork in Game.Instance.logicCircuitSystem.GetNetworks())
			{
				LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork;
				if (logicCircuitNetwork.Receivers.Count > 0 && logicCircuitNetwork.Senders.Count > 0)
				{
					bool flag = false;
					foreach (ILogicEventReceiver logicEventReceiver in logicCircuitNetwork.Receivers)
					{
						if (!logicEventReceiver.IsNullOrDestroyed())
						{
							GameObject gameObject = Grid.Objects[logicEventReceiver.GetLogicCell(), 1];
							if (gameObject != null && !gameObject.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
							{
								flag = true;
								break;
							}
						}
					}
					bool flag2 = false;
					foreach (ILogicEventSender logicEventSender in logicCircuitNetwork.Senders)
					{
						if (!logicEventSender.IsNullOrDestroyed())
						{
							GameObject gameObject2 = Grid.Objects[logicEventSender.GetLogicCell(), 1];
							if (gameObject2 != null && !gameObject2.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
							{
								flag2 = true;
								break;
							}
						}
					}
					if (flag && flag2)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007DCD RID: 32205 RVA: 0x0031F930 File Offset: 0x0031DB30
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06007DCE RID: 32206 RVA: 0x0031F932 File Offset: 0x0031DB32
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.AUTOMATE_A_BUILDING;
		}
	}
}
