using UnityEngine;
public interface IVision
{
    Component VisionSettings(float distance, float angle);
    void GetVisionData(out float visionDistance, out float visionRangeAngles);
}
