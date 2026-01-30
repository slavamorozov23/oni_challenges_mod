using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000B39 RID: 2873
public class MaterialsAvailable : SelectModuleCondition
{
	// Token: 0x060054B0 RID: 21680 RVA: 0x001EE876 File Offset: 0x001ECA76
	public override bool IgnoreInSanboxMode()
	{
		return true;
	}

	// Token: 0x060054B1 RID: 21681 RVA: 0x001EE879 File Offset: 0x001ECA79
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		return existingModule == null || ProductInfoScreen.MaterialsMet(selectedPart.CraftRecipe);
	}

	// Token: 0x060054B2 RID: 21682 RVA: 0x001EE894 File Offset: 0x001ECA94
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MATERIALS_AVAILABLE.COMPLETE;
		}
		string text = UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MATERIALS_AVAILABLE.FAILED;
		foreach (Recipe.Ingredient ingredient in selectedPart.CraftRecipe.Ingredients)
		{
			string str = "\n" + string.Format("{0}{1}: {2}", "    • ", ingredient.tag.ProperName(), GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			text += str;
		}
		return text;
	}
}
