package ru.pragmatix.wormix.serialization.structures
{
   import §_-5Y§.*;
   import §_-A1K§.§_-7u§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-bs§;
   import §_-C3f§.§_-1Y§;
   import §_-DJ§.§_-J2l§;
   import §_-DJ§.§_-fH§;
   import §_-P1z§.*;
   import §_-Y1O§.§_-V23§;
   import §_-Y1O§.§_-y1o§;
   import §_-Y1b§.§_-81l§;
   import §_-Y1b§.§_-jH§;
   import §_-b1R§.§_-Bx§;
   import §_-j1w§.§_-L1x§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-02j§;
   import feathers.controls.NumericStepper;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalAlign;
   import feathers.utils.textures.TextureCache;
   import flash.geom.Point;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.messages.structures.SelectStuffStructure;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.utils.MathUtil;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class SelectStuffStructureBinarySerializer implements §_-02j§
   {
      
      public function SelectStuffStructureBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
         var _loc4_:SelectStuffStructure = SelectStuffStructure(param1);
         param2.writeInt(_loc4_.profileId);
         param2.writeShort(_loc4_.hatId);
         param2.writeShort(_loc4_.artifactId);
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         return null;
      }
   }
}

