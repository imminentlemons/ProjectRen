
using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int familiarity;
    public int trust;
    public int comfort;

    public List<string> memoryFlags = new List<string>();
}
