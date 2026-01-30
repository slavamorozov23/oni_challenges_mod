using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001045 RID: 4165
	public class MeteorShowerEvent : GameplayEvent<MeteorShowerEvent.StatesInstance>
	{
		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x0600811E RID: 33054 RVA: 0x0033E0F2 File Offset: 0x0033C2F2
		public bool canStarTravel
		{
			get
			{
				return this.clusterMapMeteorShowerID != null && DlcManager.FeatureClusterSpaceEnabled();
			}
		}

		// Token: 0x0600811F RID: 33055 RVA: 0x0033E103 File Offset: 0x0033C303
		public string GetClusterMapMeteorShowerID()
		{
			return this.clusterMapMeteorShowerID;
		}

		// Token: 0x06008120 RID: 33056 RVA: 0x0033E10B File Offset: 0x0033C30B
		public List<MeteorShowerEvent.BombardmentInfo> GetMeteorsInfo()
		{
			return new List<MeteorShowerEvent.BombardmentInfo>(this.bombardmentInfo);
		}

		// Token: 0x06008121 RID: 33057 RVA: 0x0033E118 File Offset: 0x0033C318
		public MeteorShowerEvent(string id, float duration, float secondsPerMeteor, MathUtil.MinMax secondsBombardmentOff = default(MathUtil.MinMax), MathUtil.MinMax secondsBombardmentOn = default(MathUtil.MinMax), string clusterMapMeteorShowerID = null, bool affectedByDifficulty = true) : base(id, 0, 0, null, null)
		{
			this.allowMultipleEventInstances = true;
			this.clusterMapMeteorShowerID = clusterMapMeteorShowerID;
			this.duration = duration;
			this.secondsPerMeteor = secondsPerMeteor;
			this.secondsBombardmentOff = secondsBombardmentOff;
			this.secondsBombardmentOn = secondsBombardmentOn;
			this.affectedByDifficulty = affectedByDifficulty;
			this.bombardmentInfo = new List<MeteorShowerEvent.BombardmentInfo>();
			this.tags.Add(GameTags.SpaceDanger);
		}

		// Token: 0x06008122 RID: 33058 RVA: 0x0033E194 File Offset: 0x0033C394
		public MeteorShowerEvent AddMeteor(string prefab, float weight)
		{
			this.bombardmentInfo.Add(new MeteorShowerEvent.BombardmentInfo
			{
				prefab = prefab,
				weight = weight
			});
			return this;
		}

		// Token: 0x06008123 RID: 33059 RVA: 0x0033E1C6 File Offset: 0x0033C3C6
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new MeteorShowerEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x06008124 RID: 33060 RVA: 0x0033E1D0 File Offset: 0x0033C3D0
		public override bool IsAllowed()
		{
			return base.IsAllowed() && (!this.affectedByDifficulty || CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers).id != "ClearSkies");
		}

		// Token: 0x040061D4 RID: 25044
		private List<MeteorShowerEvent.BombardmentInfo> bombardmentInfo;

		// Token: 0x040061D5 RID: 25045
		private MathUtil.MinMax secondsBombardmentOff;

		// Token: 0x040061D6 RID: 25046
		private MathUtil.MinMax secondsBombardmentOn;

		// Token: 0x040061D7 RID: 25047
		private float secondsPerMeteor = 0.33f;

		// Token: 0x040061D8 RID: 25048
		private float duration;

		// Token: 0x040061D9 RID: 25049
		private string clusterMapMeteorShowerID;

		// Token: 0x040061DA RID: 25050
		private bool affectedByDifficulty = true;

		// Token: 0x02002742 RID: 10050
		public struct BombardmentInfo
		{
			// Token: 0x0400AEB7 RID: 44727
			public string prefab;

			// Token: 0x0400AEB8 RID: 44728
			public float weight;
		}

		// Token: 0x02002743 RID: 10051
		public class States : GameplayEventStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, MeteorShowerEvent>
		{
			// Token: 0x0600C852 RID: 51282 RVA: 0x004267C4 File Offset: 0x004249C4
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				base.InitializeStates(out default_state);
				default_state = this.planning;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.planning.Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.runTimeRemaining.Set(smi.gameplayEvent.duration, smi, false);
					this.bombardTimeRemaining.Set(smi.GetBombardOnTime(), smi, false);
					this.snoozeTimeRemaining.Set(smi.GetBombardOffTime(), smi, false);
					if (smi.gameplayEvent.canStarTravel && smi.clusterTravelDuration > 0f)
					{
						smi.GoTo(smi.sm.starMap);
						return;
					}
					smi.GoTo(smi.sm.running);
				});
				this.starMap.Enter(new StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State.Callback(MeteorShowerEvent.States.CreateClusterMapMeteorShower)).DefaultState(this.starMap.travelling);
				this.starMap.travelling.OnSignal(this.OnClusterMapDestinationReached, this.starMap.arrive);
				this.starMap.arrive.GoTo(this.running.bombarding);
				this.running.DefaultState(this.running.snoozing).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.runTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.runTimeRemaining, this.finished, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero);
				this.running.bombarding.Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					MeteorShowerEvent.States.TriggerMeteorGlobalEvent(smi, GameHashes.MeteorShowerBombardStateBegins);
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					MeteorShowerEvent.States.TriggerMeteorGlobalEvent(smi, GameHashes.MeteorShowerBombardStateEnds);
				}).Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					smi.StartBackgroundEffects();
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					smi.StopBackgroundEffects();
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.bombardTimeRemaining.Set(smi.GetBombardOnTime(), smi, false);
				}).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.bombardTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.bombardTimeRemaining, this.running.snoozing, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					smi.Bombarding(dt);
				}, UpdateRate.SIM_200ms, false);
				this.running.snoozing.Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.snoozeTimeRemaining.Set(smi.GetBombardOffTime(), smi, false);
				}).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.snoozeTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.snoozeTimeRemaining, this.running.bombarding, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero);
				this.finished.ReturnSuccess();
			}

			// Token: 0x0600C853 RID: 51283 RVA: 0x004269FD File Offset: 0x00424BFD
			public static void TriggerMeteorGlobalEvent(MeteorShowerEvent.StatesInstance smi, GameHashes hash)
			{
				Game.Instance.BoxingTrigger<int>((int)hash, smi.eventInstance.worldId);
			}

			// Token: 0x0600C854 RID: 51284 RVA: 0x00426A18 File Offset: 0x00424C18
			public static void CreateClusterMapMeteorShower(MeteorShowerEvent.StatesInstance smi)
			{
				if (smi.sm.clusterMapMeteorShower.Get(smi) == null)
				{
					GameObject prefab = Assets.GetPrefab(smi.gameplayEvent.clusterMapMeteorShowerID.ToTag());
					float arrivalTime = smi.eventInstance.eventStartTime * 600f + smi.clusterTravelDuration;
					AxialI randomCellAtEdgeOfUniverse = ClusterGrid.Instance.GetRandomCellAtEdgeOfUniverse();
					GameObject gameObject = Util.KInstantiate(prefab, null, null);
					gameObject.GetComponent<ClusterMapMeteorShowerVisualizer>().SetInitialLocation(randomCellAtEdgeOfUniverse);
					ClusterMapMeteorShower.Def def = gameObject.AddOrGetDef<ClusterMapMeteorShower.Def>();
					def.destinationWorldID = smi.eventInstance.worldId;
					def.arrivalTime = arrivalTime;
					gameObject.SetActive(true);
					smi.sm.clusterMapMeteorShower.Set(gameObject, smi, false);
				}
				GameObject go = smi.sm.clusterMapMeteorShower.Get(smi);
				go.GetDef<ClusterMapMeteorShower.Def>();
				go.Subscribe(1796608350, new Action<object>(smi.OnClusterMapDestinationReached));
			}

			// Token: 0x0400AEB9 RID: 44729
			public MeteorShowerEvent.States.ClusterMapStates starMap;

			// Token: 0x0400AEBA RID: 44730
			public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State planning;

			// Token: 0x0400AEBB RID: 44731
			public MeteorShowerEvent.States.RunningStates running;

			// Token: 0x0400AEBC RID: 44732
			public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State finished;

			// Token: 0x0400AEBD RID: 44733
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.TargetParameter clusterMapMeteorShower;

			// Token: 0x0400AEBE RID: 44734
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter runTimeRemaining;

			// Token: 0x0400AEBF RID: 44735
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter bombardTimeRemaining;

			// Token: 0x0400AEC0 RID: 44736
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter snoozeTimeRemaining;

			// Token: 0x0400AEC1 RID: 44737
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.Signal OnClusterMapDestinationReached;

			// Token: 0x02003A2B RID: 14891
			public class ClusterMapStates : GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400EB36 RID: 60214
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State travelling;

				// Token: 0x0400EB37 RID: 60215
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State arrive;
			}

			// Token: 0x02003A2C RID: 14892
			public class RunningStates : GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400EB38 RID: 60216
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State bombarding;

				// Token: 0x0400EB39 RID: 60217
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State snoozing;
			}
		}

		// Token: 0x02002744 RID: 10052
		public class StatesInstance : GameplayEventStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, MeteorShowerEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600C85C RID: 51292 RVA: 0x00426BEA File Offset: 0x00424DEA
			public float GetSleepTimerValue()
			{
				return Mathf.Clamp(GameplayEventManager.Instance.GetSleepTimer(this.gameplayEvent) - GameUtil.GetCurrentTimeInCycles(), 0f, float.MaxValue);
			}

			// Token: 0x0600C85D RID: 51293 RVA: 0x00426C14 File Offset: 0x00424E14
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, MeteorShowerEvent meteorShowerEvent) : base(master, eventInstance, meteorShowerEvent)
			{
				this.world = ClusterManager.Instance.GetWorld(this.m_worldId);
				this.difficultyLevel = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
				this.m_worldId = eventInstance.worldId;
				Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
			}

			// Token: 0x0600C85E RID: 51294 RVA: 0x00426C88 File Offset: 0x00424E88
			public void OnClusterMapDestinationReached(object obj)
			{
				base.smi.sm.OnClusterMapDestinationReached.Trigger(this);
			}

			// Token: 0x0600C85F RID: 51295 RVA: 0x00426CA0 File Offset: 0x00424EA0
			private void OnActiveWorldChanged(object data)
			{
				int first = ((global::Tuple<int, int>)data).first;
				if (this.activeMeteorBackground != null)
				{
					this.activeMeteorBackground.GetComponent<ParticleSystemRenderer>().enabled = (first == this.m_worldId);
				}
			}

			// Token: 0x0600C860 RID: 51296 RVA: 0x00426CE0 File Offset: 0x00424EE0
			public override void StopSM(string reason)
			{
				this.StopBackgroundEffects();
				base.StopSM(reason);
			}

			// Token: 0x0600C861 RID: 51297 RVA: 0x00426CEF File Offset: 0x00424EEF
			protected override void OnCleanUp()
			{
				Game.Instance.Unsubscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
				this.DestroyClusterMapMeteorShowerObject();
				base.OnCleanUp();
			}

			// Token: 0x0600C862 RID: 51298 RVA: 0x00426D18 File Offset: 0x00424F18
			private void DestroyClusterMapMeteorShowerObject()
			{
				if (base.sm.clusterMapMeteorShower.Get(this) != null)
				{
					ClusterMapMeteorShower.Instance smi = base.sm.clusterMapMeteorShower.Get(this).GetSMI<ClusterMapMeteorShower.Instance>();
					if (smi != null)
					{
						smi.StopSM("Event is being aborted");
						Util.KDestroyGameObject(smi.gameObject);
					}
				}
			}

			// Token: 0x0600C863 RID: 51299 RVA: 0x00426D70 File Offset: 0x00424F70
			public void StartBackgroundEffects()
			{
				if (this.activeMeteorBackground == null)
				{
					this.activeMeteorBackground = Util.KInstantiate(EffectPrefabs.Instance.MeteorBackground, null, null);
					float x = (this.world.maximumBounds.x + this.world.minimumBounds.x) / 2f;
					float y = this.world.maximumBounds.y;
					float z = 25f;
					this.activeMeteorBackground.transform.SetPosition(new Vector3(x, y, z));
					this.activeMeteorBackground.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
				}
			}

			// Token: 0x0600C864 RID: 51300 RVA: 0x00426E24 File Offset: 0x00425024
			public void StopBackgroundEffects()
			{
				if (this.activeMeteorBackground != null)
				{
					ParticleSystem component = this.activeMeteorBackground.GetComponent<ParticleSystem>();
					component.main.stopAction = ParticleSystemStopAction.Destroy;
					component.Stop();
					if (!component.IsAlive())
					{
						UnityEngine.Object.Destroy(this.activeMeteorBackground);
					}
					this.activeMeteorBackground = null;
				}
			}

			// Token: 0x0600C865 RID: 51301 RVA: 0x00426E78 File Offset: 0x00425078
			public float TimeUntilNextShower()
			{
				if (base.IsInsideState(base.sm.running.bombarding))
				{
					return 0f;
				}
				if (!base.IsInsideState(base.sm.starMap))
				{
					return base.sm.snoozeTimeRemaining.Get(this);
				}
				float num = base.smi.eventInstance.eventStartTime * 600f + base.smi.clusterTravelDuration - GameUtil.GetCurrentTimeInCycles() * 600f;
				if (num >= 0f)
				{
					return num;
				}
				return 0f;
			}

			// Token: 0x0600C866 RID: 51302 RVA: 0x00426F08 File Offset: 0x00425108
			public void Bombarding(float dt)
			{
				this.nextMeteorTime -= dt;
				while (this.nextMeteorTime < 0f)
				{
					if (this.GetSleepTimerValue() <= 0f)
					{
						this.DoBombardment(this.gameplayEvent.bombardmentInfo);
					}
					this.nextMeteorTime += this.GetNextMeteorTime();
				}
			}

			// Token: 0x0600C867 RID: 51303 RVA: 0x00426F68 File Offset: 0x00425168
			private void DoBombardment(List<MeteorShowerEvent.BombardmentInfo> bombardment_info)
			{
				float num = 0f;
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo in bombardment_info)
				{
					num += bombardmentInfo.weight;
				}
				num = UnityEngine.Random.Range(0f, num);
				MeteorShowerEvent.BombardmentInfo bombardmentInfo2 = bombardment_info[0];
				int num2 = 0;
				while (num - bombardmentInfo2.weight > 0f)
				{
					num -= bombardmentInfo2.weight;
					bombardmentInfo2 = bombardment_info[++num2];
				}
				Game.Instance.Trigger(-84771526, null);
				this.SpawnBombard(bombardmentInfo2.prefab);
			}

			// Token: 0x0600C868 RID: 51304 RVA: 0x0042701C File Offset: 0x0042521C
			private GameObject SpawnBombard(string prefab)
			{
				WorldContainer worldContainer = ClusterManager.Instance.GetWorld(this.m_worldId);
				float x = (float)(worldContainer.Width - 1) * UnityEngine.Random.value + (float)worldContainer.WorldOffset.x;
				float y = (float)(worldContainer.Height + worldContainer.WorldOffset.y - 1);
				float layerZ = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
				Vector3 position = new Vector3(x, y, layerZ);
				GameObject prefab2 = Assets.GetPrefab(prefab);
				if (prefab2 == null)
				{
					return null;
				}
				GameObject gameObject = Util.KInstantiate(prefab2, position, Quaternion.identity, null, null, true, 0);
				Comet component = gameObject.GetComponent<Comet>();
				if (component != null)
				{
					component.spawnWithOffset = true;
				}
				gameObject.SetActive(true);
				return gameObject;
			}

			// Token: 0x0600C869 RID: 51305 RVA: 0x004270CB File Offset: 0x004252CB
			public float BombardTimeRemaining()
			{
				return Mathf.Min(base.sm.bombardTimeRemaining.Get(this), base.sm.runTimeRemaining.Get(this));
			}

			// Token: 0x0600C86A RID: 51306 RVA: 0x004270F4 File Offset: 0x004252F4
			public float GetBombardOffTime()
			{
				float num = this.gameplayEvent.secondsBombardmentOff.Get();
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
					if (!(id == "Infrequent"))
					{
						if (!(id == "Intense"))
						{
							if (id == "Doomed")
							{
								num *= 0.5f;
							}
						}
						else
						{
							num *= 1f;
						}
					}
					else
					{
						num *= 1f;
					}
				}
				return num;
			}

			// Token: 0x0600C86B RID: 51307 RVA: 0x0042717C File Offset: 0x0042537C
			public float GetBombardOnTime()
			{
				float num = this.gameplayEvent.secondsBombardmentOn.Get();
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
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
						num *= 1f;
					}
				}
				return num;
			}

			// Token: 0x0600C86C RID: 51308 RVA: 0x00427204 File Offset: 0x00425404
			private float GetNextMeteorTime()
			{
				float num = this.gameplayEvent.secondsPerMeteor;
				num *= 256f / (float)this.world.Width;
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
					if (!(id == "Infrequent"))
					{
						if (!(id == "Intense"))
						{
							if (id == "Doomed")
							{
								num *= 0.5f;
							}
						}
						else
						{
							num *= 0.8f;
						}
					}
					else
					{
						num *= 1.5f;
					}
				}
				return num;
			}

			// Token: 0x0400AEC2 RID: 44738
			public GameObject activeMeteorBackground;

			// Token: 0x0400AEC3 RID: 44739
			[Serialize]
			public float clusterTravelDuration = -1f;

			// Token: 0x0400AEC4 RID: 44740
			[Serialize]
			private float nextMeteorTime;

			// Token: 0x0400AEC5 RID: 44741
			[Serialize]
			private int m_worldId;

			// Token: 0x0400AEC6 RID: 44742
			private WorldContainer world;

			// Token: 0x0400AEC7 RID: 44743
			private SettingLevel difficultyLevel;
		}
	}
}
