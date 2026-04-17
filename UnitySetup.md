# Pivot-Rhythm: Unity Editor Setup Guide

This guide will walk you through setting up the necessary GameObjects, Prefabs, and scene configurations to run the "Pivot-Rhythm" game, based on the scripts already created.

---

### **1. Scene & Core Managers Setup**

1.  **Open the Main Scene:** Open your main game scene (e.g., `SampleScene`).
2.  **GameManager:**
    *   Create an empty GameObject in your scene and name it `GameManager`.
    *   Drag the `GameManager.cs` script onto this GameObject.
3.  **Spawner:**
    *   Create another empty GameObject and name it `Spawner`.
    *   Drag the `Spawner.cs` script onto this GameObject.
4.  **Main Camera:**
    *   Select your `Main Camera`.
    *   Set its **Projection** to `Orthographic`.
    *   Set its **Background** color to a dark grey or black for better contrast.
    *   Ensure its position is `(0, 0, -10)`.

---

### **2. Creating the `Line` Prefab**

The `Line` needs to be a Prefab so the `Spawner` can instantiate it.

1.  **Create the Line GameObject:**
    *   In the Hierarchy, create a new 2D Object > Sprite > Square. Name it `Line`.
    *   Set its **Scale** to something like `(8, 0.2, 1)` to make it a long, thin line.
    *   Set its **Tag** to `Line`. If the tag doesn't exist, create it via `Add Tag...`.
2.  **Add Components:**
    *   **`Line.cs`:** Drag the script onto the `Line` GameObject.
    *   **`Rigidbody 2D`:** Add this component. Set its **Body Type** to `Kinematic`. This allows it to be moved by script without being affected by physics forces.
    *   **`Box Collider 2D`:** Add this component. Check the **`Is Trigger`** box. This is crucial for the `OnTriggerEnter2D` calls in the `Side.cs` script.
3.  **Create the Prefab:**
    *   Create a `Prefabs` folder in your `Assets` directory.
    *   Drag the `Line` GameObject from your Hierarchy into the `Prefabs` folder. It should turn blue in the Hierarchy.
4.  **Link Prefab to Spawner:**
    *   Select the `Spawner` GameObject in your scene.
    *   You will see a `Line Prefab` field in the Inspector. Drag the newly created `Line` Prefab from your `Prefabs` folder into this field.
5.  **Clean Up:** You can now delete the `Line` GameObject from your Hierarchy, as it will be spawned from the Prefab.

---

### **3. Creating the `Player` GameObject**

The Player is a central pivot with four colored sides.

1.  **Create the Pivot:**
    *   Create an empty GameObject, name it `PlayerPivot`.
    *   Reset its position to `(0, 0, 0)`.
    *   Drag the `CubeController.cs` script onto `PlayerPivot`.
2.  **Create the Sides:**
    *   Create a new 2D Object > Sprite > Square as a **child** of `PlayerPivot`. Name it `Side_Top`.
    *   **Position** it at `(0, 1.1, 0)` and **Scale** it to `(2, 0.2, 1)`.
    *   Duplicate `Side_Top` three times for the other sides.
3.  **Configure Each Side:**
    *   **`Side_Top` (Red):**
        *   **Position:** `(0, 1.1, 0)`, **Scale:** `(2, 0.2, 1)`
        *   Add `Side.cs` script. Set `Side Color` to **Red**.
        *   Add `Box Collider 2D`.
    *   **`Side_Bottom` (Blue):**
        *   **Position:** `(0, -1.1, 0)`, **Scale:** `(2, 0.2, 1)`
        *   Add `Side.cs` script. Set `Side Color` to **Blue**.
        *   Add `Box Collider 2D`.
    *   **`Side_Right` (Green):**
        *   **Position:** `(1.1, 0, 0)`, **Scale:** `(0.2, 2.2, 1)`
        *   Add `Side.cs` script. Set `Side Color` to **Green**.
        *   Add `Box Collider 2D`.
    *   **`Side_Left` (Yellow):**
        *   **Position:** `(-1.1, 0, 0)`, **Scale:** `(0.2, 2.2, 1)`
        *   Add `Side.cs` script. Set `Side Color` to **Yellow**.
        *   Add `Box Collider 2D`.

    *Your `PlayerPivot` hierarchy should now look like this:*
    ```
    PlayerPivot
    ├── Side_Top
    ├── Side_Bottom
    ├── Side_Right
    └── Side_Left
    ```

---

### **4. Final Checks & Running the Game**

1.  **Save Scene:** Save your scene file (`File > Save`).
2.  **Check Spawner:** Ensure the `Spawner` has the `Line` prefab linked.
3.  **Check Player:** Ensure the `PlayerPivot` has the `CubeController` script and that each child `Side` has the `Side.cs` script with the correct color set in the inspector.
4.  **Play!** Press the Play button. Lines should spawn, the cube should rotate with `A`/`D`, and you should be able to score by pressing `Space` at the right time.

If you encounter issues, use the Console window to check for errors. The most common setup error is forgetting to link the `Line` prefab to the `Spawner`.
