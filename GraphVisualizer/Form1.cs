using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphVisualizer
{
    public partial class Form1 : Form
    {
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
        }

        public Form1()
        {
            InitializeComponent();

            InitializeGraph();
            panel1.AutoScroll = true;
            panel1.HorizontalScroll.Enabled = true;
            //panel1.AutoScrollMinSize = new System.Drawing.Size(2000, 2000);
            panel1.AutoSize = true;
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseUp += panel1_MouseUp;
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
                        edges[i] = new Edge() { nodeS = el.nodeS, nodeE = el.nodeE, startX = e.X, startY = e.Y, endX = el.endX, endY = el.endY, Weight = el.Weight,Order=el.Order };
                    }
                    else if (el.nodeE == draggedNodeKey)
                    {
                        edges[i] = new Edge() { nodeS = el.nodeS, nodeE = el.nodeE, startX = el.startX, startY = el.startY, endX = e.X, endY = e.Y, Weight = el.Weight,Order=el.Order };
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

        private void InitializeGraph()
        {
            //edges.Add(1, new Edge() { To=2,Weight=10});

        }


        protected override void OnPaint(PaintEventArgs e)
        {            
            //DrawNodeWithText(e, "1", 0,0);
            //DrawNodeWithText(e, "2", 80,0);
            //DrawNodeWithText(e, "3", 100,0);
            //DrawEdge(e, 30,15, 80,15,10);
        }

        protected void DrawNodeWithText(PaintEventArgs e, string number,int x,int y)
        {
            int size = 20;
            e.Graphics.DrawEllipse(new Pen(Color.Black), new Rectangle(new Point(x,y), new Size(size, size)));
            e.Graphics.DrawString(number, new Font(new FontFamily("Times New Roman"), size - 10), new SolidBrush(Color.Red), new Point(x,y));
        }

        protected void DrawEdge(PaintEventArgs e,int node1X,int node1Y,int node2X,int node2Y,int weight,int order)
        {
            e.Graphics.DrawLine(new Pen(Color.RoyalBlue), new Point(node1X, node1Y), new Point(node2X, node2Y));            
            e.Graphics.DrawString( "(" + weight.ToString() + ")", new Font(new FontFamily("Times New Roman"), 10), new SolidBrush(Color.DarkBlue), new Point(((node1X + node2X) / 2) + (30 * (order-1)), (node1Y + node2Y) / 2));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            edges.Clear();
            nodes.Clear();
            string graphMetaData = graphInput.Text;
            string[] lines = graphMetaData.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            int currentX = 330;
            int currentY = 400;
            int increment = 100;
            panelX = 0;
            panelY = 0;

            int count = 0;
            foreach (string line in lines)
            {
                count++;

                if (count % 2 == 0)
                {
                    currentY += 200;
                    currentX = 330;
                }

                string eMetaData = Console.ReadLine();
                int v1 = int.Parse(line.Split(new char[] { ' ' })[0]);
                int v2 = int.Parse(line.Split(new char[] { ' ' })[1]);
                int w = int.Parse(line.Split(new char[] { ' ' })[2]);

                int sX = 0;
                int sY = 0;
                int eX = 0;
                int eY = 0;

                if (!nodes.ContainsKey(v1))
                {
                    currentX += increment;
                    //currentY = count % 2 == 0 ? currentY + 200 : currentY - 200;
                    sX = currentX + 15;
                    sY = currentY + 15;
                    nodes.Add(v1, new Node() { x = currentX, y = currentY });
                }
                else
                {
                    sX = nodes[v1].x + 15;
                    sY = nodes[v1].y + 15;
                }

                if (!nodes.ContainsKey(v2))
                {
                    currentX += increment;
                    //currentY = count % 2 == 0 ? currentY + 200 : currentY - 200;
                    eX = currentX + 15;
                    eY = currentY + 15;
                    nodes.Add(v2, new Node() { x = currentX, y = currentY });
                }
                else
                {
                    eX = nodes[v2].x + 15;
                    eY = nodes[v2].y + 15;
                }

                int edgeOrder = edges.Count(ed => (ed.nodeS == v1 && ed.nodeE == v2) || (ed.nodeS == v2 && ed.nodeE == v1));
                edges.Add(new Edge() { nodeS=v1,nodeE=v2, Weight=w,startX=sX,startY=sY,endX=eX,endY=eY,Order = edgeOrder + 1 });


                
                if (panelX < currentX)
                {
                    panelX = currentX;
                }

                if (panelY < currentY)
                {
                    panelY = currentY;
                }

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
                DrawEdge(e, edge.startX, edge.startY, edge.endX, edge.endY, edge.Weight,edge.Order);
            }

        }



    }
}
