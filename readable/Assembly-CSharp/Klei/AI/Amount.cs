using System;
using System.Diagnostics;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x0200101C RID: 4124
	[DebuggerDisplay("{Id}")]
	public class Amount : Resource
	{
		// Token: 0x06007FF3 RID: 32755 RVA: 0x003370A8 File Offset: 0x003352A8
		public Amount(string id, string name, string description, Attribute min_attribute, Attribute max_attribute, Attribute delta_attribute, bool show_max, Units units, float visual_delta_threshold, bool show_in_ui, string uiSprite = null, string thoughtSprite = null)
		{
			this.Id = id;
			this.Name = name;
			this.description = description;
			this.minAttribute = min_attribute;
			this.maxAttribute = max_attribute;
			this.deltaAttribute = delta_attribute;
			this.showMax = show_max;
			this.units = units;
			this.visualDeltaThreshold = visual_delta_threshold;
			this.showInUI = show_in_ui;
			this.uiSprite = uiSprite;
			this.thoughtSprite = thoughtSprite;
		}

		// Token: 0x06007FF4 RID: 32756 RVA: 0x00337118 File Offset: 0x00335318
		public void SetDisplayer(IAmountDisplayer displayer)
		{
			this.displayer = displayer;
			this.minAttribute.SetFormatter(displayer.Formatter);
			this.maxAttribute.SetFormatter(displayer.Formatter);
			this.deltaAttribute.SetFormatter(displayer.Formatter);
		}

		// Token: 0x06007FF5 RID: 32757 RVA: 0x00337154 File Offset: 0x00335354
		public AmountInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x06007FF6 RID: 32758 RVA: 0x00337164 File Offset: 0x00335364
		public AmountInstance Lookup(GameObject go)
		{
			Amounts amounts = go.GetAmounts();
			if (amounts != null)
			{
				return amounts.Get(this);
			}
			return null;
		}

		// Token: 0x06007FF7 RID: 32759 RVA: 0x00337184 File Offset: 0x00335384
		public void Copy(GameObject to, GameObject from)
		{
			AmountInstance amountInstance = this.Lookup(to);
			AmountInstance amountInstance2 = this.Lookup(from);
			amountInstance.value = amountInstance2.value;
		}

		// Token: 0x06007FF8 RID: 32760 RVA: 0x003371AB File Offset: 0x003353AB
		public string GetValueString(AmountInstance instance)
		{
			return this.displayer.GetValueString(this, instance);
		}

		// Token: 0x06007FF9 RID: 32761 RVA: 0x003371BA File Offset: 0x003353BA
		public string GetDescription(AmountInstance instance)
		{
			return this.displayer.GetDescription(this, instance);
		}

		// Token: 0x06007FFA RID: 32762 RVA: 0x003371C9 File Offset: 0x003353C9
		public string GetTooltip(AmountInstance instance)
		{
			return this.displayer.GetTooltip(this, instance);
		}

		// Token: 0x06007FFB RID: 32763 RVA: 0x003371D8 File Offset: 0x003353D8
		public void DebugSetValue(AmountInstance instance, float value)
		{
			if (this.debugSetValue != null)
			{
				this.debugSetValue(instance, value);
				return;
			}
			instance.SetValue(value);
		}

		// Token: 0x04006107 RID: 24839
		public string description;

		// Token: 0x04006108 RID: 24840
		public bool showMax;

		// Token: 0x04006109 RID: 24841
		public Units units;

		// Token: 0x0400610A RID: 24842
		public float visualDeltaThreshold;

		// Token: 0x0400610B RID: 24843
		public Attribute minAttribute;

		// Token: 0x0400610C RID: 24844
		public Attribute maxAttribute;

		// Token: 0x0400610D RID: 24845
		public Attribute deltaAttribute;

		// Token: 0x0400610E RID: 24846
		public Action<AmountInstance, float> debugSetValue;

		// Token: 0x0400610F RID: 24847
		public bool showInUI;

		// Token: 0x04006110 RID: 24848
		public string uiSprite;

		// Token: 0x04006111 RID: 24849
		public string thoughtSprite;

		// Token: 0x04006112 RID: 24850
		public IAmountDisplayer displayer;
	}
}
