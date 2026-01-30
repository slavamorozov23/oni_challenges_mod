using System;

// Token: 0x02000D7E RID: 3454
public class SymbolicConsumableItem : IConsumableUIItem
{
	// Token: 0x06006B1A RID: 27418 RVA: 0x00289660 File Offset: 0x00287860
	public SymbolicConsumableItem(string id, string name, int majorOrder, int minorOrder, bool display, string overrideSpriteName, Func<bool> revealTest)
	{
		this.id = id;
		this.name = name;
		this.majorOrder = majorOrder;
		this.minorOrder = minorOrder;
		this.display = display;
		this.overrideSpriteName = overrideSpriteName;
		this.revealTest = revealTest;
	}

	// Token: 0x170007A5 RID: 1957
	// (get) Token: 0x06006B1B RID: 27419 RVA: 0x0028969D File Offset: 0x0028789D
	string IConsumableUIItem.ConsumableId
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x06006B1C RID: 27420 RVA: 0x002896A5 File Offset: 0x002878A5
	string IConsumableUIItem.ConsumableName
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x06006B1D RID: 27421 RVA: 0x002896AD File Offset: 0x002878AD
	int IConsumableUIItem.MajorOrder
	{
		get
		{
			return this.majorOrder;
		}
	}

	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x06006B1E RID: 27422 RVA: 0x002896B5 File Offset: 0x002878B5
	int IConsumableUIItem.MinorOrder
	{
		get
		{
			return this.minorOrder;
		}
	}

	// Token: 0x170007A9 RID: 1961
	// (get) Token: 0x06006B1F RID: 27423 RVA: 0x002896BD File Offset: 0x002878BD
	bool IConsumableUIItem.Display
	{
		get
		{
			return this.display;
		}
	}

	// Token: 0x06006B20 RID: 27424 RVA: 0x002896C5 File Offset: 0x002878C5
	string IConsumableUIItem.OverrideSpriteName()
	{
		return this.overrideSpriteName;
	}

	// Token: 0x06006B21 RID: 27425 RVA: 0x002896CD File Offset: 0x002878CD
	bool IConsumableUIItem.RevealTest()
	{
		return this.revealTest();
	}

	// Token: 0x040049A6 RID: 18854
	private string id;

	// Token: 0x040049A7 RID: 18855
	private string name;

	// Token: 0x040049A8 RID: 18856
	private int majorOrder;

	// Token: 0x040049A9 RID: 18857
	private int minorOrder;

	// Token: 0x040049AA RID: 18858
	private bool display;

	// Token: 0x040049AB RID: 18859
	private string overrideSpriteName;

	// Token: 0x040049AC RID: 18860
	private Func<bool> revealTest;
}
