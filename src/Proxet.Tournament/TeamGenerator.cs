using System;
using System.Collections.Generic;
using System.IO;

namespace Proxet.Tournament
{
    public class TeamGenerator
    {
        const int CarTupesCount = 3;
        const int TeamsCount = 2;
        const int PlayersPerTeam = 9;
        public (string[] team1, string[] team2) GenerateTeams(string filePath)
        {
            //Please implement your algorithm there.

            List<Tuple<int, string>>[] SortedPlayers = GetPlayersFromFile(filePath);
            string[][] teams = new string[TeamsCount][];
            for (int i = 0; i < TeamsCount; i++) { teams[i] = new string[PlayersPerTeam]; }

            for (int i = 0; i < TeamsCount; i++)
            {
                for (int j = 0; j < CarTupesCount; j++)
                {
                    for (int b = 0; b < PlayersPerTeam / CarTupesCount; b++)
                    {
                        teams[i][j * CarTupesCount + b] = SortedPlayers[j][i * PlayersPerTeam / CarTupesCount + b].Item2;
                    }
                }
            }//creating teams

            return (teams[0], teams[1]);
        }
        private List<Tuple<int, string>>[] GetPlayersFromFile(string filePath)
        {
            List<Tuple<int, string>>[] Players = new List<Tuple<int, string>>[CarTupesCount];
            for (int i = 0; i < CarTupesCount; i++) { Players[i] = new List<Tuple<int, string>>(PlayersPerTeam / CarTupesCount * TeamsCount); }
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (reader.Peek() >= 0)
                    {
                        string[] buff = reader.ReadLine().Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        int WaitingTime;
                        int CarType;
                        if (Int32.TryParse(buff[1], out WaitingTime) && Int32.TryParse(buff[2], out CarType))
                        {
                            CarType--;

                            if (Players[CarType].Count < PlayersPerTeam / CarTupesCount * TeamsCount)
                            {
                                Players[CarType].Add(new Tuple<int, string>(WaitingTime, buff[0]));
                                if (Players[CarType].Count == PlayersPerTeam / CarTupesCount * TeamsCount)
                                {
                                    Players[CarType].Sort();
                                }
                                continue;
                            }
                            int lo = 0, hi = Players[CarType].Count - 1;
                            while (lo < hi)
                            {
                                int index = (hi + lo) / 2;
                                if (Players[CarType][index].Item1 < WaitingTime) lo = index + 1;
                                else hi = index - 1;
                            }
                            if (Players[CarType][lo].Item1 < WaitingTime)
                            {
                                lo++;

                            }
                            for (int i = 0; i < lo - 1; i++)
                            {
                                Players[CarType][i] = Players[CarType][i + 1];
                            }
                            if (lo != 0)
                            {
                                Players[CarType][lo - 1] = new Tuple<int, string>(WaitingTime, buff[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return Players;
        }
    }
}