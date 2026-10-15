package ru.pragmatix.flox.social.model
{
   import §_-13R§.§_-i17§;
   import §_-62§.§_-F1D§;
   import §_-CR§.§_-72p§;
   import §_-T1t§.§_-u19§;
   import §_-X14§.*;
   import §_-jF§.FriendSocialITabItemRenderer;
   import §_-jF§.InviteFriendSocialITabItemRenderer;
   import §_-l1g§.§_-f2L§;
   import §_-qH§.§_-S25§;
   import §_-s1Q§.Artifacts;
   import §_-u13§.MD5;
   import §_-u13§.SHA1;
   import §_-z20§.*;
   import feathers.controls.Label;
   import feathers.controls.LayoutGroup;
   import feathers.controls.List;
   import feathers.controls.PullViewDisplayMode;
   import feathers.controls.ScrollPolicy;
   import feathers.controls.renderers.DefaultListItemRenderer;
   import feathers.data.ListCollection;
   import feathers.layout.AnchorLayout;
   import feathers.layout.AnchorLayoutData;
   import feathers.layout.RelativePosition;
   import feathers.layout.VerticalLayout;
   import feathers.themes.BackgroundColor;
   import feathers.themes.styles.AlternateScrollBarStyles;
   import feathers.themes.styles.AlternateTextStyles;
   import flash.utils.ByteArray;
   import ru.pragmatix.flox.social.controller.SocialApiSettings;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.gui.§_-s2a§;
   import ru.pragmatix.wormix.model.user.UserProfile;
   import ru.pragmatix.wormix.objects.§_-01J§;
   import ru.pragmatix.wormix.social.§_-N1V§;
   import starling.display.Quad;
   import starling.events.Event;
   
   public class ResponseFormat
   {
      
      public static var XML:ResponseFormat = new ResponseFormat("XML");
      
      public static var JSON:ResponseFormat = new ResponseFormat("JSON");
      
      public var format:String;
      
      public function ResponseFormat(param1:String)
      {
         super();
         this.format = param1;
      }
   }
}

