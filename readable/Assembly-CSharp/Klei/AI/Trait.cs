using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02001057 RID: 4183
	public class Trait : Modifier, IHasDlcRestrictions
	{
		// Token: 0x06008183 RID: 33155 RVA: 0x0033F3DE File Offset: 0x0033D5DE
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x06008184 RID: 33156 RVA: 0x0033F3E6 File Offset: 0x0033D5E6
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x06008185 RID: 33157 RVA: 0x0033F3F0 File Offset: 0x0033D5F0
		public Trait(string id, string name, string description, float rating, bool should_save, ChoreGroup[] disallowed_chore_groups, bool positive_trait, bool is_valid_starter_trait) : base(id, name, description)
		{
			this.Rating = rating;
			this.ShouldSave = should_save;
			this.disabledChoreGroups = disallowed_chore_groups;
			this.PositiveTrait = positive_trait;
			this.ValidStarterTrait = is_valid_starter_trait;
			this.ignoredEffects = new string[0];
			this.requiredDlcIds = null;
			this.forbiddenDlcIds = null;
		}

		// Token: 0x06008186 RID: 33158 RVA: 0x0033F448 File Offset: 0x0033D648
		public Trait(string id, string name, string description, float rating, bool should_save, ChoreGroup[] disallowed_chore_groups, bool positive_trait, bool is_valid_starter_trait, string[] requiredDlcIds, string[] forbiddenDlcIds) : base(id, name, description)
		{
			this.Rating = rating;
			this.ShouldSave = should_save;
			this.disabledChoreGroups = disallowed_chore_groups;
			this.PositiveTrait = positive_trait;
			this.ValidStarterTrait = is_valid_starter_trait;
			this.ignoredEffects = new string[0];
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
		}

		// Token: 0x06008187 RID: 33159 RVA: 0x0033F4A4 File Offset: 0x0033D6A4
		public void AddIgnoredEffects(string[] effects)
		{
			List<string> list = new List<string>(this.ignoredEffects);
			list.AddRange(effects);
			this.ignoredEffects = list.ToArray();
		}

		// Token: 0x06008188 RID: 33160 RVA: 0x0033F4D0 File Offset: 0x0033D6D0
		public string GetName()
		{
			if (this.NameCB != null)
			{
				return this.NameCB();
			}
			return this.Name;
		}

		// Token: 0x06008189 RID: 33161 RVA: 0x0033F4EC File Offset: 0x0033D6EC
		public string GetTooltip()
		{
			string text;
			if (this.TooltipCB != null)
			{
				text = this.TooltipCB();
			}
			else
			{
				text = this.description;
				text += this.GetAttributeModifiersString(true);
				text += this.GetDisabledChoresString(true);
				text += this.GetIgnoredEffectsString(true);
				text += this.GetExtendedTooltipStr();
			}
			return text;
		}

		// Token: 0x0600818A RID: 33162 RVA: 0x0033F550 File Offset: 0x0033D750
		public string GetAttributeModifiersString(bool list_entry)
		{
			string text = "";
			foreach (AttributeModifier attributeModifier in this.SelfModifiers)
			{
				Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
				if (list_entry)
				{
					text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
				}
				text += string.Format(DUPLICANTS.TRAITS.ATTRIBUTE_MODIFIERS, attribute.Name, attributeModifier.GetFormattedString());
			}
			return text;
		}

		// Token: 0x0600818B RID: 33163 RVA: 0x0033F5F0 File Offset: 0x0033D7F0
		public string GetDisabledChoresString(bool list_entry)
		{
			string text = "";
			if (this.disabledChoreGroups != null)
			{
				string format = DUPLICANTS.TRAITS.CANNOT_DO_TASK;
				if (this.isTaskBeingRefused)
				{
					format = DUPLICANTS.TRAITS.REFUSES_TO_DO_TASK;
				}
				foreach (ChoreGroup choreGroup in this.disabledChoreGroups)
				{
					if (list_entry)
					{
						text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
					}
					text += string.Format(format, choreGroup.Name);
				}
			}
			return text;
		}

		// Token: 0x0600818C RID: 33164 RVA: 0x0033F66C File Offset: 0x0033D86C
		public string GetIgnoredEffectsString(bool list_entry)
		{
			string text = "";
			if (this.ignoredEffects != null && this.ignoredEffects.Length != 0)
			{
				for (int i = 0; i < this.ignoredEffects.Length; i++)
				{
					string text2 = this.ignoredEffects[i];
					if (list_entry)
					{
						text += DUPLICANTS.TRAITS.TRAIT_DESCRIPTION_LIST_ENTRY;
					}
					string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text2.ToUpper() + ".NAME");
					text += string.Format(DUPLICANTS.TRAITS.IGNORED_EFFECTS, arg);
					if (!list_entry && i < this.ignoredEffects.Length - 1)
					{
						text += "\n";
					}
				}
			}
			return text;
		}

		// Token: 0x0600818D RID: 33165 RVA: 0x0033F71C File Offset: 0x0033D91C
		public string GetExtendedTooltipStr()
		{
			string text = "";
			if (this.ExtendedTooltip != null)
			{
				foreach (Func<string> func in this.ExtendedTooltip.GetInvocationList())
				{
					text = text + "\n" + func();
				}
			}
			return text;
		}

		// Token: 0x0600818E RID: 33166 RVA: 0x0033F770 File Offset: 0x0033D970
		public override void AddTo(Attributes attributes)
		{
			base.AddTo(attributes);
			ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
			if (component != null && this.disabledChoreGroups != null)
			{
				foreach (ChoreGroup chore_group in this.disabledChoreGroups)
				{
					component.SetPermittedByTraits(chore_group, false);
				}
			}
		}

		// Token: 0x0600818F RID: 33167 RVA: 0x0033F7C4 File Offset: 0x0033D9C4
		public override void RemoveFrom(Attributes attributes)
		{
			base.RemoveFrom(attributes);
			ChoreConsumer component = attributes.gameObject.GetComponent<ChoreConsumer>();
			if (component != null && this.disabledChoreGroups != null)
			{
				foreach (ChoreGroup chore_group in this.disabledChoreGroups)
				{
					component.SetPermittedByTraits(chore_group, true);
				}
			}
		}

		// Token: 0x04006202 RID: 25090
		public float Rating;

		// Token: 0x04006203 RID: 25091
		public bool ShouldSave;

		// Token: 0x04006204 RID: 25092
		public bool PositiveTrait;

		// Token: 0x04006205 RID: 25093
		public bool ValidStarterTrait;

		// Token: 0x04006206 RID: 25094
		public Action<GameObject> OnAddTrait;

		// Token: 0x04006207 RID: 25095
		public Func<string> TooltipCB;

		// Token: 0x04006208 RID: 25096
		public Func<string> ExtendedTooltip;

		// Token: 0x04006209 RID: 25097
		public Func<string> ShortDescCB;

		// Token: 0x0400620A RID: 25098
		public Func<string> ShortDescTooltipCB;

		// Token: 0x0400620B RID: 25099
		public Func<string> NameCB;

		// Token: 0x0400620C RID: 25100
		public ChoreGroup[] disabledChoreGroups;

		// Token: 0x0400620D RID: 25101
		public bool isTaskBeingRefused;

		// Token: 0x0400620E RID: 25102
		public string[] ignoredEffects;

		// Token: 0x0400620F RID: 25103
		public string[] requiredDlcIds;

		// Token: 0x04006210 RID: 25104
		public string[] forbiddenDlcIds;
	}
}
