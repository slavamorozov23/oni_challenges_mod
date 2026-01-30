using System;
using Klei.AI;
using STRINGS;

namespace Database
{
	// Token: 0x02000FAC RID: 4012
	public class SkillAttributePerk : SkillPerk
	{
		// Token: 0x06007E37 RID: 32311 RVA: 0x0032291C File Offset: 0x00320B1C
		public SkillAttributePerk(string id, string attributeId, float modifierBonus, string modifierDesc, bool modifierCanStack = false) : base(id, "", null, null, delegate(MinionResume identity)
		{
		}, null, false)
		{
			SkillAttributePerk <>4__this = this;
			Klei.AI.Attribute attribute = Db.Get().Attributes.Get(attributeId);
			this.modifier = new AttributeModifier(attributeId, modifierBonus, modifierDesc, false, false, true);
			this.Name = string.Format(UI.ROLES_SCREEN.PERKS.ATTRIBUTE_EFFECT_FMT, this.modifier.GetFormattedString(), attribute.Name);
			Predicate<AttributeModifier> <>9__3;
			base.OnApply = delegate(MinionResume identity)
			{
				if (!modifierCanStack)
				{
					AttributeInstance attributeInstance = identity.GetAttributes().Get(<>4__this.modifier.AttributeId);
					Predicate<AttributeModifier> match;
					if ((match = <>9__3) == null)
					{
						match = (<>9__3 = ((AttributeModifier mod) => mod == <>4__this.modifier));
					}
					if (attributeInstance.Modifiers.FindIndex(match) != -1)
					{
						return;
					}
				}
				identity.GetAttributes().Add(<>4__this.modifier);
			};
			base.OnRemove = delegate(MinionResume identity)
			{
				identity.GetAttributes().Remove(<>4__this.modifier);
			};
		}

		// Token: 0x04005CA7 RID: 23719
		public AttributeModifier modifier;
	}
}
