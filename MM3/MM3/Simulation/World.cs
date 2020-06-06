using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM3.Simulation
{
    public sealed class World
    {
        private const int CurrentVersion = 1;

        public static string NameGeneratorDatabaseFileName { get { return "name.generator.db.bin"; } }
        public static string WorldDatabaseFileName { get { return "world.db.bin"; } }
        public static string PointOfInterestDatabaseFileName { get { return "poi.db.bin"; } }
        public static string CreatureDatabaseFileName { get { return "creature.db.bin"; } }

        public Random Rng { get; private set; }
        public Dice Dice { get; private set; }
        public NameGenerator NameGenerator { get; private set; }

        public string Name { get; private set; }
        public int Size { get; private set; }
        public Time Time { get; private set; }
        public WorldSettings Settings { get; private set; }

        private Tile[,] tiles;
        private Database pointOfInterestDatabase = new Database();
        private Database creatureDatabase = new Database();

        public World(string name)
        {
            this.Rng = new Random();
            this.Dice = new Dice(this.Rng);
            this.NameGenerator = new NameGenerator();
            this.NameGenerator.LoadFromDisk(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, World.NameGeneratorDatabaseFileName));
            this.Name = name;
        }
        public World(string name, int size) : this(name)
        {
            this.Size = size;
            this.Time = new Time(1000, 1, 1, 0, 0);
            this.Settings = new WorldSettings();
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
            return new Creature(this.creatureDatabase, tile.World, tile.Position);
        }
        public PointOfInterest GetPointOfInterest(int id)
        {
            var result = this.pointOfInterestDatabase.GetMember(id);
            if (result == null) return null;
            else return (PointOfInterest)result;
        }
        public Creature GetCreature(int id)
        {
            var result = this.creatureDatabase.GetMember(id);
            if (result == null) return null;
            else return (Creature)result;
        }


        public void Tick()
        {
            this.Time.Advance(0, 1, 0, 0, 0);
            foreach (var member in this.pointOfInterestDatabase.Members)
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
                    tile.PointOfInterest = new PointOfInterest(this.pointOfInterestDatabase, tile.World, tile.Position);
                }
            }
        }

        public void SaveToDisk()
        {
            var worldDirectory = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.Name));
            if (!worldDirectory.Exists) worldDirectory.Create();

            var worldDatabaseFilePath = Path.Combine(worldDirectory.FullName, World.WorldDatabaseFileName);
            using (var fileStream = new FileStream(worldDatabaseFilePath, FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(fileStream))
                {
                    writer.Write(World.CurrentVersion);
                    writer.Write(this.Name);
                    writer.Write(this.Size);
                    this.Time.SaveToStream(writer);
                    this.Settings.SaveToStream(writer);
                    for(int x =0;x<this.Size;x++)
                    {
                        for(int y = 0;y<this.Size;y++)
                        {
                            this.tiles[x, y].SaveToStream(writer);
                        }
                    }
                }
            }

            var pointOfInterestDatabaseFilePath = Path.Combine(worldDirectory.FullName, World.PointOfInterestDatabaseFileName);
            using (var fileStream = new FileStream(pointOfInterestDatabaseFilePath, FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(fileStream))
                {
                    foreach(var member in this.pointOfInterestDatabase.Members)
                    {
                        var poi = (PointOfInterest)member;
                        poi.SaveToStream(writer);
                    }
                }
            }

            var creatureDatabaseFilePath = Path.Combine(worldDirectory.FullName, World.CreatureDatabaseFileName);
            using (var fileStream = new FileStream(creatureDatabaseFilePath, FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(fileStream))
                {
                }
            }
        }
    }
}
