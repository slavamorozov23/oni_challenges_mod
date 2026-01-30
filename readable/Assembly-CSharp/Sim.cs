using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000C7F RID: 3199
public static class Sim
{
	// Token: 0x060061D7 RID: 25047 RVA: 0x0024230C File Offset: 0x0024050C
	public static bool IsRadiationEnabled()
	{
		return DlcManager.FeatureRadiationEnabled();
	}

	// Token: 0x060061D8 RID: 25048 RVA: 0x00242313 File Offset: 0x00240513
	public static bool IsValidHandle(int h)
	{
		return h != -1 && h != -2;
	}

	// Token: 0x060061D9 RID: 25049 RVA: 0x00242323 File Offset: 0x00240523
	public static int GetHandleIndex(int h)
	{
		return h & 16777215;
	}

	// Token: 0x060061DA RID: 25050
	[DllImport("SimDLL")]
	public static extern void SIM_Initialize(Sim.GAME_MessageHandler callback);

	// Token: 0x060061DB RID: 25051
	[DllImport("SimDLL")]
	public static extern void SIM_Shutdown();

	// Token: 0x060061DC RID: 25052
	[DllImport("SimDLL")]
	public unsafe static extern IntPtr SIM_HandleMessage(int sim_msg_id, int msg_length, byte* msg);

	// Token: 0x060061DD RID: 25053
	[DllImport("SimDLL")]
	public unsafe static extern IntPtr SIM_HandleMessages(int sim_msg_id, int msg_length, int msg_count, byte* msg);

	// Token: 0x060061DE RID: 25054
	[DllImport("SimDLL")]
	private unsafe static extern byte* SIM_BeginSave(int* size, int x, int y);

	// Token: 0x060061DF RID: 25055
	[DllImport("SimDLL")]
	private static extern void SIM_EndSave();

	// Token: 0x060061E0 RID: 25056
	[DllImport("SimDLL")]
	public static extern void SIM_DebugCrash();

	// Token: 0x060061E1 RID: 25057 RVA: 0x0024232C File Offset: 0x0024052C
	public unsafe static IntPtr HandleMessage(SimMessageHashes sim_msg_id, int msg_length, byte[] msg)
	{
		IntPtr result;
		fixed (byte[] array = msg)
		{
			byte* msg2;
			if (msg == null || array.Length == 0)
			{
				msg2 = null;
			}
			else
			{
				msg2 = &array[0];
			}
			result = Sim.SIM_HandleMessage((int)sim_msg_id, msg_length, msg2);
		}
		return result;
	}

	// Token: 0x060061E2 RID: 25058 RVA: 0x0024235C File Offset: 0x0024055C
	public unsafe static void Save(BinaryWriter writer, int x, int y)
	{
		int num;
		void* value = (void*)Sim.SIM_BeginSave(&num, x, y);
		byte[] array = new byte[num];
		Marshal.Copy((IntPtr)value, array, 0, num);
		Sim.SIM_EndSave();
		writer.Write(num);
		writer.Write(array);
	}

	// Token: 0x060061E3 RID: 25059 RVA: 0x0024239C File Offset: 0x0024059C
	public unsafe static int LoadWorld(IReader reader)
	{
		int num = reader.ReadInt32();
		byte[] array;
		byte* msg;
		if ((array = reader.ReadBytes(num)) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		IntPtr value = Sim.SIM_HandleMessage(-672538170, num, msg);
		array = null;
		if (value == IntPtr.Zero)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x060061E4 RID: 25060 RVA: 0x002423EC File Offset: 0x002405EC
	public static void AllocateCells(int width, int height, bool headless = false)
	{
		using (MemoryStream memoryStream = new MemoryStream(16))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				binaryWriter.Write(width);
				binaryWriter.Write(height);
				bool value = Sim.IsRadiationEnabled();
				binaryWriter.Write(value);
				binaryWriter.Write(headless);
				binaryWriter.Flush();
				Sim.HandleMessage(SimMessageHashes.AllocateCells, (int)memoryStream.Length, memoryStream.GetBuffer());
			}
		}
	}

