using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
namespace RaidersMod.NPCs
{
    public class FlyingSlime_Pinky : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Pinky");
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.lifeMax = 250;
            npc.defense = 15;
            npc.knockBackResist = 0.08f;
            npc.width = 20;
            npc.height = 24;
            npc.dontTakeDamageFromHostiles = true;
            npc.damage = 50;
            npc.aiStyle = -1;
            npc.friendly = false;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 100f;
            npc.noGravity = true;
            npc.noTileCollide = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(spawnInfo.player.ZoneSkyHeight && Main.hardMode)
            {
                return 0.02f;
            } else return 0f;
        }

        private float Attackcounter;
        public override void AI()
        {
            npc.TargetClosest(true);
            Attackcounter++;
            Player player = Main.player[npc.target];
            if(  npc.target < 0 || npc.target == 255 || player.dead)
            {
                npc.TargetClosest(true);
            }
            if(!npc.HasValidTarget || !player.ZoneSkyHeight)
            {
                npc.velocity.Y -= 0.3f;
                npc.timeLeft = 30;
                npc.noTileCollide = true;
            } else
            {
                npc.velocity = Vector2.Normalize(player.Center - npc.Center) * 4.4f;  

                if(Attackcounter >= 360)
                {
                    Vector2 direction = Vector2.Normalize(player.Center - npc.Center) * 5f;
                    Projectile.NewProjectile(npc.Center,direction * 2,ProjectileID.HarpyFeather,23,1);
                    Attackcounter = 0f;
                }
                npc.spriteDirection = npc.direction;
            }
        }
        private int FrameTimer;
        private int frameCount;
     public override void FindFrame(int frameHeight)
    {
        if(++FrameTimer > 8)
        {
            frameCount++;
            if(frameCount == 4) frameCount = 0;
            FrameTimer = 0;
        }
        npc.frame.Y = frameHeight * frameCount;
    }
       public override void NPCLoot()
    {
        if(Main.rand.Next(3) == 2)
        {
            Item.NewItem(npc.position,ModContent.ItemType<Items.weapons.SlimeRifle>());   
        }
        Item.NewItem(npc.position,ModContent.ItemType<Items.craftingMaterials.SlimeFeather>(),Main.rand.Next(5,9));
        Item.NewItem(npc.Center,npc.getRect().Size(),ItemID.PinkGel,Main.rand.Next(4));
    }
    }
}
