This is a **Project Context Document** designed to be pasted into any LLM or Code Assistant. It defines the core logic, architecture, and constraints for your Minimum Viable Product (MVP).

---

## **Project Brief: "Pivot-Rhythm" (MVP)**

### **1. Core Concept**
A 2D top-down rhythm/reaction game. A central 4-sided cube (the Player) must rotate to match the color of incoming lines (Notes) arriving from the four cardinal directions (Top, Bottom, Left, Right). 

### **2. Technical Stack**
* **Engine:** Unity 2D (Universal Render Pipeline/2D Core).
* **Language:** C#.
* **Operating System:** Arch Linux (Development Environment).
* **Input:** Keyboard (`A`/`D`, `Arrow Keys`, `Space`).

### **3. Mechanics & Game Logic**
* **The Player:** A central pivot point with 4 child objects representing the sides. Each side has a distinct color (Red, Blue, Green, Yellow).
* **Movement:** * Rotation is **smooth** (not instant snapping) using `Lerp` or `MoveTowards`.
    * Input: `A` or `Left Arrow` rotates the cube 90° CCW; `D` or `Right Arrow` rotates it 90° CW.
* **The Notes (Lines):** * Lines spawn at the screen edges and move toward $(0, 0, 0)$.
    * Each line has a color matching one of the 4 cube sides.
    * Only **one line** exists on screen at a time (sequential spawning).
* **The Interaction:**
    * **The Hit Window:** A Trigger-based collision zone around the cube.
    * **Success Condition:** Player presses `Space` while a line is inside the trigger **AND** the line's color matches the color of the cube side it is currently touching.
    * **Failure Condition:** 1. Pressing `Space` with a color mismatch.
        2. Pressing `Space` outside the hit window.
        3. A line passing through the cube without `Space` being pressed.
* **Difficulty Scaling:** * Speed increases exponentially with every successful hit.
    * No maximum speed (infinite scaling until "theoretically impossible").

### **4. MVP Architecture (The Bare Minimum)**
* **`GameManager.cs`**: Static variables for `GlobalSpeed` and `Score`. Handles `GameOver()` (Scene reload).
* **`CubeController.cs`**: Handles smooth `Quaternion` rotation logic based on user input.
* **`Spawner.cs`**: Simple timer-based instantiation of "Line" prefabs. Randomly selects direction (Top/Down/Left/Right) and Color.
* **`Line.cs`**: Logic for movement, trigger detection, and color-matching verification.

### **5. Visuals & Assets**
* **Player:** 4 thin rectangular sprites forming a square frame.
* **Lines:** Simple 2D rectangular sprites.
* **UI:** High Score display and a large "Retry" button on Game Over.

---

### **Prompt to give your Code Assistant:**
> "I am building a Unity 2D rhythm game. I have a central cube with 4 colored sides that rotates 90 degrees smoothly via A/D keys. Lines spawn from screen edges and move to the center. I need to implement a Trigger-based hit system where the player must press Space when the line overlaps the matching color side. Based on the **Project Context** provided, help me write the `Line.cs` logic to detect if the color of the line matches the color of the specific side of the cube it is colliding with at the moment of the Space press."
