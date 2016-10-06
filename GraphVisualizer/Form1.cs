using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphVisualizer
{
    public partial class Form1 : Form
    {
        int scaleFactor = 1;
        List<Edge> edges = new List<Edge>();
        Dictionary<int, Node> nodes = new Dictionary<int, Node>();
        int panelX = 0;
        int panelY = 0;
        int draggedNodeKey = -1;
        int dragStartX = 0;
        int dragStartY = 0;

        struct Node
        {
            public int x;
            public int y;
        }

        struct Edge
        {
            public int nodeS;
            public int nodeE;
            public int startX;
            public int startY;
            public int endX;
            public int endY;
            public int Weight;
            public int Order;
            public bool Show;
            public Color color;
            public Color weightColor;
        }

        public Form1()
        {
            InitializeComponent();
            
            panel1.AutoScroll = true;
            panel1.HorizontalScroll.Enabled = true;            
            panel1.AutoSize = true;
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseUp += panel1_MouseUp;
            panel1.MouseDoubleClick += panel1_MouseDoubleClick;

        }

        int GetNodeIdFromPoint(int x,int y)
        {
            Rectangle r = new Rectangle(x, y, 5, 5);
            int nodeId = -1;

            foreach (var node in nodes)
            {
                Rectangle nodeR = new Rectangle(node.Value.x, node.Value.y, 20, 20);
                if (nodeR.IntersectsWith(r))
                {
                    nodeId = node.Key;
                }
            }
            return nodeId;
        }

        void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int nodeId = GetNodeIdFromPoint(e.X, e.Y);
            if (nodeId != -1)
            {
                for (int i = 0; i < edges.Count(); i++)
                { 
                    var el = edges[i];
                    if (el.nodeS == nodeId)
                    {
                        edges[i] = new Edge() { nodeS = el.nodeS, nodeE = el.nodeE, startX = e.X, startY = e.Y, endX = el.endX, endY = el.endY, Weight = el.Weight, Order = el.Order, Show = !el.Show,color=Color.LightBlue,weightColor=Color.DarkBlue };
                    }
                    else if (el.nodeE == nodeId)
                    {
                        edges[i] = new Edge() { nodeS = el.nodeS, nodeE = el.nodeE, startX = el.startX, startY = el.startY, endX = e.X, endY = e.Y, Weight = el.Weight, Order = el.Order, Show = !el.Show,color=Color.LightBlue,weightColor=Color.DarkBlue };
                    }
                }
            }
            panel1.Invalidate();
        }


        void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (draggedNodeKey != -1)
            {
                nodes[draggedNodeKey] = new Node() { x = e.X, y = e.Y };

                for (int i = 0; i < edges.Count(); i++)
                {
                    var el = edges[i];
                    if (el.nodeS == draggedNodeKey)
                    {
                        edges[i] = new Edge() { nodeS = el.nodeS, nodeE = el.nodeE, startX = e.X, startY = e.Y, endX = el.endX, endY = el.endY, Weight = el.Weight, Order = el.Order, Show = el.Show, color = Color.LightBlue, weightColor = Color.DarkBlue };
                    }
                    else if (el.nodeE == draggedNodeKey)
                    {
                        edges[i] = new Edge() { nodeS = el.nodeS, nodeE = el.nodeE, startX = el.startX, startY = el.startY, endX = e.X, endY = e.Y, Weight = el.Weight, Order = el.Order, Show = el.Show, color = Color.LightBlue, weightColor = Color.DarkBlue };
                    }
                }

                draggedNodeKey = -1;
                //Rectangle refreshArea = new Rectangle(dragStartX, dragStartY, e.X - dragStartX, e.Y - dragStartY);
                panel1.Invalidate();
            }
        }

        void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Rectangle r = new Rectangle(e.X, e.Y, 5, 5);
            dragStartX = e.X;
            dragStartY = e.Y;
            foreach (var node in nodes)
            {
                Rectangle nodeR = new Rectangle(node.Value.x, node.Value.y, 20, 20);
                if (nodeR.IntersectsWith(r))
                {
                    draggedNodeKey = node.Key;
                }
            }
        }

        protected void DrawNodeWithText(PaintEventArgs e, string number,int x,int y)
        {
            int size = 20;
            e.Graphics.DrawEllipse(new Pen(Color.Black), new Rectangle(new Point(x,y), new Size(size * scaleFactor, size * scaleFactor)));
            e.Graphics.DrawString(number, new Font(new FontFamily("Times New Roman"), (size * scaleFactor) - (10*scaleFactor)), new SolidBrush(Color.Red), new Point(x, y));
        }

        protected void DrawEdge(PaintEventArgs e, int node1X, int node1Y, int node2X, int node2Y, int weight, int order, Color edgeColor, Color weightColor)
        {
            e.Graphics.DrawLine(new Pen(edgeColor), new Point(node1X, node1Y), new Point(node2X, node2Y));
            e.Graphics.DrawString("(" + weight.ToString() + ")", new Font(new FontFamily("Times New Roman"), 10), new SolidBrush(weightColor), new Point(((node1X + node2X) / 2) + (30 * (order - 1)), (node1Y + node2Y) / 2));
        }


        public void PopulateNodes(int numberOfNodes)
        {
            nodes.Clear();

            int spaceInBetween = 300;
            int n = numberOfNodes / 4;
            int currentX = 500;
            int currentY = 500;
            
            for (int i = 1; i <= numberOfNodes; i++)
            {
                if (i <= n)
                {
                    nodes.Add(i, new Node() { x = currentX, y = currentY });
                    currentX += spaceInBetween;
                    if (i == n)
                    {
                        panelX = currentX + spaceInBetween;
                        currentY += spaceInBetween;
                    }
                }
                else if (i <= 2 * n)
                {
                    nodes.Add(i, new Node() { x = currentX, y = currentY });
                    currentY += spaceInBetween;
                    if (i == (2 * n))
                    {
                        panelY = currentY;
                        currentX -= spaceInBetween;
                    }
                }
                else if (i <= (3 * n))
                {
                    nodes.Add(i, new Node() { x = currentX, y = currentY });
                    currentX -= spaceInBetween;
                    if (i == 3 * n)
                    {
                        currentY -= spaceInBetween;
                        //currentX -= spaceInBetween;
                    }
                }
                else
                {
                    nodes.Add(i, new Node() { x = currentX, y = currentY });                    
                    currentY -= spaceInBetween;
                }
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            edges.Clear();
            string graphMetaData = graphInput.Text;
            string[] lines = graphMetaData.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            panelX = 0;
            panelY = 0;

            int count = 0;
            bool executed = false;
            foreach (string line in lines)
            {
                if (!executed)
                {
                    PopulateNodes(int.Parse(line));
                    executed = true;
                    continue;
                }

                count++;

                string eMetaData = Console.ReadLine();
                int v1 = int.Parse(line.Split(new char[] { ' ' })[0]);
                int v2 = int.Parse(line.Split(new char[] { ' ' })[1]);
                int w = int.Parse(line.Split(new char[] { ' ' })[2]);

                int sX = nodes[v1].x;
                int sY = nodes[v1].y;
                int eX = nodes[v2].x;
                int eY = nodes[v2].y;

                int edgeOrder = edges.Count(ed => (ed.nodeS == v1 && ed.nodeE == v2) || (ed.nodeS == v2 && ed.nodeE == v1));
                edges.Add(new Edge() { nodeS=v1,nodeE=v2, Weight=w,startX=sX,startY=sY,endX=eX,endY=eY,Order = edgeOrder + 1,Show=allEdgesCheckBox.Checked,color=Color.LightBlue,weightColor=Color.DarkBlue });
                edges.Add(new Edge() { nodeS = v2, nodeE = v1, Weight = w, startX = eX, startY = eY, endX = sX, endY = sY, Order = edgeOrder + 2, Show = allEdgesCheckBox.Checked, color = Color.LightBlue, weightColor = Color.DarkBlue });

            }

            panel1.Invalidate();

            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Cornsilk);
            foreach (var n in nodes)
            {
                DrawNodeWithText(e, n.Key.ToString(), n.Value.x, n.Value.y);
                panel1.MinimumSize = new Size(n.Value.x + 100, n.Value.y + 100);
            }

            foreach (var edge in edges)
            {
                if (edge.Show)
                {
                    DrawEdge(e, edge.startX, edge.startY, edge.endX, edge.endY, edge.Weight, edge.Order,edge.color,edge.weightColor);
                }
            }

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(panel1.Size.Width, panel1.Size.Height);
            Rectangle bounds = new Rectangle(new Point(0, 0), panel1.Size);
            panel1.DrawToBitmap(bitmap, bounds);
            bitmap.Save(@"C:\temp\graph.jpg");
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            scaleFactor += 1;
            //panel1.Scale(scaleFactor);
            panel1.Invalidate();
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            scaleFactor -= 1;
            //panel1.Scale(scaleFactor);  
            panel1.Invalidate();
        }

        void PrintDistances(List<Edge> al, int s, int vCount,int stopNode)
        {
            Dictionary<int, int> shortestPath = new Dictionary<int, int>();
            int[] nodeDistances = new int[vCount + 1];
            HeapNode[] distances = new HeapNode[vCount];
            for (int i = 1; i <= vCount; i++)
            {
                nodeDistances[i] = int.MaxValue;
                distances[i - 1] = new HeapNode() { Index = i, Distance = int.MaxValue };
            }
            nodeDistances[s] = 0;
            distances[s - 1] = new HeapNode() { Index = s, Distance = 0 };

            Heap h = new Heap(distances);
            
            while (h.size > 0)
            {
                int node = h.Delete();
                var adjacentNodes = al.Where(e => e.nodeS == node).ToList();
                foreach (var v in adjacentNodes)
                {
                    if (h.IsInHeap(v.nodeE) && nodeDistances[node] != int.MaxValue)
                    {
                        int weight = nodeDistances[node] + v.Weight;
                        if (nodeDistances[v.nodeE] > weight)
                        {
                            nodeDistances[v.nodeE] = weight;
                            h.DecreaseKey(v.nodeE, weight);
                            if (shortestPath.ContainsKey(v.nodeE))
                            {
                                shortestPath[v.nodeE] = node;
                            }
                            else
                            {
                                shortestPath.Add(v.nodeE, node);
                            }
                        }
                    }
                }

                if (node == stopNode)
                {
                    int previous = -1;
                    int x = stopNode;
                    while (shortestPath[x] != s)
                    {
                        previous = shortestPath[x];
                        UpdateEdge(previous, x);
                        x = previous;
                    }
                    UpdateEdge(previous == -1 ? stopNode : previous, s);
                    return;
                }

            }

        }

        void UpdateEdge(int start,int end)
        {
            for (int i = 0; i < edges.Count(); i++)
            {
                var el = edges[i];
                if (edges[i].nodeS == start && edges[i].nodeE == end)
                {
                    edges[i] = new Edge() { nodeS = el.nodeS, nodeE = el.nodeE, startX = el.startX, startY = el.startY, endX = el.endX, endY = el.endY, Weight = el.Weight, Order = el.Order, Show = true, color = Color.LimeGreen, weightColor = Color.DarkOliveGreen };
                }
            }
        }

        private void shortestPathButton_Click(object sender, EventArgs e)
        {
            if (edges != null && edges.Count > 0)
            {
                int start = int.Parse(fromTextBox.Text);
                int end = int.Parse(toTextBox.Text);
                PrintDistances(edges, start, nodes.Count, end);
                panel1.Invalidate();
            }
        }

    }



    struct HeapNode
    {
        public int Index { get; set; } // vertex number
        public int Distance { get; set; }
    }

    class Heap
    {
        HeapNode[] numbers;
        public int size { get; set; }
        int[] positions;

        public Heap(HeapNode[] numbers)
        {
            this.numbers = numbers;
            size = numbers.Length;
            positions = new int[numbers.Length + 1];
            Initialize();
        }

        public void Initialize()
        {
            for (int i = 1; i < size; i++)
            {
                int newNodeIndex = i;
                int parentIndex = GetParentIndex(newNodeIndex);
                HeapNode node = numbers[newNodeIndex];
                positions[node.Index] = newNodeIndex;
                while (parentIndex >= 0 && numbers[newNodeIndex].Distance < numbers[parentIndex].Distance)
                {
                    Swap(newNodeIndex, parentIndex);
                    newNodeIndex = parentIndex;
                    parentIndex = GetParentIndex(newNodeIndex);
                }
            }
        }

        public bool IsInHeap(int v)
        {
            return positions[v] < this.size + 1 ? true : false;
        }

        public void DecreaseKey(int v, int dist)
        {
            numbers[positions[v]] = new HeapNode() { Index = v, Distance = dist };
            int decreasedNodeIndex = positions[v];
            int parentIndex = GetParentIndex(decreasedNodeIndex);
            while (parentIndex >= 0 && numbers[decreasedNodeIndex].Distance < numbers[parentIndex].Distance)
            {
                Swap(decreasedNodeIndex, parentIndex);
                decreasedNodeIndex = parentIndex;
                parentIndex = GetParentIndex(decreasedNodeIndex);
            }
        }

        public int Delete()
        {
            int root = numbers[0].Index;
            int indexToSwitch = 0;
            int leftIndex = GetLeftChildIndex(indexToSwitch);
            int rightIndex = GetRightChildIndex(indexToSwitch);
            int newSize = size - 1;
            positions[numbers[0].Index] = newSize;
            numbers[0] = numbers[newSize];
            positions[numbers[0].Index] = 0;
            while (leftIndex < newSize && (numbers[indexToSwitch].Distance > numbers[leftIndex].Distance || numbers[indexToSwitch].Distance > numbers[rightIndex].Distance))
            {
                if (numbers[leftIndex].Distance < numbers[rightIndex].Distance)
                {
                    Swap(indexToSwitch, leftIndex);
                    indexToSwitch = leftIndex;
                }
                else
                {
                    Swap(indexToSwitch, rightIndex);
                    indexToSwitch = rightIndex;
                }

                leftIndex = GetLeftChildIndex(indexToSwitch);
                rightIndex = GetRightChildIndex(indexToSwitch);
            }

            size = newSize;
            newSize--;
            return root;
        }


        private int GetLeftChildIndex(int parentIndex)
        {
            return (parentIndex << 1) + 1;
        }

        private int GetRightChildIndex(int parentIndex)
        {
            return (parentIndex << 1) + 2;
        }

        private int GetParentIndex(int childIndex)
        {
            return (childIndex - 1) >> 1;
        }

        private void Swap(int index1, int index2)
        {
            positions[numbers[index1].Index] = index2;
            positions[numbers[index2].Index] = index1;
            HeapNode temp = numbers[index1];
            numbers[index1] = numbers[index2];
            numbers[index2] = temp;
        }

    }


}
