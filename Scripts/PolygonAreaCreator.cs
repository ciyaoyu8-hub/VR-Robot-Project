using UnityEngine;
using System.Collections.Generic;

public class PolygonAreaCreator : MonoBehaviour
{
    #region 单例
    private static PolygonAreaCreator instance;

    public static PolygonAreaCreator Instance
    {
        get { return instance; }
        private set { instance = value; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning(gameObject.name + "单例唯一性检查异常");
            Destroy(gameObject);
            return;
        }
        instance = this;
        Init();
    }
    #endregion

    // 多个多边形面，每个面由一组顶点组成
    List<List<Vector3>> polygonFaces = new List<List<Vector3>>();
    private MeshFilter meshFilter;
    private MeshCollider meshCollider; // 新增MeshCollider引用
    public Material material;
    Vector3 desiredNormal = new Vector3(0, 1, 0);

    void Init()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        // 初始化MeshCollider
        meshCollider = gameObject.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }

        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            renderer = gameObject.AddComponent<MeshRenderer>();
        }

        // 确保材质存在
        if (material == null)
        {
            material = new Material(Shader.Find("Standard"));
        }
        renderer.material = material;
    }

    void CreatePolygonMeshes()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Multi-Polygon Mesh";

        List<Vector3> allVertices = new List<Vector3>();
        List<int> allTriangles = new List<int>();
        int vertexOffset = 0;

        foreach (var face in polygonFaces)
        {
            // 处理顶点顺序，确保法线方向正确
            List<Vector3> processedFace = EnsureCorrectNormalDirection(face);

            // 添加处理后的顶点
            foreach (var point in processedFace)
            {
                allVertices.Add(point);
            }

            // 创建三角形索引
            int faceVertexCount = processedFace.Count;
            for (int i = 0; i < faceVertexCount - 2; i++)
            {
                allTriangles.Add(vertexOffset);
                allTriangles.Add(vertexOffset + i + 1);
                allTriangles.Add(vertexOffset + i + 2);
            }

            vertexOffset += faceVertexCount;
        }

        mesh.vertices = allVertices.ToArray();
        mesh.triangles = allTriangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        if (meshFilter.sharedMesh != null)
        {
            Destroy(meshFilter.sharedMesh);
        }
        meshFilter.sharedMesh = mesh;

        // 更新碰撞器网格
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = mesh;
        }
    }

    // 确保多边形面的法线方向符合预期
    private List<Vector3> EnsureCorrectNormalDirection(List<Vector3> face)
    {
        // 计算当前面的法线方向
        Vector3 normal = CalculatePolygonNormal(face);

        // 检查法线是否与期望方向大致相同
        // 使用点积判断方向是否一致（大于0表示方向大致相同）
        if (Vector3.Dot(normal, desiredNormal) > 0)
        {
            return face; // 方向正确，返回原顶点列表
        }
        else
        {
            // 方向相反，反转顶点顺序
            List<Vector3> reversed = new List<Vector3>(face);
            reversed.Reverse();
            return reversed;
        }
    }

    // 计算多边形面的法线方向
    private Vector3 CalculatePolygonNormal(List<Vector3> face)
    {
        // 对于平面多边形，可以通过前三个点计算法线
        if (face.Count < 3)
            return Vector3.up;

        Vector3 v0 = face[0];
        Vector3 v1 = face[1];
        Vector3 v2 = face[2];

        // 计算两条边的向量
        Vector3 edge1 = v1 - v0;
        Vector3 edge2 = v2 - v0;

        // 叉积得到法线（垂直于平面）
        Vector3 normal = Vector3.Cross(edge1, edge2).normalized;

        return normal;
    }

    // 添加新的多边形面
    public void AddPolygonFace(List<Vector3> newFace)
    {
        if (newFace == null || newFace.Count < 3)
        {
            Debug.LogError("添加的多边形面需要至少3个点！");
            return;
        }

        polygonFaces.Add(newFace);
        UpdatePolygons();
    }

    // 提供一个方法用于在运行时更新多边形
    void UpdatePolygons()
    {
        CreatePolygonMeshes();
    }
}