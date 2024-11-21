using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveDataList", menuName = "SO/WaveDataList", order = 0)]
public class WaveDataList : ScriptableObject
{
    public List<WaveEntity> WaveList;

    public static ScriptableObject Convert(Sheet sheet)
    {
        WaveDataList waveDataList = ScriptableObject.CreateInstance<WaveDataList>();

        if (sheet is WaveSheets waveSheets)
        {
            waveDataList.WaveList = waveSheets.WaveList;
            foreach (var wave in waveDataList.WaveList)
            {
                wave.SmallWaveEntities = new List<SmallWaveEntity>();
                foreach (var samllWave in waveSheets.SmallWaveList)
                    if (samllWave.waveId == wave.id)
                        wave.SmallWaveEntities.Add(samllWave);
            }
        }

        return waveDataList;
    }
}