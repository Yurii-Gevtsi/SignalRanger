package ru.pragmatix.wormix.messages.server
{
   import §_-5Y§.ChatInputPanel;
   import §_-5Y§.StickerPanel;
   import §_-5Y§.§_-81g§;
   import §_-5Y§.§_-h2c§;
   import §_-5Y§.§_-t2O§;
   import §_-931§.§_-sz§;
   import §_-Af§.§_-k1I§;
   import §_-X14§.§_-J2b§;
   import §_-Y1O§.§_-V23§;
   import §_-Y1b§.§_-81l§;
   import §_-aM§.*;
   import §_-g2C§.§_-k1p§;
   import §_-g2C§.§_-p8§;
   import §_-j1w§.§_-L1x§;
   import §_-j22§.§_-5g§;
   import §_-l1g§.§_-L3Q§;
   import §_-l1g§.§_-Rs§;
   import §_-qH§.§_-S25§;
   import §_-s20§.§_-J2g§;
   import §_-u2J§.§_-T1K§;
   import com.hurlant.crypto.tls.§_-F16§;
   import com.hurlant.crypto.tls.§_-Tx§;
   import com.hurlant.crypto.tls.§_-a2D§;
   import feathers.controls.Label;
   import feathers.controls.LayoutGroup;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.HorizontalAlign;
   import feathers.layout.HorizontalLayout;
   import feathers.layout.HorizontalLayoutData;
   import feathers.layout.VerticalAlign;
   import feathers.layout.VerticalLayout;
   import feathers.layout.VerticalLayoutData;
   import feathers.themes.styles.AlternateTextStyles;
   import feathers.utils.textures.TextureCache;
   import flash.utils.ByteArray;
   import flash.utils.IDataOutput;
   import ru.pragmatix.flox.gui.controls.§_-NY§;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.messages.structures.EndBattleStructure;
   import ru.pragmatix.wormix.messages.structures.WeaponStructure;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.pvp.messages.ex.CallToBattle;
   import ru.pragmatix.wormix.serialization.SerializationUtils;
   import ru.pragmatix.wormix.utils.§_-g20§;
   import starling.core.Starling;
   import starling.display.Quad;
   
   public class UserIsBanned
   {
      
      public var reason:int;
      
      public var endDate:uint;
      
      public var profileId:uint;
      
      public function UserIsBanned()
      {
         super();
      }
      
      public function toString() : String
      {
         return "UserIsBanned{reason=" + String(this.reason) + ",endDate=" + String(this.endDate) + ",profileId=" + String(this.profileId) + "}";
      }
   }
}

