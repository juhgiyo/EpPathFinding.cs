/*! 
@file SearchGridForm.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding>
@date July 16, 2013
@brief SearchGridForm Interface
@version 2.0

@section LICENSE

The MIT License (MIT)

Copyright (c) 2013 Woong Gyu La <juhgiyo@gmail.com>

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

@section DESCRIPTION

An Interface for the SearchGridForm Class.

*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using General;

namespace EpPathFinding
{
    public partial class SearchGridForm : Form
    {
        const int width = 64;
        const int height = 32;
        Graphics paper;

        GridBox[][] m_rectangles;
        List<ResultBox> m_resultBox;
        List<GridLine> m_resultLine;

        GridBox m_lastBoxSelect;
        BoxType m_lastBoxType;

        BaseGrid searchGrid;
        JumpPointParam jumpParam;
        public SearchGridForm()
        {



            InitializeComponent();
            this.DoubleBuffered = true;

            m_resultBox = new List<ResultBox>();
            this.Width = (width+1) * 20;
            this.Height = (height+1) * 20 +100;
            this.MaximumSize = new Size(this.Width, this.Height);
            this.MaximizeBox = false;


            m_rectangles = new GridBox[width][];
            for (int widthTrav = 0; widthTrav < width; widthTrav++)
            {
                m_rectangles[widthTrav] = new GridBox[height];
                for (int heightTrav = 0; heightTrav < height; heightTrav++)
                {
                    if(widthTrav==(width/3) && heightTrav==(height/2))
                        m_rectangles[widthTrav][heightTrav] = new GridBox(widthTrav * 20, heightTrav * 20 + 50, BoxType.Start);
                    else if (widthTrav == 41 && heightTrav == (height / 2))
                        m_rectangles[widthTrav][heightTrav] = new GridBox(widthTrav * 20 , heightTrav * 20 + 50, BoxType.End);
                    else
                        m_rectangles[widthTrav][heightTrav] = new GridBox(widthTrav * 20, heightTrav * 20 + 50, BoxType.Normal);


                }
            }

            m_resultLine = new List<GridLine>();

            //Grid searchGrid=new Grid(width,height,movableMatrix);
            //BaseGrid searchGrid = new StaticGrid(width, height, movableMatrix);
            //searchGrid = new DynamicGrid();
            searchGrid = new DynamicGridWPool(SingletonHolder<NodePool>.Instance);
            jumpParam = new JumpPointParam(searchGrid, true, cbCrossCorners.Checked, cbCrossAdjacentPoint.Checked, HeuristicMode.EUCLIDEAN);//new JumpPointParam(searchGrid, startPos, endPos, cbCrossCorners.Checked, HeuristicMode.EUCLIDEANSQR);
            jumpParam.UseRecursive = cbUseRecursive.Checked;
            
        }



        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
            paper = e.Graphics;
            //Draw
            
            for (int widthTrav = 0; widthTrav < width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < height; heightTrav++)
                {
                    m_rectangles[widthTrav][heightTrav].DrawBox(paper,BoxType.Normal);
                }
            }
            

            
            for (int resultTrav = 0; resultTrav < m_resultBox.Count; resultTrav++)
            {
                m_resultBox[resultTrav].drawBox(paper);
            }
            

            
            for (int widthTrav = 0; widthTrav < width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < height; heightTrav++)
                {
                    m_rectangles[widthTrav][heightTrav].DrawBox(paper, BoxType.Start);
                    m_rectangles[widthTrav][heightTrav].DrawBox(paper, BoxType.End);
                    m_rectangles[widthTrav][heightTrav].DrawBox(paper, BoxType.Wall);
                }
            }
             
            for (int resultTrav = 0; resultTrav < m_resultLine.Count; resultTrav++)
            {

                m_resultLine[resultTrav].drawLine(paper);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_lastBoxSelect = null;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (m_lastBoxSelect == null)
                {
                    for (int widthTrav = 0; widthTrav < width; widthTrav++)
                    {
                        for (int heightTrav = 0; heightTrav < height; heightTrav++)
                        {
                            if (m_rectangles[widthTrav][heightTrav].boxRec.IntersectsWith(new Rectangle(e.Location, new Size(1, 1))))
                            {
                                m_lastBoxType = m_rectangles[widthTrav][heightTrav].boxType;
                                m_lastBoxSelect = m_rectangles[widthTrav][heightTrav];
                                switch (m_lastBoxType)
                                {
                                    case BoxType.Normal:
                                    case BoxType.Wall:
                                        m_rectangles[widthTrav][heightTrav].SwitchBox();
                                        this.Invalidate();
                                        break;
                                    case BoxType.Start:
                                    case BoxType.End:

                                        break;
                                }
                            }


                        }
                    }
                    
                    return;
                }
                else
                {
                    for (int widthTrav = 0; widthTrav < width; widthTrav++)
                    {
                        for (int heightTrav = 0; heightTrav < height; heightTrav++)
                        {
                            if (m_rectangles[widthTrav][heightTrav].boxRec.IntersectsWith(new Rectangle(e.Location, new Size(1, 1))))
                            {
                                if (m_rectangles[widthTrav][heightTrav] == m_lastBoxSelect)
                                {
                                    return;
                                }
                                else
                                {

                                    switch (m_lastBoxType)
                                    {
                                        case BoxType.Normal:
                                        case BoxType.Wall:
                                            if (m_rectangles[widthTrav][heightTrav].boxType == m_lastBoxType)
                                            {
                                                m_rectangles[widthTrav][heightTrav].SwitchBox();
                                                m_lastBoxSelect = m_rectangles[widthTrav][heightTrav];
                                                this.Invalidate();
                                            }
                                            break;
                                        case BoxType.Start:
                                            m_lastBoxSelect.SetNormalBox();
                                            m_lastBoxSelect = m_rectangles[widthTrav][heightTrav];
                                            m_lastBoxSelect.SetStartBox();
                                            this.Invalidate();
                                            break;
                                        case BoxType.End:
                                            m_lastBoxSelect.SetNormalBox();
                                            m_lastBoxSelect = m_rectangles[widthTrav][heightTrav];
                                            m_lastBoxSelect.SetEndBox();
                                            this.Invalidate();
                                            break;
                                    }
                                    

                                }
                            }


                        }
                    }
                }
              
            }
            
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                for (int widthTrav = 0; widthTrav < width; widthTrav++)
                {
                    for (int heightTrav = 0; heightTrav < height; heightTrav++)
                    {
                        if (m_rectangles[widthTrav][heightTrav].boxRec.IntersectsWith(new Rectangle(e.Location, new Size(1, 1))))
                        {
                            m_lastBoxType=m_rectangles[widthTrav][heightTrav].boxType;
                            m_lastBoxSelect = m_rectangles[widthTrav][heightTrav];
                            switch(m_lastBoxType)
                            {
                                case BoxType.Normal:
                                case BoxType.Wall:
                                m_rectangles[widthTrav][heightTrav].SwitchBox();
                                this.Invalidate();
                                break;
                                case BoxType.Start:
                                case BoxType.End:
                                   
                                break;
                            }
                        }


                    }
                }
                
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            for (int resultTrav = 0; resultTrav < m_resultLine.Count; resultTrav++)
            {

                m_resultLine[resultTrav].Dispose();
            }
            m_resultLine.Clear();
            for (int resultTrav = 0; resultTrav < m_resultBox.Count; resultTrav++)
            {

                m_resultBox[resultTrav].Dispose();
            }
            m_resultBox.Clear();

            GridPos startPos = new GridPos();
            GridPos endPos = new GridPos();
             for (int widthTrav = 0; widthTrav < width; widthTrav++)
                {
                    for (int heightTrav = 0; heightTrav < height; heightTrav++)
                    {
                        if (m_rectangles[widthTrav][heightTrav].boxType != BoxType.Wall)
                        {
                            searchGrid.SetWalkableAt(new GridPos(widthTrav, heightTrav), true);
                        }
                        else
                        {
                            searchGrid.SetWalkableAt(new GridPos(widthTrav, heightTrav), false);
                        }
                        if(m_rectangles[widthTrav][heightTrav].boxType==BoxType.Start)
                        {
                            startPos.x=widthTrav;
                            startPos.y=heightTrav;
                        }
                        if(m_rectangles[widthTrav][heightTrav].boxType==BoxType.End)
                        {
                            endPos.x=widthTrav;
                            endPos.y=heightTrav;
                        }

                    }
                }
             jumpParam.CrossCorner = cbCrossCorners.Checked;
             jumpParam.CrossAdjacentPoint = cbCrossAdjacentPoint.Checked;
             jumpParam.UseRecursive = cbUseRecursive.Checked;
            jumpParam.Reset(startPos, endPos);
            List<GridPos> resultList = JumpPointFinder.FindPath(jumpParam);
            
            
            for (int resultTrav = 0; resultTrav < resultList.Count-1; resultTrav++)
            {
                m_resultLine.Add(new GridLine(m_rectangles[resultList[resultTrav].x][resultList[resultTrav].y],m_rectangles[resultList[resultTrav+1].x][resultList[resultTrav+1].y]));
            }
            for (int widthTrav = 0; widthTrav < jumpParam.SearchGrid.width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < jumpParam.SearchGrid.height; heightTrav++)
                {
                    if(jumpParam.SearchGrid.GetNodeAt(widthTrav, heightTrav)==null)
                        continue;
                    if (jumpParam.SearchGrid.GetNodeAt(widthTrav, heightTrav).isOpened)
                    {
                        ResultBox resultBox = new ResultBox(widthTrav * 20, heightTrav * 20 + 50, ResultBoxType.Opened);
                        m_resultBox.Add(resultBox);
                    }
                    if (jumpParam.SearchGrid.GetNodeAt(widthTrav, heightTrav).isClosed)
                    {
                        ResultBox resultBox = new ResultBox(widthTrav * 20, heightTrav * 20 + 50, ResultBoxType.Closed);
                        m_resultBox.Add(resultBox);
                    }
                }
            }
            this.Invalidate();
        }

        private void btnClearPath_Click(object sender, EventArgs e)
        {
            for (int resultTrav = 0; resultTrav < m_resultLine.Count; resultTrav++)
            {

                m_resultLine[resultTrav].Dispose();
            }
            m_resultLine.Clear();

            for (int resultTrav = 0; resultTrav < m_resultBox.Count; resultTrav++)
            {
                m_resultBox[resultTrav].Dispose();
            }

            m_resultBox.Clear();
            this.Invalidate();
        }

        private void btnClearWall_Click(object sender, EventArgs e)
        {
            for (int resultTrav = 0; resultTrav < m_resultLine.Count; resultTrav++)
            {

                m_resultLine[resultTrav].Dispose();
            }
            m_resultLine.Clear();

            for (int resultTrav = 0; resultTrav < m_resultBox.Count; resultTrav++)
            {
                m_resultBox[resultTrav].Dispose();
            }

            m_resultBox.Clear();
            for (int widthTrav = 0; widthTrav < width; widthTrav++)
            {
                for (int heightTrav = 0; heightTrav < height; heightTrav++)
                {
                   
                    switch (m_rectangles[widthTrav][heightTrav].boxType)
                    {
                        case BoxType.Normal:
                        case BoxType.Start:
                        case BoxType.End:
                            break;
                        case BoxType.Wall:
                            m_rectangles[widthTrav][heightTrav].SetNormalBox();
                            break;
                    }
                   


                }
            }
            this.Invalidate();
        }

        private void cbCrossCorners_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCrossCorners.Checked)
            {
                cbCrossAdjacentPoint.Enabled = true;
            }
            else
            {
                cbCrossAdjacentPoint.Enabled = false;
            }
        }
    }
}
