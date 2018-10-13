using OpenTK;

public abstract class Shape
{
    public Vector3[] Vertices { get; protected set; }

    public Vector3[] Normals { get; protected set; }

    public Vector2[] Texcoords { get; protected set; }

    public int[] Indices { get; protected set; }

    public int[] Colors { get; protected set; }
}