using System.Collections.Generic;

[ExcelAsset(ExcelName = "WaveSheets", AssetPath = "Resources/SO/Sheets")]
public class WaveSheets : Sheet
{
	public List<WaveEntity> WaveList;
	public List<SmallWaveEntity> SmallWaveList;
}
