using System;
using System.Collections.Generic;
using Monocle;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BakeryBash
{
	[Serializable]
	public class Session
	{
		public List<Session.Counter> Counters = new List<Session.Counter>();
        public HashSet<string> Flags = new HashSet<string>();

		[XmlIgnore]
        public int currentScore;

        [XmlAttribute]
		public int CurrentLevelID;
        [XmlAttribute]
        public int CurrentWorldID;

        public bool GetFlag(string flag) => this.Flags.Contains(flag);

        public void SetFlag(string flag, bool setTo = true)
        {
            if (setTo)
                this.Flags.Add(flag);
            else
                this.Flags.Remove(flag);
        }

        public int GetCounter(string counter)
        {
            for (int index = 0; index < this.Counters.Count; ++index)
            {
                if (this.Counters[index].Key.Equals(counter))
                    return this.Counters[index].Value;
            }
            return 0;
        }

        public void SetCounter(string counter, int value)
        {
            for (int index = 0; index < this.Counters.Count; ++index)
            {
                if (this.Counters[index].Key.Equals(counter))
                {
                    this.Counters[index].Value = value;
                    return;
                }
            }
            this.Counters.Add(new Session.Counter()
            {
                Key = counter,
                Value = value
            });
        }

        public void IncrementCounter(string counter)
        {
            for (int index = 0; index < this.Counters.Count; ++index)
            {
                if (this.Counters[index].Key.Equals(counter))
                {
                    ++this.Counters[index].Value;
                    return;
                }
            }
            this.Counters.Add(new Session.Counter()
            {
                Key = counter,
                Value = 1
            });
        }


        [Serializable]
        public class Counter
        {
            [XmlAttribute("key")]
            public string Key;
            [XmlAttribute("value")]
            public int Value;
        }
    }
}