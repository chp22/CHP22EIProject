using UnityEngine;

[ExecuteInEditMode]
public class NeoclassicalMuseumBuilder : MonoBehaviour
{
    [Header("General Materials")]
    public Material wallMaterial;
    public Material floorMaterial;
    public Material ceilingMaterial;

    [Header("Central Ballroom Settings")]
    public Vector3 centralSize = new Vector3(20, 10, 20);

    [Header("Wing Settings")]
    public Vector3 wingSize = new Vector3(10, 8, 20);
    public float wingOffset = 0.5f; // slight gap so wings attach to the main room

    [Header("Door Settings")]
    public float doorWidth = 4f;
    public float doorHeight = 6f;
    public float wallThickness = 1f; // thickness for all walls

    void Start()
    {
        // Optionally, you can auto–generate at runtime.
        // GenerateMuseum();
    }

    /// <summary>
    /// Called from the custom editor button to generate the museum.
    /// </summary>
    public void GenerateMuseum()
    {
        ClearOldMuseum();
        BuildMuseum();
    }

    /// <summary>
    /// Removes any previously generated museum.
    /// </summary>
    void ClearOldMuseum()
    {
        Transform oldMuseum = transform.Find("NeoclassicalMuseum");
        if (oldMuseum != null)
        {
#if UNITY_EDITOR
            DestroyImmediate(oldMuseum.gameObject);
#else
            Destroy(oldMuseum.gameObject);
#endif
        }
    }

    /// <summary>
    /// Creates the central ballroom with door openings on all four walls and attaches four wings.
    /// </summary>
    void BuildMuseum()
    {
        // Create a parent container under this GameObject.
        GameObject museumParent = new GameObject("NeoclassicalMuseum");
        museumParent.transform.parent = transform;

        // Build the central ballroom.
        GameObject centralBallroom = BuildCentralBallroom();
        centralBallroom.transform.parent = museumParent.transform;

        // For each wing, the attached door in the central room is created on the appropriate wall.
        // Wings are positioned so that their door opening aligns with the corresponding door on the central room.
        float halfCentralX = centralSize.x / 2f;
        float halfCentralZ = centralSize.z / 2f;

        // North Wing: attached to the north wall (at +Z)
        GameObject northWing = BuildWing("NorthWing");
        northWing.transform.parent = museumParent.transform;
        // Position so that its south (door) wall touches the north wall of the ballroom.
        northWing.transform.position = new Vector3(0, 0, halfCentralZ + wingOffset + wingSize.z / 2f);

        // South Wing: attached to the south wall (at -Z)
        GameObject southWing = BuildWing("SouthWing");
        southWing.transform.parent = museumParent.transform;
        southWing.transform.position = new Vector3(0, 0, -halfCentralZ - wingOffset - wingSize.z / 2f);

        // East Wing: attached to the east wall (at +X)
        GameObject eastWing = BuildWing("EastWing");
        eastWing.transform.parent = museumParent.transform;
        eastWing.transform.position = new Vector3(halfCentralX + wingOffset + wingSize.x / 2f, 0, 0);

        // West Wing: attached to the west wall (at -X)
        GameObject westWing = BuildWing("WestWing");
        westWing.transform.parent = museumParent.transform;
        westWing.transform.position = new Vector3(-halfCentralX - wingOffset - wingSize.x / 2f, 0, 0);
    }

    #region Central Ballroom Construction

    /// <summary>
    /// Builds the central ballroom with a floor, ceiling, and four walls that include door openings.
    /// </summary>
    GameObject BuildCentralBallroom()
    {
        GameObject ballroom = new GameObject("CentralBallroom");

        // Floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";
        floor.transform.parent = ballroom.transform;
        floor.transform.localScale = new Vector3(centralSize.x, 1, centralSize.z);
        floor.transform.localPosition = new Vector3(0, -0.5f, 0);
        if (floorMaterial != null)
            floor.GetComponent<Renderer>().material = floorMaterial;

        // Ceiling
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ceiling.name = "Ceiling";
        ceiling.transform.parent = ballroom.transform;
        ceiling.transform.localScale = new Vector3(centralSize.x, 1, centralSize.z);
        ceiling.transform.localPosition = new Vector3(0, centralSize.y, 0);
        if (ceilingMaterial != null)
            ceiling.GetComponent<Renderer>().material = ceilingMaterial;

        // Create walls with door openings.
        // North Wall (attached to NorthWing) – horizontal wall (length along X)
        GameObject northWall = CreateWallWithDoor(ballroom.transform, "NorthWall", centralSize.x, centralSize.y, wallThickness, true, wallMaterial);
        northWall.transform.localPosition = new Vector3(0, 0, centralSize.z / 2f);

        // South Wall (attached to SouthWing) – horizontal wall
        GameObject southWall = CreateWallWithDoor(ballroom.transform, "SouthWall", centralSize.x, centralSize.y, wallThickness, true, wallMaterial);
        southWall.transform.localPosition = new Vector3(0, 0, -centralSize.z / 2f);

        // East Wall (attached to EastWing) – vertical wall (length along Z)
        GameObject eastWall = CreateWallWithDoor(ballroom.transform, "EastWall", centralSize.z, centralSize.y, wallThickness, false, wallMaterial);
        eastWall.transform.localPosition = new Vector3(centralSize.x / 2f, 0, 0);

        // West Wall (attached to WestWing) – vertical wall
        GameObject westWall = CreateWallWithDoor(ballroom.transform, "WestWall", centralSize.z, centralSize.y, wallThickness, false, wallMaterial);
        westWall.transform.localPosition = new Vector3(-centralSize.x / 2f, 0, 0);

        // Add decorative columns at the four corners.
        AddColumns(ballroom.transform, centralSize, "BallroomColumn");

        return ballroom;
    }

