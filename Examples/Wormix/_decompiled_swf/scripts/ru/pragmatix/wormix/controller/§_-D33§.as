package ru.pragmatix.wormix.controller
{
   import §_-12a§.§_-41b§;
   import §_-63U§.ProgressBarBoss;
   import §_-A1K§.§_-6L§;
   import §_-A1K§.§_-U11§;
   import §_-DJ§.§_-fH§;
   import §_-H3§.*;
   import §_-Ly§.§_-81L§;
   import §_-SP§.§_-j1f§;
   import §_-Vg§.ClanCreateView;
   import §_-Y1Q§.§_-aG§;
   import §_-Y1Q§.§_-tc§;
   import §_-YF§.§_-q2v§;
   import §_-g2r§.§_-03A§;
   import §_-gG§.§_-M2S§;
   import §_-rM§.§_-61j§;
   import §_-s20§.§_-J2g§;
   import §_-sK§.*;
   import §_-z20§.§_-V7§;
   import §_-z20§.§_-p4§;
   import §_-zI§.§_-8D§;
   import §_-zI§.§_-F3q§;
   import com.distriqt.extension.inappbilling.Purchase;
   import feathers.controls.LayoutGroup;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.ILayoutDisplayObject;
   import feathers.layout.RelativePosition;
   import flash.display.DisplayObject;
   import flash.utils.ByteArray;
   import flash.utils.IDataInput;
   import ru.pragmatix.flox.gui.controls.§_-a1t§;
   import ru.pragmatix.flox.serialization.§_-616§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.gui.menu.shop.*;
   import ru.pragmatix.wormix.messages.client.EndTurn;
   import ru.pragmatix.wormix.messages.client.GoogleValidateInappPurchase;
   import ru.pragmatix.wormix.messages.client.StartBattle;
   import ru.pragmatix.wormix.messages.structures.ShopItemStructure;
   import ru.pragmatix.wormix.messages.structures.TurnStructure;
   import ru.pragmatix.wormix.model.bonus.§_-x2M§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import ru.pragmatix.wormix.utils.§_-YQ§;
   import starling.display.Sprite;
   import starling.events.Event;
   
   public class §_-D33§
   {
      
      private const §_-y2s§:uint = 4;
      
      private var §_-ot§:Boolean = false;
      
      public function §_-D33§()
      {
         super();
      }
      
      public function create() : void
      {
      }
      
      public function §_-Y1G§() : void
      {
         if(this.§_-ot§ && Application.userProfile.getLevel() >= this.§_-y2s§)
         {
         }
      }
   }
}

