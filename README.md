EpPathFinding.cs
================
#### A jump point search algorithm for grid based games in C# ####

Introduction
------------
This project was started after I was inspired by [PathFinding.js by Xueqiao Xu](https://github.com/qiao/PathFinding.js) and [the article by D. Harabor](http://harablog.wordpress.com/2011/09/07/jump-point-search/).
It comes along with a demo to show how the agorithm execute as similar to Xueqiao Xu's [Online Demo](http://qiao.github.com/PathFinding.js/visual).

Basic Usage
------------
The usage and the demo has been made very similar to [PathFinding.js](https://github.com/qiao/PathFinding.js) for ease of usage.  

You first need to build a grid-map. (For example: width 64 and height 32): 


```c#
BaseGrid searchGrid = new StaticGrid(64, 32);
```


By default, every nodes in the grid will allow to be walked through. To set whether a node at a given coordinate is walkable or not, use the `SetWalkableAt` function.

For example, in order to set the node at (10 , 20) to be un-walkable, where 10 is the x coordinate (from left to right), and 20 is the y coordinate (from top to bottom): 


```c#
searchGrid.SetWalkableAt(10, 20, false);
 
// OR
 
searchGrid.SetWalkableAt(new GridPos(10,20),false);  
```


You may also use in a 2-d array while instantiating the `StaticGrid` class. It will initiate all the nodes in the grid with the walkability indicated by the array. (`true` for walkable otherwise not walkable): 


```c#
bool [][] movableMatrix = new bool [width][];
for(int widthTrav=0; widthTrav< 64; widthTrav++)
{
   movableMatrix[widthTrav]=new bool[height];
   for(int heightTrav=0; heightTrav < 32;  heightTrav++)
   { 
      movableMatrix[widthTrav][heightTrav]=true; 
   }  
}

Grid searchGrid = new StaticGrid(64,32, movableMatrix);
```


In order to search the route from (10,10) to (20,10), you need to create `JumpPointParam` class with grid and start/end positions. (Note: both the start point and end point must be walkable): 


```c#
GridPos startPos=new GridPos(10,10); 
GridPos endPos = new GridPos(20,10);  
JumpPointParam jpParam = new JumpPointParam(searchGrid,startPos,endPos ); 
```


You can also set/change the start and end positions later. (However the start and end positions must be set before the actual search): 


```c#
JumpPoinParam jpParam = new JumpPointParam(searchGrid);
jpParam.Reset(new GridPos(10,10), new GridPos(20,10)); 
```


To find a path, simply run `FindPath` function with `JumpPointParam` object created above: 


```c#
List<GridPos> resultPathList = JumpPointFinder.FindPath(jpParam); 
```


`JumpPointParam` class can be used as much as you want with different start/end positions unlike [PathFinding.js](https://github.com/qiao/PathFinding.js): 


```c#
jpParam.Reset(new GridPos(15,15), new GridPos(20,15));
resultPathList = JumpPointFinder.FindPath(jpParam); 
```


Advanced Usage
------------
#### Cross Corners ####
When instantiating the `JumpPointParam`, you may pass in additional parameters to indicate specific strategies to use.  

In order to make search able to walk diagonally across corner of two diagonal unwalkable nodes:   


```c#
JumpPointParam jpParam = new JumpPointParam(searchGrid,true);   
```


To make it unable to walk diagonally across two diagonal unwalkable corners: 


```c#
JumpPointParam jpParam = new JumpPointParam(searchGrid,false);   
```


#### Heuristic Functions ####
The predefined heuristics are `Heuristic.EUCLIDEAN` (default), `Heuristic.MANHATTAN`, and `Heuristic.CHEBYSHEV`.   

To use the `MANHATTAN` heuristic:


```c#
JumpPointParam jpParam = new JumpPointParam(searchGrid,true, Heuristic.MANHATTAN); 
```


You can always change the heuristics later with `SetHeuristic` function: 


```c#
jpParam.SetHeuristic(Heuristic.MANHATTAN);
```


#### Dynamic Grid ####

For my grid-based game, I had much less walkable grid nodes than un-walkable grid nodes. So above `StaticGrid` was wasting too much memory to hold un-walkable grid nodes. To avoid the memory waste, I have created `DynamicGrid`, which allocates the memory for only walkable grid nodes.


```c#
BaseGrid seachGrid = new DynamicGrid();  
```


You may also use a List of walkable GridPos, while instantiating the DynamicGrid class. It will initiate only the nodes in the grid where the walkability is true:

```c#
List<GridPos> walkableGridPosList= new List<GridPos>();
for(int widthTrav=0; widthTrav< 64; widthTrav++)
{
   movableMatrix[widthTrav]=new bool[height];
   for(int heightTrav=0; heightTrav < 32;  heightTrav++)
   {
      walkableGridPosList.Add(new GridPos(widthTrav, heightTrav));
   }
}

BaseGrid searchGrid = new DynamicGrid(walkableGridPosList);  
```


Rest of the functionality like `SetWalkableAt`, `Reset`, etc. are same as StaticGrid. 

#### Extendability ####
You can also create a sub-class of `BaseGrid` to create your own way of `Grid` class to best support your situation.

License
-------

[The MIT License](http://opensource.org/licenses/mit-license.php)

Copyright (c) 2013 Woong Gyu La <[juhgiyo@gmail.com](mailto:juhgiyo@gmail.com)>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.