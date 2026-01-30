using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x0200101D RID: 4125
	[SerializationConfig(MemberSerialization.OptIn)]
	[DebuggerDisplay("{amount.Name} {value} ({deltaAttribute.value}/{minAttribute.value}/{maxAttribute.value})")]
	public class AmountInstance : ModifierInstance<Amount>, ISaveLoadable, ISim200ms
	{
		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06007FFC RID: 32764 RVA: 0x003371F8 File Offset: 0x003353F8
		public Amount amount
		{
			get
			{
				return this.modifier;
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06007FFD RID: 32765 RVA: 0x00337200 File Offset: 0x00335400
		// (set) Token: 0x06007FFE RID: 32766 RVA: 0x00337208 File Offset: 0x00335408
		public bool paused
		{
			get
			{
				return this._paused;
			}
			set
			{
				this._paused = this.paused;
				if (this._paused)
				{
					this.Deactivate();
					return;
				}
				this.Activate();
			}
		}

		// Token: 0x06007FFF RID: 32767 RVA: 0x0033722B File Offset: 0x0033542B
		public float GetMin()
		{
			return this.minAttribute.GetTotalValue();
		}

		// Token: 0x06008000 RID: 32768 RVA: 0x00337238 File Offset: 0x00335438
		public float GetMax()
		{
			return this.maxAttribute.GetTotalValue();
		}

		// Token: 0x06008001 RID: 32769 RVA: 0x00337245 File Offset: 0x00335445
		public float GetDelta()
		{
			return this.deltaAttribute.GetTotalValue();
		}

		// Token: 0x06008002 RID: 32770 RVA: 0x00337254 File Offset: 0x00335454
		public AmountInstance(Amount amount, GameObject game_object) : base(game_object, amount)
		{
			Attributes attributes = game_object.GetAttributes();
			this.minAttribute = attributes.Add(amount.minAttribute);
			this.maxAttribute = attributes.Add(amount.maxAttribute);
			this.deltaAttribute = attributes.Add(amount.deltaAttribute);
		}

		// Token: 0x06008003 RID: 32771 RVA: 0x003372A6 File Offset: 0x003354A6
		public float SetValue(float value)
		{
			this.value = Mathf.Min(Mathf.Max(value, this.GetMin()), this.GetMax());
			return this.value;
		}

		// Token: 0x06008004 RID: 32772 RVA: 0x003372CB File Offset: 0x003354CB
		public void Publish(float delta, float previous_value)
		{
			if (this.OnDelta != null)
			{
				this.OnDelta(delta);
			}
			if (this.OnMaxValueReached != null && previous_value < this.GetMax() && this.value >= this.GetMax())
			{
				this.OnMaxValueReached();
			}
		}

		// Token: 0x06008005 RID: 32773 RVA: 0x0033730C File Offset: 0x0033550C
		public float ApplyDelta(float delta)
		{
			float previous_value = this.value;
			this.SetValue(this.value + delta);
			this.Publish(delta, previous_value);
			return this.value;
		}

		// Token: 0x06008006 RID: 32774 RVA: 0x0033733D File Offset: 0x0033553D
		public string GetValueString()
		{
			return this.amount.GetValueString(this);
		}

		// Token: 0x06008007 RID: 32775 RVA: 0x0033734B File Offset: 0x0033554B
		public string GetDescription()
		{
			return this.amount.GetDescription(this);
		}

		// Token: 0x06008008 RID: 32776 RVA: 0x00337359 File Offset: 0x00335559
		public string GetTooltip()
		{
			return this.amount.GetTooltip(this);
		}

		// Token: 0x06008009 RID: 32777 RVA: 0x00337367 File Offset: 0x00335567
		public void Activate()
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}

		// Token: 0x0600800A RID: 32778 RVA: 0x00337375 File Offset: 0x00335575
		public void Sim200ms(float dt)
		{
		}

		// Token: 0x0600800B RID: 32779 RVA: 0x00337378 File Offset: 0x00335578
		public static void BatchUpdate(List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances, float time_delta)
		{
			if (time_delta == 0f)
			{
				return;
			}
			AmountInstance.BatchUpdateContext context = new AmountInstance.BatchUpdateContext(amount_instances, time_delta);
			AmountInstance.AmmountInstanceBatchUpdateDispatcher.Instance.Reset(context);
			GlobalJobManager.Run(AmountInstance.AmmountInstanceBatchUpdateDispatcher.Instance);
			AmountInstance.AmmountInstanceBatchUpdateDispatcher.Instance.Finish();
			AmountInstance.AmmountInstanceBatchUpdateDispatcher.Instance.Reset(AmountInstance.BatchUpdateContext.EmptyContext);
		}

		// Token: 0x0600800C RID: 32780 RVA: 0x003373C5 File Offset: 0x003355C5
		public void Deactivate()
		{
			SimAndRenderScheduler.instance.Remove(this);
		}

		// Token: 0x04006113 RID: 24851
		[Serialize]
		public float value;

		// Token: 0x04006114 RID: 24852
		public AttributeInstance minAttribute;

		// Token: 0x04006115 RID: 24853
		public AttributeInstance maxAttribute;

		// Token: 0x04006116 RID: 24854
		public AttributeInstance deltaAttribute;

		// Token: 0x04006117 RID: 24855
		public Action<float> OnDelta;

		// Token: 0x04006118 RID: 24856
		public System.Action OnMaxValueReached;

		// Token: 0x04006119 RID: 24857
		public bool hide;

		// Token: 0x0400611A RID: 24858
		private bool _paused;

		// Token: 0x02002722 RID: 10018
		private struct BatchUpdateContext
		{
			// Token: 0x0600C7FC RID: 51196 RVA: 0x00424DED File Offset: 0x00422FED
			public BatchUpdateContext(List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances, float time_delta)
			{
				this.amount_instances = amount_instances;
				this.time_delta = time_delta;
			}

			// Token: 0x0400AE58 RID: 44632
			public List<UpdateBucketWithUpdater<ISim200ms>.Entry> amount_instances;

			// Token: 0x0400AE59 RID: 44633
			public float time_delta;

			// Token: 0x0400AE5A RID: 44634
			public static AmountInstance.BatchUpdateContext EmptyContext = new AmountInstance.BatchUpdateContext(null, 0f);

			// Token: 0x02003A1C RID: 14876
			public struct Result
			{
				// Token: 0x0400EB08 RID: 60168
				public AmountInstance amount_instance;

				// Token: 0x0400EB09 RID: 60169
				public float previous;

				// Token: 0x0400EB0A RID: 60170
				public float delta;
			}
		}

		// Token: 0x02002723 RID: 10019
		private class AmmountInstanceBatchUpdateDispatcher : WorkItemCollectionWithThreadContex<AmountInstance.BatchUpdateContext, List<AmountInstance.BatchUpdateContext.Result>>
		{
			// Token: 0x17000D5A RID: 3418
			// (get) Token: 0x0600C7FE RID: 51198 RVA: 0x00424E0F File Offset: 0x0042300F
			public static AmountInstance.AmmountInstanceBatchUpdateDispatcher Instance
			{
				get
				{
					if (AmountInstance.AmmountInstanceBatchUpdateDispatcher.instance == null || AmountInstance.AmmountInstanceBatchUpdateDispatcher.instance.threadContexts.Count != GlobalJobManager.ThreadCount)
					{
						AmountInstance.AmmountInstanceBatchUpdateDispatcher.instance = new AmountInstance.AmmountInstanceBatchUpdateDispatcher();
					}
					return AmountInstance.AmmountInstanceBatchUpdateDispatcher.instance;
				}
			}

			// Token: 0x0600C7FF RID: 51199 RVA: 0x00424E40 File Offset: 0x00423040
			public AmmountInstanceBatchUpdateDispatcher()
			{
				this.threadContexts = new List<List<AmountInstance.BatchUpdateContext.Result>>(GlobalJobManager.ThreadCount);
				for (int i = 0; i < GlobalJobManager.ThreadCount; i++)
				{
					this.threadContexts.Add(new List<AmountInstance.BatchUpdateContext.Result>());
				}
			}

			// Token: 0x0600C800 RID: 51200 RVA: 0x00424E83 File Offset: 0x00423083
			public void Reset(AmountInstance.BatchUpdateContext context)
			{
				this.sharedData = context;
				if (context.amount_instances == null)
				{
					this.count = 0;
					return;
				}
				this.count = (context.amount_instances.Count + 512 - 1) / 512;
			}

			// Token: 0x0600C801 RID: 51201 RVA: 0x00424EBC File Offset: 0x004230BC
			public override void RunItem(int item, ref AmountInstance.BatchUpdateContext shared_data, List<AmountInstance.BatchUpdateContext.Result> thread_context, int threadIndex)
			{
				int num = item * 512;
				int num2 = Mathf.Min(num + 512, shared_data.amount_instances.Count);
				for (int i = num; i < num2; i++)
				{
					AmountInstance amountInstance = (AmountInstance)shared_data.amount_instances[i].data;
					float num3 = amountInstance.GetDelta() * shared_data.time_delta;
					if (num3 != 0f)
					{
						thread_context.Add(new AmountInstance.BatchUpdateContext.Result
						{
							amount_instance = amountInstance,
							previous = amountInstance.value,
							delta = num3
						});
						amountInstance.SetValue(amountInstance.value + num3);
					}
				}
			}

			// Token: 0x0600C802 RID: 51202 RVA: 0x00424F5C File Offset: 0x0042315C
			public void Finish()
			{
				foreach (List<AmountInstance.BatchUpdateContext.Result> list in this.threadContexts)
				{
					foreach (AmountInstance.BatchUpdateContext.Result result in list)
					{
						result.amount_instance.Publish(result.delta, result.previous);
					}
					list.Clear();
				}
			}

			// Token: 0x0400AE5B RID: 44635
			private const int kBatchSize = 512;

			// Token: 0x0400AE5C RID: 44636
			private static AmountInstance.AmmountInstanceBatchUpdateDispatcher instance;
		}
	}
}
