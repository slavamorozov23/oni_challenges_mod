using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000B71 RID: 2929
public class ClusterMapResourceMeteor : GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>
{
	// Token: 0x060056BC RID: 22204 RVA: 0x001F9160 File Offset: 0x001F7360
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.traveling;
		this.traveling.DefaultState(this.traveling.unidentified).EventTransition(GameHashes.ClusterDestinationReached, this.leaving, null);
		this.traveling.unidentified.ParamTransition<bool>(this.IsIdentified, this.traveling.identified, GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.IsTrue);
		this.traveling.identified.ParamTransition<bool>(this.IsIdentified, this.traveling.unidentified, GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.IsFalse).ToggleStatusItem(Db.Get().MiscStatusItems.ClusterMeteorRemainingTravelTime, null);
		this.leaving.Enter(new StateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.State.Callback(ClusterMapResourceMeteor.DestinationReached));
	}

	// Token: 0x060056BD RID: 22205 RVA: 0x001F921F File Offset: 0x001F741F
	public static void DestinationReached(ClusterMapResourceMeteor.Instance smi)
	{
		smi.DestinationReached();
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x04003A85 RID: 14981
	public StateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.BoolParameter IsIdentified;

	// Token: 0x04003A86 RID: 14982
	public ClusterMapResourceMeteor.TravelingState traveling;

	// Token: 0x04003A87 RID: 14983
	public GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.State leaving;

	// Token: 0x04003A88 RID: 14984
	public GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.State left;

	// Token: 0x02001CEB RID: 7403
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600AF37 RID: 44855 RVA: 0x003D59C9 File Offset: 0x003D3BC9
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>();
		}

		// Token: 0x040089B0 RID: 35248
		public string name;

		// Token: 0x040089B1 RID: 35249
		public string description;

		// Token: 0x040089B2 RID: 35250
		public string description_Hidden;

		// Token: 0x040089B3 RID: 35251
		public string name_Hidden;

		// Token: 0x040089B4 RID: 35252
		public string eventID;

		// Token: 0x040089B5 RID: 35253
		private AxialI destination;

		// Token: 0x040089B6 RID: 35254
		public float arrivalTime;
	}

	// Token: 0x02001CEC RID: 7404
	public class TravelingState : GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.State
	{
		// Token: 0x040089B7 RID: 35255
		public GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.State unidentified;

		// Token: 0x040089B8 RID: 35256
		public GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.State identified;
	}

	// Token: 0x02001CED RID: 7405
	public new class Instance : GameStateMachine<ClusterMapResourceMeteor, ClusterMapResourceMeteor.Instance, IStateMachineTarget, ClusterMapResourceMeteor.Def>.GameInstance
	{
		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x0600AF3A RID: 44858 RVA: 0x003D59E0 File Offset: 0x003D3BE0
		public bool HasBeenIdentified
		{
			get
			{
				return base.sm.IsIdentified.Get(this);
			}
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x0600AF3B RID: 44859 RVA: 0x003D59F3 File Offset: 0x003D3BF3
		public float IdentifyingProgress
		{
			get
			{
				return this.identifyingProgress;
			}
		}

		// Token: 0x0600AF3C RID: 44860 RVA: 0x003D59FB File Offset: 0x003D3BFB
		public AxialI ClusterGridPosition()
		{
			return this.visualizer.Location;
		}

		// Token: 0x0600AF3D RID: 44861 RVA: 0x003D5A08 File Offset: 0x003D3C08
		public Instance(IStateMachineTarget master, ClusterMapResourceMeteor.Def def) : base(master, def)
		{
			this.traveler.getSpeedCB = new Func<float>(this.GetSpeed);
			this.traveler.onTravelCB = new System.Action(this.OnTravellerMoved);
		}

		// Token: 0x0600AF3E RID: 44862 RVA: 0x003D5A40 File Offset: 0x003D3C40
		private void OnTravellerMoved()
		{
			Game.Instance.Trigger(-1975776133, this);
		}

		// Token: 0x0600AF3F RID: 44863 RVA: 0x003D5A52 File Offset: 0x003D3C52
		protected override void OnCleanUp()
		{
			this.visualizer.Deselect();
			base.OnCleanUp();
		}

		// Token: 0x0600AF40 RID: 44864 RVA: 0x003D5A68 File Offset: 0x003D3C68
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

		// Token: 0x0600AF41 RID: 44865 RVA: 0x003D5AD0 File Offset: 0x003D3CD0
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

		// Token: 0x0600AF42 RID: 44866 RVA: 0x003D5B21 File Offset: 0x003D3D21
		public override void StartSM()
		{
			base.StartSM();
			this.RefreshVisuals(false);
		}

		// Token: 0x0600AF43 RID: 44867 RVA: 0x003D5B30 File Offset: 0x003D3D30
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

		// Token: 0x0600AF44 RID: 44868 RVA: 0x003D5BC4 File Offset: 0x003D3DC4
		public void Setup(AxialI destination, float arrivalTime)
		{
			this.Destination = destination;
			this.ArrivalTime = arrivalTime;
			this.destinationSelector.SetDestination(destination);
			this.traveler.RevalidatePath(false);
			int count = this.traveler.CurrentPath.Count;
			float num = arrivalTime - GameUtil.GetCurrentTimeInCycles() * 600f;
			this.Speed = (float)count / num * 600f;
		}

		// Token: 0x0600AF45 RID: 44869 RVA: 0x003D5C26 File Offset: 0x003D3E26
		public float GetSpeed()
		{
			return this.Speed;
		}

		// Token: 0x0600AF46 RID: 44870 RVA: 0x003D5C2E File Offset: 0x003D3E2E
		public void DestinationReached()
		{
			System.Action onDestinationReached = this.OnDestinationReached;
			if (onDestinationReached == null)
			{
				return;
			}
			onDestinationReached();
		}

		// Token: 0x040089B9 RID: 35257
		[Serialize]
		public AxialI Destination;

		// Token: 0x040089BA RID: 35258
		[Serialize]
		public float ArrivalTime;

		// Token: 0x040089BB RID: 35259
		[Serialize]
		private float Speed;

		// Token: 0x040089BC RID: 35260
		[Serialize]
		private float identifyingProgress;

		// Token: 0x040089BD RID: 35261
		public System.Action OnDestinationReached;

		// Token: 0x040089BE RID: 35262
		[MyCmpGet]
		private InfoDescription descriptor;

		// Token: 0x040089BF RID: 35263
		[MyCmpGet]
		private KSelectable selectable;

		// Token: 0x040089C0 RID: 35264
		[MyCmpGet]
		private ClusterMapMeteorShowerVisualizer visualizer;

		// Token: 0x040089C1 RID: 35265
		[MyCmpGet]
		private ClusterTraveler traveler;

		// Token: 0x040089C2 RID: 35266
		[MyCmpGet]
		private ClusterDestinationSelector destinationSelector;
	}
}
