using Assets.Sources.Utils;
using UnityEngine;

namespace Assets.Sources.EnemyScripts
{
    [RequireComponent(typeof(Renderer))]
    public abstract class EnemyMissileWithRenderer : EnemyMissile
    {
        private protected Renderer Renderer;
        private protected MaterialPropertyBlock MPropertyBlock;

        private protected override void OnEnable()
        {
            base.OnEnable();

            if (Renderer != null)
                Renderer.enabled = true;
        }

        public override void InitFromConfig(MissileConfig missileConfig, EnemyAttackZone attackZone)
        {
            base.InitFromConfig(missileConfig, attackZone);
            Renderer = GetComponent<Renderer>();
            MPropertyBlock = new MaterialPropertyBlock();
            SetColor(missileConfig.Color);
        }

        private protected override void Interact()
        {
            Renderer.enabled = false;
            base.Interact();
        }

        private protected void SetColor(Color color)
        {
            if (Renderer == null)
                return;

            if (MPropertyBlock == null)
                MPropertyBlock = new MaterialPropertyBlock();

            Renderer.GetPropertyBlock(MPropertyBlock);
            MPropertyBlock.SetColor(UserUtils.ColorID, color);
            Renderer.SetPropertyBlock(MPropertyBlock);
        }
    }
}