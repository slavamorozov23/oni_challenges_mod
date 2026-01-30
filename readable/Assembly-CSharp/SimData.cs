using System;

// Token: 0x02000B4A RID: 2890
public class SimData
{
	// Token: 0x04003971 RID: 14705
	public unsafe Sim.EmittedMassInfo* emittedMassEntries;

	// Token: 0x04003972 RID: 14706
	public unsafe Sim.ElementChunkInfo* elementChunks;

	// Token: 0x04003973 RID: 14707
	public unsafe Sim.BuildingTemperatureInfo* buildingTemperatures;

	// Token: 0x04003974 RID: 14708
	public unsafe Sim.DiseaseEmittedInfo* diseaseEmittedInfos;

	// Token: 0x04003975 RID: 14709
	public unsafe Sim.DiseaseConsumedInfo* diseaseConsumedInfos;
}
