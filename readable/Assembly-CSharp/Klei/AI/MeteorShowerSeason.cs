using System;
using System.Diagnostics;
using Klei.CustomSettings;

namespace Klei.AI
{
	// Token: 0x02001046 RID: 4166
	[DebuggerDisplay("{base.Id}")]
	public class MeteorShowerSeason : GameplaySeason
	{
		// Token: 0x06008125 RID: 33061 RVA: 0x0033E204 File Offset: 0x0033C404
		public MeteorShowerSeason(string id, GameplaySeason.Type type, float period, bool synchronizedToPeriod, float randomizedEventStartTime = -1f, bool startActive = false, int finishAfterNumEvents = -1, float minCycle = 0f, float maxCycle = float.PositiveInfinity, int numEventsToStartEachPeriod = 1, bool affectedByDifficultySettings = true, float clusterTravelDuration = -1f, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(id, type, period, synchronizedToPeriod, randomizedEventStartTime, startActive, finishAfterNumEvents, minCycle, maxCycle, numEventsToStartEachPeriod, requiredDlcIds, forbiddenDlcIds)
		{
			this.affectedByDifficultySettings = affectedByDifficultySettings;
			this.clusterTravelDuration = clusterTravelDuration;
		}

		// Token: 0x06008126 RID: 33062 RVA: 0x0033E250 File Offset: 0x0033C450
		[Obsolete]
		public MeteorShowerSeason(string id, GameplaySeason.Type type, string dlcId, float period, bool synchronizedToPeriod, float randomizedEventStartTime = -1f, bool startActive = false, int finishAfterNumEvents = -1, float minCycle = 0f, float maxCycle = float.PositiveInfinity, int numEventsToStartEachPeriod = 1, bool affectedByDifficultySettings = true, float clusterTravelDuration = -1f) : base(id, type, period, synchronizedToPeriod, randomizedEventStartTime, startActive, finishAfterNumEvents, minCycle, maxCycle, numEventsToStartEachPeriod, new string[]
		{
			dlcId
		}, null)
		{
		}

		// Token: 0x06008127 RID: 33063 RVA: 0x0033E292 File Offset: 0x0033C492
		public override void AdditionalEventInstanceSetup(StateMachine.Instance generic_smi)
		{
			(generic_smi as MeteorShowerEvent.StatesInstance).clusterTravelDuration = this.clusterTravelDuration;
		}

		// Token: 0x06008128 RID: 33064 RVA: 0x0033E2A8 File Offset: 0x0033C4A8
		public override float GetSeasonPeriod()
		{
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
			float num = base.GetSeasonPeriod();
			if (this.affectedByDifficultySettings && currentQualitySetting != null)
			{
				string id = currentQualitySetting.id;
				if (!(id == "Infrequent"))
				{
					if (!(id == "Intense"))
					{
						if (id == "Doomed")
						{
							num *= 1f;
						}
					}
					else
					{
						num *= 1f;
					}
				}
				else
				{
					num *= 2f;
				}
			}
			return num;
		}

		// Token: 0x040061DB RID: 25051
		public bool affectedByDifficultySettings = true;

		// Token: 0x040061DC RID: 25052
		public float clusterTravelDuration = -1f;
	}
}
