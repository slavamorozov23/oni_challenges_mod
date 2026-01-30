using System;
using System.Runtime.CompilerServices;

// Token: 0x02000847 RID: 2119
public readonly struct ClothingOutfitNameProposal
{
	// Token: 0x060039FA RID: 14842 RVA: 0x0014445F File Offset: 0x0014265F
	private ClothingOutfitNameProposal(string candidateName, ClothingOutfitNameProposal.Result result)
	{
		this.candidateName = candidateName;
		this.result = result;
	}

	// Token: 0x060039FB RID: 14843 RVA: 0x00144470 File Offset: 0x00142670
	public static ClothingOutfitNameProposal ForNewOutfit(string candidateName)
	{
		ClothingOutfitNameProposal.<>c__DisplayClass3_0 CS$<>8__locals1;
		CS$<>8__locals1.candidateName = candidateName;
		if (string.IsNullOrEmpty(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result.Error_NoInputName, ref CS$<>8__locals1);
		}
		if (ClothingOutfitTarget.DoesTemplateExist(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result.Error_NameAlreadyExists, ref CS$<>8__locals1);
		}
		return ClothingOutfitNameProposal.<ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result.NewOutfit, ref CS$<>8__locals1);
	}

	// Token: 0x060039FC RID: 14844 RVA: 0x001444BC File Offset: 0x001426BC
	public static ClothingOutfitNameProposal FromExistingOutfit(string candidateName, ClothingOutfitTarget existingOutfit, bool isSameNameAllowed)
	{
		ClothingOutfitNameProposal.<>c__DisplayClass4_0 CS$<>8__locals1;
		CS$<>8__locals1.candidateName = candidateName;
		if (string.IsNullOrEmpty(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.Error_NoInputName, ref CS$<>8__locals1);
		}
		if (!ClothingOutfitTarget.DoesTemplateExist(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.NewOutfit, ref CS$<>8__locals1);
		}
		if (!isSameNameAllowed || !(CS$<>8__locals1.candidateName == existingOutfit.ReadName()))
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.Error_NameAlreadyExists, ref CS$<>8__locals1);
		}
		if (existingOutfit.CanWriteName)
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.SameOutfit, ref CS$<>8__locals1);
		}
		return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.Error_SameOutfitReadonly, ref CS$<>8__locals1);
	}

	// Token: 0x060039FD RID: 14845 RVA: 0x00144537 File Offset: 0x00142737
	[CompilerGenerated]
	internal static ClothingOutfitNameProposal <ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result result, ref ClothingOutfitNameProposal.<>c__DisplayClass3_0 A_1)
	{
		return new ClothingOutfitNameProposal(A_1.candidateName, result);
	}

	// Token: 0x060039FE RID: 14846 RVA: 0x00144545 File Offset: 0x00142745
	[CompilerGenerated]
	internal static ClothingOutfitNameProposal <FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result result, ref ClothingOutfitNameProposal.<>c__DisplayClass4_0 A_1)
	{
		return new ClothingOutfitNameProposal(A_1.candidateName, result);
	}

	// Token: 0x0400236C RID: 9068
	public readonly string candidateName;

	// Token: 0x0400236D RID: 9069
	public readonly ClothingOutfitNameProposal.Result result;

	// Token: 0x020017EE RID: 6126
	public enum Result
	{
		// Token: 0x04007930 RID: 31024
		None,
		// Token: 0x04007931 RID: 31025
		NewOutfit,
		// Token: 0x04007932 RID: 31026
		SameOutfit,
		// Token: 0x04007933 RID: 31027
		Error_NoInputName,
		// Token: 0x04007934 RID: 31028
		Error_NameAlreadyExists,
		// Token: 0x04007935 RID: 31029
		Error_SameOutfitReadonly
	}
}
