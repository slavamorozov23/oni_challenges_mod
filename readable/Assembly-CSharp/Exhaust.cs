using System;
using UnityEngine;

// Token: 0x02000946 RID: 2374
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Exhaust")]
public class Exhaust : KMonoBehaviour, ISim200ms
{
	// Token: 0x06004233 RID: 16947 RVA: 0x0017585A File Offset: 0x00173A5A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Exhaust>(-592767678, Exhaust.OnConduitStateChangedDelegate);
		base.Subscribe<Exhaust>(-111137758, Exhaust.OnConduitStateChangedDelegate);
		base.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.NoWire;
		this.simRenderLoadBalance = true;
	}

	// Token: 0x06004234 RID: 16948 RVA: 0x00175897 File Offset: 0x00173A97
	protected override void OnSpawn()
	{
		this.OnConduitStateChanged(null);
	}

	// Token: 0x06004235 RID: 16949 RVA: 0x001758A0 File Offset: 0x00173AA0
	private void OnConduitStateChanged(object data)
	{
		this.operational.SetActive(this.operational.IsOperational && !this.vent.IsBlocked, false);
	}

	// Token: 0x06004236 RID: 16950 RVA: 0x001758CC File Offset: 0x00173ACC
	private void CalculateDiseaseTransfer(PrimaryElement item1, PrimaryElement item2, float transfer_rate, out int disease_to_item1, out int disease_to_item2)
	{
		disease_to_item1 = (int)((float)item2.DiseaseCount * transfer_rate);
		disease_to_item2 = (int)((float)item1.DiseaseCount * transfer_rate);
	}

	// Token: 0x06004237 RID: 16951 RVA: 0x001758E8 File Offset: 0x00173AE8
	public void Sim200ms(float dt)
	{
		this.operational.SetFlag(Exhaust.canExhaust, !this.vent.IsBlocked);
		if (!this.operational.IsOperational)
		{
			if (this.isAnimating)
			{
				this.isAnimating = false;
				this.recentlyExhausted = false;
				base.Trigger(-793429877, null);
			}
			return;
		}
		this.UpdateEmission();
		this.elapsedSwitchTime -= dt;
		if (this.elapsedSwitchTime <= 0f)
		{
			this.elapsedSwitchTime = 1f;
			if (this.recentlyExhausted != this.isAnimating)
			{
				this.isAnimating = this.recentlyExhausted;
				base.Trigger(-793429877, null);
			}
			this.recentlyExhausted = false;
		}
	}

	// Token: 0x06004238 RID: 16952 RVA: 0x0017599C File Offset: 0x00173B9C
	public bool IsAnimating()
	{
		return this.isAnimating;
	}

	// Token: 0x06004239 RID: 16953 RVA: 0x001759A4 File Offset: 0x00173BA4
	private void UpdateEmission()
	{
		if (this.consumer.ConsumptionRate == 0f)
		{
			return;
		}
		if (this.storage.items.Count == 0)
		{
			return;
		}
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (Grid.Solid[num])
		{
			return;
		}
		ConduitType typeOfConduit = this.consumer.TypeOfConduit;
		if (typeOfConduit != ConduitType.Gas)
		{
			if (typeOfConduit == ConduitType.Liquid)
			{
				this.EmitLiquid(num);
				return;
			}
		}
		else
		{
			this.EmitGas(num);
		}
	}

	// Token: 0x0600423A RID: 16954 RVA: 0x00175A1C File Offset: 0x00173C1C
	private bool EmitCommon(int cell, PrimaryElement primary_element, Exhaust.EmitDelegate emit)
	{
		if (primary_element.Mass <= 0f)
		{
			return false;
		}
		int num;
		int num2;
		this.CalculateDiseaseTransfer(this.exhaustPE, primary_element, 0.05f, out num, out num2);
		primary_element.ModifyDiseaseCount(-num, "Exhaust transfer");
		primary_element.AddDisease(this.exhaustPE.DiseaseIdx, num2, "Exhaust transfer");
		this.exhaustPE.ModifyDiseaseCount(-num2, "Exhaust transfer");
		this.exhaustPE.AddDisease(primary_element.DiseaseIdx, num, "Exhaust transfer");
		emit(cell, primary_element);
		if (this.vent != null)
		{
			this.vent.UpdateVentedMass(primary_element.ElementID, primary_element.Mass);
		}
		primary_element.KeepZeroMassObject = true;
		primary_element.Mass = 0f;
		primary_element.ModifyDiseaseCount(int.MinValue, "Exhaust.SimUpdate");
		if (this.lastElementEmmited != primary_element.ElementID)
		{
			this.lastElementEmmited = primary_element.ElementID;
			if (primary_element.Element != null && primary_element.Element.substance != null)
			{
				base.BoxingTrigger<Color32>(-793429877, primary_element.Element.substance.colour);
			}
		}
		this.recentlyExhausted = true;
		return true;
	}

	// Token: 0x0600423B RID: 16955 RVA: 0x00175B3C File Offset: 0x00173D3C
	private void EmitLiquid(int cell)
	{
		int num = Grid.CellBelow(cell);
		Exhaust.EmitDelegate emit = (Grid.IsValidCell(num) && !Grid.Solid[num]) ? Exhaust.emit_particle : Exhaust.emit_element;
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (component.Element.IsLiquid && this.EmitCommon(cell, component, emit))
			{
				break;
			}
		}
	}

	// Token: 0x0600423C RID: 16956 RVA: 0x00175BD8 File Offset: 0x00173DD8
	private void EmitGas(int cell)
	{
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (component.Element.IsGas && this.EmitCommon(cell, component, Exhaust.emit_element))
			{
				break;
			}
		}
	}

	// Token: 0x0400298C RID: 10636
	[MyCmpGet]
	private Vent vent;

	// Token: 0x0400298D RID: 10637
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400298E RID: 10638
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400298F RID: 10639
	[MyCmpGet]
	private ConduitConsumer consumer;

	// Token: 0x04002990 RID: 10640
	[MyCmpGet]
	private PrimaryElement exhaustPE;

	// Token: 0x04002991 RID: 10641
	private static readonly Operational.Flag canExhaust = new Operational.Flag("canExhaust", Operational.Flag.Type.Requirement);

	// Token: 0x04002992 RID: 10642
	private bool isAnimating;

	// Token: 0x04002993 RID: 10643
	private bool recentlyExhausted;

	// Token: 0x04002994 RID: 10644
	private const float MinSwitchTime = 1f;

	// Token: 0x04002995 RID: 10645
	private float elapsedSwitchTime;

	// Token: 0x04002996 RID: 10646
	private SimHashes lastElementEmmited;

	// Token: 0x04002997 RID: 10647
	private static readonly EventSystem.IntraObjectHandler<Exhaust> OnConduitStateChangedDelegate = new EventSystem.IntraObjectHandler<Exhaust>(delegate(Exhaust component, object data)
	{
		component.OnConduitStateChanged(data);
	});

	// Token: 0x04002998 RID: 10648
	private static Exhaust.EmitDelegate emit_element = delegate(int cell, PrimaryElement primary_element)
	{
		SimMessages.AddRemoveSubstance(cell, primary_element.ElementID, CellEventLogger.Instance.ExhaustSimUpdate, primary_element.Mass, primary_element.Temperature, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, -1);
	};

	// Token: 0x04002999 RID: 10649
	private static Exhaust.EmitDelegate emit_particle = delegate(int cell, PrimaryElement primary_element)
	{
		FallingWater.instance.AddParticle(cell, primary_element.Element.idx, primary_element.Mass, primary_element.Temperature, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, false, true, false);
	};

	// Token: 0x02001932 RID: 6450
	// (Invoke) Token: 0x0600A19F RID: 41375
	private delegate void EmitDelegate(int cell, PrimaryElement primary_element);
}
