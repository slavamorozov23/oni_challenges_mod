using System;
using System.Collections.Generic;

// Token: 0x0200091F RID: 2335
public class EnergySim
{
	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06004151 RID: 16721 RVA: 0x001712ED File Offset: 0x0016F4ED
	public HashSet<Generator> Generators
	{
		get
		{
			return this.generators;
		}
	}

	// Token: 0x06004152 RID: 16722 RVA: 0x001712F5 File Offset: 0x0016F4F5
	public void AddGenerator(Generator generator)
	{
		this.generators.Add(generator);
	}

	// Token: 0x06004153 RID: 16723 RVA: 0x00171304 File Offset: 0x0016F504
	public void RemoveGenerator(Generator generator)
	{
		this.generators.Remove(generator);
	}

	// Token: 0x06004154 RID: 16724 RVA: 0x00171313 File Offset: 0x0016F513
	public void AddManualGenerator(ManualGenerator manual_generator)
	{
		this.manualGenerators.Add(manual_generator);
	}

	// Token: 0x06004155 RID: 16725 RVA: 0x00171322 File Offset: 0x0016F522
	public void RemoveManualGenerator(ManualGenerator manual_generator)
	{
		this.manualGenerators.Remove(manual_generator);
	}

	// Token: 0x06004156 RID: 16726 RVA: 0x00171331 File Offset: 0x0016F531
	public void AddBattery(Battery battery)
	{
		this.batteries.Add(battery);
	}

	// Token: 0x06004157 RID: 16727 RVA: 0x00171340 File Offset: 0x0016F540
	public void RemoveBattery(Battery battery)
	{
		this.batteries.Remove(battery);
	}

	// Token: 0x06004158 RID: 16728 RVA: 0x0017134F File Offset: 0x0016F54F
	public void AddEnergyConsumer(EnergyConsumer energy_consumer)
	{
		this.energyConsumers.Add(energy_consumer);
	}

	// Token: 0x06004159 RID: 16729 RVA: 0x0017135E File Offset: 0x0016F55E
	public void RemoveEnergyConsumer(EnergyConsumer energy_consumer)
	{
		this.energyConsumers.Remove(energy_consumer);
	}

	// Token: 0x0600415A RID: 16730 RVA: 0x00171370 File Offset: 0x0016F570
	public void EnergySim200ms(float dt)
	{
		foreach (Generator generator in this.generators)
		{
			generator.EnergySim200ms(dt);
		}
		foreach (ManualGenerator manualGenerator in this.manualGenerators)
		{
			manualGenerator.EnergySim200ms(dt);
		}
		foreach (Battery battery in this.batteries)
		{
			battery.EnergySim200ms(dt);
		}
		foreach (EnergyConsumer energyConsumer in this.energyConsumers)
		{
			energyConsumer.EnergySim200ms(dt);
		}
	}

	// Token: 0x040028CD RID: 10445
	private HashSet<Generator> generators = new HashSet<Generator>();

	// Token: 0x040028CE RID: 10446
	private HashSet<ManualGenerator> manualGenerators = new HashSet<ManualGenerator>();

	// Token: 0x040028CF RID: 10447
	private HashSet<Battery> batteries = new HashSet<Battery>();

	// Token: 0x040028D0 RID: 10448
	private HashSet<EnergyConsumer> energyConsumers = new HashSet<EnergyConsumer>();
}