    #endregion

    #region Wing Construction

    /// <summary>
    /// Builds a wing room. The wing will have a door opening on the side that attaches to the central ballroom.
    /// </summary>
    GameObject BuildWing(string wingName)
    {
        GameObject wing = new GameObject(wingName);

        // Floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";
        floor.transform.parent = wing.transform;
        floor.transform.localScale = new Vector3(wingSize.x, 1, wingSize.z);
        floor.transform.localPosition = new Vector3(0, -0.5f, 0);
        if (floorMaterial != null)
            floor.GetComponent<Renderer>().material = floorMaterial;

        // Ceiling
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ceiling.name = "Ceiling";
        ceiling.transform.parent = wing.transform;
        ceiling.transform.localScale = new Vector3(wingSize.x, 1, wingSize.z);
        ceiling.transform.localPosition = new Vector3(0, wingSize.y, 0);
        if (ceilingMaterial != null)
            ceiling.GetComponent<Renderer>().material = ceilingMaterial;

        // Determine which wall attaches to the main room.
        bool isNorthWing = wingName.Contains("North");
        bool isSouthWing = wingName.Contains("South");
        bool isEastWing = wingName.Contains("East");
        bool isWestWing = wingName.Contains("West");

        // Create walls. For the wall that touches the main room, use a door opening.

        // For North wing: door wall is the SOUTH wall (local z = -wingSize.z/2)
        if (isNorthWing)
        {
            GameObject doorWall = CreateWallWithDoor(wing.transform, "SouthWall", wingSize.x, wingSize.y, wallThickness, true, wallMaterial);
            doorWall.transform.localPosition = new Vector3(0, 0, -wingSize.z / 2f);
        }
        else
        {
            // Otherwise, create a full wall.
            GameObject wall = CreateFullWall(wing.transform, "SouthWall", new Vector3(wingSize.x, wingSize.y, wallThickness), new Vector3(0, wingSize.y / 2f, -wingSize.z / 2f), wallMaterial);
        }

        // For South wing: door wall is the NORTH wall (local z = +wingSize.z/2)
        if (isSouthWing)
        {
            GameObject doorWall = CreateWallWithDoor(wing.transform, "NorthWall", wingSize.x, wingSize.y, wallThickness, true, wallMaterial);
            doorWall.transform.localPosition = new Vector3(0, 0, wingSize.z / 2f);
        }
        else
        {
            GameObject wall = CreateFullWall(wing.transform, "NorthWall", new Vector3(wingSize.x, wingSize.y, wallThickness), new Vector3(0, wingSize.y / 2f, wingSize.z / 2f), wallMaterial);
        }

        // For East wing: door wall is the WEST wall (local x = -wingSize.x/2)
        if (isEastWing)
        {
            GameObject doorWall = CreateWallWithDoor(wing.transform, "WestWall", wingSize.z, wingSize.y, wallThickness, false, wallMaterial);
            doorWall.transform.localPosition = new Vector3(-wingSize.x / 2f, 0, 0);
        }
        else
        {
            GameObject wall = CreateFullWall(wing.transform, "WestWall", new Vector3(wallThickness, wingSize.y, wingSize.z), new Vector3(-wingSize.x / 2f, wingSize.y / 2f, 0), wallMaterial);
        }

        // For West wing: door wall is the EAST wall (local x = +wingSize.x/2)
        if (isWestWing)
        {
            GameObject doorWall = CreateWallWithDoor(wing.transform, "EastWall", wingSize.z, wingSize.y, wallThickness, false, wallMaterial);
            doorWall.transform.localPosition = new Vector3(wingSize.x / 2f, 0, 0);
        }
        else
        {
            GameObject wall = CreateFullWall(wing.transform, "EastWall", new Vector3(wallThickness, wingSize.y, wingSize.z), new Vector3(wingSize.x / 2f, wingSize.y / 2f, 0), wallMaterial);
        }

        // The remaining walls (Left and Right) are created as full cubes.
        // For wings, these correspond to the walls along the shorter axis.
        // Left Wall (local x = -wingSize.x/2) if not already created.
        if (!(isEastWing || isWestWing))
        {
            CreateFullWall(wing.transform, "LeftWall", new Vector3(wallThickness, wingSize.y, wingSize.z), new Vector3(-wingSize.x / 2f, wingSize.y / 2f, 0), wallMaterial);
        }
        // Right Wall (local x = +wingSize.x/2) if not already created.
        if (!(isEastWing || isWestWing))
        {
            CreateFullWall(wing.transform, "RightWall", new Vector3(wallThickness, wingSize.y, wingSize.z), new Vector3(wingSize.x / 2f, wingSize.y / 2f, 0), wallMaterial);
        }

        // Add decorative columns at the wing's corners.
        AddColumns(wing.transform, wingSize, wingName + "Column");

        return wing;
    }

