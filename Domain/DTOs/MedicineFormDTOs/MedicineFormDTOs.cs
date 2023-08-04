namespace UNAC.AppSalud.Domain.DTOs.MedicineFormDTOs
{
    public class MedicineFormDTOs
    {
        public string medicine_id { get; set; }
        public string user_email { get; set; }
        public string medicine_type { get; set; }
        public string medicine_presentation { get; set; }
        public string medicine_administration_way { get; set; }
        public string medicine_daily_times { get; set; }
        public DateTime medicine_hour { get; set; }
        public DateTime medicine_registration { get; set; }

    }
}
