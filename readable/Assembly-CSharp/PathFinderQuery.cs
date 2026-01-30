using System;

// Token: 0x020004FB RID: 1275
public class PathFinderQuery
{
	// Token: 0x06001B91 RID: 7057 RVA: 0x00098A81 File Offset: 0x00096C81
	public virtual string Get_KProfilerName()
	{
		return "";
	}

	// Token: 0x06001B92 RID: 7058 RVA: 0x00098A88 File Offset: 0x00096C88
	public virtual bool IsMatch(int cell, int parent_cell, int cost)
	{
		return true;
	}

	// Token: 0x06001B93 RID: 7059 RVA: 0x00098A8B File Offset: 0x00096C8B
	public void SetResult(int cell, int cost, NavType nav_type)
	{
		this.resultCell = cell;
		this.resultNavType = nav_type;
	}

	// Token: 0x06001B94 RID: 7060 RVA: 0x00098A9B File Offset: 0x00096C9B
	public void ClearResult()
	{
		this.resultCell = -1;
	}

	// Token: 0x06001B95 RID: 7061 RVA: 0x00098AA4 File Offset: 0x00096CA4
	public virtual int GetResultCell()
	{
		return this.resultCell;
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x00098AAC File Offset: 0x00096CAC
	public NavType GetResultNavType()
	{
		return this.resultNavType;
	}

	// Token: 0x04001011 RID: 4113
	protected int resultCell;

	// Token: 0x04001012 RID: 4114
	private NavType resultNavType;
}
