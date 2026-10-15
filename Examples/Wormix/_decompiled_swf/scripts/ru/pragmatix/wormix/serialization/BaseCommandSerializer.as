package ru.pragmatix.wormix.serialization
{
   import §_-63U§.§_-NV§;
   import §_-A1K§.§_-TA§;
   import §_-B1y§.§_-bs§;
   import §_-Bv§.§_-G3P§;
   import §_-TF§.§_-q1j§;
   import §_-fo§.*;
   import §_-hO§.§_-v24§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-n1w§.X509CertificateCollection;
   import §_-n1w§.§_-e2U§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-x2Q§.§_-dU§;
   import §_-zI§.§_-X1q§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.events.LoaderEvent;
   import com.greensock.loading.LoaderMax;
   import com.hurlant.crypto.tls.§_-21v§;
   import com.hurlant.crypto.tls.§_-F16§;
   import feathers.controls.Button;
   import feathers.controls.ScrollPolicy;
   import feathers.data.VectorCollection;
   import flash.events.*;
   import flash.system.ApplicationDomain;
   import flash.text.TextField;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.*;
   import ru.pragmatix.wormix.messages.structures.TemporalStuffStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import starling.display.DisplayObject;
   import starling.display.Image;
   import starling.display.Sprite;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.utils.Pool;
   import starling.utils.SystemUtil;
   
   public class BaseCommandSerializer implements §_-D2L§
   {
      
      public function BaseCommandSerializer()
      {
         super();
      }
      
      protected function §_-41X§(param1:Object) : ByteArray
      {
         return new ByteArray();
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
         var _loc3_:§_-5g§ = new §_-5g§();
         _loc3_.§_-c2D§(this.getCommandId());
         var _loc4_:ByteArray = this.§_-41X§(param1);
         _loc3_.setLength(_loc4_.length);
         _loc3_.write(param2);
         param2.writeBytes(_loc4_);
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         return null;
      }
      
      public function getCommandId() : uint
      {
         return 0;
      }
   }
}

