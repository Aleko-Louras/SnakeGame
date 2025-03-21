﻿///
/// WorldPanel handles moving information between the Server and the
/// Game world.
///
/// Aleko Louras and Quinn Pritchett
/// November 2023
///
///


using System.Collections.Generic;
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
using static ObjCRuntime.Dlfcn;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Timers;
using Microsoft.Maui.Graphics;

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

    /// <summary>
    /// The primary Draw method for theWorld objects
    /// Called each time there are new updates from the server
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="dirtyRect"></param>
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if ( !initializedForDrawing )
            InitializeDrawing();

        // undo previous transformations from last frame
        canvas.ResetState();

        if (theWorld != null  )
        {
            // Draw the objects
            lock (theWorld) {

                // First center the view on the player
                theWorld.Snakes.TryGetValue(theWorld.playerID, out Snake playerSnake);
                if (playerSnake != null) {

                    float playerX = (float)playerSnake.body[playerSnake.body.Count - 1].GetX();
                    float playerY = (float)playerSnake.body[playerSnake.body.Count - 1].GetY();
                    canvas.Translate(-playerX + (viewSize / 2), -playerY + (viewSize / 2));
                }

                // Then draw the background first
                canvas.DrawImage(background, (-theWorld.Size / 2), (-theWorld.Size / 2), theWorld.Size, theWorld.Size);

                foreach (Wall w in theWorld.Walls.Values)
                {
                    double wallp1X = w.p1.GetX();
                    double wallp1Y = w.p1.GetY();

                    double wallp2X = w.p2.GetX();
                    double wallp2Y = w.p2.GetY();

                    // Vertical Wall Case
                    if (wallp1X == wallp2X)
                    {
                       

                        // First coordinate above second, draw going down y axis, with p1 on top
                        if (wallp1Y > wallp2Y)
                        {
                            double distance = wallp1Y - wallp2Y;
                            double numberOfWalls = distance / 50;
                            for (int i = 0; i <= numberOfWalls; i++)
                            {
                                DrawObjectWithTransform(canvas, wall, wallp1X, wallp1Y - (i * 50), 0, WallDrawer);
                            }
                        } else // Second coordinate abvove second, draw going down y axis with p2 on top
                        {
                            double distance = wallp2Y - wallp1Y;
                            double numberOfWalls = distance / 50;

                            for (int i = 0; i <= numberOfWalls; i++)
                            {
                                DrawObjectWithTransform(canvas, wall, wallp2X, wallp2Y - (i * 50), 0, WallDrawer);
                            }
                        }

                    }

                    // Horiontal Wall Case, the Y coords are equal compare the X
                    else if (wallp1Y == wallp2Y)
                    {

                        // First coordinate before second coordinate
                        if (wallp2X > wallp1X)
                        {
                            double distance = wallp2X - wallp1X;
                            double numberOfWalls = distance / 50;

                            for (int i = 0; i <= numberOfWalls; i++)
                            {
                                DrawObjectWithTransform(canvas, wall, wallp1X + (i * 50), wallp2Y, 0, WallDrawer);
                            }

                        } else
                        {
                            double distance = wallp1X - wallp2X;
                            double numberOfWalls = distance / 50;

                            for (int i = 0; i <= numberOfWalls; i++)
                            {
                                DrawObjectWithTransform(canvas, wall, wallp2X + (i * 50), wallp1Y , 0, WallDrawer);
                            }

                        }

                        
                    }
                }

                foreach (Powerup p in theWorld.Powerups.Values) {
                    DrawObjectWithTransform(canvas, p, p.loc.GetX(), p.loc.GetY(), 0, PowerupDrawer);
                }

                foreach (Snake s in theWorld.Snakes.Values)
                {

                    if (s.alive == false && s.snake == theWorld.playerID)
                    {
                        DrawObjectWithTransform(canvas, s, (float)s.body[s.body.Count - 1].GetX(), (float)s.body[s.body.Count - 1].GetY(), 0, GameOverDrawer);

                    }

                    if (s.alive)
                    {
                        Vector2D prev = s.body[0];
                        foreach (Vector2D v in s.body)
                        {
                            Vector2D newVec = prev - v;
                            int newVecLength = (int)newVec.Length();
                            newVec.Normalize();

                            if (newVecLength < theWorld.Size)
                            {
                                int snakeCol = s.snake % 10;
                                canvas.StrokeColor = SnakeColorPicker(snakeCol);
                                float angle = newVec.ToAngle();
                                
                                
                                DrawObjectWithTransform(canvas, newVecLength, v.X, v.Y, angle, SnakeSegmentDrawer);
                                
                                

                            }
                            
                            prev = v;
                            DrawObjectWithTransform(canvas, s, (float)s.body[s.body.Count - 1].GetX(), (float)s.body[s.body.Count - 1].GetY(), 0, NameDrawer);

                        }
                        
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

    /// <summary>
    /// For drawing the players name and score next to the snake
    /// </summary>
    /// <param name="o"></param>
    /// <param name="canvas"></param>
    private void NameDrawer(object o, ICanvas canvas) {
        Snake s = o as Snake;
        canvas.FontColor = Colors.White;
        canvas.FontSize = 18;
        canvas.Font = Font.Default;
        canvas.DrawString(s.name + " " + s.score.ToString(), 20, 0, 150, 100, HorizontalAlignment.Left, VerticalAlignment.Top);
    }

    /// <summary>
    /// For drawing the walls in the specified position
    /// </summary>
    /// <param name="o"></param>
    /// <param name="canvas"></param>
    private void WallDrawer(object o, ICanvas canvas) {
        canvas.DrawImage(wall,
                        -25,
                        -25,
                        wall.Width,
                        wall.Height);
    }

    /// <summary>
    /// Draws the Powerups
    /// </summary>
    /// <param name="o"></param>
    /// <param name="canvas"></param>
    private void PowerupDrawer(object o, ICanvas canvas) {
        Powerup p = o as Powerup;
        int width = 15;
        canvas.FillColor = Colors.Green;

        if (!p.died) {
            canvas.FillRectangle(-(width / 2), -(width / 2), width, width);
        }
    }

    /// <summary>
    /// Draws snake segments 
    /// </summary>
    /// <param name="o"></param>
    /// <param name="canvas"></param>
    private void SnakeSegmentDrawer(object o, ICanvas canvas)
    {
        int length = (int)o;
        canvas.StrokeSize = 10;
        canvas.DrawLine(0, 0, 0, -length);

    }

    /// <summary>
    /// Draws the game over screen and the correct, score dependent text
    /// </summary>
    /// <param name="o"></param>
    /// <param name="canvas"></param>
    private void GameOverDrawer(object o, ICanvas canvas)
    {
        Snake s = o as Snake;
        canvas.FillColor = Colors.Black;
        canvas.FillRectangle(-450, -450, 900, 900);
        canvas.FontColor = Colors.Red;
        canvas.FontSize = 32;
        canvas.Font = Font.Default;
        switch (s.score)
        {
            case < 5:
                canvas.DrawString("Yikes...", -150, -50, 300, 100, HorizontalAlignment.Center, VerticalAlignment.Center);
                break;
            case < 10:
                canvas.DrawString("Decent...", -150, -50, 300, 100, HorizontalAlignment.Center, VerticalAlignment.Center);
                break;
            case < 15:
                canvas.DrawString("Now its a game!", -150, -50, 300, 100, HorizontalAlignment.Center, VerticalAlignment.Center);
                break;
            case > 15:
                canvas.DrawString("Good game!", -150, -50, 300, 100, HorizontalAlignment.Center, VerticalAlignment.Center);
                break;
        }
        

    }

    /// <summary>
    /// Determines what color the snake will be 
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private Color SnakeColorPicker(int number)
    {
        switch (number)
        {
            case 1:
                    return Colors.Black;
            case 2:
                    return Colors.Wheat;
            case 3:
                    return Colors.Red;
            case 4:
                    return Colors.Green;
            case 5:
                    return Colors.Blue;
            case 6:
                    return Colors.Yellow;
            case 7:
                    return Colors.Orange;
            case 8:
                    return Colors.Purple;
            case 9:
                    return Colors.Gray;
            case 0:
                    return Colors.Cyan;

        }
        return Colors.DarkGreen;
    }
   

}
