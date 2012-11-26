﻿#region Using System
using System;
using System.Text;
#endregion
#region Using XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion
#region Using Farseer
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Samples.MediaSystem;
using FarseerPhysics.Samples.Demos.Prefabs;
using FarseerPhysics.Samples.ScreenSystem;
#endregion

namespace FarseerPhysics.Samples.Demos
{
  internal class Demo7 : PhysicsGameScreen
  {
    private Border _border;
    private Sprite _obstacle;
    private Body[] _obstacles = new Body[4];
    private Ragdoll _ragdoll;

    #region Demo description

    public override string GetTitle()
    {
      return "Ragdoll";
    }

    public override string GetDetails()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine("This demo shows how to combine physics objects to create a ragdoll.");
      sb.AppendLine(string.Empty);
      sb.AppendLine("GamePad:");
      sb.AppendLine("  - Rotate ragdoll: left and right triggers");
      sb.AppendLine("  - Move ragdoll: right thumbstick");
      sb.AppendLine("  - Move cursor: left thumbstick");
      sb.AppendLine("  - Grab object (beneath cursor): A button");
      sb.AppendLine("  - Drag grabbed object: left thumbstick");
      sb.AppendLine("  - Exit to menu: Back button");
      sb.AppendLine(string.Empty);
      sb.AppendLine("Keyboard:");
      sb.AppendLine("  - Rotate ragdoll: left and right arrows");
      sb.AppendLine("  - Move ragdoll: A,S,D,W");
      sb.AppendLine("  - Exit to menu: Escape");
      sb.AppendLine(string.Empty);
      sb.AppendLine("Mouse / Touchscreen");
      sb.AppendLine("  - Grab object (beneath cursor): Left click");
      sb.AppendLine("  - Drag grabbed object: move mouse / finger");
      return sb.ToString();
    }

    public override int GetIndex()
    {
      return 7;
    }

    #endregion

    public override void LoadContent()
    {
      base.LoadContent();

      World.Gravity = new Vector2(0f, 20f);

      _border = new Border(World, Lines, Framework.GraphicsDevice);

      _ragdoll = new Ragdoll(World, Vector2.Zero);

      for (int i = 0; i < 4; i++)
      {
        _obstacles[i] = BodyFactory.CreateRectangle(World, 5f, 1.5f, 1f);
        _obstacles[i].IsStatic = true;
      }

      _obstacles[0].Position = new Vector2(-9f, 5f);
      _obstacles[1].Position = new Vector2(-8f, -7f);
      _obstacles[2].Position = new Vector2(9f, 7f);
      _obstacles[3].Position = new Vector2(7f, -5f);

      // create sprite based on body
      _obstacle = new Sprite(AssetCreator.TextureFromShape(_obstacles[0].FixtureList[0].Shape, "stripe", AssetCreator.Green, AssetCreator.Teal, AssetCreator.Black, 1.5f));

      SetUserAgent(_ragdoll.Body, 1000f, 400f);
    }

    public override void Draw(GameTime gameTime)
    {
      Sprites.Begin(0, null, null, null, null, null, Camera.View);
      for (int i = 0; i < 4; i++)
      {
        Sprites.Draw(_obstacle.Image, ConvertUnits.ToDisplayUnits(_obstacles[i].Position),
                     null, Color.White, _obstacles[i].Rotation, _obstacle.Origin, 1f, SpriteEffects.None, 0f);
      }
      _ragdoll.Draw(Sprites);
      Sprites.End();

      _border.Draw(Camera.SimProjection, Camera.SimView);

      base.Draw(gameTime);
    }
  }
}