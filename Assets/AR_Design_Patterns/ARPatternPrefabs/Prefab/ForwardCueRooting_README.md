# Forward Cue Rooting Pattern

## 🧩 Description
The "Forward Cue Rooting" pattern is an AR interaction design pattern that visually guides users through a sequence of spatial waypoints. It leverages animated circular cues to indicate direction and progression, helping users follow a predefined path in augmented environments.

## 🎯 Interaction Goal
To intuitively guide users from one point of interest to another in a spatial AR experience, using visual cues that indicate the next target or location.

## 🛠️ Components

- **Prefab**: `ForwardCueRootingPrefab.prefab`
- **Main Script**: `CircleDrawer.cs`
- **Waypoint Script**: `WayNode.cs`
- **Supporting Scripts**:
  - `ResetFunctionManager.cs` (if present)
  - `PatternManager.cs` (if used for event broadcasting)
- **Materials & Shaders**: uses Unity's built-in colored line shader

## 🧪 Usage Instructions

1. Drag `ForwardCueRootingPrefab.prefab` into your scene.
2. Add waypoint child objects under the prefab (with `WayNode` components).
3. Configure parameters in the inspector:
   - Segment size
   - Circle spawn rate
   - Path width and appearance
4. Assign `MainCamera` tag to your AR camera to enable trigger detection.
5. Press Play (or build) to see animated cues guiding the user.

## 🔄 Dependencies

- This pattern requires a `MainCamera` with the `tag: "MainCamera"` for trigger events.
- If using `PatternManager`, make sure it's included and correctly configured.
- Waypoints should have `BoxCollider` and `Rigidbody` (set as trigger + kinematic).

## 📸 Screenshot (Optional)
_Add an image or short gif here to illustrate the pattern in action._

## 🧠 Research Context

This pattern is part of a design pattern system for AR experiences on HMDs, particularly tailored for museum and cultural heritage applications.

---

**Author**: Yu Liu  
**Affiliation**: Hochschule RheinMain