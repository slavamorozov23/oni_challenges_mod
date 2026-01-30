using System;

// Token: 0x0200096A RID: 2410
public class GasBreatherFromWorldProvider : OxygenBreather.IGasProvider
{
	// Token: 0x06004478 RID: 17528 RVA: 0x0018B52E File Offset: 0x0018972E
	public GasBreatherFromWorldProvider.BreathableCellData GetBestBreathableCellAtCurrentLocation()
	{
		return GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(Grid.PosToCell(this.oxygenBreather), GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, this.oxygenBreather);
	}

	// Token: 0x06004479 RID: 17529 RVA: 0x0018B54C File Offset: 0x0018974C
	public static GasBreatherFromWorldProvider.BreathableCellData GetBestBreathableCellAroundSpecificCell(int theSpecificCell, CellOffset[] breathRange, OxygenBreather breather)
	{
		float num;
		return GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(theSpecificCell, breathRange, breather, out num);
	}

	// Token: 0x0600447A RID: 17530 RVA: 0x0018B564 File Offset: 0x00189764
	public static GasBreatherFromWorldProvider.BreathableCellData GetBestBreathableCellAroundSpecificCell(int theSpecificCell, CellOffset[] breathRange, OxygenBreather breather, out float totalBreathableMassAroundCell)
	{
		if (breathRange == null)
		{
			breathRange = GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS;
		}
		float num = 0f;
		int cell = theSpecificCell;
		SimHashes simHashes = SimHashes.Vacuum;
		totalBreathableMassAroundCell = 0f;
		foreach (CellOffset offset in breathRange)
		{
			int num2 = Grid.OffsetCell(theSpecificCell, offset);
			SimHashes simHashes2;
			float breathableCellMass = GasBreatherFromWorldProvider.GetBreathableCellMass(num2, out simHashes2);
			totalBreathableMassAroundCell += breathableCellMass;
			if (breathableCellMass > num && breathableCellMass > breather.noOxygenThreshold)
			{
				num = breathableCellMass;
				cell = num2;
				simHashes = simHashes2;
			}
		}
		return new GasBreatherFromWorldProvider.BreathableCellData
		{
			Cell = cell,
			ElementID = simHashes,
			Mass = num,
			IsBreathable = (simHashes != SimHashes.Vacuum)
		};
	}

	// Token: 0x0600447B RID: 17531 RVA: 0x0018B618 File Offset: 0x00189818
	private static float GetBreathableCellMass(int cell, out SimHashes elementID)
	{
		elementID = SimHashes.Vacuum;
		if (Grid.IsValidCell(cell))
		{
			Element element = Grid.Element[cell];
			if (element.HasTag(GameTags.Breathable))
			{
				elementID = element.id;
				return Grid.Mass[cell];
			}
		}
		return 0f;
	}

	// Token: 0x0600447C RID: 17532 RVA: 0x0018B662 File Offset: 0x00189862
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
		this.oxygenBreather = oxygen_breather;
		this.nav = this.oxygenBreather.GetComponent<Navigator>();
	}

	// Token: 0x0600447D RID: 17533 RVA: 0x0018B67C File Offset: 0x0018987C
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x0600447E RID: 17534 RVA: 0x0018B67E File Offset: 0x0018987E
	public bool ShouldEmitCO2()
	{
		return this.nav.CurrentNavType != NavType.Tube;
	}

	// Token: 0x0600447F RID: 17535 RVA: 0x0018B691 File Offset: 0x00189891
	public bool ShouldStoreCO2()
	{
		return false;
	}

	// Token: 0x06004480 RID: 17536 RVA: 0x0018B694 File Offset: 0x00189894
	public bool IsLowOxygen()
	{
		GasBreatherFromWorldProvider.BreathableCellData bestBreathableCellAtCurrentLocation = this.GetBestBreathableCellAtCurrentLocation();
		return bestBreathableCellAtCurrentLocation.IsBreathable && bestBreathableCellAtCurrentLocation.Mass < this.oxygenBreather.lowOxygenThreshold;
	}

	// Token: 0x06004481 RID: 17537 RVA: 0x0018B6C5 File Offset: 0x001898C5
	public bool HasOxygen()
	{
		return this.oxygenBreather.prefabID.HasTag(GameTags.RecoveringBreath) || this.oxygenBreather.prefabID.HasTag(GameTags.InTransitTube) || this.GetBestBreathableCellAtCurrentLocation().IsBreathable;
	}

	// Token: 0x06004482 RID: 17538 RVA: 0x0018B702 File Offset: 0x00189902
	public bool IsBlocked()
	{
		return this.oxygenBreather.HasTag(GameTags.HasSuitTank);
	}

	// Token: 0x06004483 RID: 17539 RVA: 0x0018B714 File Offset: 0x00189914
	public bool ConsumeGas(OxygenBreather oxygen_breather, float mass_to_consume)
	{
		if (this.nav.CurrentNavType != NavType.Tube)
		{
			GasBreatherFromWorldProvider.BreathableCellData bestBreathableCellAtCurrentLocation = this.GetBestBreathableCellAtCurrentLocation();
			if (!bestBreathableCellAtCurrentLocation.IsBreathable)
			{
				return false;
			}
			SimHashes elementID = bestBreathableCellAtCurrentLocation.ElementID;
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(GasBreatherFromWorldProvider.OnSimConsumeCallbackAction, oxygen_breather, "GasBreatherFromWorldProvider");
			SimMessages.ConsumeMass(bestBreathableCellAtCurrentLocation.Cell, elementID, mass_to_consume, 3, handle.index);
		}
		return true;
	}

	// Token: 0x06004484 RID: 17540 RVA: 0x0018B778 File Offset: 0x00189978
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		SimHashes id = ElementLoader.elements[(int)mass_cb_info.elemIdx].id;
		OxygenBreather.BreathableGasConsumed(data as OxygenBreather, id, mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
	}

	// Token: 0x04002E00 RID: 11776
	public static CellOffset[] DEFAULT_BREATHABLE_OFFSETS = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1),
		new CellOffset(1, 1),
		new CellOffset(-1, 1),
		new CellOffset(1, 0),
		new CellOffset(-1, 0)
	};

	// Token: 0x04002E01 RID: 11777
	private OxygenBreather oxygenBreather;

	// Token: 0x04002E02 RID: 11778
	private Navigator nav;

	// Token: 0x04002E03 RID: 11779
	private static Action<Sim.MassConsumedCallback, object> OnSimConsumeCallbackAction = new Action<Sim.MassConsumedCallback, object>(GasBreatherFromWorldProvider.OnSimConsumeCallback);

	// Token: 0x0200199E RID: 6558
	public struct BreathableCellData
	{
		// Token: 0x04007EC3 RID: 32451
		public int Cell;

		// Token: 0x04007EC4 RID: 32452
		public SimHashes ElementID;

		// Token: 0x04007EC5 RID: 32453
		public float Mass;

		// Token: 0x04007EC6 RID: 32454
		public bool IsBreathable;
	}
}
