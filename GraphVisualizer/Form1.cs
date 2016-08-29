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
                        edges[i] = new Edge() { nodeS = el.nodeS, nodeE = el.nodeE, startX = e.X, startY = e.Y, endX = el.endX, endY = el.endY, Weight = el.Weight, Order = el.Order, Show = !el.Show };
                    }
                    else if (el.nodeE == nodeId)
                    {
                        edges[i] = new Edge() { nodeS = el.nodeS, nodeE = el.nodeE, startX = el.startX, startY = el.startY, endX = e.X, endY = e.Y, Weight = el.Weight, Order = el.Order, Show = !el.Show };
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
                        edges[i] = new Edge() { nodeS = el.nodeS, nodeE = el.nodeE, startX = e.X, startY = e.Y, endX = el.endX, endY = el.endY, Weight = el.Weight,Order=el.Order,Show=el.Show };
                    }
                    else if (el.nodeE == draggedNodeKey)
                    {
                        edges[i] = new Edge() { nodeS = el.nodeS, nodeE = el.nodeE, startX = el.startX, startY = el.startY, endX = e.X, endY = e.Y, Weight = el.Weight, Order = el.Order, Show = el.Show };
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

        protected void DrawEdge(PaintEventArgs e,int node1X,int node1Y,int node2X,int node2Y,int weight,int order)
        {
            e.Graphics.DrawLine(new Pen(Color.RoyalBlue), new Point(node1X, node1Y), new Point(node2X, node2Y));            
            e.Graphics.DrawString( "(" + weight.ToString() + ")", new Font(new FontFamily("Times New Roman"), 10), new SolidBrush(Color.DarkBlue), new Point(((node1X + node2X) / 2) + (30 * (order-1)), (node1Y + node2Y) / 2));
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
                edges.Add(new Edge() { nodeS=v1,nodeE=v2, Weight=w,startX=sX,startY=sY,endX=eX,endY=eY,Order = edgeOrder + 1,Show=false });
                edges.Add(new Edge() { nodeS = v2, nodeE = v1, Weight = w, startX = eX, startY = eY, endX = sX, endY = sY, Order = edgeOrder + 2, Show = false });

            }

            PrintDistances(edges, 1, 28);
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
                    DrawEdge(e, edge.startX, edge.startY, edge.endX, edge.endY, edge.Weight, edge.Order);
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

        void PrintDistances(List<Edge> al, int s, int vCount)
        {
            Dictionary<int, int> nodeDistances = new Dictionary<int, int>(vCount);
            for (int i = 1; i <= vCount; i++)
            {
                nodeDistances.Add(i, -1);
            }
            nodeDistances[s] = 0;

            Queue<int> currentLQueue = new Queue<int>(vCount);
            currentLQueue.Enqueue(s);

            Dictionary<int, int> alreadyVisited = new Dictionary<int, int>(vCount) { };
            while (currentLQueue.Count > 0)
            {
                int node = currentLQueue.Dequeue();                
                var neigbours = al.Where(e => e.nodeS == node).ToArray().ToList();
                foreach (var v in neigbours)
                {
                    // if node is not visisted yet
                    if (!alreadyVisited.ContainsKey(v.nodeE))
                    {
                        currentLQueue.Enqueue(v.nodeE); // Add to queue for it to be visited
                    }
                    // if distance is never set for this node, then set it to edge weight.
                    if (nodeDistances[v.nodeE] == -1)
                    {
                        nodeDistances[v.nodeE] = nodeDistances[node] + v.Weight;
                        UpdateEdge(node, v.nodeE);
                    }
                    // else if weight is set before, now update if new distance is shorter.
                    if (nodeDistances[v.nodeE] > nodeDistances[node] + v.Weight)
                    {
                        nodeDistances[v.nodeE] = nodeDistances[node] + v.Weight;
                        UpdateEdge(node, v.nodeE);
                    }
                }

                if (!alreadyVisited.ContainsKey(node))
                {
                    alreadyVisited.Add(node, 0); // Set Node as Visited since all edges processed
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
                    edges[i] = new Edge() { nodeS = el.nodeS, nodeE = el.nodeE, startX = el.startX, startY = el.startY, endX = el.endX, endY = el.endY, Weight = el.Weight, Order = el.Order, Show = true };
                }
                else if (edges[i].nodeS == start && edges[i].nodeE != end)
                {
                    edges[i] = new Edge() { nodeS = el.nodeS, nodeE = el.nodeE, startX = el.startX, startY = el.startY, endX = el.endX, endY = el.endY, Weight = el.Weight, Order = el.Order, Show = false };
                }
            }
        }


    }
}
