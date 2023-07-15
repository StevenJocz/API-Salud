namespace UNAC.AppSalud.Domain.DTOs.MedicineFormDTOs
{
    public class MedicineFormDTOs
    {
        public int medicine_id { get; set; }
        public string user_email { get; set; }
        public int medicine_type { get; set; }
        public string medicine_presentation { get; set; }
        public string medicine_administration_way { get; set; }
        public int medicine_daily_times { get; set; }
        public DateTime medicine_hour { get; set; }
        public DateTime medicine_registration_timestamp { get; set; }
    }
}
