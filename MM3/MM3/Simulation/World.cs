﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class World
    {
        public Random Rng = new Random();
        public Time Time = new Time(1000, 1, 1, 0, 0);
        public int Size;
        public Dice Dice;
        public WorldSettings Settings = new WorldSettings();
        public NameGenerator NameGenerator = new NameGenerator();

        private Tile[,] tiles;
        private Database pointOfInterestDatabase = new Database();
        private Database creatureDatabase = new Database();

        public World (int size)
        {
            this.NameGenerator.LoadFromDisk("name.generator.db.bin");
            this.Size = size;
            this.Dice = new Dice(this.Rng);
            var worldGenerator = new WorldGenerator(this);
            this.tiles = worldGenerator.GenerateTiles();
            this.GeneratePointsOfInterest(worldGenerator.GeneratePointsOfInterest());
            this.Time.MinutePassed += Time_MinutePassed;
            this.Time.HourPassed += Time_HourPassed;
            this.Time.DayPassed += Time_DayPassed;
            this.Time.MonthPassed += Time_MonthPassed;
            this.Time.YearPassed += Time_YearPassed;
        }

        private void Time_YearPassed()
        {
        }
        private void Time_MonthPassed()
        {
        }
        private void Time_DayPassed()
        {
        }
        private void Time_HourPassed()
        {
        }
        private void Time_MinutePassed()
        {
        }

        public Creature GenerateCreature(Tile tile)
        {
            return new Creature(this.creatureDatabase, tile);
        }

        public void Tick()
        {
            this.Time.Advance(0, 1, 0, 0, 0);
            foreach(var member in this.pointOfInterestDatabase.Members)
            {
                var poi = (PointOfInterest)member;
                poi.Tick(this.Time);
            }
        }

        public Position WrapPosition(Position position)
        {
            var result = new Position(position.X, position.Y);

            while (result.X < 0) { result.X = result.X + this.Size; }
            while (result.X >= this.Size) { result.X = result.X - this.Size; }
            while (result.Y < 0) { result.Y = result.Y + this.Size; }
            while (result.Y >= this.Size) { result.Y = result.Y - this.Size; }

            return result;
        }

        public Tile GetTile(Position position)
        {
            var fixedPosition = this.WrapPosition(position);
            return this.tiles[fixedPosition.X, fixedPosition.Y];
        }

        private void GeneratePointsOfInterest(HashSet<Position> selectedPositions)
        {
            foreach (var position in selectedPositions)
            {
                var tile = this.GetTile(position);
                if (tile.Terrain == Tile.TerrainTypes.Flats)
                {
                    tile.PointOfInterest = new PointOfInterest(this.pointOfInterestDatabase, tile);
                }
            }
        }
    }
}
