package ru.pragmatix.wormix.game.ai
{
   import §_-31i§.§_-62p§;
   import §_-31i§.§_-I1g§;
   import §_-31i§.§_-d1n§;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-O2H§;
   import §_-73I§.§_-K2s§;
   import §_-91B§.§_-z2l§;
   import §_-9e§.§_-73e§;
   import §_-9e§.§_-8O§;
   import §_-9e§.§_-A18§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-yG§;
   import §_-CF§.§_-E19§;
   import §_-CF§.§_-x2t§;
   import §_-DJ§.§_-J2l§;
   import §_-L1H§.§_-b2K§;
   import §_-U1i§.§_-hK§;
   import §_-Z1K§.§_-q24§;
   import §_-j22§.§_-5g§;
   import §_-k2s§.§_-Go§;
   import §_-k2s§.§_-b0§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-v2t§.§_-73F§;
   import §_-w2i§.§_-Zd§;
   import com.distriqt.extension.inappbilling.Product;
   import dragonBones.animation.WorldClock;
   import flash.display.Shape;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import flash.utils.getQualifiedClassName;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.mobile.§_-QR§;
   import ru.pragmatix.wormix.messages.server.AchieveLoginError;
   import ru.pragmatix.wormix.model.rewards.*;
   import ru.pragmatix.wormix.objects.Direction;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import ru.pragmatix.wormix.utils.§_-c1S§;
   import ru.pragmatix.wormix.weapons.Cannon;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.Mine;
   import starling.display.Image;
   import starling.events.Event;
   
   public class §_-S1i§
   {
      
      private var frameCount:uint;
      
      private var §_-Wp§:uint;
      
      private var callback:Function;
      
      private var unit:§_-01J§;
      
      private var §_-121§:Boolean;
      
      private var arsenal:§_-A1l§;
      
      private var §_-ON§:Vector.<Weapon> = new Vector.<Weapon>();
      
      private var §_-V2Y§:Boolean;
      
      private var §_-y1c§:Object;
      
      private var §_-Z10§:uint;
      
      public function §_-S1i§(param1:§_-A1l§)
      {
         super();
         this.arsenal = param1;
      }
      
      public function processUnit(param1:§_-01J§) : void
      {
         var _loc3_:§_-O2H§ = null;
         var _loc4_:Weapon = null;
         var _loc5_:int = 0;
         this.unit = param1;
         this.§_-121§ = false;
         var _loc2_:Vector.<int> = this.arsenal.§_-O2u§(param1.record.getLevel());
         for each(_loc5_ in _loc2_)
         {
            _loc3_ = §_-G1p§.§_-I4§(_loc5_);
            if(_loc3_)
            {
               _loc4_ = _loc3_.getWeapon();
               if((_loc4_.§_-Y2U§ is §_-d1n§ && §_-d1n§(_loc4_.§_-Y2U§).check(param1) || _loc4_.§_-Y2U§ is §_-62p§ && §_-62p§(_loc4_.§_-Y2U§).check(param1,_loc4_)) && §_-X1j§.randomSeed.§_-Fh§(1,100) < this.arsenal.§_-B2§(_loc5_) * 100)
               {
                  this.§_-ON§[this.§_-ON§.length] = _loc4_;
               }
            }
         }
         this.§_-U1g§();
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.FRAME,this.onFrame);
      }
      
      private function §_-U1g§() : void
      {
         if(this.§_-ON§.length > 0)
         {
            this.§_-S15§(10,this.§_-P24§);
         }
         else
         {
            this.§_-S15§(60,this.§_-J2P§);
         }
      }
      
      private function §_-P24§() : void
      {
         var shotParams:§_-b2K§ = null;
         if(this.unit == null)
         {
            return;
         }
         if(this.unit.weapon != null)
         {
            this.unit.weapon.detach();
         }
         shotParams = this.§_-ON§[0].shotParams.clone();
         this.unit.weapon = this.§_-ON§[0];
         this.unit.weapon.attach(this.unit);
         this.§_-S15§(40,function():void
         {
            if(unit == null)
            {
               return;
            }
            unit.weapon.shootWithParams(shotParams);
            if(!(§_-ON§[0].§_-Y2U§ is §_-d1n§))
            {
               arsenal.§_-A1G§(§_-ON§[0].recordId.value);
            }
            §_-ON§.splice(0,1);
            §_-U1g§();
         });
      }
      
      public function onFrame(param1:Event = null) : void
      {
         if(this.unit == null)
         {
            §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.FRAME,this.onFrame);
            return;
         }
         var _temp_1:* = this;
         var _loc2_:§_-S1i§ = this;
         var _temp_2:* = _loc2_.frameCount;
         §§push(_temp_2);
         var _loc3_:Number = _temp_2 + 1;
         _loc2_.frameCount = _loc3_;
         if(§§pop() == this.§_-Wp§ && this.callback != null)
         {
            this.callback();
         }
      }
      
      public function §_-c25§() : void
      {
         var missFactor:Number;
         var testResult:Object = null;
         var level:uint = 0;
         var weapons:Vector.<int> = null;
         var weaponId:uint = 0;
         var fns:Array = null;
         var recId:int = 0;
         var finish:Function = null;
         var weaponById:§_-O2H§ = null;
         var weapon:Weapon = null;
         var fn:Function = null;
         if(this.unit == null)
         {
            return;
         }
         testResult = null;
         level = uint(this.unit.record.getLevel());
         missFactor = this.§_-Z1u§(level);
         if(!this.§_-121§)
         {
            this.§_-V2Y§ = true;
            if(this.unit.weapon != null)
            {
               this.unit.weapon.detach();
            }
            weapons = this.arsenal.§_-O2u§(level);
            weaponId = §_-q24§.RIFLE;
            this.unit.weapon = §_-G1p§.§_-I4§(weaponId).getWeapon();
            fns = [];
            for each(recId in weapons)
            {
               weaponById = §_-G1p§.§_-I4§(recId);
               if(weaponById)
               {
                  weapon = weaponById.getWeapon();
                  if(!(weapon.§_-Y2U§ is §_-d1n§))
                  {
                     fn = §_-bs§.§_-d1Z§(function(param1:int, param2:Weapon, param3:§_-O2H§, param4:Number):void
                     {
                        fns.splice(0,1);
                        var _loc5_:* = null;
                        if(Boolean(§_-X1j§.randomSeed.§_-y2j§() < arsenal.§_-B2§(param1) && unit && param2) && Boolean(param2.§_-Y2U§) && param2.§_-Y2U§ is §_-I1g§)
                        {
                           if(param2)
                           {
                              _loc5_ = §_-I1g§(param2.§_-Y2U§).test(unit,unit.x,unit.y,param4);
                           }
                        }
                        if(_loc5_ != null)
                        {
                           _loc5_.rate *= 1 + §_-X1j§.randomSeed.§_-y2j§() * 0.5;
                        }
                        if(_loc5_ != null && (testResult == null || _loc5_.rate > testResult.rate))
                        {
                           testResult = _loc5_;
                           weaponId = param1;
                           unit.weapon = param2;
                           §_-y1c§ = _loc5_;
                           §_-Z10§ = param1;
                        }
                        if(fns.length)
                        {
                           §_-Zd§.§_-33o§(1,fns[0]);
                        }
                     },this,[recId,weapon,weaponById,missFactor],true);
                     fns[fns.length] = fn;
                  }
               }
            }
            finish = §_-bs§.§_-d1Z§(function():void
            {
               if(unit)
               {
                  arsenal.§_-A1G§(§_-Z10§);
                  unit.weapon.attach(unit);
                  §_-121§ = true;
                  §_-V2Y§ = false;
                  §_-y1X§(getParams(§_-y1c§));
               }
            },this);
            fns[fns.length] = finish;
            fns[0]();
         }
         else if(this.unit.weapon)
         {
            testResult = §_-I1g§(this.unit.weapon.§_-Y2U§).test(this.unit,this.unit.x,this.unit.y,missFactor);
            this.§_-y1X§(this.getParams(testResult));
         }
      }
      
      private function §_-Z1u§(param1:uint) : Number
      {
         var _loc2_:Number = NaN;
         if(this.unit.aiMissFactor == -1)
         {
            _loc2_ = Math.log(param1) * Math.LOG2E;
            _loc2_ = 6 - (_loc2_ < 1 ? 1 : (_loc2_ > 5 ? 5 : _loc2_));
         }
         else
         {
            _loc2_ = this.unit.aiMissFactor;
         }
         return _loc2_;
      }
      
      private function getParams(param1:Object) : §_-b2K§
      {
         var _loc2_:Number = param1 ? Number(param1.angle) : -MathUtil.PI_HALF;
         var _loc3_:uint = Boolean(param1) && Boolean(param1.power) ? uint(param1.power) : 10;
         var _loc4_:Point = Boolean(param1) && Boolean(param1.point) ? param1.point : null;
         var _loc5_:§_-b2K§ = new §_-b2K§();
         _loc5_.angle = _loc2_;
         _loc5_.power = Weapon(this.unit.weapon).powerToCharge(_loc3_);
         _loc5_.point = _loc4_;
         if(Boolean(param1) && Boolean(param1.time))
         {
            _loc5_.timer = param1.time;
         }
         return _loc5_;
      }
      
      private function §_-y1X§(param1:§_-b2K§) : void
      {
         var params:§_-b2K§ = param1;
         new §_-H3h§().chain(§_-dG§.§_-s2s§,null,this.unit,params.angle).chain(function(param1:Function):void
         {
            if(Boolean(unit) && Boolean(unit.weapon))
            {
               Weapon(unit.weapon).shootWithParams(params);
               if(Boolean(unit) && Boolean(unit.weapon) && Weapon(unit.weapon).shotsLeft.value > 0)
               {
                  §_-S15§(30,§_-J2P§);
               }
               else
               {
                  §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.FRAME,onFrame);
                  param1();
               }
            }
         },this).chain(function():void
         {
            if(!(Boolean(unit) && Boolean(unit.§_-81o§) && unit.§_-81o§ is Cannon))
            {
               §_-Zd§.§_-33o§(60,§_-T2a§);
            }
         },this).run();
      }
      
      private function §_-J2P§() : void
      {
         var dx:int = 0;
         var dy:int = 0;
         var distSQ:Number = NaN;
         var leftX:int = 0;
         var rightX:int = 0;
         var minDist:int = 150;
         var enemy:§_-01J§ = §_-n2q§.§_-41S§(this.unit);
         if(enemy != null)
         {
            dx = enemy.x - this.unit.x;
            dy = enemy.y - this.unit.y;
            distSQ = dx * dx + dy * dy;
            if(distSQ < minDist * minDist)
            {
               leftX = §_-12u§.move(this.unit,Direction.LEFT.value).x;
               rightX = §_-12u§.move(this.unit,Direction.RIGHT.value).x;
               new §_-H3h§().chain(§_-dG§.§_-p2L§,null,this.unit,enemy.x - leftX > rightX - enemy.x ? leftX : rightX).chain(function():void
               {
                  §_-Zd§.§_-33o§(10,§_-c25§);
               },this).run();
            }
            else
            {
               §_-Zd§.§_-33o§(10,this.§_-c25§);
            }
         }
      }
      
      public function §_-T2a§() : void
      {
         var direct:int = 0;
         var pathX:int = 0;
         if(Boolean(this.unit) && Boolean(!this.unit.disposed) && !(this.unit.weapon && this.unit.weapon.recordId.value > 200))
         {
            direct = §_-X1j§.randomSeed.§_-y2j§() > 0.5 ? Direction.LEFT.value : Direction.RIGHT.value;
            pathX = §_-12u§.move(this.unit,direct).x;
            §_-dG§.§_-p2L§(function():void
            {
            },this.unit,pathX);
         }
      }
      
      public function §_-S15§(param1:uint, param2:Function) : void
      {
         this.frameCount = 0;
         this.§_-Wp§ = param1;
         this.callback = param2;
      }
      
      public function §_-32n§() : void
      {
         §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.FRAME,this.onFrame);
         this.unit = null;
      }
      
      public function dispose() : void
      {
         this.unit = null;
         this.arsenal = null;
         this.callback = null;
         this.§_-ON§ = null;
         this.§_-y1c§ = null;
      }
   }
}

