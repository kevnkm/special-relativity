using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class Utility : MonoBehaviour
{
    public static void LocatePlayer(
        GameObject teleportationAnchor,
        TeleportationProvider teleportationProvider
    )
    {
        TeleportRequest teleportRequest = new TeleportRequest
        {
            destinationPosition = teleportationAnchor.transform.position,
            destinationRotation = teleportationAnchor.transform.rotation,
            matchOrientation = MatchOrientation.TargetUpAndForward
        };

        bool success = teleportationProvider.QueueTeleportRequest(teleportRequest);
        if (!success)
        {
            Debug.LogWarning("Failed to queue teleportation request.");
        }
    }
}
