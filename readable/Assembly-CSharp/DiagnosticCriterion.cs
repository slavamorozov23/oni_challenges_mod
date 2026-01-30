using System;

// Token: 0x020008E2 RID: 2274
public class DiagnosticCriterion
{
	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x06003F52 RID: 16210 RVA: 0x00163A1B File Offset: 0x00161C1B
	// (set) Token: 0x06003F53 RID: 16211 RVA: 0x00163A23 File Offset: 0x00161C23
	public string id { get; private set; }

	// Token: 0x17000469 RID: 1129
	// (get) Token: 0x06003F54 RID: 16212 RVA: 0x00163A2C File Offset: 0x00161C2C
	// (set) Token: 0x06003F55 RID: 16213 RVA: 0x00163A34 File Offset: 0x00161C34
	public string name { get; private set; }

	// Token: 0x06003F56 RID: 16214 RVA: 0x00163A3D File Offset: 0x00161C3D
	public DiagnosticCriterion(string name, Func<ColonyDiagnostic.DiagnosticResult> action)
	{
		this.name = name;
		this.evaluateAction = action;
	}

	// Token: 0x06003F57 RID: 16215 RVA: 0x00163A53 File Offset: 0x00161C53
	public void SetID(string id)
	{
		this.id = id;
	}

	// Token: 0x06003F58 RID: 16216 RVA: 0x00163A5C File Offset: 0x00161C5C
	public ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return this.evaluateAction();
	}

	// Token: 0x0400275C RID: 10076
	private Func<ColonyDiagnostic.DiagnosticResult> evaluateAction;
}
