using System;


namespace Plugin.Notifications
{
    [Flags]
    public enum AndroidActivityFlags
    {
        BroughtToFront = 4194304,
        ClearTask = 32768,
        ClearTop = 67108864,
        ClearWhenTaskReset = 524288,
        ExcludeFromRecents = 8388608,
        ForwardResult = 33554432,
        LaunchAdjacent = 4096,
        LaunchedFromHistory = 1048576,
        MultipleTask = 134217728,
        NewTask = 268435456,
        NoAnimation = 65536,
        NoHistory = 1073741824,
        NoUserAction = 262144,
        PreviousIsTop = 16777216,
        ReorderToFront = 131072,
        ResetTaskIfNeeded = 2097152,
        RetainInRecents = 8192,
        SingleTop = 536870912,
        TaskOnHome = 16384,
        DebugLogResolution = 8,
        ExcludeStoppedPackages = 16,
        FromBackground = 4,
        GrantPersistableUriPermission = 64,
        GrantPrefixUriPermission = 128,
        GrantReadUriPermission = 1,
        GrantWriteUriPermission = 2,
        IncludeStoppedPackages = 32
    }
}