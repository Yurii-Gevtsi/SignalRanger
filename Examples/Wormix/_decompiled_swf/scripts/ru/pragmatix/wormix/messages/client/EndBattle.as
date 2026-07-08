package ru.pragmatix.wormix.messages.client
{
   import §_-82a§.*;
   import §_-931§.§_-02O§;
   import §_-Y1Q§.§_-aG§;
   import §_-b1F§.§_-G4§;
   import §_-gG§.§_-Z2z§;
   import §_-r1C§.§_-h2B§;
   import §_-rM§.§_-61j§;
   import com.greensock.*;
   import com.greensock.core.*;
   import com.greensock.plugins.*;
   import feathers.controls.Button;
   import feathers.controls.ToggleButton;
   import flash.display.*;
   import flash.geom.ColorTransform;
   import flash.utils.IDataInput;
   import ru.pragmatix.wormix.messages.clans.structures.ClanAuditActionTO;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.scene.tile.AtlasTileData;
   import ru.pragmatix.wormix.weapons.ammo.DamageType;
   import starling.events.Event;
   import starling.textures.Texture;
   import starling.textures.TextureAtlas;
   import starling.utils.AssetManager;
   
   public class EndBattle
   {
      
      public static const RESULT_WINNER:int = 1;
      
      public static const RESULT_NOT_WINNER:int = -1;
      
      public static const RESULT_DRAW_GAME:int = 0;
      
      public static const RESULT_SURRENDERED:int = -4;
      
      public static const TYPE_MY_LEVEL:int = 0;
      
      public static const TYPE_HIGH_LEVEL:int = 1;
      
      public static const TYPE_LOW_LEVEL:int = 2;
      
      public static const TYPE_PVP_GAME:int = 3;
      
      public var result:uint;
      
      public var type:int;
      
      public var expBonus:int;
      
      public var battleId:int;
      
      public var missionId:int;
      
      public var items:Array = [];
      
      public var banType:uint = 0;
      
      public var banNote:String = "";
      
      public var sessionKey:String = "";
      
      public var collectedReagents:Vector.<int> = new Vector.<int>(0);
      
      public var fakeByteArraySize:int;
      
      public var totalTurnsCount:int;
      
      public var totalDamageToPlayer:int;
      
      public var totalDamageToBoss:int;
      
      public var totalUsedItems:Array = [];
      
      public function EndBattle(param1:int = 0, param2:int = 0)
      {
         super();
         this.result = param1;
         this.type = param2;
      }
      
      public function toString() : String
      {
         return "EndBattle{" + "result=" + this.result + ",battleId=" + this.battleId + ",missionId=" + this.missionId + ",banType=" + this.banType + ",banNote=" + this.banNote + ",totalTurnsCount=" + this.totalTurnsCount + ",totalDamageToPlayer=" + this.totalDamageToPlayer + ",totalDamageToBoss=" + this.totalDamageToBoss + ",sessionKey=" + this.sessionKey + ",type=" + this.type + ",expBonus=" + this.expBonus + ",items=" + this.items + ",collectedReagents=" + this.collectedReagents + ",totalUsedItems=" + this.totalUsedItems + "}";
      }
   }
}

