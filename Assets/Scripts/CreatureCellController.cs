using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCellController : MonoBehaviour {


    //public float ScreenSize; //vievport rect size(X and Y) (0.2)
    //public float FOV; (120)
    
    private float RectDistance = (1 / Mathf.Tan(60)) * 0.1f; //Distance between the camera position and the vievport rect
    //rect size: 0.2x0.2

    public Texture2D NodeTexture;

    private List<NodeRendering> NodeRenderings;
    private List<MusculeRendering> MusculeRenderings;
    











    public void Render(CreatureData data)
    {
        Vector3 CameraPosition = new Vector3(0, 0, -10);

        foreach(Node n in data.Nodes)
        {

            float NodeDistanceZ = n.NodePosition.z - CameraPosition.z;
            float k = NodeDistanceZ / RectDistance; // Triangle similarity scale
            float XYMaxOffset = k * 0.1f; // +-
            Vector2 RectOffsetRatios = new Vector2(n.NodePosition.x / XYMaxOffset, n.NodePosition.y / XYMaxOffset);

            Vector2 RectPosition = new Vector2(RectOffsetRatios.x * 0.1f, RectOffsetRatios.y * 0.1f); //0x0 for the center
            float DistanceToNode = Mathf.Sqrt((Mathf.Pow(NodeDistanceZ, 2) + Mathf.Pow(n.NodePosition.x, 2)) + Mathf.Pow(n.NodePosition.y, 2));
            float SizeOnRect = 0.3f / k; //0.3 is the node radius

            NodeRenderings.Add(new NodeRendering(RectPosition, SizeOnRect));
            
        }

        foreach(Muscule m in data.Muscles)
        {
            Vector2 StartPosition = NodeRenderings[m.NodeOneID].RectPosition;
            Vector2 EndPosition = NodeRenderings[m.NodeTwoID].RectPosition;

            float StartWidth = NodeRenderings[m.NodeOneID].RectSize / 3;//0.3 / 3 = 0.1, which is the line width
            float EndWidth = NodeRenderings[m.NodeTwoID].RectSize / 3;

            MusculeRenderings.Add(new MusculeRendering(StartPosition, EndPosition, StartWidth, EndWidth));
        }


        foreach(MusculeRendering mr in MusculeRenderings)
        {
            
        }



    }


}





public class NodeRendering
{
    public Vector2 RectPosition;
    public float RectSize;

    public NodeRendering(Vector2 PositionOnViewportRect, float SizeOnViewportRect)
    {
        RectSize = SizeOnViewportRect;
        RectPosition = PositionOnViewportRect;
    }

}

public class MusculeRendering
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public float StartWidth;
    public float EndWidth;

    public MusculeRendering(Vector2 StartPosition, Vector2 EndPosition, float StartWidth, float EndWidth)
    {
        this.StartPosition = StartPosition;
        this.EndPosition = EndPosition;
        this.StartWidth = StartWidth;
        this.EndWidth = EndWidth;
    }
}