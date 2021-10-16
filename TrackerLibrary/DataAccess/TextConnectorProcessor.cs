using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string fileName)
        {
            return $"{ConfigurationManager.AppSettings["filePath"]}\\{fileName}";
        }

        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');

                var p = new PrizeModel
                {
                    Id = int.Parse(columns[0]),
                    PlaceNumber = int.Parse(columns[1]),
                    PlaceName = columns[2],
                    PrizeAmount = decimal.Parse(columns[3]),
                    PrizePercentage = float.Parse(columns[4])
                };

                output.Add(p);
            }

            return output;
        }
        
        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');

                var p = new PersonModel
                {
                    Id = int.Parse(columns[0]),
                    FirstName = columns[1],
                    LastName = columns[2],
                    EmailAddress = columns[3],
                    PhoneNumber = columns[4],
                };

                output.Add(p);
            }

            return output;
        }

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string peopleFileName)
        {
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> people = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModels();

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');

                var t = new TeamModel
                {
                    Id = int.Parse(columns[0]),
                    TeamName = columns[1]
                };

                string[] personIds = columns[2].Split('|');
                foreach (string id in personIds)
                {
                    t.TeamMembers.Add(people.First(x => x.Id == int.Parse(id)));
                }
                
                output.Add(t);
            }

            return output;
        }

        public static List<TournamentModel> ConvertToTournamentModels(
            this List<string> lines,
            string teamFileName,
            string peopleFileName,
            string prizesFileName)
        {
            //id,TournamentName,EntryFee,id|id|id,id|id|id,id^id^id|id^id^id|id^id^id

            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = teamFileName.FullFilePath().LoadFile().ConvertToTeamModels(peopleFileName);
            List<PrizeModel> prizes = prizesFileName.FullFilePath().LoadFile().ConvertToPrizeModels();

            foreach (string line in lines)
            {
                string[] columns = line.Split(',');

                var tm = new TournamentModel
                {
                    Id = int.Parse(columns[0]),
                    TournamentName = columns[1],
                    EntryFee = decimal.Parse(columns[2])
                };
                
                string[] teamIds = columns[3].Split('|');
                foreach (string id in teamIds)
                {
                    tm.EnteredTeams.Add(teams.First(x => x.Id == int.Parse(id)));
                }

                string[] prizeIds = columns[4].Split('|');
                foreach (string id in prizeIds)
                {
                    tm.Prizes.Add(prizes.First(x => x.Id == int.Parse(id)));
                }
                 
                //TODO - capture Rounds info
                
                output.Add(tm);
            }
            
            return output;
        }
        
        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add($"{p.Id},{p.PlaceNumber},{p.PlaceName},{p.PrizeAmount},{p.PrizePercentage}");
            }
            
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
        
        public static void SaveToPeopleFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add($"{p.Id},{p.FirstName},{p.LastName},{p.EmailAddress},{p.PhoneNumber}");
            }
            
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
        
        public static void SaveToTeamsFile(this List<TeamModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel t in models)
            {
                lines.Add($"{t.Id},{t.TeamName},{ConvertPeopleListToString(t.TeamMembers)}");
            }
            
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToTournamentsFile(this List<TournamentModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TournamentModel t in models)
            {
                lines.Add($@"{t.Id},
                    {t.TournamentName},
                    {t.EntryFee},
                    {ConvertTeamsListToString(t.EnteredTeams)},
                    {ConvertPrizesListToString(t.Prizes)},
                    {ConvertRoundsListToString(t.Rounds)}");
            }
            
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
        
        private static string ConvertRoundsListToString(List<List<MatchupModel>> rounds)
        {
            string output = "";

            if (rounds.Count == 0)
            {
                return "";
            }

            foreach (List<MatchupModel> r in rounds)
            {
                output += $"{ConvertMatchupsListToString(r)}|";
            }

            output = output.Substring(0, output.Length - 1);
            
            return output;
        }

        private static string ConvertMatchupsListToString(List<MatchupModel> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {
                return "";
            }

            foreach (MatchupModel m in matchups)
            {
                output += $"{m.Id}^";
            }

            output = output.Substring(0, output.Length - 1);
            
            return output;
        }
        
        private static string ConvertPrizesListToString(List<PrizeModel> prizes)
        {
            string output = "";

            if (prizes.Count == 0)
            {
                return "";
            }

            foreach (PrizeModel p in prizes)
            {
                output += $"{p.Id}|";
            }

            output = output.Substring(0, output.Length - 1);
            
            return output;
        }

        private static string ConvertTeamsListToString(List<TeamModel> teams)
        {
            string output = "";

            if (teams.Count == 0)
            {
                return "";
            }

            foreach (TeamModel t in teams)
            {
                output += $"{t.Id}|";
            }

            output = output.Substring(0, output.Length - 1);
            
            return output;
        }

        private static string ConvertPeopleListToString(List<PersonModel> people)
        {
            string output = "";

            if (people.Count == 0)
            {
                return "";
            }

            foreach (PersonModel p in people)
            {
                output += $"{p.Id}|";
            }

            output = output.Substring(0, output.Length - 1);
            
            return output;
        }
    }
}