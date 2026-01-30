using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000B64 RID: 2916
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactPOIStates")]
public class ArtifactPOIStates : GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>
{
	// Token: 0x06005641 RID: 22081 RVA: 0x001F6E48 File Offset: 0x001F5048
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.enter;
		this.root.Enter(delegate(ArtifactPOIStates.Instance smi)
		{
			if (smi.configuration == null || smi.configuration.typeId == HashedString.Invalid)
			{
				smi.configuration = smi.GetComponent<ArtifactPOIConfigurator>().MakeConfiguration();
				smi.poiCharge = 1f;
			}
		});
		this.enter.ParamTransition<float>(this.poiCharge, this.spawnArtifact, new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Parameter<float>.Callback(ArtifactPOIStates.IsFullyCharged)).ParamTransition<float>(this.poiCharge, this.waitingForPickup, new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Parameter<float>.Callback(ArtifactPOIStates.IsNotFullyCharge));
		this.spawnArtifact.Enter(new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State.Callback(ArtifactPOIStates.SpawnArtifactOnHexCellIfFullyCharged)).EnterGoTo(this.waitingForPickup);
		this.waitingForPickup.OnSignal(this.OnHexCellInventoryChangedSignal, this.recharging, new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Parameter<StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.SignalParameter>.Callback(ArtifactPOIStates.ThereIsNoArtifactInHexCell)).EnterTransition(this.destroyOnArtifactSpawned, (ArtifactPOIStates.Instance smi) => ArtifactPOIStates.MarkedForDestroyAfterArtifactSpawned(smi) && ArtifactPOIStates.IsArtifactAvailableInHexCell(smi));
		this.recharging.OnSignal(this.OnHexCellInventoryChangedSignal, this.waitingForPickup, new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Parameter<StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.SignalParameter>.Callback(ArtifactPOIStates.IsArtifactAvailableInHexCell)).ParamTransition<float>(this.poiCharge, this.spawnArtifact, new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Parameter<float>.Callback(ArtifactPOIStates.IsFullyCharged)).EventHandler(GameHashes.NewDay, (ArtifactPOIStates.Instance smi) => GameClock.Instance, new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State.Callback(ArtifactPOIStates.AddDayWothOfCharge));
		this.destroyOnArtifactSpawned.Enter(new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State.Callback(ArtifactPOIStates.SelfDestroy));
	}

	// Token: 0x06005642 RID: 22082 RVA: 0x001F6FCF File Offset: 0x001F51CF
	public static bool IsNotFullyCharge(ArtifactPOIStates.Instance smi, float f)
	{
		return !ArtifactPOIStates.IsFullyCharge(smi);
	}

	// Token: 0x06005643 RID: 22083 RVA: 0x001F6FDA File Offset: 0x001F51DA
	public static bool IsNotFullyCharge(ArtifactPOIStates.Instance smi)
	{
		return !ArtifactPOIStates.IsFullyCharge(smi);
	}

	// Token: 0x06005644 RID: 22084 RVA: 0x001F6FE5 File Offset: 0x001F51E5
	public static bool IsFullyCharge(ArtifactPOIStates.Instance smi)
	{
		return smi.sm.poiCharge.Get(smi) >= 1f;
	}

	// Token: 0x06005645 RID: 22085 RVA: 0x001F7002 File Offset: 0x001F5202
	public static bool IsFullyCharged(ArtifactPOIStates.Instance smi, float f)
	{
		return smi.sm.poiCharge.Get(smi) >= 1f;
	}

	// Token: 0x06005646 RID: 22086 RVA: 0x001F701F File Offset: 0x001F521F
	public static bool ThereIsNoArtifactInHexCell(ArtifactPOIStates.Instance smi, StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.SignalParameter param)
	{
		return ArtifactPOIStates.ThereIsNoArtifactInHexCell(smi);
	}

	// Token: 0x06005647 RID: 22087 RVA: 0x001F7027 File Offset: 0x001F5227
	public static bool ThereIsNoArtifactInHexCell(ArtifactPOIStates.Instance smi)
	{
		return !smi.HasArtifactAvailableInHexCell();
	}

	// Token: 0x06005648 RID: 22088 RVA: 0x001F7032 File Offset: 0x001F5232
	public static bool IsArtifactAvailableInHexCell(ArtifactPOIStates.Instance smi, StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.SignalParameter param)
	{
		return ArtifactPOIStates.IsArtifactAvailableInHexCell(smi);
	}

	// Token: 0x06005649 RID: 22089 RVA: 0x001F703A File Offset: 0x001F523A
	public static bool IsArtifactAvailableInHexCell(ArtifactPOIStates.Instance smi)
	{
		return smi.HasArtifactAvailableInHexCell();
	}

	// Token: 0x0600564A RID: 22090 RVA: 0x001F7042 File Offset: 0x001F5242
	public static bool MarkedForDestroyAfterArtifactSpawned(ArtifactPOIStates.Instance smi)
	{
		return smi.configuration.DestroyOnHarvest();
	}

	// Token: 0x0600564B RID: 22091 RVA: 0x001F704F File Offset: 0x001F524F
	public static void ResetRechargeProgress(ArtifactPOIStates.Instance smi)
	{
		smi.poiCharge = 0f;
	}

	// Token: 0x0600564C RID: 22092 RVA: 0x001F705C File Offset: 0x001F525C
	public static void IncreaseArtifactSpawnedCount(ArtifactPOIStates.Instance smi)
	{
		smi.IncreaseArtifactsSpawnedCount();
	}

	// Token: 0x0600564D RID: 22093 RVA: 0x001F7064 File Offset: 0x001F5264
	public static void SelfDestroy(ArtifactPOIStates.Instance smi)
	{
		smi.gameObject.DeleteObject();
	}

	// Token: 0x0600564E RID: 22094 RVA: 0x001F7071 File Offset: 0x001F5271
	public static void AddDayWothOfCharge(ArtifactPOIStates.Instance smi)
	{
		smi.RechargePOI(600f);
	}

	// Token: 0x0600564F RID: 22095 RVA: 0x001F707E File Offset: 0x001F527E
	public static void SpawnArtifactOnHexCellIfFullyCharged(ArtifactPOIStates.Instance smi)
	{
		if (ArtifactPOIStates.IsFullyCharge(smi))
		{
			smi.SpawnArtifactOnHexCell();
			ArtifactPOIStates.ResetRechargeProgress(smi);
			ArtifactPOIStates.IncreaseArtifactSpawnedCount(smi);
		}
	}

	// Token: 0x04003A42 RID: 14914
	public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State destroyOnArtifactSpawned;

	// Token: 0x04003A43 RID: 14915
	public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State enter;

	// Token: 0x04003A44 RID: 14916
	public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State waitingForPickup;

	// Token: 0x04003A45 RID: 14917
	public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State recharging;

	// Token: 0x04003A46 RID: 14918
	public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State spawnArtifact;

	// Token: 0x04003A47 RID: 14919
	public StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.Signal OnHexCellInventoryChangedSignal;

	// Token: 0x04003A48 RID: 14920
	public StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.FloatParameter poiCharge = new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.FloatParameter(1f);

	// Token: 0x02001CD5 RID: 7381
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001CD6 RID: 7382
	public new class Instance : GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.GameInstance, IGameObjectEffectDescriptor
	{
		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x0600AECF RID: 44751 RVA: 0x003D4A73 File Offset: 0x003D2C73
		public StarmapHexCellInventory HexCellInventory
		{
			get
			{
				return this.GetHexCellInventory();
			}
		}

		// Token: 0x0600AED0 RID: 44752 RVA: 0x003D4A7B File Offset: 0x003D2C7B
		public void IncreaseArtifactsSpawnedCount()
		{
			this.numHarvests++;
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x0600AED1 RID: 44753 RVA: 0x003D4A8B File Offset: 0x003D2C8B
		// (set) Token: 0x0600AED2 RID: 44754 RVA: 0x003D4A93 File Offset: 0x003D2C93
		public float poiCharge
		{
			get
			{
				return this._poiCharge;
			}
			set
			{
				this._poiCharge = value;
				base.smi.sm.poiCharge.Set(value, base.smi, false);
			}
		}

		// Token: 0x0600AED3 RID: 44755 RVA: 0x003D4ABA File Offset: 0x003D2CBA
		public Instance(IStateMachineTarget target, ArtifactPOIStates.Def def) : base(target, def)
		{
		}

		// Token: 0x0600AED4 RID: 44756 RVA: 0x003D4AC4 File Offset: 0x003D2CC4
		public override void StartSM()
		{
			this.HexCellInventory.Subscribe(-1697596308, new Action<object>(this.OnHexCellInventoryChanged));
			base.StartSM();
		}

		// Token: 0x0600AED5 RID: 44757 RVA: 0x003D4AE9 File Offset: 0x003D2CE9
		protected override void OnCleanUp()
		{
			this.HexCellInventory.Unsubscribe(-1697596308, new Action<object>(this.OnHexCellInventoryChanged));
			base.OnCleanUp();
		}

		// Token: 0x0600AED6 RID: 44758 RVA: 0x003D4B0D File Offset: 0x003D2D0D
		private void OnHexCellInventoryChanged(object o)
		{
			base.sm.OnHexCellInventoryChangedSignal.Trigger(this);
		}

		// Token: 0x0600AED7 RID: 44759 RVA: 0x003D4B20 File Offset: 0x003D2D20
		public StarmapHexCellInventory GetHexCellInventory()
		{
			ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
			return ClusterGrid.Instance.AddOrGetHexCellInventory(component.Location);
		}

		// Token: 0x0600AED8 RID: 44760 RVA: 0x003D4B44 File Offset: 0x003D2D44
		public bool HasArtifactAvailableInHexCell()
		{
			return this.HexCellInventory.Items.Find((StarmapHexCellInventory.SerializedItem i) => i.IsEntity && Assets.GetPrefab(i.ID).HasTag(GameTags.Artifact)) != null;
		}

		// Token: 0x0600AED9 RID: 44761 RVA: 0x003D4B78 File Offset: 0x003D2D78
		public void SpawnArtifactOnHexCell()
		{
			Tag itemID = (this.artifactToHarvest != null) ? this.artifactToHarvest : this.PickNewArtifactToHarvest();
			this.artifactToHarvest = null;
			this.HexCellInventory.AddItem(itemID, 1f, Element.State.Vacuum);
		}

		// Token: 0x0600AEDA RID: 44762 RVA: 0x003D4BBC File Offset: 0x003D2DBC
		public string PickNewArtifactToHarvest()
		{
			string text;
			if (this.numHarvests <= 0 && !string.IsNullOrEmpty(this.configuration.GetArtifactID()))
			{
				text = this.configuration.GetArtifactID();
				ArtifactSelector.Instance.ReserveArtifactID(text, ArtifactType.Any);
			}
			else
			{
				text = ArtifactSelector.Instance.GetUniqueArtifactID(ArtifactType.Space);
			}
			return text;
		}

		// Token: 0x0600AEDB RID: 44763 RVA: 0x003D4C10 File Offset: 0x003D2E10
		public void RechargePOI(float dt)
		{
			float num = dt / this.configuration.GetRechargeTime();
			this.poiCharge += num;
			this.poiCharge = Mathf.Min(1f, this.poiCharge);
		}

		// Token: 0x0600AEDC RID: 44764 RVA: 0x003D4C4F File Offset: 0x003D2E4F
		public float RechargeTimeRemaining()
		{
			return (float)Mathf.CeilToInt((this.configuration.GetRechargeTime() - this.configuration.GetRechargeTime() * this.poiCharge) / 600f) * 600f;
		}

		// Token: 0x0600AEDD RID: 44765 RVA: 0x003D4C81 File Offset: 0x003D2E81
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>();
		}

		// Token: 0x04008959 RID: 35161
		[Serialize]
		public ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration configuration;

		// Token: 0x0400895A RID: 35162
		[Serialize]
		private float _poiCharge;

		// Token: 0x0400895B RID: 35163
		[Serialize]
		private int numHarvests;

		// Token: 0x0400895C RID: 35164
		[Serialize]
		public string artifactToHarvest;
	}
}
