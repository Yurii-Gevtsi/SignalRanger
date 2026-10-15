package ru.pragmatix.wormix.serialization.structures
{
   import §_-73I§.§_-Y1l§;
   import §_-A1K§.§_-TA§;
   import §_-CF§.§_-E19§;
   import §_-rM§.§_-02j§;
   import com.distriqt.extension.inappbilling.InAppBillingAvailability;
   import com.distriqt.extension.inappbilling.events.AvailabilityEvent;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import org.gestouch.core.gestouch_internal;
   import ru.pragmatix.flox.logger.§_-11S§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.messages.structures.SelectStuffResultStructure;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.weapons.*;
   import starling.core.Starling;
   import starling.display.DisplayObject;
   
   use namespace gestouch_internal;
   
   public class SelectStuffResultStructureBinarySerializer implements §_-02j§
   {
      
      public function SelectStuffResultStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:SelectStuffResultStructure = new SelectStuffResultStructure();
         _loc3_.profileId = param1.readInt();
         _loc3_.resultHat = param1.readShort();
         _loc3_.hatId = param1.readShort();
         _loc3_.resultArtifact = param1.readShort();
         _loc3_.artifactId = param1.readShort();
         return _loc3_;
      }
   }
}

