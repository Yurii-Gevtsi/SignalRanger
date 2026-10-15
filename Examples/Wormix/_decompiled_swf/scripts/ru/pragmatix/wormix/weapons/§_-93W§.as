package ru.pragmatix.wormix.weapons
{
   import §_-62§.§_-G1p§;
   import §_-SP§.§_-M4§;
   import §_-YF§.§_-L29§;
   import §_-r1C§.§_-h2B§;
   import §_-w2W§.§_-21w§;
   import feathers.core.ITextRenderer;
   import ru.pragmatix.wormix.controller.§_-X1j§;
   import ru.pragmatix.wormix.fx.Effects;
   import ru.pragmatix.wormix.messages.structures.ChatMessage;
   import ru.pragmatix.wormix.model.§_-d27§;
   import ru.pragmatix.wormix.model.user.§_-33r§;
   import ru.pragmatix.wormix.objects.§_-F3o§;
   import ru.pragmatix.wormix.scene.SortingLayer;
   import starling.animation.IAnimatable;
   import starling.core.Starling;
   import starling.display.Sprite;
   
   public class §_-93W§ implements IAnimatable
   {
      
      private const §_-tv§:String = "999";
      
      private var value:Number;
      
      private var §_-u22§:int;
      
      private var §_-r1f§:uint;
      
      private var gobj:§_-F3o§;
      
      private var dx:int;
      
      private var dy:int;
      
      private var label:Sprite;
      
      private var textColor:String;
      
      private var textShadow:String;
      
      private var §_-D2C§:String;
      
      private var §_-U2H§:String;
      
      private var red:Boolean = false;
      
      private var §_-h2j§:Boolean = false;
      
      private var §_-u1R§:Boolean = false;
      
      public function §_-93W§(param1:uint, param2:String, param3:String, param4:int = -1, param5:String = null, param6:String = null)
      {
         super();
         this.value = param1;
         this.§_-r1f§ = param4 == -1 ? uint(param1 * 0.3) : uint(param4);
         this.§_-D2C§ = param5 == null ? param2 : param5;
         this.§_-U2H§ = param6 == null ? param3 : param6;
         this.textColor = param2;
         this.textShadow = param3;
      }
      
      public function §_-u2c§(param1:Boolean) : void
      {
         this.§_-u1R§ = param1;
      }
      
      public function tick(param1:Number) : void
      {
         if(this.§_-h2j§ || this.gobj == null)
         {
            return;
         }
         this.value -= param1;
         this.§_-uI§();
      }
      
      public function update(param1:uint) : void
      {
         if(this.§_-h2j§ || this.gobj == null)
         {
            return;
         }
         this.value = param1;
         this.§_-uI§();
      }
      
      private function §_-uI§() : void
      {
         var _loc1_:int = 0;
         if(this.value < 0)
         {
            this.detach();
         }
         else
         {
            if(!this.red && this.value < this.§_-r1f§)
            {
               this.§_-y2I§();
            }
            _loc1_ = Math.ceil(this.value);
            if(_loc1_ != this.§_-u22§)
            {
               this.§_-u22§ = _loc1_;
               if(this.§_-u1R§)
               {
                  §_-h2B§.play(§_-d27§.§_-27§);
               }
            }
            this.§_-n2v§(this.§_-u22§.toString());
         }
      }
      
      private function §_-y2I§() : void
      {
         this.createLabel(this.§_-D2C§,this.§_-U2H§);
         this.red = true;
      }
      
      private function §_-n2v§(param1:String) : void
      {
         var _loc2_:int = 0;
         while(_loc2_ < this.label.numChildren)
         {
            (this.label.getChildAt(_loc2_) as ITextRenderer).text = param1;
            _loc2_++;
         }
      }
      
      public function attach(param1:§_-F3o§, param2:int = 0, param3:int = -50) : void
      {
         this.gobj = param1;
         this.dx = param2;
         this.dy = param3;
         this.createLabel(this.textColor,this.textShadow);
         this.§_-n2v§(this.value.toString());
         this.§_-h2j§ = false;
         Starling.juggler.add(this);
      }
      
      private function createLabel(param1:String, param2:String) : void
      {
         if(this.label)
         {
            if(this.label.parent)
            {
               this.label.parent.removeChild(this.label);
            }
            this.label.dispose();
         }
         this.label = Effects.makeStarlingText(this.§_-tv§,param1);
         §_-X1j§.§_-12n§.scene.addElementToCertainSortingLayer(this.label,SortingLayer.TEXT_EFFECT_LAYER);
      }
      
      public function detach() : void
      {
         if(!this.§_-h2j§ && this.label != null)
         {
            Starling.juggler.remove(this);
            this.red = false;
            this.gobj = null;
            if(this.label)
            {
               if(this.label.parent)
               {
                  this.label.parent.removeChild(this.label);
               }
               this.label.dispose();
               this.label = null;
            }
            this.§_-h2j§ = true;
         }
      }
      
      public function isCompleted() : Boolean
      {
         return this.§_-h2j§;
      }
      
      public function advanceTime(param1:Number) : void
      {
         if(!this.§_-h2j§)
         {
            if(this.gobj.disposed)
            {
               this.detach();
            }
            else if(this.label)
            {
               this.label.x = this.gobj.clip.x + this.dx - this.label.width / 2;
               this.label.y = this.gobj.clip.y + this.dy;
            }
         }
      }
   }
}

