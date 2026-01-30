using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000925 RID: 2341
public class EntityLuminescence : GameStateMachine<EntityLuminescence, EntityLuminescence.Instance, IStateMachineTarget, EntityLuminescence.Def>
{
	// Token: 0x06004184 RID: 16772 RVA: 0x0017202C File Offset: 0x0017022C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x02001923 RID: 6435
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007D06 RID: 32006
		public Color lightColor;

		// Token: 0x04007D07 RID: 32007
		public float lightRange;

		// Token: 0x04007D08 RID: 32008
		public float lightAngle;

		// Token: 0x04007D09 RID: 32009
		public Vector2 lightOffset;

		// Token: 0x04007D0A RID: 32010
		public Vector2 lightDirection;

		// Token: 0x04007D0B RID: 32011
		public global::LightShape lightShape;
	}

	// Token: 0x02001924 RID: 6436
	public new class Instance : GameStateMachine<EntityLuminescence, EntityLuminescence.Instance, IStateMachineTarget, EntityLuminescence.Def>.GameInstance
	{
		// Token: 0x0600A17D RID: 41341 RVA: 0x003ABE8C File Offset: 0x003AA08C
		public Instance(IStateMachineTarget master, EntityLuminescence.Def def) : base(master, def)
		{
			this.light.Color = def.lightColor;
			this.light.Range = def.lightRange;
			this.light.Angle = def.lightAngle;
			this.light.Direction = def.lightDirection;
			this.light.Offset = def.lightOffset;
			this.light.shape = def.lightShape;
		}

		// Token: 0x0600A17E RID: 41342 RVA: 0x003ABF08 File Offset: 0x003AA108
		public override void StartSM()
		{
			base.StartSM();
			this.luminescence = Db.Get().Attributes.Luminescence.Lookup(base.gameObject);
			AttributeInstance attributeInstance = this.luminescence;
			attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, new System.Action(this.OnLuminescenceChanged));
			this.RefreshLight();
		}

		// Token: 0x0600A17F RID: 41343 RVA: 0x003ABF68 File Offset: 0x003AA168
		private void OnLuminescenceChanged()
		{
			this.RefreshLight();
		}

		// Token: 0x0600A180 RID: 41344 RVA: 0x003ABF70 File Offset: 0x003AA170
		public void RefreshLight()
		{
			if (this.luminescence != null)
			{
				int num = (int)this.luminescence.GetTotalValue();
				this.light.Lux = num;
				bool flag = num > 0;
				if (this.light.enabled != flag)
				{
					this.light.enabled = flag;
				}
			}
		}

		// Token: 0x0600A181 RID: 41345 RVA: 0x003ABFBD File Offset: 0x003AA1BD
		protected override void OnCleanUp()
		{
			if (this.luminescence != null)
			{
				AttributeInstance attributeInstance = this.luminescence;
				attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, new System.Action(this.OnLuminescenceChanged));
			}
			base.OnCleanUp();
		}

		// Token: 0x04007D0C RID: 32012
		[MyCmpAdd]
		private Light2D light;

		// Token: 0x04007D0D RID: 32013
		private AttributeInstance luminescence;
	}
}
