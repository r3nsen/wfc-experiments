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
            //static int IdCounter = 0;
            public int id, rot;
            public string soket;
            public Rectangle rect;
            public override string ToString()
            {
                return $"id: {id}, rot: {rot}, sockets: {soket}";
            }
        }
        struct grid
        {
            public byte[] possibilities; // 8 possibilidades (8 bits)
            public byte colapsed;
            public override string ToString()
            {
                string temp = "";
                for (int i = possibilities.Length - 1; i >= 0; i--)
                {
                    temp += Convert.ToString(possibilities[i], 2).PadLeft(8, '0');
                }
                //temp = temp.PadLeft(((temp.Length / 8) + 1) * 8, '0');
                return $"possibilities: {temp}, colapsed: {colapsed}";
            }
        }
        List<tile> tiles = new List<tile>(){
            // praia agua
            new tile() { id = 00,rot = 0, soket= new string("sss-ssw-wss-sss" ), rect = new Rectangle(8 + 08 * 0, 8 + 08 * 0,8,8) },
            new tile() { id = 01,rot = 0, soket= new string("sss-ssw-www-wss" ), rect = new Rectangle(8 + 08 * 1, 8 + 08 * 0,8,8) },
            new tile() { id = 02,rot = 0, soket= new string("sss-sss-ssw-wss" ), rect = new Rectangle(8 + 08 * 2, 8 + 08 * 0,8,8) },
            new tile() { id = 03,rot = 0, soket= new string("ssw-www-wss-sss" ), rect = new Rectangle(8 + 08 * 0, 8 + 08 * 1,8,8) },
            new tile() { id = 04,rot = 0, soket= new string("www-www-www-www" ), rect = new Rectangle(8 + 08 * 1, 8 + 08 * 1,8,8) },
            new tile() { id = 04,rot = 0, soket= new string("www-www-www-www" ), rect = new Rectangle(8 + 08 * 1, 8 + 08 * 1,8,8) },
            new tile() { id = 05,rot = 0, soket= new string("wss-sss-ssw-www" ), rect = new Rectangle(8 + 08 * 2, 8 + 08 * 1,8,8) },
            new tile() { id = 06,rot = 0, soket= new string("ssw-wss-sss-sss" ), rect = new Rectangle(8 + 08 * 0, 8 + 08 * 2,8,8) },
            new tile() { id = 07,rot = 0, soket= new string("www-wss-sss-ssw" ), rect = new Rectangle(8 + 08 * 1, 8 + 08 * 2,8,8) },
            new tile() { id = 08,rot = 0, soket= new string("wss-sss-sss-ssw" ), rect = new Rectangle(8 + 08 * 2, 8 + 08 * 2,8,8) },
            //agua praia                                       
            new tile() { id = 09,rot = 0, soket= new string("www-wss-ssw-www" ), rect = new Rectangle(32 + 08 * 0, 8 + 08 * 0,8,8) },
            new tile() { id = 10,rot = 0, soket= new string("www-wss-sss-ssw" ), rect = new Rectangle(32 + 08 * 1, 8 + 08 * 0,8,8) },
            new tile() { id = 11,rot = 0, soket= new string("www-www-wss-ssw" ), rect = new Rectangle(32 + 08 * 2, 8 + 08 * 0,8,8) },
            new tile() { id = 12,rot = 0, soket= new string("wss-sss-ssw-www" ), rect = new Rectangle(32 + 08 * 0, 8 + 08 * 1,8,8) },
            new tile() { id = 13,rot = 0, soket= new string("sss-sss-sss-sss" ), rect = new Rectangle(32 + 08 * 1, 8 + 08 * 1,8,8) },
            new tile() { id = 14,rot = 0, soket= new string("ssw-www-wss-sss" ), rect = new Rectangle(32 + 08 * 2, 8 + 08 * 1,8,8) },
            new tile() { id = 15,rot = 0, soket= new string("wss-ssw-www-www" ), rect = new Rectangle(32 + 08 * 0, 8 + 08 * 2,8,8) },
            new tile() { id = 16,rot = 0, soket= new string("sss-ssw-www-wss" ), rect = new Rectangle(32 + 08 * 1, 8 + 08 * 2,8,8) },
            new tile() { id = 17,rot = 0, soket= new string("ssw-www-www-wss" ), rect = new Rectangle(32 + 08 * 2, 8 + 08 * 2,8,8) },
            //praia floresta                                   
            new tile() { id = 18,rot = 0, soket= new string("sss-ssf-fss-sss" ), rect = new Rectangle(8 + 08 * 0, 32 + 08 * 0,8,8) },
            new tile() { id = 19,rot = 0, soket= new string("sss-ssf-fff-fss" ), rect = new Rectangle(8 + 08 * 1, 32 + 08 * 0,8,8) },
            new tile() { id = 20,rot = 0, soket= new string("sss-sss-ssf-fss" ), rect = new Rectangle(8 + 08 * 2, 32 + 08 * 0,8,8) },
            new tile() { id = 21,rot = 0, soket= new string("ssw-fff-fss-sss" ), rect = new Rectangle(8 + 08 * 0, 32 + 08 * 1,8,8) },
            
            new tile() { id = 22,rot = 0, soket= new string("fff-fff-fff-fff" ), rect = new Rectangle(8 + 08 * 1, 32 + 08 * 1,8,8) },
            new tile() { id = 22,rot = 0, soket= new string("fff-fff-fff-fff" ), rect = new Rectangle(8 + 08 * 1, 32 + 08 * 1,8,8) },
            new tile() { id = 22,rot = 0, soket= new string("fff-fff-fff-fff" ), rect = new Rectangle(8 + 08 * 1, 32 + 08 * 1,8,8) },
            new tile() { id = 22,rot = 0, soket= new string("fff-fff-fff-fff" ), rect = new Rectangle(8 + 08 * 1, 32 + 08 * 1,8,8) },

            new tile() { id = 23,rot = 0, soket= new string("fss-sss-ssf-fff" ), rect = new Rectangle(8 + 08 * 2, 32 + 08 * 1,8,8) },
            new tile() { id = 24,rot = 0, soket= new string("ssf-fss-sss-sss" ), rect = new Rectangle(8 + 08 * 0, 32 + 08 * 2,8,8) },
            new tile() { id = 25,rot = 0, soket= new string("fff-fss-sss-ssf" ), rect = new Rectangle(8 + 08 * 1, 32 + 08 * 2,8,8) },
            new tile() { id = 26,rot = 0, soket= new string("fss-sss-sss-ssf" ), rect = new Rectangle(8 + 08 * 2, 32 + 08 * 2,8,8) },
            // floresta praia                                  
            new tile() { id = 27,rot = 0, soket= new string("fff-fss-ssf-fff" ), rect = new Rectangle(32 + 08 * 0, 32 + 08 * 0,8,8) },
            new tile() { id = 28,rot = 0, soket= new string("fff-fss-sss-ssf" ), rect = new Rectangle(32 + 08 * 1, 32 + 08 * 0,8,8) },
            new tile() { id = 29,rot = 0, soket= new string("fff-fff-fss-ssf" ), rect = new Rectangle(32 + 08 * 2, 32 + 08 * 0,8,8) },
            new tile() { id = 30,rot = 0, soket= new string("fss-sss-ssf-fff" ), rect = new Rectangle(32 + 08 * 0, 32 + 08 * 1,8,8) },
            new tile() { id = 31,rot = 0, soket= new string("sss-sss-sss-sss" ), rect = new Rectangle(32 + 08 * 1, 32 + 08 * 1,8,8) },
            new tile() { id = 32,rot = 0, soket= new string("ssf-fff-fss-sss" ), rect = new Rectangle(32 + 08 * 2, 32 + 08 * 1,8,8) },
            new tile() { id = 33,rot = 0, soket= new string("fss-ssf-fff-fff" ), rect = new Rectangle(32 + 08 * 0, 32 + 08 * 2,8,8) },
            new tile() { id = 34,rot = 0, soket= new string("sss-ssf-fff-fss" ), rect = new Rectangle(32 + 08 * 1, 32 + 08 * 2,8,8) },
            new tile() { id = 35,rot = 0, soket= new string("ssf-fff-fff-fss" ), rect = new Rectangle(32 + 08 * 2, 32 + 08 * 2,8,8) },

            new tile() { id = 36,rot = 0, soket= new string("www-www-www-www" ), rect = new Rectangle(0, 16,8,8) },
            new tile() { id = 37,rot = 0, soket= new string("fff-fff-fff-fff" ), rect = new Rectangle(0, 32,8,8) },
        };
        const int GRID_SIZE_X = 10;//39;
        const int GRID_SIZE_Y = 8;//21;
        grid[,] _grid = new grid[GRID_SIZE_Y, GRID_SIZE_X];

        Stack<(int num, grid[,] g)> hystory = new Stack<(int num, grid[,] g)>();

        //----
        public void AddRotateTiles(params int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    string cs = tiles[ids[i]].soket;
                    //00 01 02 - 04 05 06 - 08 09 10 - 12 13 14
                    //12 13 14 - 00 01 02 - 04 05 06 - 08 09 10
                    string newsoket =
                        $"{cs[(00 + (1 + 2) * 4 * j) % 16]}" +
                        $"{cs[(01 + (1 + 2) * 4 * j) % 16]}" +
                        $"{cs[(02 + (1 + 2) * 4 * j) % 16]}" +
                        $"-" +
                        $"{cs[(04 + (1 + 2) * 4 * j) % 16]}" +
                        $"{cs[(05 + (1 + 2) * 4 * j) % 16]}" +
                        $"{cs[(06 + (1 + 2) * 4 * j) % 16]}" +
                        $"-" +
                        $"{cs[(08 + (1 + 2) * 4 * j) % 16]}" +
                        $"{cs[(09 + (1 + 2) * 4 * j) % 16]}" +
                        $"{cs[(10 + (1 + 2) * 4 * j) % 16]}" +
                        $"-" +
                        $"{cs[(12 + (1 + 2) * 4 * j) % 16]}" +
                        $"{cs[(13 + (1 + 2) * 4 * j) % 16]}" +
                        $"{cs[(14 + (1 + 2) * 4 * j) % 16]}";
                    tiles.Add(new tile() { id = tiles[ids[i]].id, rot = j, rect = tiles[ids[i]].rect, soket = newsoket });
                }
            }

        }
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
            //tileset = Content.Load<Texture2D>("wfc tileset");
            tileset = Content.Load<Texture2D>("tileset praia wfc");
            //AddRotateTiles(0, 1);

            reset();

        }
        bool setbit(int x, int y, int index)
        {
            bool temp = !checkbit(x, y, index);
            _grid[y, x].possibilities[index / 8] |= (byte)(1 << (index % 8));
            return temp;
        }
        bool selectbit(int x, int y, int index)
        {
            //_grid[y, x].possibilities &= (byte)(1 << k);
            bool temp = checkbit(x, y, index);
            for (int i = 0; i < _grid[y, x].possibilities.Length; i++)
            {
                _grid[y, x].possibilities[i] = 0;
            }
            _grid[y, x].possibilities[index / 8] |= (byte)(1 << (index % 8));
            if (_grid[y, x].possibilities[index / 8] == 0)
                ;
            return temp;
        }
        bool unsetbit(int x, int y, int index)
        {
            bool temp = checkbit(x, y, index);
            _grid[y, x].possibilities[index / 8] &= (byte)~(1 << (index % 8));
            return temp;
        }
        bool checkbit(int x, int y, int index)
        {
            //if ((_grid[minorEntropyIndex / GRID_SIZE_X, minorEntropyIndex % GRID_SIZE_X].possibilities & (1 << k)) != 0)
            bool temp = (_grid[y, x].possibilities[index / 8] & (byte)(1 << (index % 8))) != 0;
            return temp;
            //return (_grid[y, x].possibilities[index / 8] & (byte)~(1 << (index % 8))) != 0;
        }

        void reset()
        {
            resetGridCheck();
            for (int i = 0; i < GRID_SIZE_X; i++)
                for (int j = 0; j < GRID_SIZE_Y; j++)
                {
                    //_grid[j, i].possibilities = 0b11111;
                    if (i == 0 || i == GRID_SIZE_X - 1 || j == 0 || j == GRID_SIZE_Y - 1)
                    {
                        _grid[j, i].possibilities = new byte[(tiles.Count) / 8 + 1];
                        setbit(i, j, 4);
                        _grid[j, i].colapsed = 1;
                        gridCheck[currentGrid][j, i] = -1;
                        continue;
                    }

                    _grid[j, i].possibilities = new byte[(tiles.Count) / 8 + 1];
                    for (int k = 0; k < tiles.Count; k++)
                    {
                        setbit(i, j, k);
                    }
                    _grid[j, i].colapsed = (byte)tiles.Count;
                }
            propagate();
            rand = new Random();
            iteration = 0;
        }
        Random rand = new Random(0);
        int counter = 5;
        int curr = 0;
        int iteration = 0;

        sbyte[][,] gridCheck = new sbyte[2][,] {
                        new sbyte[GRID_SIZE_Y, GRID_SIZE_X],
                        new sbyte[GRID_SIZE_Y, GRID_SIZE_X]
                    };
        int currentGrid = 0;
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
            for (int k = 0; k < tiles.Count; k++)
            {
                //if ((_grid[minorEntropyIndex / GRID_SIZE_X, minorEntropyIndex % GRID_SIZE_X].possibilities & (1 << k)) != 0)
                if (checkbit(minorEntropyIndex % GRID_SIZE_X, minorEntropyIndex / GRID_SIZE_X, k))
                {
                    if (r > 1)
                    {
                        r--;
                        //k++;
                        continue;
                    }
                    int x = minorEntropyIndex % GRID_SIZE_X, y = minorEntropyIndex / GRID_SIZE_X;
                    selectbit(x, y, k);//_grid[y, x].possibilities &= (byte)(1 << k);
                    _grid[y, x].colapsed = 1;
                    //
                    //int adjx, adjy;
                    //adjx = x - 1; adjy = y;
                    bool toReset = false;

                    // recebe posição atual e atualiza possibilidades baseado nos seus arredores caso gridcheck for -1
                    // depois de atualizar, seta gridcheck atual para 1, e arredores para -1 caso sejam 0
                    resetGridCheck();
                    gridCheck[currentGrid][y, x] = -1;
                    propagate();

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
        void resetGridCheck()
        {
            for (int ii = 0; ii < GRID_SIZE_X; ii++)
            {
                for (int jj = 0; jj < GRID_SIZE_Y; jj++)
                {
                    gridCheck[0][jj, ii] = gridCheck[1][jj, ii] = 0;
                }
            }
        }
        void propagate()
        {

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
        }
        void check(int x, int y)
        {

            if (!checkCoord(x, y)) return; //checa se coordenada é válida
            if (!checkColapse(x, y)) return;

            for (int face = 0; face < 4; face++)
            {
                if (!checkCoord(x + ((2 - face)) % 2, y + ((face - 1) % 2))) continue;

                for (int j_tiles = 0; j_tiles < tiles.Count; j_tiles++)
                {
                    bool tilechecked = false;
                    //if ((_grid[y, x].possibilities & (1 << j_tiles)) == 0) continue;
                    if (!checkbit(x, y, j_tiles)) continue;
                    for (int i_tiles = 0; i_tiles < tiles.Count; i_tiles++)
                    {
                        //if ((_grid[y + ((face - 1) % 2), x + ((2 - face)) % 2].possibilities & (1 << i_tiles)) == 0) continue;
                        if (!checkbit(x + ((2 - face)) % 2, y + ((face - 1) % 2), i_tiles)) continue;
                        bool temp = checkSockets(face, j_tiles, i_tiles);
                        tilechecked |= temp;
                    }
                    if (!tilechecked)
                    {
                        //byte temp = _grid[y, x].possibilities;
                        //_grid[y, x].possibilities &= (byte)~(1 << j_tiles);

                        if (unsetbit(x, y, j_tiles))
                            if (_grid[y, x].colapsed == 1)
                                ;
                            else
                                _grid[y, x].colapsed--;
                        //  if (temp != _grid[y, x].possibilities)
                        //      _grid[y, x].colapsed--;
                    }
                }
            }
        }
        bool checkSockets(int face, int tile1, int tile2)
        {
            //if (tiles[tile1].soket[((3 * face + 0) + 12) % 12] == tiles[tile2].soket[((3 * face + 2) + 6 + 12) % 12])
            //    if (tiles[tile1].soket[((3 * face + 1) + 12) % 12] == tiles[tile2].soket[((3 * face + 1) + 6 + 12) % 12])
            //        if (tiles[tile1].soket[((3 * face + 2) + 12) % 12] == tiles[tile2].soket[((3 * face + 0) + 6 + 12) % 12])
            //            return true;

            string cs1 = tiles[tile1].soket;
            string cs2 = tiles[tile2].soket;
            //00 01 02 - 04 05 06 - 08 09 10 - 12 13 14
            //10 09 08 - 14 13 12 - 02 01 00 - 06 05 04
            return cs1[face * 4 + 0] == cs2[((((face + 2) % 4) * 4) + 2) % 15] && cs1[face * 4 + 1] == cs2[((((face + 2) % 4) * 4) + 1) % 15] && cs1[face * 4 + 2] == cs2[((((face + 2) % 4) * 4) + 0) % 15];
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
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, 96 * 2, 54 * 2, 0, 0, 100);
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
                        for (int k = 0; k < tiles.Count; k++)
                        {
                            //if ((_grid[j, i].possibilities & (1 << k)) != 0)
                            if (checkbit(i, j, k))
                            {
                                int id = tiles[k].id;
                                float rot = (MathHelper.Tau / 4) * (tiles[k].rot);
                                Rectangle rect = tiles[k].rect;//new Rectangle(3 * id, 3 * id, 3, 3);
                                //_spriteBatch.Draw(tileset, orig + new Vector2(i, j) * 3, rect, new Color(255, 255, 255, 255 / 1), rot, new Vector2(1.5f, 1.5f), 1, SpriteEffects.None, 0);
                                _spriteBatch.Draw(tileset,
                                    destinationRectangle: new Rectangle(
                                        (int)orig.X + 4 + i * (3 + 1) * 3,
                                        (int)orig.Y + 4 + j * (3 + 1) * 3,
                                        (3 + 1) * 3,
                                        (3 + 1) * 3),
                                    rect,
                                    new Color(255, 255, 255, 255 / 1), rot, new Vector2(rect.Width, rect.Height) / 2f, SpriteEffects.None, 0);
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < tiles.Count; k++)
                        {
                            //if ((_grid[j, i].possibilities & (1 << k)) != 0)
                            if (checkbit(i, j, k))
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