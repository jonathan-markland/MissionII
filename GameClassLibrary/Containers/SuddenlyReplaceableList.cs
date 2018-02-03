using System;
using System.Collections.Generic;

namespace GameClassLibrary.Containers
{
    public class SuddenlyReplaceableList<T>
    {
        private List<T> _theList;

        public SuddenlyReplaceableList()
        {
            Clear();
        }

        public void Clear()
        {
            _theList = new List<T>();
        }

        public void Add(T objectToAdd)
        {
           _theList.Add(objectToAdd);
        }

        public void RemoveThese(List<T> listOfObjects)
        {
            // Making a fresh list with items removed.
            // Making a fresh list will have a different instance address
            // for the List<T>, thus will terminate any ForEachDo() in operation.
            var newList = new List<T>();
            newList.AddRange(_theList);
            foreach(var o in listOfObjects)
            {
                newList.Remove(o);
            }
            ReplaceWith(newList);
        }

        public void ReplaceWith(List<T> newItems)
        {
            _theList = newItems;
        }

        public void ForEachDo(Action<T> theAction)
        {
            var addressOfOriginalList = _theList;
            // Note: We support the collection being appended while this loop executes.
            var originalListLength = _theList.Count;
            for (int i = 0; i < _theList.Count; i++)
            {
                System.Diagnostics.Debug.Assert(_theList.Count >= originalListLength);
                theAction(_theList[i]);
                // _theList might have been invalidated.
                if (!object.ReferenceEquals(_theList, addressOfOriginalList)) break; // abort because client replaced list during iteration.
            }
        }
    }
}
