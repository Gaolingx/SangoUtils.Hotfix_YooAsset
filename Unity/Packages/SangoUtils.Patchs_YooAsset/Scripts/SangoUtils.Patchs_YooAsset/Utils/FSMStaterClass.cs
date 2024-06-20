using System;

namespace SangoUtils.Patchs_YooAsset.Utils
{
    internal abstract class FSMTransCommandBase { }

    internal class FSMTransCommandEnum<T> : FSMTransCommandBase where T : struct
    {
        internal T EnumId { get; private set; }

        internal FSMTransCommandEnum(T enumId)
        {
            EnumId = enumId;
        }

        public override bool Equals(object obj)
        {
            if (obj is FSMTransCommandEnum<T>)
            {
                FSMTransCommandEnum<T> commandEnum = obj as FSMTransCommandEnum<T>;
                if (commandEnum != null)
                {
                    return commandEnum.EnumId.Equals(EnumId);
                }
                return false;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return EnumId.GetHashCode();
        }
    }

    internal class FSMTransCommandData : FSMTransCommandBase
    {
        internal byte[] Data { get; private set; }

        internal FSMTransCommandData(byte[] data)
        {
            Data = data;
        }

        public override bool Equals(object obj)
        {
            if (obj is FSMTransCommandData)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }

    internal class FSMStaterItem<T> where T : struct
    {
        internal FSMTransCommandBase TransCommand { get; private set; }
        internal T TargetState { get; private set; }
        internal Func<T, FSMTransCommandBase, T, bool> TransCallBack { get; private set; }

        internal FSMStaterItem(FSMTransCommandBase command, T targetState, Func<T, FSMTransCommandBase, T, bool> callBack)
        {
            TransCommand = command;
            TargetState = targetState;
            TransCallBack = callBack;
        }
    }

    internal class FSMLinkedStaterItemBase
    {
        protected FSMLinkedStater _fsmLinkedStater;

        internal virtual void OnInit(FSMLinkedStater fsmLinkedStater)
        {
            _fsmLinkedStater = fsmLinkedStater;
        }
        internal virtual void OnEnter() { }
        internal virtual void OnUpdate() { }
        internal virtual void OnExit() { }
    }
}