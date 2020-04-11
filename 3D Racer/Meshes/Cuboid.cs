﻿using System.Diagnostics;
using System.Drawing;

namespace _3D_Racer
{
    /// <summary>
    /// Handles creation of a cuboid mesh.
    /// </summary>
    public sealed class Cuboid : Mesh
    {
        public Cuboid(Vector3D position, double length, double width, double height, bool visible = true, Bitmap_Texture texture = null,
            Color? vertex_colour = null,
            Color? edge_colour = null,
            Color? face_colour = null)
        {
            Vertex_Colour = vertex_colour ?? Color.Blue;
            Edge_Colour = edge_colour ?? Color.Black;
            Face_Colour = face_colour ?? Color.FromArgb(0xFF, 0x00, 0xFF, 0x00); // Green

            Visible = visible;

            World_Origin = new Vertex(position.X, position.Y, position.Z, 1);

            Set_Shape_Direction_1(Vector3D.Unit_X, Vector3D.Unit_Y);

            Model_Vertices = new Vertex[8]
            {
                new Vertex(0, 0, 0, Vertex_Colour), // 0
                new Vertex(1, 0, 0, Vertex_Colour), // 1
                new Vertex(1, 1, 0, Vertex_Colour), // 2
                new Vertex(0, 1, 0, Vertex_Colour), // 3
                new Vertex(0, 0, 1, Vertex_Colour), // 4
                new Vertex(1, 0, 1, Vertex_Colour), // 5
                new Vertex(1, 1, 1, Vertex_Colour), // 6
                new Vertex(0, 1, 1, Vertex_Colour) // 7
            };

            Edges = new Edge[18]
            {
                new Edge(0, 1, Edge_Colour, false), // 0
                new Edge(1, 2, Edge_Colour, false), // 1
                new Edge(0, 2, Edge_Colour, false), // 2
                new Edge(2, 3, Edge_Colour, false), // 3
                new Edge(0, 3, Edge_Colour, false), // 4
                new Edge(1, 5, Edge_Colour, false), // 5
                new Edge(5, 6, Edge_Colour, false), // 6
                new Edge(1, 6, Edge_Colour, false), // 7
                new Edge(2, 6, Edge_Colour, false), // 8
                new Edge(4, 5, Edge_Colour, false), // 9
                new Edge(4, 7, Edge_Colour, false), // 10
                new Edge(5, 7, Edge_Colour, false), // 11
                new Edge(6, 7, Edge_Colour, false), // 12
                new Edge(0, 4, Edge_Colour, false), // 13
                new Edge(3, 4, Edge_Colour, false),  // 14
                new Edge(3, 7, Edge_Colour, false), // 15
                new Edge(3, 6, Edge_Colour, false), // 16
                new Edge(1, 4, Edge_Colour, false) // 17
            };

            Faces = new Face[12]
            {
                new Face(0, 1, 2, Face_Colour, texture.File_Path), // 0
                new Face(0, 2, 3, Face_Colour, texture.File_Path), // 1
                new Face(1, 6, 2, Face_Colour, texture.File_Path), // 2
                new Face(1, 5, 6, Face_Colour, texture.File_Path), // 3
                new Face(4, 7, 5, Face_Colour, texture.File_Path), // 4
                new Face(5, 7, 6, Face_Colour, texture.File_Path), // 5
                new Face(0, 3, 4, Face_Colour, texture.File_Path), // 6
                new Face(4, 3, 7, Face_Colour, texture.File_Path), // 7
                new Face(7, 3, 6, Face_Colour, texture.File_Path), // 8
                new Face(6, 3, 2, Face_Colour, texture.File_Path), // 9
                new Face(4, 5, 0, Face_Colour, texture.File_Path), // 10
                new Face(5, 1, 0, Face_Colour, texture.File_Path) // 11
            };

            Textures = new Bitmap_Texture[1]
            {
                new Bitmap_Texture(texture.File_Path) // 0
            };

            Texture_Vertices = new Texture_Vertex[4]
            {
                new Texture_Vertex(0, 0), // 0
                new Texture_Vertex(1, 0), // 1
                new Texture_Vertex(0, 1), // 2
                new Texture_Vertex(1, 1) // 3
            };

            Texture_Faces = new Texture_Face[12]
            {
                new Texture_Face(3, 2, 0, 0), // 0
                new Texture_Face(3, 0, 1, 0), // 1
                new Texture_Face(3, 0, 1, 0), // 2
                new Texture_Face(3, 2, 0, 0), // 3
                new Texture_Face(2, 0, 3, 0), // 4
                new Texture_Face(3, 0, 1, 0), // 5
                new Texture_Face(2, 0, 3, 0), // 6
                new Texture_Face(3, 0, 1, 0), // 7
                new Texture_Face(2, 0, 3, 0), // 8
                new Texture_Face(3, 0, 1, 0), // 9
                new Texture_Face(0, 1, 2, 0), // 10
                new Texture_Face(1, 3, 2, 0) // 11
            };

            Scaling = new Vector3D(length, width, height);
            Translation = new Vector3D(position.X, position.Y, position.Z);

            Debug.WriteLine($"Cuboid created at ({position.X}, {position.Y}, {position.Z})");
        }
    }
}