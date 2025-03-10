using UnityEngine;

public class TextureScaler : MonoBehaviour {

    public enum ProjectionPlane { XY, XZ, YZ } 

    [SerializeField] private ProjectionPlane projectionPlane = ProjectionPlane.XZ;

    [SerializeField] private float scaleCoeff = .15f; 

    private MeshRenderer rend;

    private void Start() {
        rend = GetComponent<MeshRenderer>();
        UpdateTiling();
    }

    private void UpdateTiling() {

        Vector3 scale = transform.lossyScale; 

        Vector2 tiling = projectionPlane switch {
            ProjectionPlane.XY => new Vector2(scale.x, scale.y),
            ProjectionPlane.XZ => new Vector2(scale.x, scale.z),
            ProjectionPlane.YZ => new Vector2(scale.y, scale.z),
            _ => Vector2.one
        };

        rend.material.mainTextureScale = tiling * scaleCoeff;
    }

}