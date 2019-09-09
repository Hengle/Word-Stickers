using UnityEngine;

public static class ELDevice
{
	private static bool isLow = false;

	private static bool isSet = false;

	public static float notchOffsetY = 60f;

	public static bool IsLow()
	{
		if (!isSet)
		{
			if (SystemInfo.graphicsShaderLevel <= 30 || SystemInfo.processorCount <= 2)
			{
				isLow = true;
			}
			isSet = true;
		}
		return isLow;
	}

	public static bool HasNotch()
	{
		return false;
	}
}
