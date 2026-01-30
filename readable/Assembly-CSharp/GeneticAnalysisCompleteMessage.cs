using System;
using KSerialization;
using STRINGS;

// Token: 0x02000D93 RID: 3475
public class GeneticAnalysisCompleteMessage : Message
{
	// Token: 0x06006C34 RID: 27700 RVA: 0x00290E5F File Offset: 0x0028F05F
	public GeneticAnalysisCompleteMessage()
	{
	}

	// Token: 0x06006C35 RID: 27701 RVA: 0x00290E67 File Offset: 0x0028F067
	public GeneticAnalysisCompleteMessage(Tag subSpeciesID)
	{
		this.subSpeciesID = subSpeciesID;
	}

	// Token: 0x06006C36 RID: 27702 RVA: 0x00290E76 File Offset: 0x0028F076
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x06006C37 RID: 27703 RVA: 0x00290E80 File Offset: 0x0028F080
	public override string GetMessageBody()
	{
		PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = PlantSubSpeciesCatalog.Instance.FindSubSpecies(this.subSpeciesID);
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.MESSAGEBODY.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName()).Replace("{Subspecies}", subSpeciesInfo.GetNameWithMutations(subSpeciesInfo.speciesID.ProperName(), true, false)).Replace("{Info}", subSpeciesInfo.GetMutationsTooltip());
	}

	// Token: 0x06006C38 RID: 27704 RVA: 0x00290EE7 File Offset: 0x0028F0E7
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.NAME;
	}

	// Token: 0x06006C39 RID: 27705 RVA: 0x00290EF4 File Offset: 0x0028F0F4
	public override string GetTooltip()
	{
		PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = PlantSubSpeciesCatalog.Instance.FindSubSpecies(this.subSpeciesID);
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.TOOLTIP.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName());
	}

	// Token: 0x06006C3A RID: 27706 RVA: 0x00290F2C File Offset: 0x0028F12C
	public override bool IsValid()
	{
		return this.subSpeciesID.IsValid;
	}

	// Token: 0x04004A2C RID: 18988
	[Serialize]
	public Tag subSpeciesID;
}
