using System;
using UnityEngine;

// Token: 0x02000E33 RID: 3635
public class CritterSensorSideScreen : SideScreenContent
{
	// Token: 0x06007364 RID: 29540 RVA: 0x002C130A File Offset: 0x002BF50A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.countCrittersToggle.onClick += this.ToggleCritters;
		this.countEggsToggle.onClick += this.ToggleEggs;
	}

	// Token: 0x06007365 RID: 29541 RVA: 0x002C1340 File Offset: 0x002BF540
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicCritterCountSensor>() != null;
	}

	// Token: 0x06007366 RID: 29542 RVA: 0x002C1350 File Offset: 0x002BF550
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetSensor = target.GetComponent<LogicCritterCountSensor>();
		this.crittersCheckmark.enabled = this.targetSensor.countCritters;
		this.eggsCheckmark.enabled = this.targetSensor.countEggs;
	}

	// Token: 0x06007367 RID: 29543 RVA: 0x002C139C File Offset: 0x002BF59C
	private void ToggleCritters()
	{
		this.targetSensor.countCritters = !this.targetSensor.countCritters;
		this.crittersCheckmark.enabled = this.targetSensor.countCritters;
	}

	// Token: 0x06007368 RID: 29544 RVA: 0x002C13CD File Offset: 0x002BF5CD
	private void ToggleEggs()
	{
		this.targetSensor.countEggs = !this.targetSensor.countEggs;
		this.eggsCheckmark.enabled = this.targetSensor.countEggs;
	}

	// Token: 0x04004FCD RID: 20429
	public LogicCritterCountSensor targetSensor;

	// Token: 0x04004FCE RID: 20430
	public KToggle countCrittersToggle;

	// Token: 0x04004FCF RID: 20431
	public KToggle countEggsToggle;

	// Token: 0x04004FD0 RID: 20432
	public KImage crittersCheckmark;

	// Token: 0x04004FD1 RID: 20433
	public KImage eggsCheckmark;
}
