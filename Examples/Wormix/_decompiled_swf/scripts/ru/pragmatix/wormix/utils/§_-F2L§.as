package ru.pragmatix.wormix.utils
{
   import §_-31W§.§_-Kb§;
   import §_-A1K§.§_-TA§;
   import §_-N8§.*;
   import §_-P1z§.*;
   import §_-Y1O§.§_-42h§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-Y1Q§.§_-F1g§;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1b§.§_-81l§;
   import §_-dS§.§_-D3a§;
   import §_-j1w§.§_-L1x§;
   import §_-k1u§.§_-Oi§;
   import §_-r1C§.§_-h2B§;
   import §_-sQ§.§_-V1n§;
   import §_-x2Q§.§_-dU§;
   import com.distriqt.extension.inappbilling.PurchaseRequest;
   import feathers.controls.Button;
   import feathers.controls.NumericStepper;
   import feathers.layout.HorizontalLayout;
   import flash.display.DisplayObject;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.game.ai.§_-Q2U§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.NotInClanChatScreen;
   import ru.pragmatix.wormix.messages.clans.response.*;
   import ru.pragmatix.wormix.messages.clans.structures.ClanTO;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import starling.display.DisplayObject;
   import starling.events.Event;
   
   public class §_-F2L§
   {
      
      public function §_-F2L§()
      {
         super();
      }
      
      public static function §_-CT§(param1:Array, param2:§_-V1n§ = null) : Array
      {
         if(param2 == null)
         {
            param2 = new §_-V1n§(Math.random() * 1000);
         }
         var _loc3_:Array = param1.concat();
         var _loc4_:Array = [];
         while(_loc3_.length > 0)
         {
            _loc4_.push(_loc3_.splice(param2.§_-Fh§(0,_loc3_.length - 1),1)[0]);
         }
         return _loc4_;
      }
      
      public static function §_-C2T§(param1:Array, param2:§_-V1n§ = null) : *
      {
         var _loc3_:Number = NaN;
         if(param1.length > 0)
         {
            if(param2)
            {
               return param1[param2.§_-Fh§(0,param1.length - 1)];
            }
            return param1[Math.floor(Math.random() * param1.length)];
         }
         return null;
      }
      
      public static function §_-n2u§(param1:Object) : Array
      {
         var _loc3_:Object = null;
         var _loc2_:Array = [];
         for each(_loc3_ in param1)
         {
            _loc2_.push(_loc3_);
         }
         return _loc2_;
      }
      
      public static function §_-11m§(param1:Object, param2:Function, ... rest) : void
      {
         var _loc5_:Boolean = false;
         var _loc4_:int = int(param1.length);
         while(--_loc4_ > -1)
         {
            if(rest)
            {
               _loc5_ = param2.apply(null,[param1[_loc4_]].concat(rest));
            }
            else
            {
               _loc5_ = param2(param1[_loc4_]);
            }
            if(_loc5_)
            {
               break;
            }
         }
      }
      
      public static function §_-13w§(param1:Vector.<§_-01J§>, param2:Function, param3:Boolean = false) : void
      {
         §_-F2L§.§_-11m§(param1,processUnit,param2,param3);
      }
      
      private static function processUnit(param1:§_-01J§, param2:Function, param3:Boolean = false) : Boolean
      {
         if(!param1.disposed || param3)
         {
            return param2(param1);
         }
         return false;
      }
      
      public static function §_-x1v§(param1:Object, param2:String, param3:Object, param4:Function) : void
      {
         var _loc6_:Object = null;
         var _loc5_:int = int(param1.length);
         while(--_loc5_ > -1)
         {
            _loc6_ = param1[_loc5_];
            if(_loc6_[param2] != null && _loc6_[param2] == param3)
            {
               if(param4(_loc6_))
               {
                  break;
               }
            }
         }
      }
      
      public static function clone(param1:Object) : *
      {
         var _loc2_:ByteArray = new ByteArray();
         _loc2_.writeObject(param1);
         _loc2_.position = 0;
         return _loc2_.readObject();
      }
   }
}

