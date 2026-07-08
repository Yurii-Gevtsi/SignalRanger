package ru.pragmatix.wormix.serialization.structures
{
   import §_-82l§.§_-A2Y§;
   import §_-F39§.§_-a2W§;
   import §_-Y1b§.§_-OP§;
   import §_-YF§.§_-N3§;
   import §_-l1K§.*;
   import §_-rM§.§_-02j§;
   import com.amanitadesign.steam.SteamConstants;
   import com.amanitadesign.steam.SteamEvent;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import flash.geom.Point;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.social.utils.id.GetAuthKeyEvent;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.clans.chat.NotInClanChatScreen;
   import ru.pragmatix.wormix.gui.menu.clans.chat.§_-D2t§;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.*;
   import ru.pragmatix.wormix.physics.GameObjectUtils;
   import ru.pragmatix.wormix.physics.Physics;
   import ru.pragmatix.wormix.utils.StringUtil;
   import ru.pragmatix.wormix.weapons.Weapon;
   import ru.pragmatix.wormix.weapons.ammo.Ufo;
   import ru.pragmatix.wormix.weapons.ammo.type.blocker.HealBlocker;
   import starling.utils.Pool;
   
   public class ChatMessageBinarySerializer implements §_-02j§
   {
      
      public function ChatMessageBinarySerializer()
      {
         super();
      }
      
      public function serializeStructure(param1:Object, param2:IDataOutput, param3:Object) : void
      {
      }
      
      public function deserializeStructure(param1:IDataInput, param2:Object) : Object
      {
         var _loc3_:ByteArray = param1 as ByteArray;
         var _loc4_:ChatMessage = new ChatMessage();
         _loc4_.id = _loc3_.readUnsignedInt();
         _loc4_.action = §_-A2Y§.§_-Y1M§(_loc3_.readShort());
         _loc4_.logDate = _loc3_.readInt();
         _loc4_.profileId = _loc3_.readInt();
         _loc4_.platformId = _loc3_.readUTF();
         _loc4_.profileName = _loc3_.readUTF();
         _loc4_.message = _loc3_.readUTF();
         _loc4_.params = _loc3_.readUTF();
         return _loc4_;
      }
   }
}

