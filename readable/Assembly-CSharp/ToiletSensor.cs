using System;

// Token: 0x0200052A RID: 1322
public class ToiletSensor : Sensor
{
	// Token: 0x06001C91 RID: 7313 RVA: 0x0009CCBF File Offset: 0x0009AEBF
	public ToiletSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
	}

	// Token: 0x06001C92 RID: 7314 RVA: 0x0009CCD4 File Offset: 0x0009AED4
	public override void Update()
	{
		IUsable usable = null;
		int num = int.MaxValue;
		bool flag = false;
		foreach (IUsable usable2 in Components.Toilets.Items)
		{
			if (usable2.IsUsable())
			{
				flag = true;
				int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(usable2.transform.GetPosition()));
				if (navigationCost != -1 && navigationCost < num)
				{
					usable = usable2;
					num = navigationCost;
				}
			}
		}
		bool flag2 = Components.Toilets.Count > 0;
		if (usable != this.toilet || flag2 != this.areThereAnyToilets || this.areThereAnyUsableToilets != flag)
		{
			this.toilet = usable;
			this.areThereAnyToilets = flag2;
			this.areThereAnyUsableToilets = flag;
			base.Trigger(-752545459, null);
		}
	}

	// Token: 0x06001C93 RID: 7315 RVA: 0x0009CDB4 File Offset: 0x0009AFB4
	public bool AreThereAnyToilets()
	{
		return this.areThereAnyToilets;
	}

	// Token: 0x06001C94 RID: 7316 RVA: 0x0009CDBC File Offset: 0x0009AFBC
	public bool AreThereAnyUsableToilets()
	{
		return this.areThereAnyUsableToilets;
	}

	// Token: 0x06001C95 RID: 7317 RVA: 0x0009CDC4 File Offset: 0x0009AFC4
	public IUsable GetNearestUsableToilet()
	{
		return this.toilet;
	}

	// Token: 0x040010D5 RID: 4309
	private Navigator navigator;

	// Token: 0x040010D6 RID: 4310
	private IUsable toilet;

	// Token: 0x040010D7 RID: 4311
	private bool areThereAnyToilets;

	// Token: 0x040010D8 RID: 4312
	private bool areThereAnyUsableToilets;
}
