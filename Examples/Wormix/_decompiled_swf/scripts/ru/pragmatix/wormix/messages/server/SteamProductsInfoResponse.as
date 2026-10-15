package ru.pragmatix.wormix.messages.server
{
   import §_-51M§.Dragon;
   import §_-62§.§_-G1p§;
   import §_-62§.§_-W1r§;
   import §_-82a§.*;
   import §_-D3A§.§_-TZ§;
   import §_-OJ§.§_-sY§;
   import §_-T2h§.*;
   import §_-Y1b§.§_-OP§;
   import §_-YF§.§_-q2v§;
   import §_-d26§.§_-c2E§;
   import §_-d26§.§_-d1H§;
   import §_-k1u§.§_-Oi§;
   import §_-l1g§.§_-L3Q§;
   import §_-l2r§.§_-K1t§;
   import §_-r1C§.§_-U2p§;
   import §_-r1C§.§_-h2B§;
   import §_-w2i§.§_-Zd§;
   import dragonBones.animation.WorldClock;
   import feathers.data.ArrayCollection;
   import flash.events.Event;
   import flash.geom.Point;
   import flash.text.TextFormat;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.model.§_-4V§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.model.user.§_-r1d§;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.physics.CollideType;
   import ru.pragmatix.wormix.physics.Force;
   import ru.pragmatix.wormix.physics.model.PhysParams;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.utils.§_-71v§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.textures.TextureAtlas;
   import starling.utils.AssetManager;
   import starling.utils.Pool;
   
   public class SteamProductsInfoResponse
   {
      
      public var countryCode:String;
      
      public var currencyCode:String;
      
      public var products:Array;
      
      public function SteamProductsInfoResponse()
      {
         super();
      }
      
      public function toString() : String
      {
         return "SteamProductsInfoResponse{countryCode=" + String(this.countryCode) + ",currencyCode=" + String(this.currencyCode) + "}";
      }
   }
}

