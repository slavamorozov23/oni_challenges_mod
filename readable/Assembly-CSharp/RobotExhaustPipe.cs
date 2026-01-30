using System;
using TUNING;
using UnityEngine;

// Token: 0x02000B13 RID: 2835
[AddComponentMenu("KMonoBehaviour/scripts/RobotExhaustPipe")]
public class RobotExhaustPipe : KMonoBehaviour, ISim4000ms
{
	// Token: 0x060052A4 RID: 21156 RVA: 0x001E0EB0 File Offset: 0x001DF0B0
	public void Sim4000ms(float dt)
	{
		Facing component = base.GetComponent<Facing>();
		bool flip = false;
		if (component)
		{
			flip = component.GetFacing();
		}
		CO2Manager.instance.SpawnBreath(Grid.CellToPos(Grid.PosToCell(base.gameObject)), dt * this.CO2_RATE, 303.15f, flip);
	}

	// Token: 0x040037D4 RID: 14292
	private float CO2_RATE = DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_TO_CO2_CONVERSION / 2f;
}
