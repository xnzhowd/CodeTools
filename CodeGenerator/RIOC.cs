using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class Data
    {
        public string Name { get; set; }
    }

    public class ItemEnumerator<T> : IEnumerator
    {
        private List<T> _objects;

        public ItemEnumerator(List<T> objects)
        {
            this._objects = objects;
        }

        private int currentIndex = -1;
        public object Current => _objects[currentIndex];

        public bool MoveNext()
        {
            currentIndex++;
            return currentIndex < _objects.Count;
        }

        public void Reset()
        {
            currentIndex = -1;
        }
    }

    public class BindData<T> : IBindData
    {
        private List<T> _list;

        public BindData(List<T> list)
        {
            _list = list;
        }

        public IEnumerator GetEnumerator()
        {
            return new ItemEnumerator<T>(_list);
        }
    }
    public interface IBindData: IEnumerable
    {
        
    }
}
