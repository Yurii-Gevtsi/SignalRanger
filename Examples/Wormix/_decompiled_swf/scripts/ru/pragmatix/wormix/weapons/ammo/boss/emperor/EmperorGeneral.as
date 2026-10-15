package ru.pragmatix.wormix.weapons.ammo.boss.emperor
{
   import §_-B1y§.§_-F1P§;
   import flash.utils.Dictionary;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.events.Event;
   
   public class EmperorGeneral extends EmperorTerracottaWarrior
   {
      
      private static const BONUS_VALUE:int = 20;
      
      private var buffed:Dictionary = new Dictionary();
      
      public function EmperorGeneral(param1:§_-01J§, param2:Number, param3:Number)
      {
         armatureName = "TerraccotaGeneral";
         name = §_-s2a§.§_-K3t§(§_-s2a§.BOSS_EMPEROR_GENERAL);
         super(param1,param2,param3);
         this.buff(1);
      }
      
      private static function textColor(param1:int) : String
      {
         return param1 > 0 ? Effects.TEXT_COLOR_GREEN : Effects.TEXT_COLOR_RED;
      }
      
      public function buff(param1:int) : void
      {
         var _loc3_:§_-01J§ = null;
         var _loc2_:int = BONUS_VALUE * param1;
         this.addBonus(owner,_loc2_);
         param1 = param1 < 0 ? 1 : -1;
         _loc2_ = BONUS_VALUE * param1;
         for each(_loc3_ in §_-X1j§.§_-12n§.gameMaster.§_-83I§(owner))
         {
            if(param1 < 0)
            {
               this.buffed[_loc3_] = true;
               this.addBonus(_loc3_,_loc2_);
            }
            else if(param1 > 0 && Boolean(this.buffed[_loc3_]))
            {
               this.addBonus(_loc3_,_loc2_);
            }
         }
      }
      
      public function addBonus(param1:§_-01J§, param2:int) : void
      {
         param1.attributes.armor.§_-Y2z§(param2);
         param1.attributes.attack.§_-Y2z§(param2);
         var _loc3_:String = §_-F1P§.format(§_-s2a§.§_-K3t§(§_-s2a§.ATTRIBUTES_BONUS),{
            "symbol":(param2 > 0 ? "+" : ""),
            "value":param2
         });
         Effects.floatingText(_loc3_,textColor(param2),param1.x + param1.width,param1.y + param1.height * 0.5,20,BitmapFonts.AD_LIB);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(!§_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-C6§())
         {
            this.buff(-1);
         }
         super.defaultOnDisposed(param1);
      }
   }
}

