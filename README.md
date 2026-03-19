# QuadTree Optimization Demo — Unity

A hands-on Unity project demonstrating the performance difference between brute-force nearest-neighbor search and QuadTree-accelerated spatial queries.

## What It Does

A player character moves across a plane populated with 10,000 randomly spawned objects. Every frame, the game finds the **nearest object** to the player and highlights it.

Two modes can be toggled at runtime:

| Mode | Distance Checks / Frame |
|---|---|
| Brute Force | 10,000 |
| QuadTree | ~ 140 : 200 |

That's a **~98% reduction** in distance checks — same correct result, much less work.

## How It Works

### Brute Force
Every frame, iterate over all 10,000 objects and compute `Vector3.Distance` for each one.

### QuadTree
The world is spatially partitioned into a QuadTree. Each node has a capacity — when it fills up, it subdivides into 4 child nodes (NorthEast, NorthWest, SouthEast, SouthWest).

Each frame, the player queries only the nodes that intersect their AABB search region. Only objects inside those nodes get distance-checked.

```
Root (whole world)
├── NorthEast
│   ├── NE_NE  ← only this region intersects player AABB
│   └── ...
├── NorthWest  ← skipped entirely
├── SouthEast  ← skipped entirely
└── SouthWest  ← skipped entirely
```

## How to Run

1. Open the project in Unity (tested on Unity 6)
2. Open the main scene
3. Press Play
4. Move the player with WASD
5. Toggle **"Enable Quad Tree Optimization"** in the top-left UI to switch modes live
6. Watch the **Check Distance Count** drop from 10,000 to ~140

### Demonstration Video
https://github.com/user-attachments/assets/ddb7b506-3b24-4660-8903-49dc378b7c8f