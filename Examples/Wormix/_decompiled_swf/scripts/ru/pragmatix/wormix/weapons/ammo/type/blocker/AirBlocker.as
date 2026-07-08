package ru.pragmatix.wormix.weapons.ammo.type.blocker
{
   import §_-12a§.§_-bY§;
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-YF§.§_-q2v§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-h2B§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.weapons.ammo.phase.ChainPhase;
   import starling.display.DisplayObject;
   import starling.events.Event;
   import starling.utils.Pool;
   
   public class AirBlocker extends AbstractBlocker
   {
      
      protected static const MAX_UNGROUNDED_TIME:uint = 40;
      
      protected static const INITIAL_RADIUS:uint = 250;
      
      protected static const RADIUS_INCREASE_RATE:uint = 25;
      
      private static const DURATION:§_-TA§ = new §_-TA§(10);
      
      protected static const DETECTION_PERIOD:uint = 5;
      
      protected var radius:uint = 250;
      
      protected var active:Boolean = false;
      
      protected var ownerColorDetector:DisplayObject;
      
      protected var debuffClass:Class = §_-bY§;
      
      private var debuffAuraClipName:String;
      
      public function AirBlocker(param1:§_-01J§, param2:§_-C3e§, param3:Number, param4:uint, param5:String = "")
      {
         this.debuffAuraClipName = param5 || §_-q2v§.§_-c1h§;
         super(param1,param2,param3,param4,DURATION);
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.SOT,this.onSot);
         §_-h2B§.play(§_-d27§.§_-G3Z§);
         this.setOwnerTeamColor();
      }
      
      override public function nextState() : void
      {
         super.nextState();
         if(this.active && stable && §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-O1C§() % DETECTION_PERIOD == 0)
         {
            this.checkTargets();
         }
      }
      
      override protected function changeClipToIdle() : void
      {
         armature.playAnimation(§_-K1t§.§_-G1E§);
      }
      
      protected function checkTargets() : void
      {
         var _loc1_:Vector.<§_-01J§> = null;
         var _loc3_:§_-01J§ = null;
         _loc1_ = §_-X1j§.§_-12n§.gameMaster.§_-83I§(owner);
         var _loc2_:Vector.<§_-01J§> = Physics.getUnitsInCircle(x,y,this.radius,_loc1_);
         for each(_loc3_ in _loc2_)
         {
            if(!_loc3_.stable && !_loc3_.buffs.hasBuff(§_-bY§.NAME))
            {
               _loc3_.buffs.place(new this.debuffClass(this,Pool.getPoint(x,y),this.radius,MAX_UNGROUNDED_TIME,paramSet.damage.value,this.debuffAuraClipName));
            }
         }
      }
      
      override protected function onCollide(param1:Event) : void
      {
         destroyDuplicates(AirBlocker);
         super.onCollide(param1);
      }
      
      private function setOwnerTeamColor() : void
      {
         var _loc4_:DisplayObject = null;
         var _loc1_:uint = owner.squad.getNumber();
         var _loc2_:Array = ["animGreen","animRed","animYellow","animBlue"];
         var _loc3_:* = int(_loc2_.length);
         while(_loc3_--)
         {
            _loc4_ = this.showOnlySquadColor(_loc2_[_loc3_],_loc3_ == _loc1_);
            if(_loc3_ == _loc1_)
            {
               this.ownerColorDetector = _loc4_;
            }
         }
         this.updateAnimationVisibility();
      }
      
      private function showOnlySquadColor(param1:String, param2:Boolean) : DisplayObject
      {
         var _loc3_:DisplayObject = armature.source.getSlot(param1).display;
         if(_loc3_)
         {
            _loc3_.visible = param2;
         }
         return _loc3_;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         var _loc2_:Vector.<§_-01J§> = null;
         var _loc3_:§_-01J§ = null;
         §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.SOT,this.onSot);
         if(!§_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-C6§())
         {
            _loc2_ = §_-X1j§.§_-12n§.gameMaster.§_-83I§(owner);
            for each(_loc3_ in _loc2_)
            {
               if(_loc3_.buffs.hasBuff(§_-bY§.NAME))
               {
                  _loc3_.buffs.§_-e1z§(§_-bY§.NAME);
               }
            }
         }
         this.ownerColorDetector = null;
         super.defaultOnDisposed(param1);
      }
      
      protected function onSot(param1:Event) : void
      {
         if(!disposed)
         {
            this.radius += RADIUS_INCREASE_RATE;
            if(§_-X1j§.§_-12n§.gameMaster.activeUnit.squad.getTeamId() != owner.squad.getTeamId())
            {
               this.active = true;
               §_-h2B§.play(§_-d27§.§_-51E§);
            }
            else
            {
               this.active = false;
            }
            this.updateAnimationVisibility();
         }
      }
      
      private function updateAnimationVisibility() : void
      {
         this.ownerColorDetector.visible = this.active;
      }
      
      override protected function onDamaged(param1:Event) : void
      {
         var _loc2_:§_-01J§ = null;
         var _loc3_:uint = 0;
         var _loc4_:uint = 0;
         var _loc5_:ChainPhase = null;
         if(Boolean(param1.data.source) && Boolean(param1.data.source.owner) && !disposed)
         {
            _loc2_ = param1.data.source.owner;
            if(_loc2_.squad.getTeamId() != owner.squad.getTeamId())
            {
               _loc3_ = 30;
               _loc4_ = 340;
               _loc5_ = new ChainPhase(owner,x,y,_loc2_,paramSet.damage.value,_loc3_);
               _loc5_.setSpeedVector(-MathUtil.PI_HALF,_loc3_);
               _loc5_.setBounceDistance(_loc4_);
            }
         }
         super.onDamaged(param1);
      }
   }
}

