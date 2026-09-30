# Unity Technical Assessment

This is my submission for the Unity assessment. It has three small prototypes, each in its own scene.

**Built with:** Unity 6.3 LTS (6000.3.13f1), C#, TextMeshPro. The project uses the New Input Manager.

## How to run

1. Open the project in Unity 6000.3.13f1.
2. Go to Project Settings > Player and check that Active Input Handling is set to **New**.
3. Import TMP Essentials if Unity asks for it.
4. Open any scene from `Assets/Scenes` and press Play. You can also run the standalone build.

**Controls:** WASD to move, Space to jump. On the enemy scene, Numpad 1 to 4 switch modes. The keys are set on the state assets, so you can change them there.

| Scene | What it does |
|---|---|
| Artifact_Scene | Walk near an artifact and a panel shows its info |
| Enemy_Scene | Enemy switches between Idle, Patrol, Attack and Flee |
| Table_Scene | Builds a table from row/column inputs, with an optional header |

## Folder structure

```
Scripts/
  Core/      IInspectable, Artifact, InspectionEvents
  Content/   ContentBlock (abstract), Text / Image / BulletList blocks, UIFactory
  Player/    PlayerController, FollowCamera, InspectionSensor
  UI/        InspectorPanel
  Enemy/     EnemyState (abstract), Idle / Patrol / Attack / Flee states,
             EnemyStateMachine, EnemyContext, EnemyController, EnemyStateHud
  Table/     DynamicTable, TableCellView, TableStyle,
             ITableContentProvider, CoordinateContentProvider
```

---

## 1. Artifact Inspector (Task_Scene)

Move the cube close to an artifact and a panel fills up with that artifact's content. Walk away and the panel clears.

There were two requirements. Each artifact has to define its own content, and I should be able to add new kinds of UI later without touching existing code. So I split it up like this:

- **`IInspectable`** is the interface that the sensor and the panel work with. It only exposes a title, a position and a list of content blocks. `Artifact` implements it, but anything else could implement it too.
- **Content blocks** are ScriptableObjects. Each block is one piece of UI (text, image, bullet list) and knows how to build itself through `Build(parent)`. An artifact just holds a list of these, set in the Inspector.
- **`InspectionSensor`** sits on the player. It checks for the nearest inspectable about 10 times a second, using a non-alloc overlap query so it creates no garbage.
- When the focused artifact changes, the sensor fires an event through **`InspectionEvents`**. **`InspectorPanel`** listens for it, clears itself and builds the new blocks.

This way the sensor has no idea the panel exists. The panel also doesn't care what type of blocks it receives.

**Adding a new UI type** (say, a button) takes three steps:

1. Write `ButtonContentBlock : ContentBlock` and implement `Build`.
2. Create an asset from it.
3. Add the asset to any artifact's list.

Nothing else needs to change.

A few choices I made along the way:

- **Overlap query instead of trigger colliders.** Triggers with a CharacterController can be fiddly. With an overlap query, picking the nearest artifact is easy when two are close together, and artifacts don't need any physics setup.
- **UI is built at runtime, with no prefabs.** This keeps it simple for a project of this size. In a real production project I'd switch to prefab-based views with pooling.
- **The event bus is static.** It resets on domain reload, which is fine here. For a bigger project I'd use ScriptableObject event channels or dependency injection to avoid global state.

---

## 2. Enemy Behaviours (Enemy_Scene)

This is the State pattern. Each behaviour is an `EnemyState` ScriptableObject with `Enter`, `Tick` and `Exit`. Each state also stores its own display name, activation key and tint colour.

- **`IdleState`** does nothing, **`PatrolState`** walks between patrol points, and **`AttackState`** chases the player.
- **`EnemyContext`** holds what a state is allowed to use: the transform, the target, the patrol points and a movement helper. Because of this, states don't depend on the controller directly.
- **`EnemyStateMachine`** keeps track of the current state, ticks it and raises a `StateChanged` event.
- **`EnemyController`** holds the list of state assets, checks their keys and applies the tint. It doesn't know about any specific behaviour.
- **`EnemyStateHud`** shows the current mode and a key legend, both generated from the same state list.

To make sure the "add behaviour without editing old code" requirement actually held up, I added `FleeState` at the end. I wrote one class, created the asset, gave it a key and added it to the enemy's list. No existing files changed, and the HUD picked it up automatically.

Some notes on this part:

- **States are cloned per enemy at runtime.** Runtime data like the patrol index is never shared between two enemies.
- **Keys live on the state assets,** so even changing the input mapping doesn't need a code change.
- **Transitions are manual (keypress),** since that's what the task asked for. For automatic switching I'd add transition conditions on each state, like "player within range".

---

## 3. Dynamic Table (Table_Scene)

Enter the number of rows and columns and the table gets generated. A toggle turns the header row on or off.

- **`DynamicTable`** is the main script. It reads the inputs, validates them and builds the grid.
- **`TableCellView`** is the cell prefab. It only displays text and colours.
- **`TableStyle`** is a ScriptableObject that holds cell size, spacing and colours, so the look can change without touching code.
- **`ITableContentProvider`** decides what text goes in each cell. The default provider just shows coordinates.

How it works:

- **Layout:** `GridLayoutGroup` with a fixed column count, plus a `ContentSizeFitter`, inside a `ScrollRect`, so bigger tables can scroll.
- **Pooling:** when you regenerate, existing cells are reused and extra ones are hidden instead of being destroyed and re-instantiated.
- **Validation:** only whole numbers from 1 to 30 are accepted. Anything else shows an error message on screen.
- **Header:** the header is an extra row on top of the requested rows, and it has its own style.
- **Real data:** to show real data, write a class that implements `ITableContentProvider` and assign it to `table.ContentProvider`. `DynamicTable` doesn't need any changes.

---

## Patterns used

- **Observer:** `InspectionEvents` and the `StateChanged` event.
- **Strategy / Template Method:** `ContentBlock.Build()`.
- **State:** the enemy behaviours.
- **ScriptableObjects as data:** content blocks, enemy states and table style.
- **Interfaces:** `IInspectable` and `ITableContentProvider`.
- **Object pooling:** the table cells.

---

## AI usage

The assignment allowed AI tools, so here's how I used them.

**Architecture and logic.** I designed the architecture and logic myself. For all three problems I decided:

- how the systems are structured and which classes exist;
- which patterns to use and why, such as ScriptableObject-based states for the enemy instead of an enum switch;
- how each system should work: the sensor finding the nearest artifact, the event flow to the panel, how states switch, how the table reuses cells, and the validation rules.

**Code.** I used an AI assistant only to write the code for the logic I asked for. I described what each class should do and how it should work, and it wrote the C#. I reviewed everything before bringing it into the project.

**Everything else was done by me:**

- I built all three scenes: ground, player, artifacts, canvas and panel, the enemy and its patrol points, the table UI and the cell prefab.
- I created and configured all the content assets (text, images, bullet lists) and the state assets.
- I tested everything in the Editor and fixed the integration problems (sprite import mode, layout group settings, input keys), then tweaked the UI.
- I added `FleeState` to check that new behaviours really could be added without changing old code.

I'm happy to walk through any class and explain why it's built the way it is.
