using System;
using System.Collections.Generic;
using System.Text;

namespace LM1_myjnia
{
    public class CarWashMachine //Klasa reprezentuje naszą myjnię
    {
        /* W wyniku Pana komentarza zmieniłem:
         -usunięcie _currentState += coin, która dodawała monety do obecnego stanu. Obecnie logika maszyny
         stanów wygląda tak, że następny stan jest określany przez tabelę przejść _stateTransitions
         Wskutek tego spelniam uwage: kolejny stan to komórka przecięcia w tablicy między QxE
        */
        private int _currentState = 0;
        private readonly StringBuilder _road = new StringBuilder("q0");

        // Tabela przejść stanów
        private readonly Dictionary<(int, int), int> _stateTransitions = new Dictionary<(int, int), int>
        {
            // Przejścia dla monet 1, 2, 5 zł
            {(0, 1), 1}, {(0, 2), 2}, {(0, 5), 5},
            {(1, 1), 2}, {(1, 2), 3}, {(1, 5), 6},
            {(2, 1), 3}, {(2, 2), 4}, {(2, 5), 7},
            {(3, 1), 4}, {(3, 2), 5}, {(3, 5), 8},
            {(4, 1), 5}, {(4, 2), 6}, {(4, 5), 9},
            {(5, 1), 6}, {(5, 2), 7}, {(5, 5), 10},
            {(6, 1), 7}, {(6, 2), 8}, {(6, 5), 11},
            {(7, 1), 8}, {(7, 2), 9}, {(7, 5), 12},
            {(8, 1), 9}, {(8, 2), 10}, {(8, 5), 13},
            {(9, 1), 10}, {(9, 2), 11}, {(9, 5), 14},
            {(10, 1), 11}, {(10, 2), 12}, {(10, 5), 15},
            {(11, 1), 12}, {(11, 2), 13}, {(11, 5), 16},
            {(12, 1), 13}, {(12, 2), 14}, {(12, 5), 17},
            {(13, 1), 14}, {(13, 2), 15}, {(13, 5), 18},
            {(14, 1), 15}, {(14, 2), 16}, {(14, 5), 19},
            {(15, 1), 16}, {(15, 2), 17}, {(15, 5), 15},
            {(16, 1), 17}, {(16, 2), 18}, {(16, 5), 16},
            {(17, 1), 18}, {(17, 2), 19}, {(17, 5), 17},
            {(18, 1), 19}, {(18, 2), 20}, {(18, 5), 18},
            {(19, 1), 19}, {(19, 2), 19}, {(19, 5), 19}
        };

        // Tablica reszty dla każdego stanu - wskutek tego jest atrybutem stanu
        private readonly Dictionary<int, int> _changeForState = new Dictionary<int, int>
        {
            {15, 0}, {16, 1}, {17, 2}, {18, 3}, {19, 4}
        };

        public void InsertCoin(int coin)
        {
            if (_stateTransitions.TryGetValue((_currentState, coin), out int nextState))
            {
                _currentState = nextState;
                _road.Append($", q{_currentState}");
                Console.WriteLine($"Obecny stan automatu: q{_currentState}");
                Console.WriteLine($"Historia stanów automatu: {_road}");
            }
            else
            {
                Console.WriteLine("Nieprawidłowe przejście stanu.");
            }
        }

        public void PrintTicket() //Metoda do drukowania biletu
        {
            if (IsReadyToPrintTicket()) //Weryfikuje czy maszyna juz moze drukowac bilet
            {//Drukuje bilet i pokazuje reszte do wydania
                int change = _changeForState.GetValueOrDefault(_currentState, 0);
                Console.WriteLine($"Drukowanie biletu na mycie. Reszta do wydania: {change} zł.");
                ResetMachine(); //Resetuje stan maszyny i historię stanów
            }
            else
            {
                Console.WriteLine("Za mała kwota. Wrzuć więcej monet.");
            }
        }

        private void ResetMachine() //Metoda do resetowania stanu maszyny - porządek w kodzie
        {
            _currentState = 0;
            _road.Clear();
            _road.Append("q0");
        }

        public bool IsReadyToPrintTicket() 
        {
            return _currentState >= 15; //Metoda sprawdzajaca, czy maszyna jest gotowa do drukowania biletu
        }
    }

    public class Program
    {
        public static void Main()
        {
            CarWashMachine machine = new CarWashMachine(); //tworzy nowa instancje naszej myjni
            //Informacja do uzytkownika, zeby wrzucal monety
            Console.WriteLine("Wrzuć monety do osiągnięcia kwoty przynajmniej 15 zł.");
            
            //petla dziala dopóki nie osiągniemy stanu akceptującego
            while (!machine.IsReadyToPrintTicket())
            {
                Console.WriteLine("Wrzuć monetę (1, 2, 5):");
                //sprawdza czy nominal nalezy do naszego alfabetu
                if (!int.TryParse(Console.ReadLine(), out int coin) || (coin != 1 && coin != 2 && coin != 5))
                {
                    Console.WriteLine("Nieakceptowalna moneta. Spróbuj ponownie.");
                    continue;
                }

                machine.InsertCoin(coin); //wrzuca monete do maszyny
            }

            machine.PrintTicket(); //drukowanie biletu
        }
    }
}
