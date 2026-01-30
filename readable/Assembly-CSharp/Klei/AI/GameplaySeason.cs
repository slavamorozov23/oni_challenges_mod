using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Klei.AI
{
	// Token: 0x02001043 RID: 4163
	[DebuggerDisplay("{base.Id}")]
	public class GameplaySeason : Resource, IHasDlcRestrictions
	{
		// Token: 0x06008110 RID: 33040 RVA: 0x0033DCA1 File Offset: 0x0033BEA1
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x06008111 RID: 33041 RVA: 0x0033DCA9 File Offset: 0x0033BEA9
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x06008112 RID: 33042 RVA: 0x0033DCB4 File Offset: 0x0033BEB4
		public GameplaySeason(string id, GameplaySeason.Type type, float period, bool synchronizedToPeriod, float randomizedEventStartTime = -1f, bool startActive = false, int finishAfterNumEvents = -1, float minCycle = 0f, float maxCycle = float.PositiveInfinity, int numEventsToStartEachPeriod = 1, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(id, null, null)
		{
			this.type = type;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
			this.period = period;
			this.synchronizedToPeriod = synchronizedToPeriod;
			global::Debug.Assert(period > 0f, "Season " + id + "'s Period cannot be 0 or negative");
			if (randomizedEventStartTime == -1f)
			{
				this.randomizedEventStartTime = new MathUtil.MinMax(--0f * period, 0f * period);
			}
			else
			{
				this.randomizedEventStartTime = new MathUtil.MinMax(-randomizedEventStartTime, randomizedEventStartTime);
				DebugUtil.DevAssert((this.randomizedEventStartTime.max - this.randomizedEventStartTime.min) * 0.4f < period, string.Format("Season {0} randomizedEventStartTime is greater than {1}% of its period.", id, 0.4f), null);
			}
			this.startActive = startActive;
			this.finishAfterNumEvents = finishAfterNumEvents;
			this.minCycle = minCycle;
			this.maxCycle = maxCycle;
			this.events = new List<GameplayEvent>();
			this.numEventsToStartEachPeriod = numEventsToStartEachPeriod;
		}

		// Token: 0x06008113 RID: 33043 RVA: 0x0033DDB8 File Offset: 0x0033BFB8
		[Obsolete]
		public GameplaySeason(string id, GameplaySeason.Type type, string dlcId, float period, bool synchronizedToPeriod, float randomizedEventStartTime = -1f, bool startActive = false, int finishAfterNumEvents = -1, float minCycle = 0f, float maxCycle = float.PositiveInfinity, int numEventsToStartEachPeriod = 1) : this(id, type, period, synchronizedToPeriod, randomizedEventStartTime, startActive, finishAfterNumEvents, minCycle, maxCycle, numEventsToStartEachPeriod, new string[]
		{
			dlcId
		}, null)
		{
		}

		// Token: 0x06008114 RID: 33044 RVA: 0x0033DDE8 File Offset: 0x0033BFE8
		public virtual void AdditionalEventInstanceSetup(StateMachine.Instance generic_smi)
		{
		}

		// Token: 0x06008115 RID: 33045 RVA: 0x0033DDEA File Offset: 0x0033BFEA
		public virtual float GetSeasonPeriod()
		{
			return this.period;
		}

		// Token: 0x06008116 RID: 33046 RVA: 0x0033DDF2 File Offset: 0x0033BFF2
		public GameplaySeason AddEvent(GameplayEvent evt)
		{
			this.events.Add(evt);
			return this;
		}

		// Token: 0x06008117 RID: 33047 RVA: 0x0033DE01 File Offset: 0x0033C001
		public virtual GameplaySeasonInstance Instantiate(int worldId)
		{
			return new GameplaySeasonInstance(this, worldId);
		}

		// Token: 0x040061BB RID: 25019
		public const float DEFAULT_PERCENTAGE_RANDOMIZED_EVENT_START = 0f;

		// Token: 0x040061BC RID: 25020
		public const float PERCENTAGE_WARNING = 0.4f;

		// Token: 0x040061BD RID: 25021
		public const float USE_DEFAULT = -1f;

		// Token: 0x040061BE RID: 25022
		public const int INFINITE = -1;

		// Token: 0x040061BF RID: 25023
		public float period;

		// Token: 0x040061C0 RID: 25024
		public bool synchronizedToPeriod;

		// Token: 0x040061C1 RID: 25025
		public MathUtil.MinMax randomizedEventStartTime;

		// Token: 0x040061C2 RID: 25026
		public int finishAfterNumEvents = -1;

		// Token: 0x040061C3 RID: 25027
		public bool startActive;

		// Token: 0x040061C4 RID: 25028
		public int numEventsToStartEachPeriod;

		// Token: 0x040061C5 RID: 25029
		public float minCycle;

		// Token: 0x040061C6 RID: 25030
		public float maxCycle;

		// Token: 0x040061C7 RID: 25031
		public List<GameplayEvent> events;

		// Token: 0x040061C8 RID: 25032
		private string[] requiredDlcIds;

		// Token: 0x040061C9 RID: 25033
		private string[] forbiddenDlcIds;

		// Token: 0x040061CA RID: 25034
		public GameplaySeason.Type type;

		// Token: 0x040061CB RID: 25035
		[Obsolete]
		public string dlcId;

		// Token: 0x02002740 RID: 10048
		public enum Type
		{
			// Token: 0x0400AEB2 RID: 44722
			World,
			// Token: 0x0400AEB3 RID: 44723
			Cluster
		}
	}
}
