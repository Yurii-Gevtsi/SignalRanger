package ru.pragmatix.wormix.objects
{
   import §_-43V§.Random;
   import §_-L1H§.§_-b2K§;
   import §_-SP§.§_-M4§;
   import §_-U1i§.§_-J1§;
   import §_-Vg§.*;
   import §_-l1g§.§_-L3Q§;
   import com.hurlant.crypto.tls.*;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import feathers.data.ListCollection;
   import feathers.layout.AnchorLayoutData;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.physics.model.CatPhysicModel;
   import starling.events.Event;
   
   public class Waypoint extends §_-F3o§
   {
      
      public var radius:uint;
      
      private var target:§_-01J§;
      
      public var callback:Function;
      
      public var §_-V1D§:Boolean;
      
      public function Waypoint(param1:int, param2:int, param3:uint, param4:Boolean)
      {
         super(true);
         this.x = param1;
         this.y = param2;
         this.radius = param3;
         this.§_-V1D§ = param4;
         this.stable = true;
         §_-X1j§.§_-12n§.scene.gameObjectProcessor.§_-L2X§(this);
         §_-X1j§.§_-12n§.scene.addEventListener(§_-L3Q§.FRAME,this.onFrame);
      }
      
      public static function §_-22P§(param1:int, param2:int, param3:uint, param4:§_-01J§, param5:Function, param6:Boolean = true) : void
      {
         new Waypoint(param1,param2,param3,param6).§_-N2f§(param4,param5);
      }
      
      public function §_-N2f§(param1:§_-01J§, param2:Function) : void
      {
         this.target = param1;
         this.callback = param2;
      }
      
      public function onFrame(param1:Event) : void
      {
         var _loc2_:§_-01J§ = null;
         if(this.target != null && this.callback != null)
         {
            for each(_loc2_ in Physics.getUnitsInCircle(x,y,this.radius,null,this.§_-V1D§))
            {
               if(_loc2_ == this.target)
               {
                  disposed = true;
                  this.callback();
                  break;
               }
            }
         }
      }
      
      override public function defaultOnDisposed(param1:Event = null) : void
      {
         super.defaultOnDisposed(param1);
         this.target = null;
         this.callback = null;
         §_-X1j§.§_-12n§.scene.removeEventListener(§_-L3Q§.FRAME,this.onFrame);
      }
   }
}

