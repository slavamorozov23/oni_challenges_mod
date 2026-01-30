using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B6E RID: 2926
public class ClusterMapMeteorShower : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>
{
	// Token: 0x0600569C RID: 22172 RVA: 0x001F8694 File Offset: 0x001F6894
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.traveling;
		this.traveling.DefaultState(this.traveling.unidentified).EventTransition(GameHashes.ClusterDestinationReached, this.arrived, null).EventTransition(GameHashes.MissileDamageEncountered, this.destroyed, null);
		this.traveling.unidentified.ParamTransition<bool>(this.IsIdentified, this.traveling.identified, GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.IsTrue);
		this.traveling.identified.ParamTransition<bool>(this.IsIdentified, this.traveling.unidentified, GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.IsFalse).ToggleStatusItem(Db.Get().MiscStatusItems.ClusterMeteorRemainingTravelTime, null);
		this.arrived.Enter(new StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State.Callback(ClusterMapMeteorShower.DestinationReached));
		this.destroyed.Enter(new StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State.Callback(ClusterMapMeteorShower.HandleDestruction));
	}

	// Token: 0x0600569D RID: 22173 RVA: 0x001F877C File Offset: 0x001F697C
	public static void DestinationReached(ClusterMapMeteorShower.Instance smi)
	{
		smi.DestinationReached();
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x0600569E RID: 22174 RVA: 0x001F8790 File Offset: 0x001F6990
	public static void HandleDestruction(ClusterMapMeteorShower.Instance smi)
	{
		GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance(smi.def.eventID, smi.DestinationWorldID);
		if (gameplayEventInstance != null)
		{
			gameplayEventInstance.smi.StopSM("ShotDown");
		}
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x04003A7A RID: 14970
	public StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.BoolParameter IsIdentified;

	// Token: 0x04003A7B RID: 14971
	public ClusterMapMeteorShower.TravelingState traveling;

	// Token: 0x04003A7C RID: 14972
	public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State arrived;

	// Token: 0x04003A7D RID: 14973
	public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State destroyed;

	// Token: 0x02001CE7 RID: 7399
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600AF1B RID: 44827 RVA: 0x003D54F8 File Offset: 0x003D36F8
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			GameplayEvent gameplayEvent = Db.Get().GameplayEvents.Get(this.eventID);
			List<Descriptor> list = new List<Descriptor>();
			ClusterMapMeteorShower.Instance smi = go.GetSMI<ClusterMapMeteorShower.Instance>();
			if (smi != null && smi.sm.IsIdentified.Get(smi) && gameplayEvent is MeteorShowerEvent)
			{
				List<MeteorShowerEvent.BombardmentInfo> meteorsInfo = (gameplayEvent as MeteorShowerEvent).GetMeteorsInfo();
				float num = 0f;
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo in meteorsInfo)
				{
					num += bombardmentInfo.weight;
				}
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo2 in meteorsInfo)
				{
					GameObject prefab = Assets.GetPrefab(bombardmentInfo2.prefab);
					string formattedPercent = GameUtil.GetFormattedPercent((float)Mathf.RoundToInt(bombardmentInfo2.weight / num * 100f), GameUtil.TimeSlice.None);
					string txt = prefab.GetProperName() + " " + formattedPercent;
					Descriptor item = new Descriptor(txt, UI.GAMEOBJECTEFFECTS.TOOLTIPS.METEOR_SHOWER_SINGLE_METEOR_PERCENTAGE_TOOLTIP, Descriptor.DescriptorType.Effect, false);
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x0400899B RID: 35227
		public string name;

		// Token: 0x0400899C RID: 35228
		public string description;

		// Token: 0x0400899D RID: 35229
		public string description_Hidden;

		// Token: 0x0400899E RID: 35230
		public string name_Hidden;

		// Token: 0x0400899F RID: 35231
		public string eventID;

		// Token: 0x040089A0 RID: 35232
		public int destinationWorldID;

		// Token: 0x040089A1 RID: 35233
		public float arrivalTime;
	}

	// Token: 0x02001CE8 RID: 7400
	public class TravelingState : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State
	{
		// Token: 0x040089A2 RID: 35234
		public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State unidentified;

		// Token: 0x040089A3 RID: 35235
		public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State identified;
	}

	// Token: 0x02001CE9 RID: 7401
	public new class Instance : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x0600AF1E RID: 44830 RVA: 0x003D5654 File Offset: 0x003D3854
		public WorldContainer World_Destination
		{
			get
			{
				return ClusterManager.Instance.GetWorld(this.DestinationWorldID);
			}
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x0600AF1F RID: 44831 RVA: 0x003D5666 File Offset: 0x003D3866
		public string SidescreenButtonText
		{
			get
			{
				if (!base.smi.sm.IsIdentified.Get(base.smi))
				{
					return "Identify";
				}
				return "Dev Hide";
			}
		}

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x0600AF20 RID: 44832 RVA: 0x003D5690 File Offset: 0x003D3890
		public string SidescreenButtonTooltip
		{
			get
			{
				if (!base.smi.sm.IsIdentified.Get(base.smi))
				{
					return "Identifies the meteor shower";
				}
				return "Dev unidentify back";
			}
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x0600AF21 RID: 44833 RVA: 0x003D56BA File Offset: 0x003D38BA
		public bool HasBeenIdentified
		{
			get
			{
				return base.sm.IsIdentified.Get(this);
			}
		}

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x0600AF22 RID: 44834 RVA: 0x003D56CD File Offset: 0x003D38CD
		public float IdentifyingProgress
		{
			get
			{
				return this.identifyingProgress;
			}
		}

		// Token: 0x0600AF23 RID: 44835 RVA: 0x003D56D5 File Offset: 0x003D38D5
		public AxialI ClusterGridPosition()
		{
			return this.visualizer.Location;
		}

		// Token: 0x0600AF24 RID: 44836 RVA: 0x003D56E2 File Offset: 0x003D38E2
		public Instance(IStateMachineTarget master, ClusterMapMeteorShower.Def def) : base(master, def)
		{
			this.traveler.getSpeedCB = new Func<float>(this.GetSpeed);
			this.traveler.onTravelCB = new System.Action(this.OnTravellerMoved);
		}

		// Token: 0x0600AF25 RID: 44837 RVA: 0x003D5721 File Offset: 0x003D3921
		private void OnTravellerMoved()
		{
			Game.Instance.Trigger(-1975776133, this);
		}

		// Token: 0x0600AF26 RID: 44838 RVA: 0x003D5733 File Offset: 0x003D3933
		protected override void OnCleanUp()
		{
			this.visualizer.Deselect();
			Components.LongRangeMissileTargetables.Remove(base.gameObject.GetComponent<ClusterGridEntity>());
			base.OnCleanUp();
		}

		// Token: 0x0600AF27 RID: 44839 RVA: 0x003D575C File Offset: 0x003D395C
		public void Identify()
		{
			if (!this.HasBeenIdentified)
			{
				this.identifyingProgress = 1f;
				base.sm.IsIdentified.Set(true, this, false);
				Game.Instance.Trigger(1427028915, this);
				this.RefreshVisuals(true);
				if (ClusterMapScreen.Instance.IsActive())
				{
					KFMOD.PlayUISound(GlobalAssets.GetSound("ClusterMapMeteor_Reveal", false));
				}
			}
		}

		// Token: 0x0600AF28 RID: 44840 RVA: 0x003D57C4 File Offset: 0x003D39C4
		public void ProgressIdentifiction(float points)
		{
			if (!this.HasBeenIdentified)
			{
				this.identifyingProgress += points;
				this.identifyingProgress = Mathf.Clamp(this.identifyingProgress, 0f, 1f);
				if (this.identifyingProgress == 1f)
				{
					this.Identify();
				}
			}
		}

		// Token: 0x0600AF29 RID: 44841 RVA: 0x003D5818 File Offset: 0x003D3A18
		public override void StartSM()
		{
			base.StartSM();
			if (this.DestinationWorldID < 0)
			{
				this.Setup(base.def.destinationWorldID, base.def.arrivalTime);
			}
			Components.LongRangeMissileTargetables.Add(base.gameObject.GetComponent<ClusterGridEntity>());
			this.RefreshVisuals(false);
		}

		// Token: 0x0600AF2A RID: 44842 RVA: 0x003D586C File Offset: 0x003D3A6C
		public void RefreshVisuals(bool playIdentifyAnimationIfVisible = false)
		{
			if (this.HasBeenIdentified)
			{
				this.selectable.SetName(base.def.name);
				this.descriptor.description = base.def.description;
				this.visualizer.PlayRevealAnimation(playIdentifyAnimationIfVisible);
			}
			else
			{
				this.selectable.SetName(base.def.name_Hidden);
				this.descriptor.description = base.def.description_Hidden;
				this.visualizer.PlayHideAnimation();
			}
			base.Trigger(1980521255, null);
		}

		// Token: 0x0600AF2B RID: 44843 RVA: 0x003D5900 File Offset: 0x003D3B00
		public void Setup(int destinationWorldID, float arrivalTime)
		{
			this.DestinationWorldID = destinationWorldID;
			this.ArrivalTime = arrivalTime;
			AxialI location = this.World_Destination.GetComponent<ClusterGridEntity>().Location;
			this.destinationSelector.SetDestination(location);
			this.traveler.RevalidatePath(false);
			int count = this.traveler.CurrentPath.Count;
			float num = arrivalTime - GameUtil.GetCurrentTimeInCycles() * 600f;
			this.Speed = (float)count / num * 600f;
		}

		// Token: 0x0600AF2C RID: 44844 RVA: 0x003D5973 File Offset: 0x003D3B73
		public float GetSpeed()
		{
			return this.Speed;
		}

		// Token: 0x0600AF2D RID: 44845 RVA: 0x003D597B File Offset: 0x003D3B7B
		public void DestinationReached()
		{
			System.Action onDestinationReached = this.OnDestinationReached;
			if (onDestinationReached == null)
			{
				return;
			}
			onDestinationReached();
		}

		// Token: 0x0600AF2E RID: 44846 RVA: 0x003D598D File Offset: 0x003D3B8D
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600AF2F RID: 44847 RVA: 0x003D5994 File Offset: 0x003D3B94
		public bool SidescreenEnabled()
		{
			return false;
		}

		// Token: 0x0600AF30 RID: 44848 RVA: 0x003D5997 File Offset: 0x003D3B97
		public bool SidescreenButtonInteractable()
		{
			return true;
		}

		// Token: 0x0600AF31 RID: 44849 RVA: 0x003D599A File Offset: 0x003D3B9A
		public void OnSidescreenButtonPressed()
		{
			this.Identify();
		}

		// Token: 0x0600AF32 RID: 44850 RVA: 0x003D59A2 File Offset: 0x003D3BA2
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x0600AF33 RID: 44851 RVA: 0x003D59A5 File Offset: 0x003D3BA5
		public int ButtonSideScreenSortOrder()
		{
			return SORTORDER.KEEPSAKES;
		}

		// Token: 0x040089A4 RID: 35236
		[Serialize]
		public int DestinationWorldID = -1;

		// Token: 0x040089A5 RID: 35237
		[Serialize]
		public float ArrivalTime;

		// Token: 0x040089A6 RID: 35238
		[Serialize]
		private float Speed;

		// Token: 0x040089A7 RID: 35239
		[Serialize]
		private float identifyingProgress;

		// Token: 0x040089A8 RID: 35240
		public System.Action OnDestinationReached;

		// Token: 0x040089A9 RID: 35241
		[MyCmpGet]
		private InfoDescription descriptor;

		// Token: 0x040089AA RID: 35242
		[MyCmpGet]
		private KSelectable selectable;

		// Token: 0x040089AB RID: 35243
		[MyCmpGet]
		private ClusterMapMeteorShowerVisualizer visualizer;

		// Token: 0x040089AC RID: 35244
		[MyCmpGet]
		private ClusterTraveler traveler;

		// Token: 0x040089AD RID: 35245
		[MyCmpGet]
		private ClusterDestinationSelector destinationSelector;
	}
}
