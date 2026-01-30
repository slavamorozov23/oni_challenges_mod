using System;
using KSerialization;

// Token: 0x02000539 RID: 1337
[SerializationConfig(MemberSerialization.OptIn)]
public class StateMachineComponent<StateMachineInstanceType> : StateMachineComponent, ISaveLoadable where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x0009D700 File Offset: 0x0009B900
	public StateMachineInstanceType smi
	{
		get
		{
			if (this._smi == null)
			{
				this._smi = (StateMachineInstanceType)((object)Activator.CreateInstance(typeof(StateMachineInstanceType), new object[]
				{
					this
				}));
			}
			return this._smi;
		}
	}

	// Token: 0x06001CD2 RID: 7378 RVA: 0x0009D739 File Offset: 0x0009B939
	public override StateMachine.Instance GetSMI()
	{
		return this._smi;
	}

	// Token: 0x06001CD3 RID: 7379 RVA: 0x0009D746 File Offset: 0x0009B946
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this._smi != null)
		{
			this._smi.StopSM("StateMachineComponent.OnCleanUp");
			this._smi = default(StateMachineInstanceType);
		}
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x0009D77C File Offset: 0x0009B97C
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (base.isSpawned)
		{
			this.smi.StartSM();
		}
	}

	// Token: 0x06001CD5 RID: 7381 RVA: 0x0009D79C File Offset: 0x0009B99C
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this._smi != null)
		{
			this._smi.StopSM("StateMachineComponent.OnDisable");
		}
	}

	// Token: 0x040010FF RID: 4351
	private StateMachineInstanceType _smi;
}
