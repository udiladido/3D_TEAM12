using System.Collections.Generic;

[ExcelAsset(ExcelName = "JobSheets", AssetPath = "Resources/SO/Sheets")]
public class JobSheets : Sheet
{
	public List<JobEntity> JobList;
	public List<JobStatEntity> StatList;
}