	// Token: 0x060061E5 RID: 25061 RVA: 0x0024247C File Offset: 0x0024067C
	public unsafe static int Load(IReader reader)
	{
		int num = reader.ReadInt32();
		byte[] array;
		byte* msg;
		if ((array = reader.ReadBytes(num)) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		IntPtr value = Sim.SIM_HandleMessage(-672538170, num, msg);
		array = null;
		if (value == IntPtr.Zero)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x060061E6 RID: 25062 RVA: 0x002424CC File Offset: 0x002406CC
	public unsafe static void Start()
	{
		Sim.GameDataUpdate* ptr = (Sim.GameDataUpdate*)((void*)Sim.SIM_HandleMessage(-931446686, 0, null));
		Grid.elementIdx = ptr->elementIdx;
		Grid.temperature = ptr->temperature;
		Grid.radiation = ptr->radiation;
		Grid.mass = ptr->mass;
		Grid.properties = ptr->properties;
		Grid.strengthInfo = ptr->strengthInfo;
		Grid.insulation = ptr->insulation;
		Grid.diseaseIdx = ptr->diseaseIdx;
		Grid.diseaseCount = ptr->diseaseCount;
		Grid.AccumulatedFlowValues = ptr->accumulatedFlow;
		PropertyTextures.externalFlowTex = ptr->propertyTextureFlow;
		PropertyTextures.externalLiquidTex = ptr->propertyTextureLiquid;
		PropertyTextures.externalLiquidDataTex = ptr->propertyTextureLiquidData;
		PropertyTextures.externalExposedToSunlight = ptr->propertyTextureExposedToSunlight;
		Grid.InitializeCells();
	}

	// Token: 0x060061E7 RID: 25063 RVA: 0x0024258B File Offset: 0x0024078B
	public static void Shutdown()
	{
		Sim.SIM_Shutdown();
		Grid.mass = null;
	}

	// Token: 0x060061E8 RID: 25064
	[DllImport("SimDLL")]
	public unsafe static extern char* SYSINFO_Acquire();

	// Token: 0x060061E9 RID: 25065
	[DllImport("SimDLL")]
	public static extern void SYSINFO_Release();

	// Token: 0x060061EA RID: 25066 RVA: 0x0024259C File Offset: 0x0024079C
	public unsafe static int DLL_MessageHandler(int message_id, IntPtr data)
	{
		if (message_id == 0)
		{
			Sim.DLLExceptionHandlerMessage* ptr = (Sim.DLLExceptionHandlerMessage*)((void*)data);
			string stack_trace = Marshal.PtrToStringAnsi(ptr->callstack);
			string dmp_filename = Marshal.PtrToStringAnsi(ptr->dmpFilename);
			KCrashReporter.ReportSimDLLCrash("SimDLL Crash Dump", stack_trace, dmp_filename);
			return 0;
		}
		if (message_id == 1)
		{
			Sim.DLLReportMessageMessage* ptr2 = (Sim.DLLReportMessageMessage*)((void*)data);
			string msg = "SimMessage: " + Marshal.PtrToStringAnsi(ptr2->message);
			string stack_trace2;
			if (ptr2->callstack != IntPtr.Zero)
			{
				stack_trace2 = Marshal.PtrToStringAnsi(ptr2->callstack);
			}
			else
			{
				string str = Marshal.PtrToStringAnsi(ptr2->file);
				int line = ptr2->line;
				stack_trace2 = str + ":" + line.ToString();
			}
			KCrashReporter.ReportSimDLLCrash(msg, stack_trace2, null);
			return 0;
		}
		return -1;
	}

	// Token: 0x0400418C RID: 16780
	public const int InvalidHandle = -1;

	// Token: 0x0400418D RID: 16781
	public const int QueuedRegisterHandle = -2;

	// Token: 0x0400418E RID: 16782
	public const byte InvalidDiseaseIdx = 255;

	// Token: 0x0400418F RID: 16783
	public const ushort InvalidElementIdx = 65535;

	// Token: 0x04004190 RID: 16784
	public const byte SpaceZoneID = 255;

	// Token: 0x04004191 RID: 16785
	public const byte SolidZoneID = 0;

	// Token: 0x04004192 RID: 16786
	public const int ChunkEdgeSize = 32;

	// Token: 0x04004193 RID: 16787
	public const float StateTransitionEnergy = 3f;

	// Token: 0x04004194 RID: 16788
	public const float ZeroDegreesCentigrade = 273.15f;

	// Token: 0x04004195 RID: 16789
	public const float StandardTemperature = 293.15f;

	// Token: 0x04004196 RID: 16790
	public const float StandardMeltingPointOffset = 10f;

	// Token: 0x04004197 RID: 16791
	public const float StandardPressure = 101.3f;

	// Token: 0x04004198 RID: 16792
	public const float Epsilon = 0.0001f;

	// Token: 0x04004199 RID: 16793
	public const float MaxTemperature = 10000f;

	// Token: 0x0400419A RID: 16794
	public const float MinTemperature = 0f;

	// Token: 0x0400419B RID: 16795
	public const float MaxRadiation = 9000000f;

	// Token: 0x0400419C RID: 16796
	public const float MinRadiation = 0f;

	// Token: 0x0400419D RID: 16797
	public const float MaxMass = 10000f;

	// Token: 0x0400419E RID: 16798
	public const float MinMass = 1.0001f;

	// Token: 0x0400419F RID: 16799
	public const float MAX_SUBLIMATE_MASS = 1.8f;

	// Token: 0x040041A0 RID: 16800
	private const int PressureUpdateInterval = 1;

	// Token: 0x040041A1 RID: 16801
	private const int TemperatureUpdateInterval = 1;

	// Token: 0x040041A2 RID: 16802
	private const int LiquidUpdateInterval = 1;

	// Token: 0x040041A3 RID: 16803
	private const int LifeUpdateInterval = 1;

	// Token: 0x040041A4 RID: 16804
	public const byte ClearSkyGridValue = 253;

	// Token: 0x040041A5 RID: 16805
	public const int PACKING_ALIGNMENT = 4;

	// Token: 0x02001E5D RID: 7773
	// (Invoke) Token: 0x0600B470 RID: 46192
	public delegate int GAME_MessageHandler(int message_id, IntPtr data);

	// Token: 0x02001E5E RID: 7774
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DLLExceptionHandlerMessage
	{
		// Token: 0x04008E89 RID: 36489
		public IntPtr callstack;

		// Token: 0x04008E8A RID: 36490
		public IntPtr dmpFilename;
	}

	// Token: 0x02001E5F RID: 7775
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DLLReportMessageMessage
	{
		// Token: 0x04008E8B RID: 36491
		public IntPtr callstack;

		// Token: 0x04008E8C RID: 36492
		public IntPtr message;

		// Token: 0x04008E8D RID: 36493
		public IntPtr file;

		// Token: 0x04008E8E RID: 36494
		public int line;
	}

	// Token: 0x02001E60 RID: 7776
	private enum GameHandledMessages
	{
		// Token: 0x04008E90 RID: 36496
		ExceptionHandler,
		// Token: 0x04008E91 RID: 36497
		ReportMessage
	}

	// Token: 0x02001E61 RID: 7777
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PhysicsData
	{
		// Token: 0x0600B473 RID: 46195 RVA: 0x003EB040 File Offset: 0x003E9240
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.temperature);
			writer.Write(this.mass);
			writer.Write(this.pressure);
		}

		// Token: 0x04008E92 RID: 36498
		public float temperature;

		// Token: 0x04008E93 RID: 36499
		public float mass;

		// Token: 0x04008E94 RID: 36500
		public float pressure;
	}

