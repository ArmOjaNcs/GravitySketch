using UnityEngine;

public static class ParticleSpriteBaker
{
    public static void PrepareParticle(ParticleSystem ps)
    {
        var r = ps.GetComponent<Renderer>();
        var mat = r.sharedMaterial;

        mat.SetFloat("_Mode", 2); 
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }
}