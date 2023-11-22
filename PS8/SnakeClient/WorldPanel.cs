﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using IImage = Microsoft.Maui.Graphics.IImage;
#if MACCATALYST
using Microsoft.Maui.Graphics.Platform;
#else
using Microsoft.Maui.Graphics.Win2D;
#endif
using Color = Microsoft.Maui.Graphics.Color;
using System.Reflection;
using Microsoft.Maui;
using System.Net;
using Font = Microsoft.Maui.Graphics.Font;
using SizeF = Microsoft.Maui.Graphics.SizeF;
using WorldModel;
using Point = Microsoft.Maui.Graphics.Point;

namespace SnakeGame;
public class WorldPanel : IDrawable
{
    // A delegate for DrawObjectWithTransform
    // Methods matching this delegate can draw whatever they want onto the canvas
    public delegate void ObjectDrawer(object o, ICanvas canvas);

    private IImage wall;
    private IImage background;

    private bool initializedForDrawing = false;

    private IImage loadImage(string name)
    {
        Assembly assembly = GetType().GetTypeInfo().Assembly;
        string path = "SnakeClient.Resources.Images";
        using (Stream stream = assembly.GetManifestResourceStream($"{path}.{name}"))
        {
#if MACCATALYST
            return PlatformImage.FromStream(stream);
#else
            return new W2DImageLoadingService().FromStream(stream);
#endif
        }
    }

    private World theWorld;
    private GraphicsView graphicsView = new();
    private int viewSize = 900;

    public WorldPanel()
    {
        graphicsView.Drawable = this;
        
    }

    public void SetWorld(World w) {
        theWorld = w;
        
    }
    public void Invalidate() {
        graphicsView.Invalidate();
    }

    private void InitializeDrawing()
    {
        wall = loadImage( "wallsprite.png" );
        background = loadImage( "background.png" );
        initializedForDrawing = true;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if ( !initializedForDrawing )
            InitializeDrawing();

        // undo previous transformations from last frame
        canvas.ResetState();
        if (theWorld != null)
        {
            // First center the view on the player
            theWorld.Snakes.TryGetValue(theWorld.playerID, out Snake playerSnake);
            float playerX = (float)playerSnake.body[playerSnake.body.Count - 1].GetX();
            float playerY = (float)playerSnake.body[playerSnake.body.Count - 1].GetY();
            canvas.Translate(-playerX + (viewSize / 2), -playerY + (viewSize / 2));

            // Then draw the background first
            canvas.DrawImage(background, (-theWorld.Size / 2), (-theWorld.Size / 2), theWorld.Size, theWorld.Size);

            // Draw the objects
            lock (theWorld) {
                foreach (Wall w in theWorld.Walls.Values) {
                    DrawObjectWithTransform(canvas, w, w.p1.X, w.p1.Y, 0, WallDrawer);
                    DrawObjectWithTransform(canvas, w, w.p2.X, w.p2.Y, 0, WallDrawer);
                }

                foreach (Powerup p in theWorld.Powerups.Values) {
                    DrawObjectWithTransform(canvas, p, p.loc.GetX(), p.loc.GetY(), 0, PowerupDrawer); ;
                }

                foreach (Snake s in theWorld.Snakes.Values) {
                    if (s.snake == theWorld.playerID) {
                        canvas.FillColor = Colors.Blue;
                        canvas.FillRectangle((float)s.body[s.body.Count - 1].GetX(),
                                             (float)s.body[s.body.Count - 1].GetY(),
                                             50,
                                             50);
                    } else {
                        DrawObjectWithTransform(canvas,
                                                s,
                                                s.body[s.body.Count - 1].GetX(),
                                                s.body[s.body.Count - 1].GetY(),
                                                0,
                                                SnakeDrawer);

                    }
                }
            }
        }
    }

    // <summary>
    /// This method performs a translation and rotation to draw an object.
    /// </summary>
    /// <param name="canvas">The canvas object for drawing onto</param>
    /// <param name="o">The object to draw</param>
    /// <param name="worldX">The X component of the object's position in world space</param>
    /// <param name="worldY">The Y component of the object's position in world space</param>
    /// <param name="angle">The orientation of the object, measured in degrees clockwise from "up"</param>
    /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
    private void DrawObjectWithTransform(ICanvas canvas, object o, double worldX, double worldY, double angle, ObjectDrawer drawer) {
        // "push" the current transform
        canvas.SaveState();

        canvas.Translate((float)worldX, (float)worldY);
        canvas.Rotate((float)angle);
        drawer(o, canvas);

        // "pop" the transform
        canvas.RestoreState();
    }

    private void WallDrawer(object o, ICanvas canvas) {
        Wall w = o as Wall;
        canvas.DrawImage(wall,
                        (float)w.p1.X,
                        (float)w.p1.Y,
                        wall.Width,
                        wall.Height);
    }

    private void PowerupDrawer(object o, ICanvas canvas) {
        Powerup p = o as Powerup;
        int width = 10;
        canvas.FillColor = Colors.Green;

        if (!p.died) {
            canvas.FillRectangle(-(width / 2), -(width / 2), width, width);
        }
    }

    private void SnakeDrawer(object o, ICanvas canvas) {
        Snake s = o as Snake;

        canvas.FillColor = Colors.Blue;
        if (!s.died) {
            canvas.FillRectangle((float)s.body[s.body.Count - 1].GetX(),
                                 (float)s.body[s.body.Count - 1].GetY(),
                                 50,
                                 50);
        }
    }
}
