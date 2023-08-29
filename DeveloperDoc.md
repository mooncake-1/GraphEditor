# Project Structure
The source files are contained within the Source directory of the project. It has two subdirectories:
- The Model directory contains the necessary classes and interfaces for defining the graph structure, and the graph algorithms to be used on a graph
- The Form directory contains the necessary classes that are concerned about the user interaction with the canvas for creating a graph and choosing algorithms to run on that graph

##  ../Source/Model

Contains two subdirectories:
- GraphStructure, which contains the files that define a graph, and the classes that are needed for a graph, with class names corresponding to the element of the graph or the graph itself
- Algorithms, which contains also two subdirectories, ExtremalPath and GraphTraversal
-- GraphTraversal: contains the interface and the algorithms that implement that interface that are for traversing a graph, mainly BFS and DFS
-- ExtremalPath: contains the interface and the algorithms that implement that interface that are for finding the extremal paths in a graph, in this context extremal path is the single source shortest path, mainly the DAG algorithm, Dijkstra, and Bellman-Ford



## ../Source/Form
Contains two subdirectories:
- MainForm, which contains the Form classes that are about the main interactions with the canvas, those being how the user interacts with the canvas using their mouse
- SelectionForm, which contains the Form classes that are about selection what type of the graph the user wants to create (GraphSelectionForm.cs), and what are the vertices the user wants to be involved in those algorithms (..VertexSelectionForm.cs)

## Classes
### ../Source/Model
- Vertex
Represents the individual point or node in a graph. Primarily stores its position (as a PointF) and attributes like its visibility, highlighting, etc. Has 
-- Position (X, Y coordinates)
-- Highlighted attribute
--  Other visual attributes

- Edge
Represents the connection between two vertices, which can be directed or undirected. Holds references to the two vertices it connects (From and To), and its weight. Has the layout:
-- Start Vertex (From)
-- End Vertex (To)
-- Weight (integer value)
-- Flags (isDirected)

- Graph (abstract class)
Acts as a foundation for any type of graph, encompassing operations - common to all graphs. Contains a list of vertices (_vertices) and a list of edges (_edges), with methods for manipulation, searching, and visualization. Has the layout:
-- List of Vertices
-- List of Edges

- UndirectedGraph (derived from Graph):
Represents an undirected graph. 
- DirectedGraph (derived from Graph)
Represents a directed graph.
### ../Source/Algorithms/GraphTraversal
- IGraphTraversal.cs
Defines the common operations for graph traversal algorithms, including:
-- Traversing a graph.
-- Determining the existence of a path.
-- Highlighting the identified path.

- BreadthFirstSearch.cs
Uses a Queue to manage vertices for BFS.

- DepthFirstSearch.cs
Uses a Stack and a Dictionary for DFS to track vertices and their relationships.

To extend using this interface, implementing the methods from the interface are enough.

### ../Source/Algorithms/ExtremalPath
- IExtremalPath.cs 
Defines common operations for identifying and working with extremal paths in a graph. An extremal path, in this context, refers to the shortest path between vertices in a graph. The primary operations of this interface are:

- CriticalPathAlgorith.cs
Uses a Dictionary to track distances, previous vertices, and paths; and a Stack and HashSet for topological sorting of the graph vertices. 

- Dijkstra.cs
Uses a Dictionary to store distances and previous vertex relations, and a SortedSet for managing vertices based on distances. Employs an IComparer implementation (DistanceComparer) to compare vertices by their distances.

- BellmanFord.cs
Uses a Dictionary to manage distances, previous vertex relations, and shortest paths.

To extend using this interface, although not required by the interface, it is recommended to implement these two common methods apart from the required methods in the interface:

- Initialize: 
-- Sets starting conditions for vertex distances and states 
-- Resets the graph to a clean state for shortest path finding
-- Prepares the data structures that will be required during the algorithm's execution.
- Relax: Checks if the shortest path to a vertex can be improved by going through another vertex. If it can, updates the shortest path and possibly improve an already discovered path.

