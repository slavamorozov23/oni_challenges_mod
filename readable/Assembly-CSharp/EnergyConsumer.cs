using System;
using System.Collections.Generic;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x0200091C RID: 2332
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name} {WattsUsed}W")]
[AddComponentMenu("KMonoBehaviour/scripts/EnergyConsumer")]
public class EnergyConsumer : KMonoBehaviour, ISaveLoadable, IEnergyConsumer, ICircuitConnected, IGameObjectEffectDescriptor
{
	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x06004116 RID: 16662 RVA: 0x0017046F File Offset: 0x0016E66F
	public int PowerSortOrder
	{
		get
		{
			return this.powerSortOrder;
		}
	}

	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x06004117 RID: 16663 RVA: 0x00170477 File Offset: 0x0016E677
	// (set) Token: 0x06004118 RID: 16664 RVA: 0x0017047F File Offset: 0x0016E67F
	public int PowerCell { get; private set; }

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x06004119 RID: 16665 RVA: 0x00170488 File Offset: 0x0016E688
	public bool HasWire
	{
		get
		{
			return Grid.Objects[this.PowerCell, 26] != null;
		}
	}

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x0600411A RID: 16666 RVA: 0x001704A2 File Offset: 0x0016E6A2
	// (set) Token: 0x0600411B RID: 16667 RVA: 0x001704B4 File Offset: 0x0016E6B4
	public virtual bool IsPowered
	{
		get
		{
			return this.operational.GetFlag(EnergyConsumer.PoweredFlag);
		}
		protected set
		{
			this.operational.SetFlag(EnergyConsumer.PoweredFlag, value);
		}
	}

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x0600411C RID: 16668 RVA: 0x001704C7 File Offset: 0x0016E6C7
	public bool IsConnected
	{
		get
		{
			return this.CircuitID != ushort.MaxValue;
		}
	}

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x0600411D RID: 16669 RVA: 0x001704D9 File Offset: 0x0016E6D9
	public string Name
	{
		get
		{
			return this.selectable.GetName();
		}
	}

	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x0600411E RID: 16670 RVA: 0x001704E6 File Offset: 0x0016E6E6
	// (set) Token: 0x0600411F RID: 16671 RVA: 0x001704EE File Offset: 0x0016E6EE
	public bool IsVirtual { get; private set; }

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x06004120 RID: 16672 RVA: 0x001704F7 File Offset: 0x0016E6F7
	// (set) Token: 0x06004121 RID: 16673 RVA: 0x001704FF File Offset: 0x0016E6FF
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x06004122 RID: 16674 RVA: 0x00170508 File Offset: 0x0016E708
	// (set) Token: 0x06004123 RID: 16675 RVA: 0x00170510 File Offset: 0x0016E710
	public ushort CircuitID { get; private set; }

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x06004124 RID: 16676 RVA: 0x00170519 File Offset: 0x0016E719
	// (set) Token: 0x06004125 RID: 16677 RVA: 0x00170521 File Offset: 0x0016E721
	public float BaseWattageRating
	{
		get
		{
			return this._BaseWattageRating;
		}
		set
		{
			this._BaseWattageRating = value;
		}
	}

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x06004126 RID: 16678 RVA: 0x0017052A File Offset: 0x0016E72A
	public float WattsUsed
	{
		get
		{
			if (this.operational.IsActive)
			{
				return this.BaseWattageRating;
			}
			return 0f;
		}
	}

	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06004127 RID: 16679 RVA: 0x00170545 File Offset: 0x0016E745
	public float WattsNeededWhenActive
	{
		get
		{
			return this.building.Def.EnergyConsumptionWhenActive;
		}
	}

	// Token: 0x06004128 RID: 16680 RVA: 0x00170557 File Offset: 0x0016E757
	protected override void OnPrefabInit()
	{
		this.CircuitID = ushort.MaxValue;
		this.IsPowered = false;
		this.BaseWattageRating = this.building.Def.EnergyConsumptionWhenActive;
	}

