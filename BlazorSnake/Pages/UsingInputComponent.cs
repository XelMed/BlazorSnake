using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

using System;
using System.Threading;
using System.Collections.Generic;
using Blazor.Extensions;
using Microsoft.JSInterop;

namespace BlazorSnake.Pages
{
    public class UsingInputComponent : ComponentBase
    {
        [Inject]
        IJSRuntime Runtime { get; set; }

        private Canvas2DContext _context;
        protected BECanvasComponent _canvasReference;
        Timer _timer;
        int _px = 10; int _py = 10;
        int _gs = 20; int _tc = 20;
        int _ax = 15; int _ay = 15;
        int _xv = 0; int _yv = 0;
        List<(int x, int y)> _trail = new List<(int x, int y)>();
        int _tail = 5;
        protected string _key;
        protected string _code;
        protected string ControlInput { get; set; } = string.Empty;
        Random _rr = new Random();
        bool _render = false;
        protected override async Task OnAfterRenderAsync()
        {
            if (!this._render)
            {
                this._context = await this._canvasReference.CreateCanvas2DAsync();
                this._timer = new Timer(this.Game, null, 1000,
                       1000 / 15);
                this._render = true;
            }
            await this.Runtime.InvokeAsync<bool>("MySetFocus", "ControlInput");
        }
        public async void Game(object stateInfo)
        {
            if (this._context == null) return;
            this._px += this._xv;
            this._py += this._yv;
            //crossboard
            if (this._px < 0)
            {
                this._px = this._tc - 1;
            }
            if (this._px > this._tc - 1)
            {
                this._px = 0;
            }
            if (this._py < 0)
            {
                this._py = this._tc - 1;
            }
            if (this._py > this._tc - 1)
            {
                this._py = 0;
            }
            await this._context.SetFillStyleAsync("black");
            await this._context.FillRectAsync(0, 0, 400, 400);

            await this._context.SetFillStyleAsync("lime");
            for (var i = 0; i < this._trail.Count; i++)
            {
                await this._context.FillRectAsync(this._trail[i].x * this._gs, this._trail[i].y * this._gs, this._gs - 2, this._gs - 2);
                if (this._trail[i].x == this._px && this._trail[i].y == this._py)
                {
                    this._tail = 5;
                }
            }
            this._trail.Add((this._px, this._py));
            while (this._trail.Count > this._tail)
            {
                this._trail.RemoveAt(0);
            }

            if (this._ax == this._px && this._ay == this._py)
            {
                this._tail++;
                this._ax = this._rr.Next(1, this._tc);
                this._ay = this._rr.Next(1, this._tc);
            }
            await this._context.SetFillStyleAsync("red");
            await this._context.FillRectAsync(this._ax * this._gs, this._ay * this._gs, this._gs - 2, this._gs - 2);

        }
        /// <summary>
        /// Sends the equivalent <see cref="PlayKey"/> from a Key Up event to the <see cref="IGameController"/>.
        /// </summary>
        public void KeyPressInDiv(UIKeyboardEventArgs ev)
        {
            switch (ev.Code)
            {
                case "KeyA":
                    this._xv = -1; this._yv = 0;
                    break;
                case "KeyW":
                    this._xv = 0; this._yv = -1;
                    break;
                case "KeyD":
                    this._xv = 1; this._yv = 0;
                    break;
                case "KeyS":
                    this._xv = 0; this._yv = 1;
                    break;
            }
            this._key = ev.Key;
            this._code = ev.Code;
            this.ControlInput = "";
        }

    }

}
