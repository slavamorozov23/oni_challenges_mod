using System;
using UnityEngine;

// Token: 0x02000742 RID: 1858
public class DevLifeSupport : KMonoBehaviour, ISim200ms
{
	// Token: 0x06002ED8 RID: 11992 RVA: 0x0010E939 File Offset: 0x0010CB39
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.elementConsumer != null)
		{
			this.elementConsumer.EnableConsumption(true);
		}
	}

	// Token: 0x06002ED9 RID: 11993 RVA: 0x0010E95C File Offset: 0x0010CB5C
	public void Sim200ms(float dt)
	{
		Vector2I vector2I = new Vector2I(-this.effectRadius, -this.effectRadius);
		Vector2I vector2I2 = new Vector2I(this.effectRadius, this.effectRadius);
		int num;
		int num2;
		Grid.PosToXY(base.transform.GetPosition(), out num, out num2);
		int num3 = Grid.XYToCell(num, num2);
		if (Grid.IsValidCell(num3))
		{
			int world = (int)Grid.WorldIdx[num3];
			for (int i = vector2I.y; i <= vector2I2.y; i++)
			{
				for (int j = vector2I.x; j <= vector2I2.x; j++)
				{
					int num4 = Grid.XYToCell(num + j, num2 + i);
					if (Grid.IsValidCellInWorld(num4, world))
					{
						float num5 = (this.targetTemperature - Grid.Temperature[num4]) * Grid.Element[num4].specificHeatCapacity * Grid.Mass[num4];
						if (!Mathf.Approximately(0f, num5))
						{
							SimMessages.ModifyEnergy(num4, num5 * 0.2f, 5000f, (num5 > 0f) ? SimMessages.EnergySourceID.DebugHeat : SimMessages.EnergySourceID.DebugCool);
						}
					}
				}
			}
		}
	}

	// Token: 0x04001BBD RID: 7101
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x04001BBE RID: 7102
	public float targetTemperature = 294.15f;

	// Token: 0x04001BBF RID: 7103
	public int effectRadius = 7;

	// Token: 0x04001BC0 RID: 7104
	private const float temperatureControlK = 0.2f;
}
