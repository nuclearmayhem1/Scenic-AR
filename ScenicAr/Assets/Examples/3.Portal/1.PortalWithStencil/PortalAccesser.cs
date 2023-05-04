using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalAccesser : MonoBehaviour
{
    private int OutsideStencil = 1, InsideStencil = 1;

    public Material portalMaterial;
    public Material[] stencilOutMaterials;
    public Material[] stencilInMaterials;

    private void OnTriggerStay(Collider other)
    {
        var nearPlanePos = Camera.main.transform.position + Camera.main.transform.forward * Camera.main.nearClipPlane;
        var cameraRelativeToLocal = transform.worldToLocalMatrix.MultiplyPoint(nearPlanePos);

        int outsideCompareFunction = (int)CompareFunction.Equal;
        int insideCompareFunction = (int)CompareFunction.Equal;

        /*if (this.GetComponent<Collider>().bounds.Contains(nearPlanePos))
        {
            outsideCompareFunction = (int)CompareFunction.Always;
        }
        else*/
        {
            if (cameraRelativeToLocal.z < 0f)
            {
                outsideCompareFunction = (int)CompareFunction.NotEqual;
            }
            else
            {
                insideCompareFunction = (int)CompareFunction.NotEqual;
            }
        }

        foreach (var mat in stencilOutMaterials)
        {
            mat.SetInt("_StencilComp", outsideCompareFunction);
        }

        foreach (var mat in stencilInMaterials)
        {
            mat.SetInt("_StencilComp", insideCompareFunction);
        }

    }
}
