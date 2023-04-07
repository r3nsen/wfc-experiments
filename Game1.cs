using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Threading;

namespace Hello_Wave_Function_Collapse_Algorithm
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D tileset;
        // data
        struct tile
        {
            public int id, rot;
            public int[] soket;
            public override string ToString()
            {
                return $"id: {id}, rot: {rot}";
            }
        }
        struct grid
        {
            public byte possibilities; // 8 possibilidades (8 bits)
            public byte colapsed;
            public override string ToString()
            {
                return $"possibilities: {Convert.ToString(possibilities, 2).PadLeft(8, '0')}, colapsed: {colapsed}";
            }
        }
        tile[] tiles = {
            new tile() { id = 1,rot = 0, soket= new int[] { 0,0,0, 0,0,0, 0,0,0, 0,0,0 } },
            new tile() { id = 0,rot = 0, soket= new int[] { 0,1,0, 0,1,0, 0,0,0, 0,0,0 } },
            new tile() { id = 0,rot = 1, soket= new int[] { 0,0,0, 0,1,0, 0,1,0, 0,0,0 } },
            new tile() { id = 0,rot = 2, soket= new int[] { 0,0,0, 0,0,0, 0,1,0, 0,1,0 } },
            new tile() { id = 0,rot = 3, soket= new int[] { 0,1,0, 0,0,0, 0,0,0, 0,1,0 } }
        };
        const int GRID_SIZE_X = 39;
        const int GRID_SIZE_Y = 21;
        grid[,] _grid = new grid[GRID_SIZE_Y, GRID_SIZE_X];

        Stack<(int num, grid[,] g)> hystory = new Stack<(int num, grid[,] g)>();

        //----
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 960;
            _graphics.PreferredBackBufferHeight = 540;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            tileset = Content.Load<Texture2D>("wfc tileset");

            reset();

        }
        void reset()
        {
            for (int i = 0; i < GRID_SIZE_X; i++)
                for (int j = 0; j < GRID_SIZE_Y; j++)
                {
                    _grid[j, i].possibilities = 0b11111;
                    _grid[j, i].colapsed = 5;
                }
            rand = new Random(/*69*/);
            iteration = 0;
        }
        Random rand = new Random();
        int counter = 5;
        int curr = 0;
        int iteration = 0;
        protected override void Update(GameTime gameTime)
        {
            NewUpdate2(gameTime);
            base.Update(gameTime);
        }
        void NewUpdate2(GameTime gameTime)
        {
            if (counter > 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    counter--;
                return;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) reset();
            // counter = 50;

            //for (int i = 0; i < GRID_SIZE; i++)
            //{
            //    for (int j = 0; j < GRID_SIZE; j++)
            //    {
            //        _grid[i, j].possibilities = (byte)Math.Pow(2,curr);                    
            //        _grid[i, j].colapsed = 1;
            //    }
            //}
            //curr = (curr + 1) % 5;
            //return;

            int minorEntropy = 1000000000;
            int minorEntropyIndex = -1;
            for (int i = 0; i < GRID_SIZE_X * GRID_SIZE_Y; i++)
            {
                int col = _grid[i / GRID_SIZE_X, i % GRID_SIZE_X].colapsed;
                if (col > 1)
                {
                    if (col < minorEntropy)
                    {
                        minorEntropy = col;
                        minorEntropyIndex = i;
                    }
                }
            }

            int r = rand.Next(0, minorEntropy) + 1;

            if (minorEntropyIndex < 0) return;
            iteration++;
            if (iteration == 14)
                ;
            for (int k = 0; k < tiles.Length; k++)
            {
                if ((_grid[minorEntropyIndex / GRID_SIZE_X, minorEntropyIndex % GRID_SIZE_X].possibilities & (1 << k)) != 0)
                {
                    if (r > 1)
                    {
                        r--;
                        //k++;
                        continue;
                    }
                    int x = minorEntropyIndex % GRID_SIZE_X, y = minorEntropyIndex / GRID_SIZE_X;
                    _grid[y, x].possibilities &= (byte)(1 << k);
                    _grid[y, x].colapsed = 1;
                    //
                    //int adjx, adjy;
                    //adjx = x - 1; adjy = y;
                    bool toReset = false;

                    // recebe posição atual e atualiza possibilidades baseado nos seus arredores caso gridcheck for -1
                    // depois de atualizar, seta gridcheck atual para 1, e arredores para -1 caso sejam 0
                    void check(int x, int y)
                    {

                        if (!checkCoord(x, y)) return; //checa se coordenada é válida
                        if (!checkColapse(x, y)) return;

                        for (int face = 0; face < 4; face++)
                        {
                            if (y + ((face - 1) % 2) == 25)
                                ;
                            if (!checkCoord(x + ((2 - face)) % 2, y + ((face - 1) % 2))) continue;

                            for (int j_tiles = 0; j_tiles < 8; j_tiles++)
                            {
                                bool tilechecked = false;
                                if ((_grid[y, x].possibilities & (1 << j_tiles)) == 0) continue;
                                for (int i_tiles = 0; i_tiles < 8; i_tiles++)
                                {
                                    if ((_grid[y + ((face - 1) % 2), x + ((2 - face)) % 2].possibilities & (1 << i_tiles)) == 0) continue;
                                    bool temp = checkSockets(face, j_tiles, i_tiles);
                                    tilechecked |= temp;
                                }
                                if (!tilechecked)
                                {
                                    byte temp = _grid[y, x].possibilities;
                                    _grid[y, x].possibilities &= (byte)~(1 << j_tiles);
                                    if (temp != _grid[y, x].possibilities)
                                        _grid[y, x].colapsed--;
                                }
                            }
                        }
                    }
                    sbyte[][,] gridCheck = new sbyte[2][,] {
                        new sbyte[GRID_SIZE_Y, GRID_SIZE_X],
                        new sbyte[GRID_SIZE_Y, GRID_SIZE_X]
                    };
                    int currentGrid = 0;
                    gridCheck[currentGrid][y, x] = -1;
                    bool uncheckedvalues = true;
                    while (uncheckedvalues)
                    {
                        int nextGrid = (currentGrid + 1) % 2;
                        uncheckedvalues = false;
                        for (int l = 0; l < GRID_SIZE_Y; l++)
                        {
                            for (int m = 0; m < GRID_SIZE_X; m++)
                            {
                                //if (gridCheck[currentGrid][l, m] == 0 && ) gridCheck[nextGrid][l, m] = 0;
                                if (gridCheck[currentGrid][l, m] == 1) gridCheck[nextGrid][l, m] = 1;
                                if (gridCheck[currentGrid][l, m] == -1)
                                {
                                    uncheckedvalues = true;
                                    gridCheck[nextGrid][l, m] = 1;

                                    check(m, l);
                                    if (checkCoord(m, l - 1)) gridCheck[nextGrid][l - 1, m] = (sbyte)(gridCheck[nextGrid][l - 1, m] == 0 ? -1 : gridCheck[nextGrid][l - 1, m]);
                                    if (checkCoord(m, l + 1)) gridCheck[nextGrid][l + 1, m] = (sbyte)(gridCheck[nextGrid][l + 1, m] == 0 ? -1 : gridCheck[nextGrid][l + 1, m]);
                                    if (checkCoord(m - 1, l)) gridCheck[nextGrid][l, m - 1] = (sbyte)(gridCheck[nextGrid][l, m - 1] == 0 ? -1 : gridCheck[nextGrid][l, m - 1]);
                                    if (checkCoord(m + 1, l)) gridCheck[nextGrid][l, m + 1] = (sbyte)(gridCheck[nextGrid][l, m + 1] == 0 ? -1 : gridCheck[nextGrid][l, m + 1]);
                                }
                            }
                        }
                        currentGrid = nextGrid;
                    }

                    //int[] karray = new int[] { k };
                    //check(x - 1, y, 1, karray, ref gridCheck);
                    //check(x + 1, y, 3, karray, ref gridCheck);
                    //check(x, y - 1, 2, karray, ref gridCheck);
                    //check(x, y + 1, 0, karray, ref gridCheck);
                    if (toReset) reset();
                    break;
                }
            }
            base.Update(gameTime);
        }
        void NewUpdate(GameTime gameTime)
        {
            if (counter > 0)
            {
                //if (Keyboard.GetState().IsKeyDown(Keys.Left))
                counter--;
                return;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) reset();
            counter = 0;

            //for (int i = 0; i < GRID_SIZE; i++)
            //{
            //    for (int j = 0; j < GRID_SIZE; j++)
            //    {
            //        _grid[i, j].possibilities = (byte)Math.Pow(2,curr);                    
            //        _grid[i, j].colapsed = 1;
            //    }
            //}
            //curr = (curr + 1) % 5;
            //return;

            int minorEntropy = 1000000000;
            int minorEntropyIndex = -1;
            for (int i = 0; i < GRID_SIZE_X * GRID_SIZE_Y; i++)
            {
                int col = _grid[i / GRID_SIZE_Y, i % GRID_SIZE_X].colapsed;
                if (col > 1)
                {
                    if (col < minorEntropy)
                    {
                        minorEntropy = col;
                        minorEntropyIndex = i;
                    }
                }
            }

            int r = rand.Next(0, minorEntropy) + 1;

            if (minorEntropyIndex < 0) return;
            iteration++;
            if (iteration == 14)
                ;
            for (int k = 0; k < tiles.Length; k++)
            {
                if ((_grid[minorEntropyIndex / GRID_SIZE_Y, minorEntropyIndex % GRID_SIZE_X].possibilities & (1 << k)) != 0)
                {
                    if (r > 1)
                    {
                        r--;
                        //k++;
                        continue;
                    }
                    int x = minorEntropyIndex % GRID_SIZE_X, y = minorEntropyIndex / GRID_SIZE_Y;
                    _grid[y, x].possibilities &= (byte)(1 << k);
                    _grid[y, x].colapsed = 1;
                    //
                    //int adjx, adjy;
                    //adjx = x - 1; adjy = y;
                    bool toReset = false;
                    int deepness = 0;
                    void check(int x, int y, int face, int[] _tiles, ref byte[,] gridCheck)
                    {
                        deepness++;
                        if (!checkCoord(x, y))
                        {
                            deepness--; return;
                        }
                        if (gridCheck[y, x] == 1)
                        {
                            deepness--; return;
                        }
                        gridCheck[y, x] = 1;

                        for (int l = 0; l < tiles.Length; l++)
                            if ((_grid[y, x].possibilities & (1 << l)) != 0)
                            {

                                bool tilescheck = false;
                                for (int k = 0; k < _tiles.Length; k++)
                                    tilescheck |= checkSockets(face, l, _tiles[k]);

                                if (!tilescheck)
                                {
                                    if (_grid[y, x].colapsed == 1)
                                        ;
                                    _grid[y, x].possibilities &= (byte)~(1 << l);
                                    _grid[y, x].colapsed--;
                                    if (_grid[y, x].colapsed < 1)
                                        toReset = true;
                                }
                            }
                        //int[] karray = new int[_grid[y, x].colapsed];
                        //int kindex = 0;
                        //for (int i = 0; i < 8; i++)
                        //{
                        //    if ((_grid[y, x].possibilities & (1 << i)) != 0)
                        //        karray[kindex++] = i; 
                        //}
                        //check(x - 1, y, 1, karray, ref gridCheck);
                        //check(x + 1, y, 3, karray, ref gridCheck);
                        //check(x, y - 1, 2, karray, ref gridCheck);
                        //check(x, y + 1, 0, karray, ref gridCheck);
                        //deepness--;
                    }
                    byte[,] gridCheck = new byte[GRID_SIZE_Y, GRID_SIZE_X];

                    int[] karray = new int[] { k };
                    check(x - 1, y, 1, karray, ref gridCheck);
                    check(x + 1, y, 3, karray, ref gridCheck);
                    check(x, y - 1, 2, karray, ref gridCheck);
                    check(x, y + 1, 0, karray, ref gridCheck);
                    if (toReset) reset();
                    break;
                }
            }
            base.Update(gameTime);
        }
        void OldUpdate(GameTime gameTime)
        {
            if (counter > 0)
            {
                counter--;
                return;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space)) reset();
            counter = 0;

            int minorEntropy = 1000000000;
            int minorEntropyIndex = -1;
            for (int i = 0; i < GRID_SIZE_X * GRID_SIZE_Y; i++)
            {
                int col = _grid[i / GRID_SIZE_Y, i % GRID_SIZE_X].colapsed;
                if (col > 1)
                {
                    if (col < minorEntropy)
                    {
                        minorEntropy = col;
                        minorEntropyIndex = i;
                    }
                }
            }

            int r = rand.Next(0, minorEntropy) + 1;

            if (minorEntropyIndex < 0) return;
            iteration++;
            if (iteration == 14)
                ;
            for (int k = 0; k < tiles.Length; k++)
            {
                if ((_grid[minorEntropyIndex / GRID_SIZE_Y, minorEntropyIndex % GRID_SIZE_X].possibilities & (1 << k)) != 0)
                {
                    if (r > 1)
                    {
                        r--;
                        //k++;
                        continue;
                    }
                    int x = minorEntropyIndex % GRID_SIZE_X, y = minorEntropyIndex / GRID_SIZE_Y;
                    _grid[y, x].possibilities &= (byte)(1 << k);
                    _grid[y, x].colapsed = 1;
                    //
                    int adjx, adjy;
                    adjx = x - 1; adjy = y;
                    bool toReset = false;
                    void check(int x, int y, int face, int tile, ref byte[,] gridCheck)
                    {
                        if (!checkCoord(adjx, adjy)) return;
                        if (gridCheck[y, x] == 1) return;
                        gridCheck[y, x] = 1;

                        for (int l = 0; l < tiles.Length; l++)
                            if ((_grid[y, x].possibilities & (1 << l)) != 0)
                                if (!checkSockets(face, l, tile))
                                {
                                    if (_grid[y, x].colapsed == 1)
                                        ;
                                    _grid[y, x].possibilities &= (byte)~(1 << l);
                                    _grid[y, x].colapsed--;
                                    if (_grid[y, x].colapsed < 1)
                                        toReset = true;
                                }


                    }
                    byte[,] gridCheck = new byte[GRID_SIZE_Y, GRID_SIZE_X];

                    check(adjx, adjy, 1, k, ref gridCheck);
                    adjx = x + 1; adjy = y;
                    check(adjx, adjy, 3, k, ref gridCheck);
                    adjx = x; adjy = y - 1;
                    check(adjx, adjy, 2, k, ref gridCheck);
                    adjx = x; adjy = y + 1;
                    check(adjx, adjy, 0, k, ref gridCheck);
                    // if (toReset) reset();
                    break;
                }
            }
            base.Update(gameTime);
        }
        bool checkSockets(int face, int tile1, int tile2)
        {
            if (tiles[tile1].soket[((3 * face + 0) + 12) % 12] == tiles[tile2].soket[((3 * face + 2) + 6 + 12) % 12])
                if (tiles[tile1].soket[((3 * face + 1) + 12) % 12] == tiles[tile2].soket[((3 * face + 1) + 6 + 12) % 12])
                    if (tiles[tile1].soket[((3 * face + 2) + 12) % 12] == tiles[tile2].soket[((3 * face + 0) + 6 + 12) % 12])
                        return true;
            return false;
        }
        bool checkCoord(int x, int y)
        {
            return (x >= 0 && x < GRID_SIZE_X && y >= 0 && y < GRID_SIZE_Y);
        }
        bool checkColapse(int x, int y)
        {
            return _grid[y, x].colapsed > 1;
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(50, 50, 60));

            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 1), new Vector3(0, 0, 0), Vector3.Up);
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, 96 * 5, 54 * 5, 0, 0, 100);
            Matrix reset = Matrix.CreateScale(960 / 2, -540 / 2, 1) * Matrix.CreateTranslation(new Vector3(960 / 2, 540 / 2, 0));
            Vector2 orig = new Vector2(10, 10);
            _spriteBatch.Begin(blendState: BlendState.NonPremultiplied, samplerState: SamplerState.PointClamp, transformMatrix: view * projection * reset);
            for (int i = 0; i < GRID_SIZE_X; i++)
            {
                for (int j = 0; j < GRID_SIZE_Y; j++)
                {
#if false
                    int possibilitiesCount = 0;
                    for (int k = 0; k < tiles.Length; k++)
                    {
                        if ((_grid[j, i].possibilities & (1 << k)) != 0)
                            ++possibilitiesCount;
                    }
                    for (int k = 0; k < tiles.Length; k++)
                    {
                        if ((_grid[j, i].possibilities & (1 << k)) != 0)
                        {
                            int id = tiles[k].id;
                            float rot = (MathHelper.Tau / 4) * (tiles[k].rot);
                            Rectangle rect = new Rectangle(3 * id, 3 * id, 3, 3);
                            _spriteBatch.Draw(tileset, orig + new Vector2(i, j) * 3, rect, new Color(255, 255, 255, 255 / possibilitiesCount), rot, new Vector2(1.5f, 1.5f), 1, SpriteEffects.None, 0);
                        }
                    }
#else
                    if (_grid[j, i].colapsed == 1)
                    {
                        for (int k = 0; k < tiles.Length; k++)
                        {
                            if ((_grid[j, i].possibilities & (1 << k)) != 0)
                            {
                                int id = tiles[k].id;
                                float rot = (MathHelper.Tau / 4) * (tiles[k].rot);
                                Rectangle rect = new Rectangle(3 * id, 3 * id, 3, 3);
                                //_spriteBatch.Draw(tileset, orig + new Vector2(i, j) * 3, rect, new Color(255, 255, 255, 255 / 1), rot, new Vector2(1.5f, 1.5f), 1, SpriteEffects.None, 0);
                                _spriteBatch.Draw(tileset,
                                    destinationRectangle: new Rectangle((int)orig.X + 4 + i * (3 + 1) * 3, (int)orig.Y + 4 + j * (3 + 1) * 3, (3 + 1) * 3, (3 + 1) * 3), rect, new Color(255, 255, 255, 255 / 1), rot, new Vector2(1.5f, 1.5f), SpriteEffects.None, 0);
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < tiles.Length; k++)
                        {
                            if ((_grid[j, i].possibilities & (1 << k)) != 0)
                            {
                                int id = tiles[k].id;
                                float rot = (MathHelper.Tau / 4) * (tiles[k].rot);
                                Rectangle rect = new Rectangle(3 * id, 3 * id, 3, 3);
                                Vector2 veryLittleOff = new Vector2(k % 3, k / 3);
                                _spriteBatch.Draw(tileset, destinationRectangle:
                                    new Rectangle(
                                        (int)orig.X + i * 4 * 3 + (int)veryLittleOff.X * 3,
                                        (int)orig.Y + j * 4 * 3 + (int)veryLittleOff.Y * 3,
                                         3, 3),
                                        rect, new Color(255, 255, 255, 255 / 1), rot, new Vector2(1.5f, 1.5f), SpriteEffects.None, 0);
                                //break;
                            }
                        }

                    }
#endif
                }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}