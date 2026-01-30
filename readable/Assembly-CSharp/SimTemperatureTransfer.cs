using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000B4C RID: 2892
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SimTemperatureTransfer")]
public class SimTemperatureTransfer : KMonoBehaviour
{
	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x06005559 RID: 21849 RVA: 0x001F24DF File Offset: 0x001F06DF
	// (set) Token: 0x0600555A RID: 21850 RVA: 0x001F24E7 File Offset: 0x001F06E7
	public float SurfaceArea
	{
		get
		{
			return this.surfaceArea;
		}
		set
		{
			this.surfaceArea = value;
		}
	}

	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x0600555B RID: 21851 RVA: 0x001F24F0 File Offset: 0x001F06F0
	// (set) Token: 0x0600555C RID: 21852 RVA: 0x001F24F8 File Offset: 0x001F06F8
	public float Thickness
	{
		get
		{
			return this.thickness;
		}
		set
		{
			this.thickness = value;
		}
	}

	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x0600555D RID: 21853 RVA: 0x001F2501 File Offset: 0x001F0701
	// (set) Token: 0x0600555E RID: 21854 RVA: 0x001F2509 File Offset: 0x001F0709
	public float GroundTransferScale
	{
		get
		{
			return this.groundTransferScale;
		}
		set
		{
			this.groundTransferScale = value;
		}
	}

	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x0600555F RID: 21855 RVA: 0x001F2512 File Offset: 0x001F0712
	public int SimHandle
	{
		get
		{
			return this.simHandle;
		}
	}

	// Token: 0x06005560 RID: 21856 RVA: 0x001F251A File Offset: 0x001F071A
	public static void ClearInstanceMap()
	{
		SimTemperatureTransfer.handleInstanceMap.Clear();
	}

	// Token: 0x06005561 RID: 21857 RVA: 0x001F2528 File Offset: 0x001F0728
	public static void DoOreMeltTransition(int sim_handle)
	{
		SimTemperatureTransfer simTemperatureTransfer = null;
		if (!SimTemperatureTransfer.handleInstanceMap.TryGetValue(sim_handle, out simTemperatureTransfer))
		{
			return;
		}
		if (simTemperatureTransfer == null)
		{
			return;
		}
		if (simTemperatureTransfer.HasTag(GameTags.Sealed))
		{
			return;
		}
		PrimaryElement primaryElement = simTemperatureTransfer.pe;
		Element element = primaryElement.Element;
		bool flag = primaryElement.Temperature >= element.highTemp;
		bool flag2 = primaryElement.Temperature <= element.lowTemp;
		if (!flag && !flag2)
		{
			return;
		}
		if (flag && element.highTempTransitionTarget == SimHashes.Unobtanium)
		{
			return;
		}
		if (flag2 && element.lowTempTransitionTarget == SimHashes.Unobtanium)
		{
			return;
		}
		if (primaryElement.Mass > 0f)
		{
			int gameCell = Grid.PosToCell(simTemperatureTransfer.transform.GetPosition());
			float num = primaryElement.Mass;
			int num2 = primaryElement.DiseaseCount;
			SimHashes new_element = flag ? element.highTempTransitionTarget : element.lowTempTransitionTarget;
			SimHashes simHashes = flag ? element.highTempTransitionOreID : element.lowTempTransitionOreID;
			float num3 = flag ? element.highTempTransitionOreMassConversion : element.lowTempTransitionOreMassConversion;
			if (simHashes != (SimHashes)0)
			{
				float num4 = num * num3;
				int num5 = (int)((float)num2 * num3);
				if (num4 > 0.001f)
				{
					num -= num4;
					num2 -= num5;
					Element element2 = ElementLoader.FindElementByHash(simHashes);
					if (element2.IsSolid)
					{
						GameObject obj = element2.substance.SpawnResource(simTemperatureTransfer.transform.GetPosition(), num4, primaryElement.Temperature, primaryElement.DiseaseIdx, num5, true, false, true);
						element2.substance.ActivateSubstanceGameObject(obj, primaryElement.DiseaseIdx, num5);
					}
					else
					{
						SimMessages.AddRemoveSubstance(gameCell, element2.id, CellEventLogger.Instance.OreMelted, num4, primaryElement.Temperature, primaryElement.DiseaseIdx, num5, true, -1);
					}
				}
			}
			SimMessages.AddRemoveSubstance(gameCell, new_element, CellEventLogger.Instance.OreMelted, num, primaryElement.Temperature, primaryElement.DiseaseIdx, num2, true, -1);
		}
		simTemperatureTransfer.OnCleanUp();
		Util.KDestroyGameObject(simTemperatureTransfer.gameObject);
	}

