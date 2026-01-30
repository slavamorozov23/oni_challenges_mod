using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x0200101F RID: 4127
	public class Attribute : Resource, IHasDlcRestrictions
	{
		// Token: 0x06008013 RID: 32787 RVA: 0x00337444 File Offset: 0x00335644
		public Attribute(string id, bool is_trainable, Attribute.Display show_in_ui, bool is_profession, float base_value = 0f, string uiSprite = null, string thoughtSprite = null, string uiFullColourSprite = null, string[] overrideDLCIDs = null) : base(id, null, null)
		{
			string str = "STRINGS.DUPLICANTS.ATTRIBUTES." + id.ToUpper();
			this.Name = Strings.Get(new StringKey(str + ".NAME"));
			this.ProfessionName = Strings.Get(new StringKey(str + ".NAME"));
			this.Description = Strings.Get(new StringKey(str + ".DESC"));
			this.IsTrainable = is_trainable;
			this.IsProfession = is_profession;
			this.ShowInUI = show_in_ui;
			this.BaseValue = base_value;
			this.formatter = Attribute.defaultFormatter;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
			this.uiFullColourSprite = uiFullColourSprite;
			this.requiredDlcIds = overrideDLCIDs;
		}

		// Token: 0x06008014 RID: 32788 RVA: 0x00337520 File Offset: 0x00335720
		public Attribute(string id, string name, string profession_name, string attribute_description, float base_value, Attribute.Display show_in_ui, bool is_trainable, string uiSprite = null, string thoughtSprite = null, string uiFullColourSprite = null) : base(id, name)
		{
			this.Description = attribute_description;
			this.ProfessionName = profession_name;
			this.BaseValue = base_value;
			this.ShowInUI = show_in_ui;
			this.IsTrainable = is_trainable;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
			this.uiFullColourSprite = uiFullColourSprite;
			if (this.ProfessionName == "")
			{
				this.ProfessionName = null;
			}
		}

		// Token: 0x06008015 RID: 32789 RVA: 0x00337598 File Offset: 0x00335798
		public void SetFormatter(IAttributeFormatter formatter)
		{
			this.formatter = formatter;
		}

		// Token: 0x06008016 RID: 32790 RVA: 0x003375A1 File Offset: 0x003357A1
		public AttributeInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x06008017 RID: 32791 RVA: 0x003375B0 File Offset: 0x003357B0
		public AttributeInstance Lookup(GameObject go)
		{
			Attributes attributes = go.GetAttributes();
			if (attributes != null)
			{
				return attributes.Get(this);
			}
			return null;
		}

		// Token: 0x06008018 RID: 32792 RVA: 0x003375D0 File Offset: 0x003357D0
		public string GetDescription(AttributeInstance instance)
		{
			return instance.GetDescription();
		}

		// Token: 0x06008019 RID: 32793 RVA: 0x003375D8 File Offset: 0x003357D8
		public string GetTooltip(AttributeInstance instance)
		{
			return this.formatter.GetTooltip(this, instance);
		}

		// Token: 0x0600801A RID: 32794 RVA: 0x003375E7 File Offset: 0x003357E7
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600801B RID: 32795 RVA: 0x003375EF File Offset: 0x003357EF
		public string[] GetForbiddenDlcIds()
		{
			return null;
		}

		// Token: 0x0400611B RID: 24859
		private static readonly StandardAttributeFormatter defaultFormatter = new StandardAttributeFormatter(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None);

		// Token: 0x0400611C RID: 24860
		public string Description;

		// Token: 0x0400611D RID: 24861
		public float BaseValue;

		// Token: 0x0400611E RID: 24862
		public Attribute.Display ShowInUI;

		// Token: 0x0400611F RID: 24863
		public bool IsTrainable;

		// Token: 0x04006120 RID: 24864
		public bool IsProfession;

		// Token: 0x04006121 RID: 24865
		public string ProfessionName;

		// Token: 0x04006122 RID: 24866
		public List<AttributeConverter> converters = new List<AttributeConverter>();

		// Token: 0x04006123 RID: 24867
		public string uiSprite;

		// Token: 0x04006124 RID: 24868
		public string thoughtSprite;

		// Token: 0x04006125 RID: 24869
		public string uiFullColourSprite;

		// Token: 0x04006126 RID: 24870
		public string[] requiredDlcIds;

		// Token: 0x04006127 RID: 24871
		public string[] forbiddenDlcIds;

		// Token: 0x04006128 RID: 24872
		public IAttributeFormatter formatter;

		// Token: 0x02002724 RID: 10020
		public enum Display
		{
			// Token: 0x0400AE5E RID: 44638
			Normal,
			// Token: 0x0400AE5F RID: 44639
			Skill,
			// Token: 0x0400AE60 RID: 44640
			Expectation,
			// Token: 0x0400AE61 RID: 44641
			General,
			// Token: 0x0400AE62 RID: 44642
			Details,
			// Token: 0x0400AE63 RID: 44643
			Never
		}
	}
}
