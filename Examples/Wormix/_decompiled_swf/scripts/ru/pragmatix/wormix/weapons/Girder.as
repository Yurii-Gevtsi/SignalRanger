package ru.pragmatix.wormix.weapons
{
   import §_-62§.§_-W1r§;
   import §_-73d§.§_-F2J§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-F39§.§_-a2W§;
   import §_-L1H§.§_-b2K§;
   import §_-P2i§.§_-b7§;
   import §_-fo§.§_-r5§;
   import §_-jF§.ClanInviteRenderer;
   import §_-l1K§.§_-vO§;
   import §_-r1C§.§_-h2B§;
   import feathers.controls.Button;
   import feathers.controls.ScrollPolicy;
   import feathers.data.ListCollection;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.themes.styles.AlternateButtonsStyles;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import flash.display.MovieClip;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.*;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.physics.HitTest;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class Girder extends Weapon
   {
      
      public static const WIDTH:§_-TA§ = new §_-TA§(200);
      
      public static const §_-M2t§:§_-TA§ = new §_-TA§(310);
      
      public static const §_-m2D§:§_-TA§ = new §_-TA§(50);
      
      public static const §_-o1b§:§_-TA§ = new §_-TA§(10);
      
      public static const HEIGHT:§_-TA§ = new §_-TA§(20);
      
      public static const §_-j1g§:int = 100;
      
      public var §_-s2X§:Object;
      
      public function Girder()
      {
         super();
         §_-a1R§.value = false;
         §_-N1N§ = false;
         this.§_-s2X§ = {
            "indestructible":false,
            "resizable":false,
            "clipName":""
         };
         §_-z1W§(§_-vO§.§_-X1y§(this.§_-V1B§,this.§_-s2X§));
         §_-YH§ = true;
         §_-Y2U§ = new §_-F2J§();
      }
      
      override public function activate() : void
      {
         super.activate();
         ControlSets.UNIT_CONTROL_SET.deactivate();
         §_-b7§.§_-R1R§(owner);
      }
      
      override public function §_-z2Y§(param1:Object) : void
      {
         super.§_-z2Y§(param1);
         this.§_-s2X§.indestructible = §_-7u§(getProperty("indestructible")).value;
         this.§_-s2X§.resizable = §_-7u§(getProperty("resizable")).value;
         this.§_-s2X§.clipName = String(getProperty("clipGirder"));
         this.§_-s2X§.reducedAngle = getProperty("reducedAngle") ? getProperty("reducedAngle") : false;
      }
      
      private function §_-V1B§(param1:§_-b2K§) : Boolean
      {
         var _loc2_:Boolean = HitTest.girder(param1.point.x,param1.point.y,param1.angle,param1.power);
         if(!_loc2_ && §_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            §_-h2B§.play(§_-d27§.§_-b2H§);
            §_-a2W§.§_-1H§(§_-s2a§.§_-K3t§(§_-s2a§.GIRDER_IMPOSSIBLE),true);
         }
         return _loc2_;
      }
      
      override protected function shoot() : void
      {
         var _loc1_:MovieClip = §_-r5§.§_-tF§(this.§_-s2X§.clipName);
         _loc1_.x = shotParams.point.x;
         _loc1_.y = shotParams.point.y;
         this.§_-x1G§(_loc1_,shotParams.power,this.§_-s2X§.resizable);
         _loc1_.rotation = shotParams.angle;
         if(this.§_-s2X§.indestructible)
         {
            _loc1_.transform.colorTransform = Effects.GRAY_TRANSFORM;
         }
         §_-X1j§.§_-12n§.scene.addClipToGround(_loc1_,shotParams.point.x,shotParams.point.y,this.§_-s2X§.indestructible);
         §_-X1j§.§_-12n§.gameMaster.activeUnit.§_-vp§ = true;
         ControlSets.UNIT_CONTROL_SET.activate();
         if(§_-X1j§.§_-12n§.gameMaster.§_-E3b§())
         {
            §_-X1j§.§_-12n§.scene.gameInterface.§_-Q2D§();
         }
         §_-h2B§.play(§_-d27§.§_-k1F§);
         this.detach();
      }
      
      private function §_-x1G§(param1:MovieClip, param2:Number, param3:Boolean) : void
      {
         if(!param3)
         {
            param1.width = param2;
            return;
         }
         var _loc4_:MovieClip = param1.getChildByName("left") as MovieClip;
         var _loc5_:MovieClip = param1.getChildByName("right") as MovieClip;
         var _loc6_:MovieClip = param1.getChildByName("body") as MovieClip;
         _loc6_.width = param2 * 0.95;
         _loc4_.x = -param2 * 0.5;
         _loc6_.x = 0;
         _loc5_.x = param2 * 0.5;
      }
      
      override protected function §_-GV§(param1:§_-b2K§) : void
      {
         if(!active || shooting)
         {
            param1.clear();
            return;
         }
         shotParams.§_-L1P§(param1);
         §_-V2X§();
         deactivate();
         param1.clear();
      }
      
      override public function detach() : void
      {
         ControlSets.UNIT_CONTROL_SET.activate();
         super.detach();
      }
   }
}