	// Token: 0x06004129 RID: 16681 RVA: 0x00170584 File Offset: 0x0016E784
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.EnergyConsumers.Add(this);
		Building component = base.GetComponent<Building>();
		this.PowerCell = component.GetPowerInputCell();
		Game.Instance.circuitManager.Connect(this);
		Game.Instance.energySim.AddEnergyConsumer(this);
	}

	// Token: 0x0600412A RID: 16682 RVA: 0x001705D5 File Offset: 0x0016E7D5
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveEnergyConsumer(this);
		Game.Instance.circuitManager.Disconnect(this, true);
		Components.EnergyConsumers.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600412B RID: 16683 RVA: 0x00170609 File Offset: 0x0016E809
	public virtual void EnergySim200ms(float dt)
	{
		this.CircuitID = Game.Instance.circuitManager.GetCircuitID(this);
		if (!this.IsConnected)
		{
			this.IsPowered = false;
		}
		this.circuitOverloadTime = Mathf.Max(0f, this.circuitOverloadTime - dt);
	}

	// Token: 0x0600412C RID: 16684 RVA: 0x00170648 File Offset: 0x0016E848
	public virtual void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
	{
		switch (connection_status)
		{
		case CircuitManager.ConnectionStatus.NotConnected:
			this.IsPowered = false;
			return;
		case CircuitManager.ConnectionStatus.Unpowered:
			if (this.IsPowered && base.GetComponent<Battery>() == null)
			{
				this.IsPowered = false;
				this.circuitOverloadTime = 6f;
				this.PlayCircuitSound("overdraw");
				return;
			}
			break;
		case CircuitManager.ConnectionStatus.Powered:
			if (!this.IsPowered && this.circuitOverloadTime <= 0f)
			{
				this.IsPowered = true;
				this.PlayCircuitSound("powered");
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600412D RID: 16685 RVA: 0x001706CC File Offset: 0x0016E8CC
	protected void PlayCircuitSound(string state)
	{
		EventReference event_ref;
		if (state == "powered")
		{
			event_ref = Sounds.Instance.BuildingPowerOnMigrated;
		}
		else if (state == "overdraw")
		{
			event_ref = Sounds.Instance.ElectricGridOverloadMigrated;
		}
		else
		{
			event_ref = default(EventReference);
			global::Debug.Log("Invalid state for sound in EnergyConsumer.");
		}
		if (!CameraController.Instance.IsAudibleSound(base.transform.GetPosition()))
		{
			return;
		}
		float num;
		if (!this.lastTimeSoundPlayed.TryGetValue(state, out num))
		{
			num = 0f;
		}
		float value = (Time.time - num) / this.soundDecayTime;
		Vector3 position = base.transform.GetPosition();
		position.z = 0f;
		FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(event_ref, CameraController.Instance.GetVerticallyScaledPosition(position, false), 1f);
		instance.setParameterByName("timeSinceLast", value, false);
		KFMOD.EndOneShot(instance);
		this.lastTimeSoundPlayed[state] = Time.time;
	}

	// Token: 0x0600412E RID: 16686 RVA: 0x001707BA File Offset: 0x0016E9BA
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x040028B2 RID: 10418
	[MyCmpReq]
	private Building building;

	// Token: 0x040028B3 RID: 10419
	[MyCmpGet]
	protected Operational operational;

	// Token: 0x040028B4 RID: 10420
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x040028B5 RID: 10421
	[SerializeField]
	public int powerSortOrder;

	// Token: 0x040028B7 RID: 10423
	[Serialize]
	protected float circuitOverloadTime;

	// Token: 0x040028B8 RID: 10424
	public static readonly Operational.Flag PoweredFlag = new Operational.Flag("powered", Operational.Flag.Type.Requirement);

	// Token: 0x040028B9 RID: 10425
	private Dictionary<string, float> lastTimeSoundPlayed = new Dictionary<string, float>();

	// Token: 0x040028BA RID: 10426
	private float soundDecayTime = 10f;

	// Token: 0x040028BE RID: 10430
	private float _BaseWattageRating;
}
