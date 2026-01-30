using System;
using Klei.AI;

// Token: 0x02000A62 RID: 2658
public abstract class Need : KMonoBehaviour
{
	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x06004D5F RID: 19807 RVA: 0x001C2763 File Offset: 0x001C0963
	// (set) Token: 0x06004D60 RID: 19808 RVA: 0x001C276B File Offset: 0x001C096B
	public string Name { get; protected set; }

	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x06004D61 RID: 19809 RVA: 0x001C2774 File Offset: 0x001C0974
	// (set) Token: 0x06004D62 RID: 19810 RVA: 0x001C277C File Offset: 0x001C097C
	public string ExpectationTooltip { get; protected set; }

	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x06004D63 RID: 19811 RVA: 0x001C2785 File Offset: 0x001C0985
	// (set) Token: 0x06004D64 RID: 19812 RVA: 0x001C278D File Offset: 0x001C098D
	public string Tooltip { get; protected set; }

	// Token: 0x06004D65 RID: 19813 RVA: 0x001C2796 File Offset: 0x001C0996
	public Klei.AI.Attribute GetExpectationAttribute()
	{
		return this.expectationAttribute.Attribute;
	}

	// Token: 0x06004D66 RID: 19814 RVA: 0x001C27A3 File Offset: 0x001C09A3
	protected void SetModifier(Need.ModifierType modifier)
	{
		if (this.currentStressModifier != modifier)
		{
			if (this.currentStressModifier != null)
			{
				this.UnapplyModifier(this.currentStressModifier);
			}
			if (modifier != null)
			{
				this.ApplyModifier(modifier);
			}
			this.currentStressModifier = modifier;
		}
	}

	// Token: 0x06004D67 RID: 19815 RVA: 0x001C27D4 File Offset: 0x001C09D4
	private void ApplyModifier(Need.ModifierType modifier)
	{
		if (modifier.modifier != null)
		{
			this.GetAttributes().Add(modifier.modifier);
		}
		if (modifier.statusItem != null)
		{
			base.GetComponent<KSelectable>().AddStatusItem(modifier.statusItem, null);
		}
		if (modifier.thought != null)
		{
			this.GetSMI<ThoughtGraph.Instance>().AddThought(modifier.thought);
		}
	}

	// Token: 0x06004D68 RID: 19816 RVA: 0x001C2830 File Offset: 0x001C0A30
	private void UnapplyModifier(Need.ModifierType modifier)
	{
		if (modifier.modifier != null)
		{
			this.GetAttributes().Remove(modifier.modifier);
		}
		if (modifier.statusItem != null)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(modifier.statusItem, false);
		}
		if (modifier.thought != null)
		{
			this.GetSMI<ThoughtGraph.Instance>().RemoveThought(modifier.thought);
		}
	}

	// Token: 0x040033A4 RID: 13220
	protected AttributeInstance expectationAttribute;

	// Token: 0x040033A5 RID: 13221
	protected Need.ModifierType stressBonus;

	// Token: 0x040033A6 RID: 13222
	protected Need.ModifierType stressNeutral;

	// Token: 0x040033A7 RID: 13223
	protected Need.ModifierType stressPenalty;

	// Token: 0x040033A8 RID: 13224
	protected Need.ModifierType currentStressModifier;

	// Token: 0x02001B80 RID: 7040
	protected class ModifierType
	{
		// Token: 0x04008536 RID: 34102
		public AttributeModifier modifier;

		// Token: 0x04008537 RID: 34103
		public StatusItem statusItem;

		// Token: 0x04008538 RID: 34104
		public Thought thought;
	}
}
