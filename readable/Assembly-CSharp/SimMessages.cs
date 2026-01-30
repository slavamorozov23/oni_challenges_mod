using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Database;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using STRINGS;

// Token: 0x02000C80 RID: 3200
public static class SimMessages
{
	// Token: 0x060061EB RID: 25067 RVA: 0x00242654 File Offset: 0x00240854
	public unsafe static void AddElementConsumer(int gameCell, ElementConsumer.Configuration configuration, SimHashes element, byte radius, int cb_handle)
	{
		Debug.Assert(Grid.IsValidCell(gameCell));
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.AddElementConsumerMessage* ptr = stackalloc SimMessages.AddElementConsumerMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementConsumerMessage))];
		ptr->cellIdx = gameCell;
		ptr->configuration = (byte)configuration;
		ptr->elementIdx = elementIndex;
		ptr->radius = radius;
		ptr->callbackIdx = cb_handle;
		Sim.SIM_HandleMessage(2024405073, sizeof(SimMessages.AddElementConsumerMessage), (byte*)ptr);
	}

	// Token: 0x060061EC RID: 25068 RVA: 0x002426C0 File Offset: 0x002408C0
	public unsafe static void SetElementConsumerData(int sim_handle, int cell, float consumptionRate)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			return;
		}
		SimMessages.SetElementConsumerDataMessage* ptr = stackalloc SimMessages.SetElementConsumerDataMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetElementConsumerDataMessage))];
		ptr->handle = sim_handle;
		ptr->cell = cell;
		ptr->consumptionRate = consumptionRate;
		Sim.SIM_HandleMessage(1575539738, sizeof(SimMessages.SetElementConsumerDataMessage), (byte*)ptr);
	}

	// Token: 0x060061ED RID: 25069 RVA: 0x0024270C File Offset: 0x0024090C
	public unsafe static void RemoveElementConsumer(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementConsumerMessage* ptr = stackalloc SimMessages.RemoveElementConsumerMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementConsumerMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(894417742, sizeof(SimMessages.RemoveElementConsumerMessage), (byte*)ptr);
	}

	// Token: 0x060061EE RID: 25070 RVA: 0x0024275C File Offset: 0x0024095C
	public unsafe static void AddElementEmitter(float max_pressure, int on_registered, int on_blocked = -1, int on_unblocked = -1)
	{
		SimMessages.AddElementEmitterMessage* ptr = stackalloc SimMessages.AddElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementEmitterMessage))];
		ptr->maxPressure = max_pressure;
		ptr->callbackIdx = on_registered;
		ptr->onBlockedCB = on_blocked;
		ptr->onUnblockedCB = on_unblocked;
		Sim.SIM_HandleMessage(-505471181, sizeof(SimMessages.AddElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x060061EF RID: 25071 RVA: 0x002427A4 File Offset: 0x002409A4
	public unsafe static void ModifyElementEmitter(int sim_handle, int game_cell, int max_depth, SimHashes element, float emit_interval, float emit_mass, float emit_temperature, float max_pressure, byte disease_idx, int disease_count)
	{
		Debug.Assert(Grid.IsValidCell(game_cell));
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.ModifyElementEmitterMessage* ptr = stackalloc SimMessages.ModifyElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->cellIdx = game_cell;
		ptr->emitInterval = emit_interval;
		ptr->emitMass = emit_mass;
		ptr->emitTemperature = emit_temperature;
		ptr->maxPressure = max_pressure;
		ptr->elementIdx = elementIndex;
		ptr->maxDepth = (byte)max_depth;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		Sim.SIM_HandleMessage(403589164, sizeof(SimMessages.ModifyElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x060061F0 RID: 25072 RVA: 0x00242838 File Offset: 0x00240A38
	public unsafe static void RemoveElementEmitter(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementEmitterMessage* ptr = stackalloc SimMessages.RemoveElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementEmitterMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-1524118282, sizeof(SimMessages.RemoveElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x060061F1 RID: 25073 RVA: 0x00242888 File Offset: 0x00240A88
	public unsafe static void AddRadiationEmitter(int on_registered, int game_cell, short emitRadiusX, short emitRadiusY, float emitRads, float emitRate, float emitSpeed, float emitDirection, float emitAngle, RadiationEmitter.RadiationEmitterType emitType)
	{
		SimMessages.AddRadiationEmitterMessage* ptr = stackalloc SimMessages.AddRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddRadiationEmitterMessage))];
		ptr->callbackIdx = on_registered;
		ptr->cell = game_cell;
		ptr->emitRadiusX = emitRadiusX;
		ptr->emitRadiusY = emitRadiusY;
		ptr->emitRads = emitRads;
		ptr->emitRate = emitRate;
		ptr->emitSpeed = emitSpeed;
		ptr->emitDirection = emitDirection;
		ptr->emitAngle = emitAngle;
		ptr->emitType = (int)emitType;
		Sim.SIM_HandleMessage(-1505895314, sizeof(SimMessages.AddRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x060061F2 RID: 25074 RVA: 0x00242900 File Offset: 0x00240B00
	public unsafe static void ModifyRadiationEmitter(int sim_handle, int game_cell, short emitRadiusX, short emitRadiusY, float emitRads, float emitRate, float emitSpeed, float emitDirection, float emitAngle, RadiationEmitter.RadiationEmitterType emitType)
	{
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		SimMessages.ModifyRadiationEmitterMessage* ptr = stackalloc SimMessages.ModifyRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyRadiationEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->cell = game_cell;
		ptr->callbackIdx = -1;
		ptr->emitRadiusX = emitRadiusX;
		ptr->emitRadiusY = emitRadiusY;
		ptr->emitRads = emitRads;
		ptr->emitRate = emitRate;
		ptr->emitSpeed = emitSpeed;
		ptr->emitDirection = emitDirection;
		ptr->emitAngle = emitAngle;
		ptr->emitType = (int)emitType;
		Sim.SIM_HandleMessage(-503965465, sizeof(SimMessages.ModifyRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x060061F3 RID: 25075 RVA: 0x00242988 File Offset: 0x00240B88
	public unsafe static void RemoveRadiationEmitter(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveRadiationEmitterMessage* ptr = stackalloc SimMessages.RemoveRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveRadiationEmitterMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-704259919, sizeof(SimMessages.RemoveRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x060061F4 RID: 25076 RVA: 0x002429D8 File Offset: 0x00240BD8
	public unsafe static void AddElementChunk(int gameCell, SimHashes element, float mass, float temperature, float surface_area, float thickness, float ground_transfer_scale, int cb_handle)
	{
		Debug.Assert(Grid.IsValidCell(gameCell));
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		if (mass * temperature > 0f)
		{
			ushort elementIndex = ElementLoader.GetElementIndex(element);
			SimMessages.AddElementChunkMessage* ptr = stackalloc SimMessages.AddElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementChunkMessage))];
			ptr->gameCell = gameCell;
			ptr->callbackIdx = cb_handle;
			ptr->mass = mass;
			ptr->temperature = temperature;
			ptr->surfaceArea = surface_area;
			ptr->thickness = thickness;
			ptr->groundTransferScale = ground_transfer_scale;
			ptr->elementIdx = elementIndex;
			Sim.SIM_HandleMessage(1445724082, sizeof(SimMessages.AddElementChunkMessage), (byte*)ptr);
		}
	}

	// Token: 0x060061F5 RID: 25077 RVA: 0x00242A64 File Offset: 0x00240C64
	public unsafe static void RemoveElementChunk(int sim_handle, int cb_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementChunkMessage* ptr = stackalloc SimMessages.RemoveElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementChunkMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-912908555, sizeof(SimMessages.RemoveElementChunkMessage), (byte*)ptr);
	}

	// Token: 0x060061F6 RID: 25078 RVA: 0x00242AB4 File Offset: 0x00240CB4
	public unsafe static void SetElementChunkData(int sim_handle, float temperature, float heat_capacity)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			return;
		}
		SimMessages.SetElementChunkDataMessage* ptr = stackalloc SimMessages.SetElementChunkDataMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetElementChunkDataMessage))];
		ptr->handle = sim_handle;
		ptr->temperature = temperature;
		ptr->heatCapacity = heat_capacity;
		Sim.SIM_HandleMessage(-435115907, sizeof(SimMessages.SetElementChunkDataMessage), (byte*)ptr);
	}

	// Token: 0x060061F7 RID: 25079 RVA: 0x00242B00 File Offset: 0x00240D00
	public unsafe static void MoveElementChunk(int sim_handle, int cell)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.MoveElementChunkMessage* ptr = stackalloc SimMessages.MoveElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MoveElementChunkMessage))];
		ptr->handle = sim_handle;
		ptr->gameCell = cell;
		Sim.SIM_HandleMessage(-374911358, sizeof(SimMessages.MoveElementChunkMessage), (byte*)ptr);
	}

	// Token: 0x060061F8 RID: 25080 RVA: 0x00242B50 File Offset: 0x00240D50
	public unsafe static void ModifyElementChunkEnergy(int sim_handle, float delta_kj)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.ModifyElementChunkEnergyMessage* ptr = stackalloc SimMessages.ModifyElementChunkEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementChunkEnergyMessage))];
		ptr->handle = sim_handle;
		ptr->deltaKJ = delta_kj;
		Sim.SIM_HandleMessage(1020555667, sizeof(SimMessages.ModifyElementChunkEnergyMessage), (byte*)ptr);
	}

	// Token: 0x060061F9 RID: 25081 RVA: 0x00242BA0 File Offset: 0x00240DA0
	public unsafe static void ModifyElementChunkTemperatureAdjuster(int sim_handle, float temperature, float heat_capacity, float thermal_conductivity)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.ModifyElementChunkAdjusterMessage* ptr = stackalloc SimMessages.ModifyElementChunkAdjusterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementChunkAdjusterMessage))];
		ptr->handle = sim_handle;
		ptr->temperature = temperature;
		ptr->heatCapacity = heat_capacity;
		ptr->thermalConductivity = thermal_conductivity;
		Sim.SIM_HandleMessage(-1387601379, sizeof(SimMessages.ModifyElementChunkAdjusterMessage), (byte*)ptr);
	}

	// Token: 0x060061FA RID: 25082 RVA: 0x00242BFC File Offset: 0x00240DFC
	public unsafe static void AddBuildingHeatExchange(Extents extents, float mass, float temperature, float thermal_conductivity, float operating_kw, ushort elem_idx, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(Grid.XYToCell(extents.x, extents.y)))
		{
			return;
		}
		int num = Grid.XYToCell(extents.x + extents.width, extents.y + extents.height);
		if (!Grid.IsValidCell(num))
		{
			Debug.LogErrorFormat("Invalid Cell [{0}] Extents [{1},{2}] [{3},{4}]", new object[]
			{
				num,
				extents.x,
				extents.y,
				extents.width,
				extents.height
			});
		}
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		SimMessages.AddBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.AddBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddBuildingHeatExchangeMessage))];
		ptr->callbackIdx = callbackIdx;
		ptr->elemIdx = elem_idx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->thermalConductivity = thermal_conductivity;
		ptr->overheatTemperature = float.MaxValue;
		ptr->operatingKilowatts = operating_kw;
		ptr->minX = extents.x;
		ptr->minY = extents.y;
		ptr->maxX = extents.x + extents.width;
		ptr->maxY = extents.y + extents.height;
		Sim.SIM_HandleMessage(1739021608, sizeof(SimMessages.AddBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x060061FB RID: 25083 RVA: 0x00242D38 File Offset: 0x00240F38
	public unsafe static void ModifyBuildingHeatExchange(int sim_handle, Extents extents, float mass, float temperature, float thermal_conductivity, float overheat_temperature, float operating_kw, ushort element_idx)
	{
		int cell = Grid.XYToCell(extents.x, extents.y);
		Debug.Assert(Grid.IsValidCell(cell));
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		int cell2 = Grid.XYToCell(extents.x + extents.width, extents.y + extents.height);
		Debug.Assert(Grid.IsValidCell(cell2));
		if (!Grid.IsValidCell(cell2))
		{
			return;
		}
		SimMessages.ModifyBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.ModifyBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyBuildingHeatExchangeMessage))];
		ptr->callbackIdx = sim_handle;
		ptr->elemIdx = element_idx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->thermalConductivity = thermal_conductivity;
		ptr->overheatTemperature = overheat_temperature;
		ptr->operatingKilowatts = operating_kw;
		ptr->minX = extents.x;
		ptr->minY = extents.y;
		ptr->maxX = extents.x + extents.width;
		ptr->maxY = extents.y + extents.height;
		Sim.SIM_HandleMessage(1818001569, sizeof(SimMessages.ModifyBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x060061FC RID: 25084 RVA: 0x00242E2C File Offset: 0x0024102C
	public unsafe static void RemoveBuildingHeatExchange(int sim_handle, int callbackIdx = -1)
	{
		SimMessages.RemoveBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingHeatExchangeMessage))];
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		ptr->handle = sim_handle;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-456116629, sizeof(SimMessages.RemoveBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x060061FD RID: 25085 RVA: 0x00242E70 File Offset: 0x00241070
	public unsafe static void ModifyBuildingEnergy(int sim_handle, float delta_kj, float min_temperature, float max_temperature)
	{
		SimMessages.ModifyBuildingEnergyMessage* ptr = stackalloc SimMessages.ModifyBuildingEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyBuildingEnergyMessage))];
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		ptr->handle = sim_handle;
		ptr->deltaKJ = delta_kj;
		ptr->minTemperature = min_temperature;
		ptr->maxTemperature = max_temperature;
		Sim.SIM_HandleMessage(-1348791658, sizeof(SimMessages.ModifyBuildingEnergyMessage), (byte*)ptr);
	}

	// Token: 0x060061FE RID: 25086 RVA: 0x00242EC4 File Offset: 0x002410C4
	public unsafe static void RegisterBuildingToBuildingHeatExchange(int structureTemperatureHandler, int callbackIdx = -1)
	{
		SimMessages.RegisterBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RegisterBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RegisterBuildingToBuildingHeatExchangeMessage))];
		ptr->structureTemperatureHandler = structureTemperatureHandler;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-1338718217, sizeof(SimMessages.RegisterBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x060061FF RID: 25087 RVA: 0x00242F00 File Offset: 0x00241100
	public unsafe static void AddBuildingToBuildingHeatExchange(int selfHandler, int buildingInContact, int cellsInContact)
	{
		SimMessages.AddBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.AddBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddBuildingToBuildingHeatExchangeMessage))];
		ptr->selfHandler = selfHandler;
		ptr->buildingInContactHandle = buildingInContact;
		ptr->cellsInContact = cellsInContact;
		Sim.SIM_HandleMessage(-1586724321, sizeof(SimMessages.AddBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06006200 RID: 25088 RVA: 0x00242F40 File Offset: 0x00241140
	public unsafe static void RemoveBuildingInContactFromBuildingToBuildingHeatExchange(int selfHandler, int buildingToRemove)
	{
		SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage))];
		ptr->selfHandler = selfHandler;
		ptr->buildingNoLongerInContactHandler = buildingToRemove;
		Sim.SIM_HandleMessage(-1993857213, sizeof(SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06006201 RID: 25089 RVA: 0x00242F7C File Offset: 0x0024117C
	public unsafe static void RemoveBuildingToBuildingHeatExchange(int selfHandler, int callback = -1)
	{
		SimMessages.RemoveBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingToBuildingHeatExchangeMessage))];
		ptr->callbackIdx = callback;
		ptr->selfHandler = selfHandler;
		Sim.SIM_HandleMessage(697100730, sizeof(SimMessages.RemoveBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06006202 RID: 25090 RVA: 0x00242FB8 File Offset: 0x002411B8
	public unsafe static void AddDiseaseEmitter(int callbackIdx)
	{
		SimMessages.AddDiseaseEmitterMessage* ptr = stackalloc SimMessages.AddDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddDiseaseEmitterMessage))];
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(1486783027, sizeof(SimMessages.AddDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06006203 RID: 25091 RVA: 0x00242FEC File Offset: 0x002411EC
	public unsafe static void ModifyDiseaseEmitter(int sim_handle, int cell, byte range, byte disease_idx, float emit_interval, int emit_count)
	{
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.ModifyDiseaseEmitterMessage* ptr = stackalloc SimMessages.ModifyDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyDiseaseEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->gameCell = cell;
		ptr->maxDepth = range;
		ptr->diseaseIdx = disease_idx;
		ptr->emitInterval = emit_interval;
		ptr->emitCount = emit_count;
		Sim.SIM_HandleMessage(-1899123924, sizeof(SimMessages.ModifyDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06006204 RID: 25092 RVA: 0x00243050 File Offset: 0x00241250
	public unsafe static void RemoveDiseaseEmitter(int cb_handle, int sim_handle)
	{
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveDiseaseEmitterMessage* ptr = stackalloc SimMessages.RemoveDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveDiseaseEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->callbackIdx = cb_handle;
		Sim.SIM_HandleMessage(468135926, sizeof(SimMessages.RemoveDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06006205 RID: 25093 RVA: 0x00243094 File Offset: 0x00241294
	public unsafe static void SetSavedOptionValue(SimMessages.SimSavedOptions option, int zero_or_one)
	{
		SimMessages.SetSavedOptionsMessage* ptr = stackalloc SimMessages.SetSavedOptionsMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetSavedOptionsMessage))];
		if (zero_or_one == 0)
		{
			SimMessages.SetSavedOptionsMessage* ptr2 = ptr;
			ptr2->clearBits = (ptr2->clearBits | (byte)option);
			ptr->setBits = 0;
		}
		else
		{
			ptr->clearBits = 0;
			SimMessages.SetSavedOptionsMessage* ptr3 = ptr;
			ptr3->setBits = (ptr3->setBits | (byte)option);
		}
		Sim.SIM_HandleMessage(1154135737, sizeof(SimMessages.SetSavedOptionsMessage), (byte*)ptr);
	}

	// Token: 0x06006206 RID: 25094 RVA: 0x002430EC File Offset: 0x002412EC
	private static void WriteKleiString(this BinaryWriter writer, string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		writer.Write(bytes.Length);
		if (bytes.Length != 0)
		{
			writer.Write(bytes);
		}
	}

	// Token: 0x06006207 RID: 25095 RVA: 0x0024311C File Offset: 0x0024131C
	public unsafe static void CreateSimElementsTable(List<Element> elements)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(Sim.Element)) * elements.Count);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		Debug.Assert(elements.Count < 65535, "SimDLL internals assume there are fewer than 65535 elements");
		binaryWriter.Write(elements.Count);
		for (int i = 0; i < elements.Count; i++)
		{
			Sim.Element element = new Sim.Element(elements[i], elements);
			element.Write(binaryWriter);
		}
		for (int j = 0; j < elements.Count; j++)
		{
			binaryWriter.WriteKleiString(UI.StripLinkFormatting(elements[j].name));
		}
		byte[] buffer = memoryStream.GetBuffer();
		byte[] array;
		byte* msg;
		if ((array = buffer) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(1108437482, buffer.Length, msg);
		array = null;
	}

	// Token: 0x06006208 RID: 25096 RVA: 0x0024320C File Offset: 0x0024140C
	public unsafe static void CreateDiseaseTable(Diseases diseases)
	{
		MemoryStream memoryStream = new MemoryStream(1024);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(diseases.Count);
		List<Element> elements = ElementLoader.elements;
		binaryWriter.Write(elements.Count);
		for (int i = 0; i < diseases.Count; i++)
		{
			Disease disease = diseases[i];
			binaryWriter.WriteKleiString(UI.StripLinkFormatting(disease.Name));
			binaryWriter.Write(disease.id.GetHashCode());
			binaryWriter.Write(disease.strength);
			disease.temperatureRange.Write(binaryWriter);
			disease.temperatureHalfLives.Write(binaryWriter);
			disease.pressureRange.Write(binaryWriter);
			disease.pressureHalfLives.Write(binaryWriter);
			binaryWriter.Write(disease.radiationKillRate);
			for (int j = 0; j < elements.Count; j++)
			{
				ElemGrowthInfo elemGrowthInfo = disease.elemGrowthInfo[j];
				elemGrowthInfo.Write(binaryWriter);
			}
		}
		byte[] array;
		byte* msg;
		if ((array = memoryStream.GetBuffer()) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(825301935, (int)memoryStream.Length, msg);
		array = null;
	}

	// Token: 0x06006209 RID: 25097 RVA: 0x00243348 File Offset: 0x00241548
	public unsafe static void DefineWorldOffsets(List<SimMessages.WorldOffsetData> worldOffsets)
	{
		MemoryStream memoryStream = new MemoryStream(1024);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(worldOffsets.Count);
		foreach (SimMessages.WorldOffsetData worldOffsetData in worldOffsets)
		{
			binaryWriter.Write(worldOffsetData.worldOffsetX);
			binaryWriter.Write(worldOffsetData.worldOffsetY);
			binaryWriter.Write(worldOffsetData.worldSizeX);
			binaryWriter.Write(worldOffsetData.worldSizeY);
		}
		byte[] array;
		byte* msg;
		if ((array = memoryStream.GetBuffer()) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(-895846551, (int)memoryStream.Length, msg);
		array = null;
	}

	// Token: 0x0600620A RID: 25098 RVA: 0x00243418 File Offset: 0x00241618
	public static void SimDataInitializeFromCells(int width, int height, uint simSeed, Sim.Cell[] cells, float[] bgTemp, Sim.DiseaseCell[] dc, bool headless)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(uint)) + Marshal.SizeOf(typeof(bool)) + Marshal.SizeOf(typeof(bool)) + Marshal.SizeOf(typeof(Sim.Cell)) * width * height + Marshal.SizeOf(typeof(float)) * width * height + Marshal.SizeOf(typeof(Sim.DiseaseCell)) * width * height);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		binaryWriter.Write(simSeed);
		bool value = Sim.IsRadiationEnabled();
		binaryWriter.Write(value);
		binaryWriter.Write(headless);
		int num = width * height;
		for (int i = 0; i < num; i++)
		{
			cells[i].Write(binaryWriter);
		}
		for (int j = 0; j < num; j++)
		{
			binaryWriter.Write(bgTemp[j]);
		}
		for (int k = 0; k < num; k++)
		{
			dc[k].Write(binaryWriter);
		}
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_InitializeFromCells, buffer.Length, buffer);
	}

	// Token: 0x0600620B RID: 25099 RVA: 0x0024355C File Offset: 0x0024175C
	public static void SimDataResizeGridAndInitializeVacuumCells(Vector2I grid_size, int width, int height, int x_offset, int y_offset)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)));
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(grid_size.x);
		binaryWriter.Write(grid_size.y);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		binaryWriter.Write(x_offset);
		binaryWriter.Write(y_offset);
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_ResizeAndInitializeVacuumCells, buffer.Length, buffer);
	}

	// Token: 0x0600620C RID: 25100 RVA: 0x002435DC File Offset: 0x002417DC
	public static void SimDataFreeCells(int width, int height, int x_offset, int y_offset)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)));
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		binaryWriter.Write(x_offset);
		binaryWriter.Write(y_offset);
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_FreeCells, buffer.Length, buffer);
	}

	// Token: 0x0600620D RID: 25101 RVA: 0x00243644 File Offset: 0x00241844
	public unsafe static void Dig(int gameCell, int callbackIdx = -1, bool skipEvent = false)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.DigMessage* ptr = stackalloc SimMessages.DigMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.DigMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->skipEvent = skipEvent;
		Sim.SIM_HandleMessage(833038498, sizeof(SimMessages.DigMessage), (byte*)ptr);
	}

	// Token: 0x0600620E RID: 25102 RVA: 0x00243690 File Offset: 0x00241890
	public unsafe static void SetInsulation(int gameCell, float value)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.SetCellFloatValueMessage* ptr = stackalloc SimMessages.SetCellFloatValueMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetCellFloatValueMessage))];
		ptr->cellIdx = gameCell;
		ptr->value = value;
		Sim.SIM_HandleMessage(-898773121, sizeof(SimMessages.SetCellFloatValueMessage), (byte*)ptr);
	}

	// Token: 0x0600620F RID: 25103 RVA: 0x002436D4 File Offset: 0x002418D4
	public unsafe static void SetStrength(int gameCell, int weight, float strengthMultiplier)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.SetCellFloatValueMessage* ptr = stackalloc SimMessages.SetCellFloatValueMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetCellFloatValueMessage))];
		ptr->cellIdx = gameCell;
		int num = (int)(strengthMultiplier * 4f) & 127;
		int num2 = (weight & 1) << 7 | num;
		ptr->value = (float)((byte)num2);
		Sim.SIM_HandleMessage(1593243982, sizeof(SimMessages.SetCellFloatValueMessage), (byte*)ptr);
	}

	// Token: 0x06006210 RID: 25104 RVA: 0x0024372C File Offset: 0x0024192C
	public unsafe static void SetCellProperties(int gameCell, byte properties)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.CellPropertiesMessage* ptr = stackalloc SimMessages.CellPropertiesMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellPropertiesMessage))];
		ptr->cellIdx = gameCell;
		ptr->properties = properties;
		ptr->set = 1;
		Sim.SIM_HandleMessage(-469311643, sizeof(SimMessages.CellPropertiesMessage), (byte*)ptr);
	}

	// Token: 0x06006211 RID: 25105 RVA: 0x00243778 File Offset: 0x00241978
	public unsafe static void ClearCellProperties(int gameCell, byte properties)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.CellPropertiesMessage* ptr = stackalloc SimMessages.CellPropertiesMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellPropertiesMessage))];
		ptr->cellIdx = gameCell;
		ptr->properties = properties;
		ptr->set = 0;
		Sim.SIM_HandleMessage(-469311643, sizeof(SimMessages.CellPropertiesMessage), (byte*)ptr);
	}

	// Token: 0x06006212 RID: 25106 RVA: 0x002437C4 File Offset: 0x002419C4
	public unsafe static void ModifyCell(int gameCell, ushort elementIdx, float temperature, float mass, byte disease_idx, int disease_count, SimMessages.ReplaceType replace_type = SimMessages.ReplaceType.None, bool do_vertical_solid_displacement = false, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		Element element = ElementLoader.elements[(int)elementIdx];
		if (element.maxMass == 0f && mass > element.maxMass)
		{
			Debug.LogWarningFormat("Invalid cell modification (mass greater than element maximum): Cell={0}, EIdx={1}, T={2}, M={3}, {4} max mass = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.maxMass
			});
			mass = element.maxMass;
		}
		if (temperature < 0f || temperature > 10000f)
		{
			Debug.LogWarningFormat("Invalid cell modification (temp out of bounds): Cell={0}, EIdx={1}, T={2}, M={3}, {4} default temp = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.defaultValues.temperature
			});
			temperature = element.defaultValues.temperature;
		}
		if (temperature == 0f && mass > 0f)
		{
			Debug.LogWarningFormat("Invalid cell modification (zero temp with non-zero mass): Cell={0}, EIdx={1}, T={2}, M={3}, {4} default temp = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.defaultValues.temperature
			});
			temperature = element.defaultValues.temperature;
		}
		SimMessages.ModifyCellMessage* ptr = stackalloc SimMessages.ModifyCellMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyCellMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->temperature = temperature;
		ptr->mass = mass;
		ptr->elementIdx = elementIdx;
		ptr->replaceType = (byte)replace_type;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		ptr->addSubType = (do_vertical_solid_displacement ? 0 : 1);
		Sim.SIM_HandleMessage(-1252920804, sizeof(SimMessages.ModifyCellMessage), (byte*)ptr);
	}

	// Token: 0x06006213 RID: 25107 RVA: 0x002439A4 File Offset: 0x00241BA4
	public unsafe static void ModifyDiseaseOnCell(int gameCell, byte disease_idx, int disease_delta)
	{
		SimMessages.CellDiseaseModification* ptr = stackalloc SimMessages.CellDiseaseModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellDiseaseModification))];
		ptr->cellIdx = gameCell;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_delta;
		Sim.SIM_HandleMessage(-1853671274, sizeof(SimMessages.CellDiseaseModification), (byte*)ptr);
	}

	// Token: 0x06006214 RID: 25108 RVA: 0x002439E4 File Offset: 0x00241BE4
	public unsafe static void ModifyRadiationOnCell(int gameCell, float radiationDelta, int callbackIdx = -1)
	{
		SimMessages.CellRadiationModification* ptr = stackalloc SimMessages.CellRadiationModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellRadiationModification))];
		ptr->cellIdx = gameCell;
		ptr->radiationDelta = radiationDelta;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-1914877797, sizeof(SimMessages.CellRadiationModification), (byte*)ptr);
	}

	// Token: 0x06006215 RID: 25109 RVA: 0x00243A24 File Offset: 0x00241C24
	public unsafe static void ModifyRadiationParams(RadiationParams type, float value)
	{
		SimMessages.RadiationParamsModification* ptr = stackalloc SimMessages.RadiationParamsModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RadiationParamsModification))];
		ptr->RadiationParamsType = (int)type;
		ptr->value = value;
		Sim.SIM_HandleMessage(377112707, sizeof(SimMessages.RadiationParamsModification), (byte*)ptr);
	}

	// Token: 0x06006216 RID: 25110 RVA: 0x00243A5D File Offset: 0x00241C5D
	public static ushort GetElementIndex(SimHashes element)
	{
		return ElementLoader.GetElementIndex(element);
	}

	// Token: 0x06006217 RID: 25111 RVA: 0x00243A68 File Offset: 0x00241C68
	public unsafe static void ConsumeMass(int gameCell, SimHashes element, float mass, byte radius, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.MassConsumptionMessage* ptr = stackalloc SimMessages.MassConsumptionMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MassConsumptionMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->mass = mass;
		ptr->elementIdx = elementIndex;
		ptr->radius = radius;
		Sim.SIM_HandleMessage(1727657959, sizeof(SimMessages.MassConsumptionMessage), (byte*)ptr);
	}

	// Token: 0x06006218 RID: 25112 RVA: 0x00243AC8 File Offset: 0x00241CC8
	public unsafe static void EmitMass(int gameCell, ushort element_idx, float mass, float temperature, byte disease_idx, int disease_count, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.MassEmissionMessage* ptr = stackalloc SimMessages.MassEmissionMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MassEmissionMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->elementIdx = element_idx;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		Sim.SIM_HandleMessage(797274363, sizeof(SimMessages.MassEmissionMessage), (byte*)ptr);
	}

	// Token: 0x06006219 RID: 25113 RVA: 0x00243B30 File Offset: 0x00241D30
	public unsafe static void ConsumeDisease(int game_cell, float percent_to_consume, int max_to_consume, int callback_idx)
	{
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		SimMessages.ConsumeDiseaseMessage* ptr = stackalloc SimMessages.ConsumeDiseaseMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ConsumeDiseaseMessage))];
		ptr->callbackIdx = callback_idx;
		ptr->gameCell = game_cell;
		ptr->percentToConsume = percent_to_consume;
		ptr->maxToConsume = max_to_consume;
		Sim.SIM_HandleMessage(-1019841536, sizeof(SimMessages.ConsumeDiseaseMessage), (byte*)ptr);
	}

	// Token: 0x0600621A RID: 25114 RVA: 0x00243B80 File Offset: 0x00241D80
	public static void AddRemoveSubstance(int gameCell, SimHashes new_element, CellAddRemoveSubstanceEvent ev, float mass, float temperature, byte disease_idx, int disease_count, bool do_vertical_solid_displacement = true, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		SimMessages.AddRemoveSubstance(gameCell, elementIndex, ev, mass, temperature, disease_idx, disease_count, do_vertical_solid_displacement, callbackIdx);
	}

	// Token: 0x0600621B RID: 25115 RVA: 0x00243BA8 File Offset: 0x00241DA8
	public static void AddRemoveSubstance(int gameCell, ushort elementIdx, CellAddRemoveSubstanceEvent ev, float mass, float temperature, byte disease_idx, int disease_count, bool do_vertical_solid_displacement = true, int callbackIdx = -1)
	{
		if (elementIdx == 65535)
		{
			return;
		}
		Element element = ElementLoader.elements[(int)elementIdx];
		float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
		SimMessages.ModifyCell(gameCell, elementIdx, temperature2, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, do_vertical_solid_displacement, callbackIdx);
	}

	// Token: 0x0600621C RID: 25116 RVA: 0x00243BF8 File Offset: 0x00241DF8
	public static void ReplaceElement(int gameCell, SimHashes new_element, CellElementEvent ev, float mass, float temperature = -1f, byte diseaseIdx = 255, int diseaseCount = 0, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		if (elementIndex != 65535)
		{
			Element element = ElementLoader.elements[(int)elementIndex];
			float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
			SimMessages.ModifyCell(gameCell, elementIndex, temperature2, mass, diseaseIdx, diseaseCount, SimMessages.ReplaceType.Replace, false, callbackIdx);
		}
	}

	// Token: 0x0600621D RID: 25117 RVA: 0x00243C4C File Offset: 0x00241E4C
	public static void ReplaceAndDisplaceElement(int gameCell, SimHashes new_element, CellElementEvent ev, float mass, float temperature = -1f, byte disease_idx = 255, int disease_count = 0, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		if (elementIndex != 65535)
		{
			Element element = ElementLoader.elements[(int)elementIndex];
			float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
			SimMessages.ModifyCell(gameCell, elementIndex, temperature2, mass, disease_idx, disease_count, SimMessages.ReplaceType.ReplaceAndDisplace, false, callbackIdx);
		}
	}

	// Token: 0x0600621E RID: 25118 RVA: 0x00243CA0 File Offset: 0x00241EA0
	public unsafe static void ModifyEnergy(int gameCell, float kilojoules, float max_temperature, SimMessages.EnergySourceID id)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		if (max_temperature <= 0f)
		{
			Debug.LogError("invalid max temperature for cell energy modification");
			return;
		}
		SimMessages.ModifyCellEnergyMessage* ptr = stackalloc SimMessages.ModifyCellEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyCellEnergyMessage))];
		ptr->cellIdx = gameCell;
		ptr->kilojoules = kilojoules;
		ptr->maxTemperature = max_temperature;
		ptr->id = (int)id;
		Sim.SIM_HandleMessage(818320644, sizeof(SimMessages.ModifyCellEnergyMessage), (byte*)ptr);
	}

	// Token: 0x0600621F RID: 25119 RVA: 0x00243D04 File Offset: 0x00241F04
	public static void ModifyMass(int gameCell, float mass, byte disease_idx, int disease_count, CellModifyMassEvent ev, float temperature = -1f, SimHashes element = SimHashes.Vacuum)
	{
		if (element != SimHashes.Vacuum)
		{
			ushort elementIndex = SimMessages.GetElementIndex(element);
			if (elementIndex != 65535)
			{
				if (temperature == -1f)
				{
					temperature = ElementLoader.elements[(int)elementIndex].defaultValues.temperature;
				}
				SimMessages.ModifyCell(gameCell, elementIndex, temperature, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, false, -1);
				return;
			}
		}
		else
		{
			SimMessages.ModifyCell(gameCell, 0, temperature, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, false, -1);
		}
	}

	// Token: 0x06006220 RID: 25120 RVA: 0x00243D6C File Offset: 0x00241F6C
	public unsafe static void CreateElementInteractions(SimMessages.ElementInteraction[] interactions)
	{
		fixed (SimMessages.ElementInteraction[] array = interactions)
		{
			SimMessages.ElementInteraction* interactions2;
			if (interactions == null || array.Length == 0)
			{
				interactions2 = null;
			}
			else
			{
				interactions2 = &array[0];
			}
			SimMessages.CreateElementInteractionsMsg* ptr = stackalloc SimMessages.CreateElementInteractionsMsg[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CreateElementInteractionsMsg))];
			ptr->numInteractions = interactions.Length;
			ptr->interactions = interactions2;
			Sim.SIM_HandleMessage(-930289787, sizeof(SimMessages.CreateElementInteractionsMsg), (byte*)ptr);
		}
	}

	// Token: 0x06006221 RID: 25121 RVA: 0x00243DC4 File Offset: 0x00241FC4
	public unsafe static void NewGameFrame(float elapsed_seconds, List<Game.SimActiveRegion> activeRegions)
	{
		Debug.Assert(activeRegions.Count > 0, "NewGameFrame cannot be called with zero activeRegions");
		Sim.NewGameFrame* ptr = stackalloc Sim.NewGameFrame[checked(unchecked((UIntPtr)activeRegions.Count) * (UIntPtr)sizeof(Sim.NewGameFrame))];
		Sim.NewGameFrame* ptr2 = ptr;
		foreach (Game.SimActiveRegion simActiveRegion in activeRegions)
		{
			Pair<Vector2I, Vector2I> region = simActiveRegion.region;
			region.first = new Vector2I(MathUtil.Clamp(0, Grid.WidthInCells - 1, simActiveRegion.region.first.x), MathUtil.Clamp(0, Grid.HeightInCells - 1, simActiveRegion.region.first.y));
			region.second = new Vector2I(MathUtil.Clamp(0, Grid.WidthInCells, simActiveRegion.region.second.x), MathUtil.Clamp(0, Grid.HeightInCells - 1, simActiveRegion.region.second.y));
			ptr2->elapsedSeconds = elapsed_seconds;
			ptr2->minX = region.first.x;
			ptr2->minY = region.first.y;
			ptr2->maxX = region.second.x;
			ptr2->maxY = region.second.y;
			ptr2->currentSunlightIntensity = simActiveRegion.currentSunlightIntensity;
			ptr2->currentCosmicRadiationIntensity = simActiveRegion.currentCosmicRadiationIntensity;
			ptr2++;
		}
		Sim.SIM_HandleMessage(-775326397, sizeof(Sim.NewGameFrame) * activeRegions.Count, (byte*)ptr);
	}

	// Token: 0x06006222 RID: 25122 RVA: 0x00243F60 File Offset: 0x00242160
	public unsafe static void SetDebugProperties(Sim.DebugProperties properties)
	{
		Sim.DebugProperties* ptr = stackalloc Sim.DebugProperties[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(Sim.DebugProperties))];
		*ptr = properties;
		ptr->buildingTemperatureScale = properties.buildingTemperatureScale;
		ptr->buildingToBuildingTemperatureScale = properties.buildingToBuildingTemperatureScale;
		Sim.SIM_HandleMessage(-1683118492, sizeof(Sim.DebugProperties), (byte*)ptr);
	}

	// Token: 0x06006223 RID: 25123 RVA: 0x00243FAC File Offset: 0x002421AC
	public unsafe static void ModifyCellWorldZone(int cell, byte zone_id)
	{
		SimMessages.CellWorldZoneModification* ptr = stackalloc SimMessages.CellWorldZoneModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellWorldZoneModification))];
		ptr->cell = cell;
		ptr->zoneID = zone_id;
		Sim.SIM_HandleMessage(-449718014, sizeof(SimMessages.CellWorldZoneModification), (byte*)ptr);
	}

	// Token: 0x040041A6 RID: 16806
	public const int InvalidCallback = -1;

	// Token: 0x040041A7 RID: 16807
	public const float STATE_TRANSITION_TEMPERATURE_BUFER = 3f;

	// Token: 0x02001E83 RID: 7811
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementConsumerMessage
	{
		// Token: 0x04008F7F RID: 36735
		public int cellIdx;

		// Token: 0x04008F80 RID: 36736
		public int callbackIdx;

		// Token: 0x04008F81 RID: 36737
		public byte radius;

		// Token: 0x04008F82 RID: 36738
		public byte configuration;

		// Token: 0x04008F83 RID: 36739
		public ushort elementIdx;
	}

	// Token: 0x02001E84 RID: 7812
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetElementConsumerDataMessage
	{
		// Token: 0x04008F84 RID: 36740
		public int handle;

		// Token: 0x04008F85 RID: 36741
		public int cell;

		// Token: 0x04008F86 RID: 36742
		public float consumptionRate;
	}

	// Token: 0x02001E85 RID: 7813
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementConsumerMessage
	{
		// Token: 0x04008F87 RID: 36743
		public int handle;

		// Token: 0x04008F88 RID: 36744
		public int callbackIdx;
	}

	// Token: 0x02001E86 RID: 7814
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementEmitterMessage
	{
		// Token: 0x04008F89 RID: 36745
		public float maxPressure;

		// Token: 0x04008F8A RID: 36746
		public int callbackIdx;

		// Token: 0x04008F8B RID: 36747
		public int onBlockedCB;

		// Token: 0x04008F8C RID: 36748
		public int onUnblockedCB;
	}

	// Token: 0x02001E87 RID: 7815
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementEmitterMessage
	{
		// Token: 0x04008F8D RID: 36749
		public int handle;

		// Token: 0x04008F8E RID: 36750
		public int cellIdx;

		// Token: 0x04008F8F RID: 36751
		public float emitInterval;

		// Token: 0x04008F90 RID: 36752
		public float emitMass;

		// Token: 0x04008F91 RID: 36753
		public float emitTemperature;

		// Token: 0x04008F92 RID: 36754
		public float maxPressure;

		// Token: 0x04008F93 RID: 36755
		public int diseaseCount;

		// Token: 0x04008F94 RID: 36756
		public ushort elementIdx;

		// Token: 0x04008F95 RID: 36757
		public byte maxDepth;

		// Token: 0x04008F96 RID: 36758
		public byte diseaseIdx;
	}

	// Token: 0x02001E88 RID: 7816
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementEmitterMessage
	{
		// Token: 0x04008F97 RID: 36759
		public int handle;

		// Token: 0x04008F98 RID: 36760
		public int callbackIdx;
	}

	// Token: 0x02001E89 RID: 7817
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddRadiationEmitterMessage
	{
		// Token: 0x04008F99 RID: 36761
		public int callbackIdx;

		// Token: 0x04008F9A RID: 36762
		public int cell;

		// Token: 0x04008F9B RID: 36763
		public short emitRadiusX;

		// Token: 0x04008F9C RID: 36764
		public short emitRadiusY;

		// Token: 0x04008F9D RID: 36765
		public float emitRads;

		// Token: 0x04008F9E RID: 36766
		public float emitRate;

		// Token: 0x04008F9F RID: 36767
		public float emitSpeed;

		// Token: 0x04008FA0 RID: 36768
		public float emitDirection;

		// Token: 0x04008FA1 RID: 36769
		public float emitAngle;

		// Token: 0x04008FA2 RID: 36770
		public int emitType;
	}

	// Token: 0x02001E8A RID: 7818
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyRadiationEmitterMessage
	{
		// Token: 0x04008FA3 RID: 36771
		public int handle;

		// Token: 0x04008FA4 RID: 36772
		public int cell;

		// Token: 0x04008FA5 RID: 36773
		public int callbackIdx;

		// Token: 0x04008FA6 RID: 36774
		public short emitRadiusX;

		// Token: 0x04008FA7 RID: 36775
		public short emitRadiusY;

		// Token: 0x04008FA8 RID: 36776
		public float emitRads;

		// Token: 0x04008FA9 RID: 36777
		public float emitRate;

		// Token: 0x04008FAA RID: 36778
		public float emitSpeed;

		// Token: 0x04008FAB RID: 36779
		public float emitDirection;

		// Token: 0x04008FAC RID: 36780
		public float emitAngle;

		// Token: 0x04008FAD RID: 36781
		public int emitType;
	}

	// Token: 0x02001E8B RID: 7819
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveRadiationEmitterMessage
	{
		// Token: 0x04008FAE RID: 36782
		public int handle;

		// Token: 0x04008FAF RID: 36783
		public int callbackIdx;
	}

	// Token: 0x02001E8C RID: 7820
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementChunkMessage
	{
		// Token: 0x04008FB0 RID: 36784
		public int gameCell;

		// Token: 0x04008FB1 RID: 36785
		public int callbackIdx;

		// Token: 0x04008FB2 RID: 36786
		public float mass;

		// Token: 0x04008FB3 RID: 36787
		public float temperature;

		// Token: 0x04008FB4 RID: 36788
		public float surfaceArea;

		// Token: 0x04008FB5 RID: 36789
		public float thickness;

		// Token: 0x04008FB6 RID: 36790
		public float groundTransferScale;

		// Token: 0x04008FB7 RID: 36791
		public ushort elementIdx;

		// Token: 0x04008FB8 RID: 36792
		public byte pad0;

		// Token: 0x04008FB9 RID: 36793
		public byte pad1;
	}

	// Token: 0x02001E8D RID: 7821
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementChunkMessage
	{
		// Token: 0x04008FBA RID: 36794
		public int handle;

		// Token: 0x04008FBB RID: 36795
		public int callbackIdx;
	}

	// Token: 0x02001E8E RID: 7822
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetElementChunkDataMessage
	{
		// Token: 0x04008FBC RID: 36796
		public int handle;

		// Token: 0x04008FBD RID: 36797
		public float temperature;

		// Token: 0x04008FBE RID: 36798
		public float heatCapacity;
	}

	// Token: 0x02001E8F RID: 7823
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MoveElementChunkMessage
	{
		// Token: 0x04008FBF RID: 36799
		public int handle;

		// Token: 0x04008FC0 RID: 36800
		public int gameCell;
	}

	// Token: 0x02001E90 RID: 7824
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementChunkEnergyMessage
	{
		// Token: 0x04008FC1 RID: 36801
		public int handle;

		// Token: 0x04008FC2 RID: 36802
		public float deltaKJ;
	}

	// Token: 0x02001E91 RID: 7825
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementChunkAdjusterMessage
	{
		// Token: 0x04008FC3 RID: 36803
		public int handle;

		// Token: 0x04008FC4 RID: 36804
		public float temperature;

		// Token: 0x04008FC5 RID: 36805
		public float heatCapacity;

		// Token: 0x04008FC6 RID: 36806
		public float thermalConductivity;
	}

	// Token: 0x02001E92 RID: 7826
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddBuildingHeatExchangeMessage
	{
		// Token: 0x04008FC7 RID: 36807
		public int callbackIdx;

		// Token: 0x04008FC8 RID: 36808
		public ushort elemIdx;

		// Token: 0x04008FC9 RID: 36809
		public byte pad0;

		// Token: 0x04008FCA RID: 36810
		public byte pad1;

		// Token: 0x04008FCB RID: 36811
		public float mass;

		// Token: 0x04008FCC RID: 36812
		public float temperature;

		// Token: 0x04008FCD RID: 36813
		public float thermalConductivity;

		// Token: 0x04008FCE RID: 36814
		public float overheatTemperature;

		// Token: 0x04008FCF RID: 36815
		public float operatingKilowatts;

		// Token: 0x04008FD0 RID: 36816
		public int minX;

		// Token: 0x04008FD1 RID: 36817
		public int minY;

		// Token: 0x04008FD2 RID: 36818
		public int maxX;

		// Token: 0x04008FD3 RID: 36819
		public int maxY;
	}

	// Token: 0x02001E93 RID: 7827
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyBuildingHeatExchangeMessage
	{
		// Token: 0x04008FD4 RID: 36820
		public int callbackIdx;

		// Token: 0x04008FD5 RID: 36821
		public ushort elemIdx;

		// Token: 0x04008FD6 RID: 36822
		public byte pad0;

		// Token: 0x04008FD7 RID: 36823
		public byte pad1;

		// Token: 0x04008FD8 RID: 36824
		public float mass;

		// Token: 0x04008FD9 RID: 36825
		public float temperature;

		// Token: 0x04008FDA RID: 36826
		public float thermalConductivity;

		// Token: 0x04008FDB RID: 36827
		public float overheatTemperature;

		// Token: 0x04008FDC RID: 36828
		public float operatingKilowatts;

		// Token: 0x04008FDD RID: 36829
		public int minX;

		// Token: 0x04008FDE RID: 36830
		public int minY;

		// Token: 0x04008FDF RID: 36831
		public int maxX;

		// Token: 0x04008FE0 RID: 36832
		public int maxY;
	}

	// Token: 0x02001E94 RID: 7828
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyBuildingEnergyMessage
	{
		// Token: 0x04008FE1 RID: 36833
		public int handle;

		// Token: 0x04008FE2 RID: 36834
		public float deltaKJ;

		// Token: 0x04008FE3 RID: 36835
		public float minTemperature;

		// Token: 0x04008FE4 RID: 36836
		public float maxTemperature;
	}

	// Token: 0x02001E95 RID: 7829
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingHeatExchangeMessage
	{
		// Token: 0x04008FE5 RID: 36837
		public int handle;

		// Token: 0x04008FE6 RID: 36838
		public int callbackIdx;
	}

	// Token: 0x02001E96 RID: 7830
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RegisterBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x04008FE7 RID: 36839
		public int callbackIdx;

		// Token: 0x04008FE8 RID: 36840
		public int structureTemperatureHandler;
	}

	// Token: 0x02001E97 RID: 7831
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x04008FE9 RID: 36841
		public int selfHandler;

		// Token: 0x04008FEA RID: 36842
		public int buildingInContactHandle;

		// Token: 0x04008FEB RID: 36843
		public int cellsInContact;
	}

	// Token: 0x02001E98 RID: 7832
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x04008FEC RID: 36844
		public int selfHandler;

		// Token: 0x04008FED RID: 36845
		public int buildingNoLongerInContactHandler;
	}

	// Token: 0x02001E99 RID: 7833
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x04008FEE RID: 36846
		public int callbackIdx;

		// Token: 0x04008FEF RID: 36847
		public int selfHandler;
	}

	// Token: 0x02001E9A RID: 7834
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddDiseaseEmitterMessage
	{
		// Token: 0x04008FF0 RID: 36848
		public int callbackIdx;
	}

	// Token: 0x02001E9B RID: 7835
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyDiseaseEmitterMessage
	{
		// Token: 0x04008FF1 RID: 36849
		public int handle;

		// Token: 0x04008FF2 RID: 36850
		public int gameCell;

		// Token: 0x04008FF3 RID: 36851
		public byte diseaseIdx;

		// Token: 0x04008FF4 RID: 36852
		public byte maxDepth;

		// Token: 0x04008FF5 RID: 36853
		private byte pad0;

		// Token: 0x04008FF6 RID: 36854
		private byte pad1;

		// Token: 0x04008FF7 RID: 36855
		public float emitInterval;

		// Token: 0x04008FF8 RID: 36856
		public int emitCount;
	}

	// Token: 0x02001E9C RID: 7836
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveDiseaseEmitterMessage
	{
		// Token: 0x04008FF9 RID: 36857
		public int handle;

		// Token: 0x04008FFA RID: 36858
		public int callbackIdx;
	}

	// Token: 0x02001E9D RID: 7837
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetSavedOptionsMessage
	{
		// Token: 0x04008FFB RID: 36859
		public byte clearBits;

		// Token: 0x04008FFC RID: 36860
		public byte setBits;
	}

	// Token: 0x02001E9E RID: 7838
	public enum SimSavedOptions : byte
	{
		// Token: 0x04008FFE RID: 36862
		ENABLE_DIAGONAL_FALLING_SAND = 1
	}

	// Token: 0x02001E9F RID: 7839
	public struct WorldOffsetData
	{
		// Token: 0x04008FFF RID: 36863
		public int worldOffsetX;

		// Token: 0x04009000 RID: 36864
		public int worldOffsetY;

		// Token: 0x04009001 RID: 36865
		public int worldSizeX;

		// Token: 0x04009002 RID: 36866
		public int worldSizeY;
	}

	// Token: 0x02001EA0 RID: 7840
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct DigMessage
	{
		// Token: 0x04009003 RID: 36867
		public int cellIdx;

		// Token: 0x04009004 RID: 36868
		public int callbackIdx;

		// Token: 0x04009005 RID: 36869
		public bool skipEvent;
	}

	// Token: 0x02001EA1 RID: 7841
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetCellFloatValueMessage
	{
		// Token: 0x04009006 RID: 36870
		public int cellIdx;

		// Token: 0x04009007 RID: 36871
		public float value;
	}

	// Token: 0x02001EA2 RID: 7842
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellPropertiesMessage
	{
		// Token: 0x04009008 RID: 36872
		public int cellIdx;

		// Token: 0x04009009 RID: 36873
		public byte properties;

		// Token: 0x0400900A RID: 36874
		public byte set;

		// Token: 0x0400900B RID: 36875
		public byte pad0;

		// Token: 0x0400900C RID: 36876
		public byte pad1;
	}

	// Token: 0x02001EA3 RID: 7843
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetInsulationValueMessage
	{
		// Token: 0x0400900D RID: 36877
		public int cellIdx;

		// Token: 0x0400900E RID: 36878
		public float value;
	}

	// Token: 0x02001EA4 RID: 7844
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyCellMessage
	{
		// Token: 0x0400900F RID: 36879
		public int cellIdx;

		// Token: 0x04009010 RID: 36880
		public int callbackIdx;

		// Token: 0x04009011 RID: 36881
		public float temperature;

		// Token: 0x04009012 RID: 36882
		public float mass;

		// Token: 0x04009013 RID: 36883
		public int diseaseCount;

		// Token: 0x04009014 RID: 36884
		public ushort elementIdx;

		// Token: 0x04009015 RID: 36885
		public byte replaceType;

		// Token: 0x04009016 RID: 36886
		public byte diseaseIdx;

		// Token: 0x04009017 RID: 36887
		public byte addSubType;
	}

	// Token: 0x02001EA5 RID: 7845
	public enum ReplaceType
	{
		// Token: 0x04009019 RID: 36889
		None,
		// Token: 0x0400901A RID: 36890
		Replace,
		// Token: 0x0400901B RID: 36891
		ReplaceAndDisplace
	}

	// Token: 0x02001EA6 RID: 7846
	private enum AddSolidMassSubType
	{
		// Token: 0x0400901D RID: 36893
		DoVerticalDisplacement,
		// Token: 0x0400901E RID: 36894
		OnlyIfSameElement
	}

	// Token: 0x02001EA7 RID: 7847
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellDiseaseModification
	{
		// Token: 0x0400901F RID: 36895
		public int cellIdx;

		// Token: 0x04009020 RID: 36896
		public byte diseaseIdx;

		// Token: 0x04009021 RID: 36897
		public byte pad0;

		// Token: 0x04009022 RID: 36898
		public byte pad1;

		// Token: 0x04009023 RID: 36899
		public byte pad2;

		// Token: 0x04009024 RID: 36900
		public int diseaseCount;
	}

	// Token: 0x02001EA8 RID: 7848
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RadiationParamsModification
	{
		// Token: 0x04009025 RID: 36901
		public int RadiationParamsType;

		// Token: 0x04009026 RID: 36902
		public float value;
	}

	// Token: 0x02001EA9 RID: 7849
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellRadiationModification
	{
		// Token: 0x04009027 RID: 36903
		public int cellIdx;

		// Token: 0x04009028 RID: 36904
		public float radiationDelta;

		// Token: 0x04009029 RID: 36905
		public int callbackIdx;
	}

	// Token: 0x02001EAA RID: 7850
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MassConsumptionMessage
	{
		// Token: 0x0400902A RID: 36906
		public int cellIdx;

		// Token: 0x0400902B RID: 36907
		public int callbackIdx;

		// Token: 0x0400902C RID: 36908
		public float mass;

		// Token: 0x0400902D RID: 36909
		public ushort elementIdx;

		// Token: 0x0400902E RID: 36910
		public byte radius;
	}

	// Token: 0x02001EAB RID: 7851
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MassEmissionMessage
	{
		// Token: 0x0400902F RID: 36911
		public int cellIdx;

		// Token: 0x04009030 RID: 36912
		public int callbackIdx;

		// Token: 0x04009031 RID: 36913
		public float mass;

		// Token: 0x04009032 RID: 36914
		public float temperature;

		// Token: 0x04009033 RID: 36915
		public int diseaseCount;

		// Token: 0x04009034 RID: 36916
		public ushort elementIdx;

		// Token: 0x04009035 RID: 36917
		public byte diseaseIdx;
	}

	// Token: 0x02001EAC RID: 7852
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ConsumeDiseaseMessage
	{
		// Token: 0x04009036 RID: 36918
		public int gameCell;

		// Token: 0x04009037 RID: 36919
		public int callbackIdx;

		// Token: 0x04009038 RID: 36920
		public float percentToConsume;

		// Token: 0x04009039 RID: 36921
		public int maxToConsume;
	}

	// Token: 0x02001EAD RID: 7853
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyCellEnergyMessage
	{
		// Token: 0x0400903A RID: 36922
		public int cellIdx;

		// Token: 0x0400903B RID: 36923
		public float kilojoules;

		// Token: 0x0400903C RID: 36924
		public float maxTemperature;

		// Token: 0x0400903D RID: 36925
		public int id;
	}

	// Token: 0x02001EAE RID: 7854
	public enum EnergySourceID
	{
		// Token: 0x0400903F RID: 36927
		DebugHeat = 1000,
		// Token: 0x04009040 RID: 36928
		DebugCool,
		// Token: 0x04009041 RID: 36929
		FierySkin,
		// Token: 0x04009042 RID: 36930
		Overheatable,
		// Token: 0x04009043 RID: 36931
		LiquidCooledFan,
		// Token: 0x04009044 RID: 36932
		ConduitTemperatureManager,
		// Token: 0x04009045 RID: 36933
		Excavator,
		// Token: 0x04009046 RID: 36934
		HeatBulb,
		// Token: 0x04009047 RID: 36935
		WarmBlooded,
		// Token: 0x04009048 RID: 36936
		StructureTemperature,
		// Token: 0x04009049 RID: 36937
		Burner,
		// Token: 0x0400904A RID: 36938
		VacuumRadiator
	}

	// Token: 0x02001EAF RID: 7855
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct VisibleCells
	{
		// Token: 0x0400904B RID: 36939
		public Vector2I min;

		// Token: 0x0400904C RID: 36940
		public Vector2I max;
	}

	// Token: 0x02001EB0 RID: 7856
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct WakeCellMessage
	{
		// Token: 0x0400904D RID: 36941
		public int gameCell;
	}

	// Token: 0x02001EB1 RID: 7857
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ElementInteraction
	{
		// Token: 0x0400904E RID: 36942
		public uint interactionType;

		// Token: 0x0400904F RID: 36943
		public ushort elemIdx1;

		// Token: 0x04009050 RID: 36944
		public ushort elemIdx2;

		// Token: 0x04009051 RID: 36945
		public ushort elemResultIdx;

		// Token: 0x04009052 RID: 36946
		public byte pad0;

		// Token: 0x04009053 RID: 36947
		public byte pad1;

		// Token: 0x04009054 RID: 36948
		public float minMass;

		// Token: 0x04009055 RID: 36949
		public float interactionProbability;

		// Token: 0x04009056 RID: 36950
		public float elem1MassDestructionPercent;

		// Token: 0x04009057 RID: 36951
		public float elem2MassRequiredMultiplier;

		// Token: 0x04009058 RID: 36952
		public float elemResultMassCreationMultiplier;
	}

	// Token: 0x02001EB2 RID: 7858
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CreateElementInteractionsMsg
	{
		// Token: 0x04009059 RID: 36953
		public int numInteractions;

		// Token: 0x0400905A RID: 36954
		public unsafe SimMessages.ElementInteraction* interactions;
	}

	// Token: 0x02001EB3 RID: 7859
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PipeChange
	{
		// Token: 0x0400905B RID: 36955
		public int cell;

		// Token: 0x0400905C RID: 36956
		public byte layer;

		// Token: 0x0400905D RID: 36957
		public byte pad0;

		// Token: 0x0400905E RID: 36958
		public byte pad1;

		// Token: 0x0400905F RID: 36959
		public byte pad2;

		// Token: 0x04009060 RID: 36960
		public float mass;

		// Token: 0x04009061 RID: 36961
		public float temperature;

		// Token: 0x04009062 RID: 36962
		public int elementHash;
	}

	// Token: 0x02001EB4 RID: 7860
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CellWorldZoneModification
	{
		// Token: 0x04009063 RID: 36963
		public int cell;

		// Token: 0x04009064 RID: 36964
		public byte zoneID;
	}
}
