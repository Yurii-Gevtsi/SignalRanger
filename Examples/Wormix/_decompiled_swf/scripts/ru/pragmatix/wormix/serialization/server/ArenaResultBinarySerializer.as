package ru.pragmatix.wormix.serialization.server
{
   import §_-134§.§_-c11§;
   import §_-5Y§.ChatInputPanel;
   import §_-5Y§.StickerPanel;
   import §_-5Y§.§_-81g§;
   import §_-5Y§.§_-h2c§;
   import §_-5Y§.§_-t2O§;
   import §_-63U§.§_-21K§;
   import §_-A1K§.§_-TA§;
   import §_-l1K§.§_-W2f§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import feathers.layout.VerticalLayout;
   import feathers.layout.VerticalLayoutData;
   import feathers.utils.textures.TextureCache;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.game.§_-eX§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.ClanInfoPanel;
   import ru.pragmatix.wormix.messages.server.ArenaResult;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionDailyProgressStructure;
   import ru.pragmatix.wormix.messages.structures.HeroicMissionStructure;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class ArenaResultBinarySerializer implements §_-D2L§
   {
      
      public function ArenaResultBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10004;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc3_:ArenaResult = null;
         _loc3_ = new ArenaResult();
         _loc3_.battlesCount = param1.readInt();
         _loc3_.curSoloMissionId = param1.readShort();
         _loc3_.curCooperativeMissionId = param1.readShort();
         _loc3_.bossAvailable = param1.readByte() != 0;
         _loc3_.superBossAvailable = param1.readByte() != 0;
         _loc3_.heroicStructures = Vector.<HeroicMissionStructure>(SerializationUtils.readStructureVector(param1 as ByteArray,HeroicMissionStructure));
         _loc3_.delay = param1.readInt();
         _loc3_.restoreBattlesDelay = param1.readInt();
         _loc3_.restrictedWagers = SerializationUtils.readShortArray(param1 as ByteArray);
         _loc3_.restrictedWagersLeftTime = SerializationUtils.readIntArray(param1 as ByteArray);
         _loc3_.heroicMissionDailyProgresses = SerializationUtils.readStructureArray(param1 as ByteArray,HeroicMissionDailyProgressStructure);
         _loc3_.defeatContributionMoney = param1.readInt();
         _loc3_.extraBattlesTimetable = param1.readUTF();
         _loc3_.wagerWinAwardToken = param1.readInt();
         _loc3_.bossWinAwardToken = param1.readInt();
         _loc3_.advPurchases = SerializationUtils.readStructureArray(param1 as ByteArray,AdvertisingPurchaseStructure);
         return _loc3_;
      }
   }
}

