package ru.pragmatix.wormix.weapons.ammo.cloud
{
   import §_-12a§.§_-520§;
   import §_-31Y§.§_-D3B§;
   import §_-91A§.§_-sH§;
   import §_-A1K§.§_-TA§;
   import §_-L1H§.§_-C3e§;
   import §_-hO§.§_-y2Z§;
   import §_-l1g§.§_-L3Q§;
   import feathers.controls.LayoutGroup;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.fx.ParticleCloudEffect;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.scene.ObjectType;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.extensions.ColorArgb;
   
   public class BasicCloud extends §_-F3o§ implements §_-sH§
   {
      
      private static const RADIUS:§_-TA§ = new §_-TA§(62);
      
      private static const WIDTH_IN_RADII:int = 1;
      
      private var counter:§_-TA§;
      
      private var duration:uint;
      
      private var squadNumber:uint;
      
      private var effect:ParticleCloudEffect;
      
      protected var owner:§_-01J§;
      
      protected var paramSet:§_-C3e§;
      
      public function BasicCloud(param1:§_-01J§, param2:int, param3:int, param4:§_-C3e§, param5:ColorArgb, param6:uint)
      {
         var back:LayoutGroup = null;
         var owner:§_-01J§ = param1;
         var x:int = param2;
         var y:int = param3;
         var paramSet:§_-C3e§ = param4;
         var color:ColorArgb = param5;
         var duration:uint = param6;
         this.counter = new §_-TA§(0);
         super(false,ObjectType.FRONT_ENVIRONMENT,SortingLayer.EFFECT_LAYER);
         this.squadNumber = owner.squad.getNumber();
         this.x = x;
         this.y = y - (RADIUS.value >> 2);
         this.duration = duration;
         this.owner = owner;
         this.paramSet = paramSet;
         tags = new <§_-D3B§>[§_-D3B§.§_-E25§];
         physParams.collidesWith = PhysParams.COLLIDES_WITH_NOTHING;
         physParams.ignoresTerrain = true;
         this.effect = Effects.createParticleCloudEffect(color,x,y);
         back = new LayoutGroup();
         back.width = 200;
         back.height = 100;
         back.alignPivot();
         §_-z1H§ = new Sprite();
         with(§_-z1H§ as Sprite)
         {
            addChild(back);
         }
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.UNIT_DEACTIVATION,this.onEndTurn);
         stable = true;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
      }
      
      override public function updateClipPos() : void
      {
         super.updateClipPos();
         if(§_-z1H§)
         {
            this.effect.x = §_-z1H§.x;
            this.effect.y = §_-z1H§.y;
         }
      }
      
      private function onEndTurn(param1:Event) : void
      {
         var _loc2_:Number = NaN;
         var _loc4_:§_-01J§ = null;
         var _loc3_:int = -WIDTH_IN_RADII;
         while(_loc3_ <= WIDTH_IN_RADII)
         {
            _loc2_ = x + RADIUS.value * _loc3_;
            for each(_loc4_ in Physics.getUnitsInCircle(_loc2_,y,RADIUS.value))
            {
               if(this.isUnitAffected(_loc4_))
               {
                  this.applyOnUnit(_loc4_);
               }
            }
            _loc3_++;
         }
         if(++this.counter.value >= this.duration)
         {
            disposed = true;
         }
         else
         {
            dispatchEvent(new §_-y2Z§(this,this.getTurnsLeft()));
         }
      }
      
      private function getTurnsLeft() : int
      {
         return this.duration - this.counter.value;
      }
      
      protected function isUnitAffected(param1:§_-01J§) : Boolean
      {
         return !param1.buffs.hasBuff(§_-520§.NAME);
      }
      
      protected function applyOnUnit(param1:§_-01J§) : void
      {
         param1.buffs.place(new §_-520§());
      }
      
      public function isUnitInCloud(param1:§_-01J§) : Boolean
      {
         var _loc3_:Number = NaN;
         var _loc2_:int = -WIDTH_IN_RADII;
         while(_loc2_ <= WIDTH_IN_RADII)
         {
            _loc3_ = x + RADIUS.value * _loc2_;
            if(Physics.isGobjInRadius(param1 as §_-F3o§,_loc3_,y,RADIUS.value,true))
            {
               return true;
            }
            _loc2_++;
         }
         return false;
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         if(this.effect)
         {
            this.effect.stop();
            this.effect = null;
         }
         §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.UNIT_DEACTIVATION,this.onEndTurn);
         this.owner = null;
         super.defaultOnDisposed(param1);
      }
      
      public function getNameHintInfoPanel() : String
      {
         return "";
      }
      
      public function getSquadNumber() : uint
      {
         return this.squadNumber;
      }
      
      public function get hp() : int
      {
         return this.getTurnsLeft();
      }
      
      public function get doesntCountTurns() : Boolean
      {
         return false;
      }
   }
}

