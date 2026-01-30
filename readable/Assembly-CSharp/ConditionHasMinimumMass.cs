using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000BC3 RID: 3011
public class ConditionHasMinimumMass : ProcessCondition
{
	// Token: 0x06005A5F RID: 23135 RVA: 0x0020C6FF File Offset: 0x0020A8FF
	public ConditionHasMinimumMass(CommandModule command)
	{
		this.commandModule = command;
	}

	// Token: 0x06005A60 RID: 23136 RVA: 0x0020C710 File Offset: 0x0020A910
	public override ProcessCondition.Status EvaluateCondition()
	{
		int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.commandModule.GetComponent<LaunchConditionManager>()).id;
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
		if (spacecraftDestination != null && SpacecraftManager.instance.GetDestinationAnalysisState(spacecraftDestination) == SpacecraftManager.DestinationAnalysisState.Complete && spacecraftDestination.AvailableMass >= ConditionHasMinimumMass.CargoCapacity(spacecraftDestination, this.commandModule))
		{
			return ProcessCondition.Status.Ready;
		}
		return ProcessCondition.Status.Warning;
	}

	// Token: 0x06005A61 RID: 23137 RVA: 0x0020C76C File Offset: 0x0020A96C
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.commandModule.GetComponent<LaunchConditionManager>()).id;
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
		if (spacecraftDestination == null)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.NO_DESTINATION;
		}
		if (SpacecraftManager.instance.GetDestinationAnalysisState(spacecraftDestination) == SpacecraftManager.DestinationAnalysisState.Complete)
		{
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.MINIMUM_MASS, GameUtil.GetFormattedMass(spacecraftDestination.AvailableMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"));
		}
		return string.Format(UI.STARMAP.LAUNCHCHECKLIST.MINIMUM_MASS, UI.STARMAP.COMPOSITION_UNDISCOVERED_AMOUNT);
	}

	// Token: 0x06005A62 RID: 23138 RVA: 0x0020C7F4 File Offset: 0x0020A9F4
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.commandModule.GetComponent<LaunchConditionManager>()).id;
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
		bool flag = spacecraftDestination != null && SpacecraftManager.instance.GetDestinationAnalysisState(spacecraftDestination) == SpacecraftManager.DestinationAnalysisState.Complete;
		string text = "";
		if (flag)
		{
			if (spacecraftDestination.AvailableMass <= ConditionHasMinimumMass.CargoCapacity(spacecraftDestination, this.commandModule))
			{
				text = text + UI.STARMAP.LAUNCHCHECKLIST.INSUFFICENT_MASS_TOOLTIP + "\n\n";
			}
			text = text + string.Format(UI.STARMAP.LAUNCHCHECKLIST.RESOURCE_MASS_TOOLTIP, spacecraftDestination.GetDestinationType().Name, GameUtil.GetFormattedMass(spacecraftDestination.AvailableMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), GameUtil.GetFormattedMass(ConditionHasMinimumMass.CargoCapacity(spacecraftDestination, this.commandModule), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}")) + "\n\n";
		}
		float num = (spacecraftDestination != null) ? spacecraftDestination.AvailableMass : 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			CargoBay component = gameObject.GetComponent<CargoBay>();
			if (component != null)
			{
				if (flag)
				{
					float availableResourcesPercentage = spacecraftDestination.GetAvailableResourcesPercentage(component.storageType);
					float num2 = Mathf.Min(component.storage.Capacity(), availableResourcesPercentage * num);
					num -= num2;
					text = string.Concat(new string[]
					{
						text,
						component.gameObject.GetProperName(),
						" ",
						string.Format(UI.STARMAP.STORAGESTATS.STORAGECAPACITY, GameUtil.GetFormattedMass(Mathf.Min(num2, component.storage.Capacity()), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), GameUtil.GetFormattedMass(component.storage.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}")),
						"\n"
					});
				}
				else
				{
					text = string.Concat(new string[]
					{
						text,
						component.gameObject.GetProperName(),
						" ",
						string.Format(UI.STARMAP.STORAGESTATS.STORAGECAPACITY, GameUtil.GetFormattedMass(0f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), GameUtil.GetFormattedMass(component.storage.Capacity(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}")),
						"\n"
					});
				}
			}
		}
		return text;
	}

	// Token: 0x06005A63 RID: 23139 RVA: 0x0020CA60 File Offset: 0x0020AC60
	public static float CargoCapacity(SpaceDestination destination, CommandModule module)
	{
		if (module == null)
		{
			return 0f;
		}
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(module.GetComponent<AttachableBuilding>()))
		{
			CargoBay component = gameObject.GetComponent<CargoBay>();
			if (component != null && destination.HasElementType(component.storageType))
			{
				Storage component2 = component.GetComponent<Storage>();
				num += component2.capacityKg;
			}
		}
		return num;
	}

	// Token: 0x06005A64 RID: 23140 RVA: 0x0020CAF4 File Offset: 0x0020ACF4
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003C64 RID: 15460
	private CommandModule commandModule;
}
