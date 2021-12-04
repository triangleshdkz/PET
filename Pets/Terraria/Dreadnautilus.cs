using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace PET.Pets.Terraria
{
    public class Dreadnautilus_Item : PetModItem<Dreadnautilus_Projectile, Dreadnautilus_Buff>
    {
    }

    public class Dreadnautilus_Projectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 5;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }

        private float _vfxTimer = 0f;

        private const float Speed = 8f;

        private int faceTowardsOwner()
        {
            return Main.player[projectile.owner].Center.X > projectile.Center.X ? -1 : 1;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            PetPlayer petPlayer = Main.player[projectile.owner].GetModPlayer<PetPlayer>();
            if (player.dead)
                petPlayer.dreadnautilus = false;
            if (petPlayer.dreadnautilus)
                projectile.timeLeft = 2;
            Vector2 center = projectile.Center;
            Vector2 gotoPos = player.Center + new Vector2(0f, -30f);
            Vector2 difference = gotoPos - center;
            float distance = difference.Length();
            switch (projectile.ai[0])
            {
                case 0f:
                projectile.gfxOffY = (float)Math.Sin(_vfxTimer) * 2f;
                float timerAdd = 1f - projectile.velocity.Length() / Speed;
                _vfxTimer += timerAdd < 0f ? 0f : timerAdd * 0.0785f;
                projectile.rotation = projectile.rotation.AngleLerp(projectile.velocity.X * 0.065f, 0.0314f);
                if (distance > 250f)
                {
                    projectile.spriteDirection = faceTowardsOwner();
                    projectile.velocity = Vector2.Normalize(difference) * MathHelper.Clamp(projectile.velocity.Length() + 0.06f, 0f, Speed + player.velocity.Length() * 0.8f);
                }
                else if (distance > 200f && player.velocity.Length() > Speed) // simple trick that reduces the "bump" effect of this type of homing
                {
                    projectile.spriteDirection = faceTowardsOwner();
                    projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Normalize(difference) * MathHelper.Clamp(projectile.velocity.Length(), 0f, player.velocity.Length()), 0.135f);
                }
                else
                {
                    projectile.spriteDirection = faceTowardsOwner();
                    projectile.velocity *= 0.98f;
                    if (Main.rand.NextBool(800))
                    {
                        if (Main.rand.NextBool())
                            projectile.ai[0] = 2f;
                        else
                        {
                            projectile.ai[0] = 3f;
                            projectile.ai[1] = 0f;
                        }
                        projectile.netUpdate = true;
                    }
                }
                break;

                case 1f: // 
                float currentAngle = projectile.velocity.ToRotation();
                float gotoAngle = currentAngle.AngleTowards(difference.ToRotation(), MathHelper.PiOver2);
                projectile.velocity = new Vector2(12f + player.velocity.Length() * 0.25f, 0f).RotatedBy(currentAngle.AngleLerp(gotoAngle, 0.04f));
                projectile.rotation = projectile.velocity.ToRotation();
                if (distance < 240)
                {
                    projectile.ai[1]--;
                    if (projectile.ai[1] <= 0f)
                    {
                        projectile.ai[0] = 0f;
                        projectile.ai[1] = 0f;
                        projectile.netUpdate = true;
                    }
                }
                break;

                case 2f:
                projectile.velocity = Vector2.Normalize(difference) * MathHelper.Clamp(projectile.velocity.Length() + 0.06f, 0f, Speed + player.velocity.Length() * 0.8f);
                break;

                case 3f:
                projectile.ai[1]++;
                projectile.rotation = projectile.rotation.AngleLerp(projectile.velocity.X * 0.065f, 0.0314f);
                projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Normalize(difference) * MathHelper.Clamp(projectile.velocity.Length(), 0f, player.velocity.Length()), 0.135f);
                if (projectile.ai[1] > 240f)
                    projectile.ai[0] = 0f;
                break;
            }
            if (projectile.wet)
                Lighting.AddLight(projectile.Center, new Vector3(0.4f, 0.1f, 0.1f));
            projectile.frameCounter++;
            if (projectile.frameCounter > 6)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame >= Main.projFrames[projectile.type])
                    projectile.frame = 0;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            var texture = Main.projectileTexture[projectile.type];
            int frameHeight = texture.Height / Main.projFrames[projectile.type];
            var frame = new Rectangle(0, frameHeight * projectile.frame, texture.Width, frameHeight - 2);
            var origin = frame.Size() / 2f;
            var center = projectile.Center - Main.screenPosition;
            var effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            if (projectile.ai[0] == 3f)
            {
                float value = 2f + (float)(Math.Sin(projectile.ai[1] * 0.0628f) + 1d);
                for (int i = 0; i < 4; i++)
                {
                    Vector2 off;
                    switch (i)
                    {
                        default:
                        off = new Vector2(value, 0f);
                        break;
                        case 1:
                        off = new Vector2(-value, 0f);
                        break;
                        case 2:
                        off = new Vector2(0f, value);
                        break;
                        case 3:
                        off = new Vector2(0f, -value);
                        break;
                    }
                    spriteBatch.Draw(texture, center + off, frame, new Color(255, 0, 0, 0), projectile.rotation, origin, projectile.scale, effects, 0f);
                    lightColor = Color.Lerp(lightColor, new Color(255, 0, 0, 255), (float)((Math.Sin(projectile.ai[1] * 0.0628f) + 1d) / 2d) * 0.025f);
                }
            }
            spriteBatch.Draw(texture, center, frame, lightColor, projectile.rotation, origin, projectile.scale, effects, 0f);
            return false;
        }

        private void Set_Dash()
        {
            projectile.ai[0] = 0f;
            projectile.ai[1] = 0f;
        }
    }

    public class Dreadnautilus_Buff : PetModBuff
    {
        public override void SetBuffValues(PetPlayer plr) => plr.dreadnautilus = true;

        public override bool ShouldSpawnPet(Player plr) => plr.ownedProjectileCounts[ModContent.ProjectileType<Dreadnautilus_Projectile>()] < 1;

        public override void SpawnPet(Player plr)
        {
            Projectile.NewProjectile(plr.Center, new Vector2(0f, 0f), ModContent.ProjectileType<Dreadnautilus_Projectile>(), 0, 0f, plr.whoAmI);
        }
    }
}