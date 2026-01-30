using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000948 RID: 2376
public class AttributeModifierExpectation : Expectation
{
	// Token: 0x0600424A RID: 16970 RVA: 0x00175D38 File Offset: 0x00173F38
	public AttributeModifierExpectation(string id, string name, string description, AttributeModifier modifier, Sprite icon) : base(id, name, description, delegate(MinionResume resume)
	{
		resume.GetAttributes().Get(modifier.AttributeId).Add(modifier);
	}, delegate(MinionResume resume)
	{
		resume.GetAttributes().Get(modifier.AttributeId).Remove(modifier);
	})
	{
		this.modifier = modifier;
		this.icon = icon;
	}

	// Token: 0x0400299F RID: 10655
	public AttributeModifier modifier;

	// Token: 0x040029A0 RID: 10656
	public Sprite icon;
}
