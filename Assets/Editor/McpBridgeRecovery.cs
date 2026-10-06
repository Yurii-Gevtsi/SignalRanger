using System;
using System.Reflection;
using UnityEditor;

internal static class McpBridgeRecovery
{
    // Reconnect the local Editor bridge after a domain reload.
    [InitializeOnLoadMethod]
    private static void StartBridgeAfterCompilation()
    {
        EditorApplication.delayCall += () =>
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }

            var locator = Type.GetType("MCPForUnity.Editor.Services.MCPServiceLocator, MCPForUnity.Editor");
            var bridge = locator?.GetProperty("Bridge", BindingFlags.Public | BindingFlags.Static)?.GetValue(null);
            bridge?.GetType().GetMethod("StartAsync", Type.EmptyTypes)?.Invoke(bridge, null);
        };
    }
}
