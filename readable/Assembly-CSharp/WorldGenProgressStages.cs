using System;
using System.Collections.Generic;

// Token: 0x02000EDB RID: 3803
public static class WorldGenProgressStages
{
	// Token: 0x0400552A RID: 21802
	public static KeyValuePair<WorldGenProgressStages.Stages, float>[] StageWeights = new KeyValuePair<WorldGenProgressStages.Stages, float>[]
	{
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Failure, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.SetupNoise, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.GenerateNoise, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.GenerateSolarSystem, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.WorldLayout, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.CompleteLayout, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.NoiseMapBuilder, 9f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ClearingLevel, 0.5f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Processing, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Borders, 0.1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ProcessRivers, 0.1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ConvertCellsToEdges, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.DrawWorldBorder, 0.2f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.PlaceTemplates, 5f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.SettleSim, 6f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.DetectNaturalCavities, 6f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.PlacingCreatures, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Complete, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.NumberOfStages, 0f)
	};

	// Token: 0x0200213E RID: 8510
	public enum Stages
	{
		// Token: 0x040098B6 RID: 39094
		Failure,
		// Token: 0x040098B7 RID: 39095
		SetupNoise,
		// Token: 0x040098B8 RID: 39096
		GenerateNoise,
		// Token: 0x040098B9 RID: 39097
		GenerateSolarSystem,
		// Token: 0x040098BA RID: 39098
		WorldLayout,
		// Token: 0x040098BB RID: 39099
		CompleteLayout,
		// Token: 0x040098BC RID: 39100
		NoiseMapBuilder,
		// Token: 0x040098BD RID: 39101
		ClearingLevel,
		// Token: 0x040098BE RID: 39102
		Processing,
		// Token: 0x040098BF RID: 39103
		Borders,
		// Token: 0x040098C0 RID: 39104
		ProcessRivers,
		// Token: 0x040098C1 RID: 39105
		ConvertCellsToEdges,
		// Token: 0x040098C2 RID: 39106
		DrawWorldBorder,
		// Token: 0x040098C3 RID: 39107
		PlaceTemplates,
		// Token: 0x040098C4 RID: 39108
		SettleSim,
		// Token: 0x040098C5 RID: 39109
		DetectNaturalCavities,
		// Token: 0x040098C6 RID: 39110
		PlacingCreatures,
		// Token: 0x040098C7 RID: 39111
		Complete,
		// Token: 0x040098C8 RID: 39112
		NumberOfStages
	}
}
