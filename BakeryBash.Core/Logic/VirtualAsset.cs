using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocle
{
    public abstract class VirtualAsset
    {
        public int Height
        {
            get;
            internal set;
        }

        public string Name
        {
            get;
            internal set;
        }

        public int Width
        {
            get;
            internal set;
        }

        protected VirtualAsset()
        {
        }

        public virtual void Dispose()
        {
        }

        internal virtual void Reload()
        {
        }

        internal virtual void Unload()
        {
        }
    }
}