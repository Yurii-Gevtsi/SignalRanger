package ru.pragmatix.wormix.serialization.structures
{
   import §_-31N§.§_-42G§;
   import §_-62§.§_-W1r§;
   import §_-B1y§.§_-bs§;
   import §_-D3A§.§_-TZ§;
   import §_-hO§.§_-v24§;
   import §_-l1g§.§_-L3Q§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-02j§;
   import §_-w2i§.§_-Zd§;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderMax;
   import com.greensock.loading.LoaderStatus;
   import com.greensock.loading.core.*;
   import com.hurlant.crypto.tls.*;
   import dragonBones.animation.WorldClock;
   import flash.events.Event;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import flash.utils.getTimer;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.display.Image;
   import starling.events.Event;
   import starling.events.EventDispatcher;
   import starling.textures.Texture;
   import starling.utils.Align;
   
   public class HeroicMissionStructureBinarySerializer implements §_-02j§
   {
      
      public function HeroicMissionStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ByteArray = param1 as ByteArray;
         var _loc4_:HeroicMissionStructure = new HeroicMissionStructure();
         SerializationUtils.readShortVector(_loc3_,_loc4_.bossIds);
         _loc4_.mapId = _loc3_.readUnsignedInt();
         return _loc4_;
      }
   }
}

