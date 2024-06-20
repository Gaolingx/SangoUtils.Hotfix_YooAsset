using System;

namespace SangoUtils.Patchs_YooAsset
{
    internal class PatchSystemEventArgs : EventArgs
    {
        internal PatchSystemEventArgs(PatchSystemEventCode patchSystemEventCode, params object[] data)
        {
            PatchSystemEventCode = patchSystemEventCode;
            ExtensionData = data;
        }

        internal PatchSystemEventCode PatchSystemEventCode { get; }
        internal object[] ExtensionData { get; }
    }

    internal enum PatchSystemEventCode
    {
        InitializeFailed,
        PatchStatesChange,
        FoundUpdateFiles,
        DownloadProgressUpdate,
        PackageVersionUpdateFailed,
        PatchManifestUpdateFailed,
        WebFileDownloadFailed,
        ClosePatchWindow
    }

    internal class PatchUserEventArgs : EventArgs
    {
        internal PatchUserEventArgs(PatchUserEventCode patchUserEventCode)
        {
            PatchUserEventCode = patchUserEventCode;
        }

        internal PatchUserEventCode PatchUserEventCode { get; }
    }

    internal enum PatchUserEventCode
    {
        UserTryInitialize,
        UserBeginDownloadWebFiles,
        UserTryUpdatePackageVersion,
        UserTryUpdatePatchManifest,
        UserTryDownloadWebFiles
    }
}