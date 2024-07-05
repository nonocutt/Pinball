using UnityEngine;

public static class Utilities
{
    public static float heightInUnits;

    public static void SetHeightsInUnits(Camera cam)
    {
        heightInUnits = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0)).y;
        Debug.Log(heightInUnits);
    }
    public static float CalculateYLimit(float spriteHeight)
    {
        return heightInUnits - spriteHeight * 0.5f;
    }

}
