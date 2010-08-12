﻿using System.Collections.Generic;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.TestBed.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FarseerPhysics.TestBed.Tests
{
    public class YuPengPolygonTest : Test
    {
        private List<Vertices> _polygons;
        private List<TextMessage> _messages;
        private Vertices _subject;
        private Vertices _clip;
        private Vertices _selected;

        public override void Initialize()
        {
            Vector2 trans = new Vector2();
            _messages = new List<TextMessage>();
            _polygons = new List<Vertices>();

            _polygons.Add(PolygonTools.CreateGear(5f, 10, 0f, 6f));
            _polygons.Add(PolygonTools.CreateGear(4f, 15, 100f, 3f));

            trans.X = 0f;
            trans.Y = 8f;
            _polygons[0].Translate(ref trans);
            _polygons[1].Translate(ref trans);

            _polygons.Add(PolygonTools.CreateGear(5f, 10, 50f, 5f));
            
            trans.X = 22f;
            trans.Y = 17f;
            _polygons[2].Translate(ref trans);

            AddRectangle(5, 10);
            AddCircle(5, 32);

            trans.X = -20f;
            trans.Y = 8f;
            _polygons[3].Translate(ref trans);
            trans.Y = 20f;
            _polygons[4].Translate(ref trans);

            _subject = _polygons[0];
            _clip = _polygons[1];

            base.Initialize();
        }

        public override void Update(GameSettings settings, GameTime gameTime)
        {
            //If the message times out, remove it from the list.
            for (int i = _messages.Count - 1; i >= 0; i--) {
                _messages[i].ElapsedTime += settings.Hz;
                if (_messages[i].ElapsedTime > 5) {
                    _messages.Remove(_messages[i]);
                }
            }

            for (int i = 0; i < _polygons.Count; ++i) {
                if (_polygons[i] != null) {
                    Vector2[] array = _polygons[i].ToArray();
                    Color col = Color.SteelBlue;
                    if (!_polygons[i].IsCounterClockWise()) {
                        col = Color.Aquamarine;
                    }
                    if (_polygons[i] == _selected) { col = Color.LightBlue; }
                    if (_polygons[i] == _subject) {
                        col = Color.Green;
                        if (_polygons[i] == _selected) { col = Color.LightGreen; }
                    }
                    if (_polygons[i] == _clip) {
                        col = Color.DarkRed;
                        if (_polygons[i] == _selected) { col = Color.IndianRed; }
                    }
                    DebugView.DrawPolygon(ref array, _polygons[i].Count, col);
                    for (int j = 0; j < _polygons[i].Count; ++j) {
                        DebugView.DrawPoint(_polygons[i][j], .2f, Color.Red);
                    }
                }
            }

            DebugView.DrawString(500, TextLine, "A,S,D = Create Rectangle");
            TextLine += 15;

            DebugView.DrawString(500, TextLine, "Q,W,E = Create Circle");
            TextLine += 15;

            DebugView.DrawString(500, TextLine, "Click to Drag polygons");
            TextLine += 15;

            DebugView.DrawString(500, TextLine, "1 = Select Subject while dragging [green]");
            TextLine += 15;

            DebugView.DrawString(500, TextLine, "2 = Select Clip while dragging [red]");
            TextLine += 15;

            DebugView.DrawString(500, TextLine, "Space = Union");
            TextLine += 15;

            DebugView.DrawString(500, TextLine, "Backspace = Subtract");
            TextLine += 15;

            DebugView.DrawString(500, TextLine, "Shift = Intersection");
            TextLine += 15;
            TextLine += 15;

            DebugView.DrawString(500, TextLine, "Holes are colored light blue");
            TextLine += 15;

            DebugView.DrawString(500, TextLine, "#polygons: " + _polygons.Count);
            TextLine += 15;

            //DebugView.DrawString(50, TextLine, "Enter = Add to Simulation");
            //TextLine += 15;

            for (int i = _messages.Count - 1; i >= 0; i--) {
                DebugView.DrawString(50, TextLine, _messages[i].Text);
                TextLine += 15;
            }

            base.Update(settings, gameTime);
        }

        public override void Keyboard(KeyboardState state, KeyboardState oldState)
        {
            // Add Circles
            if (state.IsKeyDown(Keys.Q) && oldState.IsKeyUp(Keys.Q)) {
                AddCircle(3, 8);
            }

            // Add Circles
            if (state.IsKeyDown(Keys.W) && oldState.IsKeyUp(Keys.W)) {
                AddCircle(4, 16);
            }

            // Add Circles
            if (state.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E)) {
                AddCircle(5, 32);
            }

            // Add Rectangle
            if (state.IsKeyDown(Keys.A) && oldState.IsKeyUp(Keys.A)) {
                AddRectangle(4, 8);
            }

            // Add Rectangle
            if (state.IsKeyDown(Keys.S) && oldState.IsKeyUp(Keys.S)) {
                AddRectangle(5, 2);
            }

            // Add Rectangle
            if (state.IsKeyDown(Keys.D) && oldState.IsKeyUp(Keys.D)) {
                AddRectangle(2, 5);
            }

            // Perform a Union
            if (state.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space)) {
                if (_subject != null && _clip != null) {
                    DoBooleanOperation(PolyClipType.Union);
                }
            }

            // Perform a Subtraction
            if (state.IsKeyDown(Keys.Back) && oldState.IsKeyUp(Keys.Back)) {
                if (_subject != null && _clip != null) {
                    DoBooleanOperation(PolyClipType.Difference);
                }
            }

            // Perform a Intersection
            if (state.IsKeyDown(Keys.LeftShift) && oldState.IsKeyUp(Keys.LeftShift)) {
                if (_subject != null && _clip != null) {
                    DoBooleanOperation(PolyClipType.Intersection);
                }
            }

            // Select Subject
            if (state.IsKeyDown(Keys.D1) && oldState.IsKeyUp(Keys.D1)) {
                if (_selected != null) {
                    if (_clip == _selected) {
                        _clip = null;
                    }
                    _subject = _selected;
                }
            }

            // Select Clip
            if (state.IsKeyDown(Keys.D2) && oldState.IsKeyUp(Keys.D2)) {
                if (_selected != null) {
                    if (_subject == _selected) {
                        _subject = null;
                    }
                    _clip = _selected;
                }
            }

            // Add to Simulation
            /*if (state.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter)) {
                if (_left != null) {
                }
            }*/

            base.Keyboard(state, oldState);
        }

        public override void Mouse(MouseState state, MouseState oldState)
        {
            Vector2 position = GameInstance.ConvertScreenToWorld(state.X, state.Y);

            if (state.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released) {
                for (int i = 0; i < _polygons.Count; ++i) {
                    if (_polygons[i] != null) {
                        if (_polygons[i].PointInPolygon(position)) {
                            _selected = _polygons[i];
                            break;
                        }
                    }
                }
            }

            if (state.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed) {
                _selected = null;
            }

            MouseMove(state, oldState);
            base.Mouse(state, oldState);
        }

        private void MouseMove(MouseState state, MouseState oldState)
        {
            if (_selected != null) {
                Vector2 trans = new Vector2((state.X - oldState.X) / 12f,
                                            (oldState.Y - state.Y) / 12f);
                _selected.Translate(ref trans);
            }
        }

        private void DoBooleanOperation(PolyClipType pType)
        {
            // Do the union
            PolyClipError err;
            List<Vertices> result = YuPengClipper.Execute(_subject, _clip, pType, out err);
            _polygons.Remove(_subject);
            _polygons.Remove(_clip);
            _polygons.AddRange(result);
            _subject = null;
            _clip = null;
            _selected = null;
        }

        private void AddCircle(int radius, int numSides)
        {
            Vertices verts = PolygonTools.CreateCircle(radius, numSides);
            _polygons.Add(verts);
        }

        private void AddRectangle(int width, int height)
        {
            Vertices verts = PolygonTools.CreateRectangle(width, height);
            _polygons.Add(verts);
        }

        private void WriteMessage(string message)
        {
            _messages.Add(new TextMessage(message));
        }

        public static Test Create()
        {
            return new YuPengPolygonTest();
        }

        #region Nested type: TextMessage

        private class TextMessage
        {
            public float ElapsedTime;
            public string Text;

            public TextMessage(string text)
            {
                Text = text;
                ElapsedTime = 0;
            }
        }

        #endregion
    }
}