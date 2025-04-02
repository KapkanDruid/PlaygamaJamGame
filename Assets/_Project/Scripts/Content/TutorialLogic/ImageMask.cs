using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ImageMask : Image
{
    public override Material materialForRendering{
        get{
            Material material = new Material(base.materialForRendering);
            material.SetInt(name: "_StencilComp", value: (int)CompareFunction.NotEqual);
            return material; 
        }        
    }
    
}
