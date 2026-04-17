# Comprehensive UI Setup Guide for Pivot-Rhythm

This guide provides detailed, click-by-click instructions to create the Start Menu, Game Over screen, and Score display, and link them to the existing scripts.

---

### **Part 1: The UIManager GameObject**

This object will control all of our UI panels and text.

1.  **Create the GameObject:**
    *   In the **Hierarchy** panel (the list of all items in your scene on the left), right-click on an empty area.
    *   From the context menu, select **Create Empty**.
    *   A new `GameObject` will appear. Select it, and in the **Inspector** panel on the right, rename it to `UIManager`.
2.  **Assign the Script:**
    *   With the `UIManager` GameObject still selected, go to the **Project** panel (usually at the bottom of the screen).
    *   Navigate to `Assets > Scripts`.
    *   Click and drag the `UIManager.cs` script from the Project panel onto the Inspector for your `UIManager` GameObject.

---

### **Part 2: The Canvas**

The Canvas is the root object for all UI elements.

1.  **Create the Canvas:**
    *   Right-click in an empty area of the **Hierarchy**.
    *   Navigate to **UI > Canvas**.
    *   This will create two new objects: `Canvas` and `EventSystem`. Both are required. Do not delete the `EventSystem`.

---

### **Part 3: The Score Display**

This text will show the score while playing.

1.  **Create the Text Object:**
    *   In the **Hierarchy**, right-click on the `Canvas` object you just created.
    *   Navigate to **UI > Legacy > Text**.
    *   A `Text` object will appear as a child of the Canvas. Rename it to `ScoreText`.
2.  **Style and Position the Text:**
    *   Select `ScoreText` in the Hierarchy.
    *   In the **Inspector**, find the **Rect Transform** component. Click on the square anchor icon.
    *   While holding **Alt + Shift**, click the **top-center** box. This anchors it to the top-center of the screen.
    *   In the **Text** component below, change the following properties:
        *   **Text:** "Score: 0"
        *   **Font Size:** `24`
        *   **Alignment:** Center and Middle
        *   **Color:** White

---

### **Part 4: The Start Menu Panel**

This panel will hold our "Play" button.

1.  **Create the Panel:**
    *   Right-click on the `Canvas` in the Hierarchy.
    *   Navigate to **UI > Panel**.
    *   A `Panel` object will appear. Rename it to `StartMenuPanel`.
    *   By default, the panel has a semi-transparent white background. You can adjust its color or disable the `Image` component in the Inspector if you prefer.
2.  **Create the Play Button:**
    *   Right-click on `StartMenuPanel` in the Hierarchy.
    *   Navigate to **UI > Legacy > Button**.
    *   Rename the new `Button` object to `PlayButton`.
    *   Expand the `PlayButton` in the Hierarchy to see its child `Text` object. Select this `Text` object and change its **Text** property in the Inspector to "PLAY".
3.  **Hook up the Button's OnClick Event:**
    *   Select the `PlayButton` in the Hierarchy.
    *   In the Inspector, scroll down to the **Button** component and find the **On Click ()** section.
    *   Click the **`+`** icon at the bottom right of this section.
    *   A new event slot will appear. Drag the `UIManager` GameObject from your Hierarchy into the `None (Object)` field.
    *   Click the dropdown menu that currently says "No Function".
    *   Navigate to **UIManager > OnPlayButton ()**.

---

### **Part 5: The Game Over Panel**

This panel will hold the "Retry" button and the final score.

1.  **Create the Panel:**
    *   Right-click on the `Canvas` in the Hierarchy.
    *   Navigate to **UI > Panel**.
    *   Rename it to `GameOverPanel`.
2.  **Create the Final Score Text:**
    *   Right-click on `GameOverPanel` in the Hierarchy.
    *   Navigate to **UI > Legacy > Text**.
    *   Rename it `FinalScoreText`.
    *   Style it similarly to the main score text, but you might want to make it larger (e.g., Font Size `32`). Change its text to "Final Score: 0".
3.  **Create the Retry Button:**
    *   Right-click on `GameOverPanel` in the Hierarchy.
    *   Navigate to **UI > Legacy > Button**.
    *   Rename it `RetryButton`.
    *   Change its child `Text` object's text to "RETRY".
4.  **Hook up the Button's OnClick Event:**
    *   Select the `RetryButton`.
    *   In the **On Click ()** section, click `+`.
    *   Drag the `UIManager` GameObject into the object field.
    *   From the function dropdown, select **UIManager > OnRetryButton ()**.

---

### **Part 6: Final Linking**

The last step is to tell the `UIManager` which GameObjects to control.

1.  **Select the `UIManager`** GameObject in the Hierarchy.
2.  In the **Inspector**, you will see several empty fields in the `UIManager (Script)` component.
3.  **Drag and Drop:**
    *   Drag `StartMenuPanel` from the Hierarchy into the **Start Menu Panel** field.
    *   Drag `GameOverPanel` from the Hierarchy into the **Game Over Panel** field.
    *   Drag `ScoreText` from the Hierarchy into the **Score Text** field.
    *   Drag `FinalScoreText` from the Hierarchy into the **Final Score Text** field.

Your `UIManager` Inspector should now look like this:

```
[ UIManager (Script) ]
  UI Panels
    Start Menu Panel  ->  StartMenuPanel
    Game Over Panel   ->  GameOverPanel
  UI Text
    Score Text        ->  ScoreText
    Final Score Text  ->  FinalScoreText
```

**You are now done!** When you press Play, the script will automatically manage showing and hiding these panels based on the game's state.
