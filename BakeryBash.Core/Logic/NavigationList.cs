using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryBash
{
    public class NavigationList<T> : List<T>
    {
        private int _currentIndex = 0;
        public int CurrentIndex
        {
            get
            {
                if (_currentIndex > Count - 1) { _currentIndex = 0; }
                if (_currentIndex < 0) { _currentIndex = Count - 1; }
                return _currentIndex;
            }
            set { _currentIndex = value; }
        }

        public T MoveNext
        {
            get { _currentIndex++; return this[CurrentIndex]; }
        }

        public T MovePrevious
        {
            get { _currentIndex--; return this[CurrentIndex]; }
        }

        public T Current
        {
            get { return this[CurrentIndex]; }
        }
    }
}