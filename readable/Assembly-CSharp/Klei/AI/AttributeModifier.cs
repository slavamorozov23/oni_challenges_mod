using System;
using System.Diagnostics;

namespace Klei.AI
{
	// Token: 0x02001026 RID: 4134
	[DebuggerDisplay("{AttributeId}")]
	public class AttributeModifier
	{
		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06008051 RID: 32849 RVA: 0x003382AE File Offset: 0x003364AE
		// (set) Token: 0x06008052 RID: 32850 RVA: 0x003382B6 File Offset: 0x003364B6
		public string AttributeId { get; private set; }

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06008053 RID: 32851 RVA: 0x003382BF File Offset: 0x003364BF
		// (set) Token: 0x06008054 RID: 32852 RVA: 0x003382C7 File Offset: 0x003364C7
		public float Value { get; private set; }

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06008055 RID: 32853 RVA: 0x003382D0 File Offset: 0x003364D0
		// (set) Token: 0x06008056 RID: 32854 RVA: 0x003382D8 File Offset: 0x003364D8
		public bool IsMultiplier { get; private set; }

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06008057 RID: 32855 RVA: 0x003382E1 File Offset: 0x003364E1
		// (set) Token: 0x06008058 RID: 32856 RVA: 0x003382E9 File Offset: 0x003364E9
		public GameUtil.TimeSlice? OverrideTimeSlice { get; set; }

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06008059 RID: 32857 RVA: 0x003382F2 File Offset: 0x003364F2
		// (set) Token: 0x0600805A RID: 32858 RVA: 0x003382FA File Offset: 0x003364FA
		public bool UIOnly { get; private set; }

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x0600805B RID: 32859 RVA: 0x00338303 File Offset: 0x00336503
		// (set) Token: 0x0600805C RID: 32860 RVA: 0x0033830B File Offset: 0x0033650B
		public bool IsReadonly { get; private set; }

		// Token: 0x0600805D RID: 32861 RVA: 0x00338314 File Offset: 0x00336514
		public AttributeModifier(string attribute_id, float value, string description = null, bool is_multiplier = false, bool uiOnly = false, bool is_readonly = true)
		{
			this.AttributeId = attribute_id;
			this.Value = value;
			this.Description = ((description == null) ? attribute_id : description);
			this.DescriptionCB = null;
			this.IsMultiplier = is_multiplier;
			this.UIOnly = uiOnly;
			this.IsReadonly = is_readonly;
			this.OverrideTimeSlice = null;
		}

		// Token: 0x0600805E RID: 32862 RVA: 0x00338370 File Offset: 0x00336570
		public AttributeModifier(string attribute_id, float value, Func<string> description_cb, bool is_multiplier = false, bool uiOnly = false) : this(attribute_id, value, null, description_cb, is_multiplier, uiOnly)
		{
		}

		// Token: 0x0600805F RID: 32863 RVA: 0x00338380 File Offset: 0x00336580
		public AttributeModifier(string attribute_id, float value, Func<string> name_cb, Func<string> description_cb, bool is_multiplier = false, bool uiOnly = false)
		{
			this.AttributeId = attribute_id;
			this.Value = value;
			this.NameCB = name_cb;
			this.DescriptionCB = description_cb;
			this.Description = null;
			this.IsMultiplier = is_multiplier;
			this.UIOnly = uiOnly;
			this.OverrideTimeSlice = null;
			if (description_cb == null)
			{
				global::Debug.LogWarning("AttributeModifier being constructed without a description callback: " + attribute_id);
			}
		}

		// Token: 0x06008060 RID: 32864 RVA: 0x003383EA File Offset: 0x003365EA
		public void SetValue(float value)
		{
			this.Value = value;
		}

		// Token: 0x06008061 RID: 32865 RVA: 0x003383F4 File Offset: 0x003365F4
		public static Attribute FetchAttribute(string attributeId)
		{
			Attribute attribute = Db.Get().Attributes.TryGet(attributeId);
			if (attribute != null)
			{
				return attribute;
			}
			Attribute attribute2 = Db.Get().BuildingAttributes.TryGet(attributeId);
			if (attribute2 != null)
			{
				return attribute2;
			}
			Attribute attribute3 = Db.Get().PlantAttributes.TryGet(attributeId);
			if (attribute3 != null)
			{
				return attribute3;
			}
			Attribute attribute4 = Db.Get().CritterAttributes.TryGet(attributeId);
			if (attribute4 != null)
			{
				return attribute4;
			}
			return null;
		}

		// Token: 0x06008062 RID: 32866 RVA: 0x0033845A File Offset: 0x0033665A
		private Attribute FetchAttribute()
		{
			return AttributeModifier.FetchAttribute(this.AttributeId);
		}

		// Token: 0x06008063 RID: 32867 RVA: 0x00338468 File Offset: 0x00336668
		public string GetName()
		{
			Attribute attribute = this.FetchAttribute();
			if (attribute == null || attribute.ShowInUI == Attribute.Display.Never)
			{
				return "";
			}
			if (this.NameCB != null)
			{
				return this.NameCB();
			}
			return attribute.Name;
		}

		// Token: 0x06008064 RID: 32868 RVA: 0x003384A8 File Offset: 0x003366A8
		public string GetDescription()
		{
			if (this.DescriptionCB == null)
			{
				return this.Description;
			}
			return this.DescriptionCB();
		}

		// Token: 0x06008065 RID: 32869 RVA: 0x003384C4 File Offset: 0x003366C4
		public string GetFormattedString()
		{
			Attribute attribute = this.FetchAttribute();
			IAttributeFormatter attributeFormatter = (!this.IsMultiplier && attribute != null) ? attribute.formatter : null;
			string text = "";
			if (attributeFormatter != null)
			{
				text = attributeFormatter.GetFormattedModifier(this);
			}
			else if (this.IsMultiplier)
			{
				text += GameUtil.GetFormattedPercent(this.Value * 100f, GameUtil.TimeSlice.None);
			}
			else
			{
				text += GameUtil.GetFormattedSimple(this.Value, GameUtil.TimeSlice.None, null);
			}
			if (text != null && text.Length > 0 && text[0] != '-')
			{
				GameUtil.TimeSlice? overrideTimeSlice = this.OverrideTimeSlice;
				GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None;
				if (!(overrideTimeSlice.GetValueOrDefault() == timeSlice & overrideTimeSlice != null))
				{
					text = GameUtil.AddPositiveSign(text, this.Value > 0f);
				}
			}
			return text;
		}

		// Token: 0x06008066 RID: 32870 RVA: 0x00338582 File Offset: 0x00336782
		public AttributeModifier Clone()
		{
			return new AttributeModifier(this.AttributeId, this.Value, this.Description, false, false, true);
		}

		// Token: 0x04006144 RID: 24900
		public Func<string> NameCB;

		// Token: 0x04006145 RID: 24901
		public string Description;

		// Token: 0x04006146 RID: 24902
		public Func<string> DescriptionCB;
	}
}
