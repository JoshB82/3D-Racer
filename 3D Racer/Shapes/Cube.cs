namespace _3D_Racer
{
    public sealed class Cube : Shape
    {
        public Cube(float x, float y, float z, float side_length,
            string vertex_colour = "000000",
            string edge_colour = "000000",
            string face_colour = "000000")
        {
            Visible = true;
            Selected = false;

            Model_Origin = new Vector(0, 0, 0, 1);
            
            Model_Vertices = new Vertex[8]
            {
                new Vertex(0, 0, 0, vertex_colour), // 0
                new Vertex(1, 0, 0, vertex_colour), // 1
                new Vertex(1, 1, 0, vertex_colour), // 2
                new Vertex(0, 1, 0, vertex_colour), // 3
                new Vertex(0, 0, 1, vertex_colour), // 4
                new Vertex(1, 0, 1, vertex_colour), // 5
                new Vertex(1, 1, 1, vertex_colour), // 6
                new Vertex(0, 1, 1, vertex_colour) // 7
            };

            Edges = new Edge[18]
            {
                new Edge(0, 1, edge_colour), // 0
                new Edge(1, 2, edge_colour), // 1
                new Edge(0, 2, edge_colour, false), // 2
                new Edge(2, 3, edge_colour), // 3
                new Edge(0, 3, edge_colour), // 4
                new Edge(1, 5, edge_colour), // 5
                new Edge(5, 6, edge_colour), // 6
                new Edge(1, 6, edge_colour, false), // 7
                new Edge(2, 6, edge_colour), // 8
                new Edge(4, 5, edge_colour), // 9
                new Edge(4, 7, edge_colour), // 10
                new Edge(5, 7, edge_colour, false), // 11
                new Edge(6, 7, edge_colour), // 12
                new Edge(0, 4, edge_colour), // 13
                new Edge(3, 4, edge_colour, false),  // 14
                new Edge(3, 7, edge_colour), // 15
                new Edge(3, 6, edge_colour, false), // 16
                new Edge(1, 4, edge_colour, false) // 17
            };

            Faces = new Face[12]
            {
                new Face(0, 1, 2, face_colour), // 0
                new Face(2, 3, 4, face_colour), // 1
                new Face(5, 6, 7, face_colour), // 2
                new Face(7, 8, 1, face_colour), // 3
                new Face(9, 10, 11, face_colour), // 4
                new Face(11, 12, 6, face_colour), // 5
                new Face(13, 4, 14, face_colour), // 6
                new Face(14, 15, 10, face_colour), // 7
                new Face(3, 8, 16, face_colour), // 8
                new Face(16, 12, 15, face_colour), // 9
                new Face(9, 5, 17, face_colour), // 10
                new Face(17, 0, 13, face_colour) // 11
            };

            Matrix scale = Transform.Scale(side_length, side_length, side_length);
            Matrix translation = Transform.Translate(x, y, z);
            Model_to_world = translation * scale;
            Apply_World_Matrices();
        }
    }
}