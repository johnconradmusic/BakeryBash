using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using BakeryBash;
using Monocle;

namespace BakeryBash
{
    [Serializable]
    public class SaveData
    {
        public static SaveData Instance;
        public string Version;

        public long Time;
        public DateTime LastSave;
        public Session CurrentSession;

        public int TotalDeaths;
        public int TotalVictories;

        public int LastWorld;
        public int LastLevel;

        public HashSet<string> Flags;


        public SaveData()
        {
        }

        public static void StartNew(SaveData data)
        {

            SaveData.Instance = data;
            SaveData.Instance.AfterInitialize();
        }

        public static string GetFilename(int slot) => slot == 4 ? "debug" : slot.ToString();

        public static string GetFilename() => SaveData.GetFilename(0);

        public static bool TryDelete(int slot) => UserIO.Delete(SaveData.GetFilename(slot));

        public void AfterInitialize()
        {

        }


        public void BeforeSave() => SaveData.Instance.Version = BakeryBash.Instance.Version.ToString();

        public void AddTime(long time)
        {
            this.Time += time;
        }

        public bool HasFlag(string flag) => this.Flags.Contains(flag);

        public void SetFlag(string flag)
        {
            if (this.HasFlag(flag))
                return;
            this.Flags.Add(flag);
        }
    }
}
