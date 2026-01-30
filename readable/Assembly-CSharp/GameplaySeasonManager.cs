using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;

// Token: 0x02000969 RID: 2409
public class GameplaySeasonManager : GameStateMachine<GameplaySeasonManager, GameplaySeasonManager.Instance, IStateMachineTarget, GameplaySeasonManager.Def>
{
	// Token: 0x06004476 RID: 17526 RVA: 0x0018B4C0 File Offset: 0x001896C0
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.root;
		this.root.Enter(delegate(GameplaySeasonManager.Instance smi)
		{
			smi.Initialize();
		}).Update(delegate(GameplaySeasonManager.Instance smi, float dt)
		{
			smi.Update(dt);
		}, UpdateRate.SIM_4000ms, false);
	}

	// Token: 0x0200199B RID: 6555
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200199C RID: 6556
	public new class Instance : GameStateMachine<GameplaySeasonManager, GameplaySeasonManager.Instance, IStateMachineTarget, GameplaySeasonManager.Def>.GameInstance
	{
		// Token: 0x0600A29D RID: 41629 RVA: 0x003AF676 File Offset: 0x003AD876
		public Instance(IStateMachineTarget master, GameplaySeasonManager.Def def) : base(master, def)
		{
			this.activeSeasons = new List<GameplaySeasonInstance>();
		}

		// Token: 0x0600A29E RID: 41630 RVA: 0x003AF68C File Offset: 0x003AD88C
		public void Initialize()
		{
			this.activeSeasons.RemoveAll((GameplaySeasonInstance item) => item.Season == null);
			List<GameplaySeason> list = new List<GameplaySeason>();
			if (this.m_worldContainer != null)
			{
				ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
				using (List<string>.Enumerator enumerator = this.m_worldContainer.GetSeasonIds().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						GameplaySeason gameplaySeason = Db.Get().GameplaySeasons.TryGet(text);
						if (gameplaySeason == null)
						{
							Debug.LogWarning("world " + component.name + " has invalid season " + text);
						}
						else
						{
							if (gameplaySeason.type != GameplaySeason.Type.World)
							{
								Debug.LogWarning(string.Concat(new string[]
								{
									"world ",
									component.name,
									" has specified season ",
									text,
									", which is not a world type season"
								}));
							}
							list.Add(gameplaySeason);
						}
					}
					goto IL_146;
				}
			}
			Debug.Assert(base.GetComponent<SaveGame>() != null);
			list = (from season in Db.Get().GameplaySeasons.resources
			where season.type == GameplaySeason.Type.Cluster
			select season).ToList<GameplaySeason>();
			IL_146:
			foreach (GameplaySeason gameplaySeason2 in list)
			{
				if (Game.IsCorrectDlcActiveForCurrentSave(gameplaySeason2) && gameplaySeason2.startActive && !this.SeasonExists(gameplaySeason2) && gameplaySeason2.events.Count > 0)
				{
					this.activeSeasons.Add(gameplaySeason2.Instantiate(this.GetWorldId()));
				}
			}
			foreach (GameplaySeasonInstance gameplaySeasonInstance in new List<GameplaySeasonInstance>(this.activeSeasons))
			{
				if (!list.Contains(gameplaySeasonInstance.Season) || !Game.IsCorrectDlcActiveForCurrentSave(gameplaySeasonInstance.Season))
				{
					this.activeSeasons.Remove(gameplaySeasonInstance);
				}
			}
		}

		// Token: 0x0600A29F RID: 41631 RVA: 0x003AF8D8 File Offset: 0x003ADAD8
		private int GetWorldId()
		{
			if (this.m_worldContainer != null)
			{
				return this.m_worldContainer.id;
			}
			return -1;
		}

		// Token: 0x0600A2A0 RID: 41632 RVA: 0x003AF8F8 File Offset: 0x003ADAF8
		public void Update(float dt)
		{
			foreach (GameplaySeasonInstance gameplaySeasonInstance in this.activeSeasons)
			{
				if (gameplaySeasonInstance.ShouldGenerateEvents() && GameUtil.GetCurrentTimeInCycles() > gameplaySeasonInstance.NextEventTime)
				{
					int num = 0;
					while (num < gameplaySeasonInstance.Season.numEventsToStartEachPeriod && gameplaySeasonInstance.StartEvent(false))
					{
						num++;
					}
				}
			}
		}

		// Token: 0x0600A2A1 RID: 41633 RVA: 0x003AF978 File Offset: 0x003ADB78
		public void StartNewSeason(GameplaySeason seasonType)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(seasonType))
			{
				this.activeSeasons.Add(seasonType.Instantiate(this.GetWorldId()));
			}
		}

		// Token: 0x0600A2A2 RID: 41634 RVA: 0x003AF99C File Offset: 0x003ADB9C
		public bool SeasonExists(GameplaySeason seasonType)
		{
			return this.activeSeasons.Find((GameplaySeasonInstance e) => e.Season.IdHash == seasonType.IdHash) != null;
		}

		// Token: 0x04007EBE RID: 32446
		[Serialize]
		public List<GameplaySeasonInstance> activeSeasons;

		// Token: 0x04007EBF RID: 32447
		[MyCmpGet]
		private WorldContainer m_worldContainer;
	}
}
