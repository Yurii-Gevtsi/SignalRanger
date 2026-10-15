package ru.pragmatix.wormix.weapons
{
   import §_-A1K§.§_-TA§;
   import §_-D3A§.§_-TZ§;
   import §_-L1H§.§_-C3e§;
   import §_-b1F§.§_-G4§;
   import §_-l1g§.§_-L3Q§;
   import flash.display.BitmapData;
   import flash.display.Shape;
   import flash.geom.Point;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.objects.§_-Qi§;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-Py§;
   import ru.pragmatix.wormix.weapons.ammo.type.missile.homing.HomingMissile;
   import starling.display.Image;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   
   public class §_-A2f§ extends §_-F3o§
   {
      
      private const MAX_FRAME:uint = 60;
      
      private const §_-F2S§:uint = 4;
      
      private var §_-Oq§:uint;
      
      private var owner:§_-01J§;
      
      private var §_-xX§:§_-C3e§ = new §_-C3e§();
      
      private var countFrame:uint = 0;
      
      private var §_-23Z§:uint;
      
      private var §_-Q2Y§:BitmapData;
      
      private var §_-Y2§:Texture;
      
      private var §_-VG§:Image;
      
      private var §_-l2p§:int;
      
      public function §_-A2f§(param1:§_-01J§, param2:Number, param3:Number, param4:uint, param5:uint, param6:uint)
      {
         this.owner = param1;
         this.§_-Oq§ = param4;
         this.§_-23Z§ = param5;
         this.§_-xX§.ammoClassName = "AmAirStrike";
         this.§_-xX§.damage = new §_-TA§(param6);
         this.§_-xX§.damageRadius = new §_-TA§(50);
         this.§_-xX§.explosionRadius = new §_-TA§(35);
         this.§_-l2p§ = Math.pow(param4 + 25,2);
         super();
         var _loc7_:Shape = new Shape();
         _loc7_.graphics.lineStyle(1,26316,0.2);
         _loc7_.graphics.beginFill(26316,0.2);
         _loc7_.graphics.drawCircle(param4,param4,param4);
         _loc7_.graphics.endFill();
         this.§_-Q2Y§ = new BitmapData(_loc7_.width,_loc7_.height,true,0);
         this.§_-Q2Y§.draw(_loc7_);
         this.§_-Y2§ = Texture.fromBitmapData(this.§_-Q2Y§,false);
         this.§_-VG§ = new Image(this.§_-Y2§);
         this.§_-VG§.pivotX = this.§_-VG§.pivotY = param4;
         §_-z1H§ = this.§_-VG§;
         var _loc8_:IPhysicModel = getPhysicModel();
         _loc8_.x = param2;
         _loc8_.y = param3;
         _loc8_.stable = true;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         §_-TZ§.addListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.nextTurn);
      }
      
      override public function nextState() : void
      {
         var _loc1_:Point = null;
         var _loc2_:IPhysicModel = null;
         var _loc3_:* = 0;
         super.nextState();
         if(++this.countFrame >= this.MAX_FRAME + §_-X1j§.randomSeed.§_-Fh§(-10,10))
         {
            this.countFrame = 0;
            _loc2_ = getPhysicModel();
            _loc3_ = int(5 + §_-X1j§.randomSeed.§_-Fh§(-4,0));
            while(_loc3_--)
            {
               _loc1_ = §_-Py§.§_-41E§(this.§_-Oq§,_loc2_.x,_loc2_.y);
               Effects.createDragonbonesAnimationEffect(EffectType.EXPLOSION_ELECTRIC_SMALL,_loc1_.x,_loc1_.y,20);
               Pool.putPoint(_loc1_);
            }
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         this.owner = null;
         this.§_-xX§ = null;
         §_-TZ§.removeListener(§_-X1j§.§_-12n§.scene,§_-L3Q§.SOT,this.nextTurn);
         super.defaultOnDisposed(param1);
         this.§_-Y2§.dispose();
         this.§_-Y2§ = null;
         this.§_-Q2Y§.dispose();
         this.§_-Q2Y§ = null;
      }
      
      private function nextTurn(param1:Event) : void
      {
         var _loc2_:§_-01J§ = null;
         var _loc5_:IPhysicModel = null;
         var _loc9_:uint = 0;
         var _loc10_:int = 0;
         var _loc11_:IPhysicModel = null;
         var _loc12_:Point = null;
         var _loc3_:Vector.<§_-01J§> = §_-X1j§.§_-12n§.gameMaster.§_-83I§(this.owner);
         var _loc4_:IPhysicModel = getPhysicModel();
         var _loc6_:* = int(_loc3_.length);
         var _loc7_:int = _loc4_.x;
         var _loc8_:int = _loc4_.y;
         if(§_-X1j§.§_-12n§.gameMaster.§_-y2v§() && _loc3_.length != 0)
         {
            while(_loc6_--)
            {
               _loc2_ = _loc3_[_loc6_];
               _loc5_ = _loc2_.getPhysicModel();
               if(MathUtil.§_-Ih§(_loc5_.x,_loc5_.y,_loc7_,_loc8_) <= this.§_-l2p§)
               {
                  §_-G4§.inflictDamage(new §_-Qi§(this.owner,_loc7_,_loc8_,false,1),_loc2_ as §_-F3o§,this.§_-23Z§,60,60);
                  disposed = true;
                  return;
               }
            }
            _loc9_ = 0;
            _loc10_ = 0;
            while(_loc10_ < this.§_-F2S§)
            {
               _loc2_ = _loc3_[_loc9_];
               _loc11_ = _loc2_.getPhysicModel();
               if(!_loc2_.disposed)
               {
                  _loc12_ = MathUtil.§_-41E§(this.§_-Oq§,x,y);
                  new HomingMissile(this.owner,_loc12_,this.§_-xX§,Math.atan2(_loc11_.y - y,_loc11_.x - x) + §_-X1j§.randomSeed.§_-Fh§(-30,30) * MathUtil.PI_ONE_360,25,Pool.getPoint(_loc11_.x,_loc11_.y),0);
                  Pool.putPoint(_loc12_);
               }
               if(_loc9_ >= _loc3_.length - 1)
               {
                  _loc9_ = 0;
               }
               else
               {
                  _loc9_++;
               }
               _loc10_++;
            }
         }
         disposed = true;
      }
   }
}

