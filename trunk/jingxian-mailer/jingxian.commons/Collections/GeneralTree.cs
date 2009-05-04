
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;


namespace jingxian.collections
{

    public interface IVisitor<T>
    {
        bool HasCompleted { get; }

        void Visit(T obj);
    }
    public class OrderedVisitor<T> : IVisitor<T>
    {
        private readonly IVisitor<T> visitorToUse;

        public OrderedVisitor(IVisitor<T> visitorToUse)
        {
            Guard.ArgumentNotNull(visitorToUse, "visitorToUse");

            this.visitorToUse = visitorToUse;
        }

        public bool HasCompleted
        {
            get { return visitorToUse.HasCompleted; }
        }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "PreOrder")]
        public virtual void VisitPreOrder(T obj)
        {
            visitorToUse.Visit(obj);
        }

        public virtual void VisitPostOrder(T obj)
        {
            visitorToUse.Visit(obj);
        }

        public virtual void VisitInOrder(T obj)
        {
            visitorToUse.Visit(obj);
        }
        
        public void Visit(T obj)
        {
            visitorToUse.Visit(obj);
        }

        public IVisitor<T> VisitorToUse
        {
            get {return visitorToUse;   }
        }
    }

    [Serializable]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class Tree<T> : ICollection<T>
    {
        #region Globals

        private T nodeData;
        private Tree<T> parent;
        private readonly List<Tree<T>> childNodes = new List<Tree<T>>();

        #endregion

        public Tree(T data)
        {
            Guard.ArgumentNotNull(data, "data");

            nodeData = data;
        }

        public T Data
        {
            get { return nodeData; }
            set { Guard.ArgumentNotNull(value, "value"); nodeData = value; }
        }

        public virtual bool IsLeafNode
        {
            get { return Degree == 0; }
        }


        public bool IsEmpty
        {
            get { return (Count == 0); }
        }

        public int Count
        {
            get { return childNodes.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public Tree<T> this[int index]
        {
            get { return GetChild(index); }
        }

        public int Height
        {
            get
            {
                if (Degree == 0)
                    return 0;
                return 1 + FindMaximumChildHeight();
            }
        }

        #region ICollection<T>  Members

        public bool Contains(T item)
        {
            foreach (T thisItem in this)
            {
                if (item.Equals(thisItem))
                    return true;
            }

            return false;
        }

		public void CopyTo(T[] array, int arrayIndex)
        {

            Guard.ArgumentNotNull(array, "array");

            foreach (T item in this)
            {
                if (arrayIndex >= array.Length)
                    throw new ArgumentException("数组空间不足!", "array");
         
                array[arrayIndex++] = item;
            }
        }

        public virtual void Clear()
        {
            childNodes.Clear();
        }

        public void Add(T item)
        {
            Tree<T> child = new Tree<T>(item);
            InsertItem(Count, child);
        }

        public bool Remove(T item)
        {
            return RemoveItem(item);
        }


        public void Add(Tree<T> child)
        {
            InsertItem(Count, child);
        }

        public bool Remove(Tree<T> child)
        {
            int indexOf = childNodes.IndexOf(child);

            if (indexOf < 0)
                return false;

            RemoveItem(indexOf, child);
            return true;
        }

        public void RemoveAt(int index)
        {
            RemoveItem(index, childNodes[index]);
        }

        protected virtual bool RemoveItem(T item)
        {
            for (int i = 0; i < childNodes.Count; i++)
            {
                if (childNodes[i].Data.Equals(item))
                {
                    childNodes[i].parent = null;
                    childNodes.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        protected virtual void RemoveItem(int index, Tree<T> item)
        {
            item.parent = null;
            childNodes.RemoveAt(index);
        }

        protected virtual void InsertItem(int index, Tree<T> item)
        {
            if (item.parent != null && item.parent != this)
            {
                item.parent.Remove(item);
                item.parent = null;
            }

            if (!childNodes.Contains(item))
            {
                childNodes.Add(item);
                item.parent = this;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            Stack<Tree<T>> stack = new Stack<Tree<T>>();

            stack.Push(this);

            while (stack.Count > 0)
            {
                Tree<T> tree = stack.Pop();

                if (null == tree)
                    continue;

                yield return tree.Data;

                for (int i = 0; i < tree.Degree; i++)
                {
                    stack.Push(tree.GetChild(i));
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Public Members

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IList<Tree<T>> Ancestors
        {
            get { return GetPath(); }
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IList<Tree<T>> Childs
        {
            get { return new ReadOnlyCollection<Tree<T>>(childNodes); }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public Tree<T> Parent
        {
            get { return parent; }
            set { Guard.ArgumentNotNull(value, "value"); value.Add(this); }
        }

        public int Degree
        {
            get { return childNodes.Count; }
        }

        public Tree<T> Find(Predicate<T> condition)
        {
            Guard.ArgumentNotNull(condition, "condition");

            if (condition(Data))
                return this;
            
            for (int i = 0; i < Degree; i++)
            {
                Tree<T> ret = childNodes[i].Find(condition);

                if (ret != null)
                    return ret;
            }

            return null;
        }

        public Tree<T> GetChild(int index)
        {
            return childNodes[index];
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public IList<Tree<T>> GetPath()
        {
            return GetPath();
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        protected IList<Tree<T>> GetPath(bool includeThis)
        {
            List<Tree<T>> path = new List<Tree<T>>();

            if (includeThis)
                path.Add(this);

            for (Tree<T> node = Parent; node != null; node = node.Parent)
            {
                path.Add(node);
            }

            path.Reverse();

            return path;
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public void Sort(Comparison<Tree<T>> comparison)
        {
            Guard.ArgumentNotNull(comparison, "comparison");

            childNodes.Sort( comparison);

            for (int i = 0; i < childNodes.Count; i++)
            {
                childNodes[i].Sort( comparison);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public void Sort(IComparer<Tree<T>> comparer)
        {
            Guard.ArgumentNotNull(comparer, "comparer");

            childNodes.Sort( comparer);

            for (int i = 0; i < childNodes.Count; i++)
            {
                childNodes[i].Sort( comparer);
            }
        }

        public void DepthFirstTraversal(OrderedVisitor<T> orderedVisitor)
        {
            Guard.ArgumentNotNull(orderedVisitor, "orderedVisitor");

            if (orderedVisitor.HasCompleted)
                return;

            orderedVisitor.VisitPreOrder(Data);

            for (int i = 0; i < Degree; i++)
            {
                if (GetChild(i) != null)
                    GetChild(i).DepthFirstTraversal(orderedVisitor);
            }

            orderedVisitor.VisitPostOrder(Data);
        }

        public void BreadthFirstTraversal(IVisitor<T> visitor)
        {
            Queue<Tree<T>> visitableQueue = new Queue<Tree<T>>();

            visitableQueue.Enqueue(this);

            while (0 != visitableQueue.Count)
            {
                Tree<T> generalTree = visitableQueue.Dequeue();
                visitor.Visit(generalTree.Data);

                for (int i = 0; i < generalTree.Degree; i++)
                {
                    Tree<T> child = generalTree.GetChild(i);

                    if (child != null)
                    {
                        visitableQueue.Enqueue(child);
                    }
                }
            }
        }

        #endregion

        private int FindMaximumChildHeight()
        {
            int maximum = 0;

            for (int i = 0; i < Degree; i++)
            {
                int childHeight = GetChild(i).Height;

                if (childHeight > maximum)
                    maximum = childHeight;
            }

            return maximum;
        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }
}