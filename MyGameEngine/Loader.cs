using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace MyGameEngine
{
    public static class Loader
    {
        public static Dictionary<string, Unit> GetUnits(string directory, string filename)
        {
            var units = new Dictionary<string, Unit>();
            var con = new SQLiteConnection(@"Data Source=" + directory + @"\" + filename + ";Version=3;");
            con.Open();
            var cmd = new SQLiteCommand {Connection = con, CommandText = $"select * from Units"};
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
//                var success = Enum.TryParse<Units>(reader["Id"].ToString(), out var id);
//                if (!success || !Enum.IsDefined(typeof(Units), id))
//                {
//                    throw new MyException("Ошибка в БД");
//                }

                var success = Enum.TryParse<Types>(reader["Type"].ToString(), out var type);
                if (!success || !Enum.IsDefined(typeof(Types), type))
                {
                    throw new MyException("Ошибка в БД");
                }

                var abs = new HashSet<Abilities>();
                for (var i = 0; i < 4; i++)
                {
                    if (reader["Ability_" + i].ToString() == "")
                        continue;
                    success = Enum.TryParse<Abilities>(reader["Ability_" + i].ToString(), out var ab);
                    if (!success || !Enum.IsDefined(typeof(Abilities), ab))
                    {
                        throw new MyException("Ошибка в БД");
                    }

                    abs.Add(ab);
                }

                units.Add(reader["Id"].ToString(), new Unit(reader["Id"].ToString(), reader["Name"].ToString(), type, Convert.ToInt32(reader["Hitpoints"]),
                    Convert.ToInt32(reader["Attack"]), Convert.ToInt32(reader["Defence"]),
                    (Convert.ToInt32(reader["MinDamage"]), Convert.ToInt32(reader["MaxDamage"])), Convert.ToSingle(reader["Initiative"]), abs));
            }

            return units;
        }
    }
}