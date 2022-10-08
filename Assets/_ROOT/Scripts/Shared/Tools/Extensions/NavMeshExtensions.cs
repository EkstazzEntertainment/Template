namespace Ekstazz.Tools
{
    using UnityEngine;
    using UnityEngine.AI;

    
    public class NavMeshExtensions
    {
        private static NavMeshTriangulation nav;
        private static Mesh mesh;
        
        
        // Should be called after navmesh surface setup.
        // It requires a lot of resources that is why it is not called every time to find a point  
        public static void CalculateMesh()
        {
            nav = NavMesh.CalculateTriangulation();
            mesh = new Mesh {vertices = nav.vertices, triangles = nav.indices};
        }
        
        public static Vector3 RandomPointOnMap(Vector3 point, float radius)
        {
            for (var i = 0; i < 30; i++)
            {
                var res = GetRandomPointOnNavMesh();
                var distance = (res - point).sqrMagnitude;
                if (distance <= radius * radius) return res;
            }
            return PointOnMap(RandomPoint(point, radius));
        }
        
        public static Vector3 RandomPoint(Vector3 point, float radius)
        {
            var random = Random.insideUnitSphere * radius;
            var offset = new Vector3(random.x, 0, random.y);
            return point + offset;
        }
        
        public static Vector3 PointOnMap(Vector3 point, float maxDistance = 5)
        {
            var hasPath = NavMesh.SamplePosition(point, out var hit, maxDistance, 0);
            var res = hasPath ? hit.position : point;
            return res;
        }
        
        public static Vector3 GetRandomPointOnNavMesh()
        {
            var triangle = GetRandomTriangleOnNavMesh(); 
            var point = GetRandomPointOnTriangle(triangle);
            return point;
        }
        
        private static int GetRandomTriangleOnNavMesh()
        {
            var triangles = mesh.triangles.Length / 3;
            return Random.Range(0, triangles);
        }
        
        private static Vector3 GetRandomPointOnTriangle(int idx)
        {
            var v = new Vector3[3];

            v[0] = mesh.vertices[mesh.triangles[3 * idx + 0]];
            v[1] = mesh.vertices[mesh.triangles[3 * idx + 1]];
            v[2] = mesh.vertices[mesh.triangles[3 * idx + 2]];

            var a = v[1] - v[0];
            var b = v[2] - v[1];
            var c = v[2] - v[0];

            // Generate a random point in the trapezoid
            var result = v[0] + Random.Range(0f, 1f) * a + Random.Range(0f, 1f) * b;

            // Barycentric coordinates on triangles
            var alpha = ((v[1].z - v[2].z) * (result.x - v[2].x) + (v[2].x - v[1].x) * (result.z - v[2].z)) /
                        ((v[1].z - v[2].z) * (v[0].x - v[2].x) + (v[2].x - v[1].x) * (v[0].z - v[2].z));
            var beta = ((v[2].z - v[0].z) * (result.x - v[2].x) + (v[0].x - v[2].x) * (result.z - v[2].z)) /
                       ((v[1].z - v[2].z) * (v[0].x - v[2].x) + (v[2].x - v[1].x) * (v[0].z - v[2].z));
            var gamma = 1.0f - alpha - beta;

            // The selected point is outside of the triangle (wrong side of the trapezoid), project it inside through the center.
            if (alpha < 0 || beta < 0 || gamma < 0)
            {
                var center = v[0] + c / 2;
                center -= result;
                result += 2 * center;
            }

            return result;
        }
    }
}