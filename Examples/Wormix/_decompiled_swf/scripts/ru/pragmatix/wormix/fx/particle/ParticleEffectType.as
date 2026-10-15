package ru.pragmatix.wormix.fx.particle
{
   import starling.textures.Texture;
   
   public class ParticleEffectType
   {
      
      public static const POISON:ParticleEffectType = new ParticleEffectType("poison");
      
      public static const AMMO_TRAIL:ParticleEffectType = new ParticleEffectType("ammoTrailParticle");
      
      public static const AMMO_POISON_TRAIL:ParticleEffectType = new ParticleEffectType("AmmoPoisonTrailParticle");
      
      public static const FLYING_OBJECT_TRAIL:ParticleEffectType = new ParticleEffectType("FlyingObjectTrail");
      
      public static const LASER:ParticleEffectType = new ParticleEffectType("laserEnd");
      
      public static const REVENGE:ParticleEffectType = new ParticleEffectType("RevengeParticle");
      
      public static const ARCHBOT:ParticleEffectType = new ParticleEffectType("ArchbotParticle");
      
      public static const PALADIN_HEAL_TOP:ParticleEffectType = new ParticleEffectType("PaladinHealTop");
      
      public static const PALADIN_HEAL_BOTTOM:ParticleEffectType = new ParticleEffectType("PaladinHealBottom");
      
      public static const CLOUD:ParticleEffectType = new ParticleEffectType("cloudParticle");
      
      public static const ARMOR_REDUCTION:ParticleEffectType = new ParticleEffectType("ArmorReduction");
      
      private var configName:String;
      
      private var textureName:String;
      
      public function ParticleEffectType(param1:String, param2:String = null)
      {
         super();
         this.configName = param1;
         this.textureName = param2;
      }
      
      public function getConfigXML() : XML
      {
         return Application.assetManager.getXml(this.configName);
      }
      
      public function getTexture() : Texture
      {
         var _loc1_:String = this.textureName;
         if(_loc1_ == null)
         {
            _loc1_ = this.getConfigXML().texture.@name;
         }
         return Application.assetManager.getTexture(_loc1_);
      }
   }
}

