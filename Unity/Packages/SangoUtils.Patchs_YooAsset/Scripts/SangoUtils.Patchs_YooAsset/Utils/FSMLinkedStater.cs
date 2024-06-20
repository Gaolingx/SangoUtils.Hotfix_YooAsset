using System;
using System.Collections.Generic;

namespace SangoUtils.Patchs_YooAsset.Utils
{
    internal class FSMLinkedStater : FSMStaterBase
    {
        private readonly LinkedList<FSMLinkedStaterItemBase> _staterItemLinkedList = new LinkedList<FSMLinkedStaterItemBase>();
        private LinkedListNode<FSMLinkedStaterItemBase> _currentNode;

        internal FSMLinkedStater(object owner)
        {
            Owner = owner;
            _currentNode = _staterItemLinkedList.First;
        }

        internal object Owner { get; }

        internal string CurrentStaterName
        {
            get
            {
                if (_currentNode != null)
                {
                    return _currentNode.Value.GetType().FullName;
                }
                return string.Empty;
            }
        }

        internal void AddStaterItem<T>() where T : FSMLinkedStaterItemBase
        {
            Type staterItemType = typeof(T);
            FSMLinkedStaterItemBase item = Activator.CreateInstance(staterItemType) as FSMLinkedStaterItemBase;
            if (item != null)
            {
                AddStaterItem(item);
            }
        }

        internal void AddStaterItem(FSMLinkedStaterItemBase staterItem)
        {
            if (staterItem == null) throw new ArgumentNullException();
            _staterItemLinkedList.AddLast(staterItem);
            staterItem.OnInit(this);
        }

        internal void InvokeFirstStaterItem()
        {
            _currentNode = _staterItemLinkedList.First;
            _currentNode.Value.OnEnter();
        }

        internal void InvokeNextStaterItem()
        {
            _currentNode.Value.OnExit();
            if (_currentNode.Next != null)
            {
                _currentNode = _currentNode.Next;
                _currentNode.Value.OnEnter();
            }
        }

        internal void InvokeTargetStaterItem<T>(bool isFirstInvoke = false) where T : FSMLinkedStaterItemBase
        {
            LinkedListNode<FSMLinkedStaterItemBase> targetNode = FindTargetStaterItemNode<T>();
            if (targetNode != null)
            {
                if (!isFirstInvoke)
                {
                    _currentNode.Value.OnExit();
                }
                _currentNode = targetNode;
                _currentNode.Value.OnEnter();
            }
        }

        internal void UpdateCurrentStaterItem()
        {
            if (_currentNode != null)
            {
                _currentNode.Value.OnUpdate();
            }
        }

        internal void UpdateTargetStaterItem<T>() where T : FSMLinkedStaterItemBase
        {
            LinkedListNode<FSMLinkedStaterItemBase> targetNode = FindTargetStaterItemNode<T>();
            if (targetNode != null)
            {
                targetNode.Value.OnUpdate();
            }
        }

        internal void UpdateAllStaterItem()
        {
            LinkedListNode<FSMLinkedStaterItemBase> tempNode = _staterItemLinkedList.First;
            while (tempNode != null)
            {
                tempNode.Value.OnUpdate();
                tempNode = tempNode.Next;
            }
        }

        internal void RemoveStaterItem<T>() where T : FSMLinkedStaterItemBase
        {
            LinkedListNode<FSMLinkedStaterItemBase> targetNode = FindTargetStaterItemNode<T>();
            if (targetNode != null)
            {
                _staterItemLinkedList.Remove(targetNode);
            }
        }

        private LinkedListNode<FSMLinkedStaterItemBase> FindTargetStaterItemNode<T>() where T : FSMLinkedStaterItemBase
        {
            Type staterItemType = typeof(T);
            int staterItemId = staterItemType.GetHashCode();
            LinkedListNode<FSMLinkedStaterItemBase> targetNode = _staterItemLinkedList.First;
            while (targetNode != null)
            {
                Type existStaterItemType = targetNode.Value.GetType();
                int existStaterItemId = existStaterItemType.GetHashCode();
                if (existStaterItemId == staterItemId)
                {
                    break;
                }
                else
                {
                    targetNode = targetNode.Next;
                }
            }
            return targetNode;
        }
    }
}