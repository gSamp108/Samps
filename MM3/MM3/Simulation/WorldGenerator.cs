using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MM3.Simulation
{
    public class WorldGenerator
    {
        public int WorldSize { get { return this.World.Size; } }
        public Random Rng { get { return this.World.Rng; } }
        public World World;
        public Bounds WorldBounds { get { return new Bounds(this.WorldSize, this.WorldSize); } }

        public WorldGenerator(World world)
        {
            this.World = world;
        }

        private Position RandomPosition()
        {
            return new Position(this.Rng.Next(this.WorldSize), this.Rng.Next(this.WorldSize));
        }

        public float LandCoverage = 0.5f;
        public int LandGenerationSeeds = 8;
        public int ForestRarity = 6;
        public int HillRarity = 24;
        public int MountainRarity = 48;
        public float ColdBiomeCoverage = 0.3f;
        public float HotBiomeCoverage = 0.3f;
        public int HeatmapSpreadModifier = 2;
        public Dictionary<Position, int> TileWeight = new Dictionary<Position, int>();
        public HashSet<Position> ClosedLandTiles = new HashSet<Position>();
        public HashSet<Position> OpenLandTiles = new HashSet<Position>();
        public int MinimumLandTiles;
        public Tile[,] Result;
        public Queue<Position> TilesQueuedToClose = new Queue<Position>();
        public Position HeatmapOrigin;
        public Dictionary<Position, int> Heatmap = new Dictionary<Position, int>();
        public HashSet<Position> CurrentOpenHeatmapTiles = new HashSet<Position>();
        public HashSet<Position> NextOpenHeatmapTiles = new HashSet<Position>();
        public int HighestHeatIndex;
        public int ColdBiomeIndex;
        public int HotBiomeIndex;
        public int ColdestLandTileIndex;
        public HashSet<Position> ColdestLandTiles = new HashSet<Position>();
        public int HottestLandTileIndex;
        public HashSet<Position> HottestLandTiles = new HashSet<Position>();

        public Tile[,] GenerateTiles()
        {
            this.Result = new Tile[this.WorldSize, this.WorldSize];
            this.MinimumLandTiles = (int)(((float)(this.WorldSize * this.WorldSize)) * this.LandCoverage);

            this.SeedIslands();
            this.GenerateIslands();
            this.GenerateTerrain();
            this.GenerateHeatmap();

            return this.Result;
        }

        private void GenerateHeatmap()
        {
            this.HeatmapOrigin = this.RandomPosition();
            this.NextOpenHeatmapTiles.Add(this.HeatmapOrigin);

            while (this.NextOpenHeatmapTiles.Count > 0)
            {
                this.CurrentOpenHeatmapTiles = this.NextOpenHeatmapTiles;
                this.NextOpenHeatmapTiles = new HashSet<Position>();

                while (this.CurrentOpenHeatmapTiles.Count > 0)
                {
                    var currentTile = this.CurrentOpenHeatmapTiles.Random(this.Rng);
                    this.CurrentOpenHeatmapTiles.Remove(currentTile);
                    if (!this.Heatmap.ContainsKey(currentTile)) this.Heatmap.Add(currentTile, 0);
                    var currentHeatIndex = this.Heatmap[currentTile];

                    var currentTileTerrain = this.Result[currentTile.X, currentTile.Y].Terrain;

                    if (currentTileTerrain != Tile.TerrainTypes.Ocean && currentTileTerrain != Tile.TerrainTypes.Shallow)
                    {
                        if (this.ColdestLandTiles.Count == 0)
                        {
                            this.ColdestLandTileIndex = currentHeatIndex;
                            this.ColdestLandTiles.Add(currentTile);
                        }
                        else
                        {
                            if (currentHeatIndex < this.ColdestLandTileIndex)
                            {
                                this.ColdestLandTileIndex = currentHeatIndex;
                                this.ColdestLandTiles.Clear();
                                this.ColdestLandTiles.Add(currentTile);
                            }
                            else if (currentHeatIndex == this.ColdestLandTileIndex)
                            {
                                this.ColdestLandTiles.Add(currentTile);
                            }
                        }
                        if (currentHeatIndex > this.HottestLandTileIndex)
                        {
                            this.HottestLandTileIndex = currentHeatIndex;
                            this.HottestLandTiles.Clear();
                            this.HottestLandTiles.Add(currentTile);
                        }
                        else if (currentHeatIndex == this.HottestLandTileIndex)
                        {
                            this.HottestLandTiles.Add(currentTile);
                        }
                    }

                    foreach (var adjacentTile in currentTile.Adjacent)
                    {
                        var wrappedAdjacentTile = adjacentTile.Wrap(this.WorldBounds);
                        if (!this.Heatmap.ContainsKey(wrappedAdjacentTile))
                        {
                            this.Heatmap.Add(wrappedAdjacentTile, currentHeatIndex + 1);
                            this.NextOpenHeatmapTiles.Add(wrappedAdjacentTile);
                            if ((currentHeatIndex + 1) > this.HighestHeatIndex) this.HighestHeatIndex = currentHeatIndex + 1;
                        }
                    }
                }
            }

            this.ColdBiomeIndex = (int)(this.HighestHeatIndex * this.ColdBiomeCoverage);
            this.HotBiomeIndex = (int)(this.HighestHeatIndex * this.HotBiomeCoverage);

            this.GenerateBiome(Tile.BiomeTypes.Cold, this.ColdBiomeIndex, this.ColdestLandTiles.Random());
            this.GenerateBiome(Tile.BiomeTypes.Hot, this.HotBiomeIndex, this.HottestLandTiles.Random());
        }
        private void GenerateBiome(Tile.BiomeTypes biome, int maximumHeatIndex, Position origin)
        {
            var openTiles = new HashSet<Position>();
            var closedTiles = new HashSet<Position>();
            var heatmap = new Dictionary<Position, int>();

            openTiles.Add(origin);

            while (openTiles.Count > 0)
            {
                var currentTile = openTiles.Random();
                openTiles.Remove(currentTile);
                closedTiles.Add(currentTile);

                if (!heatmap.ContainsKey(currentTile)) heatmap.Add(currentTile, 0);
                var spreadHeatIndex = heatmap[currentTile] + 1;

                if (spreadHeatIndex < maximumHeatIndex)
                {
                    foreach (var adjacentTile in currentTile.Adjacent)
                    {
                        var wrappedAdjacentTile = adjacentTile.Wrap(this.WorldBounds);
                        var adjacentTileTerrain = this.Result[wrappedAdjacentTile.X, wrappedAdjacentTile.Y].Terrain;
                        if (adjacentTileTerrain != Tile.TerrainTypes.Ocean && adjacentTileTerrain != Tile.TerrainTypes.Shallow)
                        {
                            if (!heatmap.ContainsKey(wrappedAdjacentTile))
                            {
                                openTiles.Add(wrappedAdjacentTile);
                                heatmap.Add(wrappedAdjacentTile, spreadHeatIndex);
                                if (this.Rng.Next(this.HeatmapSpreadModifier) == 0) heatmap[wrappedAdjacentTile] += 1;
                            }
                        }
                    }
                }
            }

            foreach (var tile in closedTiles)
            {
                this.Result[tile.X, tile.Y].Biome = biome;
            }
        }
        private void GenerateTerrain()
        {
            for (int x = 0; x < this.WorldSize; x++)
            {
                for (int y = 0; y < this.WorldSize; y++)
                {
                    var tilePosition = new Position(x, y);
                    var tile = new Tile(this.World, tilePosition);
                    this.Result[tilePosition.X, tilePosition.Y] = tile;
                    if (this.ClosedLandTiles.Contains(tilePosition))
                    {
                        tile.Terrain = Tile.TerrainTypes.Flats;
                        if (this.Rng.Next(this.ForestRarity) == 0) tile.Terrain = Tile.TerrainTypes.Forest;
                        else if (this.Rng.Next(this.HillRarity) == 0) tile.Terrain = Tile.TerrainTypes.Hill;
                        else if (this.Rng.Next(this.MountainRarity) == 0) tile.Terrain = Tile.TerrainTypes.Mountain;
                    }
                    else if (this.TileWeight.ContainsKey(tilePosition))
                    {
                        tile.Terrain = Tile.TerrainTypes.Shallow;
                    }

                    tile.Biome = Tile.BiomeTypes.Temperate;
                }
            }
        }
        private void GenerateIslands()
        {
            while (this.ClosedLandTiles.Count < this.MinimumLandTiles)
            {
                this.TilesQueuedToClose.Enqueue(this.OpenLandTiles.Random(this.Rng));

                while (this.TilesQueuedToClose.Count > 0)
                {
                    var tile = this.TilesQueuedToClose.Dequeue();
                    this.OpenLandTiles.Remove(tile);

                    if (!this.ClosedLandTiles.Contains(tile))
                    {
                        this.ClosedLandTiles.Add(tile);
                        foreach (var adjacentTile in tile.Adjacent)
                        {
                            var wrappedAdjacentTile = adjacentTile.Wrap(this.WorldBounds);
                            if (!this.ClosedLandTiles.Contains(wrappedAdjacentTile)) this.OpenLandTiles.Add(wrappedAdjacentTile);
                        }
                        foreach (var nearbyTile in tile.Nearby)
                        {
                            var wrappedNearbyTile = nearbyTile.Wrap(this.WorldBounds);
                            if (!this.TileWeight.ContainsKey(wrappedNearbyTile)) this.TileWeight.Add(wrappedNearbyTile, 0);
                            this.TileWeight[wrappedNearbyTile] += 1;
                            if (this.TileWeight[wrappedNearbyTile] > 4) this.TilesQueuedToClose.Enqueue(wrappedNearbyTile);
                        }
                    }
                }
            }
        }
        private void SeedIslands()
        {
            for (int i = 0; i < this.LandGenerationSeeds; i++)
            {
                this.OpenRandomTile();
            }
            if (this.OpenLandTiles.Count == 0) this.OpenRandomTile();
        }
        private void OpenRandomTile()
        {
            this.OpenLandTiles.Add(this.RandomPosition());
        }

        public int MinimumPOISpread = 5;
        public int POISpreadVariance = 5;
        public float POISpawnChance = 0.5f;

        public HashSet<Position> GeneratePointsOfInterest()
        {
            var openList = new HashSet<Position>();
            var closedList = new HashSet<Position>();
            var spawnedPOIs = new HashSet<Position>();

            openList.Add(this.RandomPosition());

            while (openList.Count>0)
            {
                var selectedPosition = openList.Random(this.Rng);
                var spreadVariance = this.Rng.Next(this.POISpreadVariance + 1);
                var spread = this.MinimumPOISpread + spreadVariance;
                if (this.Rng.NextDouble() < this.POISpawnChance) spawnedPOIs.Add(selectedPosition);
                var spreadClosedList = new HashSet<Position>();
                var currentSpreadOpenList = new HashSet<Position>();
                var nextSpreadOpenList = new HashSet<Position>();
                currentSpreadOpenList.Add(selectedPosition);

                for (int i = 0; i < spread; i++)
                {
                    foreach (var spreadPosition in currentSpreadOpenList)
                    {
                        closedList.Add(spreadPosition);
                        spreadClosedList.Add(spreadPosition);
                        openList.Remove(spreadPosition);
                        foreach (var adjacentSpreadPosition in spreadPosition.Adjacent)
                        {
                            var wrappedAdjacentSpreadPosition = adjacentSpreadPosition.Wrap(this.WorldBounds);
                            if (!spreadClosedList.Contains(wrappedAdjacentSpreadPosition))
                                nextSpreadOpenList.Add(wrappedAdjacentSpreadPosition);
                        }
                    }
                    currentSpreadOpenList = nextSpreadOpenList;
                    nextSpreadOpenList = new HashSet<Position>();
                }

                foreach (var spreadTile in currentSpreadOpenList)
                {
                    if (!closedList.Contains(spreadTile)) openList.Add(spreadTile);
                }
            }

            return spawnedPOIs;
        }
    }
}