	// Token: 0x02001E62 RID: 7778
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Cell
	{
		// Token: 0x0600B474 RID: 46196 RVA: 0x003EB06C File Offset: 0x003E926C
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.elementIdx);
			writer.Write(0);
			writer.Write(this.insulation);
			writer.Write(0);
			writer.Write(this.pad0);
			writer.Write(this.pad1);
			writer.Write(this.pad2);
			writer.Write(this.temperature);
			writer.Write(this.mass);
		}

		// Token: 0x0600B475 RID: 46197 RVA: 0x003EB0DD File Offset: 0x003E92DD
		public void SetValues(global::Element elem, List<global::Element> elements)
		{
			this.SetValues(elem, elem.defaultValues, elements);
		}

		// Token: 0x0600B476 RID: 46198 RVA: 0x003EB0F0 File Offset: 0x003E92F0
		public void SetValues(global::Element elem, Sim.PhysicsData pd, List<global::Element> elements)
		{
			this.elementIdx = (ushort)elements.IndexOf(elem);
			this.temperature = pd.temperature;
			this.mass = pd.mass;
			this.insulation = byte.MaxValue;
			DebugUtil.Assert(this.temperature > 0f || this.mass == 0f, "A non-zero mass cannot have a <= 0 temperature");
		}

		// Token: 0x0600B477 RID: 46199 RVA: 0x003EB158 File Offset: 0x003E9358
		public void SetValues(ushort new_elem_idx, float new_temperature, float new_mass)
		{
			this.elementIdx = new_elem_idx;
			this.temperature = new_temperature;
			this.mass = new_mass;
			this.insulation = byte.MaxValue;
			DebugUtil.Assert(this.temperature > 0f || this.mass == 0f, "A non-zero mass cannot have a <= 0 temperature");
		}

		// Token: 0x04008E95 RID: 36501
		public ushort elementIdx;

		// Token: 0x04008E96 RID: 36502
		public byte properties;

		// Token: 0x04008E97 RID: 36503
		public byte insulation;

		// Token: 0x04008E98 RID: 36504
		public byte strengthInfo;

		// Token: 0x04008E99 RID: 36505
		public byte pad0;

		// Token: 0x04008E9A RID: 36506
		public byte pad1;

		// Token: 0x04008E9B RID: 36507
		public byte pad2;

		// Token: 0x04008E9C RID: 36508
		public float temperature;

		// Token: 0x04008E9D RID: 36509
		public float mass;

		// Token: 0x02002A6D RID: 10861
		public enum Properties
		{
			// Token: 0x0400BB57 RID: 47959
			GasImpermeable = 1,
			// Token: 0x0400BB58 RID: 47960
			LiquidImpermeable,
			// Token: 0x0400BB59 RID: 47961
			SolidImpermeable = 4,
			// Token: 0x0400BB5A RID: 47962
			Unbreakable = 8,
			// Token: 0x0400BB5B RID: 47963
			Transparent = 16,
			// Token: 0x0400BB5C RID: 47964
			Opaque = 32,
			// Token: 0x0400BB5D RID: 47965
			NotifyOnMelt = 64,
			// Token: 0x0400BB5E RID: 47966
			ConstructedTile = 128
		}
	}

	// Token: 0x02001E63 RID: 7779
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Element
	{
		// Token: 0x0600B478 RID: 46200 RVA: 0x003EB1AC File Offset: 0x003E93AC
		public Element(global::Element e, List<global::Element> elements)
		{
			this.id = e.id;
			this.state = (byte)e.state;
			if (e.HasTag(GameTags.Unstable))
			{
				this.state |= 8;
			}
			int num = elements.FindIndex((global::Element ele) => ele.id == e.lowTempTransitionTarget);
			int num2 = elements.FindIndex((global::Element ele) => ele.id == e.highTempTransitionTarget);
			this.lowTempTransitionIdx = (ushort)((num >= 0) ? num : 65535);
			this.highTempTransitionIdx = (ushort)((num2 >= 0) ? num2 : 65535);
			this.elementsTableIdx = (ushort)elements.IndexOf(e);
			this.specificHeatCapacity = e.specificHeatCapacity;
			this.thermalConductivity = e.thermalConductivity;
			this.solidSurfaceAreaMultiplier = e.solidSurfaceAreaMultiplier;
			this.liquidSurfaceAreaMultiplier = e.liquidSurfaceAreaMultiplier;
			this.gasSurfaceAreaMultiplier = e.gasSurfaceAreaMultiplier;
			this.molarMass = e.molarMass;
			this.strength = e.strength;
			this.flow = e.flow;
			this.viscosity = e.viscosity;
			this.minHorizontalFlow = e.minHorizontalFlow;
			this.minVerticalFlow = e.minVerticalFlow;
			this.maxMass = e.maxMass;
			this.lowTemp = e.lowTemp;
			this.highTemp = e.highTemp;
			this.highTempTransitionOreID = e.highTempTransitionOreID;
			this.highTempTransitionOreMassConversion = e.highTempTransitionOreMassConversion;
			this.lowTempTransitionOreID = e.lowTempTransitionOreID;
			this.lowTempTransitionOreMassConversion = e.lowTempTransitionOreMassConversion;
			this.sublimateIndex = (ushort)elements.FindIndex((global::Element ele) => ele.id == e.sublimateId);
			this.convertIndex = (ushort)elements.FindIndex((global::Element ele) => ele.id == e.convertId);
			this.pack0 = 0;
			if (e.substance == null)
			{
				this.colour = 0U;
			}
			else
			{
				Color32 color = e.substance.colour;
				this.colour = (uint)((int)color.a << 24 | (int)color.b << 16 | (int)color.g << 8 | (int)color.r);
			}
			this.sublimateFX = e.sublimateFX;
			this.sublimateRate = e.sublimateRate;
			this.sublimateEfficiency = e.sublimateEfficiency;
			this.sublimateProbability = e.sublimateProbability;
			this.offGasProbability = e.offGasPercentage;
			this.lightAbsorptionFactor = e.lightAbsorptionFactor;
			this.radiationAbsorptionFactor = e.radiationAbsorptionFactor;
			this.radiationPer1000Mass = e.radiationPer1000Mass;
			this.defaultValues = e.defaultValues;
		}

		// Token: 0x0600B479 RID: 46201 RVA: 0x003EB4BC File Offset: 0x003E96BC
		public void Write(BinaryWriter writer)
		{
			writer.Write((int)this.id);
			writer.Write(this.lowTempTransitionIdx);
			writer.Write(this.highTempTransitionIdx);
			writer.Write(this.elementsTableIdx);
			writer.Write(this.state);
			writer.Write(this.pack0);
			writer.Write(this.specificHeatCapacity);
			writer.Write(this.thermalConductivity);
			writer.Write(this.molarMass);
			writer.Write(this.solidSurfaceAreaMultiplier);
			writer.Write(this.liquidSurfaceAreaMultiplier);
			writer.Write(this.gasSurfaceAreaMultiplier);
			writer.Write(this.flow);
			writer.Write(this.viscosity);
			writer.Write(this.minHorizontalFlow);
			writer.Write(this.minVerticalFlow);
			writer.Write(this.maxMass);
			writer.Write(this.lowTemp);
			writer.Write(this.highTemp);
			writer.Write(this.strength);
			writer.Write((int)this.lowTempTransitionOreID);
			writer.Write(this.lowTempTransitionOreMassConversion);
			writer.Write((int)this.highTempTransitionOreID);
			writer.Write(this.highTempTransitionOreMassConversion);
			writer.Write(this.sublimateIndex);
			writer.Write(this.convertIndex);
			writer.Write(this.colour);
			writer.Write((int)this.sublimateFX);
			writer.Write(this.sublimateRate);
			writer.Write(this.sublimateEfficiency);
			writer.Write(this.sublimateProbability);
			writer.Write(this.offGasProbability);
			writer.Write(this.lightAbsorptionFactor);
			writer.Write(this.radiationAbsorptionFactor);
			writer.Write(this.radiationPer1000Mass);
			this.defaultValues.Write(writer);
		}

		// Token: 0x04008E9E RID: 36510
		public SimHashes id;

		// Token: 0x04008E9F RID: 36511
		public ushort lowTempTransitionIdx;

		// Token: 0x04008EA0 RID: 36512
		public ushort highTempTransitionIdx;

		// Token: 0x04008EA1 RID: 36513
		public ushort elementsTableIdx;

		// Token: 0x04008EA2 RID: 36514
		public byte state;

		// Token: 0x04008EA3 RID: 36515
		public byte pack0;

		// Token: 0x04008EA4 RID: 36516
		public float specificHeatCapacity;

		// Token: 0x04008EA5 RID: 36517
		public float thermalConductivity;

		// Token: 0x04008EA6 RID: 36518
		public float molarMass;

		// Token: 0x04008EA7 RID: 36519
		public float solidSurfaceAreaMultiplier;

		// Token: 0x04008EA8 RID: 36520
		public float liquidSurfaceAreaMultiplier;

		// Token: 0x04008EA9 RID: 36521
		public float gasSurfaceAreaMultiplier;

		// Token: 0x04008EAA RID: 36522
		public float flow;

		// Token: 0x04008EAB RID: 36523
		public float viscosity;

		// Token: 0x04008EAC RID: 36524
		public float minHorizontalFlow;

		// Token: 0x04008EAD RID: 36525
		public float minVerticalFlow;

		// Token: 0x04008EAE RID: 36526
		public float maxMass;

		// Token: 0x04008EAF RID: 36527
		public float lowTemp;

		// Token: 0x04008EB0 RID: 36528
		public float highTemp;

		// Token: 0x04008EB1 RID: 36529
		public float strength;

		// Token: 0x04008EB2 RID: 36530
		public SimHashes lowTempTransitionOreID;

		// Token: 0x04008EB3 RID: 36531
		public float lowTempTransitionOreMassConversion;

		// Token: 0x04008EB4 RID: 36532
		public SimHashes highTempTransitionOreID;

		// Token: 0x04008EB5 RID: 36533
		public float highTempTransitionOreMassConversion;

		// Token: 0x04008EB6 RID: 36534
		public ushort sublimateIndex;

		// Token: 0x04008EB7 RID: 36535
		public ushort convertIndex;

		// Token: 0x04008EB8 RID: 36536
		public uint colour;

		// Token: 0x04008EB9 RID: 36537
		public SpawnFXHashes sublimateFX;

		// Token: 0x04008EBA RID: 36538
		public float sublimateRate;

		// Token: 0x04008EBB RID: 36539
		public float sublimateEfficiency;

		// Token: 0x04008EBC RID: 36540
		public float sublimateProbability;

		// Token: 0x04008EBD RID: 36541
		public float offGasProbability;

		// Token: 0x04008EBE RID: 36542
		public float lightAbsorptionFactor;

		// Token: 0x04008EBF RID: 36543
		public float radiationAbsorptionFactor;

		// Token: 0x04008EC0 RID: 36544
		public float radiationPer1000Mass;

		// Token: 0x04008EC1 RID: 36545
		public Sim.PhysicsData defaultValues;
	}

	// Token: 0x02001E64 RID: 7780
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseCell
	{
		// Token: 0x0600B47A RID: 46202 RVA: 0x003EB690 File Offset: 0x003E9890
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.diseaseIdx);
			writer.Write(this.reservedInfestationTickCount);
			writer.Write(this.pad1);
			writer.Write(this.pad2);
			writer.Write(this.elementCount);
			writer.Write(this.reservedAccumulatedError);
		}

		// Token: 0x04008EC2 RID: 36546
		public byte diseaseIdx;

		// Token: 0x04008EC3 RID: 36547
		private byte reservedInfestationTickCount;

		// Token: 0x04008EC4 RID: 36548
		private byte pad1;

		// Token: 0x04008EC5 RID: 36549
		private byte pad2;

		// Token: 0x04008EC6 RID: 36550
		public int elementCount;

		// Token: 0x04008EC7 RID: 36551
		private float reservedAccumulatedError;

		// Token: 0x04008EC8 RID: 36552
		public static readonly Sim.DiseaseCell Invalid = new Sim.DiseaseCell
		{
			diseaseIdx = byte.MaxValue,
			elementCount = 0
		};
	}

	// Token: 0x02001E65 RID: 7781
	// (Invoke) Token: 0x0600B47D RID: 46205
	public delegate void GAME_Callback();

	// Token: 0x02001E66 RID: 7782
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SolidInfo
	{
		// Token: 0x04008EC9 RID: 36553
		public int cellIdx;

		// Token: 0x04008ECA RID: 36554
		public int isSolid;
	}

	// Token: 0x02001E67 RID: 7783
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct LiquidChangeInfo
	{
		// Token: 0x04008ECB RID: 36555
		public int cellIdx;
	}

	// Token: 0x02001E68 RID: 7784
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SolidSubstanceChangeInfo
	{
		// Token: 0x04008ECC RID: 36556
		public int cellIdx;
	}

	// Token: 0x02001E69 RID: 7785
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SubstanceChangeInfo
	{
		// Token: 0x04008ECD RID: 36557
		public int cellIdx;

		// Token: 0x04008ECE RID: 36558
		public ushort oldElemIdx;

		// Token: 0x04008ECF RID: 36559
		public ushort newElemIdx;
	}

	// Token: 0x02001E6A RID: 7786
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CallbackInfo
	{
		// Token: 0x04008ED0 RID: 36560
		public int callbackIdx;
	}

	// Token: 0x02001E6B RID: 7787
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GameDataUpdate
	{
		// Token: 0x04008ED1 RID: 36561
		public int numFramesProcessed;

		// Token: 0x04008ED2 RID: 36562
		public unsafe ushort* elementIdx;

		// Token: 0x04008ED3 RID: 36563
		public unsafe float* temperature;

		// Token: 0x04008ED4 RID: 36564
		public unsafe float* mass;

		// Token: 0x04008ED5 RID: 36565
		public unsafe byte* properties;

		// Token: 0x04008ED6 RID: 36566
		public unsafe byte* insulation;

		// Token: 0x04008ED7 RID: 36567
		public unsafe byte* strengthInfo;

		// Token: 0x04008ED8 RID: 36568
		public unsafe float* radiation;

		// Token: 0x04008ED9 RID: 36569
		public unsafe byte* diseaseIdx;

		// Token: 0x04008EDA RID: 36570
		public unsafe int* diseaseCount;

		// Token: 0x04008EDB RID: 36571
		public int numSolidInfo;

		// Token: 0x04008EDC RID: 36572
		public unsafe Sim.SolidInfo* solidInfo;

		// Token: 0x04008EDD RID: 36573
		public int numLiquidChangeInfo;

		// Token: 0x04008EDE RID: 36574
		public unsafe Sim.LiquidChangeInfo* liquidChangeInfo;

		// Token: 0x04008EDF RID: 36575
		public int numSolidSubstanceChangeInfo;

		// Token: 0x04008EE0 RID: 36576
		public unsafe Sim.SolidSubstanceChangeInfo* solidSubstanceChangeInfo;

		// Token: 0x04008EE1 RID: 36577
		public int numSubstanceChangeInfo;

		// Token: 0x04008EE2 RID: 36578
		public unsafe Sim.SubstanceChangeInfo* substanceChangeInfo;

		// Token: 0x04008EE3 RID: 36579
		public int numCallbackInfo;

		// Token: 0x04008EE4 RID: 36580
		public unsafe Sim.CallbackInfo* callbackInfo;

		// Token: 0x04008EE5 RID: 36581
		public int numSpawnFallingLiquidInfo;

		// Token: 0x04008EE6 RID: 36582
		public unsafe Sim.SpawnFallingLiquidInfo* spawnFallingLiquidInfo;

		// Token: 0x04008EE7 RID: 36583
		public int numDigInfo;

		// Token: 0x04008EE8 RID: 36584
		public unsafe Sim.SpawnOreInfo* digInfo;

		// Token: 0x04008EE9 RID: 36585
		public int numSpawnOreInfo;

		// Token: 0x04008EEA RID: 36586
		public unsafe Sim.SpawnOreInfo* spawnOreInfo;

		// Token: 0x04008EEB RID: 36587
		public int numSpawnFXInfo;

		// Token: 0x04008EEC RID: 36588
		public unsafe Sim.SpawnFXInfo* spawnFXInfo;

		// Token: 0x04008EED RID: 36589
		public int numUnstableCellInfo;

		// Token: 0x04008EEE RID: 36590
		public unsafe Sim.UnstableCellInfo* unstableCellInfo;

		// Token: 0x04008EEF RID: 36591
		public int numWorldDamageInfo;

		// Token: 0x04008EF0 RID: 36592
		public unsafe Sim.WorldDamageInfo* worldDamageInfo;

		// Token: 0x04008EF1 RID: 36593
		public int numBuildingTemperatures;

		// Token: 0x04008EF2 RID: 36594
		public unsafe Sim.BuildingTemperatureInfo* buildingTemperatures;

		// Token: 0x04008EF3 RID: 36595
		public int numMassConsumedCallbacks;

		// Token: 0x04008EF4 RID: 36596
		public unsafe Sim.MassConsumedCallback* massConsumedCallbacks;

		// Token: 0x04008EF5 RID: 36597
		public int numMassEmittedCallbacks;

		// Token: 0x04008EF6 RID: 36598
		public unsafe Sim.MassEmittedCallback* massEmittedCallbacks;

		// Token: 0x04008EF7 RID: 36599
		public int numDiseaseConsumptionCallbacks;

		// Token: 0x04008EF8 RID: 36600
		public unsafe Sim.DiseaseConsumptionCallback* diseaseConsumptionCallbacks;

		// Token: 0x04008EF9 RID: 36601
		public int numComponentStateChangedMessages;

		// Token: 0x04008EFA RID: 36602
		public unsafe Sim.ComponentStateChangedMessage* componentStateChangedMessages;

		// Token: 0x04008EFB RID: 36603
		public int numRemovedMassEntries;

		// Token: 0x04008EFC RID: 36604
		public unsafe Sim.ConsumedMassInfo* removedMassEntries;

		// Token: 0x04008EFD RID: 36605
		public int numEmittedMassEntries;

		// Token: 0x04008EFE RID: 36606
		public unsafe Sim.EmittedMassInfo* emittedMassEntries;

		// Token: 0x04008EFF RID: 36607
		public int numElementChunkInfos;

		// Token: 0x04008F00 RID: 36608
		public unsafe Sim.ElementChunkInfo* elementChunkInfos;

		// Token: 0x04008F01 RID: 36609
		public int numElementChunkMeltedInfos;

		// Token: 0x04008F02 RID: 36610
		public unsafe Sim.MeltedInfo* elementChunkMeltedInfos;

		// Token: 0x04008F03 RID: 36611
		public int numBuildingOverheatInfos;

		// Token: 0x04008F04 RID: 36612
		public unsafe Sim.MeltedInfo* buildingOverheatInfos;

		// Token: 0x04008F05 RID: 36613
		public int numBuildingNoLongerOverheatedInfos;

		// Token: 0x04008F06 RID: 36614
		public unsafe Sim.MeltedInfo* buildingNoLongerOverheatedInfos;

		// Token: 0x04008F07 RID: 36615
		public int numBuildingMeltedInfos;

		// Token: 0x04008F08 RID: 36616
		public unsafe Sim.MeltedInfo* buildingMeltedInfos;

		// Token: 0x04008F09 RID: 36617
		public int numCellMeltedInfos;

		// Token: 0x04008F0A RID: 36618
		public unsafe Sim.CellMeltedInfo* cellMeltedInfos;

		// Token: 0x04008F0B RID: 36619
		public int numDiseaseEmittedInfos;

		// Token: 0x04008F0C RID: 36620
		public unsafe Sim.DiseaseEmittedInfo* diseaseEmittedInfos;

		// Token: 0x04008F0D RID: 36621
		public int numDiseaseConsumedInfos;

		// Token: 0x04008F0E RID: 36622
		public unsafe Sim.DiseaseConsumedInfo* diseaseConsumedInfos;

		// Token: 0x04008F0F RID: 36623
		public int numRadiationConsumedCallbacks;

		// Token: 0x04008F10 RID: 36624
		public unsafe Sim.ConsumedRadiationCallback* radiationConsumedCallbacks;

		// Token: 0x04008F11 RID: 36625
		public unsafe float* accumulatedFlow;

		// Token: 0x04008F12 RID: 36626
		public IntPtr propertyTextureFlow;

		// Token: 0x04008F13 RID: 36627
		public IntPtr propertyTextureLiquid;

		// Token: 0x04008F14 RID: 36628
		public IntPtr propertyTextureLiquidData;

		// Token: 0x04008F15 RID: 36629
		public IntPtr propertyTextureExposedToSunlight;
	}

	// Token: 0x02001E6C RID: 7788
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnFallingLiquidInfo
	{
		// Token: 0x04008F16 RID: 36630
		public int cellIdx;

		// Token: 0x04008F17 RID: 36631
		public ushort elemIdx;

		// Token: 0x04008F18 RID: 36632
		public byte diseaseIdx;

		// Token: 0x04008F19 RID: 36633
		public byte pad0;

		// Token: 0x04008F1A RID: 36634
		public float mass;

		// Token: 0x04008F1B RID: 36635
		public float temperature;

		// Token: 0x04008F1C RID: 36636
		public int diseaseCount;
	}

	// Token: 0x02001E6D RID: 7789
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnOreInfo
	{
		// Token: 0x04008F1D RID: 36637
		public int cellIdx;

		// Token: 0x04008F1E RID: 36638
		public ushort elemIdx;

		// Token: 0x04008F1F RID: 36639
		public byte diseaseIdx;

		// Token: 0x04008F20 RID: 36640
		private byte pad0;

		// Token: 0x04008F21 RID: 36641
		public float mass;

		// Token: 0x04008F22 RID: 36642
		public float temperature;

		// Token: 0x04008F23 RID: 36643
		public int diseaseCount;
	}

	// Token: 0x02001E6E RID: 7790
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnFXInfo
	{
		// Token: 0x04008F24 RID: 36644
		public int cellIdx;

		// Token: 0x04008F25 RID: 36645
		public int fxHash;

		// Token: 0x04008F26 RID: 36646
		public float rotation;
	}

	// Token: 0x02001E6F RID: 7791
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct UnstableCellInfo
	{
		// Token: 0x04008F27 RID: 36647
		public int cellIdx;

		// Token: 0x04008F28 RID: 36648
		public ushort elemIdx;

		// Token: 0x04008F29 RID: 36649
		public byte fallingInfo;

		// Token: 0x04008F2A RID: 36650
		public byte diseaseIdx;

		// Token: 0x04008F2B RID: 36651
		public float mass;

		// Token: 0x04008F2C RID: 36652
		public float temperature;

		// Token: 0x04008F2D RID: 36653
		public int diseaseCount;

		// Token: 0x02002A6F RID: 10863
		public enum FallingInfo
		{
			// Token: 0x0400BB61 RID: 47969
			StartedFalling,
			// Token: 0x0400BB62 RID: 47970
			StoppedFalling
		}
	}

	// Token: 0x02001E70 RID: 7792
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct NewGameFrame
	{
		// Token: 0x04008F2E RID: 36654
		public float elapsedSeconds;

		// Token: 0x04008F2F RID: 36655
		public int minX;

		// Token: 0x04008F30 RID: 36656
		public int minY;

		// Token: 0x04008F31 RID: 36657
		public int maxX;

		// Token: 0x04008F32 RID: 36658
		public int maxY;

		// Token: 0x04008F33 RID: 36659
		public float currentSunlightIntensity;

		// Token: 0x04008F34 RID: 36660
		public float currentCosmicRadiationIntensity;
	}

	// Token: 0x02001E71 RID: 7793
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct WorldDamageInfo
	{
		// Token: 0x04008F35 RID: 36661
		public int gameCell;

		// Token: 0x04008F36 RID: 36662
		public int damageSourceOffset;
	}

	// Token: 0x02001E72 RID: 7794
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PipeTemperatureChange
	{
		// Token: 0x04008F37 RID: 36663
		public int cellIdx;

		// Token: 0x04008F38 RID: 36664
		public float temperature;
	}

	// Token: 0x02001E73 RID: 7795
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MassConsumedCallback
	{
		// Token: 0x04008F39 RID: 36665
		public int callbackIdx;

		// Token: 0x04008F3A RID: 36666
		public ushort elemIdx;

		// Token: 0x04008F3B RID: 36667
		public byte diseaseIdx;

		// Token: 0x04008F3C RID: 36668
		private byte pad0;

		// Token: 0x04008F3D RID: 36669
		public float mass;

		// Token: 0x04008F3E RID: 36670
		public float temperature;

		// Token: 0x04008F3F RID: 36671
		public int diseaseCount;
	}

	// Token: 0x02001E74 RID: 7796
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MassEmittedCallback
	{
		// Token: 0x04008F40 RID: 36672
		public int callbackIdx;

		// Token: 0x04008F41 RID: 36673
		public ushort elemIdx;

		// Token: 0x04008F42 RID: 36674
		public byte suceeded;

		// Token: 0x04008F43 RID: 36675
		public byte diseaseIdx;

		// Token: 0x04008F44 RID: 36676
		public float mass;

		// Token: 0x04008F45 RID: 36677
		public float temperature;

		// Token: 0x04008F46 RID: 36678
		public int diseaseCount;
	}

	// Token: 0x02001E75 RID: 7797
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseConsumptionCallback
	{
		// Token: 0x04008F47 RID: 36679
		public int callbackIdx;

		// Token: 0x04008F48 RID: 36680
		public byte diseaseIdx;

		// Token: 0x04008F49 RID: 36681
		private byte pad0;

		// Token: 0x04008F4A RID: 36682
		private byte pad1;

		// Token: 0x04008F4B RID: 36683
		private byte pad2;

		// Token: 0x04008F4C RID: 36684
		public int diseaseCount;
	}

	// Token: 0x02001E76 RID: 7798
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ComponentStateChangedMessage
	{
		// Token: 0x04008F4D RID: 36685
		public int callbackIdx;

		// Token: 0x04008F4E RID: 36686
		public int simHandle;
	}

	// Token: 0x02001E77 RID: 7799
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DebugProperties
	{
		// Token: 0x04008F4F RID: 36687
		public float buildingTemperatureScale;

		// Token: 0x04008F50 RID: 36688
		public float buildingToBuildingTemperatureScale;

		// Token: 0x04008F51 RID: 36689
		public float biomeTemperatureLerpRate;

		// Token: 0x04008F52 RID: 36690
		public byte isDebugEditing;

		// Token: 0x04008F53 RID: 36691
		public byte pad0;

		// Token: 0x04008F54 RID: 36692
		public byte pad1;

		// Token: 0x04008F55 RID: 36693
		public byte pad2;
	}

	// Token: 0x02001E78 RID: 7800
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct EmittedMassInfo
	{
		// Token: 0x04008F56 RID: 36694
		public ushort elemIdx;

		// Token: 0x04008F57 RID: 36695
		public byte diseaseIdx;

		// Token: 0x04008F58 RID: 36696
		public byte pad0;

		// Token: 0x04008F59 RID: 36697
		public float mass;

		// Token: 0x04008F5A RID: 36698
		public float temperature;

		// Token: 0x04008F5B RID: 36699
		public int diseaseCount;
	}

	// Token: 0x02001E79 RID: 7801
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedMassInfo
	{
		// Token: 0x04008F5C RID: 36700
		public int simHandle;

		// Token: 0x04008F5D RID: 36701
		public ushort removedElemIdx;

		// Token: 0x04008F5E RID: 36702
		public byte diseaseIdx;

		// Token: 0x04008F5F RID: 36703
		private byte pad0;

		// Token: 0x04008F60 RID: 36704
		public float mass;

		// Token: 0x04008F61 RID: 36705
		public float temperature;

		// Token: 0x04008F62 RID: 36706
		public int diseaseCount;
	}

	// Token: 0x02001E7A RID: 7802
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedDiseaseInfo
	{
		// Token: 0x04008F63 RID: 36707
		public int simHandle;

		// Token: 0x04008F64 RID: 36708
		public byte diseaseIdx;

		// Token: 0x04008F65 RID: 36709
		private byte pad0;

		// Token: 0x04008F66 RID: 36710
		private byte pad1;

		// Token: 0x04008F67 RID: 36711
		private byte pad2;

		// Token: 0x04008F68 RID: 36712
		public int diseaseCount;
	}

	// Token: 0x02001E7B RID: 7803
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ElementChunkInfo
	{
		// Token: 0x04008F69 RID: 36713
		public float temperature;

		// Token: 0x04008F6A RID: 36714
		public float deltaKJ;
	}

	// Token: 0x02001E7C RID: 7804
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MeltedInfo
	{
		// Token: 0x04008F6B RID: 36715
		public int handle;
	}

	// Token: 0x02001E7D RID: 7805
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CellMeltedInfo
	{
		// Token: 0x04008F6C RID: 36716
		public int gameCell;
	}

	// Token: 0x02001E7E RID: 7806
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct BuildingTemperatureInfo
	{
		// Token: 0x04008F6D RID: 36717
		public int handle;

		// Token: 0x04008F6E RID: 36718
		public float temperature;
	}

	// Token: 0x02001E7F RID: 7807
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct BuildingConductivityData
	{
		// Token: 0x04008F6F RID: 36719
		public float temperature;

		// Token: 0x04008F70 RID: 36720
		public float heatCapacity;

		// Token: 0x04008F71 RID: 36721
		public float thermalConductivity;
	}

	// Token: 0x02001E80 RID: 7808
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseEmittedInfo
	{
		// Token: 0x04008F72 RID: 36722
		public byte diseaseIdx;

		// Token: 0x04008F73 RID: 36723
		private byte pad0;

		// Token: 0x04008F74 RID: 36724
		private byte pad1;

		// Token: 0x04008F75 RID: 36725
		private byte pad2;

		// Token: 0x04008F76 RID: 36726
		public int count;
	}

	// Token: 0x02001E81 RID: 7809
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseConsumedInfo
	{
		// Token: 0x04008F77 RID: 36727
		public byte diseaseIdx;

		// Token: 0x04008F78 RID: 36728
		private byte pad0;

		// Token: 0x04008F79 RID: 36729
		private byte pad1;

		// Token: 0x04008F7A RID: 36730
		private byte pad2;

		// Token: 0x04008F7B RID: 36731
		public int count;
	}

	// Token: 0x02001E82 RID: 7810
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedRadiationCallback
	{
		// Token: 0x04008F7C RID: 36732
		public int callbackIdx;

		// Token: 0x04008F7D RID: 36733
		public int gameCell;

		// Token: 0x04008F7E RID: 36734
		public float radiation;
	}
}
