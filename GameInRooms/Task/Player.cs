namespace Task
{
    public class Player
    {
        private Dictionary<string, int> Equipment { get; set; } =  new Dictionary<string, int>();

        public int Quantity(string item) => Equipment.ContainsKey(item) ? Equipment[item] : 0;
        
        public void Collect(string item)
        {
            // Jeśli przedmiot już istnieje w ekwipunku, zwiększ jego ilość
            if (Equipment.ContainsKey(item))
            {
                Equipment[item]++;
            }
            else
            {
                // Jeśli przedmiot nie istnieje, dodaj go z ilością 1
                Equipment[item] = 1;
            }
        }
    }
}