	// Token: 0x06005562 RID: 21858 RVA: 0x001F2710 File Offset: 0x001F0910
	protected override void OnPrefabInit()
	{
		this.pe.sttOptimizationHook = this;
		this.pe.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(SimTemperatureTransfer.OnGetTemperature);
		this.pe.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(SimTemperatureTransfer.OnSetTemperature);
		PrimaryElement primaryElement = this.pe;
		primaryElement.onDataChanged = (Action<PrimaryElement>)Delegate.Combine(primaryElement.onDataChanged, new Action<PrimaryElement>(this.OnDataChanged));
	}

	// Token: 0x06005563 RID: 21859 RVA: 0x001F2780 File Offset: 0x001F0980
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Element element = this.pe.Element;
		this.cellChangedHandlerID = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, SimTemperatureTransfer.OnCellChangedDispatcher, this, "SimTemperatureTransfer.OnSpawn");
		if (!Grid.IsValidCell(Grid.PosToCell(this)) || this.pe.Element.HasTag(GameTags.Special) || element.specificHeatCapacity == 0f)
		{
			base.enabled = false;
		}
		this.SimRegister();
	}

	// Token: 0x06005564 RID: 21860 RVA: 0x001F27FE File Offset: 0x001F09FE
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.SimRegister();
		if (Sim.IsValidHandle(this.simHandle))
		{
			SimTemperatureTransfer.OnSetTemperature(this.pe, this.pe.Temperature);
		}
	}

	// Token: 0x06005565 RID: 21861 RVA: 0x001F2830 File Offset: 0x001F0A30
	protected override void OnCmpDisable()
	{
		if (Sim.IsValidHandle(this.simHandle))
		{
			float temperature = this.pe.Temperature;
			this.pe.InternalTemperature = this.pe.Temperature;
			SimMessages.SetElementChunkData(this.simHandle, temperature, 0f);
		}
		base.OnCmpDisable();
	}

	// Token: 0x06005566 RID: 21862 RVA: 0x001F2884 File Offset: 0x001F0A84
	private void OnCellChanged()
	{
		int cell = Grid.PosToCell(this);
		if (!Grid.IsValidCell(cell))
		{
			base.enabled = false;
			return;
		}
		this.SimRegister();
		if (Sim.IsValidHandle(this.simHandle))
		{
			SimMessages.MoveElementChunk(this.simHandle, cell);
			return;
		}
		this.forceDataSyncOnRegister = true;
	}

	// Token: 0x06005567 RID: 21863 RVA: 0x001F28CF File Offset: 0x001F0ACF
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(ref this.cellChangedHandlerID);
		this.SimUnregister();
		base.OnForcedCleanUp();
	}

	// Token: 0x06005568 RID: 21864 RVA: 0x001F28F0 File Offset: 0x001F0AF0
	private unsafe static float OnGetTemperature(PrimaryElement primary_element)
	{
		SimTemperatureTransfer sttOptimizationHook = primary_element.sttOptimizationHook;
		float result;
		if (Sim.IsValidHandle(sttOptimizationHook.simHandle))
		{
			int handleIndex = Sim.GetHandleIndex(sttOptimizationHook.simHandle);
			result = Game.Instance.simData.elementChunks[handleIndex].temperature;
			sttOptimizationHook.deltaKJ = Game.Instance.simData.elementChunks[handleIndex].deltaKJ;
		}
		else
		{
			result = primary_element.InternalTemperature;
		}
		return result;
	}

	// Token: 0x06005569 RID: 21865 RVA: 0x001F296C File Offset: 0x001F0B6C
	private unsafe static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		if (temperature <= 0f)
		{
			KCrashReporter.Assert(false, "STT.OnSetTemperature - Tried to set <= 0 degree temperature", null);
			temperature = 293f;
		}
		primary_element.InternalTemperature = temperature;
		SimTemperatureTransfer sttOptimizationHook = primary_element.sttOptimizationHook;
		if (Sim.IsValidHandle(sttOptimizationHook.simHandle))
		{
			float mass = primary_element.Mass;
			float heat_capacity = (mass >= 0.01f) ? (mass * primary_element.Element.specificHeatCapacity) : 0f;
			SimMessages.SetElementChunkData(sttOptimizationHook.simHandle, temperature, heat_capacity);
			int handleIndex = Sim.GetHandleIndex(sttOptimizationHook.simHandle);
			Game.Instance.simData.elementChunks[handleIndex].temperature = temperature;
		}
	}

	// Token: 0x0600556A RID: 21866 RVA: 0x001F2A0C File Offset: 0x001F0C0C
	private void OnDataChanged(PrimaryElement primary_element)
	{
		if (Sim.IsValidHandle(this.simHandle))
		{
			float heat_capacity = (primary_element.Mass >= 0.01f) ? (primary_element.Mass * primary_element.Element.specificHeatCapacity) : 0f;
			SimMessages.SetElementChunkData(this.simHandle, primary_element.Temperature, heat_capacity);
			return;
		}
		this.forceDataSyncOnRegister = true;
	}

	// Token: 0x0600556B RID: 21867 RVA: 0x001F2A68 File Offset: 0x001F0C68
	protected void SimRegister()
	{
		if (base.isSpawned && this.simHandle == -1 && base.enabled && this.pe.Mass > 0f && !this.pe.Element.IsTemperatureInsulated)
		{
			int gameCell = Grid.PosToCell(base.transform.GetPosition());
			this.simHandle = -2;
			HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle = Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(SimTemperatureTransfer.OnSimRegisteredCallback), this, "SimTemperatureTransfer.SimRegister");
			float num = this.pe.InternalTemperature;
			if (num <= 0f)
			{
				this.pe.InternalTemperature = 293f;
				num = 293f;
			}
			this.forceDataSyncOnRegister = false;
			SimMessages.AddElementChunk(gameCell, this.pe.ElementID, this.pe.Mass, num, this.surfaceArea, this.thickness, this.groundTransferScale, handle.index);
		}
	}

	// Token: 0x0600556C RID: 21868 RVA: 0x001F2B64 File Offset: 0x001F0D64
	protected unsafe void SimUnregister()
	{
		if (this.simHandle != -1 && !KMonoBehaviour.isLoadingScene)
		{
			if (Sim.IsValidHandle(this.simHandle))
			{
				int handleIndex = Sim.GetHandleIndex(this.simHandle);
				this.pe.InternalTemperature = Game.Instance.simData.elementChunks[handleIndex].temperature;
				SimMessages.RemoveElementChunk(this.simHandle, -1);
				SimTemperatureTransfer.handleInstanceMap.Remove(this.simHandle);
			}
			this.simHandle = -1;
		}
	}

	// Token: 0x0600556D RID: 21869 RVA: 0x001F2BE7 File Offset: 0x001F0DE7
	private static void OnSimRegisteredCallback(int handle, object data)
	{
		((SimTemperatureTransfer)data).OnSimRegistered(handle);
	}

	// Token: 0x0600556E RID: 21870 RVA: 0x001F2BF8 File Offset: 0x001F0DF8
	private unsafe void OnSimRegistered(int handle)
	{
		if (this != null && this.simHandle == -2)
		{
			this.simHandle = handle;
			int handleIndex = Sim.GetHandleIndex(handle);
			float temperature = Game.Instance.simData.elementChunks[handleIndex].temperature;
			float internalTemperature = this.pe.InternalTemperature;
			if (temperature <= 0f)
			{
				KCrashReporter.Assert(false, "Bad temperature", null);
			}
			SimTemperatureTransfer.handleInstanceMap[this.simHandle] = this;
			if (this.forceDataSyncOnRegister || Mathf.Abs(temperature - internalTemperature) > 0.1f)
			{
				float heat_capacity = (this.pe.Mass >= 0.01f) ? (this.pe.Mass * this.pe.Element.specificHeatCapacity) : 0f;
				SimMessages.SetElementChunkData(this.simHandle, internalTemperature, heat_capacity);
				SimMessages.MoveElementChunk(this.simHandle, Grid.PosToCell(this));
				Game.Instance.simData.elementChunks[handleIndex].temperature = internalTemperature;
			}
			if (this.onSimRegistered != null)
			{
				this.onSimRegistered(this);
			}
			if (!base.enabled)
			{
				this.OnCmpDisable();
				return;
			}
		}
		else
		{
			SimMessages.RemoveElementChunk(handle, -1);
		}
	}

	// Token: 0x0400399F RID: 14751
	[MyCmpReq]
	public PrimaryElement pe;

	// Token: 0x040039A0 RID: 14752
	private const float SIM_FREEZE_SPAWN_ORE_PERCENT = 0.8f;

	// Token: 0x040039A1 RID: 14753
	public const float MIN_MASS_FOR_TEMPERATURE_TRANSFER = 0.01f;

	// Token: 0x040039A2 RID: 14754
	public float deltaKJ;

	// Token: 0x040039A3 RID: 14755
	public Action<SimTemperatureTransfer> onSimRegistered;

	// Token: 0x040039A4 RID: 14756
	protected int simHandle = -1;

	// Token: 0x040039A5 RID: 14757
	protected bool forceDataSyncOnRegister;

	// Token: 0x040039A6 RID: 14758
	[SerializeField]
	protected float surfaceArea = 10f;

	// Token: 0x040039A7 RID: 14759
	[SerializeField]
	protected float thickness = 0.01f;

	// Token: 0x040039A8 RID: 14760
	[SerializeField]
	protected float groundTransferScale = 0.0625f;

	// Token: 0x040039A9 RID: 14761
	private static Dictionary<int, SimTemperatureTransfer> handleInstanceMap = new Dictionary<int, SimTemperatureTransfer>();

	// Token: 0x040039AA RID: 14762
	private ulong cellChangedHandlerID;

	// Token: 0x040039AB RID: 14763
	private static readonly Action<object> OnCellChangedDispatcher = delegate(object obj)
	{
		Unsafe.As<SimTemperatureTransfer>(obj).OnCellChanged();
	};
}
