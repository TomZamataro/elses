using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Teste_Tecnico_Tapps
{
    #region Shuffle Extension
    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }

    static class MyExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
    #endregion
    class Simulação
    {
        #region Declaretion
        public static List<Player> OrderOfTurns = new List<Player> { new ImpulsivePlayer(), new RandomPlayer(), new PickyPlayer(), new CautiousPlayer()};
        public static List<Propriedade> OrderOnBoard = new List<Propriedade>();
        public static List<int> RoundsCountPerMach = new List<int>();
        public static int impulsiveVictories = 0, randomVictories = 0, cautiousVictories = 0, pickyVictories = 0, rounds = 0, timeOutCount = 0;
        public static string[] GameConfig = File.ReadAllLines(@"C:\Users\valeria\Desktop\Coisas do Tom\Codes\Desafio Tapps Games\gameConfig.txt");
        public static string[,] ProprietiesAtributes = new string[20, 2];
        #endregion
        #region Methods
        public static string WhoWonMore()
        {
            int highest = Math.Max(Math.Max(Math.Max(impulsiveVictories, randomVictories), pickyVictories), cautiousVictories);
            // 4 Winners
            if (Math.Equals(Math.Equals(Math.Equals(impulsiveVictories, randomVictories), pickyVictories), cautiousVictories))
            {
                return "       Os 4 jogadores tiveram a mesma quantidade de vitórias";
            }
            // 3 Winners
            else
            {
                if (Math.Equals(Math.Equals(impulsiveVictories, randomVictories), pickyVictories) && impulsiveVictories == highest)
                {
                    return "       Os jogadores do tipo Impulsivo, Aleatório e Exigente foram os que mais ganharam";
                }
                else if (Math.Equals(Math.Equals(impulsiveVictories, randomVictories), cautiousVictories) && impulsiveVictories == highest)
                {
                    return "       Os jogadores do tipo Impulsivo, Aleatório e Cauteloso foram os que mais ganharam";
                }
                else if (Math.Equals(Math.Equals(impulsiveVictories, cautiousVictories), pickyVictories) && impulsiveVictories == highest)
                {
                    return "       Os jogadores do tipo Impulsivo, Cauteloso e Exigente foram os que mais ganharam";
                }
                else if (Math.Equals(Math.Equals(cautiousVictories, randomVictories), pickyVictories) && impulsiveVictories == highest)
                {
                    return "       Os jogadores do tipo Cauteloso, Aleatório e Exigente foram os que mais ganharam";
                }
                // 2 Winners
                else
                {
                    if (impulsiveVictories == randomVictories && impulsiveVictories == highest)
                    {
                        return "       Os jogadores do tipo Impulsivo e Aleatório foram os que mais ganharam";
                    }
                    else if (impulsiveVictories == pickyVictories && impulsiveVictories == highest)
                    {
                        return "       Os jogadores do tipo Impulsivo e Exigente foram os que mais ganharam";
                    }
                    else if (impulsiveVictories == cautiousVictories && impulsiveVictories == highest)
                    {
                        return "       Os jogadores do tipo Impulsivo e Cauteloso foram os que mais ganharam";
                    }
                    else if (randomVictories == pickyVictories && randomVictories == highest)
                    {
                        return "       Os jogadores do tipo Aleatório e Exigente foram os que mais ganharam";
                    }
                    else if (randomVictories == cautiousVictories && randomVictories == highest)
                    {
                        return "       Os jogadores do tipo Aleatório e Cauteloso foram os que mais ganharam";
                    }
                    else if (cautiousVictories == pickyVictories && cautiousVictories == highest)
                    {
                        return "       Os jogadores do tipo Cauteloso e Exigente foram os que mais ganharam";
                    }
                    // 1 Winner
                    else
                    {
                        if (highest == impulsiveVictories)
                        {
                            return "       O Jogador do tipo Impulsivo foi oque mais ganhou";
                        }
                        else if (highest == randomVictories)
                        {
                            return "       O Jogador do tipo Aleatório foi oque mais ganhou";
                        }
                        else if (highest == pickyVictories)
                        {
                            return "       O Jogador do tipo Exigente foi oque mais ganhou";
                        }
                        else
                        {
                            return "       O Jogador do tipo Cauteloso foi oque mais ganhou";
                        }
                    }
                }
            }
        }
        public static Player TimeOutWinner()
        {
            int index = 0;
            int coins = 0;
            foreach (Player p  in OrderOfTurns)
            {
                if (p.PlayerCoins > coins)
                {
                    coins = p.PlayerCoins;
                    index = OrderOfTurns.IndexOf(p);
                }
            }
            return OrderOfTurns[index];
        }
        public static string PercentageOfWinsPerType()
        {
            int total = RoundsCountPerMach.Count;
            double impulsiveWinRate = (impulsiveVictories * 100) / total, randomWinRate = (randomVictories * 100) / total, cautiousWinRate = (cautiousVictories * 100) / total, pickyWinRate = (pickyVictories * 100) / total;
            return "       A porcentagem de Vitórias para cada tipo de jogador foi:\n" +
                "           - Impulsivo: " + impulsiveWinRate + "%\n" +
                "           - Aleatório: " + randomWinRate + "%\n" +
                "           - Cauteloso: " + cautiousWinRate + "%\n" +
                "           - Exigente: " + pickyWinRate + "%";
        }
        public static double RoundsAvarrege()
        {
            int sum = 0;
            foreach (int c in RoundsCountPerMach)
            {
                sum += c;
            }
            return sum / RoundsCountPerMach.Count;
        }
        public static Propriedade WichBuy(Player p)
        {
            int pp = p.PlayerPosition;
            foreach (Propriedade prop in OrderOnBoard)
            {
                if (pp == prop.GetPosition())
                {
                    return prop;
                }
            }
            return null;
        }
        public static bool WeHaveWinner()
        {
            if (OrderOfTurns.Count == 1)
            {
                return true;
            }
            return false;
        }
        #endregion
        static void Main(string[] args)
        {
            #region Console Customisation
            Console.Title = "// Desafio Técnico Tapps Games \\\\";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Clear();
            #endregion
            int count = 1;
            // Criação do Tabuleiro
            foreach (string p in GameConfig)
            {
                string[] a = p.Split(' ', 2);
                OrderOnBoard.Add(new Propriedade(int.Parse(a[0]), int.Parse(a[1]), count));
                count++;
            }
            // Começo da Simulação
        NewMach:;
            if (!(RoundsCountPerMach.Count >= 300))
            {
                OrderOfTurns.Clear();
                OrderOfTurns.Add(new ImpulsivePlayer());
                OrderOfTurns.Add(new PickyPlayer());
                OrderOfTurns.Add(new RandomPlayer());
                OrderOfTurns.Add(new CautiousPlayer());
                OrderOfTurns.Shuffle();
                rounds = 0;
            // Turno de cada Jogador
            NewRoud:;
                for (int i = 0; i < OrderOfTurns.Count; i++)
                {
                    OrderOfTurns[i].Move();
                    if (OrderOfTurns[i].BuyOrPay(WichBuy(OrderOfTurns[i])))
                    {
                        OrderOfTurns.Remove(OrderOfTurns[i]);
                    }
                }
                rounds++;
                if (WeHaveWinner())
                {
                    if (OrderOfTurns[0].ToString().EndsWith("ImpulsivePlayer"))
                    {
                        impulsiveVictories++;
                        // Optional Output - Console.WriteLine($"\n       O jogador que ganhou a {RoundsCountPerMach.Count + 1}º partida foi o do tipo Impulsivo");
                    }
                    else if (OrderOfTurns[0].ToString().EndsWith("CautiousPlayer"))
                    {
                        cautiousVictories++;
                        // Optional Output - Console.WriteLine($"\n       O jogador que ganhou a {RoundsCountPerMach.Count + 1}º partida foi o do tipo Cauteloso");
                    }
                    else if (OrderOfTurns[0].ToString().EndsWith("PickyPlayer"))
                    {
                        pickyVictories++;
                        // Optional Output - Console.WriteLine($"\n       O jogador que ganhou a {RoundsCountPerMach.Count + 1}º partida foi o do tipo Exigente");
                    }
                    else
                    {
                        randomVictories++;
                        // Optional Output - Console.WriteLine("\n       O jogador que ganhou a {RoundsCountPerMach.Count + 1}º partida foi o do tipo Aleatório");
                    }
                    // Optional Output - Console.WriteLine($"\n       Número de Rodadas na {RoundsCountPerMach.Count + 1}º partida: {rounds}");
                    RoundsCountPerMach.Add(rounds);
                    foreach (Player p in OrderOfTurns)
                    {
                        p.Propriedades.Clear();
                        p.PlayerCoins = 300;
                        p.PlayerPosition = 1;
                    }
                    goto NewMach;
                }
                else if (rounds >= 1000)
                {
                    // Optional Output - Console.WriteLine($"\n       A {RoundsCountPerMach.Count + 1}º Partifa foi terminada por TimeOut");
                    if (TimeOutWinner().ToString().EndsWith("ImpulsivePlayer"))
                    {
                        impulsiveVictories++;
                        // Optional Output - Console.WriteLine($"\n       O jogador que ganhou a {RoundsCountPerMach.Count + 1}º partida foi o do tipo Impulsivo");
                    }
                    else if (TimeOutWinner().ToString().EndsWith("CautiousPlayer"))
                    {
                        cautiousVictories++;
                        // Optional Output - Console.WriteLine($"\n       O jogador que ganhou a {RoundsCountPerMach.Count + 1}º partida foi o do tipo Cauteloso");
                    }
                    else if (TimeOutWinner().ToString().EndsWith("PickyPlayer"))
                    {
                        pickyVictories++;
                        // Optional Output - Console.WriteLine($"\n       O jogador que ganhou a {RoundsCountPerMach.Count + 1}º partida foi o do tipo Exigente");
                    }
                    else
                    {
                        randomVictories++;
                        // Optional Output - Console.WriteLine($"\n       O jogador que ganhou a {RoundsCountPerMach.Count + 1}º partida foi o do tipo Aleatório");
                    }
                    foreach (Player p in OrderOfTurns)
                    {
                        p.Propriedades.Clear();
                        p.PlayerCoins = 300;
                        p.PlayerPosition = 1;
                    }
                    RoundsCountPerMach.Add(0);
                    timeOutCount++;
                    goto NewMach;
                }
                else
                {
                    goto NewRoud;
                }
            }
            else
            {
                Console.WriteLine($"\n       A quantidade de partidas terminadas por TimeOut foi: {timeOutCount}");
                Console.WriteLine($"\n       A média de Rodadas que uma partida demora é: {RoundsAvarrege()}");
                Console.WriteLine("\n" + WhoWonMore());
                Console.WriteLine("\n" + PercentageOfWinsPerType());
                Console.ReadKey();
            }
        }
    }
}
