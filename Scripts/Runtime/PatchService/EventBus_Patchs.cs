using System;

namespace SangoUtils.Patchs_YooAsset.Utils
{
    internal class EventBus_Patchs
    {
        public static EventBus_Patchs _this;

        public EventBus_Patchs() { _this = this; }

        private SangoPatchConfig _patchConfig;
        internal static SangoPatchConfig PatchConfig { get => _this._patchConfig; set => _this._patchConfig ??= value; }

        private event Action<object, PatchSystemEventArgs> _patchSystemEvent;
        public static void AddPatchSystemEvent(Action<object, PatchSystemEventArgs> action) { _this._patchSystemEvent += action; }
        public static void RemovePatchSystemEvent(Action<object, PatchSystemEventArgs> action) { _this._patchSystemEvent -= action; }
        public static void CallPatchSystemEvent(object sender, PatchSystemEventArgs eventArgs) { _this._patchSystemEvent?.Invoke(sender, eventArgs); }

        private event Action<object, PatchUserEventArgs> _patchUserEvent;
        public static void AddPatchUserEvent(Action<object, PatchUserEventArgs> action) { _this._patchUserEvent += action; }
        public static void RemovePatchUserEvent(Action<object, PatchUserEventArgs> action) { _this._patchUserEvent -= action; }
        public static void CallPatchUserEvent(object sender, PatchUserEventArgs eventArgs) { _this._patchUserEvent?.Invoke(sender, eventArgs); }

        private event Action<object, PatchOperationEventArgs> _patchOperationEvent;
        public static void AddPatchOperationEvent(Action<object, PatchOperationEventArgs> action) { _this._patchOperationEvent += action; }
        public static void RemovePatchOperationEvent(Action<object, PatchOperationEventArgs> action) { _this._patchOperationEvent -= action; }
        public static void CallPatchOperationEvent(object sender, PatchOperationEventArgs eventArgs) { _this._patchOperationEvent?.Invoke(sender, eventArgs); }

        private event Action<object, PatchSystem_DownloadProgressUpdateEventArgs> _patchSystem_DownloadProgressUpdateEvent;
        public static void AddPatchSystem_DownloadProgressUpdateEvent(Action<object, PatchSystem_DownloadProgressUpdateEventArgs> action) { _this._patchSystem_DownloadProgressUpdateEvent += action; }
        public static void RemovePatchSystem_DownloadProgressUpdateEvent(Action<object, PatchSystem_DownloadProgressUpdateEventArgs> action) { _this._patchSystem_DownloadProgressUpdateEvent -= action; }
        public static void CallPatchSystem_DownloadProgressUpdateEvent(object sender, PatchSystem_DownloadProgressUpdateEventArgs eventArgs) { _this._patchSystem_DownloadProgressUpdateEvent?.Invoke(sender, eventArgs); }
    }
}
