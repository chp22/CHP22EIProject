using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BlockSpawner : MonoBehaviour
{
    [Tooltip("Where new blocks will appear")]
    public Transform spawnPoint;

    [Tooltip("Optional: assign your own block prefab here (must have Collider + Rigidbody)")]
    public GameObject blockPrefab;

    [Tooltip("If no prefab, a cube will be created instead")]
    public bool usePrimitiveCube = true;

    [Tooltip("List of possible sizes for spawned blocks")]
    public List<Vector3> blockSizes = new List<Vector3> {
        new Vector3(0.2f,0.2f,0.2f),
        new Vector3(0.3f,0.1f,0.5f),
        new Vector3(0.1f,0.4f,0.1f),
        new Vector3(0.5f,0.5f,0.2f)
    };

    public void SpawnBlock()
    {
        Vector3 origin = spawnPoint != null ? spawnPoint.position : transform.position;
        GameObject go;

        if (blockPrefab != null && !usePrimitiveCube)
            go = Instantiate(blockPrefab, origin, Random.rotation);
        else
        {
            go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = origin;
            go.transform.rotation = Random.rotation;
        }

        // Random size
        go.transform.localScale = blockSizes[Random.Range(0, blockSizes.Count)];

        // Rigidbody (needed for XRGrabInteractable)
        Rigidbody rb = go.GetComponent<Rigidbody>();
        if (rb == null) rb = go.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        // Make it grab-able
        if (go.GetComponent<XRGrabInteractable>() == null)
        {
            var grab = go.AddComponent<XRGrabInteractable>();
            // Optional tweaks:
            // grab.movementType = XRBaseInteractable.MovementType.VelocityTracking;
            // grab.throwOnDetach = true;
        }

        // Ensure it’s on a layer the XR Ray Interactor can hit (Default should work)
        go.layer = LayerMask.NameToLayer("Default");
    }
}
