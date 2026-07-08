package ru.pragmatix.wormix.objects
{
   import §_-31N§.§_-42G§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-4I§;
   import §_-CF§.§_-E19§;
   import §_-D3A§.§_-TZ§;
   import §_-F39§.*;
   import §_-H2g§.§_-di§;
   import §_-H2g§.§_-s1d§;
   import §_-Tm§.§_-RF§;
   import §_-hO§.§_-v24§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import com.hurlant.crypto.tls.*;
   import com.hurlant.util.der.§_-UO§;
   import dragonBones.animation.WorldClock;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starlingbuilder.extensions.uicomponents.ContainerButtonWithStates;
   
   public class §_-V1L§ extends §_-F3o§
   {
      
      private static const ARMATURE_NAME:String = "LearningMissionMark";
      
      private var armature:§_-di§;
      
      private var arrow:§_-di§ = null;
      
      public function §_-V1L§(param1:int, param2:int)
      {
         var x:int = param1;
         var y:int = param2;
         super();
         §_-Nt§ = true;
         physParams.addForce(Force.GRAVITY);
         physParams.collidesWith = PhysParams.COLLIDES_WITH_GROUND;
         physParams.collideType = CollideType.OTHER;
         §_-TZ§.addListener(this,§_-L3Q§.§_-Qw§,this.onDamaged);
         addEventListener(§_-L3Q§.§_-o13§,function(param1:Event):void
         {
            §_-h2B§.play(§_-d27§.BONUS);
         });
         this.armature = §_-K1t§.buildArmature(ARMATURE_NAME,§_-K1t§.§_-G1E§,§_-K1t§.§_-B1s§);
         WorldClock.clock.add(this.armature);
         this.armature.playAnimation(§_-K1t§.§_-G1E§);
         _width = _height = 65;
         §_-z1H§ = this.armature.display;
         this.x = x;
         this.y = y;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this,true);
      }
      
      public function §_-l2l§(param1:uint = 2) : void
      {
         if(stable)
         {
            this.§_-s1j§(param1);
         }
         else
         {
            §_-Zd§.once(§_-Y2I§ as EventDispatcher,§_-42G§.§_-K2J§,§_-bs§.§_-d1Z§(this.§_-s1j§,this,[param1]));
         }
      }
      
      private function §_-s1j§(param1:uint) : void
      {
         this.arrow = §_-X1j§.§_-12n§.scene.showDragonBonedClip("TutorialArrowMap",x,y - §_-z1H§.height * §_-K1t§.§_-h2z§ / 2);
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         WorldClock.clock.remove(this.armature);
         super.defaultOnDisposed(param1);
         this.armature.dispose();
         this.armature = null;
         if(this.arrow)
         {
            §_-X1j§.§_-12n§.scene.hideDragonBonedClip(this.arrow);
            this.arrow = null;
         }
      }
      
      private function onDamaged(param1:Event) : void
      {
         §_-TZ§.removeListener(this,§_-L3Q§.§_-Qw§,this.onDamaged);
         physParams.collideType = CollideType.NONE;
         this.armature.playAnimation(§_-K1t§.§_-Y1c§);
         this.armature.addEventListener(§_-s1d§.COMPLETE,this.§_-yN§);
      }
      
      private function §_-yN§(param1:Event) : void
      {
         this.armature.removeEventListener(§_-s1d§.COMPLETE,this.§_-yN§);
         this.armature.stopAnimation();
         disposed = true;
      }
   }
}

