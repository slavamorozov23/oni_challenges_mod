using System;
using System.Runtime.InteropServices;

// Token: 0x02000878 RID: 2168
public class ConduitTemperatureManager
{
	// Token: 0x06003BA2 RID: 15266 RVA: 0x0014DAB6 File Offset: 0x0014BCB6
	public ConduitTemperatureManager()
	{
		ConduitTemperatureManager.ConduitTemperatureManager_Initialize();
	}

	// Token: 0x06003BA3 RID: 15267 RVA: 0x0014DADB File Offset: 0x0014BCDB
	public void Shutdown()
	{
		ConduitTemperatureManager.ConduitTemperatureManager_Shutdown();
	}

	// Token: 0x06003BA4 RID: 15268 RVA: 0x0014DAE4 File Offset: 0x0014BCE4
	public HandleVector<int>.Handle Allocate(ConduitType conduit_type, int conduit_idx, HandleVector<int>.Handle conduit_structure_temperature_handle, ref ConduitFlow.ConduitContents contents)
	{
		StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(conduit_structure_temperature_handle);
		Element element = payload.primaryElement.Element;
		BuildingDef def = payload.building.Def;
		float conduit_heat_capacity = def.MassForTemperatureModification * element.specificHeatCapacity;
		float conduit_thermal_conductivity = element.thermalConductivity * def.ThermalConductivity;
		int num = ConduitTemperatureManager.ConduitTemperatureManager_Add(contents.temperature, contents.mass, (int)contents.element, payload.simHandleCopy, conduit_heat_capacity, conduit_thermal_conductivity, def.ThermalConductivity < 1f);
		HandleVector<int>.Handle result = default(HandleVector<int>.Handle);
		result.index = num;
		int handleIndex = Sim.GetHandleIndex(num);
		if (handleIndex + 1 > this.temperatures.Length)
		{
			Array.Resize<float>(ref this.temperatures, (handleIndex + 1) * 2);
			Array.Resize<ConduitTemperatureManager.ConduitInfo>(ref this.conduitInfo, (handleIndex + 1) * 2);
		}
		this.temperatures[handleIndex] = contents.temperature;
		this.conduitInfo[handleIndex] = new ConduitTemperatureManager.ConduitInfo
		{
			type = conduit_type,
			idx = conduit_idx
		};
		return result;
	}

	// Token: 0x06003BA5 RID: 15269 RVA: 0x0014DBE8 File Offset: 0x0014BDE8
	public void SetData(HandleVector<int>.Handle handle, ref ConduitFlow.ConduitContents contents)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.temperatures[Sim.GetHandleIndex(handle.index)] = contents.temperature;
		ConduitTemperatureManager.ConduitTemperatureManager_Set(handle.index, contents.temperature, contents.mass, (int)contents.element);
	}

	// Token: 0x06003BA6 RID: 15270 RVA: 0x0014DC38 File Offset: 0x0014BE38
	public void Free(HandleVector<int>.Handle handle)
	{
		if (handle.IsValid())
		{
			int handleIndex = Sim.GetHandleIndex(handle.index);
			this.temperatures[handleIndex] = -1f;
			this.conduitInfo[handleIndex] = new ConduitTemperatureManager.ConduitInfo
			{
				type = ConduitType.None,
				idx = -1
			};
			ConduitTemperatureManager.ConduitTemperatureManager_Remove(handle.index);
		}
	}

	// Token: 0x06003BA7 RID: 15271 RVA: 0x0014DC99 File Offset: 0x0014BE99
	public void Clear()
	{
		ConduitTemperatureManager.ConduitTemperatureManager_Clear();
	}

	// Token: 0x06003BA8 RID: 15272 RVA: 0x0014DCA0 File Offset: 0x0014BEA0
	public unsafe void Sim200ms(float dt)
	{
		ConduitTemperatureManager.ConduitTemperatureUpdateData* ptr = (ConduitTemperatureManager.ConduitTemperatureUpdateData*)((void*)ConduitTemperatureManager.ConduitTemperatureManager_Update(dt, (IntPtr)((void*)Game.Instance.simData.buildingTemperatures)));
		int numEntries = ptr->numEntries;
		if (numEntries > 0)
		{
			Marshal.Copy((IntPtr)((void*)ptr->temperatures), this.temperatures, 0, numEntries);
		}
		for (int i = 0; i < ptr->numFrozenHandles; i++)
		{
			int handleIndex = Sim.GetHandleIndex(ptr->frozenHandles[i]);
			ConduitTemperatureManager.ConduitInfo conduitInfo = this.conduitInfo[handleIndex];
			Conduit.GetFlowManager(conduitInfo.type).FreezeConduitContents(conduitInfo.idx);
		}
		for (int j = 0; j < ptr->numMeltedHandles; j++)
		{
			int handleIndex2 = Sim.GetHandleIndex(ptr->meltedHandles[j]);
			ConduitTemperatureManager.ConduitInfo conduitInfo2 = this.conduitInfo[handleIndex2];
			Conduit.GetFlowManager(conduitInfo2.type).MeltConduitContents(conduitInfo2.idx);
		}
	}

	// Token: 0x06003BA9 RID: 15273 RVA: 0x0014DD89 File Offset: 0x0014BF89
	public float GetTemperature(HandleVector<int>.Handle handle)
	{
		return this.temperatures[Sim.GetHandleIndex(handle.index)];
	}

	// Token: 0x06003BAA RID: 15274
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Initialize();

	// Token: 0x06003BAB RID: 15275
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Shutdown();

	// Token: 0x06003BAC RID: 15276
	[DllImport("SimDLL")]
	private static extern int ConduitTemperatureManager_Add(float contents_temperature, float contents_mass, int contents_element_hash, int conduit_structure_temperature_handle, float conduit_heat_capacity, float conduit_thermal_conductivity, bool conduit_insulated);

	// Token: 0x06003BAD RID: 15277
	[DllImport("SimDLL")]
	private static extern int ConduitTemperatureManager_Set(int handle, float contents_temperature, float contents_mass, int contents_element_hash);

	// Token: 0x06003BAE RID: 15278
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Remove(int handle);

	// Token: 0x06003BAF RID: 15279
	[DllImport("SimDLL")]
	private static extern IntPtr ConduitTemperatureManager_Update(float dt, IntPtr building_conductivity_data);

	// Token: 0x06003BB0 RID: 15280
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Clear();

	// Token: 0x040024E7 RID: 9447
	private float[] temperatures = new float[0];

	// Token: 0x040024E8 RID: 9448
	private ConduitTemperatureManager.ConduitInfo[] conduitInfo = new ConduitTemperatureManager.ConduitInfo[0];

	// Token: 0x0200183B RID: 6203
	private struct ConduitInfo
	{
		// Token: 0x04007A51 RID: 31313
		public ConduitType type;

		// Token: 0x04007A52 RID: 31314
		public int idx;
	}

	// Token: 0x0200183C RID: 6204
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ConduitTemperatureUpdateData
	{
		// Token: 0x04007A53 RID: 31315
		public int numEntries;

		// Token: 0x04007A54 RID: 31316
		public unsafe float* temperatures;

		// Token: 0x04007A55 RID: 31317
		public int numFrozenHandles;

		// Token: 0x04007A56 RID: 31318
		public unsafe int* frozenHandles;

		// Token: 0x04007A57 RID: 31319
		public int numMeltedHandles;

		// Token: 0x04007A58 RID: 31320
		public unsafe int* meltedHandles;
	}
}