    #endregion

    #region Helper Functions

    /// <summary>
    /// Creates a wall with a centered door opening by splitting it into two segments.
    /// If horizontal is true, the wall extends along the X–axis; otherwise, along the Z–axis.
    /// </summary>
    GameObject CreateWallWithDoor(Transform parent, string wallName, float length, float height, float thickness, bool horizontal, Material mat)
    {
        GameObject wallGO = new GameObject(wallName);
        wallGO.transform.parent = parent;

        // Calculate segment lengths (both left and right segments are equal).
        float segmentLength = (length - doorWidth) / 2f;

        if (horizontal)
        {
            // Left segment.
            GameObject leftSegment = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leftSegment.name = "LeftSegment";
            leftSegment.transform.parent = wallGO.transform;
            leftSegment.transform.localScale = new Vector3(segmentLength, height, thickness);
            float leftCenterX = -(doorWidth / 2f + segmentLength / 2f);
            leftSegment.transform.localPosition = new Vector3(leftCenterX, height / 2f, 0);
            if (mat != null)
                leftSegment.GetComponent<Renderer>().material = mat;

            // Right segment.
            GameObject rightSegment = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightSegment.name = "RightSegment";
            rightSegment.transform.parent = wallGO.transform;
            rightSegment.transform.localScale = new Vector3(segmentLength, height, thickness);
            float rightCenterX = doorWidth / 2f + segmentLength / 2f;
            rightSegment.transform.localPosition = new Vector3(rightCenterX, height / 2f, 0);
            if (mat != null)
                rightSegment.GetComponent<Renderer>().material = mat;
        }
        else
        {
            // For vertical walls (extending along the Z axis).
            GameObject leftSegment = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leftSegment.name = "LeftSegment";
            leftSegment.transform.parent = wallGO.transform;
            leftSegment.transform.localScale = new Vector3(thickness, height, segmentLength);
            float leftCenterZ = -(doorWidth / 2f + segmentLength / 2f);
            leftSegment.transform.localPosition = new Vector3(0, height / 2f, leftCenterZ);
            if (mat != null)
                leftSegment.GetComponent<Renderer>().material = mat;

            GameObject rightSegment = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightSegment.name = "RightSegment";
            rightSegment.transform.parent = wallGO.transform;
            rightSegment.transform.localScale = new Vector3(thickness, height, segmentLength);
            float rightCenterZ = doorWidth / 2f + segmentLength / 2f;
            rightSegment.transform.localPosition = new Vector3(0, height / 2f, rightCenterZ);
            if (mat != null)
                rightSegment.GetComponent<Renderer>().material = mat;
        }

        return wallGO;
    }

    /// <summary>
    /// Creates a full wall (without any door opening) as a simple cube.
    /// </summary>
    GameObject CreateFullWall(Transform parent, string wallName, Vector3 scale, Vector3 localPos, Material mat)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = wallName;
        wall.transform.parent = parent;
        wall.transform.localScale = scale;
        wall.transform.localPosition = localPos;
        if (mat != null)
            wall.GetComponent<Renderer>().material = mat;
        return wall;
    }

    /// <summary>
    /// Adds decorative columns at the corners of a given rectangular area.
    /// </summary>
    void AddColumns(Transform parent, Vector3 size, string columnBaseName)
    {
        float columnHeight = size.y;
        float columnDiameter = 0.5f;
        Vector3[] positions = new Vector3[]
        {
            new Vector3(-size.x / 2f + columnDiameter, 0, -size.z / 2f + columnDiameter),
            new Vector3(size.x / 2f - columnDiameter, 0, -size.z / 2f + columnDiameter),
            new Vector3(-size.x / 2f + columnDiameter, 0, size.z / 2f - columnDiameter),
            new Vector3(size.x / 2f - columnDiameter, 0, size.z / 2f - columnDiameter)
        };

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject column = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            column.name = columnBaseName + "_" + (i + 1);
            column.transform.parent = parent;
            // Adjust the cylinder so that its full height is columnHeight.
            column.transform.localScale = new Vector3(columnDiameter, columnHeight / 2f, columnDiameter);
            column.transform.localPosition = new Vector3(positions[i].x, columnHeight / 2f, positions[i].z);
            if (wallMaterial != null)
                column.GetComponent<Renderer>().material = wallMaterial;
        }
    }

    #endregion
}
