using System;

namespace TRPO_Project.WPFA.Model
{
    public class History
    {
        public int ID_Ист { get; set; }
        public string Код { get; set; }
        public DateTime Дата { get; set; }
        public decimal Цена { get; set; }
        public int Количество { get; set; }
        public decimal Сумма { get; set; }
        public string Операция { get; set; }
    }
}
