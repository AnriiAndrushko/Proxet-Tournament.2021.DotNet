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
            string[][] teams = new string[TeamsCount][];
            for (int i = 0; i < TeamsCount; i++) { teams[i] = new string[PlayersPerTeam]; }

            List<Tuple<int, int, string>>[] Players = GetPlayersFromFile(filePath);//take data from file and store it
            LinkedList<Tuple<int, int, string>>[] SortedPlayers = new LinkedList<Tuple<int, int, string>>[CarTupesCount];

            for (int i = 0; i < CarTupesCount; i++)
            {
                Players[i].Sort();
                SortedPlayers[i] = new LinkedList<Tuple<int, int, string>>(Players[i]);
            }//sort players by wait time into 3 linked lists with different car types

            for (int i = 0; i < TeamsCount; i++)
            {
                for (int j = 0; j < CarTupesCount; j++)
                {
                    for (int b = 0; b < PlayersPerTeam / CarTupesCount; b++)
                    {
                        teams[i][j * CarTupesCount + b] = SortedPlayers[j].Last.Value.Item3;
                        SortedPlayers[j].RemoveLast();
                    }
                }
            }//creating teams
            return (teams[0], teams[1]);
        }
        private List<Tuple<int, int, string>>[] GetPlayersFromFile(string filePath)
        {
            List<Tuple<int, int, string>>[] Players = new List<Tuple<int, int, string>>[CarTupesCount];
            for (int i = 0; i < CarTupesCount; i++) { Players[i] = new List<Tuple<int, int, string>>(); }
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
                            Players[CarType].Add(new Tuple<int, int, string>(WaitingTime, CarType, buff[0]));
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