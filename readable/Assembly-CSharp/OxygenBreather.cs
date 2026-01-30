using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000A89 RID: 2697
[RequireComponent(typeof(Health))]
[AddComponentMenu("KMonoBehaviour/scripts/OxygenBreather")]
public class OxygenBreather : KMonoBehaviour, ISim200ms
{
	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x06004E4E RID: 20046 RVA: 0x001C7B4D File Offset: 0x001C5D4D
	// (set) Token: 0x06004E4D RID: 20045 RVA: 0x001C7B44 File Offset: 0x001C5D44
	public KPrefabID prefabID { get; private set; }

	// Token: 0x17000557 RID: 1367
	// (get) Token: 0x06004E4F RID: 20047 RVA: 0x001C7B55 File Offset: 0x001C5D55
	public float ConsumptionRate
	{
		get
		{
			if (this.airConsumptionRate != null)
			{
				return this.airConsumptionRate.GetTotalValue();
			}
			return 0f;
		}
	}

	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x06004E50 RID: 20048 RVA: 0x001C7B70 File Offset: 0x001C5D70
	public float CO2EmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.co2Accumulator);
		}
	}

	// Token: 0x17000559 RID: 1369
	// (get) Token: 0x06004E51 RID: 20049 RVA: 0x001C7B87 File Offset: 0x001C5D87
	public HandleVector<int>.Handle O2Accumulator
	{
		get
		{
			return this.o2Accumulator;
		}
	}

	// Token: 0x06004E52 RID: 20050 RVA: 0x001C7B90 File Offset: 0x001C5D90
	public OxygenBreather.IGasProvider GetCurrentGasProvider()
	{
		if (this.gasProviders.Count == 0)
		{
			return null;
		}
		OxygenBreather.IGasProvider result = null;
		for (int i = this.gasProviders.Count - 1; i >= 0; i--)
		{
			OxygenBreather.IGasProvider gasProvider = this.gasProviders[i];
			if (!gasProvider.IsBlocked())
			{
				result = gasProvider;
				if (gasProvider.HasOxygen())
				{
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06004E53 RID: 20051 RVA: 0x001C7BE8 File Offset: 0x001C5DE8
	public bool IsLowOxygen()
	{
		OxygenBreather.IGasProvider currentGasProvider = this.GetCurrentGasProvider();
		return currentGasProvider == null || currentGasProvider.IsLowOxygen();
	}

	// Token: 0x1700055A RID: 1370
	// (get) Token: 0x06004E54 RID: 20052 RVA: 0x001C7C07 File Offset: 0x001C5E07
	public bool HasOxygen
	{
		get
		{
			return this.hasAir;
		}
	}

	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x06004E55 RID: 20053 RVA: 0x001C7C0F File Offset: 0x001C5E0F
	public bool IsOutOfOxygen
	{
		get
		{
			return !this.hasAir;
		}
	}

	// Token: 0x06004E56 RID: 20054 RVA: 0x001C7C1A File Offset: 0x001C5E1A
	protected override void OnPrefabInit()
	{
		GameUtil.SubscribeToTags<OxygenBreather>(this, OxygenBreather.OnDeadTagAddedDelegate, true);
		this.prefabID = base.GetComponent<KPrefabID>();
	}

	// Token: 0x06004E57 RID: 20055 RVA: 0x001C7C34 File Offset: 0x001C5E34
	protected override void OnSpawn()
	{
		this.airConsumptionRate = Db.Get().Attributes.AirConsumptionRate.Lookup(this);
		this.o2Accumulator = Game.Instance.accumulators.Add("O2", this);
		this.co2Accumulator = Game.Instance.accumulators.Add("CO2", this);
		bool flag = base.gameObject.PrefabID() == BionicMinionConfig.ID;
		this.o2StatusItem = this.selectable.AddStatusItem(flag ? Db.Get().DuplicantStatusItems.BreathingO2Bionic : Db.Get().DuplicantStatusItems.BreathingO2, this);
		this.cO2StatusItem = this.selectable.AddStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, this);
		this.temperature = Db.Get().Amounts.Temperature.Lookup(this);
		NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
	}

	// Token: 0x06004E58 RID: 20056 RVA: 0x001C7D30 File Offset: 0x001C5F30
	private void BreathableGasConsumed(SimHashes elementConsumed, float massConsumed, float temperature, byte disseaseIDX, int disseaseCount)
	{
		if (this.prefabID.HasTag(GameTags.Dead) || this.O2Accumulator == HandleVector<int>.Handle.InvalidHandle)
		{
			return;
		}
		if (elementConsumed == SimHashes.ContaminatedOxygen)
		{
			base.BoxingTrigger<float>(-935848905, massConsumed);
		}
		Game.Instance.accumulators.Accumulate(this.O2Accumulator, massConsumed);
		float value = -massConsumed;
		ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, value, this.selectable.GetProperName(), null);
		if (this.onBreathableGasConsumed != null)
		{
			this.onBreathableGasConsumed(elementConsumed, massConsumed, temperature, disseaseIDX, disseaseCount);
		}
	}

	// Token: 0x06004E59 RID: 20057 RVA: 0x001C7DC2 File Offset: 0x001C5FC2
	public static void BreathableGasConsumed(OxygenBreather breather, SimHashes elementConsumed, float massConsumed, float temperature, byte disseaseIDX, int disseaseCount)
	{
		if (breather != null)
		{
			breather.BreathableGasConsumed(elementConsumed, massConsumed, temperature, disseaseIDX, disseaseCount);
		}
	}

	// Token: 0x06004E5A RID: 20058 RVA: 0x001C7DDC File Offset: 0x001C5FDC
	public void Sim200ms(float dt)
	{
		if (!this.prefabID.HasTag(GameTags.Dead))
		{
			float num = this.airConsumptionRate.GetTotalValue() * dt;
			OxygenBreather.IGasProvider currentGasProvider = this.GetCurrentGasProvider();
			bool flag = currentGasProvider != null && currentGasProvider.ConsumeGas(this, num);
			if (flag)
			{
				if (currentGasProvider.ShouldEmitCO2())
				{
					if (this.cO2StatusItem != Guid.Empty)
					{
						this.cO2StatusItem = this.selectable.AddStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, this);
					}
					float num2 = num * this.O2toCO2conversion;
					Game.Instance.accumulators.Accumulate(this.co2Accumulator, num2);
					this.accumulatedCO2 += num2;
					if (this.accumulatedCO2 >= this.minCO2ToEmit)
					{
						this.accumulatedCO2 -= this.minCO2ToEmit;
						Vector3 position = base.transform.GetPosition();
						Vector3 vector = position;
						vector.x += (this.facing.GetFacing() ? (-this.mouthOffset.x) : this.mouthOffset.x);
						vector.y += this.mouthOffset.y;
						vector.z -= 0.5f;
						if (Mathf.FloorToInt(vector.x) != Mathf.FloorToInt(position.x))
						{
							vector.x = Mathf.Floor(position.x) + (this.facing.GetFacing() ? 0.01f : 0.99f);
						}
						CO2Manager.instance.SpawnBreath(vector, this.minCO2ToEmit, this.temperature.value, this.facing.GetFacing());
					}
				}
				else if (currentGasProvider.ShouldStoreCO2())
				{
					if (this.cO2StatusItem != Guid.Empty)
					{
						this.cO2StatusItem = this.selectable.AddStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, this);
					}
					Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
					if (equippable != null)
					{
						float num3 = num * this.O2toCO2conversion;
						Game.Instance.accumulators.Accumulate(this.co2Accumulator, num3);
						this.accumulatedCO2 += num3;
						if (this.accumulatedCO2 >= this.minCO2ToEmit)
						{
							this.accumulatedCO2 -= this.minCO2ToEmit;
							equippable.GetComponent<Storage>().AddGasChunk(SimHashes.CarbonDioxide, this.minCO2ToEmit, this.temperature.value, byte.MaxValue, 0, false, true);
						}
					}
				}
				else if (this.cO2StatusItem != Guid.Empty)
				{
					this.selectable.RemoveStatusItem(this.cO2StatusItem, false);
					this.cO2StatusItem = Guid.Empty;
				}
			}
			if (flag != this.hasAir)
			{
				this.hasAirTimer.Start();
				if (this.hasAirTimer.TryStop(2f))
				{
					this.hasAir = flag;
					base.Trigger(-933153513, BoxedBools.Box(this.hasAir));
					return;
				}
			}
			else
			{
				this.hasAirTimer.Stop();
			}
		}
	}

	// Token: 0x06004E5B RID: 20059 RVA: 0x001C80E8 File Offset: 0x001C62E8
	public void AddGasProvider(OxygenBreather.IGasProvider gas_provider)
	{
		global::Debug.Assert(gas_provider != null, "Error at OxygenBreather.cs  adding gas provider, the gas provider param is null!");
		global::Debug.Assert(!this.gasProviders.Contains(gas_provider), "Error at OxygenBreather.cs adding gas provider, the gas provider was already added to the gas providers list!");
		this.gasProviders.Add(gas_provider);
		gas_provider.OnSetOxygenBreather(this);
	}

	// Token: 0x06004E5C RID: 20060 RVA: 0x001C8124 File Offset: 0x001C6324
	public bool RemoveGasProvider(OxygenBreather.IGasProvider provider)
	{
		if (this.gasProviders.Count > 0 && this.gasProviders.Contains(provider))
		{
			OxygenBreather.IGasProvider gasProvider = this.gasProviders[this.gasProviders.Count - 1];
			this.gasProviders.Remove(provider);
			provider.OnClearOxygenBreather(this);
			return true;
		}
		return false;
	}

	// Token: 0x06004E5D RID: 20061 RVA: 0x001C8180 File Offset: 0x001C6380
	private void OnDeath(object data)
	{
		base.enabled = false;
		this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.BreathingO2, false);
		this.selectable.RemoveStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, false);
	}

	// Token: 0x06004E5E RID: 20062 RVA: 0x001C81CC File Offset: 0x001C63CC
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.o2Accumulator);
		Game.Instance.accumulators.Remove(this.co2Accumulator);
		this.o2Accumulator = HandleVector<int>.InvalidHandle;
		this.co2Accumulator = HandleVector<int>.InvalidHandle;
		while (this.gasProviders.Count > 0)
		{
			OxygenBreather.IGasProvider provider = this.gasProviders[this.gasProviders.Count - 1];
			this.RemoveGasProvider(provider);
		}
		base.OnCleanUp();
	}

	// Token: 0x04003429 RID: 13353
	public float O2toCO2conversion = 0.5f;

	// Token: 0x0400342A RID: 13354
	public Vector2 mouthOffset;

	// Token: 0x0400342B RID: 13355
	[Serialize]
	public float accumulatedCO2;

	// Token: 0x0400342C RID: 13356
	[SerializeField]
	public float minCO2ToEmit = 0.3f;

	// Token: 0x0400342D RID: 13357
	private bool hasAir = true;

	// Token: 0x0400342E RID: 13358
	private Timer hasAirTimer = new Timer();

	// Token: 0x0400342F RID: 13359
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04003430 RID: 13360
	[MyCmpGet]
	private Facing facing;

	// Token: 0x04003431 RID: 13361
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04003433 RID: 13363
	private HandleVector<int>.Handle o2Accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04003434 RID: 13364
	private HandleVector<int>.Handle co2Accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04003435 RID: 13365
	private AmountInstance temperature;

	// Token: 0x04003436 RID: 13366
	public float lowOxygenThreshold;

	// Token: 0x04003437 RID: 13367
	public float noOxygenThreshold;

	// Token: 0x04003438 RID: 13368
	private AttributeInstance airConsumptionRate;

	// Token: 0x04003439 RID: 13369
	public Action<SimHashes, float, float, byte, int> onBreathableGasConsumed;

	// Token: 0x0400343A RID: 13370
	private static readonly EventSystem.IntraObjectHandler<OxygenBreather> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<OxygenBreather>(GameTags.Dead, delegate(OxygenBreather component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x0400343B RID: 13371
	private List<OxygenBreather.IGasProvider> gasProviders = new List<OxygenBreather.IGasProvider>();

	// Token: 0x0400343C RID: 13372
	private Guid o2StatusItem;

	// Token: 0x0400343D RID: 13373
	private Guid cO2StatusItem;

	// Token: 0x02001BA0 RID: 7072
	public interface IGasProvider
	{
		// Token: 0x0600AA88 RID: 43656
		void OnSetOxygenBreather(OxygenBreather oxygen_breather);

		// Token: 0x0600AA89 RID: 43657
		void OnClearOxygenBreather(OxygenBreather oxygen_breather);

		// Token: 0x0600AA8A RID: 43658
		bool ConsumeGas(OxygenBreather oxygen_breather, float amount);

		// Token: 0x0600AA8B RID: 43659
		bool ShouldEmitCO2();

		// Token: 0x0600AA8C RID: 43660
		bool ShouldStoreCO2();

		// Token: 0x0600AA8D RID: 43661
		bool IsLowOxygen();

		// Token: 0x0600AA8E RID: 43662
		bool HasOxygen();

		// Token: 0x0600AA8F RID: 43663
		bool IsBlocked();
	}
}
