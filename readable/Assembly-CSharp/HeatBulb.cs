using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000A9E RID: 2718
[AddComponentMenu("KMonoBehaviour/scripts/HeatBulb")]
public class HeatBulb : KMonoBehaviour, ISim200ms
{
	// Token: 0x06004EDC RID: 20188 RVA: 0x001CA465 File Offset: 0x001C8665
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.kanim.Play("off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06004EDD RID: 20189 RVA: 0x001CA490 File Offset: 0x001C8690
	public void Sim200ms(float dt)
	{
		float num = this.kjConsumptionRate * dt;
		Vector2I vector2I = this.maxCheckOffset - this.minCheckOffset + 1;
		int num2 = vector2I.x * vector2I.y;
		float num3 = num / (float)num2;
		int num4;
		int num5;
		Grid.PosToXY(base.transform.GetPosition(), out num4, out num5);
		for (int i = this.minCheckOffset.y; i <= this.maxCheckOffset.y; i++)
		{
			for (int j = this.minCheckOffset.x; j <= this.maxCheckOffset.x; j++)
			{
				int num6 = Grid.XYToCell(num4 + j, num5 + i);
				if (Grid.IsValidCell(num6) && Grid.Temperature[num6] > this.minTemperature)
				{
					this.kjConsumed += num3;
					SimMessages.ModifyEnergy(num6, -num3, 5000f, SimMessages.EnergySourceID.HeatBulb);
				}
			}
		}
		float num7 = this.lightKJConsumptionRate * dt;
		if (this.kjConsumed > num7)
		{
			if (!this.lightSource.enabled)
			{
				this.kanim.Play("open", KAnim.PlayMode.Once, 1f, 0f);
				this.kanim.Queue("on", KAnim.PlayMode.Once, 1f, 0f);
				this.lightSource.enabled = true;
			}
			this.kjConsumed -= num7;
			return;
		}
		if (this.lightSource.enabled)
		{
			this.kanim.Play("close", KAnim.PlayMode.Once, 1f, 0f);
			this.kanim.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.lightSource.enabled = false;
	}

	// Token: 0x040034AA RID: 13482
	[SerializeField]
	private float minTemperature;

	// Token: 0x040034AB RID: 13483
	[SerializeField]
	private float kjConsumptionRate;

	// Token: 0x040034AC RID: 13484
	[SerializeField]
	private float lightKJConsumptionRate;

	// Token: 0x040034AD RID: 13485
	[SerializeField]
	private Vector2I minCheckOffset;

	// Token: 0x040034AE RID: 13486
	[SerializeField]
	private Vector2I maxCheckOffset;

	// Token: 0x040034AF RID: 13487
	[MyCmpGet]
	private Light2D lightSource;

	// Token: 0x040034B0 RID: 13488
	[MyCmpGet]
	private KBatchedAnimController kanim;

	// Token: 0x040034B1 RID: 13489
	[Serialize]
	private float kjConsumed;
}
