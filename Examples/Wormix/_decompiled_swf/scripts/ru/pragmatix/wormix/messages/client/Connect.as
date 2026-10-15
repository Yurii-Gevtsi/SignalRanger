package ru.pragmatix.wormix.messages.client
{
   import §_-03T§.§_-2p§;
   import §_-82a§.*;
   import §_-F2M§.*;
   import §_-YF§.§_-q2v§;
   import §_-aM§.*;
   import §_-b1F§.§_-G4§;
   import §_-r1C§.§_-h2B§;
   import feathers.controls.Button;
   import flash.display.BitmapData;
   import flash.geom.Point;
   import flash.net.Socket;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.EffectType;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.clans.request.edit.ChangeClanDescriptionRequest;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import ru.pragmatix.wormix.weapons.*;
   import starling.display.DisplayObjectContainer;
   
   public class Connect
   {
      
      public var socialProfileId:String;
      
      public var authKey:String;
      
      public var ids:Array;
      
      public var socialNetId:uint;
      
      public var isSecure:Boolean;
      
      public function Connect()
      {
         super();
      }
      
      public function toString() : String
      {
         return "Connect{" + "socialProfileId=" + this.socialProfileId + ",socialNetId=" + this.socialNetId + ",authKey=" + this.authKey + ",ids=" + this.ids + "}";
      }
   }
}

