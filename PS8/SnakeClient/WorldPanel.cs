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



namespace SnakeGame;
public class WorldPanel : IDrawable
{

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
    private int viewSize = 2000;

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
            canvas.Translate((float)viewSize / 2, (float)viewSize / 2);

            //theWorld.Snakes.TryGetValue(theWorld.playerID, out var value);

            theWorld.Snakes.TryGetValue(theWorld.playerID, out Snake playerSnake);
            double playerX = playerSnake.body[0].GetX();
            double playerY = playerSnake.body[0].GetY();
            //float playerY = (float)value.body[value.body.Count].GetY();

            //canvas.Translate(-playerX + (viewSize / 2), -playerY + (viewSize / 2));
            //canvas.DrawImage(background, 0, 0, background.Width, background.Height);
            canvas.DrawImage(background, (-theWorld.Size / 2), (-theWorld.Size / 2), theWorld.Size, theWorld.Size);
            //canvas.DrawImage(wall, 0, 0, wall.Width, wall.Height);
            //lock (theWorld) {
            //    foreach (Wall w in theWorld.Walls.Values)
            //    {
            //        canvas.DrawImage(wall, (float)w.p1.X, (float)w.p1.Y, wall.Width, wall.Height);
            //    }

            //}
        }
        Console.WriteLine("Redraw");
        Debug.WriteLine("Redraw");
    }

}
