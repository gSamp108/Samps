using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MM3.Simulation
{
    public class WorldGenerator
    {
        public float LandCoverage = 0.5f;
        public int LandGenerationSeeds = 8;
        public int ForestRarity = 6;
        public int HillRarity = 24;
        public int MountainRarity = 48;
        public float ColdBiomeCoverage = 0.3f;
        public float HotBiomeCoverage = 0.3f;

        public Dictionary<Position, int> TileWeight;
        public HashSet<Position> ClosedLandTiles;
        public HashSet<Position> OpenLandTiles;
        public int MinimumLandTiles;
        public Tile[,] Result;
        public int WorldSize;
        public Random Rng;
        public Queue<Position> TilesQueuedToClose;
        public Bounds WorldBounds;
        public World World;
        public Position HeatmapOrigin;
        public Dictionary<Position, int> Heatmap;
        public HashSet<Position> CurrentOpenHeatmapTiles;
        public HashSet<Position> NextOpenHeatmapTiles;
        public int HighestHeatIndex;
        public int ColdBiomeIndex;
        public int HotBiomeIndex;

        public Tile[,] Generate(Random rng, World world)
        {
            this.Rng = rng;
            this.World = world;
            this.WorldSize = this.World.Size;
            this.WorldBounds = new Bounds(this.WorldSize, this.WorldSize);
            this.Result = new Tile[this.WorldSize, this.WorldSize];
            this.MinimumLandTiles = (int)(((float)(this.WorldSize * this.WorldSize)) * this.LandCoverage);
            this.ClosedLandTiles = new HashSet<Position>();
            this.OpenLandTiles = new HashSet<Position>();
            this.TileWeight = new Dictionary<Position, int>();
            this.TilesQueuedToClose = new Queue<Position>();
            this.Heatmap = new Dictionary<Position, int>();
            this.CurrentOpenHeatmapTiles = new HashSet<Position>();
            this.NextOpenHeatmapTiles = new HashSet<Position>();
            this.HighestHeatIndex = 0;
            this.ColdBiomeIndex = 0;
            this.HotBiomeIndex = 0;

            this.SeedIslands();
            this.GenerateIslands();
            this.GenerateHeatmap();
            this.GenerateTerrain();

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
            this.HotBiomeIndex = this.HighestHeatIndex - ((int)(this.HighestHeatIndex * this.ColdBiomeCoverage));
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

                    if (this.Heatmap[tilePosition] <= this.ColdBiomeIndex) tile.Biome = Tile.BiomeTypes.Cold;
                    else if (this.Heatmap[tilePosition] >= this.HotBiomeIndex) tile.Biome = Tile.BiomeTypes.Hot;
                    else tile.Biome = Tile.BiomeTypes.Temperate;
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

        private Position RandomPosition()
        {
            return new Position(this.Rng.Next(this.WorldSize), this.Rng.Next(this.WorldSize));
        }
    }
}