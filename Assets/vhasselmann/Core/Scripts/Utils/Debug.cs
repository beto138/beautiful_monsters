#if DEBUG
#define VERBOSE
#endif

#if !UNITY_EDITOR
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Internal;
using System;

/// <summary>
/// Overrides Unity default Debug class to avoid debug.log in release builds.
/// This class is not in BRSFramework namespace on purpose, or else it wont replace UnityEngine.Debug
/// </summary>
public static class Debug
{
	//
	// Summary:
	//     Opens or closes developer console.
	public static bool developerConsoleVisible { get { return UnityEngine.Debug.developerConsoleVisible; } set { UnityEngine.Debug.developerConsoleVisible = value; } }
	//
	// Summary:
	//     In the Build Settings dialog there is a check box called "Development Build".
	public static bool isDebugBuild { get { return UnityEngine.Debug.isDebugBuild; } }

	//
	// Summary:
	//     Assert the condition.
	public static void Assert(bool condition)
	{
		#if UNITY_ASSERTIONS
		UnityEngine.Debug.Assert(condition);
		#endif
	}

	//
	// Summary:
	//     Assert the condition.
	public static void Assert(bool condition, string message)
	{
		#if UNITY_ASSERTIONS
		UnityEngine.Debug.Assert(condition, message);
		#endif
	}

	//
	// Summary:
	//     Assert the condition.
	public static void Assert(bool condition, string format, params object[] args)
	{
		#if UNITY_ASSERTIONS
		UnityEngine.Debug.AssertFormat(condition, format, args);
		#endif
	}

	public static void Break()
	{
		UnityEngine.Debug.Break();
	}

	public static void ClearDeveloperConsole()
	{
		UnityEngine.Debug.ClearDeveloperConsole();
	}

	public static void DebugBreak()
	{
		UnityEngine.Debug.DebugBreak();
	}

	//
	// Summary:
	//     Draws a line between specified start and end points.
	public static void DrawLine(Vector3 start, Vector3 end)
	{
		UnityEngine.Debug.DrawLine(start, end);
	}
	//
	// Summary:
	//     Draws a line between specified start and end points.
	public static void DrawLine(Vector3 start, Vector3 end, Color color)
	{
		UnityEngine.Debug.DrawLine(start, end, color);
	}

	//
	// Summary:
	//     Draws a line between specified start and end points.
	public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
	{
		UnityEngine.Debug.DrawLine(start, end, color, duration);
	}

	//
	// Summary:
	//     Draws a line between specified start and end points.
	public static void DrawLine(Vector3 start, Vector3 end, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
	{
		UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
	}

	//
	// Summary:
	//     Draws a line from start to start + dir in world coordinates.
	public static void DrawRay(Vector3 start, Vector3 dir)
	{
		UnityEngine.Debug.DrawRay(start, dir);
	}

	//
	// Summary:
	//     Draws a line from start to start + dir in world coordinates.
	public static void DrawRay(Vector3 start, Vector3 dir, Color color)
	{
		UnityEngine.Debug.DrawRay(start, dir, color);
	}

	//
	// Summary:
	//     Draws a line from start to start + dir in world coordinates.
	public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
	{
		UnityEngine.Debug.DrawRay(start, dir, color, duration);
	}

	//
	// Summary:
	//     Draws a line from start to start + dir in world coordinates.
	public static void DrawRay(Vector3 start, Vector3 dir, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
	{
		UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
	}

	public static void LogInfo(object message, UnityEngine.Object context = null)
	{
		#if VERBOSE
		UnityEngine.Debug.Log(message, context);
		#endif
	}

	public static void LogInfoCat(params object[] args)
	{
		#if VERBOSE
		UnityEngine.Debug.Log(string.Concat(args));
		#endif
	}

	//
	// Summary:
	//     Logs message to the Unity Console.
	public static void Log(object message, UnityEngine.Object context = null)
	{
		#if VERBOSE
		UnityEngine.Debug.Log(message, context);
		#endif
	}

	//
	// Summary:
	//     Logs message to the Unity Console.
	public static void LogCat(params object[] args)
	{
		#if VERBOSE
		UnityEngine.Debug.Log(string.Concat(args));
		#endif
	}

	//
	// Summary:
	//     A variant of Debug.Log that logs an error message to the console.
	public static void LogError(object message, UnityEngine.Object context = null)
	{
		UnityEngine.Debug.LogError(message, context);
	}

	//
	// Summary:
	//     A variant of Debug.Log that logs an error message to the console.
	public static void LogErrorCat(params object[] args)
	{
		UnityEngine.Debug.LogError(string.Concat(args));
	}

	//
	// Summary:
	//     Logs a formatted error message to the Unity console.
	public static void LogErrorFormat(string format, params object[] args)
	{
		UnityEngine.Debug.LogErrorFormat(format, args);
	}

	//
	// Summary:
	//     Logs a formatted error message to the Unity console.
	public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
	{
		UnityEngine.Debug.LogErrorFormat(context, format, args);
	}

	//
	// Summary:
	//     A variant of Debug.Log that logs an error message to the console.
	public static void LogException(Exception exception)
	{
		UnityEngine.Debug.LogException(exception);
	}

	//
	// Summary:
	//     A variant of Debug.Log that logs an error message to the console.
	public static void LogException(Exception exception, UnityEngine.Object context)
	{
		UnityEngine.Debug.LogException(exception, context);
	}

	//
	// Summary:
	//     Logs a formatted message to the Unity Console.
	public static void LogFormat(string format, params object[] args)
	{
		#if VERBOSE
		UnityEngine.Debug.LogFormat(format, args);
		#endif
	}

	//
	// Summary:
	//     Logs a formatted message to the Unity Console.
	public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
	{
		#if VERBOSE
		UnityEngine.Debug.LogFormat(context, format, args);
		#endif
	}

	//
	// Summary:
	//     A variant of Debug.Log that logs a warning message to the console.
	public static void LogWarning(object message, UnityEngine.Object context = null)
	{
		UnityEngine.Debug.LogWarning(message, context);
	}

	//
	// Summary:
	//     A variant of Debug.Log that logs a warning message to the console.
	public static void LogWarningCat(params object[] args)
	{
		UnityEngine.Debug.LogWarning(string.Concat(args));
	}

	//
	// Summary:
	//     Logs a formatted warning message to the Unity Console.
	public static void LogWarningFormat(string format, params object[] args)
	{
		UnityEngine.Debug.LogWarningFormat(format, args);
	}

	//
	// Summary:
	//     Logs a formatted warning message to the Unity Console.
	public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
	{
		UnityEngine.Debug.LogWarningFormat(context, format, args);
	}
}
#endif