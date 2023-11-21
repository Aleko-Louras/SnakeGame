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

        //canvas.Translate((float)viewSize / 2, (float)viewSize / 2);

        // example code for how to draw
        // (the image is not visible in the starter code)
        canvas.DrawImage(background, 0, 0, 900, 900);
        canvas.DrawImage(wall, 0, 0, wall.Width, wall.Height);
        lock (theWorld) {
            foreach (Wall w in theWorld.Walls.Values) {
                canvas.DrawImage(wall, (float)w.p1.X, (float)w.p1.Y, wall.Width, wall.Height);
            }
        }
        Console.WriteLine("Redraw");
        Debug.WriteLine("Redraw");
    }

}
