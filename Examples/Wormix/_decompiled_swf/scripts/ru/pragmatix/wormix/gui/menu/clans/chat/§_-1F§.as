package ru.pragmatix.wormix.gui.menu.clans.chat
{
   import §_-A2U§.§_-A23§;
   import §_-X14§.*;
   import §_-x2Q§.§_-dU§;
   import com.amanitadesign.steam.SteamConstants;
   import com.amanitadesign.steam.SteamEvent;
   import feathers.controls.ScreenNavigator;
   import feathers.controls.ScreenNavigatorItem;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.social.utils.id.GetAuthKeyEvent;
   import ru.pragmatix.wormix.objects.§_-01J§;
   
   public class §_-1F§ extends §_-jO§
   {
      
      public const NOT_IN_CLAN:String = "Not in clan screen";
      
      public const CLAN_CHAT:String = "Clan chat screen";
      
      private var §_-Xe§:ScreenNavigator;
      
      public function §_-1F§()
      {
         super();
      }
      
      override protected function initialize() : void
      {
         super.initialize();
         this.§_-Xe§ = new ScreenNavigator();
         this.§_-Xe§.addScreen(this.NOT_IN_CLAN,new ScreenNavigatorItem(NotInClanChatScreen));
         this.§_-Xe§.addScreen(this.CLAN_CHAT,new ScreenNavigatorItem(§_-D2t§));
         addChild(this.§_-Xe§);
         this.§_-y20§();
      }
      
      override protected function draw() : void
      {
         super.draw();
         if(isInvalid(INVALIDATION_FLAG_SIZE) && Boolean(_owner))
         {
            this.§_-Xe§.width = _owner.width;
            this.§_-Xe§.height = _owner.height;
         }
         else if(isInvalid(§_-dU§.§_-H1y§))
         {
            if(this.§_-R2T§() != this.§_-Xe§.activeScreenID)
            {
               this.§_-y20§();
            }
         }
      }
      
      private function §_-y20§() : void
      {
         this.§_-Xe§.showScreen(this.§_-R2T§());
      }
      
      private function §_-R2T§() : String
      {
         if(§_-A23§.userClan)
         {
            return this.CLAN_CHAT;
         }
         return this.NOT_IN_CLAN;
      }
      
      override public function dispose() : void
      {
         this.§_-Xe§ = null;
         super.dispose();
      }
   }
}

