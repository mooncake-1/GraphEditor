# GraphEditor
## Overview
GraphEditor is a visual tool designed to create, manipulate, and analyze graphs. You can add vertices and edges, modify edge weights, and apply various graph traversal and pathfinding algorithms.

## Features
### 1. Graph Creation
- **Directed/Undirected Graphs:** Choose between creating a directed or undirected graph.
- **Vertex Management:** Add vertices by left-clicking on the canvas. The vertices are automatically numbered.
- **Edge Management:** Drag from one vertex to another to create an edge. Right-click to remove a vertex or an edge.
- **Weight Modification:** Click on an edge to modify its weight.
### 2. Graph Visualization
- **Canvas Interaction:** Zoom, pan, and interact with the graph in real time.
- **Visual Feedback:** Real-time updates during graph traversals, showing the current state of the algorithm.
### 3. Graph Traversal
- **Breadth-First Search (BFS):** Apply BFS to traverse the graph.
- **Depth-First Search (DFS):** Apply DFS to traverse the graph.
- **Path Checking:** Check if a path exists between two selected vertices.
### 4. Extremal Path Algorithms
- **Dijkstra's Algorithm:** Find the shortest paths from a selected start vertex.
- **Bellman-Ford Algorithm:** Apply Bellman-Ford to find shortest paths.
- **Critical Path Algorithm:** Utilize the critical path algorithm to identify critical paths.
## Requirements
- Windows operating system
- .NET Framework 4.7.2 or higher
## Usage
### Getting Started
1. **Launch the Application:** Start the GraphEditor application.
1. **Select Graph Type:** Choose between a directed or undirected graph.
1. **Manipulate the Graph:** Use the mouse to add vertices and edges and manipulate the graph.
### Graph Operations
- **Left-Click:** Add a new vertex or select an edge to modify its weight.
- **Left-Click + Drag:** Draw a new edge between vertices.
- **Right-Click:** Remove a vertex or edge.
### Applying Algorithms
  1. **Select Algorithm from Menu:** Choose the desired traversal or pathfinding algorithm from the menu.
  1. **Select Start Vertex:** Some algorithms such as traversal and shortest path will prompt for a start vertex.
  1. **Select Start and End Vertex (if required):** For algorithms like checking the path, you will need to select both a start and an end vertex. 
  1. **Observe the Algorithm in Action:** Watch the algorithm traverse the graph and see the results on the canvas.
