using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F8D RID: 3981
	public class VentXKG : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DBE RID: 32190 RVA: 0x0031F267 File Offset: 0x0031D467
		public VentXKG(SimHashes element, float kilogramsToVent)
		{
			this.element = element;
			this.kilogramsToVent = kilogramsToVent;
		}

		// Token: 0x06007DBF RID: 32191 RVA: 0x0031F280 File Offset: 0x0031D480
		public override bool Success()
		{
			float num = 0f;
			foreach (UtilityNetwork utilityNetwork in Conduit.GetNetworkManager(ConduitType.Gas).GetNetworks())
			{
				FlowUtilityNetwork flowUtilityNetwork = utilityNetwork as FlowUtilityNetwork;
				if (flowUtilityNetwork != null)
				{
					foreach (FlowUtilityNetwork.IItem item in flowUtilityNetwork.sinks)
					{
						Vent component = item.GameObject.GetComponent<Vent>();
						if (component != null)
						{
							num += component.GetVentedMass(this.element);
						}
					}
				}
			}
			return num >= this.kilogramsToVent;
		}

		// Token: 0x06007DC0 RID: 32192 RVA: 0x0031F348 File Offset: 0x0031D548
		public void Deserialize(IReader reader)
		{
			this.element = (SimHashes)reader.ReadInt32();
			this.kilogramsToVent = reader.ReadSingle();
		}

		// Token: 0x06007DC1 RID: 32193 RVA: 0x0031F364 File Offset: 0x0031D564
		public override string GetProgress(bool complete)
		{
			float num = 0f;
			foreach (UtilityNetwork utilityNetwork in Conduit.GetNetworkManager(ConduitType.Gas).GetNetworks())
			{
				FlowUtilityNetwork flowUtilityNetwork = utilityNetwork as FlowUtilityNetwork;
				if (flowUtilityNetwork != null)
				{
					foreach (FlowUtilityNetwork.IItem item in flowUtilityNetwork.sinks)
					{
						Vent component = item.GameObject.GetComponent<Vent>();
						if (component != null)
						{
							num += component.GetVentedMass(this.element);
						}
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.VENTED_MASS, GameUtil.GetFormattedMass(complete ? this.kilogramsToVent : num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), GameUtil.GetFormattedMass(this.kilogramsToVent, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"));
		}

		// Token: 0x04005C49 RID: 23625
		private SimHashes element;

		// Token: 0x04005C4A RID: 23626
		private float kilogramsToVent;
	}
}
