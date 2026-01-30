using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x0200104E RID: 4174
	public class Modifier : Resource
	{
		// Token: 0x06008145 RID: 33093 RVA: 0x0033E8F0 File Offset: 0x0033CAF0
		public Modifier(string id, string name, string description) : base(id, name)
		{
			this.description = description;
		}

		// Token: 0x06008146 RID: 33094 RVA: 0x0033E90C File Offset: 0x0033CB0C
		public void Add(AttributeModifier modifier)
		{
			if (modifier.AttributeId != "")
			{
				this.SelfModifiers.Add(modifier);
			}
		}

		// Token: 0x06008147 RID: 33095 RVA: 0x0033E92C File Offset: 0x0033CB2C
		public virtual void AddTo(Attributes attributes)
		{
			foreach (AttributeModifier modifier in this.SelfModifiers)
			{
				attributes.Add(modifier);
			}
		}

		// Token: 0x06008148 RID: 33096 RVA: 0x0033E980 File Offset: 0x0033CB80
		public virtual void RemoveFrom(Attributes attributes)
		{
			foreach (AttributeModifier modifier in this.SelfModifiers)
			{
				attributes.Remove(modifier);
			}
		}

		// Token: 0x040061EF RID: 25071
		public string description;

		// Token: 0x040061F0 RID: 25072
		public List<AttributeModifier> SelfModifiers = new List<AttributeModifier>();
	}
}
