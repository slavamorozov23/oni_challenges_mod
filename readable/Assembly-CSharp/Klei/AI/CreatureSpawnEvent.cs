using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001040 RID: 4160
	public class CreatureSpawnEvent : GameplayEvent<CreatureSpawnEvent.StatesInstance>
	{
		// Token: 0x06008109 RID: 33033 RVA: 0x0033DB80 File Offset: 0x0033BD80
		public CreatureSpawnEvent() : base("HatchSpawnEvent", 0, 0, null, null)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.CREATURE_SPAWN.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.CREATURE_SPAWN.DESCRIPTION;
		}

		// Token: 0x0600810A RID: 33034 RVA: 0x0033DBB1 File Offset: 0x0033BDB1
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new CreatureSpawnEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x040061B2 RID: 25010
		public const string ID = "HatchSpawnEvent";

		// Token: 0x040061B3 RID: 25011
		public const float UPDATE_TIME = 4f;

		// Token: 0x040061B4 RID: 25012
		public const float NUM_TO_SPAWN = 10f;

		// Token: 0x040061B5 RID: 25013
		public const float duration = 40f;

		// Token: 0x040061B6 RID: 25014
		public static List<string> CreatureSpawnEventIDs = new List<string>
		{
			"Hatch",
			"Squirrel",
			"Puft",
			"Crab",
			"Drecko",
			"Mole",
			"LightBug",
			"Pacu"
		};

		// Token: 0x0200273A RID: 10042
		public class StatesInstance : GameplayEventStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, CreatureSpawnEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600C835 RID: 51253 RVA: 0x00425F4C File Offset: 0x0042414C
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, CreatureSpawnEvent creatureEvent) : base(master, eventInstance, creatureEvent)
			{
			}

			// Token: 0x0600C836 RID: 51254 RVA: 0x00425F62 File Offset: 0x00424162
			private void PickCreatureToSpawn()
			{
				this.creatureID = CreatureSpawnEvent.CreatureSpawnEventIDs.GetRandom<string>();
			}

			// Token: 0x0600C837 RID: 51255 RVA: 0x00425F74 File Offset: 0x00424174
			private void PickSpawnLocations()
			{
				Vector3 position = Components.Telepads.Items.GetRandom<Telepad>().transform.GetPosition();
				int num = 100;
				ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
				GameScenePartitioner.Instance.GatherEntries((int)position.x - num / 2, (int)position.y - num / 2, num, num, GameScenePartitioner.Instance.plants, pooledList);
				foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
				{
					KPrefabID kprefabID = (KPrefabID)scenePartitionerEntry.obj;
					if (!kprefabID.GetComponent<TreeBud>())
					{
						base.smi.spawnPositions.Add(kprefabID.transform.GetPosition());
					}
				}
				pooledList.Recycle();
			}

			// Token: 0x0600C838 RID: 51256 RVA: 0x00426048 File Offset: 0x00424248
			public void InitializeEvent()
			{
				this.PickCreatureToSpawn();
				this.PickSpawnLocations();
			}

			// Token: 0x0600C839 RID: 51257 RVA: 0x00426056 File Offset: 0x00424256
			public void EndEvent()
			{
				this.creatureID = null;
				this.spawnPositions.Clear();
			}

			// Token: 0x0600C83A RID: 51258 RVA: 0x0042606C File Offset: 0x0042426C
			public void SpawnCreature()
			{
				if (this.spawnPositions.Count > 0)
				{
					Vector3 random = this.spawnPositions.GetRandom<Vector3>();
					Util.KInstantiate(Assets.GetPrefab(this.creatureID), random).SetActive(true);
				}
			}

			// Token: 0x0400AEA3 RID: 44707
			[Serialize]
			private List<Vector3> spawnPositions = new List<Vector3>();

			// Token: 0x0400AEA4 RID: 44708
			[Serialize]
			private string creatureID;
		}

		// Token: 0x0200273B RID: 10043
		public class States : GameplayEventStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, CreatureSpawnEvent>
		{
			// Token: 0x0600C83B RID: 51259 RVA: 0x004260B0 File Offset: 0x004242B0
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.initialize_event;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.initialize_event.Enter(delegate(CreatureSpawnEvent.StatesInstance smi)
				{
					smi.InitializeEvent();
					smi.GoTo(this.spawn_season);
				});
				this.start.DoNothing();
				this.spawn_season.Update(delegate(CreatureSpawnEvent.StatesInstance smi, float dt)
				{
					smi.SpawnCreature();
				}, UpdateRate.SIM_4000ms, false).Exit(delegate(CreatureSpawnEvent.StatesInstance smi)
				{
					smi.EndEvent();
				});
			}

			// Token: 0x0600C83C RID: 51260 RVA: 0x00426144 File Offset: 0x00424344
			public override EventInfoData GenerateEventPopupData(CreatureSpawnEvent.StatesInstance smi)
			{
				return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
				{
					location = GAMEPLAY_EVENTS.LOCATIONS.PRINTING_POD,
					whenDescription = GAMEPLAY_EVENTS.TIMES.NOW
				};
			}

			// Token: 0x0400AEA5 RID: 44709
			public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State initialize_event;

			// Token: 0x0400AEA6 RID: 44710
			public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State spawn_season;

			// Token: 0x0400AEA7 RID: 44711
			public GameStateMachine<CreatureSpawnEvent.States, CreatureSpawnEvent.StatesInstance, GameplayEventManager, object>.State start;
		}
	}
}
