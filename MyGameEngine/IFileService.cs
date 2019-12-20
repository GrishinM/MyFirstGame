using System.Collections.Generic;

namespace MyGameEngine
{
    public interface IFileService
    {
        void Save(string directory, string filename, IEnumerable<Army> armys);
        (Dictionary<string, Army>, Dictionary<string, BattleUnitsStack>) Open(string directory, string filename, Dictionary<string, Unit> units);
    }
}