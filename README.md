[![Gratipay][gratipay-image]][gratipay-url]
EpPathFinding.cs
================
#### A jump point search algorithm for grid based games in C# ####

For 3D Environment
-----------------
You may take a look at [EpPathFinding3D.cs](https://github.com/juhgiyo/EpPathFinding3D.cs)

Introduction
------------
This project was started after I was inspired by [PathFinding.js by Xueqiao Xu](https://github.com/qiao/PathFinding.js) and [the article by D. Harabor](http://harablog.wordpress.com/2011/09/07/jump-point-search/).
It comes along with a demo to show how the agorithm execute as similar to Xueqiao Xu's [Online Demo](http://qiao.github.com/PathFinding.js/visual).

Unity Integration Guide
------------

Copy `EpPathFinding.cs\PathFinder` folder intto your `Unity Project's Assets` folder. Then within the script file, you want to use the `EpPathFinding.cs`, just add `using EpPathFinding.cs;` namespace at the top of the file, and use it as the guide below.


(If you have a problem when compiling, please refer to [Unity Forum](http://forum.unity3d.com/threads/monodevelop-problems-with-default-parameters.67867/#post-898994)) 


Also `EpPathFinding.cs` depends on [C5](https://github.com/sestoft/C5).

Pre-compiled C5.dll for Unity is included in `EpPathFinding3D.cs\PathFinder\UnityC5` folder.

(Please refer to [C5 on Unity3D](https://github.com/sestoft/C5#c5-on-unity3d), if you have any dependency issue with C5.)


Nuget Package
------------
[Nuget Package](https://www.nuget.org/packages/EpPathFinding.cs/)


Basic Usage
------------
The usage and the demo has been made very similar to [PathFinding.js](https://github.com/qiao/PathFinding.js) for ease of usage.  

You first need to build a grid-map. (For example: width 64 and height 32): 


```c#
BaseGrid searchGrid = new StaticGrid(64, 32);
```


By default, every nodes in the grid will NOT allow to be walked through. To set whether a node at a given coordinate is walkable or not, use the `SetWalkableAt` function.

For example, in order to set the node at (10 , 20) to be walkable, where 10 is the x coordinate (from left to right), and 20 is the y coordinate (from top to bottom): 


```c#
searchGrid.SetWalkableAt(10, 20, true);
 
// OR
 
searchGrid.SetWalkableAt(new GridPos(10,20),true);  
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

#### Find the path even the end node is unwalkable  ####
When instantiating the `JumpPointParam`, you may pass in additional parameter to make the search able to find path even the end node is unwalkable grid:   
```
Note that it automatically sets to true as a default when the parameter is not specified.
```

```c#
JumpPointParam jpParam = new JumpPointParam(searchGrid,true);
```


If `iAllowEndNodeUnWalkable` is false the FindPath will return the empty path if the end node is unwalkable: 


```c#
JumpPointParam jpParam = new JumpPointParam(searchGrid,false);   
```


#### DiagonalMovement.Always (Cross Adjacent Point) ####

In order to make search able to walk diagonally across corner of two diagonal unwalkable nodes:   


```c#
JumpPointParam jpParam = new JumpPointParam(searchGrid,true,DiagonalMovement.Always);   
```


#### DiagonalMovement.IfAtLeastOneWalkable (Cross Corner) ####
When instantiating the `JumpPointParam`, to make the search able to walk diagonally when one of the side is unwalkable grid:  

```c#
JumpPointParam jpParam = new JumpPointParam(searchGrid,true,DiagonalMovement.IfAtLeastOneWalkable);   
```


#### DiagonalMovement.OnlyWhenNoObstacles ####

To make it unable to walk diagonally when one of the side is unwalkable and rather go around the corner: 


```c#
JumpPointParam jpParam = new JumpPointParam(searchGrid,true,DiagonalMovement.OnlyWhenNoObstacles);   
```

#### DiagonalMovement.Never ####

To make it unable to walk diagonally: 


```c#
// Special thanks to Nil Amar for the idea!
JumpPointParam jpParam = new JumpPointParam(searchGrid,true,DiagonalMovement.Never);   
```


#### Heuristic Functions ####
The predefined heuristics are `Heuristic.EUCLIDEAN` (default), `Heuristic.MANHATTAN`, and `Heuristic.CHEBYSHEV`.   

To use the `MANHATTAN` heuristic:


```c#
JumpPointParam jpParam = new JumpPointParam(searchGrid,true, DiagonalMovement.Always, Heuristic.MANHATTAN); 
```


You can always change the heuristics later with `SetHeuristic` function: 


```c#
jpParam.SetHeuristic(Heuristic.MANHATTAN);
```


#### Dynamic Grid ####

For my grid-based game, I had much less walkable grid nodes than un-walkable grid nodes. So above `StaticGrid` was wasting too much memory to hold un-walkable grid nodes. To avoid the memory waste, I have created `DynamicGrid`, which allocates the memory for only walkable grid nodes. 


(Please note that there is trade off of memory and performance. This degrades the performance to improve memory usage.)


```c#
BaseGrid seachGrid = new DynamicGrid();  
```


You may also use a `List` of walkable `GridPos`, while instantiating the `DynamicGrid` class. It will initiate only the nodes in the grid where the walkability is `true`:

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


Rest of the functionality like `SetWalkableAt`, `Reset`, etc. are same as `StaticGrid`. 


#### Dynamic Grid With Pool ####

In many cases, you might want to reuse the same grid for next search. And this can be extremely useful when used with `PartialGridWPool`, since you don't have to allocate the grid again.


```c#
NodePool nodePool = new NodePool();
BaseGrid seachGrid = new DynamicGridWPool(nodePool);  
```


Rest of the functionality like `SetWalkableAt`, `Reset`, etc. are same as `DynamicGrid`. 


#### Partial Grid With Pool ####

As mentioned above, if you want to search only partial of the grid for performance reason, you can use `PartialGridWPool`. Just give the `GridRect` which shows the portion of the grid you want to search within.


```c#
NodePool nodePool = new NodePool();
...
BaseGrid seachGrid = new PartialGridWPool(nodePool, new GridRect(1,3,15,30);  
```


Rest of the functionality like `SetWalkableAt`, `Reset`, etc. are same as `DynamicGridWPool`. 


#### UseRecursive ####
You may use recursive function or loop function to find the path. This can be simply done by setting UseRecursive flag in JumpPointParam:
```
Note that the default is false, which uses loop function.
```

```c#
// To use recursive function
JumpPointParam jpParam = new JumpPointParam(...);
jpParam.UseRecursive = true;  
```


To change back to loop function


```c#
// To change back to loop function 
jpParam.UseRecursive = false; 
```

#### Extendability ####
You can also create a sub-class of `BaseGrid` to create your own way of `Grid` class to best support your situation.

Donation
-------
[![Gratipay][gratipay-image]][gratipay-url]

or donation by Pledgie  
<a href='https://pledgie.com/campaigns/27762'><img alt='Click here to lend your support to: Open Source Visual C++/C# Projects and make a donation at pledgie.com !' src='https://pledgie.com/campaigns/27762.png?skin_name=chrome' border='0' ></a>

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

[gratipay-image]: https://img.shields.io/gratipay/juhgiyo.svg?style=flat
[gratipay-url]: https://gratipay.com/juhgiyo/


[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/juhgiyo/eppathfinding.cs/trend.png)](https://bitdeli.com/free "Bitdeli Badge")

