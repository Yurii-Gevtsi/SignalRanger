package ru.pragmatix.wormix.controls
{
   public class Command
   {
      
      private static const commands:Object = {};
      
      public static const Left:Command = new Command("Left");
      
      public static const Right:Command = new Command("Right");
      
      public static const Up:Command = new Command("Up");
      
      public static const Down:Command = new Command("Down");
      
      public static const Jump:Command = new Command("Jump");
      
      public static const BackJump:Command = new Command("BackJump");
      
      public static const Shoot:Command = new Command("Shoot");
      
      public static const Bag:Command = new Command("Bag");
      
      public static const Escape:Command = new Command("Escape");
      
      public static const Use:Command = new Command("Use");
      
      public static const Alt:Command = new Command("Alt");
      
      public static const Shift:Command = new Command("Shift");
      
      public static const Alt_Shift:Command = new Command("Alt_Shift");
      
      public static const all:Array = [Left,Right,Up,Down,Jump,BackJump,Shoot,Alt,Shift,Alt_Shift,Bag,Escape,Use];
      
      public var name:String;
      
      public var state:CommandState;
      
      public function Command(param1:String)
      {
         super();
         this.name = param1;
         commands[param1] = this;
      }
      
      public static function getByName(param1:String) : Command
      {
         return commands[param1] as Command;
      }
      
      public function withState(param1:CommandState) : Command
      {
         this.state = param1;
         return this;
      }
   }
}

