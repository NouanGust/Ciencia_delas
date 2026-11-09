using UnityEngine;
using UnityEngine.UI;

namespace CienciaDelas
{
    // Draw only the four edges; a regular Outline duplicates the entire Image quad.
    public sealed class MolduraFoco : BaseMeshEffect
    {
        public Color cor = new Color(.3f, .85f, 1);
        [Min(1)] public float espessura = 2;

        public override void ModifyMesh(VertexHelper malha)
        {
            if (!IsActive()) return;
            malha.Clear();
            var r = ((RectTransform)transform).rect;
            float e = Mathf.Min(espessura, Mathf.Min(r.width, r.height) / 2);
            void Faixa(float x, float y, float w, float h)
            {
                int n = malha.currentVertCount;
                malha.AddVert(new Vector3(x, y), cor, Vector2.zero);
                malha.AddVert(new Vector3(x, y + h), cor, Vector2.zero);
                malha.AddVert(new Vector3(x + w, y + h), cor, Vector2.zero);
                malha.AddVert(new Vector3(x + w, y), cor, Vector2.zero);
                malha.AddTriangle(n, n + 1, n + 2); malha.AddTriangle(n + 2, n + 3, n);
            }
            Faixa(r.xMin, r.yMin, r.width, e);
            Faixa(r.xMin, r.yMax - e, r.width, e);
            Faixa(r.xMin, r.yMin + e, e, r.height - 2 * e);
            Faixa(r.xMax - e, r.yMin + e, e, r.height - 2 * e);
        }
    }
}
