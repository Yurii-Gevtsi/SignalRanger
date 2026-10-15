package ru.pragmatix.wormix.utils
{
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-LE§;
   import §_-A1K§.§_-TA§;
   import §_-A1K§.§_-U11§;
   import §_-CF§.§_-E19§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-L1H§.§_-C3e§;
   import §_-Nq§.*;
   import §_-P1B§.§_-F1q§;
   import §_-TF§.§_-q1j§;
   import §_-TF§.§_-y2n§;
   import §_-Vg§.ClanEditView;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-b1F§.§_-G4§;
   import §_-k2s§.§_-8c§;
   import §_-k2s§.§_-C3A§;
   import §_-k2s§.§_-fG§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-sQ§.§_-H3D§;
   import §_-zI§.§_-F3q§;
   import config.§_-V2w§;
   import feathers.core.ITextRenderer;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import flash.geom.Rectangle;
   import flash.net.registerClassAlias;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.controls.sets.ControlSets;
   import ru.pragmatix.wormix.fonts.BitmapFonts;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.items.craft.§_-IE§;
   import ru.pragmatix.wormix.model.items.craft.§_-Y2u§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-8K§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.model.IPhysicModel;
   import ru.pragmatix.wormix.pvp.messages.server.PvpRetryCommandRequestServer;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.weapons.§_-7o§;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.utils.Color;
   import starling.utils.Pool;
   import starling.utils.RectangleUtil;
   import starling.utils.ScaleMode;
   
   public class §_-L2i§
   {
      
      public function §_-L2i§()
      {
         super();
      }
      
      private static function registerClass(param1:Class) : void
      {
         registerClassAlias("cloning." + String(param1),param1);
      }
      
      public static function clone(param1:Object) : *
      {
         var _loc2_:ByteArray = new ByteArray();
         _loc2_.writeObject(param1);
         _loc2_.position = 0;
         return _loc2_.readObject();
      }
      
      public static function §_-P2u§(param1:Object) : Object
      {
         var _loc3_:String = null;
         var _loc2_:Object = {};
         for(_loc3_ in param1)
         {
            _loc2_[_loc3_] = param1[_loc3_];
         }
         return _loc2_;
      }
      
      public static function §_-UU§(param1:Object) : void
      {
         var _loc2_:String = null;
         for(_loc2_ in param1)
         {
            if(param1[_loc2_] is §_-TA§)
            {
            }
         }
      }
      
      public static function §_-N1j§(param1:Object) : Object
      {
         var _loc3_:String = null;
         var _loc2_:Object = {};
         for(_loc3_ in param1)
         {
            if(param1[_loc3_] is §_-TA§)
            {
               _loc2_[_loc3_] = new §_-TA§(§_-TA§(param1[_loc3_]).value);
            }
            else if(param1[_loc3_] is §_-7u§)
            {
               _loc2_[_loc3_] = new §_-7u§(§_-7u§(param1[_loc3_]).value);
            }
            else if(param1[_loc3_] is §_-LE§)
            {
               _loc2_[_loc3_] = new §_-LE§(§_-LE§(param1[_loc3_]).value);
            }
            else
            {
               _loc2_[_loc3_] = param1[_loc3_];
            }
         }
         return _loc2_;
      }
      
      public static function §_-K15§(param1:Object, param2:Object) : Object
      {
         var _loc3_:String = null;
         for(_loc3_ in param2)
         {
            param1[_loc3_] = param2[_loc3_];
         }
         return param1;
      }
   }
}

