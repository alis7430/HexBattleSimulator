using System;
using UnityEngine;

public static class URLogger
{
    // Master switch for logging. When false, normal logs and warnings are suppressed.
    private static bool _enabled = true;

    // If true, errors are always logged even when `_enabled` is false.
    private static bool _alwaysLogErrors = true;

    // Hex string (RGB) used for normal logs. Example: "FF0000" for red.
    // When null or empty, no color wrapping is applied.
    private static string _logColorHex = null;

    /// <summary>
    /// Enable or disable normal logging (Log / LogWarning).
    /// </summary>
    public static void SetEnabled(bool enabled)
    {
        _enabled = enabled;
    }

    /// <summary>
    /// Returns whether normal logging is enabled.
    /// </summary>
    public static bool IsEnabled()
    {
        return _enabled;
    }

    /// <summary>
    /// Configure whether errors should always be logged even when logging is disabled.
    /// </summary>
    public static void SetAlwaysLogErrors(bool always)
    {
        _alwaysLogErrors = always;
    }

    /// <summary>
    /// Set the normal-log color. Pass a UnityEngine.Color to use the console rich-text color.
    /// Pass null to clear the color and use default console color.
    /// </summary>
    public static void SetLogColor(Color? color)
    {
        if (color == null)
        {
            _logColorHex = null;
            return;
        }

        _logColorHex = ColorUtility.ToHtmlStringRGB(color.Value);
    }

    /// <summary>
    /// Reset any custom log color so logs use the console default color.
    /// </summary>
    public static void ResetLogColor()
    {
        _logColorHex = null;
    }

    private static string ApplyColor(string msg)
    {
        if (string.IsNullOrEmpty(_logColorHex)) return msg;
        return $"<color=#{_logColorHex}>{msg}</color>";
    }

    // --- Log methods ---

    public static void Log(string msg)
    {
        if (!_enabled) return;
        Debug.Log(ApplyColor(msg));
    }

    public static void Log(string format, params object[] args)
    {
        if (!_enabled) return;
        Debug.Log(ApplyColor(string.Format(format, args)));
    }

    public static void Log(string msg, UnityEngine.Object context)
    {
        if (!_enabled) return;
        Debug.Log(ApplyColor(msg), context);
    }

    public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
    {
        if (!_enabled) return;
        Debug.Log(ApplyColor(string.Format(format, args)), context);
    }

    // --- Warning methods ---

    public static void LogWarning(string msg)
    {
        if (!_enabled) return;
        Debug.LogWarning(msg);
    }

    public static void LogWarning(string format, params object[] args)
    {
        if (!_enabled) return;
        Debug.LogWarning(string.Format(format, args));
    }

    public static void LogWarning(string msg, UnityEngine.Object context)
    {
        if (!_enabled) return;
        Debug.LogWarning(msg, context);
    }

    // --- Error methods ---

    public static void LogError(string msg)
    {
        if (!_enabled && !_alwaysLogErrors) return;
        Debug.LogError(msg);
    }

    public static void LogError(string format, params object[] args)
    {
        if (!_enabled && !_alwaysLogErrors) return;
        Debug.LogError(string.Format(format, args));
    }

    public static void LogError(string msg, UnityEngine.Object context)
    {
        if (!_enabled && !_alwaysLogErrors) return;
        Debug.LogError(msg, context);
    }
}
