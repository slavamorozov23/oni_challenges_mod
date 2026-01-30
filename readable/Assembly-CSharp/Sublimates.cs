using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000BE8 RID: 3048
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Sublimates")]
public class Sublimates : KMonoBehaviour, ISim200ms
{
	// Token: 0x170006A4 RID: 1700
	// (get) Token: 0x06005B6C RID: 23404 RVA: 0x002116DC File Offset: 0x0020F8DC
	public float Temperature
	{
		get
		{
			return this.primaryElement.Temperature;
		}
	}

	// Token: 0x06005B6D RID: 23405 RVA: 0x002116E9 File Offset: 0x0020F8E9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Sublimates>(-2064133523, Sublimates.OnAbsorbDelegate);
		base.Subscribe<Sublimates>(1335436905, Sublimates.OnSplitFromChunkDelegate);
		this.simRenderLoadBalance = true;
	}

	// Token: 0x06005B6E RID: 23406 RVA: 0x0021171A File Offset: 0x0020F91A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.flowAccumulator = Game.Instance.accumulators.Add("EmittedMass", this);
		this.RefreshStatusItem(Sublimates.EmitState.Emitting);
	}

	// Token: 0x06005B6F RID: 23407 RVA: 0x00211744 File Offset: 0x0020F944
	protected override void OnCleanUp()
	{
		this.flowAccumulator = Game.Instance.accumulators.Remove(this.flowAccumulator);
		base.OnCleanUp();
	}

	// Token: 0x06005B70 RID: 23408 RVA: 0x00211768 File Offset: 0x0020F968
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable != null)
		{
			Sublimates component = pickupable.GetComponent<Sublimates>();
			if (component != null)
			{
				this.sublimatedMass += component.sublimatedMass;
			}
		}
	}

	// Token: 0x06005B71 RID: 23409 RVA: 0x002117A8 File Offset: 0x0020F9A8
	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = data as Pickupable;
		PrimaryElement primaryElement = pickupable.PrimaryElement;
		Sublimates component = pickupable.GetComponent<Sublimates>();
		if (component == null)
		{
			return;
		}
		float mass = this.primaryElement.Mass;
		float mass2 = primaryElement.Mass;
		float num = mass / (mass2 + mass);
		this.sublimatedMass = component.sublimatedMass * num;
		float num2 = 1f - num;
		component.sublimatedMass *= num2;
	}

	// Token: 0x06005B72 RID: 23410 RVA: 0x00211814 File Offset: 0x0020FA14
	private unsafe bool SimMightOffcellOverpressure(int cell, SimHashes offgass)
	{
		SimHashes id = Grid.Element[cell].id;
		if (id == offgass || id == SimHashes.Vacuum)
		{
			return false;
		}
		IntPtr intPtr = stackalloc byte[(UIntPtr)12];
		*intPtr = Grid.CellLeft(cell);
		*(intPtr + 4) = Grid.CellRight(cell);
		*(intPtr + (IntPtr)2 * 4) = Grid.CellAbove(cell);
		ReadOnlySpan<int> readOnlySpan = new Span<int>(intPtr, 3);
		bool result = false;
		ReadOnlySpan<int> readOnlySpan2 = readOnlySpan;
		for (int i = 0; i < readOnlySpan2.Length; i++)
		{
			int num = *readOnlySpan2[i];
			if (Grid.IsValidCell(num))
			{
				if (Grid.Element[num].id == id)
				{
					return false;
				}
				if (Grid.Element[num].id == offgass)
				{
					result = true;
					if (Grid.Mass[num] < this.info.maxDestinationMass)
					{
						return false;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06005B73 RID: 23411 RVA: 0x002118D4 File Offset: 0x0020FAD4
	public void Sim200ms(float dt)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		bool flag = this.HasTag(GameTags.Sealed);
		Pickupable component = base.GetComponent<Pickupable>();
		Storage storage = (component != null) ? component.storage : null;
		if (flag && !this.decayStorage)
		{
			return;
		}
		if (flag && storage != null && storage.HasTag(GameTags.CorrosionProof))
		{
			return;
		}
		Element element = ElementLoader.FindElementByHash(this.info.sublimatedElement);
		if (this.primaryElement.Temperature <= element.lowTemp)
		{
			this.RefreshStatusItem(Sublimates.EmitState.BlockedOnTemperature);
			return;
		}
		float num2 = Grid.Mass[num];
		if (num2 < this.info.maxDestinationMass)
		{
			float num3 = this.primaryElement.Mass;
			if (num3 > 0f)
			{
				float num4 = Mathf.Pow(num3, this.info.massPower);
				float num5 = Mathf.Max(this.info.sublimationRate, this.info.sublimationRate * num4);
				num5 *= dt;
				num5 = Mathf.Min(num5, num3);
				this.sublimatedMass += num5;
				num3 -= num5;
				if (this.sublimatedMass > this.info.minSublimationAmount)
				{
					float num6 = this.sublimatedMass / this.primaryElement.Mass;
					byte diseaseIdx;
					int num7;
					if (this.info.diseaseIdx == 255)
					{
						diseaseIdx = this.primaryElement.DiseaseIdx;
						num7 = (int)((float)this.primaryElement.DiseaseCount * num6);
						this.primaryElement.ModifyDiseaseCount(-num7, "Sublimates.SimUpdate");
					}
					else
					{
						float num8 = this.sublimatedMass / this.info.sublimationRate;
						diseaseIdx = this.info.diseaseIdx;
						num7 = (int)((float)this.info.diseaseCount * num8);
					}
					float num9 = Mathf.Min(this.sublimatedMass, this.info.maxDestinationMass - num2);
					if (num9 <= 0f || this.SimMightOffcellOverpressure(num, element.id))
					{
						this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
						return;
					}
					this.Emit(num, num9, this.primaryElement.Temperature, diseaseIdx, num7);
					this.sublimatedMass = Mathf.Max(0f, this.sublimatedMass - num9);
					this.primaryElement.Mass = Mathf.Max(0f, this.primaryElement.Mass - num9);
					this.UpdateStorage();
					this.RefreshStatusItem(Sublimates.EmitState.Emitting);
					if (flag && this.decayStorage && storage != null)
					{
						storage.BoxingTrigger<BuildingHP.DamageSourceInfo>(-794517298, new BuildingHP.DamageSourceInfo
						{
							damage = 1,
							source = BUILDINGS.DAMAGESOURCES.CORROSIVE_ELEMENT,
							popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CORROSIVE_ELEMENT,
							fullDamageEffectName = "smoke_damage_kanim"
						});
						return;
					}
				}
			}
			else if (this.sublimatedMass > 0f)
			{
				float num10 = Mathf.Min(this.sublimatedMass, this.info.maxDestinationMass - num2);
				if (num10 > 0f && !this.SimMightOffcellOverpressure(num, element.id))
				{
					this.Emit(num, num10, this.primaryElement.Temperature, this.primaryElement.DiseaseIdx, this.primaryElement.DiseaseCount);
					this.sublimatedMass = Mathf.Max(0f, this.sublimatedMass - num10);
					this.primaryElement.Mass = Mathf.Max(0f, this.primaryElement.Mass - num10);
					this.UpdateStorage();
					this.RefreshStatusItem(Sublimates.EmitState.Emitting);
					return;
				}
				this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
				return;
			}
			else if (!this.primaryElement.KeepZeroMassObject)
			{
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
		}
		else
		{
			this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
		}
	}

	// Token: 0x06005B74 RID: 23412 RVA: 0x00211CA4 File Offset: 0x0020FEA4
	private void UpdateStorage()
	{
		Pickupable component = base.GetComponent<Pickupable>();
		if (component != null && component.storage != null)
		{
			component.storage.Trigger(-1697596308, base.gameObject);
		}
	}

	// Token: 0x06005B75 RID: 23413 RVA: 0x00211CE8 File Offset: 0x0020FEE8
	private void Emit(int cell, float mass, float temperature, byte disease_idx, int disease_count)
	{
		SimMessages.AddRemoveSubstance(cell, this.info.sublimatedElement, CellEventLogger.Instance.SublimatesEmit, mass, temperature, disease_idx, disease_count, true, -1);
		Game.Instance.accumulators.Accumulate(this.flowAccumulator, mass);
		if (this.spawnFXHash != SpawnFXHashes.None)
		{
			base.transform.GetPosition().z = Grid.GetLayerZ(Grid.SceneLayer.Front);
			Game.Instance.SpawnFX(this.spawnFXHash, base.transform.GetPosition(), 0f);
		}
	}

	// Token: 0x06005B76 RID: 23414 RVA: 0x00211D70 File Offset: 0x0020FF70
	public float AvgFlowRate()
	{
		return Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);
	}

	// Token: 0x06005B77 RID: 23415 RVA: 0x00211D88 File Offset: 0x0020FF88
	private void RefreshStatusItem(Sublimates.EmitState newEmitState)
	{
		if (newEmitState == this.lastEmitState)
		{
			return;
		}
		switch (newEmitState)
		{
		case Sublimates.EmitState.Emitting:
			if (this.info.sublimatedElement == SimHashes.Oxygen)
			{
				this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingOxygenAvg, this);
			}
			else
			{
				this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingGasAvg, this);
			}
			break;
		case Sublimates.EmitState.BlockedOnPressure:
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingBlockedHighPressure, this);
			break;
		case Sublimates.EmitState.BlockedOnTemperature:
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingBlockedLowTemperature, this);
			break;
		}
		this.lastEmitState = newEmitState;
	}

	// Token: 0x04003CE2 RID: 15586
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04003CE3 RID: 15587
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003CE4 RID: 15588
	[SerializeField]
	public SpawnFXHashes spawnFXHash;

	// Token: 0x04003CE5 RID: 15589
	public bool decayStorage;

	// Token: 0x04003CE6 RID: 15590
	[SerializeField]
	public Sublimates.Info info;

	// Token: 0x04003CE7 RID: 15591
	[Serialize]
	private float sublimatedMass;

	// Token: 0x04003CE8 RID: 15592
	private HandleVector<int>.Handle flowAccumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04003CE9 RID: 15593
	private Sublimates.EmitState lastEmitState = (Sublimates.EmitState)(-1);

	// Token: 0x04003CEA RID: 15594
	private static readonly EventSystem.IntraObjectHandler<Sublimates> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<Sublimates>(delegate(Sublimates component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x04003CEB RID: 15595
	private static readonly EventSystem.IntraObjectHandler<Sublimates> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<Sublimates>(delegate(Sublimates component, object data)
	{
		component.OnSplitFromChunk(data);
	});

	// Token: 0x02001D81 RID: 7553
	[Serializable]
	public struct Info
	{
		// Token: 0x0600B146 RID: 45382 RVA: 0x003DCC0C File Offset: 0x003DAE0C
		public Info(float rate, float min_amount, float max_destination_mass, float mass_power, SimHashes element, byte disease_idx = 255, int disease_count = 0)
		{
			this.sublimationRate = rate;
			this.minSublimationAmount = min_amount;
			this.maxDestinationMass = max_destination_mass;
			this.massPower = mass_power;
			this.sublimatedElement = element;
			this.diseaseIdx = disease_idx;
			this.diseaseCount = disease_count;
		}

		// Token: 0x04008B7F RID: 35711
		public float sublimationRate;

		// Token: 0x04008B80 RID: 35712
		public float minSublimationAmount;

		// Token: 0x04008B81 RID: 35713
		public float maxDestinationMass;

		// Token: 0x04008B82 RID: 35714
		public float massPower;

		// Token: 0x04008B83 RID: 35715
		public byte diseaseIdx;

		// Token: 0x04008B84 RID: 35716
		public int diseaseCount;

		// Token: 0x04008B85 RID: 35717
		[HashedEnum]
		public SimHashes sublimatedElement;
	}

	// Token: 0x02001D82 RID: 7554
	private enum EmitState
	{
		// Token: 0x04008B87 RID: 35719
		Emitting,
		// Token: 0x04008B88 RID: 35720
		BlockedOnPressure,
		// Token: 0x04008B89 RID: 35721
		BlockedOnTemperature
	}
}
