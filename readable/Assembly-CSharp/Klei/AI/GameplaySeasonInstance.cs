using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001044 RID: 4164
	[SerializationConfig(MemberSerialization.OptIn)]
	public class GameplaySeasonInstance : ISaveLoadable
	{
		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06008118 RID: 33048 RVA: 0x0033DE0A File Offset: 0x0033C00A
		public float NextEventTime
		{
			get
			{
				return this.nextPeriodTime + this.randomizedNextTime;
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06008119 RID: 33049 RVA: 0x0033DE19 File Offset: 0x0033C019
		public GameplaySeason Season
		{
			get
			{
				if (this._season == null)
				{
					this._season = Db.Get().GameplaySeasons.TryGet(this.seasonId);
				}
				return this._season;
			}
		}

		// Token: 0x0600811A RID: 33050 RVA: 0x0033DE44 File Offset: 0x0033C044
		public GameplaySeasonInstance(GameplaySeason season, int worldId)
		{
			this.seasonId = season.Id;
			this.worldId = worldId;
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			if (season.synchronizedToPeriod)
			{
				float seasonPeriod = this.Season.GetSeasonPeriod();
				this.nextPeriodTime = (Mathf.Floor(currentTimeInCycles / seasonPeriod) + 1f) * seasonPeriod;
			}
			else
			{
				this.nextPeriodTime = currentTimeInCycles;
			}
			this.CalculateNextEventTime();
		}

		// Token: 0x0600811B RID: 33051 RVA: 0x0033DEAC File Offset: 0x0033C0AC
		private void CalculateNextEventTime()
		{
			float seasonPeriod = this.Season.GetSeasonPeriod();
			this.randomizedNextTime = UnityEngine.Random.Range(this.Season.randomizedEventStartTime.min, this.Season.randomizedEventStartTime.max);
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			float num = this.nextPeriodTime + this.randomizedNextTime;
			while (num < currentTimeInCycles || num < this.Season.minCycle)
			{
				this.nextPeriodTime += seasonPeriod;
				num = this.nextPeriodTime + this.randomizedNextTime;
			}
		}

		// Token: 0x0600811C RID: 33052 RVA: 0x0033DF34 File Offset: 0x0033C134
		public bool StartEvent(bool ignorePreconditions = false)
		{
			bool result = false;
			this.CalculateNextEventTime();
			this.numStartEvents++;
			List<GameplayEvent> list;
			if (!ignorePreconditions)
			{
				list = (from x in this.Season.events
				where x.IsAllowed()
				select x).ToList<GameplayEvent>();
			}
			else
			{
				list = this.Season.events;
			}
			List<GameplayEvent> list2 = list;
			if (list2.Count > 0)
			{
				list2.ForEach(delegate(GameplayEvent x)
				{
					x.CalculatePriority();
				});
				list2.Sort();
				int maxExclusive = Mathf.Min(list2.Count, 5);
				GameplayEvent eventType = list2[UnityEngine.Random.Range(0, maxExclusive)];
				GameplayEventManager.Instance.StartNewEvent(eventType, this.worldId, new Action<StateMachine.Instance>(this.Season.AdditionalEventInstanceSetup));
				result = true;
			}
			this.allEventWillNotRunAgain = true;
			using (List<GameplayEvent>.Enumerator enumerator = this.Season.events.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.WillNeverRunAgain())
					{
						this.allEventWillNotRunAgain = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600811D RID: 33053 RVA: 0x0033E070 File Offset: 0x0033C270
		public bool ShouldGenerateEvents()
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(this.worldId);
			if (!world.IsDupeVisited && !world.IsRoverVisted)
			{
				return false;
			}
			if ((this.Season.finishAfterNumEvents != -1 && this.numStartEvents >= this.Season.finishAfterNumEvents) || this.allEventWillNotRunAgain)
			{
				return false;
			}
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			return currentTimeInCycles > this.Season.minCycle && currentTimeInCycles < this.Season.maxCycle;
		}

		// Token: 0x040061CC RID: 25036
		public const int LIMIT_SELECTION = 5;

		// Token: 0x040061CD RID: 25037
		[Serialize]
		public int numStartEvents;

		// Token: 0x040061CE RID: 25038
		[Serialize]
		public int worldId;

		// Token: 0x040061CF RID: 25039
		[Serialize]
		private readonly string seasonId;

		// Token: 0x040061D0 RID: 25040
		[Serialize]
		private float nextPeriodTime;

		// Token: 0x040061D1 RID: 25041
		[Serialize]
		private float randomizedNextTime;

		// Token: 0x040061D2 RID: 25042
		private bool allEventWillNotRunAgain;

		// Token: 0x040061D3 RID: 25043
		private GameplaySeason _season;
	}
}
