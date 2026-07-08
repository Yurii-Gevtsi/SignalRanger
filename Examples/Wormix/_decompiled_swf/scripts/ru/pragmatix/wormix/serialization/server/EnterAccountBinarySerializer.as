package ru.pragmatix.wormix.serialization.server
{
   import §_-5Y§.ChatInputPanel;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-Bv§.§_-G3P§;
   import §_-Vg§.*;
   import §_-n1w§.*;
   import §_-rM§.§_-02j§;
   import §_-rM§.§_-61j§;
   import §_-rM§.§_-D2L§;
   import §_-u13§.MD2;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-u13§.§_-F3§;
   import com.hurlant.util.der.Sequence;
   import com.hurlant.util.der.§_-cz§;
   import com.hurlant.util.der.§_-l1b§;
   import feathers.controls.text.TextFormatUtils;
   import feathers.layout.AnchorLayout;
   import feathers.layout.VerticalAlign;
   import feathers.themes.FontColor;
   import feathers.themes.FontName;
   import feathers.themes.FontSize;
   import flash.text.TextFormatAlign;
   import flash.utils.ByteArray;
   import flash.utils.Dictionary;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.messages.clans.main.server.ClanInviteStructure;
   import ru.pragmatix.wormix.messages.server.EnterAccount;
   import ru.pragmatix.wormix.messages.structures.AdvertisingPurchaseStructure;
   import ru.pragmatix.wormix.messages.structures.DepositStructure;
   import ru.pragmatix.wormix.messages.structures.LoginAwardStructure;
   import ru.pragmatix.wormix.messages.structures.ReconnectToSimpleBattleResultStructure;
   import ru.pragmatix.wormix.messages.structures.RestrictionItemStructure;
   import ru.pragmatix.wormix.messages.structures.SocialLinkInfoStructure;
   import ru.pragmatix.wormix.messages.structures.UserProfileStructure;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import starling.events.Event;
   
   public class EnterAccountBinarySerializer implements §_-D2L§
   {
      
      public function EnterAccountBinarySerializer()
      {
         super();
      }
      
      public function getCommandId() : uint
      {
         return 10001;
      }
      
      public function serializeCommand(param1:Object, param2:IDataOutput) : void
      {
      }
      
      public function deserializeCommand(param1:IDataInput, param2:§_-61j§) : Object
      {
         var _loc6_:String = null;
         var _loc7_:String = null;
         var _loc8_:int = 0;
         var _loc9_:§_-02j§ = null;
         var _loc3_:EnterAccount = new EnterAccount();
         var _loc4_:ByteArray = new ByteArray();
         param1.readBytes(_loc4_,0,param1.bytesAvailable);
         _loc3_.userProfileStructure = UserProfileStructure(SerializationUtils.readStructure(_loc4_,UserProfileStructure));
         _loc3_.userProfileStructures = SerializationUtils.readStructureArray(_loc4_,UserProfileStructure);
         _loc3_.loginAwards = SerializationUtils.readStructureArray(_loc4_,LoginAwardStructure);
         _loc3_.onlineFriends = _loc4_.readShort();
         _loc3_.friends = _loc4_.readShort();
         _loc3_.sessionKey = _loc4_.readUTF();
         _loc3_.availableSearchKeys = _loc4_.readByte();
         _loc3_.reagents = SerializationUtils.readIntVector(_loc4_);
         _loc3_.curSoloMissionId = _loc4_.readShort();
         _loc3_.curCooperativeMissionId = _loc4_.readShort();
         _loc3_.invites = Vector.<ClanInviteStructure>(SerializationUtils.readStructureVector(_loc4_,ClanInviteStructure));
         _loc3_.renameAct = _loc4_.readByte();
         _loc3_.serverTime = _loc4_.readInt();
         _loc3_.backpackConfig1 = SerializationUtils.readShortArray(_loc4_);
         _loc3_.backpackConfig2 = SerializationUtils.readShortArray(_loc4_);
         _loc3_.backpackConfig3 = SerializationUtils.readShortArray(_loc4_);
         _loc3_.activeBackpackConfig = _loc4_.readByte();
         _loc3_.hotkeys = SerializationUtils.readShortArray(_loc4_);
         _loc3_.loginSequence = _loc4_.readByte();
         _loc3_.races = _loc4_.readShort();
         _loc3_.selectRaceTime = _loc4_.readInt();
         _loc3_.depositStructures = SerializationUtils.readStructureArray(_loc4_,DepositStructure);
         _loc3_.skins = SerializationUtils.readByteArray(_loc4_);
         _loc3_.lastPaymentTime = _loc4_.readInt();
         _loc3_.restrictions = SerializationUtils.readStructureArray(_loc4_,RestrictionItemStructure);
         _loc3_.cookies = new Dictionary();
         var _loc5_:int = int(_loc4_.readUnsignedShort());
         if(_loc5_)
         {
            _loc5_ /= 2;
            _loc8_ = 0;
            while(_loc8_ < _loc5_)
            {
               _loc6_ = _loc4_.readUTF();
               _loc7_ = _loc4_.readUTF();
               _loc3_.cookies[_loc6_] = _loc7_;
               _loc8_++;
            }
         }
         _loc3_.vipSubscriptionId = _loc4_.readInt();
         if(_loc4_.readShort() == 1)
         {
            _loc9_ = §_-616§.§_-fk§(ReconnectToSimpleBattleResultStructure);
            _loc3_.reconnectResult = ReconnectToSimpleBattleResultStructure(_loc9_.deserializeStructure(_loc4_,null));
         }
         _loc3_.socialLinks = SerializationUtils.readStructureArray(_loc4_,SocialLinkInfoStructure);
         _loc3_.ads = SerializationUtils.readStructureArray(_loc4_,AdvertisingPurchaseStructure);
         _loc3_.isSecure = §_-U11§.isSecure(_loc4_,§_-6L§.§_-95§);
         return _loc3_;
      }
   }
}

