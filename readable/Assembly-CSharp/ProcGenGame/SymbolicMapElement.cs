using System;

namespace ProcGenGame
{
	// Token: 0x02000EE2 RID: 3810
	public interface SymbolicMapElement
	{
		// Token: 0x060079FC RID: 31228
		void ConvertToMap(Chunk world, TerrainCell.SetValuesFunction SetValues, float temperatureMin, float temperatureRange, SeededRandom rnd);
	}
}
