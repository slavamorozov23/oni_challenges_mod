using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000B78 RID: 2936
public class RocketStats
{
	// Token: 0x0600574B RID: 22347 RVA: 0x001FC112 File Offset: 0x001FA312
	public RocketStats(CommandModule commandModule)
	{
		this.commandModule = commandModule;
	}

	// Token: 0x0600574C RID: 22348 RVA: 0x001FC124 File Offset: 0x001FA324
	public float GetRocketMaxDistance()
	{
		float totalMass = this.GetTotalMass();
		float totalThrust = this.GetTotalThrust();
		float num = ROCKETRY.CalculateMassWithPenalty(totalMass);
		float num2 = Mathf.Max(0f, totalThrust - num);
		RoboPilotModule component = this.commandModule.GetComponent<RoboPilotModule>();
		if (component != null)
		{
			num2 = Mathf.Min(num2, component.GetDataBankRange());
		}
		return num2;
	}

	// Token: 0x0600574D RID: 22349 RVA: 0x001FC175 File Offset: 0x001FA375
	public float GetTotalMass()
	{
		return this.GetDryMass() + this.GetWetMass();
	}

	// Token: 0x0600574E RID: 22350 RVA: 0x001FC184 File Offset: 0x001FA384
	public float GetDryMass()
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null)
			{
				num += component.GetComponent<PrimaryElement>().Mass;
			}
		}
		return num;
	}

	// Token: 0x0600574F RID: 22351 RVA: 0x001FC200 File Offset: 0x001FA400
	public float GetWetMass()
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null)
			{
				FuelTank component2 = component.GetComponent<FuelTank>();
				OxidizerTank component3 = component.GetComponent<OxidizerTank>();
				SolidBooster component4 = component.GetComponent<SolidBooster>();
				if (component2 != null)
				{
					num += component2.storage.MassStored();
				}
				if (component3 != null)
				{
					num += component3.storage.MassStored();
				}
				if (component4 != null)
				{
					num += component4.fuelStorage.MassStored();
				}
			}
		}
		return num;
	}

	// Token: 0x06005750 RID: 22352 RVA: 0x001FC2CC File Offset: 0x001FA4CC
	public Tag GetEngineFuelTag()
	{
		RocketEngine mainEngine = this.GetMainEngine();
		if (mainEngine != null)
		{
			return mainEngine.fuelTag;
		}
		return null;
	}

	// Token: 0x06005751 RID: 22353 RVA: 0x001FC2F8 File Offset: 0x001FA4F8
	public float GetTotalFuel(bool includeBoosters = false)
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			FuelTank component = gameObject.GetComponent<FuelTank>();
			Tag engineFuelTag = this.GetEngineFuelTag();
			if (component != null)
			{
				num += component.storage.GetAmountAvailable(engineFuelTag);
			}
			if (includeBoosters)
			{
				SolidBooster component2 = gameObject.GetComponent<SolidBooster>();
				if (component2 != null)
				{
					num += component2.fuelStorage.GetAmountAvailable(component2.fuelTag);
				}
			}
		}
		return num;
	}

	// Token: 0x06005752 RID: 22354 RVA: 0x001FC3A8 File Offset: 0x001FA5A8
	public float GetTotalOxidizer(bool includeBoosters = false)
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			OxidizerTank component = gameObject.GetComponent<OxidizerTank>();
			if (component != null)
			{
				num += component.GetTotalOxidizerAvailable();
			}
			if (includeBoosters)
			{
				SolidBooster component2 = gameObject.GetComponent<SolidBooster>();
				if (component2 != null)
				{
					num += component2.fuelStorage.GetAmountAvailable(GameTags.OxyRock);
				}
			}
		}
		return num;
	}

	// Token: 0x06005753 RID: 22355 RVA: 0x001FC448 File Offset: 0x001FA648
	public float GetAverageOxidizerEfficiency()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		dictionary[SimHashes.LiquidOxygen.CreateTag()] = 0f;
		dictionary[SimHashes.OxyRock.CreateTag()] = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			OxidizerTank component = gameObject.GetComponent<OxidizerTank>();
			if (component != null)
			{
				foreach (KeyValuePair<Tag, float> keyValuePair in component.GetOxidizersAvailable())
				{
					if (dictionary.ContainsKey(keyValuePair.Key))
					{
						Dictionary<Tag, float> dictionary2 = dictionary;
						Tag key = keyValuePair.Key;
						dictionary2[key] += keyValuePair.Value;
					}
				}
			}
		}
		float num = 0f;
		float num2 = 0f;
		foreach (KeyValuePair<Tag, float> keyValuePair2 in dictionary)
		{
			num += keyValuePair2.Value * RocketStats.oxidizerEfficiencies[keyValuePair2.Key];
			num2 += keyValuePair2.Value;
		}
		if (num2 == 0f)
		{
			return 0f;
		}
		return num / num2 * 100f;
	}

	// Token: 0x06005754 RID: 22356 RVA: 0x001FC5D8 File Offset: 0x001FA7D8
	public float GetTotalThrust()
	{
		float totalFuel = this.GetTotalFuel(false);
		float totalOxidizer = this.GetTotalOxidizer(false);
		float averageOxidizerEfficiency = this.GetAverageOxidizerEfficiency();
		RocketEngine mainEngine = this.GetMainEngine();
		if (mainEngine == null)
		{
			return 0f;
		}
		return (mainEngine.requireOxidizer ? (Mathf.Min(totalFuel, totalOxidizer) * (mainEngine.efficiency * (averageOxidizerEfficiency / 100f))) : (totalFuel * mainEngine.efficiency)) + this.GetBoosterThrust();
	}

	// Token: 0x06005755 RID: 22357 RVA: 0x001FC644 File Offset: 0x001FA844
	public float GetBoosterThrust()
	{
		float num = 0f;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			SolidBooster component = gameObject.GetComponent<SolidBooster>();
			if (component != null)
			{
				float amountAvailable = component.fuelStorage.GetAmountAvailable(ElementLoader.FindElementByHash(SimHashes.OxyRock).tag);
				float amountAvailable2 = component.fuelStorage.GetAmountAvailable(ElementLoader.FindElementByHash(SimHashes.Iron).tag);
				num += component.efficiency * Mathf.Min(amountAvailable, amountAvailable2);
			}
		}
		return num;
	}

	// Token: 0x06005756 RID: 22358 RVA: 0x001FC6F8 File Offset: 0x001FA8F8
	public float GetEngineEfficiency()
	{
		RocketEngine mainEngine = this.GetMainEngine();
		if (mainEngine != null)
		{
			return mainEngine.efficiency;
		}
		return 0f;
	}

	// Token: 0x06005757 RID: 22359 RVA: 0x001FC724 File Offset: 0x001FA924
	public RocketEngine GetMainEngine()
	{
		RocketEngine rocketEngine = null;
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			rocketEngine = gameObject.GetComponent<RocketEngine>();
			if (rocketEngine != null && rocketEngine.mainEngine)
			{
				break;
			}
		}
		return rocketEngine;
	}

	// Token: 0x06005758 RID: 22360 RVA: 0x001FC798 File Offset: 0x001FA998
	public float GetTotalOxidizableFuel()
	{
		float totalFuel = this.GetTotalFuel(false);
		float totalOxidizer = this.GetTotalOxidizer(false);
		return Mathf.Min(totalFuel, totalOxidizer);
	}

	// Token: 0x04003ABC RID: 15036
	private CommandModule commandModule;

	// Token: 0x04003ABD RID: 15037
	public static Dictionary<Tag, float> oxidizerEfficiencies = new Dictionary<Tag, float>
	{
		{
			SimHashes.OxyRock.CreateTag(),
			ROCKETRY.OXIDIZER_EFFICIENCY.LOW
		},
		{
			SimHashes.LiquidOxygen.CreateTag(),
			ROCKETRY.OXIDIZER_EFFICIENCY.HIGH
		}
	};
}
